using CQC.ConTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQC.Controls.Models
{
    public class TestClassModel : List<TestCaseModel>, IBaseModel
    {
        #region 字段

        private string m_Name = ControlResources.TestClassName;         // 测试名称
        private string m_NewName = string.Empty;                        // 重命名
        private readonly NodeType m_NodeType = NodeType.testclass;      // 节点类型
        private ProjectType m_ProjectType = ProjectType.Blank;            // 工程类型
        private TestProject m_TestProject = null;                       // 测试工程对象
        private bool m_IsNewTestModel = true;                          // true新建模型，false已存在模型

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置测试名称
        /// </summary>
        public string Name { get => m_Name; set => m_Name = value; }

        /// <summary>
        /// 获取或设置测试文件路径
        /// </summary>
        public string FoldPath
        {
            get
            {
                return Path.Combine(m_TestProject.projectroot, m_TestProject.ProjectName, "TestClasses");
            }
        }

        /// <summary>
        /// 重命名
        /// </summary>
        public string NewName { get => m_NewName; set => m_NewName = value; }

        /// <summary>
        /// 获取节点类型
        /// </summary>
        public NodeType NodeType => m_NodeType;

        /// <summary>
        /// 获取或设置工程类型
        /// </summary>
        public ProjectType ProjectType { get => m_ProjectType; set => m_ProjectType = value; }

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

        public bool IsNewTestModel { get => m_IsNewTestModel; set => m_IsNewTestModel = value; }

        public bool IsDeleted { get; set; }

        #endregion

        #region 构造函数

        public TestClassModel(TestProject testProject)
        {
            m_TestProject = testProject;
            m_TestProject.ClassModelList.Add(this);
            IsDeleted = false;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 文件保存
        /// </summary>
        public void Save()
        {
            string clsPath = Path.Combine(FoldPath, this.Name);
            string clsNewPath = Path.Combine(FoldPath, this.NewName);

            if (true == this.IsDeleted)
            {
                if (false == this.IsNewTestModel)
                {
                    if (true == Directory.Exists(clsPath))
                    {
                        Directory.Delete(clsPath, true);
                    }
                }

                return;
            }

            if (true == this.IsNewTestModel)
            {
                if (true == IsRename)
                {
                    Directory.CreateDirectory(clsNewPath);
                }
                else
                {
                    Directory.CreateDirectory(clsPath);
                }
            }
            else
            {
                if (true == IsRename)
                {
                    Directory.Move(clsPath, clsNewPath);
                }
            }

            foreach (var item in this)
            {
                item.Save();
            }
        }

        public void Disposed()
        {
            this.IsDeleted = true;
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

        #endregion
    }
}
