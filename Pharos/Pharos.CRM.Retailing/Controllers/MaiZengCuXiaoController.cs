﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Logic;
namespace Pharos.CRM.Retailing.Controllers
{
    public class MaiZengCuXiaoController : BaseController
    {
        //
        // GET: /MaiZengCuXiao/

        public ActionResult Index()
        {
            ViewBag.states = EnumToSelect(typeof(SaleState), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList(int page = 1, int rows = 30)
        {
            int count = 0;
            var list = CommodityPromotionService.MaiZengPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        [HttpPost]
        public ActionResult LoadDetailList(string zhekouId,short type)
        {
            int count = 0;
            var list = CommodityPromotionService.FindManZengDetailsById(zhekouId,type,out count);
            return ToDataGrid(list, count);
        }
        public ActionResult Save(string id)
        {
            ViewBag.customers = EnumToSelect(typeof(CustomerType), selectValue: 0);
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部", emptyValue: "-1");
            ViewBag.times = CommonRules.TimeLines;
            var parents = ProductCategoryService.GetParentTypes().Select(o => new DropdownItem() { Value = o.CategorySN.ToString(), Text = o.Title }).ToList();
            ViewBag.parenttypes = parents.ToJson();
            parents = ProductBrandService.GetList().Select(o => new DropdownItem(o.BrandSN.ToString(), o.Title)).ToList();
            ViewBag.brands = parents.ToJson();
            var obj = new CommodityPromotion();
            if (!id.IsNullOrEmpty())
            {
                obj = CommodityPromotionService.FindById(id);
                if (obj.Timeliness == 1)
                    ViewBag.times = new List<string>() { obj.StartAging1, obj.EndAging1, obj.StartAging2, obj.EndAging2, obj.StartAging3, obj.EndAging3 };
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateInput(false)]
        [SysPermissionValidate(Code = Sys.SysConstLimits.促销管理_创建促销)]
        public ActionResult Save(CommodityPromotion obj)
        {
            obj.StoreId = Request["StoreId"];
            var re = CommodityPromotionService.MaiZengSaveOrUpdate(obj, Request["Times"]);
            return Content(re.ToJson());
        }
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.促销管理_移除促销)]
        public ActionResult Delete(string[] ids)
        {
            var re = FreeGiftService.DeleteById(ids);
            return new JsonNetResult(re);
        }
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.促销管理_状态设定)]
        public ActionResult SetState(string ids, short state)
        {
            var sid = ids.Split(',').Distinct();
            var list = CommodityPromotionService.FindList(o => sid.Contains(o.Id));
            list.ForEach(o => { o.State = state; });
            var re = CommodityPromotionService.Update(list);
            if (re.Successed)
            {
                var stores = string.Join(",", list.Select(o => o.StoreId).Distinct());
                Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = Sys.SysCommonRules.CompanyId, StoreId = stores, Target = "CommodityFreeGiftPackage" });
            }
            return new JsonNetResult(re);
        }
        public ActionResult Detail(string id)
        {
            ViewBag.customers = EnumToSelect(typeof(CustomerType), selectValue: 0);
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部");
            ViewBag.times = CommonRules.TimeLines;
            var obj = new CommodityPromotion();
            if (!id.IsNullOrEmpty())
            {
                obj = CommodityPromotionService.FindById(id);
                if (obj.Timeliness == 1)
                    ViewBag.times = new List<string>() { obj.StartAging1, obj.EndAging1, obj.StartAging2, obj.EndAging2, obj.StartAging3, obj.EndAging3 };
            }
            return View(obj);
        }
    }
}
