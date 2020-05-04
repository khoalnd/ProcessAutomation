using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessAutomation.Main.Ultility
{
    public class Constant
    {
        #region Email Setting
        public const string SMTP_ADDRESS = "smtp.gmail.com";
        public const int PORT_NUMBER = 587;
        public const bool ENABLE_SSL = true;
        public const string EMAIL_FROM_ID = "processautomationmailsystem@gmail.com";
        public const string EMAIL_FROM_PASS = "automation2020";
        public const string EMAIL_TO = "haivovan20101994@gmail.com"; //"Phanminhchau2906@gmail.com";
        #endregion

        #region Extract Message
        public const string REG_EXTRACT_MESSAGE = "(\\+CMGL: \\d+)+(,\".*?\",)+(\".*?\",)+(,\".*?\")+(\n|\r\n)+(.*)";
        public const string REG_EXTRACT_MONEY = "(tang)+(.*?VND)";
        public const string REG_EXTRACT_ACCOUNT = "( ND )+(cb|hlc|30s|gdvn)+(.*? )";
        public static List<string> WEBS_NAME = new List<string> { "cb", "hlc", "30s", "gdvn" };
        #endregion
    }
}
