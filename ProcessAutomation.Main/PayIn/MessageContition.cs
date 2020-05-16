using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessAutomation.Main.PayIn
{
    public class MessageContition
    {
        public List<string> WebSRun { get; set; }

        public MessageContition()
        {
            WebSRun = new List<string>();
        }
    }
}
