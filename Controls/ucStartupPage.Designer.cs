namespace CQC.ConTest
{
    partial class ucStartupPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucStartupPage));
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(this.components);
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gridRecent = new DevExpress.XtraGrid.GridControl();
            this.gvRecent = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProjectName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProjectIcon = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ucImageButton2 = new CQC.ConTest.ucImageButton();
            this.ucImageButton1 = new CQC.ConTest.ucImageButton();
            this.ucImageButton3 = new CQC.ConTest.ucImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecent)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.SvgImageSize = new System.Drawing.Size(16, 16);
            // 
            // svgImageCollection1
            // 
            this.svgImageCollection1.Add("switchtimescalesto", "image://svgimages/scheduling/switchtimescalesto.svg");
            this.svgImageCollection1.Add("groupfieldscollection", "image://svgimages/snap/groupfieldscollection.svg");
            this.svgImageCollection1.Add("formatcells", "image://svgimages/spreadsheet/formatcells.svg");
            this.svgImageCollection1.Add("workweekview", "image://svgimages/scheduling/workweekview.svg");
            this.svgImageCollection1.Add("editcomment", "image://svgimages/spreadsheet/editcomment.svg");
            this.svgImageCollection1.Add("actions_checkcircled", "image://svgimages/icon builder/actions_checkcircled.svg");
            this.svgImageCollection1.Add("actions_deletecircled", "image://svgimages/icon builder/actions_deletecircled.svg");
            this.svgImageCollection1.Add("actions_forbid", "image://svgimages/icon builder/actions_forbid.svg");
            // 
            // popupMenu1
            // 
            this.popupMenu1.Name = "popupMenu1";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl2.Location = new System.Drawing.Point(4, 58);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(831, 36);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "Open recent";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl3.Location = new System.Drawing.Point(843, 58);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(518, 36);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "Get start";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl1.Location = new System.Drawing.Point(4, 4);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(831, 46);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "What would you like to do?";
            // 
            // gridRecent
            // 
            this.gridRecent.DataSource = this.bindingSource1;
            this.gridRecent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRecent.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridRecent.Location = new System.Drawing.Point(4, 102);
            this.gridRecent.MainView = this.gvRecent;
            this.gridRecent.Margin = new System.Windows.Forms.Padding(4);
            this.gridRecent.Name = "gridRecent";
            this.gridRecent.Size = new System.Drawing.Size(831, 662);
            this.gridRecent.TabIndex = 19;
            this.gridRecent.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRecent});
            // 
            // gvRecent
            // 
            this.gvRecent.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Transparent;
            this.gvRecent.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvRecent.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colProjectName,
            this.colTime,
            this.colProjectIcon,
            this.colDescription});
            this.gvRecent.DetailHeight = 437;
            this.gvRecent.GridControl = this.gridRecent;
            this.gvRecent.Images = this.svgImageCollection1;
            this.gvRecent.Name = "gvRecent";
            this.gvRecent.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvRecent.OptionsView.AutoCalcPreviewLineCount = true;
            this.gvRecent.OptionsView.EnableAppearanceEvenRow = true;
            this.gvRecent.OptionsView.ShowGroupPanel = false;
            this.gvRecent.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gvRecent.OptionsView.ShowIndicator = false;
            this.gvRecent.OptionsView.ShowPreview = true;
            this.gvRecent.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gvRecent.PreviewFieldName = "Description";
            this.gvRecent.PreviewIndent = 0;
            this.gvRecent.PreviewLineCount = 3;
            this.gvRecent.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvResult_CustomUnboundColumnData);
            this.gvRecent.DoubleClick += new System.EventHandler(this.gvRecent_DoubleClick);
            // 
            // colProjectName
            // 
            this.colProjectName.AppearanceCell.BackColor = System.Drawing.Color.Transparent;
            this.colProjectName.AppearanceCell.BackColor2 = System.Drawing.Color.Transparent;
            this.colProjectName.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.colProjectName.AppearanceCell.Options.UseBackColor = true;
            this.colProjectName.AppearanceCell.Options.UseFont = true;
            this.colProjectName.Caption = "PROJECT NAME";
            this.colProjectName.FieldName = "ProjectName";
            this.colProjectName.ImageOptions.ImageIndex = 1;
            this.colProjectName.MinWidth = 27;
            this.colProjectName.Name = "colProjectName";
            this.colProjectName.OptionsColumn.AllowEdit = false;
            this.colProjectName.OptionsColumn.AllowFocus = false;
            this.colProjectName.Visible = true;
            this.colProjectName.VisibleIndex = 1;
            this.colProjectName.Width = 135;
            // 
            // colTime
            // 
            this.colTime.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colTime.AppearanceCell.Options.UseFont = true;
            this.colTime.Caption = "TIME";
            this.colTime.FieldName = "LastModified";
            this.colTime.ImageOptions.ImageIndex = 0;
            this.colTime.MinWidth = 27;
            this.colTime.Name = "colTime";
            this.colTime.OptionsColumn.AllowEdit = false;
            this.colTime.OptionsColumn.AllowFocus = false;
            this.colTime.Visible = true;
            this.colTime.VisibleIndex = 2;
            this.colTime.Width = 139;
            // 
            // colProjectIcon
            // 
            this.colProjectIcon.Caption = " ";
            this.colProjectIcon.ColumnEdit = this.repositoryItemPictureEdit1;
            this.colProjectIcon.FieldName = "ProjectIcon";
            this.colProjectIcon.MinWidth = 27;
            this.colProjectIcon.Name = "colProjectIcon";
            this.colProjectIcon.OptionsColumn.AllowEdit = false;
            this.colProjectIcon.OptionsColumn.AllowFocus = false;
            this.colProjectIcon.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.True;
            this.colProjectIcon.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.colProjectIcon.OptionsColumn.AllowSize = false;
            this.colProjectIcon.OptionsColumn.FixedWidth = true;
            this.colProjectIcon.OptionsFilter.AllowAutoFilter = false;
            this.colProjectIcon.OptionsFilter.AllowFilter = false;
            this.colProjectIcon.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.colProjectIcon.Visible = true;
            this.colProjectIcon.VisibleIndex = 0;
            this.colProjectIcon.Width = 33;
            // 
            // colDescription
            // 
            this.colDescription.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.colDescription.AppearanceCell.Options.UseBackColor = true;
            this.colDescription.Caption = "DESCRIPTION";
            this.colDescription.FieldName = "Description";
            this.colDescription.MinWidth = 27;
            this.colDescription.Name = "colDescription";
            this.colDescription.OptionsColumn.AllowEdit = false;
            this.colDescription.OptionsColumn.AllowFocus = false;
            this.colDescription.Width = 585;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.52344F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.47656F));
            this.tableLayoutPanel1.Controls.Add(this.labelControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelControl3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelControl2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gridRecent, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1365, 764);
            this.tableLayoutPanel1.TabIndex = 20;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.ucImageButton2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.ucImageButton1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ucImageButton3, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(843, 102);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(518, 662);
            this.tableLayoutPanel2.TabIndex = 20;
            // 
            // ucImageButton2
            // 
            this.ucImageButton2.Detail = "Creat a new test project";
            this.ucImageButton2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucImageButton2.Header = "Creat a project";
            this.ucImageButton2.Location = new System.Drawing.Point(4, 84);
            this.ucImageButton2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucImageButton2.Name = "ucImageButton2";
            this.ucImageButton2.Size = new System.Drawing.Size(401, 69);
            this.ucImageButton2.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ucImageButton2.SvgImage")));
            this.ucImageButton2.TabIndex = 1;
            this.ucImageButton2.MouseDownClick += new System.EventHandler(this.ucImageButton2_Click);
            // 
            // ucImageButton1
            // 
            this.ucImageButton1.Detail = "Open an exsiting test project";
            this.ucImageButton1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucImageButton1.Header = "Open a project";
            this.ucImageButton1.Location = new System.Drawing.Point(4, 5);
            this.ucImageButton1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucImageButton1.Name = "ucImageButton1";
            this.ucImageButton1.Size = new System.Drawing.Size(401, 69);
            this.ucImageButton1.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ucImageButton1.SvgImage")));
            this.ucImageButton1.TabIndex = 2;
            this.ucImageButton1.MouseDownClick += new System.EventHandler(this.ucImageButton1_Click);
            // 
            // ucImageButton3
            // 
            this.ucImageButton3.Detail = "Get help infomation";
            this.ucImageButton3.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucImageButton3.Header = "Get help";
            this.ucImageButton3.Location = new System.Drawing.Point(4, 163);
            this.ucImageButton3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucImageButton3.Name = "ucImageButton3";
            this.ucImageButton3.Size = new System.Drawing.Size(401, 69);
            this.ucImageButton3.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ucImageButton3.SvgImage")));
            this.ucImageButton3.TabIndex = 3;
            this.ucImageButton3.MouseDownClick += new System.EventHandler(this.ucImageButton3_Click);
            // 
            // ucStartupPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ucStartupPage";
            this.Size = new System.Drawing.Size(1365, 764);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecent)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private DevExpress.Utils.SvgImageCollection svgImageCollection1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.GridControl gridRecent;
        private DevExpress.XtraGrid.Views.Grid.GridView gvRecent;
        private DevExpress.XtraGrid.Columns.GridColumn colProjectName;
        private DevExpress.XtraGrid.Columns.GridColumn colTime;
        private DevExpress.XtraGrid.Columns.GridColumn colProjectIcon;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private ucImageButton ucImageButton2;
        private ucImageButton ucImageButton1;
        private ucImageButton ucImageButton3;
    }
}
