using CQC.Controls;
using CQC.Controls.Models;
using CQC.WordTool;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList.Nodes;
using FieldIot.HARTDD;
using FieldIot.ProfibusDP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace CQC.ConTest
{
    public partial class frmMain : DevExpress.XtraBars.ToolbarForm.ToolbarForm
    {
        //List<Stream> fileStreams;
        string ProjectName = "No Test Project";
        TestProject tp;
        public bool bafterSel = false;
        ucResultList resultList;
        ucStartupPage startup;
        List<TestProject> recentProjects;
        BindingSource tresList;

        mainform mp;
        DPComm profibusDP;

        ucProjectSettings projectsettings;

        const int sleeptime = 000;
        public static char[] spaces;
        TestDevices devices;
        int newtestcasecount;
        //int devicecount;

        testSchedule testToExec;
        HartTestFuncs hartFunctions;
        DPTestFuncs dpFunctions;

        //StreamWriter 7;

        SynchronizationContext mSyncContext = null;
        OverlayTextPainter overlayLabel = new OverlayTextPainter();
        //OverlayImagePainter overlayButton = new OverlayImagePainter(buttonImage, hotButtonImage, OnCancelButtonClick);

        DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        public frmMain()
        {
            try
            {
                InitializeComponent();
                mp = new mainform(this);
                profibusDP = new DPComm(this.serialPort1, System.Environment.CurrentDirectory + "\\TestDD\\DP\\");
                resultList = new ucResultList();
                testToExec = new testSchedule();
                recentProjects = new List<TestProject>();
                startup = new ucStartupPage(recentProjects);
                startup.LoadProjectEvent += LoadProject;
                startup.OpenProjectEvent += iOpenFile_ItemClick;
                startup.CreateProjectEvent += iNewProject_ItemClick;
                startup.HelpEvent += iAbout_ItemClick;

                splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(Loading), true, true);
                splashScreenManager1.ClosingDelay = 500;
                spaces = new char[] { ' ', '\t', '\n' };
                hartFunctions = new HartTestFuncs();
                dpFunctions = new DPTestFuncs(profibusDP);
                tresList = new BindingSource();
                mSyncContext = SynchronizationContext.Current;
            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                //MessageBox.Show(e.Message);
            }
            //this.Text = DXperience.Demos.DemoHelper.GetFormText("Visual Studio Inspired UI");
            //Icon = DevExpress.Utils.ResourceImageHelper.CreateIconFromResourcesEx("ConTest.Resources.AppIcon.ico", typeof(frmMain).Assembly);
        }

        public frmMain(bool bSkin)
        {
            try
            {
                InitializeComponent();
                mp = new mainform(this);
                profibusDP = new DPComm(this.serialPort1, System.Environment.CurrentDirectory + "\\TestDD\\DP\\");
                resultList = new ucResultList();
                testToExec = new testSchedule();
                recentProjects = new List<TestProject>();
                startup = new ucStartupPage(recentProjects);
                startup.LoadProjectEvent += LoadProject;
                startup.OpenProjectEvent += iOpenFile_ItemClick;
                startup.CreateProjectEvent += iNewProject_ItemClick;
                startup.HelpEvent += iAbout_ItemClick;

                splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(Loading), true, true);
                splashScreenManager1.ClosingDelay = 500;
                spaces = new char[] { ' ', '\t', '\n' };
                hartFunctions = new HartTestFuncs();
                dpFunctions = new DPTestFuncs(profibusDP);
                tresList = new BindingSource();
                mSyncContext = SynchronizationContext.Current;
                if (bSkin)
                {
                    skinDropDownButtonItem1.Visibility = BarItemVisibility.Always;
                    skinPaletteDropDownButtonItem1.Visibility = BarItemVisibility.Always;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                //MessageBox.Show(e.Message);
            }
            //this.Text = DXperience.Demos.DemoHelper.GetFormText("Visual Studio Inspired UI");
            //Icon = DevExpress.Utils.ResourceImageHelper.CreateIconFromResourcesEx("ConTest.Resources.AppIcon.ico", typeof(frmMain).Assembly);
        }

        private void SaveToRecentList()
        {

        }

        private void LoadRecentList()
        {

        }

        public void initComBox()
        {
            string[] com_array = System.IO.Ports.SerialPort.GetPortNames();
            int com_number = com_array.Length;
            serialPortComboEdit.Items.Clear();
            for (int i = 0; i < com_number; i++)
            {
                serialPortComboEdit.Items.Add(com_array[i]);
                //comboBox2.Items.Add(asCOM_ListInfo[i].strHardwareID);
            }
            if (com_number > 0)
            {
                biSerCombo.EditValue = com_array[0];
            }
        }

        public returncode USART_Send(byte[] data, byte len, byte add = 0, StreamWriter sw = null)
        {
            /*
            if (serialPort1.IsOpen && mp.gsRspInfo.ucSendState != mainform.MSG_PENDING)
            {
                mp.gsRspInfo.ucSendState = mainform.MSG_PENDING;
                //rcvlen = 0;
                serialPort1.Write(data, 0, len);
            }
            else if (mp.gsRspInfo.ucSendState == mainform.MSG_PENDING)
            {
                return returncode.eCommErr;
            }
            */
            if (serialPort1.IsOpen)
            {
                mp.gsRspInfo.ucSendState = mainform.MSG_PENDING;
                //rcvlen = 0;
                serialPort1.Write(data, 0, len);
                string msgSent = mp.buildStringTypeInfo(len, data);
                if (sw != null)
                {
                    saveLogfile(sw, "Message sent to device.");
                    saveLogfile(sw, msgSent);
                }
            }
            else
            {
                if (sw != null)
                {
                    saveLogfile(sw, "Message recieved from device.");
                }
                return returncode.eSerErr;
            }
            return returncode.eOk;
        }

        public returncode RecvHartData(StreamWriter sw)
        {
            //rcvlen = 0;
            DateTime dt = DateTime.Now;
            Thread.Sleep(300); //等待100毫秒

            if (serialPort1.IsOpen)
            {
                int n = serialPort1.BytesToRead;//先记录下来，避免丢失
                byte[] buf = new byte[n];
                serialPort1.Read(buf, 0, n);//读取缓冲区数据

                for (int i = 0; i < n; i++)
                {
                    mp.rcvbuf[i + mp.rcvlen] = buf[i];
                    //               str = str + Convert.ToString(buf[i]), buf[i]) + " ";
                    //textBox1.Text = textBox1.Text + String.Format("0x{0:X2}", buf[i]) + " ";
                }
                mp.rcvlen += (byte)n;
            }

            else
            {
                return returncode.eCloseErr;
            }

            int d;
            do
            {
                Thread.Sleep(100); //等待100毫秒
                d = serialPort1.BytesToRead;//先记录下来，避免丢失
                byte[] buf = new byte[d];
                serialPort1.Read(buf, 0, d);//读取缓冲区数据

                for (int i = 0; i < d; i++)
                {
                    mp.rcvbuf[i + mp.rcvlen] = buf[i];
                    //               str = str + Convert.ToString(buf[i]), buf[i]) + " ";
                    //textBox1.Text = textBox1.Text + String.Format("0x{0:X2}", buf[i]) + " ";
                }
                mp.rcvlen += (byte)d;
            } while (d != 0);

            string msgRecv = mp.buildStringTypeInfo((byte)mp.rcvlen, mp.rcvbuf);
            saveLogfile(sw, "Message recieved from device.");
            saveLogfile(sw, msgRecv);
            //saveLogfile(sw, "Command 0 recevied.");

            if (mp.rcvlen == 0)
            {
                return returncode.eTimeOutErr;

                /*
                Thread.Sleep(100);
                if (DateTime.Now.Subtract(dt).TotalMilliseconds > 800) //如果5秒后仍然无数据返回，则视为超时
                {
                    strManufacturer = "000026";
                    strDeviceType = "0006";
                    //MessageBox.Show("无响应");
                    return comcode.eTimeOutErr;
                    //break;
                    //throw new Exception("串口无响应");
                }
                */
            }
            return returncode.eOk;
        }

        void frmMain_Load(object sender, System.EventArgs e)
        {
            //this.fileStreams = Program.CreateResourceStreams();
            //ucProjectExplorer.InitTreeView(treeView1);
            if (tp == null)
            {
                BeginInvoke(new MethodInvoker(InitDemo));
                barStaticItem1.Caption = ProjectName;
                //iNewCase.Enabled = false;
                iCloseProject.Enabled = false;
                iProjectSettings.Enabled = false;
                iStart.Enabled = false;
                siExecute.Enabled = false;
                initComBox();
            }
            else
            {
                barStaticItem1.Caption = tp.ProjectName;
            }

            ucOutput1.ScrollToBottom();

        }

        void barManager1_Merge(object sender, BarManagerMergeEventArgs eve)
        {
            try
            {
                barManager.Bars["Edit"].Merge(eve.ChildManager.Bars["Edit"]);

                if (eve.ChildManager.Bars["Edit"].ItemLinks[0].Item.Name == "undo")
                {
                    BarItemLinkCollection blc = eve.ChildManager.Bars["Edit"].ItemLinks;
                    if (siEdit.ItemLinks.Count <= 3)
                    {
                        siEdit.ItemLinks.Add(blc[0].Item, true);//undo
                        siEdit.ItemLinks.Add(blc[1].Item);//redo
                        siEdit.ItemLinks.Add(blc[2].Item, true);//cut
                        siEdit.ItemLinks.Add(blc[3].Item);//copy
                        siEdit.ItemLinks.Add(blc[4].Item);//paste
                        siEdit.ItemLinks.Add(blc[5].Item, true);//delete
                        siEdit.ItemLinks.Add(blc[9].Item);//wordwrap
                        siEdit.ItemLinks.Add(blc[6].Item);//select all
                    }
                }


                /*
                foreach(BarItemLink bil in eve.ChildManager.Bars["Edit"].ItemLinks)
                {
                    siEdit.ItemLinks.Add(bil.Item);
                }
                */



            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        void barManager1_UnMerge(object sender, BarManagerMergeEventArgs e)
        {
            barManager.Bars["Edit"].UnMerge();
            if (siEdit.ItemLinks.Count > 3)
            {
                siEdit.ItemLinks.RemoveAt(10);//select all
                siEdit.ItemLinks.RemoveAt(9);//wordwrap
                siEdit.ItemLinks.RemoveAt(8);//delete
                siEdit.ItemLinks.RemoveAt(7);//paste
                siEdit.ItemLinks.RemoveAt(6);//copy
                siEdit.ItemLinks.RemoveAt(5);//cut
                siEdit.ItemLinks.RemoveAt(4);//redo
                siEdit.ItemLinks.RemoveAt(3);//undo
            }
        }

        void InitDemo()
        {
            AddNewStartUp("Start Page");
            SplashScreenManager.HideImage(50, this);
        }

        bool AddNewDevice(string fileName, bool bFromNode = false)
        {
            tabbedView.BeginUpdate();

            foreach (BaseDocument doc in tabbedView.Documents)
            {
                if (doc.Caption == fileName)
                {
                    tabbedView.Controller.Activate(doc);
                    tabbedView.EndUpdate();
                    return false;
                }
            }
            ucDeviceProperty deviceproperty;

            if (devices[fileName] == null)
            {
                if (tp.TypeofProject == ProjectType.DP)
                {
                    DPDevice dev = new DPDevice(devices);
                    dev.devName = fileName;
                    deviceproperty = new ucDeviceProperty(dev);
                    devices[fileName] = dev;
                }
                else if (tp.TypeofProject == ProjectType.Hart)
                {
                    HartDevice dev = new HartDevice(devices);
                    dev.devName = fileName;
                    dev.DDPathRoot = System.Environment.CurrentDirectory + "\\TestDD\\HART\\";
                    deviceproperty = new ucDeviceProperty(dev);
                    devices[fileName] = dev;
                }
                else
                {
                    tabbedView.EndUpdate();
                    return false;
                }
                testProjectExplorer.AddDeviceNode(fileName);
            }
            else
            {
                if (bFromNode)
                {
                    deviceproperty = new ucDeviceProperty(devices[fileName]);
                }
                else
                {
                    tabbedView.EndUpdate();
                    return false;
                }
            }

            BaseDocument document;
            deviceproperty.Name = fileName;
            deviceproperty.Text = fileName;
            document = tabbedView.AddDocument(deviceproperty);
            document.Footer = Directory.GetCurrentDirectory();

            showsuperTips(document, fileName, deviceproperty.getDesc(devices[fileName]), (int)NodeType.devices);

            deviceproperty.DevCfgChanged += new System.EventHandler(this.device_SettingChanged);
            //projectsettings.LoadSetting();
            tabbedView.Controller.Activate(document);
            tabbedView.EndUpdate();
            return true;
        }

        void AddNewSettings(string fileName)
        {
            tabbedView.BeginUpdate();
            BaseDocument document;
            if (projectsettings == null || projectsettings.IsDisposed)
            {
                projectsettings = new ucProjectSettings(tp);
                projectsettings.Name = fileName;
                projectsettings.Text = fileName;
                document = tabbedView.AddDocument(projectsettings);
                document.Footer = Directory.GetCurrentDirectory();

                showsuperTips(document, "Project Settings", "Change the Test Project settings", (int)NodeType.settings);

                projectsettings.SettingChanged += new System.EventHandler(this.project_SettingChanged);
                //projectsettings.LoadSetting();
                tabbedView.Controller.Activate(document);
            }
            else
            {
                if (tabbedView.Documents.TryGetValue(projectsettings, out document))
                {
                    tabbedView.Controller.Activate(document);
                }
                else
                {
                    projectsettings = new ucProjectSettings(tp);
                    projectsettings.Name = fileName;
                    projectsettings.Text = fileName;
                    document = tabbedView.AddDocument(projectsettings);
                    document.Footer = Directory.GetCurrentDirectory();
                    projectsettings.SettingChanged += new System.EventHandler(this.project_SettingChanged);
                    //projectsettings.LoadSetting();
                    tabbedView.Controller.Activate(document);
                }
            }
            tabbedView.EndUpdate();
        }

        void device_SettingChanged(object sender, System.EventArgs e)
        {
            PropertyGrid pg = sender as PropertyGrid;
            ucDeviceProperty ps = pg.Parent as ucDeviceProperty;
            BaseDocument bd = null;
            if (tabbedView.Documents.TryGetValue(ps, out bd))
            {
                if (tp != null)
                {
                    tp.saved = false;
                }
                if (bd.Caption[bd.Caption.Length - 1] != '*')
                {
                    //bd.Caption += "*";
                }
            }
        }
        void project_SettingChanged(object sender, System.EventArgs e)
        {
            PropertyGrid pg = sender as PropertyGrid;
            ucProjectSettings ps = pg.Parent as ucProjectSettings;
            BaseDocument bd = null;
            if (tabbedView.Documents.TryGetValue(ps, out bd))
            {
                if (tp != null)
                {
                    tp.saved = false;
                }
                if (bd.Caption[bd.Caption.Length - 1] != '*')
                {
                    //bd.Caption += "*";
                }
            }
        }

        void DispTestCase(string fileName, string wholename = null)
        {
            tabbedView.BeginUpdate();

            foreach (BaseDocument doc in tabbedView.Documents)
            {
                if (doc.Tag != null && wholename != null && ((docDesc)doc.Tag).wholename == wholename)
                {
                    tabbedView.Controller.Activate(doc);
                    tabbedView.EndUpdate();
                    return;
                }
            }

            BaseDocument document;
            ucTextEditor testcase = new ucTextEditor();
            testcase.Name = fileName;
            testcase.Text = fileName;
            document = tabbedView.AddDocument(testcase);
            document.Footer = Directory.GetCurrentDirectory();
            testcase.LoadCode(wholename);
            docDesc desc = new docDesc();
            desc.wholename = wholename;
            desc.doctype = docType.testcase;
            desc.doccon = testcase;
            document.Tag = desc;
            showsuperTips(document, fileName, wholename, (int)NodeType.testcase);
            tabbedView.Controller.Activate(document);
            testcase.editor.TextChanged += new System.EventHandler(this.rtPad_TextChanged);
            tabbedView.EndUpdate();
        }

        private void DispTestCase(TestCaseModel caseModel)
        {
            tabbedView.BeginUpdate();

            foreach (BaseDocument doc in tabbedView.Documents)
            {
                string wholename = caseModel.FoldPath;
                if (doc.Tag != null && (doc.Tag as docDesc).wholename == wholename)
                {
                    tabbedView.Controller.Activate(doc);
                    tabbedView.EndUpdate();
                    return;
                }
            }

            BaseDocument document;
            ucTextEditor testcase = new ucTextEditor();
            caseModel.TextCntrol = testcase;
            testcase.Name = caseModel.Name;
            testcase.Text = caseModel.Name;
            document = tabbedView.AddDocument(testcase);
            document.Footer = Directory.GetCurrentDirectory();
            testcase.LoadCode(caseModel);
            document.Tag = caseModel;
            docDesc desc = new docDesc();
            desc.wholename = caseModel.FoldPath;
            desc.doctype = docType.testcase;
            desc.doccon = testcase;
            document.Tag = desc;

            showsuperTips(document, caseModel.Name, caseModel.Name, (int)NodeType.testcase);
            tabbedView.Controller.Activate(document);
            testcase.editor.TextChanged += new System.EventHandler(this.rtPad_TextChanged);
            tabbedView.EndUpdate();
        }

        void DispTestLog(string filePath, string wholename = null)
        {
            if (false == File.Exists(filePath))
            {
                ucOutput1.text += "The Case Never Be Executed";
                ucOutput1.text += "\r\n";
                return;
            }

            tabbedView.BeginUpdate();

            foreach (BaseDocument doc in tabbedView.Documents)
            {
                if (doc.Tag != null && ((docDesc)doc.Tag).wholename == wholename)
                {
                    tabbedView.Controller.Activate(doc);
                    tabbedView.EndUpdate();
                    return;
                }
            }

            BaseDocument document;
            ucTextEditor testLog = new ucTextEditor();
            testLog.Name = filePath;
            testLog.Text = wholename;
            document = tabbedView.AddDocument(testLog);
            document.Footer = Directory.GetCurrentDirectory();
            testLog.LoadCode(filePath, true, true, true);
            docDesc desc = new docDesc();
            desc.wholename = wholename;
            desc.doctype = docType.log;
            desc.doccon = testLog;
            document.Tag = desc;
            showsuperTips(document, filePath, wholename, (int)NodeType.logcase);
            tabbedView.Controller.Activate(document);
            tabbedView.EndUpdate();
        }

        void DispTestReport(string fileName, string wholename = null)
        {
            if (bTesting)
            {
                //testreport.LoadString("Test is excuting...\nThe test report cannot be displayed.");
                XtraMessageBox.Show("Test is excuting...\nThe test report cannot be displayed.", "Test report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string filePath = Path.Combine(tp.projectroot, tp.ProjectName, "TestReport.html");

            tabbedView.BeginUpdate();

            foreach (BaseDocument doc in tabbedView.Documents)
            {
                if (doc.Tag != null && ((docDesc)doc.Tag).wholename == wholename)
                {
                    ucTestReport testReport = doc.Control as ucTestReport;
                    testReport.LoadTestReport(filePath);
                    tabbedView.Controller.Activate(doc);
                    tabbedView.EndUpdate();
                    return;
                }
            }

            BaseDocument document;

            ucTestReport testreport = new ucTestReport(filePath);

            testreport.Name = fileName;
            testreport.Text = fileName;
            document = tabbedView.AddDocument(testreport);
            document.Footer = Directory.GetCurrentDirectory();

            docDesc desc = new docDesc();
            desc.wholename = wholename;
            desc.doctype = docType.report;
            desc.doccon = testreport;
            document.Tag = desc;


            showsuperTips(document, fileName, wholename, (int)NodeType.report);
            tabbedView.Controller.Activate(document);

            tabbedView.EndUpdate();
        }

        void DispCommonDoc(string fileName, string wholename = null)
        {
            tabbedView.BeginUpdate();

            foreach (BaseDocument doc in tabbedView.Documents)
            {
                if (doc.Tag != null && ((docDesc)doc.Tag).wholename == wholename)
                {
                    tabbedView.Controller.Activate(doc);
                    tabbedView.EndUpdate();
                    return;
                }
            }

            BaseDocument document;
            ucTextEditor testreport = new ucTextEditor();
            testreport.Name = fileName;
            testreport.Text = fileName;
            document = tabbedView.AddDocument(testreport);
            document.Footer = Directory.GetCurrentDirectory();
            testreport.LoadCode(wholename, false, true, true);
            docDesc desc = new docDesc();
            desc.wholename = wholename;
            desc.doctype = docType.notused;
            desc.doccon = testreport;
            document.Tag = desc;
            showsuperTips(document, fileName, wholename, (int)NodeType.other);
            tabbedView.Controller.Activate(document);

            tabbedView.EndUpdate();
        }

        void rtPad_TextChanged(object sender, System.EventArgs e)
        {
            ICSharpCode.TextEditor.TextEditorControl tec = sender as ICSharpCode.TextEditor.TextEditorControl;
            ucTextEditor te = tec.Parent as ucTextEditor;
            BaseDocument bd = null;
            if (tabbedView.Documents.TryGetValue(te, out bd))
            {
                /*
                TestCaseModel caseModel = (TestCaseModel)bd.Tag;
                ucTextEditor ute = (ucTextEditor)caseModel.TextCntrol;
                caseModel.TextValue = ute.GetTextValue();
                ute.updateBarButton();
                */
                te.updateBarButton();
                if (tp != null)
                {
                    tp.saved = false;
                }
                if (bd.Caption[bd.Caption.Length - 1] != '*')
                {
                    bd.Caption += "*";
                }
            }

        }

        void showsuperTips(BaseDocument document, string title, string desc, int imageindex)
        {
            SuperToolTip sTooltip1 = new SuperToolTip();
            // Create a tooltip item that represents a header. 
            ToolTipTitleItem titleItem1 = new ToolTipTitleItem();
            titleItem1.Text = title;
            // Create a tooltip item that represents the SuperTooltip's contents. 
            ToolTipItem item1 = new ToolTipItem();
            item1.Image = NodeImageCollection.GetImage(imageindex);
            if (desc != null && desc != "")
            {
                item1.Text = desc;
            }
            else
            {
                item1.Text = "Not saved yet";
            }
            // Add the tooltip items to the SuperTooltip. 
            sTooltip1.Items.Add(titleItem1);
            sTooltip1.Items.Add(item1);
            sTooltip1.FixedTooltipWidth = false;
            sTooltip1.MaxWidth = 3000;
            //(document as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document).Tooltip = "Project Settings";

            (document as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document).SuperTip = sTooltip1;
            (document as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document).ImageOptions.Image = item1.Image;
        }

        void AddNewResult(string fileName, Stream content = null)
        {
            tabbedView.BeginUpdate();
            resultList.Name = fileName;
            resultList.Text = fileName;
            BaseDocument document = null;
            tabbedView.Documents.TryGetValue(resultList, out document);// (.FirstOrDefault(x => x.Control.GetType() == someType);
            if (document == null)
            {
                resultList = new ucResultList(tresList);
                resultList.Name = fileName;
                resultList.Text = fileName;

                document = tabbedView.AddDocument(resultList);
                document.Footer = Directory.GetCurrentDirectory();

                showsuperTips(document, "Test Results", "Show Test Results", (int)NodeType.result);
            }
            else
            {
            }

            //resultList.LoadResult();
            //ucCodeEditor control = new ucCodeEditor();
            tabbedView.Controller.Activate(document);
            tabbedView.EndUpdate();
        }

        void AddNewStartUp(string fileName)
        {
            startup = new ucStartupPage(recentProjects);
            startup.LoadProjectEvent += LoadProject;
            startup.OpenProjectEvent += iOpenFile_ItemClick;
            startup.CreateProjectEvent += iNewProject_ItemClick;
            startup.HelpEvent += iAbout_ItemClick;
            tabbedView.BeginUpdate();
            startup.Name = fileName;
            startup.Text = fileName;
            BaseDocument document = null;
            tabbedView.Documents.TryGetValue(startup, out document);// (.FirstOrDefault(x => x.Control.GetType() == someType);
            if (document == null)
            {
                document = tabbedView.AddDocument(startup);
                document.Footer = Directory.GetCurrentDirectory();

                showsuperTips(document, "Start Page", "Start up page for testing", 14);
            }
            else
            {
            }

            startup.LoadProjectList();
            //ucCodeEditor control = new ucCodeEditor();
            tabbedView.Controller.Activate(document);
            tabbedView.EndUpdate();
        }

        void repositoryItemComboBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && eFind.EditValue != null)
            {
                repositoryItemComboBox1.Items.Add(eFind.EditValue.ToString());
            }
        }

        void iNewItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Form1 newpro = new Form1();
            //newpro.ShowDialog();

            switch (e.Item.Name)
            {
                case "iNew":
                    break;

                case "iNewCase":
                case "iAddExistingCase":
                    //if (tp != null)
                    {
                        newtestcasecount++;
                        testProjectExplorer.AddTestCaseNode(new DXMenuItem()
                        { Tag = e.Item.Tag }, null);
                        //DispTestCase("Test Case" + newtestcasecount.ToString() + ".cas");
                    }
                    break;

                case "binewHartProject":
                    if (tp != null && !tp.saved)
                    {
                        if (XtraMessageBox.Show("The Current Project is not saved, confirm to create a new one?", "New Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            return;
                        }
                    }

                    CloseProject();

                    tp = new TestProject();
                    //hartDevice = new HartDevice();
                    tp.TypeofProject = ProjectType.Hart;
                    tp.ProjectName = "New HART Test Project";
                    devices = new TestDevices(tp);
                    tp.Devices = devices;
                    barStaticItem1.Caption = tp.ProjectName;
                    iNewCase.Enabled = true;
                    iCloseProject.Enabled = true;
                    iProjectSettings.Enabled = true;
                    iStart.Enabled = true;
                    siExecute.Enabled = true;
                    iAddHartDevice.Enabled = true;
                    iAddDPDevice.Enabled = false;
                    testProjectExplorer.AddHartNodes(devices, tp.TestCaseDir, tp.ProjectName);
                    break;

                case "binewBlankProject":
                    if (tp != null && !tp.saved)
                    {
                        if (XtraMessageBox.Show("The Current Project is not saved, confirm to create a new one?", "New Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            return;
                        }
                    }

                    CloseProject();

                    tp = new TestProject();
                    tp.TypeofProject = ProjectType.Blank;
                    tp.ProjectName = "New Test Project";
                    barStaticItem1.Caption = tp.ProjectName;
                    iNewCase.Enabled = true;
                    iCloseProject.Enabled = true;
                    iProjectSettings.Enabled = true;
                    iStart.Enabled = true;
                    siExecute.Enabled = true;
                    iAddHartDevice.Enabled = false;
                    iAddDPDevice.Enabled = false;
                    testProjectExplorer.AddBlankNodes();
                    break;

                case "binewDPProject":
                    if (tp != null && !tp.saved)
                    {
                        if (XtraMessageBox.Show("The Current Project is not saved, confirm to create a new one?", "New Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            return;
                        }
                    }

                    CloseProject();

                    tp = new TestProject();
                    //dpDevice = new DPDevice();
                    tp.TypeofProject = ProjectType.DP;
                    tp.ProjectName = "New Profibus DP Test Project";
                    devices = new TestDevices(tp);
                    tp.Devices = devices;
                    barStaticItem1.Caption = tp.ProjectName;
                    iNewCase.Enabled = true;
                    iCloseProject.Enabled = true;
                    iProjectSettings.Enabled = true;
                    iStart.Enabled = true;
                    siExecute.Enabled = true;
                    iAddHartDevice.Enabled = false;
                    iAddDPDevice.Enabled = true;
                    testProjectExplorer.AddDPNodes(devices, tp.TestCaseDir, tp.ProjectName);
                    break;

                case "iAddDPDevice":
                case "iAddHartDevice":
                    {
                        testProjectExplorer.AddDeviceNode(new DXMenuItem() { Tag = e.Item.Tag }, null);
                    }
                    break;
                default:
                    break;
            }
        }

        void iAbout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //BarManager.About();
            AboutInfo ab = new AboutInfo();
            ab.StartPosition = FormStartPosition.CenterParent;
            ab.ShowDialog();
        }

        void iSolutionExplorer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel1.Show();
        }

        void iProperties_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel2.Show();
        }

        void iTaskList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel3.Show();
        }

        void iFindResults_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel4.Show();
        }

        void iOutput_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel5.Show();
        }

        void iToolbox_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //dockPanel6.Show();
        }

        void solutionExplorer_PropertiesItemClick(object sender, System.EventArgs e)
        {
            dockPanel2.Show();
        }

        void solutionExplorer_TreeViewItemDoubleClick(object sender, System.EventArgs e)
        {
            DevExpress.XtraTreeList.TreeList treeView = sender as DevExpress.XtraTreeList.TreeList;

            if (treeView.FocusedNode.Tag == null)
            {
                return;
            }

            NodeInfo ni = (NodeInfo)treeView.FocusedNode.Tag;
            switch (ni.ntype)
            {
                case NodeType.root:
                    break;

                case NodeType.testclass:
                    ;
                    break;

                case NodeType.testcase:
                    TestCaseModel caseModel = ni.SourceData as TestCaseModel;
                    DispTestCase(caseModel);
                    break;

                case NodeType.logcase:
                    DispTestLog(ni.nodename, ni.wholename);
                    //DispTestLog(ni.wholename);
                    break;

                case NodeType.result:
                    AddNewResult("Test Result");
                    break;

                case NodeType.hartdevice:
                //AddNewDevice("Hart Device");
                //break;

                case NodeType.dpdevice:
                //AddNewDevice("DP Device");
                //break;

                case NodeType.devices:
                    AddNewDevice(treeView.FocusedNode.GetDisplayText(0), true);
                    break;

                case NodeType.report:
                    DispTestReport("Test Report", tp.projectroot + tp.ProjectName + "\\TestReport.html");
                    break;

                case NodeType.settings:
                    AddNewSettings("Project Settings");
                    break;

                default:
                    break;
            }
        }

        void iSaveLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "XML files (*.xml)|*.xml";
                dlg.Title = "Save Layout";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Refresh(true);
                    barManager.SaveToXml(dlg.FileName);
                    Refresh(false);
                }
            }
        }

        void iLoadLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs eve)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "XML files (*.xml)|*.xml|All files|*.*";
                dlg.Title = "Restore Layout";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Refresh(true);
                    try
                    {
                        barManager.RestoreFromXml(dlg.FileName);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                    Refresh(false);
                }
            }
        }

        Cursor currentCursor;
        void Refresh(bool isWait)
        {
            if (isWait)
            {
                currentCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                Cursor.Current = currentCursor;
            }
            this.Refresh();
        }

        void iExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }


        delegate void SetOutputCallback(int linenum, string format, params object[] args);
        void sendOutput(int linenum, string format, params object[] args)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (this.ucOutput1.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.ucOutput1.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.ucOutput1.Disposing || this.ucOutput1.IsDisposed)
                    {
                        return;
                    }
                }
                SetOutputCallback d = new SetOutputCallback(sendOutput);
                ucOutput1.Invoke(d, new object[] { linenum, format, args });
            }
            else
            {
                ucOutput1.text += String.Format(format, args); ;
                for (int i = 0; i < linenum; i++)
                {
                    ucOutput1.text += "\r\n";
                }
            }
        }

        List<TreeListNode> classNodes;
        int numTestclass;
        int numTestcase;

        private void GetClassNodes(TreeListNodes Nodes)
        {
            foreach (TreeListNode node in Nodes)
            {
                NodeInfo ni = node.Tag as NodeInfo;
                if (ni.ntype == NodeType.testclass)
                {
                    testClass tclass = new testClass();
                    tclass.name = ni.nodename;
                    tclass.classID = ni.nodeid;
                    testToExec.Add(tclass);
                    numTestclass++;
                    classNodes.Add(node);
                }
                //// 如果当前节点下还包括子节点，就调用递归
                else if (node.Nodes.Count > 0)
                {
                    GetClassNodes(node.Nodes);
                }

            }
        }

        bool prepareTest()
        {
            testToExec = new testSchedule();
            classNodes = new List<TreeListNode>();
            numTestcase = 0;
            numTestclass = 0;
            foreach (TreeListNode tln in testProjectExplorer.TreeView.Nodes)
            {
                if (tln.Nodes.Count > 0)
                {
                    GetClassNodes(tln.Nodes);
                }
            }

            foreach (TreeListNode classtln in classNodes)
            {
                sendOutput(1, "Parsing Test Class {0}...", classtln.GetValue(0), true);
                foreach (TreeListNode tln in classtln.Nodes)
                {
                    NodeInfo ni = tln.Tag as NodeInfo;
                    if (ni.ntype == NodeType.testcase)
                    {
                        testCase tcase = new testCase(tp.TypeofProject);
                        tcase.name = ni.nodename;
                        tcase.classID = ni.parentid;
                        tcase.caseID = tln.Id;

                        if (!parseTestCase(ni.wholename, ref tcase))
                        {
                            sendOutput(1, "Test Case {0} parse failed", tcase.name);
                            //saveLogfile(sw, "Test Case {0} parse failed", tcase.name);
                            testToExec.Clear();
                            return false;
                        }
                        sendOutput(1, "Adding Test Case {0}...", tcase.name);
                        //saveLogfile(sw, "Adding Test Case {0}...", tcase.name);
                        numTestcase++;
                        testToExec[ni.parentid].Add(tcase);
                    }
                }
                sendOutput(2, "Test Class {0} parsed OK.", classtln.GetValue(0));
                //saveLogfile(sw, "Test Class {0} parsed OK.", classtln.GetValue(0));
            }

            return true;
        }

        bool parseTestCase(string caseFilename, ref testCase tc)
        {
            try
            {
                XDocument xdoc = XDocument.Load(caseFilename);
                XElement xtestcase = xdoc.Element("TestCase");
                string dev = xtestcase.Attribute("Device").Value;

                if (devices[dev] == null)
                {
                    sendOutput(2, "Invalid device.");
                    ;//log invalid device
                    return false;
                }
                else
                {
                    tc.device = devices[dev];
                }

                tc.description = xtestcase.Element("Description").Value;

                XElement testFunctions = xtestcase.Element("TestFunctions");
                IEnumerable<XElement> testFuncs = testFunctions.Elements("TestFunction");

                foreach (XElement func in testFuncs)
                {
                    TestModule tfunc = new TestModule();
                    tfunc.repeat = Convert.ToInt32(func.Attribute("Repeat").Value);
                    tfunc.name = func.Attribute("Name").Value;
                    tfunc.paraNum = Convert.ToInt32(func.Attribute("ParaNumber").Value);

                    XElement paras = func.Element("Parameters");
                    IEnumerable<XElement> funcparas = paras.Elements("Parameter");
                    foreach (XElement para in funcparas)
                    {
                        parameter par = new parameter();
                        par.dtype = (paramDataType)Enum.Parse(typeof(paramDataType), para.Attribute("DataType").Value);
                        par.name = para.Attribute("Name").Value;
                        par.setValue(para.Value);
                        tfunc.funcPara.Add(par);
                    }

                    XElement rets = func.Element("Returns");
                    tfunc.funcRes.response = (rspCode)Enum.Parse(typeof(rspCode), rets.Attribute("Response").Value);
                    IEnumerable<XElement> results = rets.Elements("Return");
                    foreach (XElement resu in results)
                    {
                        result rest = new result();
                        rest.rtype = (resultDataType)Enum.Parse(typeof(resultDataType), resu.Attribute("DataType").Value);
                        rest.name = resu.Attribute("Name").Value;
                        rest.setValue(resu.Value);
                        tfunc.funcRes.Add(rest);
                    }

                    tc.testFuncs.Add(tfunc);
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);//Log to taskList???
                sendOutput(1, "Error: " + e.Message);
                //saveLogfile(sw, "Error: " + e.Message);

                return false;
            }
            return true;
        }

        int numofcase;
        int casepassed;
        int casefailed;
        int casenotrun;

        IOverlaySplashScreenHandle loadHandle = null;

        void startTestExec()
        {
            iStart.Enabled = false;
            numofcase = 0;
            casepassed = 0;
            casefailed = 0;
            casenotrun = 0;
            tresList.Clear();
            sendOutput(1, "");

            //splashScreenManager1.Properties.ParentForm.Enabled = false;
            //SplashScreenManager.ShowDefaultWaitForm(this, true, true);
            //splashScreenManager1.ShowWaitForm();
            //splashScreenManager1.SetWaitFormCaption("Test started.");     // 标题
            //splashScreenManager1.SetWaitFormDescription("Executing Test Schedule.....");     // 信息
            //loadHandle = SplashScreenManager.ShowOverlayForm(this);
            OverlayWindowOptions op = new OverlayWindowOptions();
            overlayLabel.Text = "Executing Test Schedule.....\r\nPlease wait...";
            overlayLabel.Font = new Font("Tahoma", 10);
            //OverlayWindowCompositePainter owcp = new OverlayWindowCompositePainter(overlayLabel, overlayLabel, overlayLabel);
            LineAnimationParams lineAnimationParams = new LineAnimationParams(10, 2, 3);
            loadHandle = SplashScreenManager.ShowOverlayForm(this, true, true, null, null, 100, customPainter : new OverlayWindowCompositePainter(overlayLabel), animationType : WaitAnimationType.Line, lineAnimationParameters : new LineAnimationParams(10, 8, 3));

            Thread.Sleep(sleeptime);
        }

        void endTestExec()
        {
            //splashScreenManager1.SetWaitFormCaption("Test finished.");     // 标题
            //splashScreenManager1.SetWaitFormDescription("Gernerating test report....");     // 信息
            Thread.Sleep(sleeptime);
            //splashScreenManager1.CloseWaitForm();

            SplashScreenManager.CloseOverlayForm(loadHandle);
            sendOutput(2, "{0} cases executed, {1} PASSED, {2} FAILED, {3} NOTRUN", numofcase, casepassed, casefailed, casenotrun);
            sendOutput(2, "========== Test Ended ==========");

            bTesting = false;

            //splashScreenManager1.Properties.ParentForm.Enabled = true;
            //iStart.Enabled = true;
        }

        delegate void setTresListCallback(TestResult tr);
        void addtoTresList(TestResult tr)
        {

            if (this.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.Disposing || this.IsDisposed)
                    {
                        return;
                    }
                }
                setTresListCallback d = new setTresListCallback(addtoTresList);
                Invoke(d, tr);
            }
            else
            {
                tresList.Add(tr);
            }
        }

        bool bTesting;

        void executeTestSchedule(object callBackEvent)
        {
            try
            {

                bTesting = true;
                //string reportfile = tp.projectroot + tp.ProjectName + "\\TestReport.rpt";
                //FileStream fs = new FileStream(reportfile, FileMode.Create);
                //swReport = new StreamWriter(fs);
                setBarItemProperty(iStart, barItemProperty.enabled, false);
                sendOutput(2, "========== Test Started by {0} From {1} ==========", tp.Tester, tp.Organization);

                //save first?
                sendOutput(2, "---------- Preparing Test Cases... ----------");
                if (prepareTest())
                {
                    sendOutput(2, "Test Case Loaded successed.");
                    sendOutput(2, "---------- {0} Test classes, {1} Test cases ----------", numTestclass, numTestcase);
                }
                else
                {
                    sendOutput(1, "Test Case Loaded failed, test Aborted.");
                    sendOutput(1, "Check the error list for detail.");
                    sendOutput(2, "========== Test ended with errors ==========");
                    setBarItemProperty(iStart, barItemProperty.enabled, true);

                    bTesting = false;
                    return;
                }

                sendOutput(2, "---------- Performing Test... ----------");

                try
                {
                    foreach (testClass tcl in testToExec)
                    {
                        sendOutput(2, "Excuting test cases in test class {0}...", tcl.name);
                        foreach (testCase tc in tcl)
                        {
                            numofcase++;
                            if (tp.TypeofProject == ProjectType.Hart)
                            {
                                executeHartTestcase(tc);
                            }
                            else
                            {
                                executeDPTestcase(tc);
                            }
                        }
                        sendOutput(2, "---------- Test Class {0} finished---------- ", tcl.name);
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                    sendOutput(1, "Error: " + e.Message);

                    casefailed++;
                }
                Thread.Sleep(1000);

                // 保存测试日志数据
                SaveTestLog(ucOutput1.text);
                if (tp.TestReport)
                {
                    SaveTestReport(testToExec);
                }

                sendOutput(2, "---------- Test reported created ----------");
                setBarItemProperty(iStart, barItemProperty.enabled, true);

            }
            finally
            {
                if (null != callBackEvent)
                {
                    Action action = callBackEvent as Action;
                    action();
                }
            }
        }

        /// <summary>
        /// 保存测试Log数据
        /// </summary>
        private void SaveTestLog(string value)
        {
            try
            {
                string LogFilePath = tp.projectroot + tp.ProjectName + "\\TestLog.tlg";
                using (FileStream fileStream = new FileStream(LogFilePath, FileMode.Create, FileAccess.Write))
                {
                    StreamWriter streamWriter = new StreamWriter(fileStream);
                    streamWriter.Write(value);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ConvertTestResultToString(TestResult testresult)
        {
            try
            {
                StringBuilder strBuilder = new StringBuilder();

                if (null != testresult && null != testresult.Errors)
                {
                    foreach (var errorValue in testresult.Errors)
                    {
                        strBuilder.AppendLine(errorValue);
                    }
                }

                return strBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region SaveReport

        /// <summary>
        /// 保存测试Log数据
        /// </summary>
        private void SaveTestReport(List<testClass> results)
        {
            try
            {
                string filePath = Path.Combine(tp.projectroot, tp.ProjectName, "TestReport.html");

                string templatePath = @"Resources\TestReport.docx";
                if (ProjectType.DP == tp.TypeofProject)
                {
                    templatePath = @"Resources\TestReport_DP.docx";
                }
                else
                {
                    templatePath = @"Resources\TestReport_Hart.docx";
                }

                using (Word word = new Word(filePath, templatePath))
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.AppendLine(string.Format("   Name:     {0}", tp.Tester));
                    strBuilder.AppendLine(string.Format("   Company:     {0}", tp.Organization));
                    strBuilder.AppendLine(string.Format("   Address:       {0}", tp.Address));
                    strBuilder.AppendLine(string.Format("   Email:          {0}", tp.Email));
                    strBuilder.AppendLine(string.Format("   Phone:         {0}", tp.Phone));
                    word.UpdateBookmarkValue("Contact_Information", strBuilder.ToString());

                    SetDeviceInfo(word, results);

                    word.UpdateBookmarkValue("TestOrganization", tp.Organization);
                    word.UpdateBookmarkValue("TestDate", DateTime.Now.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("en-GB")));
                    word.UpdateBookmarkValue("Org_Address", tp.Address);
                    word.SetCurrentBookmark("TestClass");

                    foreach (testClass item in results)
                    {
                        SaveTestClass(word, item);
                    }
                    word.Update();

                    word.Save(filePath);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                string msg = ex.Message;
            }
        }

        /// <summary>
        /// 设置设备信息
        /// </summary>
        /// <param name="testClassList"></param>
        /// <returns></returns>
        private void SetDeviceInfo(Word word, List<testClass> testClassList)
        {
            try
            {
                if (null == word || null == testClassList)
                {
                    return;
                }

                testClass testClass = testClassList.Find(testCls => { return testCls.Count > 0; });

                int count = 0;

                testClassList.ForEach(item => { count += item.Count; });

                testCase testCase = testClass.First();

                if (true == testCase.device is HartDevice)
                {
                    HartDevice device = testCase.device as HartDevice;

                    word.UpdateBookmarkValue("DeviceName", device.Model);
                    word.UpdateBookmarkValue("TypeofDevice", device.TypeofDevice);

                    word.UpdateBookmarkValue("BeginTestingDate", tp.BeginTestingDate);
                    word.UpdateBookmarkValue("ExecutiveDate", tp.ExecutiveDate);
                    word.UpdateBookmarkValue("DocName", tp.DocName);

                    word.UpdateBookmarkValue("DeviceRevision", device.DeviceRevision);
                    word.UpdateBookmarkValue("HardwareRevision", device.DDRevision);
                    word.UpdateBookmarkValue("ProtocolRevision", device.ProtocolRevision);
                    word.UpdateBookmarkValue("SoftwareRevision", device.SoftwareRevision);
                    word.UpdateBookmarkValue("TestCount", count.ToString());

                    word.UpdateBookmarkValue("DeviceType", "0x" + device.DeviceType.ToUpper());

                    StringBuilder strBuilder = new StringBuilder();

                    strBuilder.AppendLine(string.Format(" Manufacturer Name:                                 {0}", tp.Organization));
                    strBuilder.AppendLine(string.Format("   Model Name(s):                                        {0}", device.Model));
                    strBuilder.AppendLine(string.Format("   Manufacture ID Code(HEX):                   {0}", "0x" + device.manufID.ToUpper()));
                    strBuilder.AppendLine(string.Format("   Expanded Device Type Code(HEX):       {0}", "0x" + device.DeviceType.ToUpper()));
                    strBuilder.AppendLine(string.Format("   Device Profile(HEX):                                {0}", device.SlaveAddr));
                    strBuilder.AppendLine(string.Format("   Device Revision:                                       {0}", device.DeviceRevision));
                    strBuilder.AppendLine(string.Format("   Hardware Revision:                                  {0}", device.DDRevision));
                    strBuilder.AppendLine(string.Format("   Software Revision:                                    {0}", device.SoftwareRevision));
                    strBuilder.AppendLine(string.Format("   HART Protocol Revision:  {0}", device.ProtocolRevision));
                    strBuilder.AppendLine(string.Format("   Burst Mode Support: {0}", "NO"));
                    strBuilder.AppendLine(string.Format("   Physical Layers Supported:                      {0}", "FSK"));
                    strBuilder.AppendLine(string.Format("   FSK Physical Device Category: {0}", "4 - wire high - impedance transmitte"));
                    word.UpdateBookmarkValue("DUT_Identification", strBuilder.ToString());
                }
                else if (true == testCase.device is DPDevice)
                {
                    DPDevice device = testCase.device as DPDevice;
                    word.UpdateBookmarkValue("DeviceName", device.devName);
                    word.UpdateBookmarkValue("DeviceType", "0x" + device.IdentNumber.ToUpper());

                    word.UpdateBookmarkValue("DeviceName", device.Model);
                    word.UpdateBookmarkValue("TypeofDevice", device.TypeofDevice);

                    word.UpdateBookmarkValue("BeginTestingDate", tp.BeginTestingDate);
                    word.UpdateBookmarkValue("ExecutiveDate", tp.ExecutiveDate);
                    word.UpdateBookmarkValue("DocName", tp.DocName);

                    word.UpdateBookmarkValue("DeviceRevision", device.DeviceRevision);
                    word.UpdateBookmarkValue("HardwareRevision", device.DDRevision);
                    word.UpdateBookmarkValue("ProtocolRevision", device.ProtocolRevision);
                    word.UpdateBookmarkValue("SoftwareRevision", device.SoftwareRevision);
                    word.UpdateBookmarkValue("TestCount", count.ToString());

                    word.UpdateBookmarkValue("DeviceType", "0x" + device.IdentNumber.ToUpper());

                    StringBuilder strBuilder = new StringBuilder();

                    strBuilder.AppendLine(string.Format(" Manufacturer Name:                                 {0}", tp.Organization));
                    strBuilder.AppendLine(string.Format("   Model Name(s):                                        {0}", device.Model));
                    //strBuilder.AppendLine(string.Format("   Manufacture ID Code(HEX):                   {0}", "0x" + device.manufID.ToUpper()));
                    strBuilder.AppendLine(string.Format("   Expanded Device Type Code(HEX):       {0}", "0x" + device.IdentNumber.ToUpper()));
                    strBuilder.AppendLine(string.Format("   Device Profile(HEX):                                {0}", device.SlaveAddr));
                    strBuilder.AppendLine(string.Format("   Device Revision:                                       {0}", device.DeviceRevision));
                    strBuilder.AppendLine(string.Format("   Hardware Revision:                                  {0}", device.DDRevision));
                    strBuilder.AppendLine(string.Format("   Software Revision:                                    {0}", device.SoftwareRevision));
                    strBuilder.AppendLine(string.Format("   PROFIBUS DP Protocol Revision:  {0}", device.ProtocolRevision));
                    strBuilder.AppendLine(string.Format("   Burst Mode Support: {0}", "NO"));
                    strBuilder.AppendLine(string.Format("   Physical Layers Supported:                      {0}", "FSK"));
                    strBuilder.AppendLine(string.Format("   FSK Physical Device Category: {0}", "4 - wire high - impedance transmitte"));
                    word.UpdateBookmarkValue("DUT_Identification", strBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void SaveTestClass(Word word, testClass source)
        {
            try
            {
                word.WriteParagraph(source.name, 1, 12, 2, -3);
                word.MoveCurrentRange(1);
                word.WriteParagraph("DUT Identification ", 1, 11, 3);
                word.WriteParagraph("Manufacturer Name:                                Model Name(s): ");
                word.WriteParagraph("Manufacture ID Code (HEX):                        Expanded Device Type code : ");
                word.WriteParagraph("Device ID(HEX):");
                word.MoveCurrentRange(5);

                word.WriteParagraph("Test Result Summary", 1, 11, 3);
                word.MoveCurrentRange(1);

                word.CreateTable(source.Count);

                int index = 2;
                foreach (testCase item in source)
                {
                    TestResult testresult = item.getResult();

                    StringBuilder strBuilder = new StringBuilder();
                    foreach (var errorValue in testresult.Errors)
                    {
                        strBuilder.AppendLine(errorValue);
                    }
                    word.InsertToTable(index++, item.name, testresult.Result.ToString(), testresult.Comment, strBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        enum barItemProperty
        {
            unknown,
            enabled,
            visible,
            text,
            caption,
            description
        }

        delegate void setBarItemPropertyCallback(BarItem cont, barItemProperty prop, object value);
        void setBarItemProperty(BarItem cont, barItemProperty prop, object value)
        {

            if (this.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.Disposing || this.IsDisposed)
                    {
                        return;
                    }
                }
                setBarItemPropertyCallback d = new setBarItemPropertyCallback(setBarItemProperty);
                Invoke(d, new object[] { cont, prop, value });
            }
            else
            {
                switch (prop)
                {
                    case barItemProperty.caption:
                        cont.Caption = (string)value;
                        break;

                    case barItemProperty.description:
                        cont.Description = (string)value;
                        break;

                    case barItemProperty.text:
                        cont.Caption = (string)value;
                        break;

                    case barItemProperty.enabled:
                        cont.Enabled = (bool)value;
                        break;

                    case barItemProperty.visible:
                        cont.Visibility = (BarItemVisibility)value;
                        break;

                    default:
                        break;
                }
            }
        }

        private void iStart_ItemClick(object sender, ItemClickEventArgs eve)
        {
            foreach (BaseDocument doc in tabbedView.Documents)
            {
                //if (doc.Tag != null && ((docDesc)doc.Tag).wholename == tp.projectroot + tp.ProjectName + "\\TestReport.rpt")
                //{
                //    //ucTextEditor testreport = ((docDesc)doc.Tag).doccon as ucTextEditor;
                //    //testreport.LoadCode(tp.projectroot + tp.ProjectName + "\\TestReport.rpt", true);
                //    //tabbedView.Controller.Activate(doc);
                //    tabbedView.Controller.Close(doc);
                //    break;
                //}
            }

            startTestExec();

            Thread th = new Thread(executeTestSchedule);
            th.Start(new Action(endTestExec));

            /*
            foreach (BaseDocument doc in tabbedView.Documents)
            {
                if (doc.Tag != null && ((docDesc)doc.Tag).wholename == tp.projectroot + tp.ProjectName + "\\TestReport.rpt")
                {
                    ucTextEditor testreport = ((docDesc)doc.Tag).doccon as ucTextEditor;
                    testreport.LoadCode(tp.projectroot + tp.ProjectName + "\\TestReport.rpt", true);
                    //tabbedView.Controller.Activate(doc);
                    return;
                }
            }
            */

        }

        bool initDevice(object dev, StreamWriter sw)
        {
            bool bDevOk = false;
            if (tp.TypeofProject == ProjectType.Hart)
            {
                HartDevice hdev = (dev as HartDevice);
                mp.hcfg.bOffline = hdev.Offline;
                mp.strManufacturer = hdev.manufID;
                mp.strDeviceType = hdev.DeviceType;
                mp.strDeviceRevision = hdev.DeviceRevision;
                mp.strDDRevision = hdev.DDRevision;
                mp.hcfg.ddRoot = hdev.DDPathRoot;

                rtNewDev b = mp.newDevice(sw);

                if (b == rtNewDev.eSuc)
                {
                    mp.hartDev.instantiate(0);
                    sendOutput(2, "Device {0} created successed", hdev.devName);
                    saveLogfile(sw, "Device {0} created successed", hdev.devName);
                    bDevOk = true;
                    //return true;
                }
                else
                {
                    bDevOk = false;
                    if (b == rtNewDev.eNoDD)
                    {
                        sendOutput(2, "No DD File for Device {0}", hdev.devName);
                        saveLogfile(sw, "No DD File for Device {0}", hdev.devName);
                    }
                    if (b == rtNewDev.eDDFailed)
                    {
                        sendOutput(2, "No DD File for Device {0}", hdev.devName);
                        saveLogfile(sw, "No DD File for Device {0}", hdev.devName);
                    }
                    if (b == rtNewDev.eCommTimeout)
                    {
                        sendOutput(2, "Device {0} connnect timeout", hdev.devName);
                        saveLogfile(sw, "Device {0} connnect timeout", hdev.devName);
                    }
                }
            }
            else if (tp.TypeofProject == ProjectType.DP)
            {
                profibusDP.clearDev();
                //profibusDP = new DPComm();
                //return true;
                DPDevice ddev = dev as DPDevice;
                CDEV_CFG_INFO cDevCfgInfoTemp = new CDEV_CFG_INFO();

                //str1[0]：ucAddrS
                cDevCfgInfoTemp.ucAddrS = (byte)ddev.SlaveAddr;

                //str1[1]：ucIdentH
                cDevCfgInfoTemp.ucIdentH = Convert.ToByte(ddev.IdentNumber.Substring(0, 2), 16);// byte.Parse(str1[1]);

                //str1[2]：ucIdentL
                cDevCfgInfoTemp.ucIdentL = Convert.ToByte(ddev.IdentNumber.Substring(2, 2), 16); //byte.Parse(str1[2]);

                //str1[3]：ucMinTsdr
                cDevCfgInfoTemp.ucMinTsdr = ddev.MinTsdr;

                //str1[4]：ucInputLen
                cDevCfgInfoTemp.ucInputLen = ddev.InputLen;

                //str1[5]：ucOutputLen
                cDevCfgInfoTemp.ucOutputLen = ddev.OutputLen;

                //str1[6]：ucCfgDataLen
                cDevCfgInfoTemp.ucCfgDataLen = ddev.CfgDataLen;

                //str1[7]...：ucCfgData
                for (int j = 0; j < cDevCfgInfoTemp.ucCfgDataLen; j++)
                {
                    cDevCfgInfoTemp.aucCfgData[j] = Convert.ToByte(ddev.CfgData.Substring(j * 2, 2), 16);
                }

                //str1[]：ucUserExtPrmDataLen
                cDevCfgInfoTemp.ucUserExtPrmDataLen = ddev.UserExtPrmDataLen;
                if (cDevCfgInfoTemp.ucUserExtPrmDataLen > 0)
                {
                    //str1[7]...：aucUserExtPrmData
                    for (int j = 0; j < cDevCfgInfoTemp.ucUserExtPrmDataLen; j++)
                    {
                        cDevCfgInfoTemp.aucUserExtPrmData[j] = Convert.ToByte(ddev.UserPrmData, 16);
                    }
                }

                cDevCfgInfoTemp.gsdFileName = ddev.gsdFile;

                profibusDP.addDevice(cDevCfgInfoTemp);

                CFRAME_PARSE_NODE cframe = new CFRAME_PARSE_NODE();

                if (ddev.Offline)
                {
                    sendOutput(2, "Device {0} created successed", ddev.devName);
                    saveLogfile(sw, "Device {0} created successed", ddev.devName);
                    bDevOk = true;
                }
                else if (profibusDP.diagDevice(0, ref cframe))
                {
                    sendOutput(2, "Device {0} created successed", ddev.devName);
                    saveLogfile(sw, "Device {0} created successed", ddev.devName);
                    bDevOk = true;
                }
                else
                {
                    sendOutput(2, "Device {0} created failed", ddev.devName);
                    saveLogfile(sw, "Device {0} created failed", ddev.devName);
                    bDevOk = false;
                }
            }
            //return false;
            return bDevOk;
        }

        void saveLogfile(StreamWriter sw, string format, params object[] args)
        {
            sw.WriteLine(String.Format(format, args));
        }

        /*
       void saveLogfile(string format, params object[] args)
        {
            sw.WriteLine(String.Format(format, args));
        }
        */

        public results cmdRes;

        public void procRcvHartData(returncode rc, int trannum, uint cmdnum, cmdOperationType_t operation)
        {
            cmdRes = new results();
            if (rc != returncode.eOk)
            {
                Log(String.Format(Resource.CmdRcvErr, cmdnum, trannum, Resource.ErrorTimeout), LogType.Error);
                cmdRes.response = rspCode.negitive;
                cmdRes.resDesc = String.Format(Resource.CmdRcvErr, cmdnum, trannum, Resource.ErrorTimeout);
                return;
            }

            if (mp.rcvlen != 0)
            {

                string msgRcv = mp.buildStringTypeInfo(mp.rcvlen, mp.rcvbuf);

                for (int i = 0; i < mp.rcvlen; i++)
                {
                    mp.HartRcvState(mp.rcvbuf[i]);
                }
                if (mp.gsRspInfo.aucRspCode[0] != 0)
                {

                    CCmdList cl = (CCmdList)mp.hartDev.getListPtr(itemType_t.iT_Command);
                    CDDLCmd cm = cl.getCmdByNumber((int)cmdnum);
                    hCRespCodeList respCodeList;// treat as a vector of <hCrespCode>
                    respCodeList = new hCRespCodeList();
                    cm.getRespCodes(ref respCodeList);

                    string errinfo = "";

                    foreach (hCrespCode hcc in respCodeList)
                    {
                        if (hcc.val == mp.gsRspInfo.aucRspCode[0])
                        {
                            errinfo = hcc.descS;
                        }
                    }
                    Log(String.Format(Resource.CmdRcvErr, cmdnum, trannum, errinfo), LogType.Error);
                    cmdRes.response = rspCode.negitive;
                    cmdRes.resDesc = String.Format(Resource.CmdRcvErr, cmdnum, trannum, errinfo);
                }
                cmdRes.response = rspCode.positive;
                mp.rcvlen = 0;
            }
            else
            {
                string msgRcv = mp.buildStringTypeInfo(mp.rcvlen, mp.rcvbuf);
                cmdRes.response = rspCode.negitive;
                cmdRes.resDesc = String.Format(Resource.CmdRcvErr, cmdnum, trannum, Resource.ErrorTimeout);
            }
        }

        void executeHartTestcase(testCase tc)
        {
            string logfile = tp.projectroot + tp.ProjectName + "\\TestClasses\\" + testToExec[tc.classID].name + "\\" + Path.GetFileNameWithoutExtension(tc.name) + ".tlg";
            FileStream fs = new FileStream(logfile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            try
            {
                TestResult tr = tc.caseresult;
                char[] enter = { '\r', '\n' };
                tr.Description = tc.description.TrimStart(enter).TrimEnd(enter);
                tr.TestClass = testToExec[tc.classID].name;
                tr.TestCase = tc.name;
                tr.Time = DateTime.Now.ToString();
                tr.Comment = "Hart Test case";

                sendOutput(2, "---------- Preparing Test Device {0} ----------", (tc.device as HartDevice).devName);
                saveLogfile(sw, "---------- Preparing Test Device {0} ----------", (tc.device as HartDevice).devName);
                bool bDevOk = initDevice(tc.device, sw);
                if (bDevOk)
                {
                    hartFunctions.HartDev = mp.hartDev;
                }
                else
                {
                    sendOutput(2, "Test Device {0} is not available.", (tc.device as HartDevice).devName);
                    saveLogfile(sw, "Test Device {0} is not available.", (tc.device as HartDevice).devName);
                    tr.Result = TestRes.FAILED;
                    casefailed++;
                    tr.Errors.Add(string.Format("Test Device {0} is not available.", (tc.device as HartDevice).devName));

                    sendOutput(2, "Test Case {0} FAILED", tc.name);
                    saveLogfile(sw, "Test Case {0} FAILED", tc.name);
                    addtoTresList(tr);
                    sendOutput(2, "---------- Test Case {0} finished ----------", tc.name);
                    saveLogfile(sw, "---------- Test Case {0} finished ----------", tc.name);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    return;
                }
                sendOutput(2, "---------- Test Case {0} started ----------", tc.name);
                saveLogfile(sw, "---------- Test Case {0} started ----------", tc.name);

                parameters pars;
                results ret;
                bool bOk = true;
                hartFunctions.setLogsw(sw);
                foreach (TestModule tf in tc.testFuncs)
                {
                    pars = tf.funcPara;
                    if (hartFunctions[tf.name] != null)
                    {
                        hartFunctions[tf.name].func(pars, out ret);
                    }
                    else
                    {
                        sendOutput(1, "Function not executed, function " + tf.name + " found");
                        saveLogfile(sw, "Function not executed, function " + tf.name + " found");
                        bOk = false;
                        continue;
                    }
                    sendOutput(1, "Function {0} executed.", tf.name);
                    saveLogfile(sw, "Function {0} executed.", tf.name);
                    if (ret.response != tf.funcRes.response)
                    {
                        //log here
                        sendOutput(1, "Response is not expected.");
                        saveLogfile(sw, "Response is not expected.");
                        tr.Errors.Add("Response is not expected.");

                        bOk = false;
                        continue;
                    }
                    else
                    {
                        sendOutput(1, "Response is expected {0}.", ret.response.ToString());
                        saveLogfile(sw, "Response is expected {0}.", ret.response.ToString());
                        foreach (result relexpt in tf.funcRes)
                        {
                            result rel = ret[relexpt.name];
                            if (rel == null)
                            {
                                sendOutput(1, "Result {0} is invalid, please check Test Case.", relexpt.name);
                                saveLogfile(sw, "Result {0} is invalid, please check Test Case.", relexpt.name);
                                tr.Errors.Add(string.Format("Result {0} is invalid, please check Test Case.", relexpt.name));
                                bOk = false;
                            }
                            else
                            {
                                if (!rel.value.Equals(relexpt.value))
                                {
                                    if (relexpt.rtype == resultDataType.floatpoint)
                                    {
                                        if (Convert.ToSingle(rel.value) < (Convert.ToSingle(relexpt.value) + 0.01)
                                            && Convert.ToSingle(rel.value) > (Convert.ToSingle(relexpt.value) - 0.01))
                                        {
                                            sendOutput(1, "Result {0} is expected value {1}.", rel.name, relexpt.value);
                                            saveLogfile(sw, "Result {0} is expected value {1}.", rel.name, relexpt.value);
                                        }
                                        else
                                        {
                                            sendOutput(1, "Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value);
                                            saveLogfile(sw, "Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value);

                                            tr.Errors.Add(string.Format("Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value));
                                            bOk = false;
                                        }
                                    }
                                    else
                                    {
                                        sendOutput(1, "Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value);
                                        saveLogfile(sw, "Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value);

                                        tr.Errors.Add(string.Format("Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value));
                                        bOk = false;
                                    }
                                }
                                else
                                {
                                    sendOutput(1, "Result {0} is expected value {1}.", rel.name, relexpt.value);
                                    saveLogfile(sw, "Result {0} is expected value {1}.", rel.name, relexpt.value);
                                }
                            }
                        }
                    }
                }
                if (bOk)
                {
                    tr.Result = TestRes.PASSED;
                    casepassed++;
                    sendOutput(2, "Test Case {0} PASSED", tc.name);
                    saveLogfile(sw, "Test Case {0} PASSED", tc.name);
                }
                else
                {
                    tr.Result = TestRes.FAILED;
                    casefailed++;
                    sendOutput(2, "Test Case {0} FAILED", tc.name);
                    saveLogfile(sw, "Test Case {0} FAILED", tc.name);
                }
                addtoTresList(tr);
                sendOutput(2, "---------- Test Case {0} finished ----------", tc.name);
                saveLogfile(sw, "---------- Test Case {0} finished ----------", tc.name);
                sw.Flush();
                sw.Close();
                fs.Close();
            }

            catch (Exception exp)
            {
                //MessageBox.Show(exp.ToString());
                sendOutput(2, "Error:" + exp.Message);
                saveLogfile(sw, "Error:" + exp.Message);
                casefailed++;
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }

        void executeDPTestcase(testCase tc)
        {
            string logfile = tp.projectroot + tp.ProjectName + "\\TestClasses\\" + testToExec[tc.classID].name + "\\" + Path.GetFileNameWithoutExtension(tc.name) + ".tlg";
            FileStream fs = new FileStream(logfile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            try
            {
                TestResult tr = tc.caseresult;
                char[] enter = { '\r', '\n' };
                tr.Description = tc.description.TrimStart(enter).TrimEnd(enter);
                tr.TestClass = testToExec[tc.classID].name;
                tr.TestCase = tc.name;
                tr.Time = DateTime.Now.ToString();
                tr.Comment = "Profibus DP Test case";

                sendOutput(2, "---------- Preparing Test Device {0} ----------", (tc.device as DPDevice).devName);
                saveLogfile(sw, "---------- Preparing Test Device {0} ----------", (tc.device as DPDevice).devName);
                bool bDevOk = initDevice(tc.device, sw);
                if (bDevOk)
                {
                    dpFunctions.DPDev = profibusDP;
                    sendOutput(2, "Test Device {0} is initialized.", (tc.device as DPDevice).devName);
                    saveLogfile(sw, "Test Device {0} is initialized.", (tc.device as DPDevice).devName);
                }
                else
                {
                    sendOutput(2, "Test Device {0} is not available.", (tc.device as DPDevice).devName);
                    saveLogfile(sw, "Test Device {0} is not available.", (tc.device as DPDevice).devName);
                    tr.Result = TestRes.FAILED;
                    tr.Errors.Add(string.Format("Test Device {0} is not available.", (tc.device as DPDevice).devName));

                    casefailed++;
                    sendOutput(2, "Test Case {0} FAILED", tc.name);
                    saveLogfile(sw, "Test Case {0} FAILED", tc.name);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    return;
                }
                sendOutput(2, "---------- Test Case {0} started ----------", tc.name);
                saveLogfile(sw, "---------- Test Case {0} started ----------", tc.name);
                parameters pars;
                results ret;
                bool bOk = true;
                dpFunctions.setLogsw(sw);
                foreach (TestModule tf in tc.testFuncs)
                {
                    pars = tf.funcPara;
                    if (dpFunctions[tf.name] != null)
                    {
                        dpFunctions[tf.name].func(pars, out ret);
                        sendOutput(1, "Function {0} executed.", tf.name);
                        saveLogfile(sw, "Function {0} executed.", tf.name);
                        if (ret.response != tf.funcRes.response)
                        {
                            //log here
                            sendOutput(1, "Response is not expected.");
                            saveLogfile(sw, "Response is not expected.");
                            bOk = false;
                            tr.Errors.Add("Response is not expected.");

                            continue;
                        }
                        else
                        {
                            sendOutput(1, "Response is expected {0}.", ret.response.ToString());
                            saveLogfile(sw, "Response is expected {0}.", ret.response.ToString());
                            foreach (result relexpt in tf.funcRes)
                            {
                                result rel = ret[relexpt.name];
                                if (rel == null)
                                {
                                    sendOutput(1, "Result {0} is invalid, please check Test Case.", relexpt.name);
                                    saveLogfile(sw, "Result {0} is invalid, please check Test Case.", relexpt.name);
                                    tr.Errors.Add(string.Format("Result {0} is invalid, please check Test Case.", relexpt.name));

                                    bOk = false;
                                }
                                else
                                {
                                    if (!rel.value.Equals(relexpt.value))
                                    {
                                        if (relexpt.rtype == resultDataType.floatpoint)
                                        {
                                            if (Convert.ToSingle(rel.value) < (Convert.ToSingle(relexpt.value) + 0.01)
                                                && Convert.ToSingle(rel.value) > (Convert.ToSingle(relexpt.value) - 0.01))
                                            {
                                                sendOutput(1, "Result {0} is expected value {1}.", rel.name, relexpt.value);
                                                saveLogfile(sw, "Result {0} is expected value {1}.", rel.name, relexpt.value);
                                            }
                                            else
                                            {
                                                sendOutput(1, "Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value);
                                                saveLogfile(sw, "Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value);
                                                tr.Errors.Add(string.Format("Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value));

                                                bOk = false;
                                            }
                                        }
                                        else
                                        {
                                            sendOutput(1, "Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value);
                                            saveLogfile(sw, "Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value);
                                            tr.Errors.Add(string.Format("Result {0} is {1}, is not expected value {2}.", rel.name, rel.value, relexpt.value));
                                            bOk = false;
                                        }
                                    }
                                    else
                                    {
                                        sendOutput(1, "Result {0} is expected value {1}.", rel.name, relexpt.value);
                                        saveLogfile(sw, "Result {0} is expected value {1}.", rel.name, relexpt.value);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        sendOutput(1, "Test function {0} is not found.", tf.name);
                        saveLogfile(sw, "Test function {0} is not found.", tf.name);
                        tr.Errors.Add(string.Format("Test function {0} is not found.", tf.name));
                        bOk = false;
                        continue;
                    }
                }
                if (bOk)
                {
                    tr.Result = TestRes.PASSED;
                    casepassed++;
                    sendOutput(2, "Test Case {0} PASSED", tc.name);
                    saveLogfile(sw, "Test Case {0} PASSED", tc.name);
                }
                else
                {
                    tr.Result = TestRes.FAILED;
                    casefailed++;
                    sendOutput(2, "Test Case {0} FAILED", tc.name);
                    saveLogfile(sw, "Test Case {0} FAILED", tc.name);
                }
                addtoTresList(tr);
                sendOutput(2, "---------- Test Case {0} finished ----------", tc.name);
                saveLogfile(sw, "---------- Test Case {0} finished ----------", tc.name);
                sw.Flush();
                sw.Close();
                fs.Close();
            }

            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
                casefailed++;
                sw.Flush();
                sw.Close();
                fs.Close();
            }

        }

        public void Log(string info, LogType type = LogType.Info)
        {

        }

        public returncode GetIdentity(byte pollAddr, StreamWriter sw)
        {
            saveLogfile(sw, "Polling address {0}.", pollAddr);
            //pollAddr,  chksum
            if (serialPort1.IsOpen)
            {
                byte[] Buffer = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0x02, 0x00, 0x00, 0x00, 0x02 };
                mp.rcvlen = 0;
                serialPort1.Write(Buffer, 0, 10);
                string msgSent = mp.buildStringTypeInfo((byte)Buffer.Length, Buffer);
                saveLogfile(sw, "Message sent to device.");
                saveLogfile(sw, msgSent);
                saveLogfile(sw, "Command 0 sent.");
            }
            else
            {
                saveLogfile(sw, "Port is not avaliable.");
                return returncode.eSerErr;
            }
            return returncode.eOk;
        }

        private void tabbedView_DocumentClosing(object sender, DocumentCancelEventArgs e)
        {
            //BaseDocument doc = e.Document;

            //if (doc.Caption[doc.Caption.Length - 1] == '*')
            //{
            //    if (XtraMessageBox.Show("The File \"" + doc.Caption.Substring(0, doc.Caption.Length - 1) + "\" is not saved, confirm to close?", "Closing File", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            //    {
            //        e.Cancel = true;
            //        return;
            //    }
            //}
        }

        private void iCloseProject_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!tp.saved)
            {
                if (XtraMessageBox.Show("The Current Project is not saved, confirm to Close?", "Closing Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    CloseProject();
                }
            }
            else
            {
                CloseProject();
            }
        }

        private void CloseProject()
        {
            tabbedView.Controller.CloseAll();
            tabbedView.Documents.Clear();
            testProjectExplorer.Clear();
            //iNewCase.Enabled = false;
            iCloseProject.Enabled = false;
            iProjectSettings.Enabled = false;
            iStart.Enabled = false;
            siExecute.Enabled = false;
            iAddHartDevice.Enabled = false;
            iAddDPDevice.Enabled = false;
            iAddExistingCase.Enabled = false;
            siAddDevice.Enabled = false;

            ucOutput1.Clear();

            projectsettings = null;
            tp = null;
            devices = null;
            newtestcasecount = 0;
            //devicecount = 0;
        }

        private void iProjectSettings_ItemClick(object sender, ItemClickEventArgs e)
        {
            AddNewSettings("Project Settings");
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tp != null && !tp.saved)
            {
                if (XtraMessageBox.Show("The Current Project is not saved, confirm to Close?", "Closing Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = false;
            }

        }

        private void SaveProject(string savepath, bool bUpdate = true)
        {
            /*
            TestResult tr = new TestResult();
            addtoTresList(tr);
            */
            //StreamWriter sw = new StreamWriter(savepath);

            XmlTextWriter myXmlTextWriter = new XmlTextWriter(savepath, null);

            myXmlTextWriter.Formatting = Formatting.Indented;

            myXmlTextWriter.WriteStartDocument(false);
            myXmlTextWriter.WriteStartElement("CTSProject");

            myXmlTextWriter.WriteComment("Conformance Test Project");

            myXmlTextWriter.WriteStartElement("Project");

            myXmlTextWriter.WriteAttributeString("ProjectName", tp.ProjectName);
            myXmlTextWriter.WriteAttributeString("TypeofProject", tp.TypeofProject.ToString());

            myXmlTextWriter.WriteElementString("FileName", tp.FileName);
            myXmlTextWriter.WriteElementString("timemodified", tp.timemodified.ToString());
            myXmlTextWriter.WriteElementString("projectroot", tp.projectroot);
            myXmlTextWriter.WriteElementString("AutoSave", tp.AutoSave.ToString());
            myXmlTextWriter.WriteElementString("TestReport", tp.TestReport.ToString());
            myXmlTextWriter.WriteElementString("FullPath", tp.FullPath);
            myXmlTextWriter.WriteElementString("Description", tp.Description);


            myXmlTextWriter.WriteElementString("ExecutiveDate", tp.ExecutiveDate);
            myXmlTextWriter.WriteElementString("BeginTestingDate", tp.BeginTestingDate);
            myXmlTextWriter.WriteElementString("DocName", tp.DocName);

            myXmlTextWriter.WriteElementString("Organization", tp.Organization);
            myXmlTextWriter.WriteElementString("Address", tp.Address);
            myXmlTextWriter.WriteElementString("Tester", tp.Tester);
            myXmlTextWriter.WriteElementString("Phone", tp.Phone);
            myXmlTextWriter.WriteElementString("Email", tp.Email);

            myXmlTextWriter.WriteEndElement();

            myXmlTextWriter.WriteStartElement("Devices");

            if (tp.TypeofProject == ProjectType.DP)
            {
                myXmlTextWriter.WriteAttributeString("DeviceType", tp.TypeofProject.ToString());

                List<string> devnamelist = new List<string>();
                foreach (DPDevice dev in (devices.devList as List<DPDevice>))
                {
                    if (!devnamelist.Contains(dev.devName))
                    {
                        devnamelist.Add(dev.devName);
                        myXmlTextWriter.WriteStartElement("DPDevice");
                        myXmlTextWriter.WriteElementString("devName", dev.devName);
                        myXmlTextWriter.WriteElementString("SlaveAddr", dev.SlaveAddr.ToString());
                        myXmlTextWriter.WriteElementString("IdentNumber", dev.IdentNumber);
                        myXmlTextWriter.WriteElementString("MinTsdr", dev.MinTsdr.ToString());
                        myXmlTextWriter.WriteElementString("InputLen", dev.InputLen.ToString());
                        myXmlTextWriter.WriteElementString("OutputLen", dev.OutputLen.ToString());
                        myXmlTextWriter.WriteElementString("UserExtPrmDataLen", dev.UserExtPrmDataLen.ToString());
                        myXmlTextWriter.WriteElementString("UserExtPrmData", dev.UserPrmData);
                        myXmlTextWriter.WriteElementString("CfgDataLen", dev.CfgDataLen.ToString());
                        myXmlTextWriter.WriteElementString("CfgData", dev.CfgData);
                        myXmlTextWriter.WriteElementString("Description", dev.Description);
                        myXmlTextWriter.WriteElementString("Offline", dev.Offline.ToString());
                        myXmlTextWriter.WriteElementString("gsdFile", dev.gsdFile);

                        myXmlTextWriter.WriteElementString("Model", dev.Model);
                        myXmlTextWriter.WriteElementString("DDRevision", dev.DDRevision);
                        myXmlTextWriter.WriteElementString("TypeofDevice", dev.TypeofDevice);
                        myXmlTextWriter.WriteElementString("DeviceRevision", dev.DeviceRevision);
                        myXmlTextWriter.WriteElementString("Description", dev.Description);
                        myXmlTextWriter.WriteElementString("ProtocolRevision", dev.ProtocolRevision);
                        myXmlTextWriter.WriteElementString("SoftwareRevision", dev.SoftwareRevision);
                    }
                    else
                    {
                        if (XtraMessageBox.Show("The device name should be unique, continue saving with duplicated device removed?", "Save Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        {
                            break;
                        }
                    }
                    myXmlTextWriter.WriteEndElement();
                }
            }

            else if (tp.TypeofProject == ProjectType.Hart)
            {
                myXmlTextWriter.WriteAttributeString("DeviceType", tp.TypeofProject.ToString());

                List<string> devnamelist = new List<string>();
                foreach (HartDevice dev in (devices.devList as List<HartDevice>))
                {
                    if (!devnamelist.Contains(dev.devName))
                    {
                        devnamelist.Add(dev.devName);
                        myXmlTextWriter.WriteStartElement("HartDevice");
                        myXmlTextWriter.WriteElementString("devName", dev.devName);
                        myXmlTextWriter.WriteElementString("SlaveAddr", dev.SlaveAddr.ToString());
                        myXmlTextWriter.WriteElementString("DeviceTag", dev.DeviceTag);
                        myXmlTextWriter.WriteElementString("DeviceType", dev.DeviceType);
                        myXmlTextWriter.WriteElementString("Model", dev.Model);
                        myXmlTextWriter.WriteElementString("DDPathRoot", dev.DDPathRoot);
                        myXmlTextWriter.WriteElementString("Offline", dev.Offline.ToString());
                        myXmlTextWriter.WriteElementString("manufID", dev.manufID);
                        myXmlTextWriter.WriteElementString("DDRevision", dev.DDRevision);
                        myXmlTextWriter.WriteElementString("TypeofDevice", dev.TypeofDevice);
                        myXmlTextWriter.WriteElementString("DeviceRevision", dev.DeviceRevision);
                        myXmlTextWriter.WriteElementString("Description", dev.Description);
                        myXmlTextWriter.WriteElementString("ProtocolRevision", dev.ProtocolRevision);
                        myXmlTextWriter.WriteElementString("SoftwareRevision", dev.SoftwareRevision);
                        myXmlTextWriter.WriteEndElement();
                    }
                    else
                    {
                        if (XtraMessageBox.Show("The device name should be unique, continue saving with duplicated device removed?", "Save Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        {
                            break;
                        }
                    }
                }
            }

            myXmlTextWriter.WriteEndElement();


            myXmlTextWriter.WriteEndElement();

            myXmlTextWriter.Flush();
            myXmlTextWriter.Close();

            if (bUpdate)
            {
                UpdateProjectName();
            }
        }

        private void CopyDirectory(string srcPath, string desPath)

        {
            //string folderName = srcPath.Substring(srcPath.LastIndexOf("\\") + 1);

            string desfolderdir = desPath;// + "\\" + folderName;

            //if (desPath.LastIndexOf("\\") == (desPath.Length - 1))
            {
                //desfolderdir = desPath + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcPath);

            foreach (string file in filenames)
            {
                if (Directory.Exists(file))
                {
                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, currentdir);
                }

                else
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);
                    srcfileName = desfolderdir + "\\" + srcfileName;

                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }
                    File.Copy(file, srcfileName);
                }
            }
        }

        private bool LoadProject(string filePath)
        {

            //if (tp != null || projectsettings != null)
            {
                CloseProject();
            }
            try
            {
                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormDescription("Openning Project.....");     // 信息
                Thread.Sleep(sleeptime);
                tp = new TestProject();
                tp.IsNewProject = false;

                XmlTextReader reader = new XmlTextReader(filePath);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "Project":
                                tp.ProjectName = reader.GetAttribute("ProjectName");
                                string projtype = reader.GetAttribute("TypeofProject");
                                switch (projtype)
                                {
                                    case "Hart":
                                        tp.TypeofProject = ProjectType.Hart;
                                        devices = new TestDevices(tp);
                                        tp.Devices = devices;
                                        break;

                                    case "DP":
                                        tp.TypeofProject = ProjectType.DP;
                                        devices = new TestDevices(tp);
                                        tp.Devices = devices;
                                        break;

                                    default:
                                        tp.TypeofProject = ProjectType.Blank;
                                        break;
                                }
                                break;

                            case "FileName":
                                tp.FileName = reader.ReadElementString().Trim();
                                break;

                            case "timemodified":
                                tp.timemodified = Convert.ToDateTime(reader.ReadElementString().Trim());
                                break;

                            case "projectroot":
                                //read only tp.projectroot = reader.ReadElementString().Trim();
                                break;

                            case "AutoSave":
                                tp.AutoSave = Convert.ToBoolean(reader.ReadElementString().Trim());
                                break;

                            case "TestReport":
                                tp.TestReport = Convert.ToBoolean(reader.ReadElementString().Trim());
                                break;

                            case "FullPath":
                                //tp.FullPath = reader.ReadElementString().Trim();
                                break;

                            case "Description":
                                tp.Description = reader.ReadElementString().Trim();
                                break;

                            case "ExecutiveDate":
                                tp.ExecutiveDate = reader.ReadElementString().Trim();
                                break;

                            case "BeginTestingDate":
                                tp.BeginTestingDate = reader.ReadElementString().Trim();
                                break;

                            case "DocName":
                                tp.DocName = reader.ReadElementString().Trim();
                                break;

                            case "Organization":
                                tp.Organization = reader.ReadElementString().Trim();
                                break;
                            case "Address":
                                tp.Address = reader.ReadElementString().Trim();
                                break;
                            case "Tester":
                                tp.Tester = reader.ReadElementString().Trim();
                                break;
                            case "Phone":
                                tp.Phone = reader.ReadElementString().Trim();
                                break;
                            case "Email":
                                tp.Email = reader.ReadElementString().Trim();
                                break;
                            case "DPDevice":
                                break;

                            case "Devices":
                                XElement hele = XElement.ReadFrom(reader) as XElement;
                                IEnumerable<XElement> elements;
                                if (tp.TypeofProject == ProjectType.Hart)
                                {
                                    elements = hele.Elements("HartDevice");
                                    foreach (var eleme in elements)
                                    {
                                        HartDevice dev = new HartDevice(devices);
                                        dev.devName = eleme.Element("devName").Value;
                                        dev.SlaveAddr = Convert.ToInt32(eleme.Element("SlaveAddr").Value);
                                        dev.DeviceTag = eleme.Element("DeviceTag").Value;
                                        dev.DeviceType = eleme.Element("DeviceType").Value;

                                        if (null != eleme.Element("Model"))
                                        {
                                            dev.Model = eleme.Element("Model").Value;
                                        }
                                        dev.DDPathRoot = eleme.Element("DDPathRoot").Value;
                                        dev.Offline = Convert.ToBoolean(eleme.Element("Offline").Value);
                                        dev.manufID = eleme.Element("manufID").Value;
                                        dev.DDRevision = eleme.Element("DDRevision").Value;

                                        if (null != eleme.Element("TypeofDevice"))
                                        {
                                            dev.TypeofDevice = eleme.Element("TypeofDevice").Value;
                                        }

                                        if (null != eleme.Element("DeviceRevision"))
                                        {
                                            dev.DeviceRevision = eleme.Element("DeviceRevision").Value;
                                        }

                                        if (null != eleme.Element("Description"))
                                        {
                                            dev.Description = eleme.Element("Description").Value;
                                        }

                                        if (null != eleme.Element("SoftwareRevision"))
                                        {
                                            dev.SoftwareRevision = eleme.Element("SoftwareRevision").Value;
                                        }

                                        if (null != eleme.Element("ProtocolRevision"))
                                        {
                                            dev.ProtocolRevision = eleme.Element("ProtocolRevision").Value;
                                        }
                                    }
                                }
                                else
                                {
                                    elements = hele.Elements("DPDevice");
                                    foreach (var eleme in elements)
                                    {
                                        DPDevice ddev = new DPDevice(devices);
                                        ddev.devName = eleme.Element("devName").Value;
                                        ddev.SlaveAddr = Convert.ToInt32(eleme.Element("SlaveAddr").Value);
                                        if (eleme.Element("IdentNumber") != null)
                                        {
                                            ddev.IdentNumber = eleme.Element("IdentNumber").Value;
                                        }
                                        if (eleme.Element("MinTsdr") != null)
                                        {
                                            ddev.MinTsdr = Convert.ToByte(eleme.Element("MinTsdr").Value);
                                        }
                                        if (eleme.Element("InputLen") != null)
                                        {
                                            ddev.InputLen = Convert.ToByte(eleme.Element("InputLen").Value);
                                        }
                                        if (eleme.Element("OutputLen") != null)
                                        {
                                            ddev.OutputLen = Convert.ToByte(eleme.Element("OutputLen").Value);
                                        }
                                        if (eleme.Element("UserExtPrmDataLen") != null)
                                        {
                                            ddev.UserExtPrmDataLen = Convert.ToByte(eleme.Element("UserExtPrmDataLen").Value);
                                        }
                                        if (eleme.Element("UserExtPrmData") != null)
                                        {
                                            ddev.UserPrmData = eleme.Element("UserExtPrmData").Value;
                                        }
                                        if (eleme.Element("CfgDataLen") != null)
                                        {
                                            ddev.CfgDataLen = Convert.ToByte(eleme.Element("CfgDataLen").Value);
                                        }
                                        if (eleme.Element("CfgData") != null)
                                        {
                                            ddev.CfgData = eleme.Element("CfgData").Value;
                                        }
                                        if (eleme.Element("Description") != null)
                                        {
                                            ddev.Description = eleme.Element("Description").Value;
                                        }
                                        if (eleme.Element("Offline") != null)
                                        {
                                            ddev.Offline = Convert.ToBoolean(eleme.Element("Offline").Value);
                                        }
                                        if (eleme.Element("gsdFile") != null)
                                        {
                                            ddev.gsdFile = eleme.Element("gsdFile").Value;
                                        }


                                        if (null != eleme.Element("Model"))
                                        {
                                            ddev.Model = eleme.Element("Model").Value;
                                        }

                                        if (null != eleme.Element("DDRevision"))
                                        {
                                            ddev.DDRevision = eleme.Element("DDRevision").Value;
                                        }

                                        if (null != eleme.Element("TypeofDevice"))
                                        {
                                            ddev.TypeofDevice = eleme.Element("TypeofDevice").Value;
                                        }

                                        if (null != eleme.Element("DeviceRevision"))
                                        {
                                            ddev.DeviceRevision = eleme.Element("DeviceRevision").Value;
                                        }

                                        if (null != eleme.Element("SoftwareRevision"))
                                        {
                                            ddev.SoftwareRevision = eleme.Element("SoftwareRevision").Value;
                                        }

                                        if (null != eleme.Element("ProtocolRevision"))
                                        {
                                            ddev.ProtocolRevision = eleme.Element("ProtocolRevision").Value;
                                        }
                                    }
                                }

                                break;

                            case "HartDevice":
                                break;

                            default:
                                break;
                        }
                    }
                }
                if (reader.NodeType == XmlNodeType.EndElement)
                {

                }

                reader.Close();

                iNewCase.Enabled = true;
                iCloseProject.Enabled = true;
                iProjectSettings.Enabled = true;
                iStart.Enabled = true;
                siExecute.Enabled = true;
                iAddHartDevice.Enabled = ProjectType.Hart == tp.TypeofProject ? true : false;
                iAddDPDevice.Enabled = ProjectType.DP == tp.TypeofProject ? true : false;

                string dirPath = string.Empty;
                if (tp.TypeofProject == ProjectType.Hart || tp.TypeofProject == ProjectType.DP)
                {
                    dirPath = Path.Combine(Path.GetDirectoryName(filePath),
                        Path.GetFileNameWithoutExtension(filePath), "TestClasses");
                }
                else
                {
                    testProjectExplorer.AddBlankNodes();
                }
                testProjectExplorer.LoadTreeView(tp, dirPath);

                iAddExistingCase.Enabled = true;
                siAddDevice.Enabled = true;
                tp.saved = true;

                UpdateProjectName(false);
                splashScreenManager1.SetWaitFormCaption("Loaded, please wait....");     // 标题
                splashScreenManager1.SetWaitFormDescription("Project loaded, displaying contents.....");     // 信息
                Thread.Sleep(sleeptime);
                splashScreenManager1.CloseWaitForm();

                SaveHistoryFile(filePath, tp.TypeofProject);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                splashScreenManager1.CloseWaitForm();
                CloseProject();
                return false;
            }
            return true;
        }

        public void saveprojectfile()
        {
            if (tp == null)
            {
                return;
            }

            string saveaspath = "";//save as
            if (tp.FullPath != null)
            {
                saveaspath = tp.FullPath;
                FileInfo fi = new FileInfo(saveaspath);//创建directory
                fi.Directory.Create();
                //tp.FullPath = saveaspath;

                //int la = saveaspath.LastIndexOf('\\');
                //int len = saveaspath.LastIndexOf('.');
                //len = len - la - 1;
                //saveaspath = saveaspath.Substring(la + 1, len);
                tp.FileName = System.IO.Path.GetFileName(saveaspath);
                //tp.ProjectName = System.IO.Path.GetFileNameWithoutExtension(saveaspath);
                tp.saved = true;
                tp.timemodified = DateTime.Now;
                SaveProject(saveaspath, false);
            }
            else
            {
                SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
                DirectoryInfo prodir = new DirectoryInfo(tp.projectroot);
                prodir.Create();

                dialog.InitialDirectory = tp.projectroot;
                dialog.Filter = "file(*.cts)|*.cts";//设置对话框保存的文件类型
                dialog.FileName = tp.ProjectName;
                string prdir = "";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//将ok返回默认用户公共对话框
                {
                    saveaspath = dialog.FileName;//获取文件路径和文件名
                    prdir = dialog.InitialDirectory;
                }
                if (saveaspath != "" && prdir == tp.projectroot)
                {
                    FileInfo fi = new FileInfo(saveaspath);//创建文件
                    fi.Directory.Create();
                    //SaveWaveToFile();//这句是自己创建的保存函数方法，
                    //tp.FullPath = saveaspath;
                    //int la = saveaspath.LastIndexOf('\\');
                    //int len = saveaspath.LastIndexOf('.');
                    //len = len - la - 1;
                    //saveaspath = saveaspath.Substring(la + 1, len);
                    tp.FileName = System.IO.Path.GetFileName(saveaspath);
                    //tp.ProjectName = System.IO.Path.GetFileNameWithoutExtension(saveaspath);
                    tp.saved = true;
                    tp.timemodified = DateTime.Now;
                    SaveProject(saveaspath);
                }
                else
                {
                    XtraMessageBox.Show("The project must save in .\\project folder", "Save Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void iSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Save();
            //return;
            //iOpenFile.Enabled = !iOpenFile.Enabled;

            if (tabbedView.ActiveDocument == null || tabbedView.ActiveDocument.Tag == null)
            {
                return;
            }
            docDesc dsc = (docDesc)tabbedView.ActiveDocument.Tag;
            if (dsc.doctype == docType.testcase)
            {
                if (dsc.wholename != null)
                {
                    ucTextEditor te = (ucTextEditor)dsc.doccon;
                    te.SaveToFile(dsc.wholename);
                    tabbedView.ActiveDocument.Caption = tabbedView.ActiveDocument.Caption.Trim('*');
                }
                else
                {
                    string filePath = Path.Combine(tp.FullPath, tp.FileName);
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.InitialDirectory = System.Environment.CurrentDirectory + "\\TestCases\\";
                    dlg.Filter = "Test Case(*.cas)|*.cas";
                    dlg.Title = "Save Test Case";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        string openname = dlg.FileName;
                        ucTextEditor te = (ucTextEditor)dsc.doccon;
                        te.SaveToFile(openname);
                        tabbedView.ActiveDocument.Caption = Path.GetFileName(openname);
                        dsc.wholename = openname;
                        showsuperTips(tabbedView.ActiveDocument, Path.GetFileName(openname), openname, (int)NodeType.testcase);
                        tabbedView.ActiveDocument.Tag = dsc;
                        iSave.Caption = "&Save " + Path.GetFileName(openname);
                        iSaveAs.Caption = "Save " + Path.GetFileName(openname) + " &as";
                        tabbedView.ActiveDocument.Caption = tabbedView.ActiveDocument.Caption.Trim('*');
                    }
                }
            }
        }

        private void Save()
        {
            SaveProject(tp.FullPath);
            testProjectExplorer.Save();
            tp.saved = true;

            MessageBox.Show("保存成功");
        }

        void UpdateProjectName(bool bNode = true)
        {
            barStaticItem1.Caption = tp.ProjectName;
            if (bNode)
            {
                testProjectExplorer.setProjectName(tp.ProjectName);
            }
            if (projectsettings != null)
            {
                projectsettings.UpdateSettings(tp);
                projectsettings.RefreshPro();
            }

            string saveaspath = "";//save as
            if (tp.FullPath != null)
            {
                saveaspath = tp.FullPath;
                FileInfo fi = new FileInfo(saveaspath);//创建directory
                fi.Directory.Create();
                //tp.FullPath = saveaspath;

                //int la = saveaspath.LastIndexOf('\\');
                //int len = saveaspath.LastIndexOf('.');
                //len = len - la - 1;
                //saveaspath = saveaspath.Substring(la + 1, len);
                tp.FileName = System.IO.Path.GetFileName(saveaspath);
                //tp.ProjectName = System.IO.Path.GetFileNameWithoutExtension(saveaspath);
                tp.saved = true;
                tp.timemodified = DateTime.Now;
                SaveProject(saveaspath, false);
            }

        }

        private void iSaveAs_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (tabbedView.ActiveDocument == null || tabbedView.ActiveDocument.Tag == null)
            {
                return;
            }
            docDesc dsc = (docDesc)tabbedView.ActiveDocument.Tag;
            if (dsc.doctype == docType.testcase)
            {
                SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
                dialog.InitialDirectory = tp.projectroot + tp.ProjectName + "\\TestCases\\";
                dialog.Filter = "Test Case(*.cas)|*.cas";//设置对话框保存的文件类型
                dialog.Title = "Save Test Case";
                dialog.FileName = Path.GetFileNameWithoutExtension(dsc.wholename);
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string openname = dialog.FileName;
                    ucTextEditor te = (ucTextEditor)dsc.doccon;
                    te.SaveToFile(openname);
                    tabbedView.ActiveDocument.Caption = Path.GetFileName(openname);
                    dsc.wholename = openname;
                    showsuperTips(tabbedView.ActiveDocument, Path.GetFileName(openname), openname, (int)NodeType.testcase);
                    tabbedView.ActiveDocument.Tag = dsc;
                    iSave.Caption = "&Save " + Path.GetFileName(openname);
                    iSaveAs.Caption = "Save " + Path.GetFileName(openname) + " &as";
                    tabbedView.ActiveDocument.Caption = tabbedView.ActiveDocument.Caption.Trim('*');
                }
                /*
                if (saveaspath != "" && prdir == tp.projectroot)
                {
                    FileInfo fi = new FileInfo(saveaspath);//创建文件
                    fi.Directory.Create();
                    //SaveWaveToFile();//这句是自己创建的保存函数方法，
                    //tp.FullPath = saveaspath;

                    DirectoryInfo dr = new DirectoryInfo(saveaspath);
                    dr.Create();

                    //int la = saveaspath.LastIndexOf('\\');
                    //int len = saveaspath.LastIndexOf('.');
                    //len = len - la - 1;
                    //saveaspath = saveaspath.Substring(la + 1, len);
                    tp.FileName = System.IO.Path.GetFileName(saveaspath);
                    //tp.ProjectName = System.IO.Path.GetFileNameWithoutExtension(saveaspath);
                    tp.saved = true;
                    tp.timemodified = DateTime.Now;
                    SaveProject(saveaspath);
                }
                else
                {
                    XtraMessageBox.Show("The project must save in .\\project folder", "Save Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                */
            }
        }

        private void testProjectExplorer_CellValueChanged(object sender, System.EventArgs e)
        {
            DevExpress.XtraTreeList.TreeList treeView = sender as DevExpress.XtraTreeList.TreeList;

            if (treeView.FocusedNode.Tag == null)
            {
                return;
            }

            NodeInfo ni = (NodeInfo)treeView.FocusedNode.Tag;

            tp.saved = false;

            if (ni.ntype == NodeType.root)
            {

                string newname = (string)treeView.FocusedNode.GetValue(0);

                if (newname.TrimEnd(spaces) == "" || File.Exists(tp.projectroot + "\\" + newname + ".cts"))
                {
                    treeView.FocusedNode.SetValue(0, ni.nodename);
                    return;
                }

                string srcfile = tp.projectroot + "\\" + ni.nodename + ".cts";
                string desfile = tp.projectroot + "\\" + newname + ".cts";
                string srcdir = tp.projectroot + "\\" + ni.nodename;
                string desdir = tp.projectroot + "\\" + newname;

                File.Move(srcfile, desfile);
                Directory.Move(srcdir, desdir);

                tp.ProjectName = (string)treeView.FocusedNode.GetValue(0);
                tp.FileName = tp.ProjectName + ".cts";
                //tp.FullPath = tp.projectroot + "\\" + tp.FileName;
                ni.nodename = newname;

                UpdateProjectName();

            }
            else if (ni.ntype == NodeType.devices)
            {
                string newname = (string)treeView.FocusedNode.GetValue(0);
                if (newname.TrimEnd(spaces) == "" || devices[newname] != null)
                {
                    treeView.FocusedNode.SetValue(0, ni.nodename);
                    return;
                }
                devices.setnewName(ni.nodename, newname);
                foreach (BaseDocument doc in tabbedView.Documents)
                {
                    if (doc.Caption == ni.nodename)
                    {
                        ucDeviceProperty deviceproperty = (ucDeviceProperty)doc.Control;
                        deviceproperty.refreshDev();
                        doc.Caption = newname;
                        tabbedView.Controller.Activate(doc);
                    }
                }
                ni.nodename = newname;
            }
            else if (ni.ntype == NodeType.testclass)
            {
                NodeInfo nodeInfo = treeView.FocusedNode.Tag as NodeInfo;
                TestClassModel model = nodeInfo.SourceData as TestClassModel;

                string newname = (string)treeView.FocusedNode.GetValue(0);
                if (newname.TrimEnd(spaces) == "")
                {
                    MessageBox.Show("The Name Can't Be Null");
                    treeView.FocusedNode.SetValue(0, ni.nodename);
                    return;
                }
                else if (false == model.Name.Equals(newname)
                    && true == Directory.Exists(Path.Combine(model.FoldPath, newname)))
                {
                    MessageBox.Show("The Test Class Name Has Existed");
                    treeView.FocusedNode.SetValue(0, ni.nodename);
                    return;
                }

                model.NewName = newname;
                ni.nodename = newname;

                foreach (var item in model)
                {
                    foreach (BaseDocument doc in tabbedView.Documents)
                    {
                        if (doc.Tag != null && doc.Tag == item)
                        {
                            ucTextEditor textEditor = (ucTextEditor)doc.Control;
                            if (null != textEditor)
                            {
                                textEditor.LoadCode(item);
                            }
                            showsuperTips(doc, newname, newname, (int)NodeType.testcase);
                            break;
                        }
                    }
                }
            }
            else if (ni.ntype == NodeType.testcase)
            {
                string newname = (string)treeView.FocusedNode.GetValue(0);

                NodeInfo nodeInfo = treeView.FocusedNode.Tag as NodeInfo;
                TestCaseModel model = nodeInfo.SourceData as TestCaseModel;

                if (newname.TrimEnd(spaces) == "")
                {
                    MessageBox.Show("The Name Can't Be Null");
                    treeView.FocusedNode.SetValue(0, ni.nodename);
                    return;
                }
                else if (false == model.Name.Equals(newname)
                    && true == File.Exists(Path.Combine(model.ParentNodedata.FoldPath,
                    model.ParentNodedata.Name, newname + ".cas")))
                {
                    MessageBox.Show("The Test Case Name Has Existed");
                    treeView.FocusedNode.SetValue(0, ni.nodename);
                    return;
                }

                model.NewName = newname;
                ni.nodename = newname;
                string casedir = Path.GetDirectoryName(ni.wholename);

                foreach (BaseDocument doc in tabbedView.Documents)
                {
                    if (doc.Tag != null && doc.Tag == model)
                    {
                        doc.Caption = newname;
                        ucTextEditor textEditor = (ucTextEditor)doc.Control;
                        if (null != textEditor)
                        {
                            textEditor.LoadCode(model);
                        }
                        showsuperTips(doc, newname, newname, (int)NodeType.testcase);
                        break;
                    }
                }
            }

        }

        private void iOpenFile_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = System.Environment.CurrentDirectory + "\\Project\\";
            dlg.Filter = "Test Project(*.cts)|*.cts|Test Case(*.cas)|*.cas|Test Report(*.html)|*.html|Test Log(*.tlg)|*.tlg|All files (*.*)|*.*";
            dlg.FilterIndex = 5;
            dlg.Title = "Open File";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string openname = dlg.FileName;
                string ext = Path.GetExtension(openname);
                switch (ext)
                {
                    case ".cts":
                        if (tp != null && !tp.saved)
                        {
                            if (XtraMessageBox.Show("The Current Project is not saved, confirm to open another one?", "Open Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                            {
                                return;
                            }
                        }
                        if (openname.Substring(0, openname.LastIndexOf('\\')) == (System.Environment.CurrentDirectory + "\\Project"))
                        {
                            if (!LoadProject(dlg.FileName))
                            {
                                XtraMessageBox.Show("The project format is not correct", "Open Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("The project must save in .\\project folder", "Open Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;

                    case ".cas":
                        DispTestCase(Path.GetFileName(openname), openname);
                        break;

                    case ".html":
                        DispTestReport(Path.GetFileNameWithoutExtension(openname), openname);
                        break;

                    case ".tlg":
                        break;

                    default:
                        DispCommonDoc(Path.GetFileName(openname), openname);
                        break;
                }

            }
        }

        private void iOpenSolution_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (tp != null && !tp.saved)
            {
                if (XtraMessageBox.Show("The Current Project is not saved, confirm to open another one?", "Open Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    return;
                }
            }
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = System.Environment.CurrentDirectory + "\\Project\\";
            dlg.Filter = "Test Project(*.cts)|*.cts";
            dlg.Title = "Open Project";
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string openname = dlg.FileName;
                if (openname.Substring(0, openname.LastIndexOf('\\')) == (System.Environment.CurrentDirectory + "\\Project"))
                {
                    if (!LoadProject(dlg.FileName))
                    {
                        XtraMessageBox.Show("The project format is not correct", "Open Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("The project must save in .\\project folder", "Open Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void iNewProject_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (tp != null && !tp.saved)
            {
                if (XtraMessageBox.Show("The Current Project is not saved, confirm to create a new one?", "New Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    return;
                }
            }
            CloseProject();

            tp = new TestProject();
            NewProject newpro = new NewProject(tp);
            DialogResult res = newpro.ShowDialog(this);
            if (res != DialogResult.OK)
            {
                if (res == DialogResult.Abort)
                {
                    if (tp.ProjectName == null)
                    {
                        XtraMessageBox.Show("The Project already exists", "New Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (tp.ProjectName == "")
                    {
                        XtraMessageBox.Show("The Project Name is invalid", "New Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                CloseProject();
                return;
            }

            string saveaspath = tp.projectroot + "\\" + tp.ProjectName + ".cts";

            FileInfo fi = new FileInfo(saveaspath);//创建文件
            fi.Directory.Create();
            tp.FileName = System.IO.Path.GetFileName(saveaspath);
            tp.saved = true;
            tp.timemodified = DateTime.Now;
            string prdirbase = saveaspath.Substring(0, saveaspath.Length - 4);
            DirectoryInfo dr = new DirectoryInfo(prdirbase);
            dr.Create();

            string casesdir = tp.projectroot + "\\" + tp.ProjectName + "\\TestCases\\";
            dr = new DirectoryInfo(casesdir);
            dr.Create();

            /*Test Report*/
            string reportfile = tp.projectroot + tp.ProjectName + "\\TestReport.html";
            FileStream fs = new FileStream(reportfile, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("//No test performed.");
            sw.WriteLine("");
            sw.WriteLine("Time:" + DateTime.Now.ToString());
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("Passed 0");
            sw.WriteLine("Failed 0");
            sw.WriteLine("NotRun 0");
            sw.Close();

            string dirPath = string.Empty;
            if (tp.TypeofProject == ProjectType.DP)
            {
                dirPath = Path.Combine(System.Environment.CurrentDirectory, "TestLib", "DPTest");
            }
            else if (tp.TypeofProject == ProjectType.Hart)
            {
                dirPath = Path.Combine(System.Environment.CurrentDirectory, "TestLib", "HartTest");
            }
            devices = new TestDevices(tp);
            string casepath = "";
            tp.Devices = devices;
            casepath = Path.Combine(System.Environment.CurrentDirectory, "Project", tp.ProjectName, "TestClasses");
            CopyDirectory(dirPath, casepath);
            testProjectExplorer.LoadTreeView(tp, casepath);
            InitControlState();
            SaveProject(saveaspath);
            // 保存历史文件信息
            SaveHistoryFile(saveaspath, tp.TypeofProject);
        }

        /// <summary>
        /// 保存历史文件
        /// </summary>
        /// <param name="projectPath"></param>
        private void SaveHistoryFile(string projectPath, ProjectType projectType)
        {
            try
            {
                // 文件不存在，不进入历史文件
                if (false == File.Exists(projectPath))
                {
                    return;
                }

                HistoryModel model = new HistoryModel()
                {
                    FilePath = projectPath,
                    LastModified = DateTime.Now,
                    ProjectType = projectType
                };
                HistoryModels.Save(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化控件状态
        /// </summary>
        private void InitControlState()
        {
            barStaticItem1.Caption = tp.ProjectName;
            iNewCase.Enabled = true;
            iCloseProject.Enabled = true;
            iProjectSettings.Enabled = true;
            iStart.Enabled = true;
            siExecute.Enabled = true;

            iAddHartDevice.Enabled = ProjectType.Hart == tp.TypeofProject ? true : false;
            iAddDPDevice.Enabled = ProjectType.DP == tp.TypeofProject ? true : false;

            iAddExistingCase.Enabled = true;
            siAddDevice.Enabled = true;
        }

        private void tabbedView_DocumentActivated(object sender, DocumentEventArgs e)
        {
            if (e.Document.Tag == null)
            {
                return;
            }

            return;
            /*
            docDesc dsc = (docDesc)e.Document.Tag;
            if (dsc.doctype == docType.testcase)
            {
                string filename;
                if (e.Document.Caption[e.Document.Caption.Length - 1] == '*')
                {
                    filename = e.Document.Caption.Substring(0, e.Document.Caption.Length - 1);
                }
                else
                {
                    filename = e.Document.Caption;
                }
                iSave.Caption = "&Save " + filename;
                iSaveAs.Caption = "Save " + filename + " &as";
            }
            */
        }

        private void iSaveAll_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (tp != null)
            {
                saveprojectfile();
                testProjectExplorer.Save();
                tp.saved = true;
            }
            foreach (BaseDocument bd in tabbedView.Documents)
            {
                docDesc dsc = (docDesc)bd.Tag;
                if (dsc != null && dsc.doctype == docType.testcase)
                {
                    if (dsc.wholename != null)
                    {
                        ucTextEditor te = (ucTextEditor)dsc.doccon;
                        te.SaveToFile(dsc.wholename);
                        bd.Caption = bd.Caption.Trim('*');
                    }
                }
            }
        }

        private void dockPanel5_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            ucOutput1.tBox.Clear();
        }

        private void dockPanel5_CustomButtonChecked(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button == dockPanel5.CustomHeaderButtons[0])
            {
                ucOutput1.tBox.WordWrap = true;
            }
        }

        private void dockPanel5_CustomButtonUnchecked(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button == dockPanel5.CustomHeaderButtons[0])
            {
                ucOutput1.tBox.WordWrap = false;
            }
        }

        private void testProjectExplorer_deviceRemoving(object sender, EventArgs e)
        {
            devices.removeDevice(sender as string);
            tp.saved = false;
        }

        private void biRefreshCom_ItemClick(object sender, ItemClickEventArgs e)
        {
            initComBox();
        }

        private void toolbarFormControl1_Merge(object sender, DevExpress.XtraBars.ToolbarForm.ToolbarFormMergeEventArgs e)
        {
            ;
        }

        private void biOpenPort_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.PortName = biSerCombo.EditValue.ToString();
                    this.serialPort1.BaudRate = 1200;
                    this.serialPort1.Parity = System.IO.Ports.Parity.Odd;

                    serialPort1.Open();
                    biSerCombo.Enabled = false;
                    biOpenPort.Caption = Resource.Close;
                    biOpenPort.Hint = "Close port";
                    biOpenPort.ImageIndex = 55;

                    //string info = String.Format(Resource.portopened, toolStripComboBox1.Text);
                    //Log(info, LogType.Ok);

                }
                catch (Exception ex)
                {
                    /**
                     * 1.异常消息
                     * 2.异常模块名称
                     * 3.异常方法名称
                     * 4.异常行号
                     */
                    String str = "";
                    str += ex.Message + "\n";//异常消息
                    //str += ex.StackTrace + "\n";//提示出错位置，不会定位到方法内部去
                    //str += ex.ToString() + "\n";//将方法内部和外部所有出错的位置提示出来
                    MessageBox.Show(str, Resource.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //MessageBox.Show("Open " + Convert.ToString(toolStripComboBox1.Text), "提示");

            }
            else
            {
                serialPort1.Close();
                biSerCombo.Enabled = true;
                biOpenPort.Caption = Resource.Open;
                biOpenPort.Hint = "Open port";
                biOpenPort.ImageIndex = 54;

                //string info = String.Format(Resource.portclosed, toolStripComboBox1.Text);
                //Log(info);
            }
        }

        private void biSerCombo_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void frmMain_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();       //获得路径
            DispTestCase(Path.GetFileName(path), path);
        }

        private void popupMenu1_BeforePopup(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != testProjectExplorer && true == testProjectExplorer.CanCreateTestCase())
            {
                this.iNewCase.Enabled = true;
                this.iNewCase.Tag = testProjectExplorer.GetFocusedNode();
            }
            else
            {
                this.iNewCase.Enabled = false;
            }
        }

        private void popupMenu3_BeforePopup(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != testProjectExplorer && true == testProjectExplorer.CanCreateTestCase())
            {
                this.iAddExistingCase.Enabled = true;
                this.iAddExistingCase.Tag = testProjectExplorer.GetFocusedNode();
            }
            else
            {
                this.iAddExistingCase.Enabled = false;
            }

            if (null != testProjectExplorer && true == testProjectExplorer.CanCreateDevice())
            {
                bool isDPProject = ProjectType.DP == tp.TypeofProject;
                this.iAddHartDevice.Enabled = !isDPProject;
                this.iAddDPDevice.Enabled = isDPProject;
                this.iAddHartDevice.Tag = testProjectExplorer.GetFocusedNode();
                this.iAddDPDevice.Tag = testProjectExplorer.GetFocusedNode();
            }
            else
            {
                this.iAddHartDevice.Enabled = false;
                this.iAddDPDevice.Enabled = false;
            }
        }

        private void startuppage_ItemClick(object sender, ItemClickEventArgs e)
        {
            AddNewStartUp("Start Page");
        }

        private void testProjectExplorer_DisplayTextEvent(TestCaseModel obj)
        {
            try
            {
                if (null == obj)
                {
                    return;
                }

                string path = obj.GetTestLogPath();
                DispTestLog(path, Path.GetFileName(path));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    class docDesc
    {
        public string wholename;
        public docType doctype;
        public Control doccon;
    }

    enum docType
    {
        notused,
        startpage,
        setting,
        result,
        testcase,
        device,
        report,
        log
    }

    class testSchedule : List<testClass>
    {
        public bool changed = false;

        public testClass this[string name]
        {
            get
            {
                foreach (testClass tc in this)
                {
                    if (tc.name == name)
                    {
                        return tc;
                    }
                }
                return null;
            }
            set
            {
                if (!this.Contains(value))
                {
                    this.Add(value);
                }
            }
        }

        public new testClass this[int ID]
        {
            get
            {
                foreach (testClass tc in this)
                {
                    if (tc.classID == ID)
                    {
                        return tc;
                    }
                }
                return null;
            }
            set
            {
                if (!this.Contains(value))
                {
                    this.Add(value);
                }
            }
        }

    }

    class testClass : List<testCase>
    {
        public bool changed = false;
        public string name;
        public int classID;

        public testCase this[string name]
        {
            get
            {
                foreach (testCase tc in this)
                {
                    if (tc.name == name)
                    {
                        return tc;
                    }
                }
                return null;
            }
            set
            {
                if (!this.Contains(value))
                {
                    this.Add(value);
                }
            }
        }

        public new testCase this[int ID]
        {
            get
            {
                foreach (testCase tc in this)
                {
                    if (tc.caseID == ID)
                    {
                        return tc;
                    }
                }
                return null;
            }
            set
            {
                if (!this.Contains(value))
                {
                    this.Add(value);
                }
            }
        }

    }

    class testCase
    {
        public bool changed = false;
        public int classID = 0;
        public int caseID = 0;
        public string name;
        public string description;
        public ProjectType type;
        public object device = null;//dp or hart
        public List<TestModule> testFuncs;

        public TestResult caseresult;

        public testCase(ProjectType pt)
        {
            type = pt;
            testFuncs = new List<TestModule>();
            caseresult = new TestResult();
        }

        public TestResult getResult()
        {
            return caseresult;
        }

    }

    class parameter
    {
        public string name = null;
        public paramDataType dtype = paramDataType.unknown;
        public object value = null;

        public void setValue(string svalue)
        {
            switch (dtype)
            {
                case paramDataType.integer:
                    value = Convert.ToInt32(svalue);
                    break;

                case paramDataType.floatpoint:
                    value = Convert.ToSingle(svalue);
                    break;

                case paramDataType.octetstring:
                    value = Convert.ToByte(svalue);
                    break;

                case paramDataType.visiblestring:
                default:
                    value = svalue;
                    break;
            }
        }

    }

    class parameters : List<parameter>
    {
        public parameters()
        {
        }
    }

    public class result
    {
        public string name = null;
        public resultDataType rtype = resultDataType.unknown;
        public object value = null;

        public void setValue(string svalue)
        {
            switch (rtype)
            {
                case resultDataType.integer:
                    if (svalue.Contains("0x"))
                    {
                        value = Convert.ToInt32(svalue, 16);
                    }
                    else
                    {
                        value = Convert.ToInt32(svalue);
                    }
                    break;

                case resultDataType.floatpoint:
                    value = Convert.ToSingle(svalue);
                    break;

                case resultDataType.octetstring:
                    if (svalue.Contains("0x"))
                    {
                        value = Convert.ToByte(svalue, 16);
                    }
                    else
                    {
                        value = Convert.ToByte(svalue);
                    }
                    break;

                case resultDataType.visiblestring:
                default:
                    value = svalue;
                    break;
            }
        }

    }

    public class results : List<result>
    {
        public rspCode response;
        public string resDesc;

        public results()
        {
            response = rspCode.unknown;
        }

        public result this[string name]
        {
            get
            {
                foreach (result rt in this)
                {
                    if (rt.name == name)
                    {
                        return rt;
                    }
                }
                return null;
            }
        }

    }

    class TestModule
    {
        public string name = null;
        public int paraNum = 0;
        public parameters funcPara;
        public results funcRes;
        public int repeat = 1;

        public TestModule()
        {
            funcPara = new parameters();
            funcRes = new results();
        }
    }

    enum paramDataType
    {
        unknown,
        integer,
        floatpoint,
        visiblestring,
        octetstring
    }

    public enum resultDataType
    {
        unknown,
        integer,
        floatpoint,
        visiblestring,
        devinfo,
        variable,
        command,
        method,
        octetstring
    }

    public enum rspCode
    {
        unknown,
        positive,
        negitive
    }
}
