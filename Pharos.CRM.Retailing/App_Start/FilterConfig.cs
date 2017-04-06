using System.Web;
using System.Web.Mvc;

namespace Pharos.CRM.Retailing
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}