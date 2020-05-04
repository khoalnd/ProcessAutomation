using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessAutomation.Main.PayIn
{
    interface IAutomationPayIn
    {
        void startPayIN();
        bool checkProcessDone();
    }
}
