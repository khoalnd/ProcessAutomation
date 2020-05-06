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
            this.lblPayIn = new System.Windows.Forms.Label();
            this.btnStopPayIn = new System.Windows.Forms.Button();
            this.btnStartPayIn = new System.Windows.Forms.Button();
            this.webLayout = new System.Windows.Forms.WebBrowser();
            this.tabReaMessage = new System.Windows.Forms.TabPage();
            this.proBarReadMessage = new System.Windows.Forms.ProgressBar();
            this.btnStopReadMessage = new System.Windows.Forms.Button();
            this.btnStartReadMessage = new System.Windows.Forms.Button();
            this.connectPortBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SerialPortCombobox = new System.Windows.Forms.ComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.lblErrorReadMessage = new System.Windows.Forms.Label();
            this.tabPayIn.SuspendLayout();
            this.tabReaMessage.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPayIn
            // 
            this.tabPayIn.Controls.Add(this.lblPayIn);
            this.tabPayIn.Controls.Add(this.btnStopPayIn);
            this.tabPayIn.Controls.Add(this.btnStartPayIn);
            this.tabPayIn.Controls.Add(this.webLayout);
            this.tabPayIn.Location = new System.Drawing.Point(4, 33);
            this.tabPayIn.Name = "tabPayIn";
            this.tabPayIn.Padding = new System.Windows.Forms.Padding(3);
            this.tabPayIn.Size = new System.Drawing.Size(936, 594);
            this.tabPayIn.TabIndex = 1;
            this.tabPayIn.Text = "Nạp Tiền";
            this.tabPayIn.UseVisualStyleBackColor = true;
            // 
            // lblPayIn
            // 
            this.lblPayIn.AutoSize = true;
            this.lblPayIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayIn.ForeColor = System.Drawing.Color.Red;
            this.lblPayIn.Location = new System.Drawing.Point(16, 72);
            this.lblPayIn.Name = "lblPayIn";
            this.lblPayIn.Size = new System.Drawing.Size(86, 31);
            this.lblPayIn.TabIndex = 12;
            this.lblPayIn.Text = "label2";
            // 
            // btnStopPayIn
            // 
            this.btnStopPayIn.Location = new System.Drawing.Point(217, 18);
            this.btnStopPayIn.Name = "btnStopPayIn";
            this.btnStopPayIn.Size = new System.Drawing.Size(216, 45);
            this.btnStopPayIn.TabIndex = 11;
            this.btnStopPayIn.Text = "Dừng nạp tiền";
            this.btnStopPayIn.UseVisualStyleBackColor = true;
            // 
            // btnStartPayIn
            // 
            this.btnStartPayIn.Location = new System.Drawing.Point(22, 18);
            this.btnStartPayIn.Name = "btnStartPayIn";
            this.btnStartPayIn.Size = new System.Drawing.Size(175, 45);
            this.btnStartPayIn.TabIndex = 10;
            this.btnStartPayIn.Text = "Bắt đầu nạp tiền";
            this.btnStartPayIn.UseVisualStyleBackColor = true;
            // 
            // webLayout
            // 
            this.webLayout.Location = new System.Drawing.Point(3, 114);
            this.webLayout.MinimumSize = new System.Drawing.Size(20, 20);
            this.webLayout.Name = "webLayout";
            this.webLayout.Size = new System.Drawing.Size(927, 480);
            this.webLayout.TabIndex = 9;
            // 
            // tabReaMessage
            // 
            this.tabReaMessage.Controls.Add(this.lblErrorReadMessage);
            this.tabReaMessage.Controls.Add(this.proBarReadMessage);
            this.tabReaMessage.Controls.Add(this.btnStopReadMessage);
            this.tabReaMessage.Controls.Add(this.btnStartReadMessage);
            this.tabReaMessage.Controls.Add(this.connectPortBtn);
            this.tabReaMessage.Controls.Add(this.label1);
            this.tabReaMessage.Controls.Add(this.SerialPortCombobox);
            this.tabReaMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabReaMessage.Location = new System.Drawing.Point(4, 33);
            this.tabReaMessage.Name = "tabReaMessage";
            this.tabReaMessage.Padding = new System.Windows.Forms.Padding(3);
            this.tabReaMessage.Size = new System.Drawing.Size(936, 594);
            this.tabReaMessage.TabIndex = 0;
            this.tabReaMessage.Text = "Đọc Tin Nhắn";
            this.tabReaMessage.UseVisualStyleBackColor = true;
            // 
            // proBarReadMessage
            // 
            this.proBarReadMessage.Location = new System.Drawing.Point(22, 164);
            this.proBarReadMessage.MarqueeAnimationSpeed = 1;
            this.proBarReadMessage.Maximum = 10000;
            this.proBarReadMessage.Name = "proBarReadMessage";
            this.proBarReadMessage.Size = new System.Drawing.Size(603, 34);
            this.proBarReadMessage.Step = 1;
            this.proBarReadMessage.TabIndex = 9;
            // 
            // btnStopReadMessage
            // 
            this.btnStopReadMessage.Location = new System.Drawing.Point(254, 96);
            this.btnStopReadMessage.Name = "btnStopReadMessage";
            this.btnStopReadMessage.Size = new System.Drawing.Size(197, 45);
            this.btnStopReadMessage.TabIndex = 8;
            this.btnStopReadMessage.Text = "Dừng đọc tin nhắn";
            this.btnStopReadMessage.UseVisualStyleBackColor = true;
            this.btnStopReadMessage.Click += new System.EventHandler(this.btnStopReadMessage_Click);
            // 
            // btnStartReadMessage
            // 
            this.btnStartReadMessage.Location = new System.Drawing.Point(22, 96);
            this.btnStartReadMessage.Name = "btnStartReadMessage";
            this.btnStartReadMessage.Size = new System.Drawing.Size(207, 45);
            this.btnStartReadMessage.TabIndex = 7;
            this.btnStartReadMessage.Text = "Bắt đầu đọc tin nhắn";
            this.btnStartReadMessage.UseVisualStyleBackColor = true;
            this.btnStartReadMessage.Click += new System.EventHandler(this.btnStartReadMessage_Click);
            // 
            // connectPortBtn
            // 
            this.connectPortBtn.Location = new System.Drawing.Point(501, 30);
            this.connectPortBtn.Name = "connectPortBtn";
            this.connectPortBtn.Size = new System.Drawing.Size(124, 33);
            this.connectPortBtn.TabIndex = 5;
            this.connectPortBtn.Text = "Kết Nối";
            this.connectPortBtn.UseVisualStyleBackColor = true;
            this.connectPortBtn.Click += new System.EventHandler(this.connectPortBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Cổng Kết Nối";
            // 
            // SerialPortCombobox
            // 
            this.SerialPortCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SerialPortCombobox.FormattingEnabled = true;
            this.SerialPortCombobox.Location = new System.Drawing.Point(175, 30);
            this.SerialPortCombobox.Name = "SerialPortCombobox";
            this.SerialPortCombobox.Size = new System.Drawing.Size(304, 32);
            this.SerialPortCombobox.TabIndex = 3;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabReaMessage);
            this.tabControl.Controls.Add(this.tabPayIn);
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(1, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(944, 631);
            this.tabControl.TabIndex = 10;
            // 
            // lblErrorReadMessage
            // 
            this.lblErrorReadMessage.AutoSize = true;
            this.lblErrorReadMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorReadMessage.ForeColor = System.Drawing.Color.Red;
            this.lblErrorReadMessage.Location = new System.Drawing.Point(16, 228);
            this.lblErrorReadMessage.Name = "lblErrorReadMessage";
            this.lblErrorReadMessage.Size = new System.Drawing.Size(86, 31);
            this.lblErrorReadMessage.TabIndex = 13;
            this.lblErrorReadMessage.Text = "label2";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 635);
            this.Controls.Add(this.tabControl);
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.tabPayIn.ResumeLayout(false);
            this.tabPayIn.PerformLayout();
            this.tabReaMessage.ResumeLayout(false);
            this.tabReaMessage.PerformLayout();
            this.tabControl.ResumeLayout(false);
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
        private System.Windows.Forms.ProgressBar proBarReadMessage;
        private System.Windows.Forms.Button btnStopReadMessage;
        private System.Windows.Forms.Button btnStartReadMessage;
        private System.Windows.Forms.Button connectPortBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SerialPortCombobox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Label lblPayIn;
        private System.Windows.Forms.Label lblErrorReadMessage;
    }
}