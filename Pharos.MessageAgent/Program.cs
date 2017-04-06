using System;
using System.ServiceProcess;

namespace Pharos.MessageAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            if (System.Environment.UserInteractive)
            {
                string parameter = string.Concat(args);
                if (args == null || args.Length == 0)
                {
                    Console.WriteLine("请输入操作指令（-i、-u、-start、-stop）:");
                    parameter = Console.ReadLine();
                }
                switch (parameter)
                {
                    case "-i":
                        ServiceHelper.InstallWindowsService();
                        break;
                    case "-u":
                        ServiceHelper.UninstallWindowsService();
                        break;
                    case "-start":
                        ServiceHelper.StartService();
                        break;
                    case "-stop":
                        ServiceHelper.StopService();
                        break;
                    case "-debug":
                        var service = new MessagingAgentServerService();
                        service.Start();
                        Console.WriteLine("回车关闭服务！");
                        Console.ReadLine();
                        service.Close();

                        break;
                }
            }
            else
            {
                ServiceBase[] servicesToRun;
                servicesToRun = new ServiceBase[] { new MessagingAgentServerService() };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
