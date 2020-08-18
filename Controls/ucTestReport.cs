using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CQC.Controls.Models;
using System.IO;
using System.Xml;

namespace CQC.Controls
{
    public partial class ucTestReport : DevExpress.XtraEditors.XtraUserControl
    {

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public ucTestReport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="caseModels"></param>
        public ucTestReport(string filePath)
            : this()
        {
            LoadTestReport(filePath);
        }

        #endregion

        #region 方法

        public void LoadTestReport(string filePath)
        {
            try
            {
                webBrowser1.Url = new Uri(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


    }
}
