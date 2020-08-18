using System.IO;
using System.Windows.Forms;
using DevExpress.XtraRichEdit;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using System;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils.Svg;
using System.Data;
using System.Collections.Generic;
using CQC.Controls.Models;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraBars;

namespace CQC.ConTest
{
    public partial class ucStartupPage : UserControl
    {
        #region 委托

        public delegate bool LoadProjectHandler(string path);
        public event LoadProjectHandler LoadProjectEvent;
        public event ItemClickEventHandler OpenProjectEvent;
        public event ItemClickEventHandler CreateProjectEvent;
        public event ItemClickEventHandler HelpEvent;

        #endregion
        private List<TestProject> projectList;
        public ucStartupPage()
        {
            InitializeComponent();
        }

        public ucStartupPage(List<TestProject> plist)
        {
            InitializeComponent();
            projectList = plist;
        }

        /// <summary>
        /// 加载历史工程列表
        /// </summary>
        public void LoadProjectList()
        {
            try
            {
                // 历史文件模型
                HistoryModels models = HistoryModels.Open();

                models.Sort((model1, model2) =>
                {
                    return model1.LastModified > model2.LastModified ? -1
                    : model1.LastModified == model2.LastModified ? 0 : 1;
                });

                BindingSource bindingSource = new BindingSource();
                foreach (var item in models)
                {
                    if (false == File.Exists(item.FilePath))
                    {
                        continue;
                    }
                    bindingSource.Add(item);
                }

                gridRecent.DataSource = bindingSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddResultItem()
        {
            gvRecent.AddNewRow();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TestProject tp = new TestProject();
            tp.Description = "this is a DP ss f ";
            tp.timemodified = DateTime.Now;
            tp.TypeofProject = ProjectType.DP;
            tp.ProjectName = "bbb";
            bindingSource1.Add(tp);
            tp = new TestProject();
            tp.Description = "this is a hart ss f ";
            tp.timemodified = DateTime.Now;
            tp.TypeofProject = ProjectType.Hart;
            tp.ProjectName = "aaa";
            bindingSource1.Add(tp);
        }

        private void gvResult_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "ProjectIcon" && e.IsGetData)
            {
                try
                {
                    if (e.Row != null)
                    {
                        HistoryModel row = (HistoryModel)e.Row;
                        {
                            SvgImage img = svgImageCollection1[(int)row.ProjectType];
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

        private void gvRecent_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView gridView = sender as GridView;

                if (null == gridView)
                {
                    return;
                }

                // 选中项目
                HistoryModel selectModel = gridView.GetFocusedRow() as HistoryModel;

                if (null == selectModel)
                {
                    return;
                }
                else
                {
                    if (null != LoadProjectEvent)
                    {
                        LoadProjectEvent(selectModel.FilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打开工程事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucImageButton1_Click(object sender, EventArgs e)
        {
            if (null != OpenProjectEvent)
            {
                OpenProjectEvent(null, null);
            }
        }

        /// <summary>
        /// 创建工程事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucImageButton2_Click(object sender, EventArgs e)
        {
            if (null != CreateProjectEvent)
            {
                CreateProjectEvent(null, null);
            }
        }

        /// <summary>
        /// 帮助文档事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucImageButton3_Click(object sender, EventArgs e)
        {
            if (null != HelpEvent)
            {
                HelpEvent(null, null);
            }
        }
    }


}
