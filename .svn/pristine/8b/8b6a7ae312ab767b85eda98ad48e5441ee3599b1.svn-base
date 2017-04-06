using Pharos.Logic;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;

namespace Pharos.CRM.Retailing.Controllers
{
    public class NoticeController : BaseController
    {
        //
        // GET: /Notice/

        public ActionResult Index()
        {
            return View();
        }

        //公告管理
        public ActionResult NoticeManagement()
        {
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部");
            return View();
        }

        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = NoticeService.FindPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }

        //修改公告
        public ActionResult Save(long? id)
        {
            var model = new Notice();
            if (id != null)
            {
                model = NoticeService.FindById(id);
            }
            else
            {
                model.BeginDate = DateTime.Now;
                model.ExpirationDate = DateTime.Now.AddDays(5);
            }
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部",emptyValue:"-1");
            ViewBag.state = EnumToSelect(typeof(NoticeState), selectValue: 0);
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(Notice obj)
        {
            obj.StoreId = Request["StoreId"];
            var re = NoticeService.NoticeSaveOrUpdate(obj);
            return Content(re.ToJson());
        }


        [HttpPost]
        public ActionResult Delete(long[] ids)
        {
            var re = NoticeService.DeleteByIds(ids);
            return new JsonNetResult(re);
        }

        [HttpPost]
        public ActionResult ChangeState(long[] ids, int type)
        {
            var re = NoticeService.ChangeState(ids, type);
            return new JsonNetResult(re);
        }

        //公告详情
        public ActionResult Detail(long? id)
        {
            var model = new Notice();
            if (id != null)
            {
                model = NoticeService.FindById(id);
            }
            else
            {
                model.BeginDate = DateTime.Now;
                model.ExpirationDate = DateTime.Now.AddDays(5);
            }
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部");
            ViewBag.state = EnumToSelect(typeof(NoticeState), selectValue: 0);
            return View(model);
        }
    }
}
