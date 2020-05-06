using MongoDB.Bson;
using MongoDB.Driver;
using ProcessAutomation.DAL;
using ProcessAutomation.Main.Services;
using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessAutomation.Main.PayIn
{
    public class CBSite : IAutomationPayIn
    {
        MailService mailService = new MailService();
        Helper helper = new Helper();
        WebBrowser webLayout;
        List<Message> data = new List<Message>();
        private const string web_name = "cay bang";
        private const string url = "https://caybang.club/";
        private const string index_URL = url + "Login";
        private const string user_URL = url + "Users";
        private const string agencies_URL = url + "Users/Agencies";
        private const string addMoney_URL = url + "Users/AddMoneyToUser";
        private bool isFinishProcess = true;
        Message currentMessage;

        public CBSite(List<Message> data, WebBrowser web)
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
            Void v;
            TaskCompletionSource<Void> tcs = null;
            WebBrowserDocumentCompletedEventHandler documentComplete = null;
            documentComplete = new WebBrowserDocumentCompletedEventHandler((s, e) =>
            {
                webLayout.DocumentCompleted -= documentComplete;
                tcs.SetResult(v);
            });

            isFinishProcess = false;
            var triedAccessWeb = 1;
            try
            {
                var process = "OpenWeb";
                do
                {
                    switch (process)
                    {
                        case "OpenWeb":
                            tcs = new TaskCompletionSource<Void>();
                            webLayout.ScriptErrorsSuppressed = true;
                            webLayout.DocumentCompleted += documentComplete;
                            webLayout.Navigate(url);
                            await tcs.Task;

                            if (webLayout.Url.ToString() != index_URL)
                            {
                                if (triedAccessWeb == 5)
                                {
                                    SendNotificationForError(
                                            "Trang Web Không Truy Cập Được",
                                            $"Trang Web {web_name} không thể truy cập");

                                    process = "Finish";
                                    break;
                                }
                                else
                                {
                                    Thread.Sleep(60000);
                                    triedAccessWeb++;
                                    process = "OpenWeb";
                                }
                            }
                            else
                            {
                                process = "Login";
                            }
                            break;
                        case "Login":
                            // check already has cookie
                            if (webLayout.Url.ToString() == index_URL)
                            {
                                tcs = new TaskCompletionSource<Void>();
                                Login();
                                webLayout.ScriptErrorsSuppressed = true;
                                webLayout.DocumentCompleted += documentComplete;
                                throw new Exception("test");
                                await tcs.Task;

                                if (webLayout.Url.ToString() == index_URL)
                                {
                                    SendNotificationForError("Account Đăng Nhập Lỗi", $"Account đăng nhập web {web_name} bị lỗi");
                                    process = "Finish";
                                    break;
                                }
                            }
                            process = "AccessToDaily";
                            break;
                        case "AccessToDaily":
                            tcs = new TaskCompletionSource<Void>();
                            AccessToDaily();
                            webLayout.ScriptErrorsSuppressed = true;
                            webLayout.DocumentCompleted += documentComplete;
                            await tcs.Task;

                            if (webLayout.Url.ToString() != agencies_URL)
                            {
                                SendNotificationForError(
                                    "Truy cập vào đại lý bị lỗi", $"Trang đại lý web {web_name} bị lỗi");
                                process = "Finish";
                                break;
                            }

                            process = "SearchUser";
                            break;
                        case "SearchUser":
                            currentMessage = data.FirstOrDefault();
                            var accountFound = SearchUser();
                            if(accountFound == null)
                            {
                                // save record
                                SaveRecord("Không tìm thấy user");
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
                            tcs = new TaskCompletionSource<Void>();
                            AccessToPayIn();
                            webLayout.ScriptErrorsSuppressed = true;
                            webLayout.DocumentCompleted += documentComplete;
                            await tcs.Task;

                            if (!webLayout.Url.ToString().Contains(addMoney_URL))
                            {
                                var errorMessage = $"Trang cộng tiền web {web_name} bị lỗi";
                                SendNotificationForError(
                                    "Truy cập vào trang cộng tiền bị lỗi", errorMessage);

                                process = "Finish";
                                break;
                            }

                            process = "PayIn";
                            break;
                        case "PayIn":
                            tcs = new TaskCompletionSource<Void>();
                            PayIn();
                            webLayout.ScriptErrorsSuppressed = true;
                            webLayout.DocumentCompleted += documentComplete;
                            await tcs.Task;

                            if (!webLayout.Url.ToString().Contains(agencies_URL))
                            {
                                var errorMessage = $"Cộng tiền account { currentMessage.Account } ở web {web_name} bị lỗi";
                                SendNotificationForError(
                                    "Cộng tiền không thành công", $"Cộng tiền account { currentMessage.Account } ở web {web_name} bị lỗi");

                                SaveRecord(errorMessage);
                            }
                            else
                            {
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
                            break;
                    }
                } while (!isFinishProcess || !helper.CheckInternetConnection());

            }
            catch (Exception)
            {
                isFinishProcess = true;
                throw;
            }
            
            return;
        }

        private void Login() 
        {
            var htmlLogin = webLayout.Document;
            var inputUserName = htmlLogin.GetElementById("Username");
            var inputPassword = htmlLogin.GetElementById("Password");
            var btnLogin = htmlLogin.GetElementById("login");

            if (inputUserName != null && inputPassword != null)
            {
                inputUserName.SetAttribute("value", "aut3obank");
                inputPassword.SetAttribute("value", "Abc@12345");
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
            MongoDatabase<AccountData> accountData = new MongoDatabase<AccountData>("AccountData");
            var userAccount = accountData.
                Query.Where(x => x.IDAccount == currentMessage.Account).FirstOrDefault();

            if (userAccount == null || string.IsNullOrEmpty(userAccount.CB))
                return userAccount;

            var html = webLayout.Document;
            var userFilter = html.GetElementById("phone");
            userFilter.SetAttribute("value", userAccount.CB);
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
            Thread.Sleep(100);
            return userAccount;
        }

        private void AccessToPayIn()
        {
            var html = webLayout.Document;
            var aTag = html.GetElementsByTagName("a");
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

        private void PayIn()
        {
            //var html = webLayout.Document;
            //var amount = html.GetElementById("Amount");
            //var btnAdd = html.GetElementById("add_money_button");
            //amount.SetAttribute("value", currentMessage.Money);
            //btnAdd.InvokeMember("Click");
            //Thread.Sleep(100);
            var html = webLayout.Document;
            var amount = html.GetElementsByTagName("input");
            foreach (HtmlElement item in amount)
            {
                var value = item.GetAttribute("value");
                if (value == "BACK")
                {
                    item.InvokeMember("Click");
                    break;
                }
            }
        }

        private void SendNotificationForError(string subject, string message)
        {
            //mailService.SendEmail(subject, message);
        }

        private void SaveRecord(string error = "")
        {
            MongoDatabase<Message> database = new MongoDatabase<Message>("Message");
            var updateOption = Builders<Message>.Update
            .Set(p => p.IsProcessed, true)
            .Set(p => p.Error, error);

            database.UpdateOne(x => x.Id == currentMessage.Id, updateOption);
        }
    }
}
