using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CQC.Controls.Models;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Localization;
using DevExpress.XtraTreeList.Menu;
using DevExpress.XtraTreeList.Nodes;

namespace CQC.ConTest
{
    public partial class ucProjectExplorer : System.Windows.Forms.UserControl
    {
        public TreeList TreeView
        {
            get
            {
                return treeView;
            }
        }

        private bool bShowAllFiles;
        string testcaseDir;
        ProjectType pType;
        TestDevices devList;
        string projectname;
        private TreeListNode m_TestScheduleNode = null;         // 测试列表节点
        private TestProject m_TestProject = null;               // 测试工程

        public event Action<TestCaseModel> DisplayTextEvent;

        public ucProjectExplorer()
        {
            InitializeComponent();
            InitTreeView(treeView);
            treeView.CustomDrawNodeCell += treeView_CustomDrawNodeCell;
            treeView.AfterCollapse += treeView_AfterCollapse;
            treeView.AfterExpand += treeView_AfterExpand;
            //AddAllNodes(iShow.Down);
        }

        public static void InitTreeView(DevExpress.XtraTreeList.TreeList treeView)
        {
            //TreeListColumn column = treeView.Columns.Add();
            //column.Visible = true;
            treeView.OptionsView.ShowColumns = false;
            treeView.OptionsView.ShowIndicator = false;
            treeView.OptionsView.ShowVertLines = false;
            treeView.OptionsView.ShowHorzLines = false;
            //treeView.OptionsBehavior.Editable = false;
            treeView.OptionsSelection.EnableAppearanceFocusedCell = false;
        }

        void treeView_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            if (e.Node.Id == 0)
            {
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
        }

        void SetIndex(TreeListNode node, int index, bool expand)
        {
            int newIndex = expand ? index - 1 : index + 1;
            if (node.StateImageIndex == index)
            {
                node.StateImageIndex = newIndex;
            }
        }

        void treeView_AfterExpand(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            SetIndex(e.Node, 7, true);
            SetIndex(e.Node, 9, true);
        }

        void treeView_AfterCollapse(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            SetIndex(e.Node, 6, false);
            SetIndex(e.Node, 8, false);
        }

        public void Clear()
        {
            treeView.Nodes.Clear();
        }

        void AddAllNodesOld(bool showAll)
        {
            treeView.Nodes.Clear();
            treeView.AppendNode(new object[] { "Solution \'VisualStudioInspiredUIDemo\' (1 project)" }, -1, -1, -1, 3); //0
            treeView.AppendNode(new object[] { "VisualStudioInspiredUIDemo" }, -1, -1, -1, 4);//1
            treeView.AppendNode(new object[] { "Properties" }, 1, -1, -1, 2);//2
            treeView.AppendNode(new object[] { "References" }, 1, -1, -1, 5);//3
            treeView.AppendNode(new object[] { "System" }, 3, -1, -1, 5);
            treeView.AppendNode(new object[] { "System.Drawing" }, 3, -1, -1, 5);
            treeView.AppendNode(new object[] { "System.Windows.Forms" }, 3, -1, -1, 5);
            treeView.AppendNode(new object[] { "DevExpress.Utils" }, 3, -1, -1, 5);
            treeView.AppendNode(new object[] { "DevExpress.XtraBars" }, 3, -1, -1, 5);
            treeView.AppendNode(new object[] { "DevExpress.XtraEditors" }, 3, -1, -1, 5);
            if (showAll)
            {
                treeView.AppendNode(new object[] { "bin" }, 1, -1, -1, 9); //10
                treeView.AppendNode(new object[] { "Debug" }, 10, -1, -1, 9);
                treeView.AppendNode(new object[] { "Release" }, 10, -1, -1, 9);
                treeView.AppendNode(new object[] { "obj" }, 1, -1, -1, 9);//13
                treeView.AppendNode(new object[] { "Debug" }, 13, -1, -1, 9);
                treeView.AppendNode(new object[] { "Release" }, 13, -1, -1, 9);
            }
            treeView.AppendNode(new object[] { "Test Logs" }, 0, -1, -1, 7);//16/10
            treeView.AppendNode(new object[] { "Resources" }, 1, -1, -1, 7);//17
            treeView.AppendNode(new object[] { "AssemblyInfo.cs" }, 2, -1, -1, 10);
            treeView.AppendNode(new object[] { "Results" }, 0, -1, -1, 11);//19
            treeView.AppendNode(new object[] { "ucCodeEditor.cs" }, showAll ? 16 : 10, -1, -1, 12); //20
            treeView.AppendNode(new object[] { "ucProjectExplorer.cs" }, showAll ? 16 : 10, -1, -1, 12); //21
            treeView.AppendNode(new object[] { "ucToolbox.cs" }, showAll ? 16 : 10, -1, -1, 12); //22
            if (showAll)
            {
                treeView.AppendNode(new object[] { "frmMain.resx" }, 19, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucCodeEditor.resx" }, 20, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucProjectExplorer.resx" }, 21, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucToolbox.resx" }, 22, -1, -1, 13);
            }
            treeView.ExpandAll();
        }

