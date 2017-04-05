﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using Pharos.EmbeddedIISExpress.Properties;
using System.Reflection;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace Pharos.EmbeddedIISExpress
{
    public class IISExpressSeverManager
    {
        private readonly string IISPATH = @"C:\Windows\System32\inetsrv";
        private readonly string IISPATHx64 = @"C:\Program Files\IIS Express\";
        private readonly string IISPATHx86 = @"C:\Program Files (x86)\IIS Express\";
        private readonly string CALLAPP = "iisexpress.exe";
        private readonly string MSIEXECQUIET = @"/i {0} /quiet";
        private readonly string SETSITEPATHFORMAT = "/path:{0} /port:{1} /clr:{2}";
        private readonly string DotNet40Path = "C:/Windows/Microsoft.NET/Framework64/v4.0.30319/aspnet_regiis.exe";
        private readonly string HISTORYPATH;
        private readonly string IISSETUP = @"C:\Windows\System32\OptionalFeatures.exe";
        public IISExpressSeverManager()
        {
            // var mydoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var assembly = Assembly.GetEntryAssembly().GetName().Name;
            HISTORYPATH = Path.Combine(@"C:\", "EmbeddedIISExpress", assembly);
        }

        public bool IsRunIIS { get; set; }
        private Process CurrentServer { get; set; }
        public void Run(int port, string sitePath, string clr = "v4.0", string iis = "IISExpress")
        {
            var historyPath = Path.Combine(HISTORYPATH, port.ToString());
            var exeCmd = "";
            var exePath = "";
            ProcessStartInfo processStartInfo;
            Console.WriteLine("正在写入目录权限......");
            exeCmd = string.Format("{0} /inheritance:e /grant:R everyone:F", sitePath);
            processStartInfo = new ProcessStartInfo("ICACLS", exeCmd);
            var process = Process.Start(processStartInfo);
            process.WaitForExit();
            exeCmd = string.Format("{0}* /inheritance:e /grant:R everyone:F", sitePath);
            processStartInfo = new ProcessStartInfo("ICACLS", exeCmd);
            process = Process.Start(processStartInfo);
            process.WaitForExit();




            Console.WriteLine("正在检查运行环境......");
            if (File.Exists(historyPath))
            {
                try
                {
                    var history = File.ReadAllText(historyPath);
                    CurrentServer = Process.GetProcessById(Convert.ToInt16(history));
                    if (CurrentServer != null)
                        return;
                }
                catch (Exception)
                {

                }
            }
            var appcmdPath = Path.Combine(IISPATH, "appcmd.exe");
        RunStart:
            if (File.Exists(appcmdPath) && ServiceController.GetServices().Any(o => o.ServiceName == "W3SVC"))
            {
                var service = new ServiceController("W3SVC");
                if (!(service.Status != ServiceControllerStatus.Running || service.Status != ServiceControllerStatus.ContinuePending || service.Status != ServiceControllerStatus.StartPending))
                    service.Start();
                exeCmd = string.Format("list site AutoSite{0}", port, sitePath);
                processStartInfo = new ProcessStartInfo(appcmdPath, exeCmd);
                processStartInfo.UseShellExecute = false;
                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.CreateNoWindow = true;
                process = Process.Start(processStartInfo);
                StreamReader reader = process.StandardOutput;
                string line = reader.ReadLine();
                process.WaitForExit();
                if (!string.IsNullOrEmpty(line))
                {
                    exeCmd = string.Format("APPCMD start SITE AutoSite{0}", port);
                    processStartInfo = new ProcessStartInfo(appcmdPath, exeCmd);
                    goto Start;
                }
                if (clr == "v4.0")
                {
                    Console.WriteLine("正在注册asp.net......");

                    exeCmd = "-i";
                    processStartInfo = new ProcessStartInfo(DotNet40Path, exeCmd);
                    process = Process.Start(processStartInfo);
                    process.WaitForExit();

                }
                exeCmd = string.Format("add site /name:AutoSite{0} /bindings:http/*:{0}: /physicalPath:{1}", port, sitePath);
                processStartInfo = new ProcessStartInfo(appcmdPath, exeCmd);
                var installProcess = Process.Start(processStartInfo);
                installProcess.WaitForExit();
                
                exeCmd = string.Format("add apppool /name:AutoSite{0}AppPool /managedRuntimeVersion:{1} /managedPipelineMode:Integrated", port, clr);
                processStartInfo = new ProcessStartInfo(appcmdPath, exeCmd);
                installProcess = Process.Start(processStartInfo);
                installProcess.WaitForExit();

                exeCmd = string.Format("set app AutoSite{0}/ -applicationPool:AutoSite{0}AppPool", port);
                processStartInfo = new ProcessStartInfo(appcmdPath, exeCmd);
                installProcess = Process.Start(processStartInfo);
                installProcess.WaitForExit();

                exeCmd = string.Format("APPCMD start SITE AutoSite{0}", port);
                processStartInfo = new ProcessStartInfo(appcmdPath, exeCmd);
                installProcess = Process.Start(processStartInfo);
                installProcess.WaitForExit();
                IsRunIIS = true;
            }
            else if (Environment.Is64BitOperatingSystem && Directory.Exists(IISPATHx64) && iis == "IISExpress")
            {
                exePath = Path.Combine(IISPATHx64, CALLAPP);
                exeCmd = string.Format(SETSITEPATHFORMAT, sitePath, port, clr);

                processStartInfo = new ProcessStartInfo(exePath, exeCmd);
            }
            else if (Directory.Exists(IISPATHx86) && iis == "IISExpress")
            {
                exePath = Path.Combine(IISPATHx86, CALLAPP);
                exeCmd = string.Format(SETSITEPATHFORMAT, sitePath, port, clr);
                processStartInfo = new ProcessStartInfo(exePath, exeCmd);

            }
            else
            {
                CallIISInstall(iis);
                goto RunStart;
            }
        Start:
            Console.WriteLine("正在启动站点");
            CurrentServer = Process.Start(processStartInfo);
            if (!Directory.Exists(historyPath))
            {
                Directory.CreateDirectory(historyPath);
            }
            File.WriteAllText(Path.Combine(historyPath, "history.data"), CurrentServer.Id.ToString());
            Console.WriteLine(string.Format("已启动站点http://localhost:{0}", port));

        }
        public void CallIISInstall(string iis)
        {
            if (iis == "IISExpress")
            {
                Console.WriteLine("正在安装IISExpress......");
                var tempPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".msi");
                File.WriteAllBytes(tempPath, Resources.iisexpress_1_11_x86_zh_CN);
                var exeCmd = string.Format(MSIEXECQUIET, tempPath);
                var processStartInfo = new ProcessStartInfo("msiexec", exeCmd);
                var installProcess = Process.Start(processStartInfo);
                installProcess.WaitForExit();
            }
            else if (iis == "IIS")
            {
                //if (Environment.UserInteractive)
                //{
                //    Console.WriteLine("正在安装IIS......");
                //    var processStartInfo = new ProcessStartInfo(IISSETUP, "");

                //    var installProcess = Process.Start(processStartInfo);
                //    installProcess.WaitForExit();
                //}
                
            }
        }
    }
}
