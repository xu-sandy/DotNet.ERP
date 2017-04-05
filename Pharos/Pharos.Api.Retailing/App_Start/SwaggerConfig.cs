using System.Web.Http;
#if(Local!= true)
using WebActivatorEx;
#endif
using Pharos.Api.Retailing;
using Swashbuckle.Application;
using System.IO;
using System.Reflection;
using System.Linq;
using System;
using System.Configuration;
#if(Local!= true)
[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]
#endif
namespace Pharos.Api.Retailing
{
    public class SwaggerConfig
    {
#if(Local!= true)
        public static void Register()
#endif
#if(Local==true)
        public static void Register(HttpConfiguration config)
#endif
        {

            if (ConfigurationManager.AppSettings.AllKeys.Any(o => o == "HelpMode") && ConfigurationManager.AppSettings["HelpMode"] == "1")
            {
                var thisAssembly = typeof(SwaggerConfig).Assembly;
#if(Local!=true)
                GlobalConfiguration.Configuration
#endif
#if(Local==true)
            config
#endif
.EnableSwagger(c =>
    {

        c.SingleApiVersion(Assembly.GetExecutingAssembly().GetName().Version.ToString(), Assembly.GetExecutingAssembly().GetName().Name);

        c.IncludeXmlComments(GetXmlCommentsPath());

        c.DescribeAllEnumsAsStrings();

    }).EnableSwaggerUi();
            }
        }

        private static string GetXmlCommentsPath()
        {
#if(Local != true)
            var commentsFile = string.Format(@"{1}BIN\{0}.XML", Assembly.GetExecutingAssembly().GetName().Name, AppDomain.CurrentDomain.BaseDirectory);
#endif
#if(Local == true)
            var commentsFile = string.Format(@"{1}\{0}.XML", Assembly.GetExecutingAssembly().GetName().Name, AppDomain.CurrentDomain.BaseDirectory);
#endif
            return commentsFile;
        }
    }
}
