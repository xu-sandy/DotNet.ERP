﻿using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.OMS.Retailing.Controllers
{
    public class SysLimitController : BaseController
    {
        #region 私有对象
        [Ninject.Inject]
        SysLimitService LimitService { get; set; }
        
        #endregion
        //
        // GET: /SysLimit/
        [SysPermissionValidate]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = LimitService.FindPageList(out count);
            return ToDataGrid(list, count);
        }

        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            return new JsonNetResult(LimitService.Deletes(ids));
        }

        public ActionResult Save(int? limitid, int? pmenuId)
        {
            var obj = new SysLimits() { PLimitId = pmenuId.GetValueOrDefault(), Status = true };
            if (limitid.HasValue)
            {
                obj = LimitService.GetLimit(limitid.Value);
            }
            if (pmenuId.HasValue)
            {
                var pm = LimitService.GetMenu(pmenuId.Value);
                if (pm != null) ViewBag.ParentMenu = pm.Title;
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult Save(SysLimits obj)
        {
            var re = LimitService.SaveOrUpdate(obj);
            return new OpActionResult(re);
        }

        [HttpPost]
        public void MoveItem(short mode, int limitid)
        {
            LimitService.MoveItem(mode, limitid);
        }

        [HttpPost]
        public void SetState(int limitid, short status)
        {
            LimitService.SetState(limitid, status);
        }
        [HttpPost]
        public ActionResult Remove(int limitid)
        {
            return new JsonNetResult(LimitService.Remove(limitid));
        }
    }
}
