namespace CQC.ConTest {
    partial class ucSolutionExplorer {
        protected override void Dispose(bool disposing) {
            if(disposing) {
                if(components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSolutionExplorer));
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.iRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.iShow = new DevExpress.XtraBars.BarButtonItem();
            this.iProperties = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
            this.fileTypeSvgImages = new DevExpress.Utils.SvgImageCollection(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeView = new DevExpress.XtraTreeList.TreeList();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileTypeSvgImages)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeView)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager.Controller = this.barAndDockingController;
            this.barManager.DockControls.Add(this.barDockControl1);
            this.barManager.DockControls.Add(this.barDockControl2);
            this.barManager.DockControls.Add(this.barDockControl3);
            this.barManager.DockControls.Add(this.barDockControl4);
            this.barManager.Form = this;
            this.barManager.Images = this.fileTypeSvgImages;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.iRefresh,
            this.iShow,
            this.iProperties});
            // 
            // bar1
            // 
            this.bar1.BarName = "Explorer";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.FloatLocation = new System.Drawing.Point(53, 102);
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.iRefresh),
            new DevExpress.XtraBars.LinkPersistInfo(this.iShow, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.iProperties, true)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.RotateWhenVertical = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Explorer";
            // 
            // iRefresh
            // 
            this.iRefresh.Caption = "Refresh";
            this.iRefresh.Hint = "Refresh";
            this.iRefresh.Id = 0;
            this.iRefresh.ImageOptions.ImageIndex = 0;
            this.iRefresh.Name = "iRefresh";
            // 
            // iShow
            // 
            this.iShow.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.iShow.Caption = "Show All Files";
            this.iShow.Hint = "Show All Files";
            this.iShow.Id = 1;
            this.iShow.ImageOptions.ImageIndex = 1;
            this.iShow.Name = "iShow";
            this.iShow.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iShow_ItemClick);
            // 
            // iProperties
            // 
            this.iProperties.Caption = "Properties";
            this.iProperties.Hint = "Properties";
            this.iProperties.Id = 2;
            this.iProperties.Name = "iProperties";
            this.iProperties.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iProperties_ItemClick);
            // 
            // barAndDockingController
            // 
            this.barAndDockingController.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControl1
            // 
            this.barDockControl1.CausesValidation = false;
            this.barDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl1.Location = new System.Drawing.Point(0, 0);
            this.barDockControl1.Manager = this.barManager;
            this.barDockControl1.Size = new System.Drawing.Size(288, 28);
            // 
            // barDockControl2
            // 
            this.barDockControl2.CausesValidation = false;
            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControl2.Location = new System.Drawing.Point(0, 288);
            this.barDockControl2.Manager = this.barManager;
            this.barDockControl2.Size = new System.Drawing.Size(288, 0);
            // 
            // barDockControl3
            // 
            this.barDockControl3.CausesValidation = false;
            this.barDockControl3.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControl3.Location = new System.Drawing.Point(0, 28);
            this.barDockControl3.Manager = this.barManager;
            this.barDockControl3.Size = new System.Drawing.Size(0, 260);
            // 
            // barDockControl4
            // 
            this.barDockControl4.CausesValidation = false;
            this.barDockControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl4.Location = new System.Drawing.Point(288, 28);
            this.barDockControl4.Manager = this.barManager;
            this.barDockControl4.Size = new System.Drawing.Size(0, 260);
            // 
            // fileTypeSvgImages
            // 
            this.fileTypeSvgImages.Add("0_refresh_16xLG", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.0_refresh_16xLG"))));
            this.fileTypeSvgImages.Add("1_ShowAllFiles", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.1_ShowAllFiles"))));
            this.fileTypeSvgImages.Add("2_Property_501", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.2_Property_501"))));
            this.fileTypeSvgImages.Add("3_Solution_8309", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.3_Solution_8309"))));
            this.fileTypeSvgImages.Add("4_CSharpProject_SolutionExplorerNode", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.4_CSharpProject_SolutionExplorerNode"))));
            this.fileTypeSvgImages.Add("5_reference_16xLG", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.5_reference_16xLG"))));
            this.fileTypeSvgImages.Add("6_FolderOpen", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.6_FolderOpen"))));
            this.fileTypeSvgImages.Add("7_Folder", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.7_Folder"))));
            this.fileTypeSvgImages.Add("8_HiddenFolder_428", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.8_HiddenFolder_428"))));
            this.fileTypeSvgImages.Add("9_HiddenFolder_427", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.9_HiddenFolder_427"))));
            this.fileTypeSvgImages.Add("10_CSharpFile_SolutionExplorerNode", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.10_CSharpFile_SolutionExplorerNode"))));
            this.fileTypeSvgImages.Add("11_dialog_16xLG", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.11_dialog_16xLG"))));
            this.fileTypeSvgImages.Add("12_UserControl", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.12_UserControl"))));
            this.fileTypeSvgImages.Add("13_VSProject_generatedfile", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("fileTypeSvgImages.13_VSProject_generatedfile"))));
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.treeView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(288, 260);
            this.panel1.TabIndex = 4;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(288, 260);
            this.treeView.StateImageList = this.fileTypeSvgImages;
            this.treeView.TabIndex = 0;
            this.treeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDoubleClick);
            // 
            // ucSolutionExplorer
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.barDockControl3);
            this.Controls.Add(this.barDockControl4);
            this.Controls.Add(this.barDockControl2);
            this.Controls.Add(this.barDockControl1);
            this.Name = "ucSolutionExplorer";
            this.Size = new System.Drawing.Size(288, 288);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileTypeSvgImages)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControl1;
        private DevExpress.XtraBars.BarDockControl barDockControl2;
        private DevExpress.XtraBars.BarDockControl barDockControl3;
        private DevExpress.XtraBars.BarDockControl barDockControl4;
        private DevExpress.XtraBars.BarButtonItem iRefresh;
        private DevExpress.XtraBars.BarButtonItem iShow;
        private DevExpress.XtraBars.BarButtonItem iProperties;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraTreeList.TreeList treeView;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController;
        private System.ComponentModel.IContainer components;
        private DevExpress.Utils.SvgImageCollection fileTypeSvgImages;
    }
}
