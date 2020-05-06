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
    public class GDSite : IAutomationPayIn
    {
        MailService mailService = new MailService();
        Helper helper = new Helper();
        WebBrowser webLayout;
        List<Message> data = new List<Message>();
        private const string web_name = "gia dinh vina";
        private const string url = "https://giadinhvina.com.vn/";
        private const string index_URL = url + "HIMONEY/HiMM/";
        private const string user_URL = url + "HIMONEY/HiMM/helloVMV.php";
        private const string agencies_URL = url + "HIMONEY/HiMM/chuyenkhoan.php";
        private const string addMoney_URL = url + "Users/AddMoneyToUser";
        private bool isFinishProcess = true;
        Message currentMessage;

        public GDSite(List<Message> data, WebBrowser web)
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
                HtmlDocument doc = webLayout.Document;
                HtmlElement head = doc.GetElementsByTagName("head")[0];
                HtmlElement script = doc.CreateElement("script");
                script.SetAttribute("text", "window.alert = " +
                    "function (e) {" +
                    "setTimeout(function(){ window.location.replace('https://giadinhvina.com.vn/HIMONEY/HiMM/helloVMV.php'); }, 10000);" +
                    "};");
                head.AppendChild(script);
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
                            webLayout.Navigate(index_URL);
                            await tcs.Task;

                            if (!(webLayout.DocumentText.Contains("Đăng nhập")))
                            {
                                if (triedAccessWeb == 5)
                                {
                                    SendNotificationForError(
                                            "Trang Web Không Truy Cập Được",
                                            $"Trang Web {web_name} không thể truy cập");

                                    isFinishProcess = true;
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
                            if (!(webLayout.Url.ToString() == user_URL))
                            {
                                tcs = new TaskCompletionSource<Void>();
                                Login();
                             
                                webLayout.ScriptErrorsSuppressed = true;
                                webLayout.DocumentCompleted += documentComplete;
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
                                    isFinishProcess = true;
                                    break;
                                }
                            }
                            process = "AccessToDaily";
                            break;
                        case "AccessToDaily":
                            tcs = new TaskCompletionSource<Void>();
                            AccessToDaily();
                            //webLayout.Document.InvokeScript("sayHello");

                            webLayout.ScriptErrorsSuppressed = true;
                            webLayout.DocumentCompleted += documentComplete;
                            await tcs.Task;

                            if (!(webLayout.Url.ToString() == agencies_URL))
                            {
                                SendNotificationForError(
                                    "Truy cập vào đại lý bị lỗi", $"Trang đại lý web {web_name} bị lỗi");
                                isFinishProcess = true;
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
                                if (data.Count > 0)
                                {
                                    process = "OpenWeb";
                                }
                                else
                                {
                                    isFinishProcess = true;
                                }
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
                            if (data.Count > 0)
                            {
                                process = "OpenWeb";
                            }
                            else {
                                isFinishProcess = true;
                            }
                            break;
                    }
                } while (!isFinishProcess || !helper.CheckInternetConnection());

            }
            catch (Exception ex)
            {
                throw;
            }
            
            return;
        }

        private void Login() 
        {
            var htmlLogin = webLayout.Document;
            var inputTag = htmlLogin.GetElementsByTagName("input");
            foreach (HtmlElement item in inputTag)
            {
                var name = item.GetAttribute("name");
                if (name != null && name == "txtemail")
                {
                    item.SetAttribute("value", "autobank@gmail.com");
                }
                else if (name != null && name == "ntxtupass")
                {
                    item.SetAttribute("value", "Abc@12345");
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
            MongoDatabase<AccountData> accountData = new MongoDatabase<AccountData>("AccountData");
            var userAccount = accountData.
                Query.Where(x => x.IDAccount == currentMessage.Account).FirstOrDefault();

            if (userAccount == null || string.IsNullOrEmpty(userAccount.GD))
                return null;

            return userAccount;
        }

        private void PayIn()
        {
            var html = webLayout.Document;
            var amount = html.GetElementsByTagName("input");
            foreach (HtmlElement item in amount)
            {
                var value = item.GetAttribute("name");
                if (value == "txt_email")
                {
                    item.SetAttribute("value", "Autobank2@gmail.com");
                    break;
                }
                else if (value == "txt_gia")
                {
                    item.SetAttribute("value", "20000");
                    break;
                }
            }
            var button = html.GetElementsByTagName("button");
            foreach (HtmlElement item in button)
            {
                var btnSubmit = item.GetAttribute("name");
                if (btnSubmit == "chuyenkhoantronghethong")
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
