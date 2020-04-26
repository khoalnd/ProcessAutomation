using ProcessAutomation.Main.Ultility;
using System.Net;
using System.Net.Mail;

namespace ProcessAutomation.Main.Services
{
    public class MailService
    {
        public void SendEmail(string subject, string content)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(Constant.EMAIL_FROM_ID);
                mail.To.Add(Constant.EMAIL_TO);
                mail.Subject = subject;
                mail.Body = content;

                using (SmtpClient smtp = new SmtpClient(Constant.SMTP_ADDRESS, Constant.PORT_NUMBER))
                {
                    smtp.Credentials = new NetworkCredential(Constant.EMAIL_FROM_ID, Constant.EMAIL_FROM_PASS);
                    smtp.EnableSsl = Constant.ENABLE_SSL;
                    smtp.Send(mail);
                }
            }
        }
    }
}
