﻿using Pharos.Logic.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.CRM.Retailing.Controllers
{
    public class DeviceController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.stores = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部");
            return View();
        }

        [HttpPost]
        public ActionResult FindPageList()
        {
            int count;
            var list = DeviceRegInfoService.FindPageList(out count);
            return ToDataGrid(list, count);
        }

        public ActionResult FindPageByWhere(int machineType, string store, int status, string keyword)
        {
            int count;
            var list = DeviceRegInfoService.FindPageListByWhere(machineType, store, status, keyword, out count);
            return ToDataGrid(list, count);
        }


        [HttpPost]
        public ActionResult SetDevState(string ids, short state)
        {
            var result = DeviceRegInfoService.SetDevState(ids, state);
            return new Pharos.Utility.JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult SaveMemo(int id, string memo)
        {
            var result = DeviceRegInfoService.SetMemo(id, memo);
            return new Pharos.Utility.JsonNetResult(result);
        }
    }
}
