using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Sys;

namespace Pharos.CMS.Retailing.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            ViewBag.WelcomeText = "欢迎光临";
            var list = new List<Pharos.Sys.Models.MenuModel>();

            var memu = new Pharos.Sys.Models.MenuModel() { Id = "10", Value = "10", Name = "内容管理", Level = 0 };
            memu.Children = new List<Pharos.Sys.Models.MenuModel>()
            {
                new Pharos.Sys.Models.MenuModel(){Id="11",Value="11",Name="信息列表",Url=Url.Action("Index", "Contents"),Level=1}
            };            
            list.Add(memu);

            var memu2 = new Pharos.Sys.Models.MenuModel() { Id = "20", Value = "20", Name = "网站配置", Level = 0 };
            memu2.Children = new List<Pharos.Sys.Models.MenuModel>()
            {
                new Pharos.Sys.Models.MenuModel(){Id="21",Value="21",Name="栏目分类",Url=Url.Action("Category", "Setting"),Level=1}
            };                   
            list.Add(memu2);

            ViewBag.Menus = list;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
