using Pharos.Logic.BLL;
using Pharos.Service.Retailing.Marketing;
using Pharos.Service.Retailing.RefreshCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Service.Retailing.RechargeGiftsCache;
using Pharos.Logic.MemberDomain.QuanChengTaoProviders;
using System.Diagnostics;
using System.ServiceProcess;

namespace Pharos.Service.Retailing
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
                        MarketingManageCenter.InitStoreMarketing();
                        Console.WriteLine("促销启动！");
                        ProductCacheManager.Subscribe();
                        Console.WriteLine("产品缓存变更订阅已启动！");
                        //RechargeGiftsManager.Start();
                        //Console.WriteLine("充值赠送已启动！");
                        QuanChengTaoIntegralRuleService.Run();
                        Console.WriteLine("积分规则运算已启动！");
                        Console.ReadLine();
                        break;
                }
            }
            else
            {
                ServiceBase[] servicesToRun;
                servicesToRun = new ServiceBase[] { new ERPService() };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
