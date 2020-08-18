using System.Windows.Forms;
using DevExpress.XtraEditors;
using CQC.Controls;

namespace CQC.ConTest
{
    public partial class ucOutput : XtraUserControl
    {
        public ucOutput()
        {
            InitializeComponent();
            //textBox.ContextMenuStrip = new ContextMenuStrip();
            //textBox.Text = ;
            //textBox.AppendText(" ------ Test Started by CQC ------\r\n\r\n Preparing Test Cases...\r\n Updating devices...\r\n Performing main compilation...\r\n\r\n Test complete -- \r\nGenerating reports...\r\n\r\n\r\n ---------------------- Done ----------------------\r\n\r\n     Test: 101 passed, 5 failed, 10 not run");

            textBox.ReadOnly = true;
        }

        public TextBox tBox
        {
            get
            {
                return textBox;
            }
        }

        public void Clear()
        {
            textBox.Clear();
        }

        public string text
        {
            get
            {
                return textBox.Text;
            }
            set
            {
                textBox.Text = value;
            }
        }

        public void ScrollToBottom()
        {
            Utils.ScrollToBottom(textBox);
        }

        void textBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }

        private void textBox_TextChanged(object sender, System.EventArgs e)
        {
            Utils.ScrollToBottom(textBox);
        }

        private void ucOutput_Enter(object sender, System.EventArgs e)
        {
        }

        private void biClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            textBox.Clear();
        }

        private void biWordWrap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            textBox.WordWrap = biWordWrap.Down;
        }
    }
}
