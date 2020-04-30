using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessAutomation.Main.Services
{
    public class DevicePortCOMService
    {
        private bool isPortWork(SerialPort serialPort)
        {
            try
            {
                serialPort.Write("AT" + Environment.NewLine);
                System.Threading.Thread.Sleep(50);
                var response = serialPort.ReadExisting();
                return response.Contains("OK");
            }
            catch
            {
                return false;
            }
        }

        public SerialPort GetPortCOM(string portName)
        {
            try
            {
                var serialPort = new SerialPort
                {
                    PortName = portName,
                    BaudRate = 9600,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    DataBits = 8,
                    Handshake = Handshake.RequestToSend,
                    DtrEnable = true,
                    RtsEnable = true,
                    NewLine = Environment.NewLine,
                };

                serialPort.Open();
                if (isPortWork(serialPort))
                    return serialPort;
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }

}
