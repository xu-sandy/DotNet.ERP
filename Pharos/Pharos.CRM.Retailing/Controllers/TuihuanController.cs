﻿using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic.BLL;
using Pharos.Logic;
using Pharos.Logic.Entity;
namespace Pharos.CRM.Retailing.Controllers
{
    /// <summary>
    /// 门店退换货
    /// </summary>
    public class TuihuanController : BaseController
    {
        //
        // GET: /Tuihuan/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList(int page = 1, int rows = 30)
        {
            int count;
            var list = TuihuanService.FindPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult Huang(string id)
        {
            ViewBag.reasons = ListToSelect(SysDataDictService.GetReasonTitle().Select(o => new SelectListItem() { Text = o.Title, Value = o.DicSN.ToString() }));
            return View("HuangNew");
        }
        [HttpPost]
        public ActionResult Huang(int reason, decimal? retprice, string insertolded, string payno)
        {
            var re = TuihuanService.SaveHuang(reason, retprice, insertolded, payno);
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult HuanProduct(string barcode)
        {
            var list = TuihuanService.HuanProduct(barcode);
            return new JsonNetResult(list);
        }
        public ActionResult Tui(string id)
        {
            ViewBag.reasons = ListToSelect(SysDataDictService.GetReasonTitle().Select(o => new SelectListItem() { Text = o.Title, Value = o.DicSN.ToString() }));
            return View("TuiNew");
        }
        [HttpPost]
        public ActionResult Tui(int reason, decimal? retprice, string insertolded, string payno)
        {
            var re = TuihuanService.SaveTui(reason, retprice, insertolded, payno);
            return Content(re.ToJson());
        }
        public ActionResult TuiZhengDan(string id)
        {
            ViewBag.reasons = ListToSelect(SysDataDictService.GetReasonTitle().Select(o => new SelectListItem() { Text = o.Title, Value = o.DicSN.ToString() }));
            return View();
        }
        [HttpPost]
        public ActionResult SaveTuiZhengDan(int reason, decimal? retprice, string insertolded, string payno)
        {
            var re = TuihuanService.SaveTui(reason, retprice, insertolded, payno, true);
            return Content(re.ToJson());
        }
        public ActionResult FindOldPageList(string barcode, string paysn)
        {
            //var list = TuihuanService.FindOldPageList(barcode, paysn);
            return ToDataGrid(null, 0);
        }
        public ActionResult FindNewPageList(string barcode)
        {
            //var list = TuihuanService.FindNewPageList(barcode);
            return ToDataGrid(null, 0);
        }
        [HttpPost]
        public ActionResult Delete(long[] ids)
        {
            var re = new OpResult() { Successed = true };
            var list = TuihuanService.FindList(o => ids.Contains(o.Id));
            re = TuihuanService.Delete(list);
            return new JsonNetResult(re);
        }
        [HttpPost]
        public ActionResult GetSaleDetailInput(string paySn, string barcode)
        {
            if (paySn.IsNullOrEmpty() || barcode.IsNullOrEmpty()) return new EmptyResult();
            var list = SaleDetailService.GetSaleDetailInput(paySn, barcode);
            return ToDataGrid(list, 20);
        }
        [HttpPost]
        public ActionResult SetFinish(string ids)
        {
            var sid = ids.Split(',').Select(o => long.Parse(o));
            var list = TuihuanService.FindList(o => sid.Contains(o.Id));
            list.Each(o => { o.State = 2; });
            var op = TuihuanService.Update(list);
            return new JsonNetResult(op);
        }

        /// <summary>
        /// 输入自动完成商品
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSaleOrderInput(string searchName, int? maxRows)
        {
            if (searchName.IsNullOrEmpty()) return new EmptyResult();
            var express = DynamicallyLinqHelper.True<SaleOrders>().And(o => o.PaySN != null && o.PaySN.Contains(searchName) && o.State==0 && o.Type==0 && o.CompanyId==CommonService.CompanyId);
            var list = BaseService<SaleOrders>.FindList(express, takeNum: maxRows.HasValue ? maxRows.Value : 20);
            return ToDataGrid(list, 0);
        }
        /// <summary>
        /// 由流水号获取销售明细
        /// 商品退回明细编辑页 /Tuihuan/Tui
        /// </summary>
        /// <param name="paySN"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSaleDetailByPaySN(string paysn)
        {
            var list = TuihuanService.GetDetailsToTuiHuan(paysn);
            return ToDataGrid(list, 0);
        }
        [HttpPost]
        public ActionResult GetProductInput(string searchName)
        {
            if (searchName.IsNullOrEmpty()) return new EmptyResult();
            var list = TuihuanService.GetBarcodeInput(searchName);
            return ToDataGrid(list, 20);
        }
    }
}