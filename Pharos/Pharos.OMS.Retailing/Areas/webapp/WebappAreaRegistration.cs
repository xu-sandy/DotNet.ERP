using System.Web.Mvc;

namespace Pharos.OMS.Retailing.Areas.webapp
{
    public class WebappAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "webapp";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "webapp_default",
                "webapp/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
