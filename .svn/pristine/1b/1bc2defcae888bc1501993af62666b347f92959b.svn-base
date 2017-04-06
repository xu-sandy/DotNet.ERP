using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharos.EmbeddedIISExpress;

namespace Pharos.POS.ClientService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            if (Environment.UserInteractive)
            {
                string parameter = string.Concat(args);
                switch (parameter)
                {
                    case "-i":
                        ServiceHelper.InstallWindowsService();
                        goto RunIIS;
                    case "-u":
                        ServiceHelper.UninstallWindowsService();
                        return;
                    case "-start":
                        ServiceHelper.StartService();
                        goto RunIIS;
                    case "-stop":
                        ServiceHelper.StopService();
                        break;
                    default:
                        var status = ServiceHelper.CheckServiceStatus();
                        if (status == null)
                        {
                            ServiceHelper.InstallWindowsService();

                        }
                        else if (status != ServiceControllerStatus.Running)
                        {
                            ServiceHelper.StartService();
                        }
                RunIIS:
                        {
                            POSService.storeManagerThread = Thread.CurrentThread;
                            IISExpressSeverManager manager = new IISExpressSeverManager();
                            var config = StoreManageCenterConfig.GetConfig();
                            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Store");
                            manager.Run(config.Port, path, "v4.0", "IIS");
                            break;
                        }

                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                ServiceBase[] servicesToRun;
                servicesToRun = new ServiceBase[] { new POSService() };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
