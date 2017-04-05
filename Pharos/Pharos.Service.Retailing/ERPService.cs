using Pharos.Logic.MemberDomain.QuanChengTaoProviders;
using Pharos.Service.Retailing.Marketing;
using Pharos.Service.Retailing.RefreshCache;
using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace Pharos.Service.Retailing
{
    partial class ERPService : ServiceBase
    {
        public ERPService()
        {
            InitializeComponent();
            Disposed += ERPService_Disposed;
        }

        protected override void OnStart(string[] args)
        {
            MarketingManageCenter.InitStoreMarketing();
            ProductCacheManager.Subscribe();
            QuanChengTaoIntegralRuleService.Run();
        }

        protected override void OnStop()
        {
            base.OnStop();
            Dispose();
        }

        void ERPService_Disposed(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Dispose();
            Process.GetCurrentProcess().Kill();
        }


    }
}
