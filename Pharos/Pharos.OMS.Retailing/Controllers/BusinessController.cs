﻿using System.Linq;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.BLL;

namespace Pharos.OMS.Retailing.Controllers
{
    public class BusinessController : BaseController
    {
        //
        // GET: /Brand/
        [Ninject.Inject]
        BusinessService BusinessService { get; set; }
        [SysPermissionValidate]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FindPageList(string title, int? classfyId)
        {
            var count = 0;
            var list = BusinessService.GetTreeList(Request.Params);
            return ToDataGrid(list, count);
        }
        [HttpPost]
        [SysPermissionValidate(205)]
        public ActionResult Delete(int[] ids)
        {
            var op = BusinessService.Deletes(ids);
            return new OpActionResult(op);
        }
        //新增品牌
        [SysPermissionValidate(204)]
        public ActionResult Save(int? id)
        {
            var obj = new Business();
            if (id.HasValue)
            {
                obj = BusinessService.GetOne(id.Value);
            }
            ViewBag.parents =ListToSelect( BusinessService.GetParentList(false).Select(o=>new SelectListItem(){Text=o.Title,Value=o.ById}),emptyTitle:"请选择");
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult Save(Business obj)
        {
            var op = BusinessService.SaveOrUpdate(obj);
            return new OpActionResult(op);
        }
        [HttpPost]
        public ActionResult SetState(string ids, short state)
        {
            var op = BusinessService.SetState(ids, state);
            return new JsonNetResult(op);
        }
    }
}
