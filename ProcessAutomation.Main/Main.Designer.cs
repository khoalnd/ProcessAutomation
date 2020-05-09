namespace ProcessAutomation.Main
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerCheckPayInProcess = new System.Windows.Forms.Timer(this.components);
            this.timerCheckChildProcess = new System.Windows.Forms.Timer(this.components);
            this.timerReadMessage = new System.Windows.Forms.Timer(this.components);
            this.tabPayIn = new System.Windows.Forms.TabPage();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnStopPayIn = new System.Windows.Forms.Button();
            this.btnStartPayIn = new System.Windows.Forms.Button();
            this.webLayout = new System.Windows.Forms.WebBrowser();
            this.tabReaMessage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.proBarReadMessage = new System.Windows.Forms.ProgressBar();
            this.btnStopReadMessage = new System.Windows.Forms.Button();
            this.btnStartReadMessage = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.connectPortBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SerialPortCombobox = new System.Windows.Forms.ComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblErrorReadMessage = new System.Windows.Forms.Label();
            this.tabPayIn.SuspendLayout();
            this.tabReaMessage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPayIn
            // 
            this.tabPayIn.Controls.Add(this.progressBar1);
            this.tabPayIn.Controls.Add(this.btnStopPayIn);
            this.tabPayIn.Controls.Add(this.btnStartPayIn);
            this.tabPayIn.Controls.Add(this.webLayout);
            this.tabPayIn.Location = new System.Drawing.Point(4, 33);
            this.tabPayIn.Name = "tabPayIn";
            this.tabPayIn.Padding = new System.Windows.Forms.Padding(3);
            this.tabPayIn.Size = new System.Drawing.Size(1125, 755);
            this.tabPayIn.TabIndex = 1;
            this.tabPayIn.Text = "Nạp Tiền";
            this.tabPayIn.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(458, 25);
            this.progressBar1.MarqueeAnimationSpeed = 1;
            this.progressBar1.Maximum = 10000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(660, 34);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 12;
            // 
            // btnStopPayIn
            // 
            this.btnStopPayIn.Location = new System.Drawing.Point(217, 18);
            this.btnStopPayIn.Name = "btnStopPayIn";
            this.btnStopPayIn.Size = new System.Drawing.Size(216, 45);
            this.btnStopPayIn.TabIndex = 11;
            this.btnStopPayIn.Text = "Dừng nạp tiền";
            this.btnStopPayIn.UseVisualStyleBackColor = true;
            this.btnStopPayIn.Click += new System.EventHandler(this.btnStopPayIn_Click);
            // 
            // btnStartPayIn
            // 
            this.btnStartPayIn.Location = new System.Drawing.Point(22, 18);
            this.btnStartPayIn.Name = "btnStartPayIn";
            this.btnStartPayIn.Size = new System.Drawing.Size(175, 45);
            this.btnStartPayIn.TabIndex = 10;
            this.btnStartPayIn.Text = "Bắt đầu nạp tiền";
            this.btnStartPayIn.UseVisualStyleBackColor = true;
            this.btnStartPayIn.Click += new System.EventHandler(this.btnStartPayIn_Click);
            // 
            // webLayout
            // 
            this.webLayout.Location = new System.Drawing.Point(3, 89);
            this.webLayout.MinimumSize = new System.Drawing.Size(20, 20);
            this.webLayout.Name = "webLayout";
            this.webLayout.Size = new System.Drawing.Size(1119, 666);
            this.webLayout.TabIndex = 9;
            // 
            // tabReaMessage
            // 
            this.tabReaMessage.Controls.Add(this.groupBox2);
            this.tabReaMessage.Controls.Add(this.groupBox1);
            this.tabReaMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabReaMessage.Location = new System.Drawing.Point(4, 33);
            this.tabReaMessage.Name = "tabReaMessage";
            this.tabReaMessage.Padding = new System.Windows.Forms.Padding(3);
            this.tabReaMessage.Size = new System.Drawing.Size(1125, 755);
            this.tabReaMessage.TabIndex = 0;
            this.tabReaMessage.Text = "Đọc Tin Nhắn";
            this.tabReaMessage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Controls.Add(this.lblErrorReadMessage);
            this.groupBox2.Controls.Add(this.proBarReadMessage);
            this.groupBox2.Controls.Add(this.btnStopReadMessage);
            this.groupBox2.Controls.Add(this.btnStartReadMessage);
            this.groupBox2.Location = new System.Drawing.Point(7, 96);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1115, 659);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Đọc Tin Nhắn";
            // 
            // proBarReadMessage
            // 
            this.proBarReadMessage.Location = new System.Drawing.Point(454, 37);
            this.proBarReadMessage.MarqueeAnimationSpeed = 1;
            this.proBarReadMessage.Maximum = 10000;
            this.proBarReadMessage.Name = "proBarReadMessage";
            this.proBarReadMessage.Size = new System.Drawing.Size(638, 27);
            this.proBarReadMessage.Step = 1;
            this.proBarReadMessage.TabIndex = 16;
            // 
            // btnStopReadMessage
            // 
            this.btnStopReadMessage.Location = new System.Drawing.Point(238, 34);
            this.btnStopReadMessage.Name = "btnStopReadMessage";
            this.btnStopReadMessage.Size = new System.Drawing.Size(197, 38);
            this.btnStopReadMessage.TabIndex = 15;
            this.btnStopReadMessage.Text = "Dừng đọc tin nhắn";
            this.btnStopReadMessage.UseVisualStyleBackColor = true;
            this.btnStopReadMessage.Click += new System.EventHandler(this.btnStopReadMessage_Click);
            // 
            // btnStartReadMessage
            // 
            this.btnStartReadMessage.Location = new System.Drawing.Point(6, 34);
            this.btnStartReadMessage.Name = "btnStartReadMessage";
            this.btnStartReadMessage.Size = new System.Drawing.Size(207, 38);
            this.btnStartReadMessage.TabIndex = 14;
            this.btnStartReadMessage.Text = "Bắt đầu đọc tin nhắn";
            this.btnStartReadMessage.UseVisualStyleBackColor = true;
            this.btnStartReadMessage.Click += new System.EventHandler(this.btnStartReadMessage_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.connectPortBtn);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.SerialPortCombobox);
            this.groupBox1.Location = new System.Drawing.Point(7, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1115, 84);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kết Nối Thiết Bị";
            // 
            // connectPortBtn
            // 
            this.connectPortBtn.Location = new System.Drawing.Point(483, 33);
            this.connectPortBtn.Name = "connectPortBtn";
            this.connectPortBtn.Size = new System.Drawing.Size(124, 33);
            this.connectPortBtn.TabIndex = 8;
            this.connectPortBtn.Text = "Kết Nối";
            this.connectPortBtn.UseVisualStyleBackColor = true;
            this.connectPortBtn.Click += new System.EventHandler(this.connectPortBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "Cổng Kết Nối";
            // 
            // SerialPortCombobox
            // 
            this.SerialPortCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SerialPortCombobox.FormattingEnabled = true;
            this.SerialPortCombobox.Location = new System.Drawing.Point(154, 33);
            this.SerialPortCombobox.Name = "SerialPortCombobox";
            this.SerialPortCombobox.Size = new System.Drawing.Size(304, 32);
            this.SerialPortCombobox.TabIndex = 6;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabReaMessage);
            this.tabControl.Controls.Add(this.tabPayIn);
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(1, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1133, 792);
            this.tabControl.TabIndex = 10;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 169);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1115, 481);
            this.dataGridView1.TabIndex = 18;
            // 
            // lblErrorReadMessage
            // 
            this.lblErrorReadMessage.AutoSize = true;
            this.lblErrorReadMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorReadMessage.ForeColor = System.Drawing.Color.Red;
            this.lblErrorReadMessage.Location = new System.Drawing.Point(8, 87);
            this.lblErrorReadMessage.Name = "lblErrorReadMessage";
            this.lblErrorReadMessage.Size = new System.Drawing.Size(86, 31);
            this.lblErrorReadMessage.TabIndex = 17;
            this.lblErrorReadMessage.Text = "label2";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1135, 793);
            this.Controls.Add(this.tabControl);
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.tabPayIn.ResumeLayout(false);
            this.tabReaMessage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerCheckPayInProcess;
        private System.Windows.Forms.Timer timerCheckChildProcess;
        private System.Windows.Forms.Timer timerReadMessage;
        private System.Windows.Forms.TabPage tabPayIn;
        private System.Windows.Forms.Button btnStopPayIn;
        private System.Windows.Forms.Button btnStartPayIn;
        private System.Windows.Forms.WebBrowser webLayout;
        private System.Windows.Forms.TabPage tabReaMessage;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button connectPortBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SerialPortCombobox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar proBarReadMessage;
        private System.Windows.Forms.Button btnStopReadMessage;
        private System.Windows.Forms.Button btnStartReadMessage;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblErrorReadMessage;
    }
}