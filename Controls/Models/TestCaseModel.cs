using CQC.ConTest;
using DevExpress.DirectX.Common.Direct2D;
using DevExpress.XtraVerticalGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CQC.Controls.Models
{
    public class TestCaseModel : IBaseModel
    {
        #region 字段

        private string m_Name = ControlResources.TestCaseName;                  // 测试名称
        private string m_TextValue = string.Empty;                              // 测试内容
        private string m_NewName = string.Empty;                                // 重命名
        private readonly TestClassModel m_ParentNodedata = null;                // 父节点
        private readonly NodeType m_NodeType = NodeType.testcase;               // 节点类型
        private bool m_IsNewTestModel = true;                          // true新建模型，false已存在模型

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置测试名称
        /// </summary>
        public string Name
        {
            get => m_Name;
            set => m_Name = value;
        }

        /// <summary>
        /// 获取或设置测试文件路径
        /// </summary>
        public string FoldPath
        {
            get
            {
                string foldPath = string.Empty;
                if (true == ParentNodedata.IsRename)
                {
                    foldPath = Path.Combine(ParentNodedata.FoldPath, ParentNodedata.NewName + ".cas");
                }
                else
                {
                    foldPath = Path.Combine(ParentNodedata.FoldPath, ParentNodedata.Name, this.Name + ".cas");
                }

                return foldPath;
            }
        }

        /// <summary>
        /// 获取或设置测试内容
        /// </summary>
        public string TextValue
        {
            get => m_TextValue;
            set => m_TextValue = value;
        }

        public string NewName
        {
            get => m_NewName;
            set => m_NewName = value;
        }

        public bool IsChanged
        {
            get;
            set;
        }

        public bool IsRename
        {
            get
            {
                if (true == string.IsNullOrEmpty(this.m_NewName))
                {
                    return false;
                }
                else
                {
                    return !m_NewName.Equals(m_Name);
                }
            }
        }

        /// <summary>
        /// 获取节点类型
        /// </summary>
        public NodeType NodeType => m_NodeType;

        public TestClassModel ParentNodedata => m_ParentNodedata;

        public ucTextEditor TextCntrol { get; set; }

        public bool IsNewTestModel { get => m_IsNewTestModel; set => m_IsNewTestModel = value; }

        public bool IsDeleted { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parentModel"></param>
        public TestCaseModel(TestClassModel parentModel)
        {
            m_ParentNodedata = parentModel;
            m_TextValue = string.Format(ControlResources.TestCaseTemplate, this.m_Name, parentModel.Name);
            parentModel.Add(this);
            IsDeleted = false;
        }

        #endregion

        #region 方法

        public void Save()
        {
            string name = this.Name;
            string clsName = ParentNodedata.Name;

            if (true == ParentNodedata.IsRename)
            {
                clsName = ParentNodedata.NewName;
            }

            if (true == this.IsDeleted)
            {
                if (false == this.IsNewTestModel)
                {
                    File.Delete(Path.Combine(ParentNodedata.FoldPath, clsName, this.Name + ".cas"));
                }
                this.IsDeleted = false;
                return;
            }

            if (true == this.IsRename)
            {
                name = this.NewName;

                if (false == this.IsNewTestModel)
                {
                    File.Move(Path.Combine(ParentNodedata.FoldPath, clsName, this.Name + ".cas"),
                        Path.Combine(ParentNodedata.FoldPath, clsName, this.NewName + ".cas"));
                }
                Name = NewName;
            }

            string filePath = Path.Combine(ParentNodedata.FoldPath, clsName, name + ".cas");

            using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                StreamWriter streamWriter = new StreamWriter(stream);
                streamWriter.Write(this.TextValue);
                streamWriter.Close();
            }
        }

        public void Disposed()
        {
            IsDeleted = true;
        }

        #endregion

        #region 接口

        /// <summary>
        /// 获取最终名称
        /// </summary>
        /// <returns></returns>
        public string GetFinalName()
        {
            return true == IsRename ? this.m_NewName : this.m_Name;
        }

        /// <summary>
        /// 获取Log文件路径
        /// </summary>
        public string GetTestLogPath()
        {
            try
            {
                string name = GetFinalName();
                string clsName = ParentNodedata.Name;

                if (true == ParentNodedata.IsRename)
                {
                    clsName = ParentNodedata.NewName;
                }

                return Path.Combine(ParentNodedata.FoldPath, clsName, name + ".tlg");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
