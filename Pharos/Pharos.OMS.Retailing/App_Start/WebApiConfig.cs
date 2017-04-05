using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Pharos.OMS.Retailing
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Filters.Add(new ResponseFilterAttribute());
            config.DependencyResolver = new NinjectDependencyResolverForWebApi();

            //响应返回 application/ json
            config.Formatters.Clear();
            var jsonFormatter = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            var settings = jsonFormatter.SerializerSettings;
            var timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            //这里使用自定义日期格式
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm";
            settings.Converters.Add(timeConverter);
            config.Formatters.Add(jsonFormatter);
            // 返回 Json 设为大小写 Camel 驼峰形式
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        }
    }
}