        public void setProjectName(string pname)
        {
            TreeListNode node = treeView.Nodes[0];
            node.SetValue(0, pname);
        }

        #region 加载树

        public void LoadTreeView(TestProject tp, string dirPath)
        {
            m_TestProject = tp;
            if (tp.TypeofProject == ProjectType.DP)
            {
                AddDPNodes(tp.Devices, dirPath, tp.ProjectName);
            }
            else if (tp.TypeofProject == ProjectType.Hart)
            {
                AddHartNodes(tp.Devices, dirPath, tp.ProjectName);
            }
        }

        public void AddDeviceNode(string devname)
        {
            TreeListNode node = treeView.AppendNode(new object[] { devname }, 2, -1, -1, 12); //4
            NodeInfo ninfo = new NodeInfo();
            ninfo.ntype = NodeType.devices;
            ninfo.nodename = devname;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;
            treeView.FocusedNode = node;
        }

        public void AddHartNodes(TestDevices devs, string casedir, string proname)
        {
            testcaseDir = casedir;
            pType = ProjectType.Hart;
            projectname = proname;
            devList = devs;
            treeView.Nodes.Clear();

            TreeListNode node;
            NodeInfo ninfo = new NodeInfo();
            node = treeView.AppendNode(new object[] { proname }, -1, -1, -1, 3); //0
            ninfo.nodename = proname;
            ninfo.ntype = NodeType.root;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            m_TestScheduleNode = treeView.AppendNode(new object[] { "Test Schedule" }, 0, -1, -1, 4);//1
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testschedule;
            m_TestScheduleNode.StateImageIndex = (int)ninfo.ntype;
            m_TestScheduleNode.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Hart Device" }, 0, -1, -1, 12); //2
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.hartdevice;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Results" }, 0, -1, -1, 7);//3
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.result;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Report" }, 0, -1, -1, 7);//4
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.report;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;
            /*
            node = treeView.AppendNode(new object[] { "Test Logs" }, 0, -1, -1, 7);//5
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.logroot;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;
            */
            node = treeView.AppendNode(new object[] { "Settings" }, 0, -1, -1, 12); //6
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.settings;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            foreach (HartDevice hdev in (List<HartDevice>)devs.devList)
            {
                node = treeView.AppendNode(new object[] { hdev.devName }, 2, -1, -1, 12);
                ninfo = new NodeInfo();
                ninfo.ntype = NodeType.devices;
                ninfo.nodename = hdev.devName;
                ninfo.SourceData = hdev;
                node.StateImageIndex = (int)ninfo.ntype;
                node.Tag = (NodeInfo)ninfo;
            }

            AddTestCasesNode(casedir);

            treeView.ExpandAll();
        }

        public void AddDPNodes(TestDevices devs, string casedir, string proname)
        {
            treeView.Nodes.Clear();

            testcaseDir = casedir;
            pType = ProjectType.DP;
            projectname = proname;
            devList = devs;

            TreeListNode node;
            NodeInfo ninfo = new NodeInfo();
            node = treeView.AppendNode(new object[] { proname }, -1, -1, -1, 3); //0
            ninfo.ntype = NodeType.root;
            ninfo.nodename = proname;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            m_TestScheduleNode = treeView.AppendNode(new object[] { "Test Schedule" }, 0, -1, -1, 4);//1
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testschedule;
            m_TestScheduleNode.StateImageIndex = (int)ninfo.ntype;
            m_TestScheduleNode.Tag = (NodeInfo)ninfo;

            //test cases...

            node = treeView.AppendNode(new object[] { "DP Device" }, 0, -1, -1, 12); //22
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.dpdevice;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Report" }, 0, -1, -1, 7);//5
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.report;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            /*
            node = treeView.AppendNode(new object[] { "Test Logs" }, 0, -1, -1, 7);//16/10
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.logroot;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            //test logs
            */

            node = treeView.AppendNode(new object[] { "Results" }, 0, -1, -1, 7);//17
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.result;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Settings" }, 0, -1, -1, 12); //22
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.settings;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            foreach (DPDevice ddev in (List<DPDevice>)devs.devList)
            {
                node = treeView.AppendNode(new object[] { ddev.devName }, 2, -1, -1, 12); //5
                ninfo = new NodeInfo();
                ninfo.ntype = NodeType.devices;
                ninfo.nodename = ddev.devName;
                node.StateImageIndex = (int)ninfo.ntype;
                ninfo.SourceData = ddev;
                node.Tag = (NodeInfo)ninfo;
            }

            AddTestCasesNode(casedir);

            treeView.ExpandAll();
        }

