using Pharos.Logic.OMS.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QCT.Pay.Admin.Controllers
{
    public class ReplyController : BaseController
    {
        [Ninject.Inject]
        PlanService PlanService { get; set; }
        [Ninject.Inject]
        DictionaryService DictionaryService { get; set; }
        [Ninject.Inject]
        UserService UserService { get; set; }
        //
        // GET: /Reply/

        public ActionResult Index()
        {
            ViewBag.users = ListToSelect(UserService.GetList(false).Select(o => new SelectListItem() { Value = o.UserId, Text = o.FullName }), emptyTitle: "全部");
            var dicts = DictionaryService.GetChildList(new List<int>() { 369, 370 });
            ViewBag.status = ListToSelect(dicts.Where(o => o.DicPSN == 370).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.types = ListToSelect(dicts.Where(o => o.DicPSN == 369).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult IndexPageList()
        {
            var count = 0;
            var nvl = new System.Collections.Specialized.NameValueCollection() { Request.Params };
            nvl.Add("detail", "1");
            var list = PlanService.MyReplyIndexPageList(nvl, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult MyIndex()
        {
            ViewBag.users = ListToSelect(UserService.GetList(false).Select(o => new SelectListItem() { Value = o.UserId, Text = o.FullName }), emptyTitle: "全部");
            var dicts = DictionaryService.GetChildList(new List<int>() { 369, 370 });
            ViewBag.status = ListToSelect(dicts.Where(o => o.DicPSN == 370).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.types = ListToSelect(dicts.Where(o => o.DicPSN == 369).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult MyIndexPageList()
        {
            var count = 0;
            var list = PlanService.MyReplyIndexPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
    }
}
