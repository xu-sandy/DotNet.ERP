﻿using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Logic;
using System.Data;
using Pharos.Sys.BLL;
using Pharos.Sys.Entity;
namespace Pharos.CRM.Retailing.Controllers
{
    //盘点管理
    public class TakestockController : BaseController
    {
        //
        // GET: /Takestock/
        private readonly SysUserInfoBLL _userBLL = new SysUserInfoBLL();
        public ActionResult Index()
        {
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "请选择");
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var footer = new object();
            var list = TakeStockService.FindPageList(Request.Params, out count, ref footer);
            //var l = list.FirstOrDefault(o => o.CheckBatch == "2_0722_1");
            return ToDataGrid(list, count, footer);
        }
        [Obsolete]
        public ActionResult FindStockList(string lockStoreID, string checkBatch)
        {
            int count = 0;
            var list = TakeStockService.FindStockList(lockStoreID, checkBatch);
            return ToDataGrid(list, count);
        }
        [HttpPost]
        public ActionResult FindStockLogList(string checkBatch, string checkUID, string state)
        {
            int count = 0;
            var list = TakeStockService.FindStockLogList(checkBatch, checkUID, state);
            var footer = new List<object>() { 
                new {Number=0,SubUnit="小计:"}
            };
            return ToDataGrid(list, count, footer);
        }
        public ActionResult Save(string id)
        {
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "请选择");
            return View(new TreasuryLocks());
        }
        [HttpPost]
        public ActionResult Save(TreasuryLocks obj, short state, short hasrepeat)
        {
            var op = TakeStockService.SaveOrUpdate(obj, Request["Updated"], Request["Inserted"], Request["Deleted"], Request["ActualDate"], state, hasrepeat);
            if (op.Successed) TakeStockService.RemoveCache(obj.CheckBatch);
            return Content(op.ToJson());
        }
        public ActionResult ReSave(int id)
        {
            var obj = TakeStockService.GetObjByid(id);
            string storeId = Convert.ToString(obj.GetPropertyValue("LockStoreID"));
            var users = ListToSelect(_userBLL.GetStoreUser(storeId, null,0).Select(o => new SelectListItem() { Value = o.UID, Text = o.FullName }), emptyTitle: "请选择");
            ViewBag.users = users;
            return View(obj);
        }
        [HttpPost]
        public ActionResult ReSave(StockTaking stock)
        {
            var op = TakeStockService.ReSave(stock);
            return Content(op.ToJson());
        }
        [HttpPost]
        public ActionResult TakeStockLogs(string checkBatch, string barcode)
        {
            var list = TakeStockService.TakeStockLogs(checkBatch,barcode);
            return ToDataGrid(list, list.Count());
        }
        public ActionResult CrrectSave(int id)
        {
            var obj = TakeStockService.GetObjByid(id);
            return View(obj);
        }
        [HttpPost]
        public ActionResult CrrectSave(int id, decimal correctNumber)
        {
            var op = TakeStockService.CrrectSave(id, correctNumber);
            return Content(op.ToJson());
        }
        public ActionResult StockLock()
        {
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "请选择");
            ViewBag.brands = ListToSelect(ProductBrandService.GetList(true).Select(o => new SelectListItem() { Value = o.BrandSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            return View(new TreasuryLocks());
        }
        [HttpPost]
        public ActionResult StockLock(TreasuryLocks obj)
        {
            obj.LockCategorySN = Request["selectCategorySN"];
            var op = TakeStockService.AddStockLock(obj);
            return Content(op.ToJson());
        }
        public ActionResult SelectList()
        {
            return View();
        }
        public ActionResult Import()
        {
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "请选择");
            ViewBag.users = new List<SelectListItem>() { new SelectListItem() { Text = Sys.CurrentUser.FullName, Value = Sys.CurrentUser.UID, Selected = true } };
            var pos = new List<SelectListItem>();
            for (int i = 65; i < 15 + 65; i++)
            {
                var val = (char)i + "";
                pos.Add(new SelectListItem() { Text = val, Value = val });
            }
            ViewBag.pos = pos;
            return View(new TreasuryLocks());
        }
        [HttpPost]
        public ActionResult Import(TreasuryLocks obj, char codeCol, char countCol, int? minRow, int? maxRow, string checkUID)
        {
            var re = TakeStockService.Import(obj, Request.Files, codeCol, countCol, minRow.GetValueOrDefault(), maxRow.GetValueOrDefault(), checkUID);
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult GetBatchAndCategory(string storeId)
        {
            var no = TakeStockService.GetBatchAndCategory(storeId);
            return Content(no);
        }
        [HttpPost]
        public ActionResult StoreSelect(string storeId, short? state, string date, string checkBatch)
        {
            var express = DynamicallyLinqHelper.True<TreasuryLocks>().And(o => o.LockStoreID == storeId, storeId.IsNullOrEmpty())
                .And(o => o.State == state, !state.HasValue).And(o => o.CompanyId == CommonService.CompanyId);
            if (!date.IsNullOrEmpty())
            {
                var start = DateTime.Parse(date);
                var end = start.AddMonths(1);
                express = express.And(o => o.LockDate >= start && o.LockDate < end);
            }
            var list = BaseService<TreasuryLocks>.FindList(express).OrderByDescending(o => o.LockDate).Select(o => new DropdownItem(o.CheckBatch)).ToList();
            if (!checkBatch.IsNullOrEmpty()) list.Where(o => o.Value == checkBatch).Each(o => o.IsSelected=true);
            list.Insert(0, new DropdownItem("", "请选择", checkBatch.IsNullOrEmpty()));

            var users = _userBLL.GetStoreUser(storeId, null,0).Select(o => new DropdownItem(o.UID, o.FullName)).ToList();
            users.Insert(0, new DropdownItem("", "请选择", true));
            return new JsonNetResult(new { batchs = list, users = users });
        }
        [HttpPost]
        public ActionResult CheckerSelect(string batchNo)
        {
            var list = TakeStockService.GetCheckerByBatchnoList(batchNo).ToList();
            list.Insert(0, new DropdownItem("", "全部", true));
            return new JsonNetResult(list);
        }
        [HttpPost]
        public ActionResult BatchSelect(string storeId, string batchNo)
        {
            var list = TakeStockService.FindBarcodesByBatch(batchNo);
            return Json(list);
        }
        public ActionResult ReSaveList()
        {
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "请选择");
            return View();
        }
        [HttpPost]
        public ActionResult ReBatchSelect(string storeId, string batchNo)
        {
            var list = TakeStockService.FindBarcodesByBatch(batchNo, 1);
            return Json(list);
        }
        public ActionResult Export()
        {
            int count = 0;
            object footer = null;
            var dt = TakeStockService.FindPageList(Request.Params, out count, ref footer, false);
            if (dt == null || dt.Rows.Count <= 0) return RedirectAlert("Index", "暂无数据!");
            dt.Columns.Add("盘点员工号");
            dt.Columns.Add("序号");
            //string[] fields = { "StoreTitle", "CheckBatch", "Barcode", "Title", "BrandTitle", "SubUnit", "ActualNumber"};
            //string[] names = { "盘点门店", "盘点批次", "条码", "品名", "品牌", "单位", "实盘数量" };
            string[] fields = { "序号", "CategoryTitle", "Barcode", "Title", "BrandTitle", "SubUnit", "ActualNumber", "盘点员工号" };
            string[] names = { "序号","品类", "条码", "品名", "品牌", "单位", "实盘数量", "盘点员工号" };
            var title = "";
            var i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                dr["ActualNumber"] = DBNull.Value;
                dr["序号"] = i++;
            }
            int[] merger = { };
            new ExportExcel() { IsBufferOutput = true, HeaderText = title }.ToExcel(dt.Rows[0]["StoreTitle"] + "_盘点_" + dt.Rows[0]["CheckBatch"], dt, fields, names, merger,formatFileName:false);
            return new EmptyResult();
        }

        public ActionResult Approval()
        {
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "请选择");
            return View();
        }
        [HttpPost]
        public ActionResult ApprovalPass(string checkBatch)
        {
            var op = TakeStockService.ApprovalPass(checkBatch);
            return new JsonNetResult(op);
        }
        [HttpPost]
        public ActionResult FindApprovalList(string storeId, string checkBatch)
        {
            int count = 0;
            object list = null, footer = null;
            if (!storeId.IsNullOrEmpty() && !checkBatch.IsNullOrEmpty())
                list = TakeStockService.FindPageList(Request.Params, out count, ref footer);
            return ToDataGrid(list, count, footer);
        }
        public ActionResult Report()
        {
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "请选择");
            return View();
        }
        [HttpPost]
        public ActionResult ReportList(string storeId, string checkBatch)
        {
            int count = 0;
            DataTable dt = null;
            object footer = null;
            if (!storeId.IsNullOrEmpty() && !checkBatch.IsNullOrEmpty())
                dt = TakeStockService.ReportList(Request.Params, out count, ref footer);
            return ToDataGrid(dt, count, footer);
        }
        public ActionResult SubExport(string storeId, string checkBatch)
        {
            int count = 0;
            var nl = new System.Collections.Specialized.NameValueCollection() { Request.Params };
            //nl.Add("state", "1");
            object footer = null;
            var dt = TakeStockService.ReportList(nl, out count, ref footer, false);
            if (dt == null || dt.Rows.Count <= 0) return RedirectAlert("Report", "暂无数据,请先确认是否已审核!");
            dt.Columns.Add("序号");
            dt.Columns.Add("复盘数量");
            dt.Columns.Add("盘点员工号");
            for (var i = 0; i < dt.Rows.Count; i++)
                dt.Rows[i]["序号"] = i + 1;
            string[] fields = { "序号", "SureTitle", "SupplierTitle", "CategoryTitle", "Barcode", "Title", "Size", "SubUnit", "SysPrice", "LockNumber", "ActualNumber", "复盘数量", "盘点员工号" };
            string[] names = { "序号", "差异确认", "供应商", "品类", "条码", "品名", "规格", "单位", "系统售价", "锁库库存", "盘点数量", "复盘数量", "盘点员工号" };
            var totalCols = new int[] { };
            var title = "";
            new ExportExcel() { IsBufferOutput = true, HeaderText = title }.ToExcel("盘点", dt, fields, names, null, totalCols);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult GetProductInput(string searchName, string checkBatch, string storeId, short? state)
        {
            var list = TakeStockService.GetProductInput(searchName, checkBatch, storeId, state);
            return ToDataGrid(list, 0);
        }
        [HttpPost]
        public void PostCache(string rows, string checkBatch)
        {
            TakeStockService.PostCache(rows, checkBatch);
        }
        [HttpPost]
        public void AutoBalance()
        {
            TakeStockService.AutoInventoryBalance();
        }
        [HttpPost]
        public ActionResult SetSure(string ids,short sure)
        {
            var idls= ids.Split(',').Select(o => int.Parse(o)).ToList();
            var list= TakeStockService.FindList(o => idls.Contains(o.Id));
            list.Each(o => o.Sure = sure);
            var op = TakeStockService.Update(list);
            return new JsonNetResult(op);
        }
    }
}