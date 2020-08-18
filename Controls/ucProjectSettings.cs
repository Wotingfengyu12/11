using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CQC.ConTest
{
    public partial class ucProjectSettings : XtraUserControl
    {
        /*
        [DefaultValue(true)]
        public bool ShowDescription { get; set; }
        [DefaultValue(true)]
        public bool ShowCategories { get; set; }
        */

        public TestProject projectCfg;

        public ucProjectSettings(TestProject tp)
        {
            InitializeComponent();
            //propertyGrid.PropertyGrid.AutoGenerateRows = true;
            AddControls(this, comboBox);
            comboBox.SelectedIndex = 0;
            projectCfg = tp;
        }

        public ucProjectSettings()
        {
            InitializeComponent();
            //propertyGrid.PropertyGrid.AutoGenerateRows = true;
            AddControls(this, comboBox);
            comboBox.SelectedIndex = 0;
            projectCfg = new TestProject();
        }

        public void UpdateSettings(TestProject tp)
        {
            projectCfg = tp;
            propertyGrid1.SelectedObject = projectCfg;
        }

        void comboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //propertyGrid.PropertyGrid.SelectedObject = projectCfg;
        }

        void AddControls(Control container, ComboBoxEdit cb)
        {
            foreach (object obj in container.Controls)
            {
                cb.Properties.Items.Add(obj);
                if (obj is Control)
                {
                    AddControls(obj as Control, cb);
                }
            }
        }

        private void ucProjectSettings_Load(object sender, System.EventArgs e)
        {
            propertyGrid1.SelectedObject = projectCfg;
        }

        public event EventHandler SettingChanged;
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (SettingChanged != null)
            {
                SettingChanged(s, EventArgs.Empty);
            }
        }

        public void RefreshPro()
        {
            propertyGrid1.Refresh();
        }
    }
}
