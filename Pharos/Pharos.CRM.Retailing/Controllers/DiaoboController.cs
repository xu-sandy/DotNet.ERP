﻿using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Sys;
namespace Pharos.CRM.Retailing.Controllers
{
    /// <summary>
    /// 门店调拨申请
    /// </summary>
    public class DiaoboController : BaseController
    {
        //
        // GET: /Diaobo/

        public ActionResult Index()
        {
            ViewBag.states = EnumToSelect(typeof(HouseMoveState), emptyTitle: "全部");
            ViewBag.CurStoreId = CurrentUser.StoreId;
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList(int page = 1, int rows = 30)
        {
            int count;
            var list = DiaoboService.FindPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult Save(string id)
        {
            var list = WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title });
            ViewBag.outshops = list.Where(o => o.Value != Sys.CurrentUser.StoreId).ToList();
            ViewBag.inshops = list.Where(o => o.Value == Sys.CurrentUser.StoreId).ToList();
            var obj = new STHouseMove();
            if (!id.IsNullOrEmpty())
            {
                obj = DiaoboService.FindById(id);
                var pro = BaseService<VwProduct>.Find(o =>o.CompanyId==CommonService.CompanyId && o.Barcode == obj.Barcode);
                if (pro != null)
                {
                    ViewData["ProductTitle"] = pro.Title;
                    ViewData["SysPrice"] = pro.SysPrice;
                    ViewData["StockCount"] = pro.StockNums;
                }
            }
            else {
                obj.OrderQuantity = 1;
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult Save(STHouseMove obj)
        {
            var re = DiaoboService.SaveOrUpdate(obj);
            return Content(re.ToJson());
        }
        public ActionResult Detail(string id)
        {
            var obj = DiaoboService.FindAllById(id);
            return View(obj.IsNullThrow());
        }
        public ActionResult Handler(string id)
        {
            var obj = DiaoboService.FindAllById(id);
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult Handler(STHouseMove obj)
        {
            var house= DiaoboService.FindById(obj.Id);
            house.State = obj.State;
            house.DeliveryUID = Sys.CurrentUser.UID;
            house.DeliveryQuantity = obj.DeliveryQuantity;
            house.StockOut = obj.StockOut;
            var re = DiaoboService.Update(house);
            return Content(re.ToJson());
        }
        public ActionResult Receiver(string id)
        {
            var obj = DiaoboService.FindAllById(id);
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult Receiver(STHouseMove obj)
        {
            var re = DiaoboService.Receiver(obj);
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult ReBack(string ids)
        {
            var sid = ids.Split(',');
            var houses = DiaoboService.FindList(o=>sid.Contains(o.Id));
            houses.Each(o =>
            {
                o.State = (short)HouseMoveState.已撤回;
            });
            var re = DiaoboService.Update(houses);
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult Delete(string[] ids)
        {
            var re = new OpResult() { Successed = true };
            var list= DiaoboService.FindList(o => ids.Contains(o.Id));
            re = DiaoboService.Delete(list);
            return new JsonNetResult(re);
        }
        [HttpPost]
        public ActionResult ListPartial(string barcode)
        {
            var dicts = CommodityService.GetStockNums(barcode);
            return PartialView(dicts);
        }
    }
}
