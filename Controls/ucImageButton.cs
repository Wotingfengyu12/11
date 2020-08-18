using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CQC.ConTest
{
    public partial class ucImageButton : XtraUserControl
    {
        public event EventHandler MouseDownClick;

        [CategoryAttribute("Element"), DescriptionAttribute("The button image")]
        //[Editor("DevExpress.Utils.Design.SvgImageEditor,DevExpress.Design.v19.2, Version=19.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a", typeof(UITypeEditor))]
        public DevExpress.Utils.Svg.SvgImage SvgImage
        {
            get
            {
                return svgImageBox1.SvgImage;
            }
            set
            {
                svgImageBox1.SvgImage = value;
            }
        }

        [CategoryAttribute("Element"), DescriptionAttribute("The button header")]
        public string Header
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        [CategoryAttribute("Element"), DescriptionAttribute("The button detail")]
        public string Detail
        {
            get
            {
                return label2.Text;
            }
            set
            {
                label2.Text = value;
            }
        }

        public ucImageButton()
        {
            InitializeComponent();
            //textBox.ContextMenuStrip = new ContextMenuStrip(); 
        }

        System.Drawing.Color orgColor;
        private void svgImageBox1_MouseEnter(object sender, System.EventArgs e)
        {
            orgColor = this.BackColor;
            this.BackColor = System.Drawing.Color.AliceBlue;
        }

        private void svgImageBox1_MouseLeave(object sender, System.EventArgs e)
        {
            this.BackColor = orgColor;
        }

        private void ucImageButton_Click(object sender, System.EventArgs e)
        {
            if (null != MouseDownClick)
            {
                MouseDownClick(sender, e);
            }
        }
    }
}
