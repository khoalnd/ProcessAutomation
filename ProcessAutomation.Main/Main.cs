using ProcessAutomation.Main.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProcessAutomation.Main
{
    public partial class Main : Form
    {
        System.Timers.Timer readMesageTimer;
        System.Timers.Timer payinProcessTimer;
        DevicePortCOMService serialPortService = new DevicePortCOMService();
        SerialPort serialPort = new SerialPort();
        MessageService messageService = new MessageService();

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

            messageService.StartReadMessage(serialPort);
            InitReadMessageTimer();
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
                if (needStop)
                    readMesageTimer.Start();
            }
        }

        private void InitPayInProcessTimer()
        {
            readMesageTimer = new System.Timers.Timer(10000);
            payinProcessTimer.AutoReset = false;
            payinProcessTimer.Elapsed += StartPayIn;
            readMesageTimer.Start();
        }

        private void StartPayIn(object sender, ElapsedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
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