using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProcessAutomation.DAL;
using ProcessAutomation.Main.PayIn;
using ProcessAutomation.Main.Services;
using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;

namespace ProcessAutomation.Main
{
    public partial class Main : Form
    {
        DevicePortCOMService serialPortService = new DevicePortCOMService();
        SerialPort serialPort = new SerialPort();
        MessageService messageService = new MessageService();
        IAutomationPayIn iAutomationPayin;
        bool isCurrentPayInProcessDone = true;
        Dictionary<string, List<Message>> listMessage = new Dictionary<string, List<Message>>();
        System.Timers.Timer timerReadMessage1;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            AddPortsToCombobox();

            timerReadMessage1 = new System.Timers.Timer(10000);
            timerReadMessage1.AutoReset = true;
            timerReadMessage1.Elapsed += new ElapsedEventHandler(this.StartReadMessage);

            timerCheckPayInProcess = new System.Windows.Forms.Timer();
            timerCheckPayInProcess.Interval = (10000);
            timerCheckPayInProcess.Tick += new EventHandler(StartPayIn);

            timerCheckChildProcess = new System.Windows.Forms.Timer();
            timerCheckChildProcess.Interval = (5000);
            timerCheckChildProcess.Tick += new EventHandler(Process);

            lblErrorReadMessage.Hide();
            btnStopReadMessage.Hide();
            btnStopPayIn.Hide();
            lblReadMessageProgress.Hide();
            lblPayInProgress.Hide();
        }
        private void btnStartReadMessage_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("Chưa kết nối thiết bị");
                return;
            }

            lblErrorReadMessage.Hide();
            lblReadMessageProgress.Show();
            //proBarReadMessage.Style = ProgressBarStyle.Marquee;
            //proBarReadMessage.MarqueeAnimationSpeed = 1;
            btnStopReadMessage.Show();
            btnStartReadMessage.Hide();

            if (!timerReadMessage1.Enabled)
                timerReadMessage1.Start();
        }

        private void btnStopReadMessage_Click(object sender, EventArgs e)
        {
            //proBarReadMessage.MarqueeAnimationSpeed = 0;
            //proBarReadMessage.Style = ProgressBarStyle.Blocks;
            lblReadMessageProgress.Hide();
            timerReadMessage1.Stop();
            btnStopReadMessage.Hide();
            btnStartReadMessage.Show();
        }

        private void btnStartPayIn_Click(object sender, EventArgs e)
        {
            btnStopPayIn.Show();
            btnStartPayIn.Hide();
            lblPayInProgress.Show();
            //proBarPayIn.Style = ProgressBarStyle.Marquee;
            //proBarPayIn.MarqueeAnimationSpeed = 1;
            if (!timerCheckPayInProcess.Enabled)
                timerCheckPayInProcess.Start();
        }

        private void btnStopPayIn_Click(object sender, EventArgs e)
        {
            //proBarPayIn.MarqueeAnimationSpeed = 0;
            //proBarPayIn.Style = ProgressBarStyle.Blocks;
            lblPayInProgress.Hide();
            timerCheckPayInProcess.Stop();
            btnStopPayIn.Hide();
            btnStartPayIn.Show();
        }

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
            MessageBox.Show("Kết nối thiết bị thành công");
        }

        private void StartReadMessage(object sender, ElapsedEventArgs e)
        {
            try
            {
                messageService.StartReadMessage(serialPort);
            }
            catch (Exception ex)
            {
                btnStopReadMessage.Hide();
                btnStartReadMessage.Show();
                timerReadMessage1.Stop();
                lblErrorReadMessage.Text = "Có lỗi hệ thống: " + ex.Message
                    + Environment.NewLine + "Hãy kiểm tra và bắt đầu lại";
            }
        }

        private void StartPayIn(object sender, EventArgs e)
        {
            try
            {
                if (!isCurrentPayInProcessDone)
                    return;

                listMessage = new Dictionary<string, List<Message>>();
                listMessage = messageService.ReadMessage();
                if (listMessage.Count == 0)
                    isCurrentPayInProcessDone = true;
                else
                {
                    isCurrentPayInProcessDone = false;
                    if (!timerCheckChildProcess.Enabled)
                    {
                        timerCheckChildProcess.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                btnStopPayIn.Hide();
                btnStartPayIn.Show();
                MessageBox.Show(ex.Message);
            }
        }

        private void Process(object sender, EventArgs e)
        {
            try
            {
                if (listMessage.Count == 0)
                {
                    isCurrentPayInProcessDone = true;
                    timerCheckChildProcess.Stop();
                    return;
                }

                if (listMessage.ContainsKey("cb") && listMessage["cb"].Count > 0)
                {
                    if (iAutomationPayin == null || !(iAutomationPayin is CBSite))
                    {
                        iAutomationPayin = new CBSite(new List<Message>(listMessage["cb"]), webLayout);
                        iAutomationPayin.startPayIN();
                    }

                    if (!iAutomationPayin.checkProcessDone())
                        return;

                    listMessage.Remove("cb");
                    iAutomationPayin = null;
                }
                else if (listMessage.ContainsKey("hl") && listMessage["hl"].Count > 0)
                {
                    if (iAutomationPayin == null || !(iAutomationPayin is HLCSite))
                    {
                        iAutomationPayin = new HLCSite(new List<Message>(listMessage["hl"]), webLayout);
                        iAutomationPayin.startPayIN();
                    }

                    if (!iAutomationPayin.checkProcessDone())
                        return;

                    listMessage.Remove("hl");
                    iAutomationPayin = null;
                }
                else if (listMessage.ContainsKey("gd") && listMessage["gd"].Count > 0)
                {
                    //if (iAutomationPayin == null || !(iAutomationPayin is GDSite))
                    //{
                    //    iAutomationPayin = new GDSite(new List<Message>(listMessage["gd"]), webLayout);
                    //    iAutomationPayin.startPayIN();
                    //}

                    //if (!iAutomationPayin.checkProcessDone())
                    //    return;

                    listMessage.Remove("gd");
                }
                else if (listMessage["nt"] != null && listMessage["nt"].Count > 0)
                {
                    //if (iAutomationPayin == null || !(iAutomationPayin is CBSite))
                    //{
                    //    iAutomationPayin = new CBSite(listMessage["nt"], webLayout);
                    //    iAutomationPayin.startPayIN();
                    //}

                    //if (!iAutomationPayin.checkProcessDone())
                    //    return;

                    listMessage.Remove("nt");
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            else
            {
                MessageBox.Show("Hãy kết nối thiết bị");
            }
        }

        private void btnShowHistory_Click(object sender, EventArgs e)
        {
            var database = new MongoDatabase<Message>(typeof(Message).Name);
            List<Message> listMessge = database.Query.ToList();
            dataGridView1.Columns[4].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.Columns[4].Frozen = false;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView1.Columns[8].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //dataGridView1.Columns[8].Frozen = false;
            //dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12);
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.DataSource = listMessge.OrderByDescending(x => x.Id).Take(100).ToList();
        }

        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                if (e.Value != null && e.Value is bool)
                {
                    e.Value = (bool)e.Value ? "Hợp Lệ" : "Không";
                    e.FormattingApplied = true;
                }
            }

            if (e.ColumnIndex == 6)
            {
                if (e.Value != null && e.Value is bool)
                {
                    e.Value = (bool)e.Value ? "Rồi" : "Chưa";
                    e.FormattingApplied = true;
                }
            }

            if (e.ColumnIndex == 7)
            {
                if (e.Value != null && e.Value is BsonDateTime)
                {
                    e.Value = DateTime.Parse(e.Value.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                }
            }

            foreach (DataGridViewRow Myrow in dataGridView1.Rows)
            {  
                if ((Myrow.Cells[8].Value != null &&
                    !string.IsNullOrEmpty(Myrow.Cells[8].Value.ToString())) ||
                    (Myrow.Cells[5].Value != null &&
                     Myrow.Cells[5].Value is bool && !((bool)Myrow.Cells[5].Value)))
                {
                    Myrow.DefaultCellStyle.BackColor = Color.Bisque;
                }
                else
                {
                    Myrow.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
    }
}