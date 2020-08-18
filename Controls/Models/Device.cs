using CQC.Controls.Converters;
using CQC.Controls.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQC.ConTest
{
    public class TestDevices
    {
        private List<DPDevice> dPDevices;
        private List<HartDevice> hartDevices;

        public object devList
        {
            get
            {
                if (projectType == ProjectType.DP)
                {
                    return dPDevices;
                }
                else if (projectType == ProjectType.Hart)
                {
                    return hartDevices;
                }
                else
                {
                    return null;
                }
            }
        }

        public void removeDevice(string devname)
        {
            if (projectType == ProjectType.DP)
            {
                dPDevices.Remove(this[devname] as DPDevice);
            }
            else if (projectType == ProjectType.Hart)
            {
                hartDevices.Remove(this[devname] as HartDevice);
            }
        }

        ProjectType projectType;

        public void Add(DPDevice dev)
        {
            dPDevices.Add(dev);
        }

        public void Add(HartDevice dev)
        {
            hartDevices.Add(dev);
        }

        public TestDevices()
        {
            dPDevices = new List<DPDevice>();
            hartDevices = new List<HartDevice>();
        }

        /*
        public object GetList()
        {
            if (projectType == ProjectType.DP)
            {
                devList = dPDevices;
            }
            else if (projectType == ProjectType.Hart)
            {
                devList = hartDevices;
            }
            else
            {
                devList = null;
            }
            return devList;
        }
        */

        public TestDevices(TestProject testProject)
        {
            projectType = testProject.TypeofProject;
            if (projectType == ProjectType.DP)
            {
                dPDevices = new List<DPDevice>();
            }
            else if (projectType == ProjectType.Hart)
            {
                hartDevices = new List<HartDevice>();
            }
        }

        public bool setnewName(string originame, string newname)
        {
            if (projectType == ProjectType.DP)
            {
                foreach (DPDevice dpdev in dPDevices)
                {
                    if (originame == dpdev.devName)
                    {
                        dpdev.devName = newname;
                        return true;
                    }
                }
            }
            else if (projectType == ProjectType.Hart)
            {
                foreach (HartDevice hartdev in hartDevices)
                {
                    if (originame == hartdev.devName)
                    {
                        hartdev.devName = newname;
                        return true;
                    }
                }
            }
            return false;
        }

        public object this[string devicename]
        {
            get
            {
                if (projectType == ProjectType.DP)
                {
                    foreach (DPDevice dpdev in dPDevices)
                    {
                        if (devicename == dpdev.devName)
                        {
                            return dpdev;
                        }
                    }
                }
                else if (projectType == ProjectType.Hart)
                {
                    foreach (HartDevice hartdev in hartDevices)
                    {
                        if (devicename == hartdev.devName)
                        {
                            return hartdev;
                        }
                    }
                }
                return null;
            }
            set
            {
                if (projectType == ProjectType.DP)
                {
                    if (!dPDevices.Contains((DPDevice)value))
                    {
                        dPDevices.Add((DPDevice)value);
                    }
                }
                if (projectType == ProjectType.Hart)
                {
                    if (!hartDevices.Contains((HartDevice)value))
                    {
                        hartDevices.Add((HartDevice)value);
                    }
                }
            }
        }

    }

    [TypeConverter(typeof(DeviceConverter))]
    public class DPDevice : Device
    {
        #region 字段

        private readonly NodeType m_NodeType = NodeType.devices;               // 节点类型
        private readonly TestDevices m_ParentTestDevices = null;                      // 测试设备

        #endregion

        #region 属性

        /// <summary>
        /// 获取节点类型
        /// </summary>
        [Browsable(false)]
        public NodeType NodeType => m_NodeType;

        [CategoryAttribute("General"), DescriptionAttribute("DP Device slave address"), ReadOnlyAttribute(false)]
        public int SlaveAddr
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device Ident Number"), ReadOnlyAttribute(false)]
        public string IdentNumber
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device Min Tsdr"), ReadOnlyAttribute(false)]
        public byte MinTsdr
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device input data length"), ReadOnlyAttribute(false)]
        public byte InputLen
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device output data length"), ReadOnlyAttribute(false)]
        public byte OutputLen
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device config data length"), ReadOnlyAttribute(false)]
        public byte CfgDataLen
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device config data"), ReadOnlyAttribute(false)]
        public string CfgData
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device user parameter data length"), ReadOnlyAttribute(false)]
        public byte UserExtPrmDataLen
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device user parameter data"), ReadOnlyAttribute(false)]
        public string UserPrmData
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device Name(example: L_MAG)"), ReadOnlyAttribute(false)]
        public string Model
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device revision number"), ReadOnlyAttribute(false)]
        public string DeviceRevision
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device DD revision number"), ReadOnlyAttribute(false)]
        public string DDRevision
        {
            get;
            set;
        }


        [CategoryAttribute("General"), DescriptionAttribute("DP Device HART Protocol Revision"), ReadOnlyAttribute(false)]
        public string ProtocolRevision
        {
            get;
            set;
        }


        [CategoryAttribute("General"), DescriptionAttribute("DP Device Software Revision"), ReadOnlyAttribute(false)]
        public string SoftwareRevision
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("DP Device detailed information"), ReadOnlyAttribute(false)]
        public string Description
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("Type of Device (example: pressure transmitter)"), ReadOnlyAttribute(false)]
        public string TypeofDevice
        {
            get;
            set;
        }

        [CategoryAttribute("Advanced"), DescriptionAttribute("Operate DD only without online devices."), ReadOnlyAttribute(false)]
        public bool Offline
        {
            get;
            set;
        }

        [CategoryAttribute("Advanced"), DescriptionAttribute("GSD file name."), ReadOnlyAttribute(false)]
        public string gsdFile
        {
            get;
            set;
        }

        [Browsable(false)]
        public TestDevices ParentTestDevices => m_ParentTestDevices;

        #endregion

        #region 构造函数

        public DPDevice(TestDevices devices)
        {
            devName = "DP Device";
            m_ParentTestDevices = devices;
            m_ParentTestDevices.Add(this);
        }

        #endregion

        #region 接口

        public override void Save()
        {
        }

        public override void Disposed()
        {
            m_ParentTestDevices.removeDevice(this.devName);
        }

        #endregion
    }

    [TypeConverter(typeof(DeviceConverter))]
    public class HartDevice : Device
    {
        #region 字段

        private readonly NodeType m_NodeType = NodeType.devices;                 // 节点类型
        private readonly TestDevices m_ParentTestDevices = null;                    // 所在测试设备列表

        #endregion

        #region 属性

        /// <summary>
        /// 获取节点类型
        /// </summary>
        [Browsable(false)]
        public NodeType NodeType => m_NodeType;

        [CategoryAttribute("General"), DescriptionAttribute("device slave address"), ReadOnlyAttribute(false)]
        public int SlaveAddr
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("Hart device TAG"), ReadOnlyAttribute(false)]
        public string DeviceTag
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("Device Manufacture ID"), ReadOnlyAttribute(false)]
        public string manufID
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("Hart Device type"), ReadOnlyAttribute(false)]
        public string DeviceType
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("Hart Device Name(example: L_MAG)"), ReadOnlyAttribute(false)]
        public string Model
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("Hart Device revision number"), ReadOnlyAttribute(false)]
        public string DeviceRevision
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("Hart Device DD revision number"), ReadOnlyAttribute(false)]
        public string DDRevision
        {
            get;
            set;
        }


        [CategoryAttribute("General"), DescriptionAttribute("Hart Device HART Protocol Revision"), ReadOnlyAttribute(false)]
        public string ProtocolRevision
        {
            get;
            set;
        }


        [CategoryAttribute("General"), DescriptionAttribute("Hart Device Software Revision"), ReadOnlyAttribute(false)]
        public string SoftwareRevision
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("Hart Device detailed information"), ReadOnlyAttribute(false)]
        public string Description
        {
            get;
            set;
        }

        [CategoryAttribute("General"), DescriptionAttribute("Type of Device (example: pressure transmitter)"), ReadOnlyAttribute(false)]
        public string TypeofDevice
        {
            get;
            set;
        }

        [CategoryAttribute("Advanced"), DescriptionAttribute("Device Description file root path"), ReadOnlyAttribute(false)]
        public string DDPathRoot
        {
            get;
            set;
        }

        [CategoryAttribute("Advanced"), DescriptionAttribute("Operate DD only without online devices."), ReadOnlyAttribute(false)]
        public bool Offline
        {
            get;
            set;
        }

        [Browsable(false)]
        public TestDevices ParentTestDevices => m_ParentTestDevices;

        #endregion

        #region 构造函数

        public HartDevice(TestDevices devices)
        {
            devName = "Hart Device";
            this.DDPathRoot = Path.Combine(System.Environment.CurrentDirectory, "TestDD", "HART");
            m_ParentTestDevices = devices;
            m_ParentTestDevices.Add(this);
        }

        #endregion

        #region 接口

        public override void Save()
        {
        }

        public override void Disposed()
        {
            m_ParentTestDevices.removeDevice(this.devName);
        }

        #endregion
    }

    public abstract class Device : IBaseModel
    {

        #region 属性

        [CategoryAttribute("General"), DescriptionAttribute("Hart Device Name"), ReadOnlyAttribute(false)]
        public string devName
        {
            get;
            set;
        }

        [Browsable(false)]
        public bool IsNewTestModel { get; set; }

        [Browsable(false)]
        public bool IsDeleted { get; set; }

        #endregion

        #region 接口

        public virtual void Save()
        {
        }

        public virtual void Disposed()
        {
        }

        #endregion
    }
}
