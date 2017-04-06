using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Pharos.OMS.Retailing
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : Ninject.Web.Common.NinjectHttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted(); 
            Application_Start();
        }
        protected override Ninject.IKernel CreateKernel()
        {
            var factory = new NinjectControllerFactory();
            ControllerBuilder.Current.SetControllerFactory(factory);
            factory.ninjectKernel.Bind<Pharos.OMS.Retailing.Sync.IService1>().To<Pharos.OMS.Retailing.Sync.Service1>();
            return factory.ninjectKernel;
        }
        //保证同个SessionId
        void Session_Start(object sender, EventArgs e)
        {
        }
        void Session_End(object sender, EventArgs e)
        {
        }
    }
}