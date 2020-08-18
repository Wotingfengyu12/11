using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace CQC.ConTest
{
    public partial class NewProject : DevExpress.XtraEditors.XtraForm
    {
        TestProject proinfo;

        public NewProject()
        {
            InitializeComponent();
        }

        public NewProject(TestProject tp)
        {
            InitializeComponent();
            proinfo = tp;
            radioGroup1.SelectedIndex = 0;
        }

        private void NewProject_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.TrimEnd(frmMain.spaces) == "")
            {
                this.DialogResult = DialogResult.Abort;
                proinfo.ProjectName = "";
                return;
            }
            else if (File.Exists(proinfo.projectroot + "\\" + textBox1.Text + ".cts"))
            {
                this.DialogResult = DialogResult.Abort;
                proinfo.ProjectName = null;
                return;
            }

            if (radioGroup1.SelectedIndex == 0)
            {
                proinfo.TypeofProject = ProjectType.Hart;
            }
            else
            {
                proinfo.TypeofProject = ProjectType.DP;
            }
            
            proinfo.ProjectName = textBox1.Text;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
        }
    }
}