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
            this.SerialPortCombobox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.connectPortBtn = new System.Windows.Forms.Button();
            this.webLayout = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // SerialPortCombobox
            // 
            this.SerialPortCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SerialPortCombobox.FormattingEnabled = true;
            this.SerialPortCombobox.Location = new System.Drawing.Point(191, 41);
            this.SerialPortCombobox.Name = "SerialPortCombobox";
            this.SerialPortCombobox.Size = new System.Drawing.Size(304, 21);
            this.SerialPortCombobox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cổng Kết Nối";
            // 
            // connectPortBtn
            // 
            this.connectPortBtn.Location = new System.Drawing.Point(526, 41);
            this.connectPortBtn.Name = "connectPortBtn";
            this.connectPortBtn.Size = new System.Drawing.Size(104, 23);
            this.connectPortBtn.TabIndex = 2;
            this.connectPortBtn.Text = "Kết Nối";
            this.connectPortBtn.UseVisualStyleBackColor = true;
            this.connectPortBtn.Click += new System.EventHandler(this.connectPortBtn_Click);
            // 
            // webLayout
            // 
            this.webLayout.Location = new System.Drawing.Point(38, 108);
            this.webLayout.MinimumSize = new System.Drawing.Size(20, 20);
            this.webLayout.Name = "webLayout";
            this.webLayout.Size = new System.Drawing.Size(592, 414);
            this.webLayout.TabIndex = 3;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 534);
            this.Controls.Add(this.webLayout);
            this.Controls.Add(this.connectPortBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SerialPortCombobox);
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SerialPortCombobox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button connectPortBtn;
        private System.Windows.Forms.WebBrowser webLayout;
    }
}