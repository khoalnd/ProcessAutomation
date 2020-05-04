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
        StringBuilder data = new StringBuilder();
        private string web_name = "cay bang";
        private string index_URL = "https://caybang.club/Login";
        private string user_URL = "https://caybang.club/Users";
        private string daily_URL = "https://caybang.club/Users/Agencies";
        private bool isFinishProcess = true;

        public CBSite(StringBuilder data, WebBrowser web)
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
                            webLayout.Navigate(index_URL);
                            await tcs.Task;

                            if (!(webLayout.DocumentText.Contains("ĐĂNG NHẬP")))
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
                            webLayout.ScriptErrorsSuppressed = true;
                            webLayout.DocumentCompleted += documentComplete;
                            await tcs.Task;

                            if (webLayout.Url.ToString() == daily_URL)
                            {
                                SendNotificationForError(
                                    "Truy cập vào đại lý bị lỗi", $"Trang đại lý web {web_name} bị lỗi");
                                isFinishProcess = true;
                                break;
                            }

                            process = "SearchUser";
                            break;
                        case "SearchUser":
                            SearchUser(ref isFinishProcess);
                            break;
                        case "PayIn":
                            break;
                        case "SendNotification":
                            break;
                        default:
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
            var inputUserName = htmlLogin.GetElementById("Username");
            var inputPassword = htmlLogin.GetElementById("Password");
            var btnLogin = htmlLogin.GetElementById("login");

            if (inputUserName != null && inputPassword != null)
            {
                inputUserName.SetAttribute("value", "autobank");
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
                if (href != null && href == daily_URL)
                {
                    item.InvokeMember("Click");
                    break;
                }
            }
        }

        private void SearchUser(ref bool isFinishProcess)
        {
            var html = webLayout.Document;
            var userFilter = html.GetElementById("phone");
            userFilter.SetAttribute("value", "");
            userFilter.SetAttribute("value", "abc");
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
        }

        private void PayIn(ref bool isFinishProcess)
        {
            // TODO
        }

        private void SendNotification(ref bool isFinishProcess)
        {
            // TODO
        }

        private void SendNotificationForError(string subject, string message)
        {
            //mailService.SendEmail(subject, message);
            isFinishProcess = true;
        }
    }
}
