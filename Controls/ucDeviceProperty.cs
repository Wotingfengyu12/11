using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CQC.ConTest
{
    public partial class ucDeviceProperty : XtraUserControl
    {
        public DPDevice dpDevice;
        public HartDevice hartDevice;
        public ProjectType devType;

        public ucDeviceProperty(DPDevice dev)
        {
            InitializeComponent();
            dpDevice = dev;
            propertyGrid1.SelectedObject = dpDevice;
            devType = ProjectType.DP;
        }

        public ucDeviceProperty(object dev)
        {
            InitializeComponent();

            if(dev.GetType() == typeof(HartDevice))
            {
                hartDevice = (HartDevice)dev;
                propertyGrid1.SelectedObject = hartDevice;
                devType = ProjectType.Hart;
            }
            else if (dev.GetType() == typeof(DPDevice))
            {
                dpDevice = (DPDevice)dev;
                propertyGrid1.SelectedObject = dpDevice;
                devType = ProjectType.DP;
            }
        }

        public string getDesc(object dev)
        {
            if (dev.GetType() == typeof(HartDevice))
            {
                return (dev as HartDevice).Description;
            }
            else if (dev.GetType() == typeof(DPDevice))
            {
                return (dev as DPDevice).Description;
            }
            return "";
        }

        public ucDeviceProperty(HartDevice dev)
        {
            InitializeComponent();
            hartDevice = dev;
            propertyGrid1.SelectedObject = hartDevice;
            devType = ProjectType.Hart;
        }

        public void UpdateDevice(DPDevice dev)
        {
            dpDevice = dev;
            devType = ProjectType.DP;
            propertyGrid1.SelectedObject = dpDevice;
            propertyGrid1.Refresh();
        }

        public void UpdateDevice(HartDevice dev)
        {
            hartDevice = dev;
            devType = ProjectType.Hart;
            propertyGrid1.SelectedObject = hartDevice;
            propertyGrid1.Refresh();
        }

        private void ucProjectSettings_Load(object sender, System.EventArgs e)
        {
            //propertyGrid1.SelectedObject = projectCfg;

        }

        public event EventHandler DevCfgChanged;
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (DevCfgChanged != null)
            {
                DevCfgChanged(s, EventArgs.Empty);
            }
        }

        public void refreshDev()
        {
            propertyGrid1.Refresh();
        }

    }
}
