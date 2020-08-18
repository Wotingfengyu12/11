using Microsoft.Office.Interop.Word;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using MSWord = Microsoft.Office.Interop.Word;

namespace CQC.WordTool
{
    public class Word : IDisposable
    {
        #region 字段

        private MSWord.Application m_WordApp = null;        // Word应用程序变量
        private MSWord.Document m_WordDoc = null;           // Word文档变量
        private string m_FilePath = string.Empty;           // 文件保存路径
        private Selection m_CurrentSelection = null;        // 当前选中对象
        private MSWord.Table m_CurrentTable = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Word(string filePath, string templatePath)
        {
            try
            {
                m_FilePath = filePath;
                Init(templatePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 方法

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init(string templatePath)
        {
            try
            {

                // 由于使用的是COM 库，因此有许多变量需要用Missing.Value 代替
                Object Nothing = Missing.Value;
                object path = Path.GetFullPath(templatePath);

                try
                {
                    m_WordApp = (Application)Interaction.GetObject(null, "Word.Application");
                    IEnumerator enumerator = m_WordApp.Documents.GetEnumerator();

                    foreach (Document currentDoc in m_WordApp.Documents)
                    {

                        if (true == path.Equals(currentDoc.FullName))
                        {
                            m_WordDoc = currentDoc;
                            break;
                        }
                    }
                }
                catch
                {
                    m_WordApp = new MSWord.ApplicationClass();
                }

                if (null == m_WordDoc)
                {
                    //新建一个word对象
                    m_WordDoc = m_WordApp.Documents.Open(path, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateTable(int rowCount)
        {
            Object nothing = Missing.Value;

            m_CurrentTable = m_WordDoc.Tables.Add(m_CurrentSelection.Range, rowCount + 1, 4, ref nothing, ref nothing);
            m_CurrentTable.Borders.Enable = 1;//默认表格没有边框
                                              //给表格中添加内容
                                              //设置表头
            SetCellValue(m_CurrentTable, 1, 1, "Test title", 4, 10.5f);
            SetCellValue(m_CurrentTable, 1, 2, "Result", 4, 10.5f);
            SetCellValue(m_CurrentTable, 1, 3, "Deposition", 4, 10.5f);
            SetCellValue(m_CurrentTable, 1, 4, "Comments", 4, 10.5f);

            MoveCurrentRange(rowCount + 1);

            //**错误原因——word中表格的行列是从1开始的，而不是从0开始的。**

            //            //给表中添加信息（信息是从txt中读取的）
            //            for (int row = 2; row < tableRow + 1; row++)
            //            {
            //                for (int column = 1; column <= tableColumn; column++)
            //                {
            //                    table.Cell(row, column).Range.Font.Size = 8;//设置表格中字体大小
            //                    table.Cell(row, column).Range.Bold = 0;//设置表格中字体不加粗
            //                    table.Cell(row, column).Range.Text = tableData[row - 2, column - 1].ToString();//给表格中添加信息
            //                }
            //            }
            //            object wordLine = MSWord.WdUnits.wdLine;//换行
            //            object count = tableRow + 1;//换行的数目
            //            wordApp.Selection.MoveDown(ref wordLine, count, nothing); //向下移动count行
            //            wordApp.Selection.TypeParagraph();

            //            6.添加文字

            //object nothing = Type.Missing;
            //            object EndOfDoc = "\\endofdoc";

            //            object wordLine = MSWord.WdUnits.wdLine;
            //            object count = tableRow + 1;
            //            wordApp.Selection.MoveDown(ref wordLine, count, nothing);
            //            object what = MSWord.WdGoToItem.wdGoToBookmark;//定位到当前文档末尾
            //            wordApp.Selection.GoTo(what, nothing, nothing, EndOfDoc);
            //            wordApp.Selection.TypeParagraph();
            //            wordApp.Selection.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphLeft;
            //            wordApp.Selection.Font.Size = 20;
            //            wordApp.Selection.Font.Bold = 2;
            //            wordApp.Selection.TypeText("3.详情");
            //            wordApp.Selection.TypeParagraph();
            //            wordApp.Selection.Font.Size = 10;
            //            wordApp.Selection.Font.Bold = 0;
            //            wordApp.Selection.TypeText("实时");
            //            wordApp.Selection.TypeParagraph();

            //            7.添加表格

            //MSWord.Range range = wordDoc.Bookmarks.get_Item(ref EndOfDoc).Range;

            //            MSWord.Table subtable = wordDoc.Tables.Add(range, 2, 11, ref nothing, ref nothing);
            //            subtable.Borders.Enable = 1;//默认表格没有边框
            //            subtable.Cell(1, 1).Range.Text = "序号";
            //            subtable.Cell(1, 1).Range.Bold = 2;
            //            subtable.Cell(1, 1).Range.Font.Size = 8;
            //            subtable.Cell(1, 2).Range.Text = "区间";
            //            subtable.Cell(1, 2).Range.Bold = 2;
            //            subtable.Cell(1, 2).Range.Font.Size = 8;
            //            subtable.Cell(1, 3).Range.Text = "距离(m)";
            //            subtable.Cell(1, 3).Range.Bold = 2;
            //            subtable.Cell(1, 3).Range.Font.Size = 8;
            //            subtable.Cell(1, 4).Range.Text = "坐标";
            //            subtable.Cell(1, 4).Range.Bold = 2;
            //            subtable.Cell(1, 4).Range.Font.Size = 8;
            //            subtable.Cell(1, 5).Range.Text = "类型";
            //            subtable.Cell(1, 5).Range.Bold = 2;
            //            subtable.Cell(1, 5).Range.Font.Size = 8;
            //            subtable.Cell(1, 6).Range.Text = "水平";
            //            subtable.Cell(1, 6).Range.Bold = 2;
            //            subtable.Cell(1, 6).Range.Font.Size = 8;
            //            subtable.Cell(1, 7).Range.Text = "垂直";
            //            subtable.Cell(1, 7).Range.Bold = 2;
            //            subtable.Cell(1, 7).Range.Font.Size = 8;
            //            subtable.Cell(1, 8).Range.Text = "净空";
            //            subtable.Cell(1, 8).Range.Bold = 2;
            //            subtable.Cell(1, 8).Range.Font.Size = 8;
            //            subtable.Cell(1, 9).Range.Text = "水平";
            //            subtable.Cell(1, 9).Range.Bold = 2;
            //            subtable.Cell(1, 9).Range.Font.Size = 8;
            //            subtable.Cell(1, 10).Range.Text = "垂直";
            //            subtable.Cell(1, 10).Range.Bold = 2;
            //            subtable.Cell(1, 10).Range.Font.Size = 8;
            //            subtable.Cell(1, 11).Range.Text = "备注";
            //            subtable.Cell(1, 11).Range.Bold = 2;
            //            subtable.Cell(1, 11).Range.Font.Size = 8;

            //            for (int column = 1; column <= TableColumn; column++)
            //            {

            //                SubTable.Cell(2, column).Range.Text = LineData[row, column - 1].ToString();
            //                SubTable.Cell(2, column).Range.Font.Size = 8;
            //            }

        }

        /// <summary>
        /// 设置单元格信息
        /// </summary>
        /// <param name="table"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="text"></param>
        /// <param name="bold"></param>
        /// <param name="size"></param>
        private void SetCellValue(
            Table table, int row, int column, string text, int bold, float size)
        {
            try
            {
                table.Cell(row, column).Range.Text = text;
                table.Cell(row, column).Range.Bold = bold;
                table.Cell(row, column).Range.Font.Size = size;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 接口

        public void Save(string savePath)
        {
            try
            {

                object path = savePath;//保存为Word2003文档

                if (File.Exists((string)path))
                {
                    File.Delete((string)path);
                }
                //由于使用的是COM 库，因此有许多变量需要用Missing.Value 代替
                Object Nothing = Missing.Value;

                //WdSaveDocument为Word2003文档的保存格式(文档后缀.doc)\wdFormatDocumentDefault为Word2007的保存格式(文档后缀.docx)
                object format = MSWord.WdSaveFormat.wdFormatHTML;
                //将wordDoc 文档对象的内容保存为DOC 文档,并保存到path指定的路径
                m_WordDoc.SaveAs(ref path, ref format, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            //由于使用的是COM 库，因此有许多变量需要用Missing.Value 代替
            Object Nothing = Missing.Value;
            //关闭wordDoc文档
            m_WordDoc.Close(ref Nothing, ref Nothing, ref Nothing);
            //关闭wordApp组件对象
            m_WordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
        }

        #endregion

        public void CreateWord()
        {
            try
            {
                object path;//文件路径
                string strContent;//文件内容

                m_WordApp = new MSWord.ApplicationClass();//初始化

                //if (File.Exists((string)path))
                //{
                //    File.Delete((string)path);
                //}

                path = Path.GetFullPath(@"Resources\TestReport.docx");
                //由于使用的是COM 库，因此有许多变量需要用Missing.Value 代替
                Object Nothing = Missing.Value;
                //新建一个word对象
                m_WordDoc = m_WordApp.Documents.Open(path, ref Nothing, ref Nothing, ref Nothing, ref Nothing);


                //WdSaveDocument为Word2003文档的保存格式(文档后缀.doc)\wdFormatDocumentDefault为Word2007的保存格式(文档后缀.docx)
                object format = MSWord.WdSaveFormat.wdFormatDocument;
                object savePath = @"D:\1.doc";
                //将wordDoc 文档对象的内容保存为DOC 文档,并保存到path指定的路径
                m_WordDoc.SaveAs(ref savePath, ref format, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //关闭wordDoc文档
                m_WordDoc.Close(ref Nothing, ref Nothing, ref Nothing);
                //关闭wordApp组件对象
                m_WordApp.Quit(ref Nothing, ref Nothing, ref Nothing);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetCurrentBookmark(string bookmarkName)
        {
            try
            {
                Bookmarks bookmarks = m_WordApp.ActiveDocument.Bookmarks;
                foreach (Bookmark item in bookmarks)
                {
                    if (true == bookmarkName.Equals(item.Name))
                    {
                        item.Select();
                        m_CurrentSelection = m_WordApp.Selection;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新书签内容
        /// </summary>
        /// <param name="bookmarkName"></param>
        /// <param name="value"></param>
        public void UpdateBookmarkValue(string bookmarkName, string value)
        {
            try
            {
                Bookmarks bookmarks = m_WordApp.ActiveDocument.Bookmarks;
                foreach (Bookmark item in bookmarks)
                {
                    if (true == item.Name.Contains(bookmarkName))
                    {
                        item.Range.Text = value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteParagraph(string value, int bold, int size, int lineLevel)
        {
            try
            {
                object oMissing = Missing.Value;

                //Insert a paragraph at the beginning of the document.
                Paragraph oPara1 = SetParagraph(value, bold, size, ref oMissing);
                oPara1.OutlineLevel = (WdOutlineLevel)lineLevel;
                oPara1.Range.InsertParagraphAfter();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteParagraph(string value, int bold, int size, int lineLevel, int styleIndex)
        {
            try
            {
                object oMissing = Missing.Value;

                //Insert a paragraph at the beginning of the document.
                Paragraph oPara1 = SetParagraph(value, bold, size, ref oMissing);
                oPara1.set_Style((WdBuiltinStyle)(styleIndex));
                oPara1.OutlineLevel = (WdOutlineLevel)lineLevel;
                oPara1.Range.InsertParagraphAfter();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteParagraph(string value)
        {
            try
            {
                object oMissing = Missing.Value;

                //Insert a paragraph at the beginning of the document.
                Paragraph oPara1 = SetParagraph(value, 0, 10, ref oMissing);
                oPara1.OutlineLevel = WdOutlineLevel.wdOutlineLevelBodyText;
                oPara1.Range.InsertParagraphAfter();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Paragraph SetParagraph(string value, int bold, int size, ref object oMissing)
        {
            try
            {
                Paragraph oPara1 = m_WordDoc.Content.Paragraphs.Add(ref oMissing);
                oPara1.Range.Text = value;
                oPara1.Range.Font.Bold = bold;
                oPara1.Range.Font.Size = size;
                return oPara1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MoveCurrentRange(object count)
        {
            object nothing = Missing.Value;

            object wordLine = MSWord.WdUnits.wdLine;//换行
            m_CurrentSelection.MoveDown(ref wordLine, count, nothing); //向下移动count行
            m_CurrentSelection.Paragraphs.OutlineLevel = WdOutlineLevel.wdOutlineLevelBodyText;
            m_CurrentSelection.TypeParagraph();
        }

        public void InsertToTable(int row, string name, string state, string desc, string comments)
        {
            try
            {
                SetCellValue(m_CurrentTable, row, 1, name, 4, 10.5f);
                SetCellValue(m_CurrentTable, row, 2, state, 4, 10.5f);
                SetCellValue(m_CurrentTable, row, 3, desc, 4, 10.5f);
                SetCellValue(m_CurrentTable, row, 4, comments, 4, 10.5f);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 文档更新
        /// </summary>
        public void Update()
        {
            try
            {
                int count = m_WordDoc.TablesOfContents.Count;
                for (int i = 0; i < count; i++)
                {
                    m_WordDoc.TablesOfContents[i + 1].Update();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
