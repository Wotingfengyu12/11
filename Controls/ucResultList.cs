using System.IO;
using System.Windows.Forms;
using DevExpress.XtraRichEdit;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using System;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils.Svg;
using System.Data;
using System.Windows.Documents;
using System.Collections.Generic;

namespace CQC.ConTest
{
    public partial class ucResultList : UserControl
    {
        public ucResultList()
        {
            InitializeComponent();
        }

        public ucResultList(BindingSource bs)
        {
            InitializeComponent();
            gridControlResult.DataSource = bs;
        }

        public void LoadResult()
        {
            repositoryItemImageComboBox1.Items.Clear();
            repositoryItemImageComboBox1.Items.Add("PASSED", TestRes.PASSED, 5);
            repositoryItemImageComboBox1.Items.Add("FAILED", TestRes.FAILED, 6);
            repositoryItemImageComboBox1.Items.Add("NOT RUN", TestRes.NOT_RUN, 7);


            //textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            //richEditControl1.LoadDocument(stream, DocumentFormat.Rtf);
            //richEditControl1.Document.Sections[0].Page.Width = 10000;
        }

        public void AddResultItem()
        {
            gvResult.AddNewRow();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //repositoryItemImageComboBox1.tex

            TestResult tr = new TestResult();
            tr.Comment = "xxxxx";
            tr.Result = TestRes.PASSED;
            tr.Description = "barButtonItem3_ItemClick\n barButtonItem3_ItemClick \n barButtonItem3_ItemClick";
            tr.TestClass = "DP V0";
            tr.TestCase = "GSD";
            tr.Time = DateTime.Now.ToUniversalTime().ToString();
            repositoryItemImageComboBox1.GlyphAlignment = DevExpress.Utils.HorzAlignment.Center;
            bindingSource1.Add(tr);
        }

        public void addResult(TestResult tr)
        {
            bindingSource1.Add(tr);
        }

        public void clearResult()
        {
            bindingSource1.Clear();
        }

        private void gvResult_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "ResultIcon" && e.IsGetData)
            {
                try
                {
                    if (e.Row != null)
                    {
                        TestResult row = (TestResult)e.Row;
                        {
                            SvgImage img = svgImageCollection1[Convert.ToInt32(row.Result)];
                            e.Value = img;
                        }
                    }
                }
                catch (NullReferenceException nullReferenceException)
                {
                    string err = nullReferenceException.Message;
                }
            }
        }
    }

    public class TestResult
    {
        private List<string> m_Errors = null;

        public TestResult()
        {
            Result = TestRes.NOT_RUN;
            Time = "Not Available";
            m_Errors = new List<string>();
        }

        public string TestClass
        {
            get;
            set;
        }

        public string TestCase
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Time
        {
            get;
            set;
        }

        public TestRes Result
        {
            get;
            set;
        }

        public string Comment
        {
            get;
            set;
        }
        public List<string> Errors { get => m_Errors; set => m_Errors = value; }

    }

    public enum TestRes
    {
        PASSED = 5,
        FAILED,
        NOT_RUN
    }
}
