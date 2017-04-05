using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Pharos.Store.Retailing
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Clear();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            routes.Add(
               "DomainRoute", new DomainRoute(
               "{dom}.{d1}.{d0}",
               "{controller}/{action}/{id}",
               new { controller = "Store", action = "Login", id = UrlParameter.Optional }
            ));

            routes.Add(
                "StoreRoute", new DomainRoute(
               "store{cid}-{sid}.{d1}.{d0}",
               "{controller}/{action}/{id}",
              new { controller = "Store", action = "Login", id = UrlParameter.Optional }
            ));

            routes.Add(
                "StoreIpRoute", new DomainRoute(
                "{ip1}.{ip2}.{ip3}.{ip4}",
                //"store{cid}-{sid}/{controller}/{action}/{id}",
                "{controller}/{action}/{id}",
                new { controller = "Store", action = "Login", id = UrlParameter.Optional }
            ));

            routes.Add(
                "StoreLocalhostRoute", new DomainRoute(
                "{localhost}",
                "store{cid}-{sid}/{controller}/{action}/{id}",
                new { controller = "Store", action = "Login", id = UrlParameter.Optional }
            ));
        }
    }
}