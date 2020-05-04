using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MongoDB.Driver;
using ProcessAutomation.DAL;

namespace ProcessAutomation.Main.Services
{
    public class MessageService
    {
        MongoDatabase<Message> database = new MongoDatabase<Message>("Message");
        CSV csvHelper = new CSV();
        public void StartReadMessage(SerialPort serialPort)
        {
            serialPort.Write("AT+CMGF=1" + Environment.NewLine);
            serialPort.Write("AT+CMGL=\"ALL\"" + Environment.NewLine);
            System.Threading.Thread.Sleep(500);
            var response = serialPort.ReadExisting();
            var rule = new Regex(Constant.REG_EXTRACT_MESSAGE);
            var matches = rule.Matches(response);
            SaveMessage(matches);

            // Delele message after read from sim
            //serialPort.Write("AT+CMGD=,4" + Environment.NewLine);
            //System.Threading.Thread.Sleep(50);
            //serialPort.ReadExisting();
        }

        public List<Message> ReadMessage()
        {
            return database.Query.Where(x => x.IsProcessed == false && x.IsSatisfied == true).ToList();
        }

        private void SaveMessage(MatchCollection matches)
        {
            try
            {
                if (matches.Count > 0)
                {
                    var dataToWrite = new StringBuilder();
                    List<Message> messages = new List<Message>();
                    foreach (Match match in matches)
                    {
                        var mess = AnalyzeMessage(match.Groups[6].ToString());
                        mess.RecievedDate = string.Join(
                            string.Empty, match.Groups[4].Value.Trim()
                            .Replace("+00", "")
                            .Replace("\"", "")
                            .Skip(1));
                        mess.MessageContent = match.Groups[6].Value.Trim();

                        dataToWrite.AppendFormat(
                            "{0},{1},{2},{3},{4},{5},{6},{7}",
                            mess.Account,
                            mess.Money,
                            mess.Web,
                            mess.RecievedDate,
                            mess.IsSatisfied,
                            mess.IsProcessed,
                            mess.Error,
                            mess.MessageContent + Environment.NewLine
                        );
                        messages.Add(mess);
                    }
                    csvHelper.WriteToFile(dataToWrite, $"{DateTime.Now.ToString("dd-MM-yyyy")}.csv");
                    database.InsertMany(messages);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Message AnalyzeMessage(string mess)
        {
            var result = new Message();

            // Check account
            var matches = new Regex(Constant.REG_EXTRACT_ACCOUNT).Match(mess).Groups;
            if (matches.Count > 0 && matches[1].Value != "")
            {
                result.Web = matches[2].ToString().Trim();
                result.Account = matches[3].ToString().Trim();
                result.IsSatisfied = Constant.WEBS_NAME
                .Any(x => result.Web.Contains(x));
            }
            
            // Check money
            if (result.IsSatisfied)
            {
                var match = new Regex(Constant.REG_EXTRACT_MONEY).Match(mess).Groups[2] ?? null;
                if (string.IsNullOrEmpty(match.Value))
                    return result;

                var money = match.Value?.Replace("VND", "");
                decimal outMoney = 0;

                result.IsSatisfied = false;
                if (decimal.TryParse(money, out outMoney)/* && outMoney >= 500000*/)
                {
                    result.Money = outMoney.ToString();
                    result.IsSatisfied = true;
                }
            }
            return result;
        }
    }
}
