using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CQC.ConTest
{
    public partial class ucProperties : XtraUserControl
    {
        public ucProperties()
        {
            InitializeComponent();
            propertyGrid.PropertyGrid.AutoGenerateRows = true;
        }
    }
}
