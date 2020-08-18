using CQC.Controls.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQC.ConTest
{
    public class TestProject
    {
        #region 字段

        public bool saved;
        //public string projectname;
        private ProjectType type;
        private string name;
        private int iCaseNum;
        private string filename;
        //private string fullpath;
        private bool bsave;
        private bool bGenReport;
        private DateTime _modified;

        private TestDevices m_Devices = null;       // 测试设备

        #endregion

        #region 属性

        [Browsable(false)]
        public readonly List<TestClassModel> ClassModelList = new List<TestClassModel>();

        [Browsable(false)]
        public DateTime timemodified
        {
            get
            {
                return _modified;
            }
            set

            {
                _modified = value;
            }
        }

        [CategoryAttribute("General"), DescriptionAttribute("Name of the test project"), ReadOnlyAttribute(true)]
        public string ProjectName
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.Modified = true;
                }
            }
        }

        [CategoryAttribute("General"), DescriptionAttribute("Number of Test Cases in the project"), ReadOnlyAttribute(true)]
        public int NumOfCases
        {
            get
            {
                return this.iCaseNum;
            }
            set
            {
                if (this.iCaseNum != value)
                {
                    this.iCaseNum = value;
                    this.Modified = true;
                }
            }
        }

        [CategoryAttribute("General"), DescriptionAttribute("The type of this test project"), ReadOnlyAttribute(true)]
        public ProjectType TypeofProject
        {
            get
            {
                return this.type;
            }
            set
            {
                if (this.type != value)
                {
                    this.type = value;
                    this.Modified = true;
                }
            }
        }

        [CategoryAttribute("General"), DescriptionAttribute("The path of this test project"), ReadOnlyAttribute(true)]
        public string FileName
        {
            get
            {
                return this.filename;
            }
            set
            {
                if (this.filename != value)
                {
                    this.filename = value;
                    this.Modified = true;
                }
            }
        }

        [CategoryAttribute("General"), DescriptionAttribute("The path of this test project"), ReadOnlyAttribute(true)]
        public string FullPath
        {
            get
            {
                //return this.fullpath;
                return this.projectroot + filename;
            }
            /*
            set
            {
                if (this.fullpath != value)
                {
                    this.fullpath = value;
                    this.Modified = true;
                }
            }
            */
        }

        [CategoryAttribute("General"), DescriptionAttribute("The root directory of the test cases"), ReadOnlyAttribute(true)]
        public string TestCaseDir
        {
            get
            {
                if (TypeofProject == ProjectType.DP)
                {
                    return projectroot + name + "\\DPTest";
                }
                else if (TypeofProject == ProjectType.Hart)
                {
                    return projectroot + name + "\\HartTest";
                }
                return null;
            }
        }

        [CategoryAttribute("General"), DescriptionAttribute("The root directory of the project"), ReadOnlyAttribute(true)]
        public string projectroot
        {
            get
            {
                /*if (fullpath != null)
                {
                    int ld = fullpath.LastIndexOf('\\');
                    string dir = fullpath.Substring(0, ld);
                    return dir;
                }
                else*/
                {
                    return System.Environment.CurrentDirectory + "\\Project\\";
                }
            }
        }

        [CategoryAttribute("General"), DescriptionAttribute("The detailed description of this test project")]
        public string Description
        {
            get;
            set;
        }

        #region Test Organization

        [CategoryAttribute("Operator"), DescriptionAttribute("Organization of this test project")]
        public string Organization
        {
            get;
            set;
        }

        [CategoryAttribute("Operator"), DescriptionAttribute("Organization Address")]
        public string Address { get; set; }

        #endregion

        #region Tester

        [CategoryAttribute("Operator"), DescriptionAttribute("Tester of this test project")]
        public string Tester
        {
            get;
            set;
        }

        [CategoryAttribute("Operator"),DisplayName("Tester Email"), DescriptionAttribute("Tester Email")]
        public string Email { get; set; }
        
        [CategoryAttribute("Operator"),DisplayName("Tester Phone"), DescriptionAttribute("Tester Phone")]
        public string Phone { get; set; }

        #endregion

        #region Operator

        [CategoryAttribute("Operator"), DisplayName("Begin Testing Date"), DescriptionAttribute("Begin Testing Date")]
        public string BeginTestingDate { get; set; }

        [CategoryAttribute("Operator"), DisplayName("Executive Date"), DescriptionAttribute("Executive Date")]
        public string ExecutiveDate { get; set; }

        [CategoryAttribute("Operator"), DisplayName("Document File"), DescriptionAttribute("The product specification including device specific details as per Field Device Specification Guide (HCF_LIT-18) ")]
        public string DocName { get; set; }

        #endregion

        [CategoryAttribute("Save"), DescriptionAttribute("The time that modified.")]
        public string LastModified
        {
            get
            {
                if (_modified != null && _modified.Ticks != 0)
                {
                    return _modified.ToString();
                }
                else
                {
                    return "Never saved yet";
                }
            }
            /*
            set
            {
                if (this._modified != value)
                {
                    this._modified = value;
                    this.Modified = true;
                }
            }
            */
        }

        [CategoryAttribute("Save"), DescriptionAttribute("Save the project automatically.")]
        public bool AutoSave
        {
            get
            {
                return bsave;
            }

            set
            {
                if (this.bsave != value)
                {
                    this.bsave = value;
                    this.Modified = true;
                }
            }
        }

        [CategoryAttribute("Save"), DescriptionAttribute("Generate test report or not.")]
        public bool TestReport
        {
            get
            {
                return bGenReport;
            }

            set
            {
                if (this.bGenReport != value)
                {
                    this.bGenReport = value;
                    this.Modified = true;
                }
            }
        }

        [Browsable(false)]
        public bool Modified
        {
            get;
            set;
        }
        public TestDevices Devices { get => m_Devices; set => m_Devices = value; }

        [Browsable(false)]
        public bool IsNewProject { get; set; }
        #endregion

        #region 构造函数

        public TestProject()
        {
            this.Modified = true;
            IsNewProject = true;
            //_modified = new DateTime();
        }

        #endregion
    }

    public enum ProjectType
    {
        Blank,
        Hart,
        DP
    }
}