        public void AddTestCasesNode(string srcPath)
        {
            try
            {
                string[] testclasses = Directory.GetFileSystemEntries(srcPath);

                foreach (string testclass in testclasses)
                {
                    if (Directory.Exists(testclass))//dir
                    {
                        TreeListNode node;
                        NodeInfo ninfo = new NodeInfo();
                        string tc = testclass.Substring(testclass.LastIndexOf('\\') + 1);
                        TestClassModel classModel = new TestClassModel(m_TestProject);
                        classModel.ProjectType = this.pType;
                        classModel.Name = tc;
                        classModel.IsNewTestModel = m_TestProject.IsNewProject;

                        node = treeView.AppendNode(new object[] { tc }, 1, -1, -1, 4);//1
                        ninfo = new NodeInfo();
                        ninfo.nodename = tc;
                        ninfo.wholename = testclass;
                        ninfo.SourceData = classModel;
                        ninfo.ntype = NodeType.testclass;
                        ninfo.nodeid = node.Id;
                        ninfo.parentid = 1;
                        node.StateImageIndex = (int)ninfo.ntype;
                        node.Tag = (NodeInfo)ninfo;

                        string[] testcases = Directory.GetFileSystemEntries(testclass);

                        List<string> nameofcases = new List<string>();

                        foreach (string testcase in testcases)
                        {
                            if (File.Exists(testcase))//file
                            {
                                if (Path.GetExtension(testcase) == ".cas")//test case
                                {
                                    nameofcases.Add(Path.GetFileNameWithoutExtension(testcase));
                                    TreeListNode nodec;
                                    NodeInfo ninfoc = new NodeInfo();
                                    string tcs = Path.GetFileNameWithoutExtension(testcase);
                                    nodec = treeView.AppendNode(new object[] { tcs }, node.Id, -1, -1, 4);//1

                                    TestCaseModel caseModel = new TestCaseModel(classModel);
                                    caseModel.Name = tcs;
                                    caseModel.TextValue = GetTestCaseValue(testcase);
                                    caseModel.IsNewTestModel = m_TestProject.IsNewProject;

                                    ninfoc.nodename = tcs;
                                    ninfoc.wholename = testcase;
                                    ninfoc.ntype = NodeType.testcase;
                                    ninfoc.SourceData = caseModel;
                                    ninfoc.nodeid = nodec.Id;
                                    ninfoc.parentid = node.Id;
                                    nodec.StateImageIndex = (int)ninfoc.ntype;
                                    nodec.Tag = (NodeInfo)ninfoc;

                                    string na = testcase.Substring(0, testcase.Length - 4) + ".tlg";
                                    if (File.Exists(na) && bShowAllFiles)//log exist
                                    {
                                        TreeListNode nodel;
                                        string tcl = Path.GetFileName(na);
                                        NodeInfo ninfol = new NodeInfo();
                                        nodel = treeView.AppendNode(new object[] { tcl }, nodec.Id, -1, -1, 4);//1
                                        ninfol.nodename = tcl;
                                        ninfol.nodeid = nodel.Id;
                                        ninfol.parentid = nodec.Id;
                                        ninfol.wholename = na;
                                        ninfol.ntype = NodeType.logcase;
                                        nodel.StateImageIndex = (int)ninfol.ntype;
                                        nodel.Tag = (NodeInfo)ninfol;
                                    }

                                }
                                else if (bShowAllFiles && !nameofcases.Contains(Path.GetFileNameWithoutExtension(testcase)))//common file
                                {
                                    TreeListNode nodec;
                                    NodeInfo ninfoc = new NodeInfo();
                                    string tcs = testcase.Substring(testcase.LastIndexOf('\\') + 1);
                                    nodec = treeView.AppendNode(new object[] { tcs }, node.Id, -1, -1, 4);//1
                                    ninfoc.nodename = tcs;
                                    ninfoc.wholename = testcase;
                                    ninfoc.ntype = NodeType.other;
                                    nodec.StateImageIndex = (int)ninfoc.ntype;
                                    nodec.Tag = (NodeInfo)ninfoc;
                                }
                            }

                        }
                    }
                    else//file
                    {

                    }
                }
            }
            catch
            {
                XtraMessageBox.Show("There is something wrong in loading Test Cases", "Load Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddBlankNodes()
        {
            treeView.Nodes.Clear();

            TreeListNode node;
            NodeInfo ninfo = new NodeInfo();
            node = treeView.AppendNode(new object[] { "New Test Project" }, -1, -1, -1, 3); //0
            ninfo.ntype = NodeType.root;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Schedule" }, 0, -1, -1, 4);//1
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testschedule;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            //test cases...

            node = treeView.AppendNode(new object[] { "Test Logs" }, 0, -1, -1, 7);//16/10
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.logroot;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            //test logs

            node = treeView.AppendNode(new object[] { "Results" }, 0, -1, -1, 7);//17
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.result;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Settings" }, 0, -1, -1, 12); //22
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.settings;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;
            treeView.ExpandAll();
        }

        public string GetTestCaseValue(string testcasePath)
        {
            string textValue = string.Empty;
            if (false == File.Exists(testcasePath))
            {
                return textValue;
            }

            using (FileStream fileStream = new FileStream(testcasePath, FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(fileStream);
                textValue = reader.ReadToEnd();
                reader.Close();
            }

            return textValue;
        }

        #endregion

        #region 接口

        /// <summary>
        /// 创建节点功能可用状态
        /// </summary>
        /// <returns></returns>
        public bool CanCreateTestCase()
        {
            bool result = false;

            if (null != treeView && null != treeView.FocusedNode)
            {
                NodeInfo nodeInfo = (NodeInfo)treeView.FocusedNode.Tag;

                result = NodeType.testclass == nodeInfo.ntype ? true : false;
            }

            return result;
        }


        /// <summary>
        /// 创建节点功能可用状态
        /// </summary>
        /// <returns></returns>
        public bool CanCreateDevice()
        {
            bool result = false;

            if (null != treeView && null != treeView.FocusedNode)
            {
                NodeInfo nodeInfo = (NodeInfo)treeView.FocusedNode.Tag;

                result = NodeType.dpdevice == nodeInfo.ntype || NodeType.hartdevice == nodeInfo.ntype ? true : false;
            }

            return result;
        }

        /// <summary>
        /// 创建节点功能可用状态
        /// </summary>
        /// <returns></returns>
        public bool CanCreateDPDevice()
        {
            bool result = false;

            if (null != treeView && null != treeView.FocusedNode)
            {
                NodeInfo nodeInfo = (NodeInfo)treeView.FocusedNode.Tag;

                result = NodeType.dpdevice == nodeInfo.ntype ? true : false;
            }

            return result;
        }

        public TreeListNode GetFocusedNode()
        {

            if (null != treeView && null != treeView.FocusedNode)
            {
                return treeView.FocusedNode;
            }

            return null;
        }
        #endregion

        void AddAllNodes(bool showAll)
        {
            treeView.Nodes.Clear();

            TreeListNode node;
            NodeInfo ninfo = new NodeInfo();

            node = treeView.AppendNode(new object[] { "New Test Project" }, -1, -1, -1, 3); //0
            ninfo.ntype = NodeType.root;
            node.StateImageIndex = (int)NodeType.root;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Schedule" }, 0, -1, -1, 4);//1
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testschedule;
            node.StateImageIndex = (int)NodeType.testschedule;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Class1" }, 1, -1, -1, 2);//2
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testclass;
            node.StateImageIndex = (int)NodeType.testclass;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Class2" }, 1, -1, -1, 5);//3
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testclass;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Case1" }, 3, -1, -1, 5);
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testcase;
            node.StateImageIndex = (int)NodeType.testcase;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Case2" }, 3, -1, -1, 5);
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testcase;
            node.StateImageIndex = (int)NodeType.testcase;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Case3" }, 3, -1, -1, 5);
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testcase;
            node.StateImageIndex = (int)NodeType.testcase;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Case4" }, 3, -1, -1, 5);
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testcase;
            node.StateImageIndex = (int)NodeType.testcase;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Case5" }, 3, -1, -1, 5);
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testcase;
            node.StateImageIndex = (int)NodeType.testcase;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Test Case6" }, 3, -1, -1, 5);
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testcase;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            if (showAll)
            {
                treeView.AppendNode(new object[] { "bin" }, 1, -1, -1, 9); //10
                treeView.AppendNode(new object[] { "Debug" }, 10, -1, -1, 9);
                treeView.AppendNode(new object[] { "Release" }, 10, -1, -1, 9);
                treeView.AppendNode(new object[] { "obj" }, 1, -1, -1, 9);//13
                treeView.AppendNode(new object[] { "Debug" }, 13, -1, -1, 9);
                treeView.AppendNode(new object[] { "Release" }, 13, -1, -1, 9);
            }

            node = treeView.AppendNode(new object[] { "Test Logs" }, 0, -1, -1, 7);//16/10
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.logroot;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "Results" }, 0, -1, -1, 7);//17
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.result;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "AssemblyInfo.cs" }, 2, -1, -1, 10);
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.testcase;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "frmMain.cs" }, 1, -1, -1, 11);//19

