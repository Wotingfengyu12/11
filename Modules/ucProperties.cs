using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CQC.ConTest {
    public partial class ucProperties : XtraUserControl {
        public ucProperties() {
            InitializeComponent();
            propertyGrid.PropertyGrid.AutoGenerateRows = true;
            AddControls(this, comboBox);
            comboBox.SelectedIndex = 0;
        }
        void comboBox_SelectedIndexChanged(object sender, System.EventArgs e) {
            propertyGrid.PropertyGrid.SelectedObject = comboBox.SelectedItem;
        }
        void AddControls(Control container, ComboBoxEdit cb) {
            foreach(object obj in container.Controls) {
                cb.Properties.Items.Add(obj);
                if(obj is Control) AddControls(obj as Control, cb);
            }
        }
    }
}
