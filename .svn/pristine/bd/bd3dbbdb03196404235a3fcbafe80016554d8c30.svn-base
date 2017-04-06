using Pharos.Logic.OMS;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QCT.Pay.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Ninject.Inject]
        MenuService MenuService { get; set; }
        public ActionResult Index()
        {
            if (!CurrentUser.IsLogin)
            {
                return RedirectToAction("Login", "User");
            }     
            //todo: 模拟数据
            string mode = Request["mode"];
            ViewBag.accessCount = 0;
            ViewBag.WelcomeText = "欢迎光临";
            ViewBag.CurUserName = CurrentUser.FullName;
            ViewBag.CurLoginName = CurrentUser.UserName;
            var menus = MenuService.GetUserMenus(CurrentUser.UID);
            var list = new List<MenuModel>();
            foreach (var m in menus.Where(o => o.PMenuId == 0))
            {
                var memu = new MenuModel() { Id = m.MenuId.ToString(), Value = m.MenuId.ToString(), Name = m.Title, Level = 0 };
                memu.Children = new List<MenuModel>();
                foreach(var cm in menus.Where(o=>o.PMenuId==m.MenuId).OrderBy(o=>o.SortOrder))
                    memu.Children.Add(new MenuModel() { Id = cm.MenuId.ToString(), Value = cm.MenuId.ToString(), Name = cm.Title, Url = cm.URL, Level = 1 });
                list.Add(memu);                
            }
            ViewBag.Menus = list;
            return View();
        }

    }
}
