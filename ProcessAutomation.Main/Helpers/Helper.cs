using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProcessAutomation.Main.Helpers
{
    public class Helper
    {
        public static bool CheckInternetConnection()
        {
			try
			{
                using (var client = new WebClient() { Proxy = null })
                using (client.OpenRead("http://www.google.com"))
                {
                    return true;
                }

            }
			catch (Exception)
			{
                return false;
			}
        }

        public string DecodeFromHexToString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
