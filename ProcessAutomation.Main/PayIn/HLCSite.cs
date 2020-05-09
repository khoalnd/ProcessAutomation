using MongoDB.Driver;
using ProcessAutomation.DAL;
using ProcessAutomation.Main.Services;
using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessAutomation.Main.PayIn
{
    public class HLCSite : IAutomationPayIn
    {
        MailService mailService = new MailService();
        Helper helper = new Helper();
        private WebBrowser webLayout;
        private List<Message> data = new List<Message>();
        private const string web_name = "hanhlangcu";
        private const string url = "https://hanhlangcu.com/";
        private const string index_URL = url + "Login";
        private const string user_URL = url + "Users";
        private const string agencies_URL = url + "Users/Agencies";
        private const string addMoney_URL = url + "Users/AddMoneyToUser";
        private bool isFinishProcess = true;
        Message currentMessage;
        Void v;
        TaskCompletionSource<Void> tcs = null;
        WebBrowserDocumentCompletedEventHandler documentComplete = null;

        public HLCSite(List<Message> data, WebBrowser web)
        {
            this.data = data;
            this.webLayout = web;
        }

        public bool checkProcessDone()
        {
            return isFinishProcess;
        }

        public void startPayIN()
        {
            StartProcess();
        }

        struct Void { };

        async Task StartProcess()
        {
            try
            {
                documentComplete = new WebBrowserDocumentCompletedEventHandler((s, e) =>
                {
                    if (webLayout.DocumentText.Contains("res://ieframe.dll"))
                    {
                        tcs.SetException(new Exception("Lỗi không có kết nối internet"));
                        return;
                    }
                    webLayout.DocumentCompleted -= documentComplete;
                    tcs.SetResult(v);
                });
                isFinishProcess = false;
                AccountData userAccount = new AccountData();
                var adminAccount = new AdminAccount();

                var process = checkAccountAdmin(ref adminAccount);
                do
                {
                    switch (process)
                    {
                        case "OpenWeb":
                            CreateSyncTask();
                            webLayout.Navigate(url);
                            await tcs.Task;
                            await Task.Delay(5000);

                            process = "Login";
                            if (webLayout.Url.ToString() == user_URL)
                            {
                                process = "CheckAmountAccount";
                                break;
                            }

                            if (webLayout.Url.ToString() != index_URL)
                            {
                                SendNotificationForError(
                                        "Trang Web Không Truy Cập Được",
                                        $"{web_name} không thể truy cập");

                                process = "Finish";
                                break;
                            }

                            break;
                        case "Login":
                            CreateSyncTask();
                            Login(adminAccount);
                            await tcs.Task;
                            await Task.Delay(5000);

                            if (webLayout.Url.ToString() == index_URL)
                            {
                                SendNotificationForError("Account Admin Đăng Nhập Lỗi",
                                    $"{web_name} : Account admin đăng nhập web bị lỗi");
                                process = "Finish";
                                break;
                            }
                            process = "CheckAmountAccount";

                            break;
                        case "CheckAmountAccount":
                            var isAmountEnough = CheckAmountAccount();
                            await Task.Delay(3000);

                            if (!isAmountEnough)
                            {
                                SendNotificationForError("Account không đủ số tiền tối thiểu",
                                    $"{web_name} : Account admin không đủ số tiền tối thiểu");
                                process = "Finish";
                                break;
                            }
                            process = "AccessToDaily";
                            break;
                        case "AccessToDaily":
                            CreateSyncTask();
                            AccessToDaily();
                            await tcs.Task;
                            await Task.Delay(5000);

                            if (webLayout.Url.ToString() != agencies_URL)
                            {
                                SendNotificationForError(
                                    "Truy cập vào đại lý bị lỗi",
                                    $"{web_name} : Trang đại lý bị lỗi");
                                process = "Finish";
                                break;
                            }

                            process = "SearchUser";
                            break;
                        case "SearchUser":
                            currentMessage = data.FirstOrDefault();
                            userAccount = SearchUser();
                            if (userAccount == null)
                            {
                                // save record
                                SaveRecord($"Không tìm thấy user " +
                                $"{web_name} : user id {currentMessage.Account}");

                                data.Remove(currentMessage);
                                if (data.Count == 0)
                                {
                                    process = "Finish";
                                    break;
                                }
                                process = "OpenWeb";
                                break;
                            }
                            process = "AccessToPayIn";
                            break;
                        case "AccessToPayIn":
                            AccessToPayIn(userAccount);
                            await Task.Delay(5000);

                            if (!webLayout.Url.ToString().Contains(addMoney_URL))
                            {
                                var errorMessage = $"" +
                                    $"Truy cập trang cộng tiền web {web_name} bị lỗi hoặc" +
                                    $" {web_name} : không tìm thấy account của User id {userAccount.IDAccount}";
                                SendNotificationForError(
                                    "Truy cập vào trang cộng tiền bị lỗi", errorMessage);

                                SaveRecord(errorMessage);
                                data.Remove(currentMessage);
                                if (data.Count == 0)
                                {
                                    process = "Finish";
                                    break;
                                }
                                process = "OpenWeb";
                                break;
                            }

                            process = "PayIn";
                            break;
                        case "PayIn":
                            CreateSyncTask();
                            PayIn();
                            await tcs.Task;
                            await Task.Delay(5000);

                            if (!webLayout.Url.ToString().Contains(agencies_URL))
                            {
                                var errorMessage = $"Cộng tiền account { currentMessage.Account } bị lỗi";
                                SendNotificationForError(
                                    "Cộng tiền không thành công",
                                    $"{web_name} : Cộng tiền account { currentMessage.Account } bị lỗi");

                                SaveRecord(errorMessage);
                            }
                            else
                            {
                                SendNotificationForError(
                                    "Cộng tiền thành công",
                                    $"{web_name} : Cộng tiền thành công account { currentMessage.Account }, " +
                                    $"số tiền { currentMessage.Money }");
                                SaveRecord();
                            }

                            data.Remove(currentMessage);
                            if (data.Count == 0)
                            {
                                process = "Finish";
                                break;
                            }
                            process = "OpenWeb";
                            break;
                        case "Finish":
                            isFinishProcess = true;
                            webLayout.Navigate("about:blank");
                            break;
                    }
                } while (!isFinishProcess);
            }
            catch (Exception ex)
            {
                isFinishProcess = true;
                if (ex.Message.Contains("Lỗi không có kết nối internet"))
                {
                    Application.Exit();
                    
                }

                SendNotificationForError(
                    "Lỗi không xác định",
                    $"{web_name} : {ex.Message}");
            }

            return;
        }

        private void CreateSyncTask()
        {
            tcs = new TaskCompletionSource<Void>();
            webLayout.ScriptErrorsSuppressed = true;
            webLayout.DocumentCompleted += documentComplete;
        }

        private void Login(AdminAccount adminAccount)
        {
            var htmlLogin = webLayout.Document;
            var inputUserName = htmlLogin.GetElementById("Username");
            var inputPassword = htmlLogin.GetElementById("Password");
            var btnLogin = htmlLogin.GetElementById("login");

            if (inputUserName != null && inputPassword != null)
            {
                inputUserName.SetAttribute("value", adminAccount.AccountName);
                inputPassword.SetAttribute("value", adminAccount.Password);
                btnLogin.InvokeMember("Click");
            }
        }

        private void AccessToDaily()
        {
            var htmlIndex = webLayout.Document;
            var aTag = htmlIndex.GetElementsByTagName("a");
            foreach (HtmlElement item in aTag)
            {
                var href = item.GetAttribute("href");
                if (href != null && href == agencies_URL)
                {
                    item.InvokeMember("Click");
                    break;
                }
            }
        }

        private AccountData SearchUser()
        {
            MongoDatabase<AccountData> accountData = new MongoDatabase<AccountData>(typeof(AccountData).Name);
            var userAccount = accountData.
                Query.Where(x => x.IDAccount == currentMessage.Account).FirstOrDefault();

            if (userAccount == null || string.IsNullOrEmpty(userAccount.HLC))
                return null;

            var html = webLayout.Document;
            var userFilter = html.GetElementById("phone");
            userFilter.SetAttribute("value", userAccount.HLC);
            var aTag = html.GetElementsByTagName("a");
            foreach (HtmlElement item in aTag)
            {
                var btnTimKiem = item.InnerHtml;
                if (btnTimKiem == "TÌM KIẾM")
                {
                    item.InvokeMember("Click");
                    break;
                }
            }
            return userAccount;
        }

        private bool CheckAmountAccount()
        {
            try
            {
                HtmlElement tdResult = null;
                var html = webLayout.Document;
                var table = html.GetElementsByTagName("table")[0];
                var trs = table.GetElementsByTagName("tr");
                foreach (HtmlElement tr in trs)
                {
                    var tds = tr.GetElementsByTagName("td");
                    foreach (HtmlElement td in tds)
                    {
                        try
                        {
                            string value = td.InnerText;
                            if (value != null && value.Contains("SỐ DƯ TÀI KHOẢN"))
                            {
                                tdResult = tds[1]; //[1] is amount of money
                                break;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                if (tdResult != null)
                {
                    decimal outMoney = 0;
                    return (decimal.TryParse(tdResult.InnerHtml.Replace("VNĐ", "").Trim(), out outMoney) 
                        && outMoney >= Constant.SATISFIED_AMOUNT_ACCOUNT);
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void AccessToPayIn(AccountData accountData)
        {
            HtmlElement trFound = null;
            var html = webLayout.Document;
            var table = html.GetElementsByTagName("table")[0];
            var trs = table.GetElementsByTagName("tr");
            foreach (HtmlElement tr in trs)
            {
                var tds = tr.GetElementsByTagName("td");
                foreach (HtmlElement td in tds)
                {
                    try
                    {
                        string value = td.InnerText;
                        if (value != null && value == accountData.HLC)
                        {
                            trFound = tr;
                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            if (trFound != null)
            {
                var aTag = trFound.GetElementsByTagName("a");
                foreach (HtmlElement item in aTag)
                {
                    var btnTimKiem = item.InnerHtml;
                    if (btnTimKiem == "CỘNG TIỀN")
                    {
                        item.InvokeMember("Click");
                        break;
                    }
                }
            }
        }

        private void PayIn()
        {
            var html = webLayout.Document;
            var amount = html.GetElementById("Amount");
            var btnAdd = html.GetElementById("add_money_button");
            amount.SetAttribute("value", Constant.TEST_MONEY.ToString()); //TODO
            btnAdd.InvokeMember("Click");
            //Thread.Sleep(100);
            //var html = webLayout.Document;
            //var amount = html.GetElementsByTagName("input");
            //foreach (HtmlElement item in amount)
            //{
            //    var value = item.GetAttribute("value");
            //    if (value == "BACK")
            //    {
            //        item.InvokeMember("Click");
            //        break;
            //    }
            //}
        }

        private string checkAccountAdmin(ref AdminAccount account)
        {
            var dataAccount = new MongoDatabase<AdminAccount>(typeof(AdminAccount).Name);
            var accountToPay = dataAccount.Query.Where(x => x.Web == web_name).FirstOrDefault();
            if (accountToPay != null)
            {
                account = accountToPay;
                return "OpenWeb";
            }

            SendNotificationForError(
                "Lỗi Account Admin",
                $"Không lấy được hoặc không tồn tại account admin trang web {web_name}");

            return "Finish";
        }

        private void SendNotificationForError(string subject, string message)
        {
            mailService.SendEmail(subject, message);
        }

        private void SaveRecord(string error = "")
        {
            MongoDatabase<Message> database = new MongoDatabase<Message>(typeof(Message).Name);
            var updateOption = Builders<Message>.Update
            .Set(p => p.IsProcessed, true)
            .Set(p => p.Error, error)
            .Set(p => p.DateExcute, DateTime.Now);

            database.UpdateOne(x => x.Id == currentMessage.Id, updateOption);
        }
    }
}
