using Pharos.Logic.OMS;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.OMS.Retailing.Controllers
{
    public class HomeController : Controller
    {
        [Ninject.Inject]
        MenuService MenuService { get; set; }
        [Ninject.Inject]
        ProductPublishVerService PublishVerService { get; set; }
        public ActionResult Index()
        {
            if (!CurrentUser.IsLogin)
            {
                return RedirectToAction("Login", "SysUser");
            }     
            //todo: 模拟数据
            string mode = Request["mode"];
            ViewBag.accessCount = 0;
            ViewBag.WelcomeText = "欢迎光临";
            ViewBag.CurUserName = CurrentUser.FullName;
            ViewBag.CurLoginName = CurrentUser.UserName;
            var menus = MenuService.GetUserMenus(CurrentUser.UID);
            var ms= menus.GroupBy(o => new { o.MenuId, o.PMenuId, o.Title, o.URL,o.SortOrder }).Select(o=>new{o.Key.MenuId,o.Key.PMenuId,o.Key.Title,o.Key.URL,o.Key.SortOrder});
            var list = new List<MenuModel>();
            foreach (var m in ms.Where(o => o.PMenuId == 0))
            {
                var memu = new MenuModel() { Id = m.MenuId.ToString(), Value = m.MenuId.ToString(), Name = m.Title, Level = 0 };
                memu.Children = new List<MenuModel>();
                foreach (var cm in ms.Where(o => o.PMenuId == m.MenuId).OrderBy(o => o.SortOrder))
                    memu.Children.Add(new MenuModel() { Id = cm.MenuId.ToString(), Value = cm.MenuId.ToString(), Name = cm.Title, Url = cm.URL, Level = 1 });
                list.Add(memu);                
            }
            ViewBag.Menus = list;
            return View();
        }
        /// <summary>
        /// 系统管理-页面错误返回页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetHasPublish()
        {
            var ver= PublishVerService.GetHasPublish(0, 1);
            return Content(ver);
        }
        [HttpPost]
        public ActionResult UpdateData()
        {
            return new OpActionResult(MenuService.UpdateData());
        }
    }
}
