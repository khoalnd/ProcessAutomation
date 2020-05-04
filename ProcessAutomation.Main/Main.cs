using ProcessAutomation.Main.PayIn;
using ProcessAutomation.Main.Services;
using System;
using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;

namespace ProcessAutomation.Main
{
    public partial class Main : Form
    {
        System.Timers.Timer readMesageTimer;
        DevicePortCOMService serialPortService = new DevicePortCOMService();
        SerialPort serialPort = new SerialPort();
        MessageService messageService = new MessageService();
        IAutomationPayIn iAutomationPayin;
        bool isPayInProcessDone = true;
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // add serial port com from computer
            AddPortsToCombobox();
        }

        #region Read Message
        private void connectPortBtn_Click(object sender, EventArgs e)
        {
            var portName = SerialPortCombobox.Text;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("Hãy chọn cổng kết nối");
                return;
            }

            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort = null;
            }

            serialPort = serialPortService.GetPortCOM(portName);
            if (serialPort == null)
            {
                MessageBox.Show("Lỗi thiết bị, hãy kiểm tra lại");
                return;
            }

            //messageService.StartReadMessage(serialPort);
            //InitReadMessageTimer();
            //InitPayInProcessTimer();
        }

        private void InitReadMessageTimer()
        {
            readMesageTimer = new System.Timers.Timer(5000);
            readMesageTimer.AutoReset = false;
            readMesageTimer.Elapsed += StartReadMessage;
            readMesageTimer.Start();
        }

        private void StartReadMessage(object sender, ElapsedEventArgs e)
        {
            var needStop = false;
            try
            {
                messageService.StartReadMessage(serialPort);
            }
            catch (Exception ex)
            {
                readMesageTimer.Enabled = false;
                needStop = true;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (!needStop)
                    readMesageTimer.Start();
            }
        }

        private void InitPayInProcessTimer()
        {
            timer1.Interval = (10000);
            timer1.Tick += new EventHandler(StartPayIn);
            timer1.Start();
        }

        private void StartPayIn(object sender, EventArgs e)
        {
            try
            {
                if (isPayInProcessDone)
                {
                    var dataForProcess = messageService.ReadMessage();
                    if (dataForProcess.Count == 0)
                        isPayInProcessDone = true;
                    else
                    {

                        isPayInProcessDone = false;
                        if (iAutomationPayin != null && !iAutomationPayin.checkProcessDone())
                        {
                            return;
                        }
                        iAutomationPayin = new CBSite(null, webLayout);
                        iAutomationPayin.startPayIN();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddPortsToCombobox()
        {
            var portNames = SerialPort.GetPortNames();
            if (portNames != null && portNames.Length > 0)
            {
                SerialPortCombobox.Items.AddRange(portNames);
                SerialPortCombobox.SelectedIndex = portNames.Length - 1;
            }
        }

        #endregion
    }
}