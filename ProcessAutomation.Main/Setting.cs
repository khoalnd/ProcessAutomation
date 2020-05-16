using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessAutomation.Main
{
    public partial class Setting : Form
    {
        public List<string> webToRun { get; set; }
        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            if (webToRun.IndexOf(Constant.CAYBANG) != -1) cbCayBang.Checked = true;
            if (webToRun.IndexOf(Constant.HANHLANG) != -1) cbHanhLang.Checked = true;
            if (webToRun.IndexOf(Constant.GIADINHVN) != -1) cbGiaDinh.Checked = true;
            if (webToRun.IndexOf(Constant.NT30s) != -1) cb30s.Checked = true;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            webToRun = new List<string>();
            if (cbCayBang.Checked) webToRun.Add(Constant.CAYBANG);
            if (cbHanhLang.Checked) webToRun.Add(Constant.HANHLANG);
            if (cbGiaDinh.Checked) webToRun.Add(Constant.GIADINHVN);
            if (cb30s.Checked) webToRun.Add(Constant.NT30s);
        }

        private void GetSettingMinimumMoney()
        {

        }
    }
}
