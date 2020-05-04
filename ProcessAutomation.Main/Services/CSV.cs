using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessAutomation.Main.Services
{
    public class CSV
    {
        private string PATH = AppDomain.CurrentDomain.BaseDirectory;
        private StringBuilder HEADER = new StringBuilder(
            "taikhoan, sotien, web, ngaynhan, hople, daxuly, loi, noidung " + Environment.NewLine);

        public void WriteToFile(StringBuilder data, string fileName)
        {
            StringBuilder dataToWrite = data;
            string filePath = Path.Combine(PATH, $"{fileName}");
            if (!File.Exists(filePath))
            {
                dataToWrite = HEADER.Append(data);
            }
            File.AppendAllText(filePath, dataToWrite.ToString());
        }

        public StringBuilder ReadFromFile(string fileName)
        {
            StringBuilder listA = new StringBuilder();
            using (var reader = new StreamReader(Path.Combine(PATH, $"{fileName}")))
            {                
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    listA.Append(line);
                }
            }
            return listA;
        }

    }
}
