using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;

namespace CQC.ConTest
{
    public partial class ucSolutionExplorer : System.Windows.Forms.UserControl
    {
        public ucSolutionExplorer()
        {
            InitializeComponent();
            InitTreeView(treeView);
            treeView.CustomDrawNodeCell += treeView_CustomDrawNodeCell;
            treeView.AfterCollapse += treeView_AfterCollapse;
            treeView.AfterExpand += treeView_AfterExpand;
            AddAllNodes(iShow.Down);
        }
        public static void InitTreeView(DevExpress.XtraTreeList.TreeList treeView)
        {
            TreeListColumn column = treeView.Columns.Add();
            column.Visible = true;
            treeView.OptionsView.ShowColumns = false;
            treeView.OptionsView.ShowIndicator = false;
            treeView.OptionsView.ShowVertLines = false;
            treeView.OptionsView.ShowHorzLines = false;
            treeView.OptionsBehavior.Editable = false;
            treeView.OptionsSelection.EnableAppearanceFocusedCell = false;
        }
        void treeView_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            if (e.Node.Id == 1)
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
        }
        void SetIndex(TreeListNode node, int index, bool expand)
        {
            int newIndex = expand ? index - 1 : index + 1;
            if (node.StateImageIndex == index)
                node.StateImageIndex = newIndex;
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
        void AddAllNodes(bool showAll)
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
            treeView.AppendNode(new object[] { "Modules" }, 1, -1, -1, 7);//16/10
            treeView.AppendNode(new object[] { "Resources" }, 1, -1, -1, 7);//17
            treeView.AppendNode(new object[] { "AssemblyInfo.cs" }, 2, -1, -1, 10);
            treeView.AppendNode(new object[] { "frmMain.cs" }, 1, -1, -1, 11);//19
            treeView.AppendNode(new object[] { "ucCodeEditor.cs" }, showAll ? 16 : 10, -1, -1, 12); //20
            treeView.AppendNode(new object[] { "ucSolutionExplorer.cs" }, showAll ? 16 : 10, -1, -1, 12); //21
            treeView.AppendNode(new object[] { "ucToolbox.cs" }, showAll ? 16 : 10, -1, -1, 12); //22
            if (showAll)
            {
                treeView.AppendNode(new object[] { "frmMain.resx" }, 19, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucCodeEditor.resx" }, 20, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucSolutionExplorer.resx" }, 21, -1, -1, 13);
                treeView.AppendNode(new object[] { "ucToolbox.resx" }, 22, -1, -1, 13);
            }
            treeView.ExpandAll();
        }
        public event EventHandler PropertiesItemClick;
        void iShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddAllNodes(((DevExpress.XtraBars.BarButtonItem)e.Item).Down);
        }
        void iProperties_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (PropertiesItemClick != null)
                PropertiesItemClick(sender, EventArgs.Empty);
        }
        public event EventHandler TreeViewItemClick;
        void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (TreeViewItemClick != null)
                TreeViewItemClick(sender, EventArgs.Empty);
        }
    }
}
