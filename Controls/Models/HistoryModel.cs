using CQC.ConTest;
using DevExpress.Export.Binary;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CQC.Controls.Models
{
    /// <summary>
    /// 历史文件模型
    /// </summary>
    [XmlType("HistoryModel")]
    public class HistoryModel
    {
        #region 属性

        /// <summary>
        /// 文件名称
        /// </summary>
        [XmlIgnore]
        public string ProjectName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(FilePath);
            }
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        [XmlElement]
        public string FilePath { get; set; }

        /// <summary>
        /// 文件打开时间
        /// </summary>
        [XmlElement]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// 工程类型
        /// </summary>
        [XmlAttribute]
        public ProjectType ProjectType { get; set; }

        #endregion

        #region 构造函数

        #endregion

    }

    /// <summary>
    /// 历史文件模型集合
    /// </summary>
    public class HistoryModels : List<HistoryModel>
    {
        /// <summary>
        /// 历史文件路径
        /// </summary>
        private static string HistoryFilePath = Path.Combine(
                    System.Environment.CurrentDirectory, "History.xml");

        #region 接口

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static HistoryModels Open()
        {
            try
            {
                if (false == File.Exists(HistoryFilePath))
                {
                    return new HistoryModels();
                }

                using (FileStream fileStream = new FileStream(HistoryFilePath, FileMode.Open))
                {
                    XmlSerializer xmldes = new XmlSerializer(typeof(HistoryModels));
                    return xmldes.Deserialize(fileStream) as HistoryModels;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存历史文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void Save(HistoryModel historyModel)
        {
            try
            {
                if (null == historyModel)
                {
                    return;
                }

                // 历史文件信息
                HistoryModels historyModels = HistoryModels.Open();
                // 获取文件存在的历史文件信息
                historyModels.RemoveAll(item => { return false == File.Exists(item.FilePath); });

                // 查询模型是否存在
                HistoryModel model = historyModels.Find(item => { return item.FilePath.Equals(historyModel.FilePath); });

                if (null == model)
                {
                    historyModels.Add(historyModel);
                }
                else
                {
                    model.LastModified = historyModel.LastModified;
                }

                using (MemoryStream Stream = new MemoryStream())
                using (FileStream fileStream = new FileStream(HistoryFilePath, FileMode.Create))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(HistoryModels));
                    //序列化对象
                    xml.Serialize(Stream, historyModels);

                    fileStream.Write(Stream.ToArray(), 0, Stream.ToArray().Length);
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
