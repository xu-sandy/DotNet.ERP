﻿using Pharos.Logic.BLL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Logic.Entity;

namespace Pharos.CRM.Retailing.Controllers
{
    /// <summary>
    /// 销售管理控制器
    /// </summary>
    public class SaleManagementController : BaseController
    {
        #region 修改订单
        public ActionResult Index()
        {
            ViewBag.cashiers = ListToSelect(SaleOrdersService.GetCashiers(), emptyTitle: "全部");
            ViewBag.salers = ListToSelect(SaleOrdersService.GetSalers(), emptyTitle: "全部");
            ViewBag.stores = ListToSelect(WarehouseService.GetList(true).Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title, Selected = Sys.CurrentUser.StoreId == o.StoreId }), emptyTitle: "全部");
            ViewBag.apiCodes = ListToSelect(ApiLibraryService.GetAllApiLibrary().Select(a => new SelectListItem() { Value = a.ApiCode.ToString(), Text = a.Title }), emptyTitle: "全部");
            return View();
        }

        [HttpPost]
        public ActionResult FindPageList()
        {
            int count;
            object footer;
            var list = SaleOrdersService.QuerySaleOrdersPageList(Request.Params, out count, out footer);
            return ToDataGrid(list, count, footer);
        }

        public ActionResult SaleOrderDetail(string paySN)
        {
            //var model = SaleOrdersService.Find(a => a.CompanyId == CommonService.CompanyId && a.PaySN == paySN);
            
            //var temp = UserInfoService.Find(a => a.UID == model.CreateUID);
            //ViewBag.cashier = temp == null ? "" : temp.FullName;
            //temp = UserInfoService.Find(a => a.UID == model.Salesman);
            //ViewBag.shoppingGuide = temp == null ? "" : temp.FullName; ;
            //ViewBag.store = WarehouseService.IsExist(a => a.CompanyId == CommonService.CompanyId && a.StoreId == model.StoreId) ? WarehouseService.Find(a => a.CompanyId == CommonService.CompanyId && a.StoreId == model.StoreId).Title : "";
            ViewBag.salesClassify = ListToSelect(SysDataDictService.GetDictionaryList(Logic.DicType.销售分类).Select(a => new SelectListItem() { Value = a.DicSN.ToString(), Text = a.Title }));
            //ViewBag.thirdPartyTradeNo = ConsumptionPaymentService.IsExist(a => a.PaySN == paySN) ? ConsumptionPaymentService.Find(a => a.PaySN == paySN).ApiOrderSN : "";
            //ViewBag.thirdPartyTradeNo = PayNotifyResultService.IsExist(a => a.CompanyId == CommonService.CompanyId && a.PaySN == paySN) ? PayNotifyResultService.Find(a => a.CompanyId == CommonService.CompanyId && a.PaySN == paySN).TradeNo : "";
            var obj = SaleOrdersService.GetDetailBypaysn(paySN);
            return View(obj);
        }
        [HttpPost]
        public ActionResult GetSaleOrderDetails(string paySN)
        {
            object footer = null;
            var list = SaleDetailService.GetSaleDetailsWithReturnFlag(paySN, ref footer);
            return new JsonNetResult(new { rows = list, footer = footer });
        }
        [HttpPost]
        public ActionResult GetConsumptionPayments(string paySN)
        {
            var list = SaleDetailService.GetConsumptionPayments(paySN);
            return ToDataGrid(list, 0);
        }

        [HttpPost]
        public ActionResult SaleOrderDetail(string paySN, decimal preferentialPrice, decimal totalAmount, string changedDetails)
        {
            return Content("修改订单功能已关闭");
            var result = SaleOrdersService.UpdateSaleOrder(paySN, preferentialPrice, totalAmount, changedDetails, Sys.CurrentUser.UID);
            return Content(result.ToJson());
        }
        #endregion
    }
}
