//using Pharos.Api.Retailing.Areas.HelpPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Routing;
#if(!Local)
using Pharos.MessageTransferAgenClient.WebPackageHandler;
#endif
#if Local
using Pharos.POS.ClientService;
#endif

namespace Pharos.Api.Retailing
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
#if(!Local)
            config.Routes.MapHttpRoute(
                name: "DomainEvent",
                routeTemplate: "Multiflora/MessageTransferAgent",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                 handler: new WebPackageHandler()
                );
#endif
#if Local
            SwaggerConfig.Register(config);

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{*assetPath}",
                defaults: new { assetPath = "Index.html", id = RouteParameter.Optional },
                constraints: new { assetPath = @".+" },
                handler: new RequestFileHandler()
                );
#endif
        }
    }
}