            node = treeView.AppendNode(new object[] { "ucCodeEditor.cs" }, showAll ? 16 : 10, -1, -1, 12); //20
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.logfolder;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "ucProjectExplorer.cs" }, showAll ? 16 : 10, -1, -1, 12); //21
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.logfolder;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            node = treeView.AppendNode(new object[] { "ucToolbox.cs" }, showAll ? 16 : 10, -1, -1, 12); //22
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.logfolder;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;

            if (showAll)
            {
                treeView.AppendNode(new object[] { "frmMain.resx" }, 19, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucCodeEditor.resx" }, 20, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucProjectExplorer.resx" }, 21, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucToolbox.resx" }, 22, -1, -1, 13);
            }
            node = treeView.AppendNode(new object[] { "Settings", "Settings111" }, 0, -1, -1, 12); //22
            ninfo = new NodeInfo();
            ninfo.ntype = NodeType.settings;
            node.StateImageIndex = (int)ninfo.ntype;
            node.Tag = (NodeInfo)ninfo;
            treeView.ExpandAll();
        }

        void AddRootNodes(bool showAll, string projectName)
        {
            treeView.Nodes.Clear();
            treeView.AppendNode(new object[] { "Test' (1 project)" }, -1, -1, -1, 3); //0
            treeView.AppendNode(new object[] { "VisualStudioInspiredUIDemo" }, -1, -1, -1, 4);//1
            treeView.AppendNode(new object[] { "Properties" }, 1, -1, -1, 2);//2
            treeView.AppendNode(new object[] { "References" }, 1, -1, -1, 5);//3
            treeView.AppendNode(new object[] { "System" }, 3, -1, -1, 5);
            treeView.AppendNode(new object[] { "System.Drawing" }, 3, -1, -1, 5);
            treeView.AppendNode(new object[] { "System.Windows.Forms" }, 3, -1, -1, 5);
            treeView.AppendNode(new object[] { "DevExpress.Utils" }, 3, -1, -1, 5);
            treeView.AppendNode(new object[] { "DevExpress.XtraBars" }, 3, -1, -1, 5);
            treeView.AppendNode(new object[] { "DevExpress.XtraEditors" }, 3, -1, -1, 5);
            if (showAll)
            {
                treeView.AppendNode(new object[] { "bin" }, 1, -1, -1, 9); //10
                treeView.AppendNode(new object[] { "Debug" }, 10, -1, -1, 9);
                treeView.AppendNode(new object[] { "Release" }, 10, -1, -1, 9);
                treeView.AppendNode(new object[] { "obj" }, 1, -1, -1, 9);//13
                treeView.AppendNode(new object[] { "Debug" }, 13, -1, -1, 9);
                treeView.AppendNode(new object[] { "Release" }, 13, -1, -1, 9);
            }
            treeView.AppendNode(new object[] { "Modules" }, 1, -1, 9, 7);//16/10
            treeView.AppendNode(new object[] { "Resources" }, 1, -1, 9, 7);//17
            treeView.AppendNode(new object[] { "AssemblyInfo.cs" }, 2, -1, -1, 10);
            treeView.AppendNode(new object[] { "frmMain.cs" }, 1, -1, -1, 11);//19
            treeView.AppendNode(new object[] { "ucCodeEditor.cs" }, showAll ? 16 : 10, -1, -1, 12); //20
            treeView.AppendNode(new object[] { "ucProjectExplorer.cs" }, showAll ? 16 : 10, -1, -1, 12); //21
            treeView.AppendNode(new object[] { "ucToolbox.cs" }, showAll ? 16 : 10, -1, -1, 12); //22
            if (showAll)
            {
                treeView.AppendNode(new object[] { "frmMain.resx" }, 19, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucCodeEditor.resx" }, 20, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucProjectExplorer.resx" }, 21, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucToolbox.resx" }, 22, -1, -1, 13);
            }
            treeView.ExpandAll();
        }

        public void UpdateNodes()
        {
            if (pType == ProjectType.DP)
            {
                AddDPNodes(devList, testcaseDir, projectname);
            }
            else if (pType == ProjectType.Hart)
            {
                AddHartNodes(devList, testcaseDir, projectname);
            }
            else
            {

            }
        }

        public event EventHandler PropertiesItemClick;

        void iShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //AddAllNodes(((DevExpress.XtraBars.BarButtonItem)e.Item).Down);
            bShowAllFiles = ((DevExpress.XtraBars.BarButtonItem)e.Item).Down;
            UpdateNodes();
        }

        void iProperties_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (PropertiesItemClick != null)
            {
                PropertiesItemClick(sender, EventArgs.Empty);
            }
        }

        public event EventHandler TreeViewItemDoubleClick;

        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            ;
        }

        private void addNodeMenuItemClick(object sender, EventArgs e)
        {
            treeView.Nodes.Add("555", "555", "555", "New Department", "10000", "New Location");
        }

        TreeListMenu tMenu;

        #region 鼠标点击

        public void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (TreeViewItemDoubleClick != null)
            {
                TreeViewItemDoubleClick(sender, EventArgs.Empty);
            }
        }

        #endregion

        #region 菜单操作

        /// <summary>
        /// 菜单显示事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            TreeList treeList = sender as TreeList;
            TreeListHitInfo hitInfo = treeList.CalcHitInfo(e.Point);
            tMenu = e.Menu;
            //e.Menu.Items.Clear();

            if (hitInfo.Node.Tag == null)
            {
                return;
            }

            NodeInfo ni = (NodeInfo)hitInfo.Node.Tag;
            DXMenuItem menuItem;
            switch (ni.ntype)
            {
                case NodeType.root:
                    //DXMenuItem rmenu = new DXMenuItem("Rename", new EventHandler(this.RenameNode), fileTypeSvgImages.GetImage(0));
                    //rmenu.Tag = hitInfo.Node;
                    //e.Menu.Items.Add(rmenu);
                    break;
                // 测试计划表
                case NodeType.testschedule:
                    CreateDXMenuItem(e, "Create Test Class", hitInfo.Node,
                        new EventHandler(AddTestClassNode), fileTypeSvgImages.GetImage(40));
                    break;
                // 测试类
                case NodeType.testclass:
                    CreateDXMenuItem(e, "Create Test Case", hitInfo.Node,
                        new EventHandler(AddTestCaseNode), fileTypeSvgImages.GetImage(40));
                    CreateDXMenuItem(e, "Delete", hitInfo.Node,
                        new EventHandler(DisposeTestLog), fileTypeSvgImages.GetImage(41));
                    break;
                // 测试节点
                case NodeType.testcase:
                    CreateDXMenuItem(e, "Display Test Log", hitInfo.Node,
                        new EventHandler(DisplayTestLog), fileTypeSvgImages.GetImage(27));
                    CreateDXMenuItem(e, "Delete", hitInfo.Node,
                      new EventHandler(DisposeTestLog), fileTypeSvgImages.GetImage(41));
                    break;

                case NodeType.hartdevice:
                case NodeType.dpdevice:
                    CreateDXMenuItem(e, "Create Device", hitInfo.Node,
                        new EventHandler(AddDeviceNode), fileTypeSvgImages.GetImage(0));
                    break;
                case NodeType.devices:
                    CreateDXMenuItem(e, "Delete", hitInfo.Node,
                        new EventHandler(DisposeTestLog), fileTypeSvgImages.GetImage(0));
                    break;

                case NodeType.result:
                    break;

                default:
                    break;
            }
            /*
                 menuItem = new DXMenuItem("All", new EventHandler(this.clearAllMenuItemClick), fileTypeSvgImages.GetImage(0));
                 menuItem.Tag = hitInfo.Column;
                 e.Menu.Items.Add(menuItem);

                 TreeListHitInfo hitInfo = treeList.CalcHitInfo(e.Point);

                 // prohibiting summary footer menu for the "Department" column 
                 if (hitInfo.HitInfoType == HitInfoType.SummaryFooter &&
                   hitInfo.Column.Caption == "Department")
                     e.Allow = false;

                 // removing the "Runtime columns customization" item of the column header menu 
                 if (hitInfo.HitInfoType == HitInfoType.Column)
                 {
                     string caption = TreeListLocalizer.Active.GetLocalizedString(GetMenuColumnCustomizationStringId(treeList));
                     e.Menu.Items.Remove(e.Menu.Items.FirstOrDefault(x => x.Caption == caption));
                 }
                 */
        }

        #region 菜单事件

        /// <summary>
        /// 添加测试类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddTestClassNode(object sender, EventArgs e)
        {
            try
            {
                if (null == sender || false == sender is DXMenuItem)
                {
                    return;
                }

                TreeListNode node = (sender as DXMenuItem).Tag as TreeListNode;

                if (null == node)
                {
                    return;
                }

                NodeInfo nodeInfo = node.Tag as NodeInfo;

                TestClassModel model = new TestClassModel(m_TestProject);
                model.ProjectType = this.pType;
                CreateNode(model, node.Id);
                if (false == node.Expanded)
                {
                    node.Expand();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加测试节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddTestCaseNode(object sender, EventArgs e)
        {
            try
            {
                if (null == sender || false == sender is DXMenuItem)
                {
                    return;
                }

                TreeListNode node = (sender as DXMenuItem).Tag as TreeListNode;

                if (null == node)
                {
                    return;
                }

                NodeInfo nodeInfo = node.Tag as NodeInfo;

                TestCaseModel model = new TestCaseModel(nodeInfo.SourceData as TestClassModel);
                CreateNode(model, node.Id);
                if (false == node.Expanded)
                {
                    node.Expand();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加设备节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddDeviceNode(object sender, EventArgs e)
        {
            try
            {
                if (null == sender || false == sender is DXMenuItem)
                {
                    return;
                }

                TreeListNode node = (sender as DXMenuItem).Tag as TreeListNode;

                if (null == node)
                {
                    return;
                }

                NodeInfo nodeInfo = node.Tag as NodeInfo;

                if (ProjectType.Hart == this.pType)
                {
                    HartDevice device = new HartDevice(devList);
                    CreateNode(device, node.Id);
                }
                else if (ProjectType.DP == this.pType)
                {
                    DPDevice device = new DPDevice(devList);
                    CreateNode(device, node.Id);
                }

                if (false == node.Expanded)
                {
                    node.Expand();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DisplayTestLog(object sender, EventArgs e)
        {
            try
            {
                if (null == sender || false == sender is DXMenuItem)
                {
                    return;
                }

                TreeListNode node = (sender as DXMenuItem).Tag as TreeListNode;

                if (null == node)
                {
                    return;
                }

                NodeInfo nodeInfo = node.Tag as NodeInfo;

                TestCaseModel model = nodeInfo.SourceData as TestCaseModel;

                if (null != DisplayTextEvent)
                {
                    DisplayTextEvent(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DisposeTestLog(object sender, EventArgs e)
        {
            try
            {
                if (null == sender || false == sender is DXMenuItem)
                {
                    return;
                }

                TreeListNode node = (sender as DXMenuItem).Tag as TreeListNode;

                if (null == node)
                {
                    return;
                }

                NodeInfo nodeInfo = node.Tag as NodeInfo;

                IBaseModel model = nodeInfo.SourceData as IBaseModel;
                model.Disposed();

                node.ParentNode.Nodes.Remove(node);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="parentId"></param>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        private TreeListNode CreateNode(TestClassModel model, int parentId)
        {
            try
            {
                TreeListNode node = CreateNode(model.Name, parentId, model.NodeType);
                NodeInfo ninfo = new NodeInfo();
                ninfo.nodename = model.Name;
                ninfo.nodeid = node.Id;
                ninfo.parentid = parentId;
                ninfo.ProjectType = this.pType;
                ninfo.ntype = model.NodeType;
                ninfo.SourceData = model;
                node.Tag = (NodeInfo)ninfo;

                return node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="parentId"></param>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        private TreeListNode CreateNode(TestCaseModel model, int parentId)
        {
            try
            {
                TreeListNode node = CreateNode(model.Name, parentId, model.NodeType);
                NodeInfo ninfo = new NodeInfo();
                ninfo.nodename = model.Name;
                ninfo.nodeid = node.Id;
                ninfo.parentid = parentId;
                ninfo.ntype = model.NodeType;
                ninfo.SourceData = model;
                node.Tag = (NodeInfo)ninfo;

                return node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="parentId"></param>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        private TreeListNode CreateNode(HartDevice model, int parentId)
        {
            try
            {
                TreeListNode node = CreateNode(model.devName, parentId, model.NodeType);
                NodeInfo ninfo = new NodeInfo();
                ninfo.nodename = model.devName;
                ninfo.nodeid = node.Id;
                ninfo.parentid = parentId;
                ninfo.ProjectType = this.pType;
                ninfo.ntype = model.NodeType;
                ninfo.SourceData = model;
                node.Tag = (NodeInfo)ninfo;

                return node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="parentId"></param>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        private TreeListNode CreateNode(DPDevice model, int parentId)
        {
            try
            {
                TreeListNode node = CreateNode(model.devName, parentId, model.NodeType);
                NodeInfo ninfo = new NodeInfo();
                ninfo.nodename = model.devName;
                ninfo.nodeid = node.Id;
                ninfo.parentid = parentId;
                ninfo.ProjectType = this.pType;
                ninfo.ntype = model.NodeType;
                ninfo.SourceData = model;
                node.Tag = (NodeInfo)ninfo;

                return node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="parentId"></param>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        private TreeListNode CreateNode(string caption, int parentId, NodeType nodeType)
        {
            try
            {
                TreeListNode node = treeView.AppendNode(
                    new object[] { caption }, parentId, -1, -1, 4);
                node.StateImageIndex = (int)nodeType;
                return node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="e"></param>
        /// <param name="catption"></param>
        /// <param name="sourceData"></param>
        /// <param name="handler"></param>
        /// <param name="img"></param>
        private void CreateDXMenuItem(PopupMenuShowingEventArgs e, string catption,
            object sourceData, EventHandler handler, Image img)
        {
            try
            {
                DXMenuItem menuItem = new DXMenuItem(catption, handler, img);
                menuItem.Tag = sourceData;
                e.Menu.Items.Add(menuItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region 文件保存

        public void Save()
        {
            foreach (IBaseModel baseModel in m_TestProject.ClassModelList)
            {
                baseModel.Save();
            }
        }

        #endregion

        public event EventHandler deviceRemoving;
        private void removeDevice(object sender, EventArgs e)
        {
            /*
            TreeListColumn clickedColumn = (sender as DXMenuItem).Tag as TreeListColumn;
            if (clickedColumn == null)
            {
                return;
            }
            TreeList tl = clickedColumn.TreeList;
            foreach (TreeListColumn column in tl.Columns)
            {
                column.SummaryFooter = SummaryItemType.None;
            }
            */
            string devname = (string)treeView.FocusedNode.GetValue(0);
            treeView.DeleteNode(treeView.FocusedNode);
            deviceRemoving?.Invoke(devname, e);
        }

        private void clearAllMenuItemClick(object sender, EventArgs e)
        {
            TreeListColumn clickedColumn = (sender as DXMenuItem).Tag as TreeListColumn;
            if (clickedColumn == null)
            {
                return;
            }
            TreeList tl = clickedColumn.TreeList;
            foreach (TreeListColumn column in tl.Columns)
            {
                column.SummaryFooter = SummaryItemType.None;
            }
            treeView.DeleteNode(treeView.FocusedNode);
        }

        private void RenameNode(object sender, EventArgs e)
        {
            TreeListNode node = (sender as DXMenuItem).Tag as TreeListNode;
            if (node == null)
            {
                return;
            }
            //node.
        }

        private TreeListStringId GetMenuColumnCustomizationStringId(TreeList treeList)
        {
            if (treeList.OptionsView.ShowBandsMode == DefaultBoolean.True || (treeList.OptionsView.ShowBandsMode == DefaultBoolean.Default && treeList.Bands.Count > 0))
            {
                return TreeListStringId.MenuColumnBandCustomization;
            }
            return TreeListStringId.MenuColumnColumnCustomization;
        }

        private void biSearch_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (biSearch.Down)
            {
                treeView.ShowFindPanel();
            }
            else
            {
                treeView.HideFindPanel();
            }
        }

        private void treeView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TreeListNode fnode = treeView.FocusedNode;
            NodeInfo ni = (NodeInfo)fnode.Tag;
            if (ni != null && ni.ntype != NodeType.root
                && ni.ntype != NodeType.testcase
                && ni.ntype != NodeType.devices
                && NodeType.testclass != ni.ntype)
            {
                e.Cancel = true;
            }
            else
            {
                treeView.OptionsSelection.EnableAppearanceFocusedCell = true;
                treeView.Appearance.FocusedCell.BackColor = System.Drawing.Color.Transparent;
            }
        }

        private void treeView_HiddenEditor(object sender, EventArgs e)
        {
            treeView.OptionsSelection.EnableAppearanceFocusedCell = false;
        }

        public event EventHandler CellValueChanged;
        private void treeView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (CellValueChanged != null)
            {
                CellValueChanged(sender, e);
            }

        }

        private void treeView_Click(object sender, EventArgs e)
        {
            ;
        }

        private void iRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateNodes();
        }

        private void iRoot_ItemClick(object sender, ItemClickEventArgs e)
        {
            treeView.FocusedNode = treeView.Nodes[0];
        }

    }

    public enum NodeType
    {
        root = 29,
        testschedule = 31,
        testclass = 25,
        testcase = 26,
        result = 30,
        logroot = 27,
        logfolder = 7,
        logcase = 24,
        settings = 36,
        hartdevice = 32,
        dpdevice = 33,
        devices = 37,
        report = 38,
        other = 39
    }

    public class NodeInfo
    {
        public NodeType ntype;
        public string nodename;
        public string wholename;
        public int nodeid;
        public int parentid;
        public ProjectType ProjectType;
        public object SourceData;
    }

}
