namespace CQC.ConTest {
    partial class ucProjectExplorer {
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucProjectExplorer));
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.iRoot = new DevExpress.XtraBars.BarButtonItem();
            this.iCollapse = new DevExpress.XtraBars.BarButtonItem();
            this.iExpand = new DevExpress.XtraBars.BarButtonItem();
            this.iRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.iShow = new DevExpress.XtraBars.BarButtonItem();
            this.biSearch = new DevExpress.XtraBars.BarButtonItem();
            this.iProperties = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
            this.fileTypeSvgImages = new DevExpress.Utils.SvgImageCollection(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeView = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileTypeSvgImages)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
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
            this.iProperties,
            this.iRoot,
            this.iCollapse,
            this.iExpand,
            this.biSearch});
            this.barManager.MaxItemId = 8;
            // 
            // bar1
            // 
            this.bar1.BarName = "Explorer";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.FloatLocation = new System.Drawing.Point(53, 102);
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.iRoot),
            new DevExpress.XtraBars.LinkPersistInfo(this.iCollapse),
            new DevExpress.XtraBars.LinkPersistInfo(this.iExpand),
            new DevExpress.XtraBars.LinkPersistInfo(this.iRefresh),
            new DevExpress.XtraBars.LinkPersistInfo(this.iShow, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.biSearch, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.iProperties)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.RotateWhenVertical = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Explorer";
            // 
            // iRoot
            // 
            this.iRoot.Caption = "Root";
            this.iRoot.Id = 4;
            this.iRoot.ImageOptions.ImageIndex = 18;
            this.iRoot.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.iRoot.Name = "iRoot";
            this.iRoot.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iRoot_ItemClick);
            // 
            // iCollapse
            // 
            this.iCollapse.Caption = "collapse";
            this.iCollapse.Id = 5;
            this.iCollapse.ImageOptions.ImageIndex = 17;
            this.iCollapse.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.iCollapse.Name = "iCollapse";
            // 
            // iExpand
            // 
            this.iExpand.Caption = "Expand";
            this.iExpand.Id = 6;
            this.iExpand.ImageOptions.ImageIndex = 16;
            this.iExpand.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.iExpand.Name = "iExpand";
            // 
            // iRefresh
            // 
            this.iRefresh.Caption = "Refresh";
            this.iRefresh.Hint = "Refresh";
            this.iRefresh.Id = 0;
            this.iRefresh.ImageOptions.ImageIndex = 19;
            this.iRefresh.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.iRefresh.Name = "iRefresh";
            this.iRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iRefresh_ItemClick);
            // 
            // iShow
            // 
            this.iShow.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.iShow.Caption = "Show All Files";
            this.iShow.Hint = "Show All Files";
            this.iShow.Id = 1;
            this.iShow.ImageOptions.ImageIndex = 1;
            this.iShow.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.iShow.Name = "iShow";
            this.iShow.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iShow_ItemClick);
            // 
            // biSearch
            // 
            this.biSearch.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.biSearch.Caption = "Search Projects";
            this.biSearch.Id = 7;
            this.biSearch.ImageOptions.ImageIndex = 35;
            this.biSearch.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.biSearch.Name = "biSearch";
            this.biSearch.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.biSearch_ItemClick);
            // 
            // iProperties
            // 
            this.iProperties.Caption = "Properties";
            this.iProperties.Hint = "Properties";
            this.iProperties.Id = 2;
            this.iProperties.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.iProperties.Name = "iProperties";
            this.iProperties.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
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
            this.barDockControl1.Size = new System.Drawing.Size(500, 30);
            // 
            // barDockControl2
            // 
            this.barDockControl2.CausesValidation = false;
            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControl2.Location = new System.Drawing.Point(0, 500);
            this.barDockControl2.Manager = this.barManager;
            this.barDockControl2.Size = new System.Drawing.Size(500, 0);
            // 
            // barDockControl3
            // 
            this.barDockControl3.CausesValidation = false;
            this.barDockControl3.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControl3.Location = new System.Drawing.Point(0, 30);
            this.barDockControl3.Manager = this.barManager;
            this.barDockControl3.Size = new System.Drawing.Size(0, 470);
            // 
            // barDockControl4
            // 
            this.barDockControl4.CausesValidation = false;
            this.barDockControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl4.Location = new System.Drawing.Point(500, 30);
            this.barDockControl4.Manager = this.barManager;
            this.barDockControl4.Size = new System.Drawing.Size(0, 470);
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
            this.fileTypeSvgImages.Add("14_actions_home", "image://svgimages/icon builder/actions_home.svg");
            this.fileTypeSvgImages.Add("15_paymentrefund", "image://svgimages/outlook inspired/paymentrefund.svg");
            this.fileTypeSvgImages.Add("16open2", "image://svgimages/actions/open2.svg");
            this.fileTypeSvgImages.Add("17up", "image://svgimages/actions/up.svg");
            this.fileTypeSvgImages.Add("18bo_category", "image://svgimages/business objects/bo_category.svg");
            this.fileTypeSvgImages.Add("19_convertto", "image://svgimages/dashboards/convertto.svg");
            this.fileTypeSvgImages.Add("20_searchsettingbutton", "image://svgimages/pdf viewer/searchsettingbutton.svg");
            this.fileTypeSvgImages.Add("21_pivottableoptions", "image://svgimages/spreadsheet/pivottableoptions.svg");
            this.fileTypeSvgImages.Add("22_workinghours", "image://svgimages/scheduling/workinghours.svg");
            this.fileTypeSvgImages.Add("23_bo_unknown", "image://svgimages/business objects/bo_unknown.svg");
            this.fileTypeSvgImages.Add("24_editquery", "image://svgimages/dashboards/editquery.svg");
            this.fileTypeSvgImages.Add("25_rowtotalsposition", "image://svgimages/dashboards/rowtotalsposition.svg");
            this.fileTypeSvgImages.Add("26_showlegend", "image://svgimages/dashboards/showlegend.svg");
            this.fileTypeSvgImages.Add("27singlepageview", "image://svgimages/pdf viewer/singlepageview.svg");
            this.fileTypeSvgImages.Add("28_differentoddevenpages", "image://svgimages/richedit/differentoddevenpages.svg");
            this.fileTypeSvgImages.Add("29_textalignment0", "image://svgimages/diagramicons/textalignment/textalignment0.svg");
            this.fileTypeSvgImages.Add("30_bo_appointment", "image://svgimages/business objects/bo_appointment.svg");
            this.fileTypeSvgImages.Add("31_switchtimescalesto", "image://svgimages/scheduling/switchtimescalesto.svg");
            this.fileTypeSvgImages.Add("32_printtitles", "image://svgimages/spreadsheet/printtitles.svg");
            this.fileTypeSvgImages.Add("33_filterquery", "image://svgimages/dashboards/filterquery.svg");
            this.fileTypeSvgImages.Add("34_format", "image://svgimages/spreadsheet/format.svg");
            this.fileTypeSvgImages.Add("35_enablesearch", "image://svgimages/dashboards/enablesearch.svg");
            this.fileTypeSvgImages.Add("36_managerules", "image://svgimages/spreadsheet/managerules.svg");
            this.fileTypeSvgImages.Add("37_gaugestylethreeforthcircular", "image://svgimages/reports/gaugestylethreeforthcircular.svg");
            this.fileTypeSvgImages.Add("38_snaptogglefieldhighlighting", "image://svgimages/snap/snaptogglefieldhighlighting.svg");
            this.fileTypeSvgImages.Add("39_newcomment", "image://svgimages/richedit/newcomment.svg");
            this.fileTypeSvgImages.Add("actions_add", "image://svgimages/icon builder/actions_add.svg");
            this.fileTypeSvgImages.Add("actions_remove", "image://svgimages/icon builder/actions_remove.svg");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.treeView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 470);
            this.panel1.TabIndex = 4;
            // 
            // treeView
            // 
            this.treeView.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.OptionsBehavior.AllowExpandOnDblClick = false;
            this.treeView.OptionsBehavior.EditorShowMode = DevExpress.XtraTreeList.TreeListEditorShowMode.Click;
            this.treeView.OptionsEditForm.EditFormColumnCount = 1;
            this.treeView.OptionsEditForm.ShowOnF2Key = DevExpress.Utils.DefaultBoolean.True;
            this.treeView.OptionsFind.ShowCloseButton = false;
            this.treeView.OptionsSelection.SelectNodesOnRightClick = true;
            this.treeView.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            this.treeView.Size = new System.Drawing.Size(500, 470);
            this.treeView.StateImageList = this.fileTypeSvgImages;
            this.treeView.TabIndex = 0;
            this.treeView.HiddenEditor += new System.EventHandler(this.treeView_HiddenEditor);
            this.treeView.PopupMenuShowing += new DevExpress.XtraTreeList.PopupMenuShowingEventHandler(this.treeView_PopupMenuShowing);
            this.treeView.CellValueChanged += new DevExpress.XtraTreeList.CellValueChangedEventHandler(this.treeView_CellValueChanged);
            this.treeView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.treeView_ShowingEditor);
            this.treeView.Click += new System.EventHandler(this.treeView_Click);
            this.treeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDoubleClick);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "treeListColumn1";
            this.treeListColumn1.ColumnEdit = this.repositoryItemTextEdit1;
            this.treeListColumn1.FieldName = "treeListColumn1";
            this.treeListColumn1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("treeListColumn1.ImageOptions.SvgImage")));
            this.treeListColumn1.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // ucProjectExplorer
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.barDockControl3);
            this.Controls.Add(this.barDockControl4);
            this.Controls.Add(this.barDockControl2);
            this.Controls.Add(this.barDockControl1);
            this.Name = "ucProjectExplorer";
            this.Size = new System.Drawing.Size(500, 500);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileTypeSvgImages)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
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
        private DevExpress.Utils.SvgImageCollection fileTypeSvgImages;
        private DevExpress.XtraBars.BarButtonItem iRoot;
        private DevExpress.XtraBars.BarButtonItem iCollapse;
        private DevExpress.XtraBars.BarButtonItem iExpand;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem biSearch;
        private System.ComponentModel.IContainer components;
    }
}
