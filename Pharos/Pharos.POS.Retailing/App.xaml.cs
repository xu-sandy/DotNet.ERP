﻿using Pharos.POS.Retailing.Models;
using Pharos.Wpf.Extensions;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Runtime.InteropServices;
using Pharos.POS.Retailing.Devices.POSDevices;
using Pharos.Wpf;

namespace Pharos.POS.Retailing
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    HotKey.LoadConfig();
                }
                catch
                {
                    //this.Dispatcher.Invoke(new Action(() =>
                    //{
                    //    Toast.ShowMessage("快捷键设置异常，将使用默认快捷键设置！", Application.Current.MainWindow);
                    //}));

                    //Todo 恢复默认设置
                }
            });
            this.DefaultSettings();
            this.Startup += App_Startup;
        }




        void App_Startup(object sender, StartupEventArgs e)
        {

            SplashScreen s = new SplashScreen(@"\Images\Login\logo_max.png");
            s.Show(true);

            if (e.Args.Length == 0 || e.Args[0] != "Restart")
            {
                Global.MachineSettings.Load();
                
                var arr = System.Diagnostics.Process.GetProcessesByName("Pharos.POS.Retailing");
                var posapp = arr.OrderBy(o => o.StartTime).FirstOrDefault();
                if (posapp != null && arr.Count() != 1)
                {
                    Win32ForApplication.ShowWindowAsync(posapp.MainWindowHandle, Win32ForApplication.SW_SHOW);
                    Win32ForApplication.SetForegroundWindow(posapp.MainWindowHandle);
                    Application.Current.Shutdown();
                }
                if (Global.MachineSettings != null && Global.MachineSettings.Enable)
                {
                  //  this.StartupUri = new Uri("Login.xaml", UriKind.Relative);
                    Login page= new Login();
                    page.Show();
                }
                else
                {
                   // this.StartupUri = new Uri("MachineSettings.xaml", UriKind.Relative);
                    MachineSettings page = new MachineSettings();
                    page.Show();
                }
            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var win = Application.Current.MainWindow;
            Toast.ShowMessage(e.Exception.Message, null);
            e.Handled = true;
        }
    }

    public static class Win32ForApplication
    {
        /// <summary>
        /// 该函数设置由不同线程产生的窗口的显示状态。
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分。</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零。</returns>
        [DllImport("User32.dll")]
        internal static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>
        /// 该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。系统给创建前台窗口的线程分配的权限稍高于其他线程。
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄。</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。</returns>
        [DllImport("User32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);
        internal const int SW_SHOWNORMAL = 1;
        internal const int SW_SHOWNOACTIVATE = 4;
        internal const int SW_SHOW = 5;
    }
}
