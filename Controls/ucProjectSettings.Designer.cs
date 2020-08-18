namespace CQC.ConTest {
    partial class ucProjectSettings
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tab1 = new DevExpress.XtraVerticalGrid.Tab();
            this.tab2 = new DevExpress.XtraVerticalGrid.Tab();
            this.tab3 = new DevExpress.XtraVerticalGrid.Tab();
            this.comboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBox.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 20);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.Size = new System.Drawing.Size(750, 286);
            this.propertyGrid1.TabIndex = 5;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // tab1
            // 
            this.tab1.Caption = "Tab 1";
            this.tab1.Name = "tab1";
            // 
            // tab2
            // 
            this.tab2.Caption = "Tab 2";
            this.tab2.Name = "tab2";
            // 
            // tab3
            // 
            this.tab3.Caption = "Tab 3";
            this.tab3.Name = "tab3";
            // 
            // comboBox
            // 
            this.comboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox.Location = new System.Drawing.Point(0, 0);
            this.comboBox.Name = "comboBox";
            this.comboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBox.Size = new System.Drawing.Size(750, 20);
            this.comboBox.TabIndex = 3;
            this.comboBox.Visible = false;
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // ucProjectSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.comboBox);
            this.Name = "ucProjectSettings";
            this.Size = new System.Drawing.Size(750, 306);
            this.Load += new System.EventHandler(this.ucProjectSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.comboBox.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private DevExpress.XtraVerticalGrid.Tab tab1;
        private DevExpress.XtraVerticalGrid.Tab tab2;
        private DevExpress.XtraVerticalGrid.Tab tab3;
        private DevExpress.XtraEditors.ComboBoxEdit comboBox;
    }
}
