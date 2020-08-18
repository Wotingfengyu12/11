using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CQC.ConTest
{
    static class Program
    {
        private const int SW_HIDE = 0; 　　//从任务栏隐藏
        private const int SW_NORMAL = 1;    //正常弹出窗体 
        private const int SW_MAXIMIZE = 3;    //最大化弹出窗体 
        private const int SW_SHOWNOACTIVATE = 4; 　　//激活窗体/恢复窗体/还原窗体
        private const int SW_SHOW = 5; 　　//显示窗体，最小化时不会最大化
        private const int SW_MINIMIZE = 6; 　　//最小化
        private const int SW_RESTORE = 9;
        private const int SW_SHOWDEFAULT = 10;

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [STAThread]
        static void Main(string[] args)
        {
            Process instance = GetRunningInstance();
            if (instance == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                bool bSkinCustom = false;
                if (args.Length != 0)
                {
                    bSkinCustom = (args[0] == "-Skin");
                }
                DevExpress.UserSkins.BonusSkins.Register();
                DevExpress.Skins.SkinManager.EnableFormSkins();
                /*
                if (isBarDemo)
                {
                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2016 Colorful");
                }
                else
                {
                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Visual Studio 2013 Blue");
                }
                */
                var splashScreenImage = DevExpress.Utils.ResourceImageHelper.CreateImageFromResourcesEx("CQC.ConTest.Resources.SplashScreenNew.png", CurrentAssembly);
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowImage(splashScreenImage, true, false);
                Application.Run(new frmMain(bSkinCustom));
            }
            else
            {
                HandleRunningInstance(instance);
            }
        }

        /// <summary>
        /// 获取当前是否具有相同进程。
        /// </summary>
        /// <returns></returns>
        public static Process GetRunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历正在有相同名字运行的例程   
            foreach (Process process in processes)
            {
                //忽略现有的例程   
                if (process.Id != current.Id)
                    //确保例程从EXE文件运行 
                    if (System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                        return process;
            }
            return null;
        }
        /// <summary>
        /// 激活原有的进程。
        /// </summary>
        /// <param name="instance"></param>
        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, SW_MAXIMIZE);
            SetForegroundWindow(instance.MainWindowHandle);
        }

        private static void ShowWindowAsync(IntPtr mainWindowHandle, object wS_SHOWNORMAL)
        {
            throw new NotImplementedException();
        }

        static Assembly currentAssemblyCore;
        static Assembly CurrentAssembly
        {
            get
            {
                if (currentAssemblyCore == null)
                {
                    currentAssemblyCore = Assembly.GetExecutingAssembly();
                }
                return currentAssemblyCore;
            }
        }
        /*
        internal static List<Stream> CreateResourceStreams()
        {
            List<Stream> fileStreams = new List<Stream>();
            fileStreams.Add(DevExpress.Utils.ResourceImageHelper.FindStream("ConTest.Resources.ProgramText.rtf", CurrentAssembly));
            fileStreams.Add(DevExpress.Utils.ResourceImageHelper.FindStream("ConTest.Resources.ProgramText2.rtf", CurrentAssembly));
            fileStreams.Add(DevExpress.Utils.ResourceImageHelper.FindStream("ConTest.Resources.ProgramText3.rtf", CurrentAssembly));
            return fileStreams;
        }
        */
        internal static Stream GetDocumentStream(string fileName)
        {
            return DevExpress.Utils.ResourceImageHelper.FindStream(string.Format("ConTest.Resources.{0}.rtf", fileName), CurrentAssembly);
        }
    }
}
