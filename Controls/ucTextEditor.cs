using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using CQC.Controls.Models;
using DevExpress.Utils.MVVM;
using DevExpress.XtraRichEdit;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace CQC.ConTest
{
    public partial class ucTextEditor : DevExpress.XtraEditors.XtraUserControl
    {
        private TestCaseModel m_SourceData = null;

        public ucTextEditor()
        {
            InitializeComponent();
            textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("TestCase");
        }

        public ucTextEditor(string highlight)
        {
            InitializeComponent();
            textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(highlight);
        }

        public ICSharpCode.TextEditor.TextEditorControl editor
        {
            get
            {
                return textEditorControl1;
            }
        }

        public void LoadString(string str)
        {
            textEditorControl1.LoadString(str);
        }

        public void LoadCode(string file, bool bReadonly = false, bool bAutoLoadHighlight = false, bool bAutoDetectHighlight = false)
        {
            if (file != null)
            {
                textEditorControl1.LoadFile(file, bAutoLoadHighlight, bAutoDetectHighlight);
            }
            if (bReadonly)
            {
                textEditorControl1.ReadOnly = true;
            }
            //richEditControl1.LoadDocument(stream, DocumentFormat.Rtf);
            //richEditControl1.Document.Sections[0].Page.Width = 10000;
        }

        public void LoadCode(TestCaseModel caseModel, bool bReadonly = false, bool bAutoLoadHighlight = false, bool bAutoDetectHighlight = false)
        {
            if (caseModel != null)
            {
                m_SourceData = caseModel;
                try
                {
                    using (Stream stream = new MemoryStream())
                    {
                        XmlDocument doc = new XmlDocument();
                        TextReader textReader = new StringReader(caseModel.TextValue);
                        doc.Load(textReader);
                        XmlNode node = (XmlNode)doc.DocumentElement;
                        XmlAttribute nameAttr = node.Attributes["Name"];
                        nameAttr.Value = caseModel.GetFinalName();
                        XmlAttribute classAttr = node.Attributes["Class"];
                        classAttr.Value = caseModel.ParentNodedata.GetFinalName();
                        caseModel.TextValue = ConvertXmlToString(doc);
                    }
                }
                catch (Exception)
                {
                }
                textEditorControl1.Text = caseModel.TextValue;

            }
            if (bReadonly)
            {
                textEditorControl1.ReadOnly = true;
            }
            //richEditControl1.LoadDocument(stream, DocumentFormat.Rtf);
            //richEditControl1.Document.Sections[0].Page.Width = 10000;
        }

        #region 方法

        /// <summary>
        /// 将XmlDocument转化为string
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public string ConvertXmlToString(XmlDocument xmlDoc)
        {
            string xmlString = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            using (XmlTextWriter writer = new XmlTextWriter(stream, null))
            {
                writer.Formatting = Formatting.Indented;
                xmlDoc.Save(writer);
                StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
                stream.Position = 0;
                xmlString = sr.ReadToEnd();
                writer.Close();
                sr.Close();
            }
            return xmlString;
        }

        #endregion

        #region 接口

        public string GetTextValue()
        {
            return textEditorControl1.Text;
        }

        #endregion

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new ICSharpCode.TextEditor.Actions.Copy().Execute(textEditorControl1.ActiveTextAreaControl.TextArea);
            textEditorControl1.ActiveTextAreaControl.TextArea.Focus();
        }

        public void updateBarButton()
        {
            undo.Enabled = textEditorControl1.ActiveTextAreaControl.Document.UndoStack.CanUndo;
            redo.Enabled = textEditorControl1.ActiveTextAreaControl.Document.UndoStack.CanRedo;
            cut.Enabled = copy.Enabled = delete.Enabled = textEditorControl1.ActiveTextAreaControl.SelectionManager.HasSomethingSelected;
            paste.Enabled = textEditorControl1.ActiveTextAreaControl.TextArea.ClipboardHandler.EnablePaste;
            selectAll.Enabled = !string.IsNullOrEmpty(textEditorControl1.ActiveTextAreaControl.Document.TextContent);
        }

        private void popupMenu1_BeforePopup(object sender, System.ComponentModel.CancelEventArgs e)
        {
            updateBarButton();
        }

        int startindex = 0;

        private void undo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            textEditorControl1.Undo();
        }

        public void SaveToFile(string filename)
        {
            //textEditorControl1.SaveFile(filename);
        }

        private void redo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            textEditorControl1.Redo();
        }

        private void paste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new ICSharpCode.TextEditor.Actions.Paste().Execute(textEditorControl1.ActiveTextAreaControl.TextArea);
            textEditorControl1.ActiveTextAreaControl.TextArea.Focus();

        }

        private void cut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new ICSharpCode.TextEditor.Actions.Cut().Execute(textEditorControl1.ActiveTextAreaControl.TextArea);
            textEditorControl1.ActiveTextAreaControl.TextArea.Focus();

        }

        private void selectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new ICSharpCode.TextEditor.Actions.SelectWholeDocument().Execute(textEditorControl1.ActiveTextAreaControl.TextArea);
            textEditorControl1.ActiveTextAreaControl.TextArea.Focus();

        }

        private void delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new ICSharpCode.TextEditor.Actions.Delete().Execute(textEditorControl1.ActiveTextAreaControl.TextArea);
            textEditorControl1.ActiveTextAreaControl.TextArea.Focus();
        }

        private void biFind_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string text = (string)search.EditValue;//取得要查找的文本
            if (text == null || text == "")
            {
                return;
            }

            var offset = this.textEditorControl1.Text.IndexOf(text, startindex, StringComparison.CurrentCultureIgnoreCase);
            textEditorControl1.ActiveTextAreaControl.SelectionManager.ClearSelection();
            if (offset >= 0)
            {
                startindex = offset + text.Length;//为查找下一个做准备
                var start = this.textEditorControl1.Document.OffsetToPosition(offset);
                var end = this.textEditorControl1.Document.OffsetToPosition(offset + text.Length);
                this.textEditorControl1.ActiveTextAreaControl.SelectionManager.SetSelection(new DefaultSelection(this.textEditorControl1.Document, start, end));

                //滚动到选择的位置。
                this.textEditorControl1.ActiveTextAreaControl.Caret.Position = end;
                this.textEditorControl1.ActiveTextAreaControl.TextArea.ScrollToCaret();
            }
            else
            {
                startindex = 0;//循环查找
            }
        }

        private void ucTextEditor_Load(object sender, EventArgs e)
        {
            updateBarButton();
        }

        private void textEditorControl1_SelectionChanged(object sender, EventArgs e)
        {
            updateBarButton();
        }

        private void textEditorControl1_Load(object sender, EventArgs e)
        {
            textEditorControl1.setSelectionChangedEvent();
            textEditorControl1.setPositionChangedEvent();
        }

        private void biWordwrap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ;// textEditorControl1.ActiveTextAreaControl.TextArea.wrap
        }

        private void textEditorControl1_PositionChanged(object sender, EventArgs e)
        {
            bsiLine.Caption = "Ln: " + (textEditorControl1.ActiveTextAreaControl.Caret.Line + 1).ToString();
            bsiChar.Caption = "Ch: " + (textEditorControl1.ActiveTextAreaControl.Caret.Column + 1).ToString();
        }

        private void textEditorControl1_TextChanged(object sender, EventArgs e)
        {
            if (null != m_SourceData)
            {
                m_SourceData.TextValue = this.textEditorControl1.Text;
            }
        }
    }
}
