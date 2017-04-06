﻿using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Logic;
namespace Pharos.Store.Retailing.Controllers
{
    public class KunBangXiaoShouController : BaseController
    {
        //
        // GET: /KunBangXiaoShou/

        public ActionResult Index()
        {
            ViewBag.states = EnumToSelect(typeof(SaleState), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = CommodityPromotionService.KubangPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        [HttpPost]
        public ActionResult LoadDetailList(string zhekouId)
        {
            int count = 0;
            var list = CommodityPromotionService.FindKubangDetailsById(zhekouId, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult Save(int? id)
        {
            ViewBag.customers = EnumToSelect(typeof(CustomerType), selectValue: 0);
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部", emptyValue: "-1");
            ViewBag.times = CommonRules.TimeLines;
            var obj = new PromotionBundling();
            if (id.HasValue)
            {
                var detail = BaseService<BundlingList>.FindById(id);
                obj = CommodityPromotionService.FindKuBangById(detail.CommodityId);
                if(obj.Timeliness==1)
                    ViewBag.times = new List<string>() { obj.StartAging1, obj.EndAging1, obj.StartAging2, obj.EndAging2, obj.StartAging3, obj.EndAging3 };
            }
            return View(obj.IsNullThrow());
        }
        #region 门店后台不让修改促销相关信息
        //[HttpPost]
        //[SysPermissionValidate(Code = Sys.SysConstLimits.促销管理_创建促销)]
        //public ActionResult Save(PromotionBundling obj)
        //{
        //    obj.StoreId = Request["StoreId"];
        //    obj.GenerateCode = true;//自动生成新捆绑条码
        //    var re = CommodityPromotionService.KubangSaveOrUpdate(obj, Request["Times"]);
        //    return Content(re.ToJson());
        //}
        //[HttpPost]
        //[SysPermissionValidate(Code = Sys.SysConstLimits.促销管理_移除促销)]
        //public ActionResult Delete(string[] ids)
        //{
        //    var re = BundlingService.DeleteById(ids);
        //    return new JsonNetResult(re);
        //}
        //[HttpPost]
        //[SysPermissionValidate(Code = Sys.SysConstLimits.促销管理_状态设定)]
        //public ActionResult SetState(string ids, short state)
        //{
        //    var sid = ids.Split(',').Distinct();
        //    var list = CommodityPromotionService.FindList(o => sid.Contains(o.Id));
        //    list.ForEach(o => { o.State = state; });
        //    var re = CommodityPromotionService.Update(list);
        //    if (re.Successed)
        //    {
        //        var stores = string.Join(",", list.Select(o => o.StoreId).Distinct());
        //        //Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = Sys.SysCommonRules.CompanyId, StoreId = stores, Target = "CommodityBundlingPackage" });
        //    }
        //    return new JsonNetResult(re);
        //} 
        #endregion
        public ActionResult Detail(int? id, string commodityId)
        {
            ViewBag.customers = EnumToSelect(typeof(CustomerType), selectValue: 0);
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部");
            ViewBag.times = CommonRules.TimeLines;
            var obj = new PromotionBundling();
            if (id.HasValue)
            {
                var detail = BaseService<BundlingList>.FindById(id);
                obj = CommodityPromotionService.FindKuBangById(detail.CommodityId);
                if (obj.Timeliness == 1)
                    ViewBag.times = new List<string>() { obj.StartAging1, obj.EndAging1, obj.StartAging2, obj.EndAging2, obj.StartAging3, obj.EndAging3 };
            }
            else
            {
                if (!string.IsNullOrEmpty(commodityId))
                {
                    obj = CommodityPromotionService.FindKuBangById(commodityId);
                    if (obj.Timeliness == 1)
                        ViewBag.times = new List<string>() { obj.StartAging1, obj.EndAging1, obj.StartAging2, obj.EndAging2, obj.StartAging3, obj.EndAging3 };
                }
            }
            return View(obj.IsNullThrow());
        }
    }
}