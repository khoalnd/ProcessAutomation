using MongoDB.Bson;
using MongoDB.Driver;
using ProcessAutomation.DAL;
using ProcessAutomation.Main.Services;
using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessAutomation.Main.PayIn
{
    public class NT30sSite : IAutomationPayIn
    {
        MailService mailService = new MailService();
        Helper helper = new Helper();
        WebBrowser webLayout;
        List<Message> data = new List<Message>();
        private const string web_name = "naptien30s";
        private const string url = "https://naptien30s.vn/";
        private const string index_URL = url + "HIMONEY/HiMM/";
        private const string user_URL = url + "HIMONEY/HiMM/helloVMV.php";
        private const string agencies_URL = url + "HIMONEY/HiMM/chuyenkhoan.php";
        private bool isFinishProcess = true;
        Message currentMessage;
        Void v;
        TaskCompletionSource<Void> tcs = null;
        WebBrowserDocumentCompletedEventHandler documentComplete = null;
        WebBrowserNavigatedEventHandler documentNavigatedComplete = null;

        public NT30sSite(List<Message> data, WebBrowser web)
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
            documentComplete = new WebBrowserDocumentCompletedEventHandler((s, e) =>
            {
                HtmlDocument doc = webLayout.Document;
                HtmlElement head = doc.GetElementsByTagName("head")[0];
                HtmlElement script = doc.CreateElement("script");
                script.SetAttribute("text", "window.alert = function(e){" +
                    "if(e.indexOf('Tai Khoan Cua Ban Da Dang Nhap')" +
                    "{ window.location.replace(" + url + ");}" +
                    "else { };");
                head.AppendChild(script);
                webLayout.DocumentCompleted -= documentComplete;
                tcs.SetResult(v);
            });
           
            documentNavigatedComplete = new WebBrowserNavigatedEventHandler((s, e) =>
            {
                HtmlDocument doc = webLayout.Document;
                HtmlElement head = doc.GetElementsByTagName("head")[0];
                HtmlElement script = doc.CreateElement("script");
                script.SetAttribute("text", "window.alert = function(e){" +
                    "if(e.indexOf('Chuyển Khoản Thành Công') !== -1) {}" +
                    "else{window.location.replace(" + url + ");}};");
                head.AppendChild(script);
                webLayout.Navigated -= documentNavigatedComplete;
            });

            isFinishProcess = false;
            var triedAccessWeb = 1;
            var adminAccount = new AdminAccount();
            try
            {
                var process = checkAccountAdmin(ref adminAccount);
                do
                {
                    switch (process)
                    {
                        case "OpenWeb":
                            CreateSyncTask();
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
                                triedAccessWeb = 1;
                                process = "Login";
                            }
                            break;
                        case "Login":
                            // check already has cookie
                            if (webLayout.Url.ToString() != user_URL)
                            {
                                CreateSyncTask();
                                Login(adminAccount);
                                await tcs.Task;

                                if (webLayout.Url.ToString() == index_URL)
                                {
                                    var subject = "Account Đăng Nhập Lỗi";
                                    if (webLayout.DocumentText.Contains("TÀI KHOẢN HOẶC MẬT KHẨU KHÔNG ĐÚNG"))
                                    {
                                        SendNotificationForError(
                                            subject, $"Account đăng nhập web {web_name} bị lỗi");
                                    }
                                    else
                                    {
                                        SendNotificationForError(
                                            subject, $"Lỗi không xác định khi đăng nhập web {web_name}");
                                    }
                                    process = "Finish";
                                    break;
                                }
                            }
                            process = "AccessToDaily";
                            break;
                        case "AccessToDaily":
                            CreateSyncTask();
                            AccessToDaily();
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
                            process = "PayIn";
                            break;
                        case "PayIn":
                            CreateSyncTask();
                            PayIn();
                            webLayout.Navigated += documentNavigatedComplete;
                            await tcs.Task;

                            if (!webLayout.Url.ToString().Contains(agencies_URL))
                            {
                                var errorMessage = $"Cộng tiền account { currentMessage.Account } ở web {web_name} bị lỗi";
                                SendNotificationForError(
                                    "Cộng tiền không thành công", $"Cộng tiền account { currentMessage.Account } ở web {web_name} bị lỗi");

                                //SaveRecord(errorMessage);
                            }
                            else
                            {
                                //SaveRecord();
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
                throw;
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
            var inputTag = htmlLogin.GetElementsByTagName("input");
            foreach (HtmlElement item in inputTag)
            {
                var name = item.GetAttribute("name");
                if (name != null && name == "txtemail")
                {
                    item.SetAttribute("value", adminAccount.AccountName);
                }
                else if (name != null && name == "ntxtupass")
                {
                    item.SetAttribute("value", adminAccount.Password);
                }
            }

            var btnLogin = htmlLogin.GetElementById("btn-login");
            btnLogin.InvokeMember("Click");
        }
       
        private void AccessToDaily()
        {
            var htmlIndex = webLayout.Document;
            var aTag = htmlIndex.GetElementsByTagName("a");
            foreach (HtmlElement item in aTag)
            {
                var href = item.GetAttribute("href");
                var innerHTML = item.InnerHtml;
                if (href != null && href == agencies_URL && innerHTML.Contains("Chuyển Khoản Cấp Dưới"))
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

            if (userAccount == null || string.IsNullOrEmpty(userAccount.NT))
                return null;

            return userAccount;
        }

        void PayIn()
        {
            var html = webLayout.Document;
            var amount = html.GetElementsByTagName("input");
            foreach (HtmlElement item in amount)
            {
                var value = item.GetAttribute("name");
                if (value == "txt_email")
                {
                    item.SetAttribute("value", "Autobank1@gmail.com");
                }
                else if (value == "txt_gia")
                {
                    item.SetAttribute("value", "20000");
                }
            }
            var button = html.GetElementsByTagName("button");
            foreach (HtmlElement item in button)
            {
                var btnSubmit = item.GetAttribute("name");
                //"kiemtraemailnguoinhan"
                //"chuyenkhoantronghethong"
                if (btnSubmit == "chuyenkhoantronghethong")
                {
                    item.InvokeMember("Click");
                    break;
                }
            }
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
            //mailService.SendEmail(subject, message);
        }

        private void SaveRecord(string error = "")
        {
            MongoDatabase<Message> database = new MongoDatabase<Message>(typeof(Message).Name);
            var updateOption = Builders<Message>.Update
            .Set(p => p.IsProcessed, true)
            .Set(p => p.Error, error);

            database.UpdateOne(x => x.Id == currentMessage.Id, updateOption);
        }
    }
}
