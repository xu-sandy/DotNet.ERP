using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.CRM.Retailing.Controllers
{
    /// <summary>
    /// 会员活动推送控制器
    /// </summary>
    public class PushController : BaseController
    {
        //
        // GET: /Push/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FindPageList()
        {
            int count;
            var list = PushService.FindPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult Save(int? id)
        {
            var model = new Push();
            if (id != null)
                model = PushService.FindById(id);
            ViewBag.type = ListToSelect(new Pharos.Logic.CommonDic().GetDicList(Logic.DicType.推送方式).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), "全部");
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(Push model, List<int> type)
        {
            if (model.State == 1)
                return Content(PushService.Push(model, type).ToJson());
            return Content(PushService.Save(model, type).ToJson());
        }

        public ActionResult Detail(int id)
        {
            var model = PushService.FindById(id);
            ViewBag.type = ListToSelect(new Pharos.Logic.CommonDic().GetDicList(Logic.DicType.推送方式).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), "全部");
            return View(model);
        }

        public ActionResult Delete(int[] ids)
        {
            return new JsonNetResult(PushService.Delete(ids));
        }

        [HttpPost]
        public ActionResult GetPushResult(string pushId, string typeStr)
        {
            int count;
            var list = PushService.GetPushResult(pushId, typeStr, Request.Params, out count);
            return ToDataGrid(list, count);
        }

        [HttpPost]
        public ActionResult PushNow(int[] ids)
        {
            return Content(PushService.PushNow(ids).ToJson());
        }
    }
}
