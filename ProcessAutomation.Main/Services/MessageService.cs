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
        CSV csvHelper = new CSV();
        const string regExtractMessage = "(\\+CMGL: \\d+)+(,\".*?\",)+(\".*?\",)+(,\".*?\")+(\n|\r\n)+(.*)";
        const string regExtractMoney = "(tang)+(.*?VND)";
        const string regExtractAccount = "( ND )+(.*? )";
        List<string> webs = new List<string> { "cb","hlc","30s", "gdvn" }; 

        public void StartReadMessage(SerialPort serialPort)
        {
            serialPort.Write("AT+CMGF=1" + Environment.NewLine);
            serialPort.Write("AT+CMGL=\"ALL\"" + Environment.NewLine);
            System.Threading.Thread.Sleep(50);
            var response = serialPort.ReadExisting();
            var rule = new Regex(regExtractMessage);
            var matches = rule.Matches(response);
            SaveMessage(matches);
            
            // Delele message after read from sim

            //serialPort.Write("AT+CMGD=,4" + Environment.NewLine);
            //System.Threading.Thread.Sleep(50);
            //serialPort.ReadExisting();
        }

        public void ReadMessage()
        {
            var test = csvHelper.ReadFromFile($"{DateTime.Now.ToString("dd-MM-yyyy")}.csv");
            csvHelper.WriteToFile(test, $"{DateTime.Now.ToString("dd-MM-yyyy")}_result.csv");
        }

        private void SaveMessage(MatchCollection matches)
        {
            try
            {
                if (matches.Count > 0)
                {
                    var dataToWrite = new StringBuilder();
                    foreach (Match match in matches)
                    {
                        var messAfterAnalyze = AnalyzeMessage(match.Groups[6].ToString());

                        dataToWrite.AppendFormat(
                            "{0},{1},{2},{3},{4},{5}",
                            messAfterAnalyze.Item1,
                            messAfterAnalyze.Item2,
                            match.Groups[3],
                            match.Groups[6].Value.Replace("\r",""),
                            messAfterAnalyze.Item3.ToString(),
                            "0" + Environment.NewLine
                        );
                    }
                    csvHelper.WriteToFile(dataToWrite, $"{DateTime.Now.ToString("dd-MM-yyyy")}.csv");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private (string, string, bool) AnalyzeMessage(string mess)
        {
            var money = string.Empty;
            var account = string.Empty;
            var isSatisfied = false;

            var match = new Regex(regExtractMoney).Match(mess).Groups[2] ?? null;
            if (string.IsNullOrEmpty(match.Value))
                return (money, account, isSatisfied);

            money = match.Value?.Replace("VND", "");
            decimal outMoney = 0;
            if (decimal.TryParse(money, out outMoney) && outMoney >= 500000)
            {
                money = outMoney.ToString();
                isSatisfied = true;
            }

            if (!isSatisfied)
                return (money, account, isSatisfied);

            match = new Regex(regExtractAccount).Match(mess).Groups[2] ?? null;
            if (string.IsNullOrEmpty(match.Value))
                return (money, account, !isSatisfied);

            account = match.Value;
            isSatisfied = webs
                .Any(x => match.Value.Contains(x));

            return (money, account, isSatisfied);
        }
    }
}
