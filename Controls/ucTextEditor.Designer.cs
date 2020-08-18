namespace CQC.ConTest
{
    partial class ucTextEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucTextEditor));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.editBar = new DevExpress.XtraRichEdit.UI.CommonBar();
            this.undo = new DevExpress.XtraBars.BarButtonItem();
            this.redo = new DevExpress.XtraBars.BarButtonItem();
            this.cut = new DevExpress.XtraBars.BarButtonItem();
            this.copy = new DevExpress.XtraBars.BarButtonItem();
            this.paste = new DevExpress.XtraBars.BarButtonItem();
            this.delete = new DevExpress.XtraBars.BarButtonItem();
            this.selectAll = new DevExpress.XtraBars.BarButtonItem();
            this.search = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.biFind = new DevExpress.XtraBars.BarButtonItem();
            this.biWordwrap = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.bsiLine = new DevExpress.XtraBars.BarStaticItem();
            this.bsiChar = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(this.components);
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.textEditorControl1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.editBar,
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.svgImageCollection1;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.copy,
            this.paste,
            this.cut,
            this.undo,
            this.selectAll,
            this.delete,
            this.redo,
            this.search,
            this.biFind,
            this.biWordwrap,
            this.bsiLine,
            this.bsiChar});
            this.barManager1.MaxItemId = 341;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit2,
            this.repositoryItemTextEdit1});
            // 
            // editBar
            // 
            this.editBar.BarName = "Edit";
            this.editBar.Control = null;
            this.editBar.DockCol = 0;
            this.editBar.DockRow = 0;
            this.editBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.editBar.HideWhenMerging = DevExpress.Utils.DefaultBoolean.True;
            this.editBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.undo),
            new DevExpress.XtraBars.LinkPersistInfo(this.redo),
            new DevExpress.XtraBars.LinkPersistInfo(this.cut, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.copy),
            new DevExpress.XtraBars.LinkPersistInfo(this.paste),
            new DevExpress.XtraBars.LinkPersistInfo(this.delete, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.selectAll),
            new DevExpress.XtraBars.LinkPersistInfo(this.search, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.biFind),
            new DevExpress.XtraBars.LinkPersistInfo(this.biWordwrap, true)});
            this.editBar.Text = "Edit";
            // 
            // undo
            // 
            this.undo.Caption = "Undo";
            this.undo.Id = 323;
            this.undo.ImageOptions.ImageIndex = 14;
            this.undo.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.undo.Name = "undo";
            this.undo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.undo_ItemClick);
            // 
            // redo
            // 
            this.redo.Caption = "Redo";
            this.redo.Id = 326;
            this.redo.ImageOptions.ImageIndex = 15;
            this.redo.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.redo.Name = "redo";
            this.redo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.redo_ItemClick);
            // 
            // cut
            // 
            this.cut.Caption = "Cut";
            this.cut.Id = 321;
            this.cut.ImageOptions.ImageIndex = 2;
            this.cut.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.cut.Name = "cut";
            this.cut.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.cut_ItemClick);
            // 
            // copy
            // 
            this.copy.Caption = "Copy";
            this.copy.Id = 319;
            this.copy.ImageOptions.ImageIndex = 17;
            this.copy.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.copy.ItemAppearance.Normal.Options.UseBackColor = true;
            this.copy.Name = "copy";
            this.copy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // paste
            // 
            this.paste.Caption = "Paste";
            this.paste.Id = 320;
            this.paste.ImageOptions.ImageIndex = 18;
            this.paste.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.paste.Name = "paste";
            this.paste.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.paste_ItemClick);
            // 
            // delete
            // 
            this.delete.Caption = "Delete";
            this.delete.Id = 325;
            this.delete.ImageOptions.ImageIndex = 9;
            this.delete.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.delete.Name = "delete";
            this.delete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.delete_ItemClick);
            // 
            // selectAll
            // 
            this.selectAll.Caption = "Select all";
            this.selectAll.Id = 324;
            this.selectAll.ImageOptions.ImageIndex = 8;
            this.selectAll.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.selectAll.Name = "selectAll";
            this.selectAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.selectAll_ItemClick);
            // 
            // search
            // 
            this.search.Caption = "search";
            this.search.Edit = this.repositoryItemTextEdit2;
            this.search.EditWidth = 150;
            this.search.Id = 332;
            this.search.Name = "search";
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // biFind
            // 
            this.biFind.Caption = "Search";
            this.biFind.Id = 336;
            this.biFind.ImageOptions.ImageIndex = 12;
            this.biFind.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.biFind.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F));
            this.biFind.Name = "biFind";
            this.biFind.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.biFind_ItemClick);
            // 
            // biWordwrap
            // 
            this.biWordwrap.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.biWordwrap.Caption = "Wordwrap";
            this.biWordwrap.Description = "Toggle word wrap";
            this.biWordwrap.Id = 337;
            this.biWordwrap.ImageOptions.ImageIndex = 13;
            this.biWordwrap.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.biWordwrap.Name = "biWordwrap";
            this.biWordwrap.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.biWordwrap_ItemClick);
            // 
            // bar2
            // 
            this.bar2.BarName = "Status";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar2.FloatLocation = new System.Drawing.Point(400, 381);
            this.bar2.HideWhenMerging = DevExpress.Utils.DefaultBoolean.False;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiLine),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiChar)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DisableClose = true;
            this.bar2.OptionsBar.DisableCustomization = true;
            this.bar2.OptionsBar.DrawBorder = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.Text = "Status";
            // 
            // bsiLine
            // 
            this.bsiLine.Caption = "Ln:";
            this.bsiLine.Id = 338;
            this.bsiLine.Name = "bsiLine";
            // 
            // bsiChar
            // 
            this.bsiChar.Caption = "Ch:";
            this.bsiChar.Id = 340;
            this.bsiChar.Name = "bsiChar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.barDockControlTop.Size = new System.Drawing.Size(1003, 30);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 368);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.barDockControlBottom.Size = new System.Drawing.Size(1003, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 30);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 338);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1003, 30);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 338);
            // 
            // svgImageCollection1
            // 
            this.svgImageCollection1.Add("15_copy", "image://svgimages/edit/copy.svg");
            this.svgImageCollection1.Add("pastespecial", "image://svgimages/richedit/pastespecial.svg");
            this.svgImageCollection1.Add("clip", "image://svgimages/dashboards/clip.svg");
            this.svgImageCollection1.Add("bo_address", "image://svgimages/business objects/bo_address.svg");
            this.svgImageCollection1.Add("bo_document", "image://svgimages/business objects/bo_document.svg");
            this.svgImageCollection1.Add("find", "image://svgimages/find/find.svg");
            this.svgImageCollection1.Add("redo", "image://svgimages/history/redo.svg");
            this.svgImageCollection1.Add("undo", "image://svgimages/history/undo.svg");
            this.svgImageCollection1.Add("selectall", "image://svgimages/pdf viewer/selectall.svg");
            this.svgImageCollection1.Add("deletecomment", "image://svgimages/comments/deletecomment.svg");
            this.svgImageCollection1.Add("removedataitems", "image://svgimages/dashboards/removedataitems.svg");
            this.svgImageCollection1.Add("replace", "image://svgimages/outlook inspired/replace.svg");
            this.svgImageCollection1.Add("actions_zoom", "image://svgimages/icon builder/actions_zoom.svg");
            this.svgImageCollection1.Add("13wordwrap", "image://svgimages/dashboards/wordwrap.svg");
            this.svgImageCollection1.Add("15_Undo", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("svgImageCollection1.15_Undo"))));
            this.svgImageCollection1.Add("16_Redo", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("svgImageCollection1.16_Redo"))));
            this.svgImageCollection1.Add("17_Cut", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("svgImageCollection1.17_Cut"))));
            this.svgImageCollection1.Add("18_Copy", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("svgImageCollection1.18_Copy"))));
            this.svgImageCollection1.Add("19_Paste", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("svgImageCollection1.19_Paste"))));
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // textEditorControl1
            // 
            this.textEditorControl1.AllowDrop = true;
            this.textEditorControl1.AutoSize = true;
            this.textEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditorControl1.Highlighting = null;
            this.textEditorControl1.IsIconBarVisible = true;
            this.textEditorControl1.LineViewerStyle = ICSharpCode.TextEditor.Document.LineViewerStyle.FullRow;
            this.textEditorControl1.Location = new System.Drawing.Point(0, 30);
            this.textEditorControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textEditorControl1.Name = "textEditorControl1";
            this.barManager1.SetPopupContextMenu(this.textEditorControl1, this.popupMenu1);
            this.textEditorControl1.Size = new System.Drawing.Size(1003, 338);
            this.textEditorControl1.TabIndex = 5;
            this.textEditorControl1.Text = resources.GetString("textEditorControl1.Text");
            this.textEditorControl1.SelectionChanged += new System.EventHandler(this.textEditorControl1_SelectionChanged);
            this.textEditorControl1.PositionChanged += new System.EventHandler(this.textEditorControl1_PositionChanged);
            this.textEditorControl1.TextChanged += new System.EventHandler(this.textEditorControl1_TextChanged);
            this.textEditorControl1.Load += new System.EventHandler(this.textEditorControl1_Load);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.undo),
            new DevExpress.XtraBars.LinkPersistInfo(this.redo),
            new DevExpress.XtraBars.LinkPersistInfo(this.cut, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.copy),
            new DevExpress.XtraBars.LinkPersistInfo(this.paste),
            new DevExpress.XtraBars.LinkPersistInfo(this.delete, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.selectAll)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            this.popupMenu1.BeforePopup += new System.ComponentModel.CancelEventHandler(this.popupMenu1_BeforePopup);
            // 
            // bar1
            // 
            this.bar1.BarName = "Search";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.FloatLocation = new System.Drawing.Point(930, 167);
            this.bar1.FloatSize = new System.Drawing.Size(260, 26);
            this.bar1.Offset = 565;
            this.bar1.Text = "Search";
            // 
            // ucTextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textEditorControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "ucTextEditor";
            this.Size = new System.Drawing.Size(1003, 395);
            this.Load += new System.EventHandler(this.ucTextEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraRichEdit.UI.CommonBar editBar;
        private DevExpress.XtraBars.BarButtonItem copy;
        private DevExpress.XtraBars.BarButtonItem paste;
        private ICSharpCode.TextEditor.TextEditorControl textEditorControl1;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem cut;
        private DevExpress.Utils.SvgImageCollection svgImageCollection1;
        private DevExpress.XtraBars.BarButtonItem undo;
        private DevExpress.XtraBars.BarButtonItem selectAll;
        private DevExpress.XtraBars.BarButtonItem delete;
        private DevExpress.XtraBars.BarButtonItem redo;
        private DevExpress.XtraBars.BarEditItem search;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem biFind;
        private DevExpress.XtraBars.BarButtonItem biWordwrap;
        private DevExpress.XtraBars.BarStaticItem bsiLine;
        private DevExpress.XtraBars.BarStaticItem bsiChar;
        private DevExpress.XtraBars.Bar bar2;
    }
}
