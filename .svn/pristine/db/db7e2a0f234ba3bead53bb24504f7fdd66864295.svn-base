using Pharos.Logic.ApiData.Pos;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Pharos.Api.Retailing
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        static void ConfigureApi(HttpConfiguration config)
        {
            config.Filters.Add(new ApiExceptionFilterAttribute());
            config.Filters.Add(new ResponseFilterAttribute());
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            ConfigureApi(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            StoreManager.Init();
            //MessageCenter.Sub();
        }
    }

}