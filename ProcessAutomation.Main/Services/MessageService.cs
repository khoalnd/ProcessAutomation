using ProcessAutomation.Main.Helpers;
using ProcessAutomation.Main.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProcessAutomation.Main.Services
{
    public class MessageService
    {
        MailService mail = new MailService();
        Helper helper = new Helper();
        const string regAnalyzeMessage = "(\\+CMGL: \\d+)+(,\".*?\",)+(\".*?\",)+(,\".*?\")+(\n|\r\n)+(.*)";
        
        public void StartReadMessage1(object sender, SerialDataReceivedEventArgs e) {
            SerialPort sp = (SerialPort)sender;
            string inMessage = sp.ReadLine();
        }

        public void StartReadMessage(SerialPort serialPort)
        {
            serialPort.Write("AT+CMGF=1" + Environment.NewLine);
            serialPort.Write("AT+CMGL=\"ALL\"" + Environment.NewLine);
            System.Threading.Thread.Sleep(1000);
            var response = serialPort.ReadExisting();
            var rule = new Regex(regAnalyzeMessage);
            var matches = rule.Matches(response);
            //foreach(Match item in matches)
            //{
            //    var groups = item;
            //}
        }

        private void ClassifyMessage()
        {

        }

        private void SaveMessage()
        {

        }
    }
}
