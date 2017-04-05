﻿using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.CRM.Retailing.Controllers
{
    public class PrivilegeOrderController:BaseController
    {
        public ActionResult Solution()
        {
            ViewBag.Modes = ListToSelect(SysDataDictService.GetDictionaryList(Logic.DicType.返利模式).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), "全部");
            //ViewBag.Suppliers = ListToSelect(SupplierService.GetList().Where(o => o.BusinessType == 1).Select(o => new SelectListItem() { Value = o.Id, Text = o.Title.IsNullOrEmpty() ? o.FullTitle : o.Title }), "请选择");
            return View();
        }
        [HttpPost]
        public ActionResult SolutionList()
        {
            var count = 20;
            var list = PrivilegeOrderService.SolutionList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult SaveSolution(int? id)
        {
            ViewBag.Modes = ListToSelect(SysDataDictService.GetDictionaryList(Logic.DicType.返利模式).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }));
            ViewBag.Suppliers = ListToSelect(SupplierService.GetList().Where(o => o.BusinessType == 1).Select(o => new SelectListItem() { Value = o.Id, Text = o.FullTitle }), emptyTitle: "请选择");
            var obj = PrivilegeOrderService.GetObj(id);
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        [SysPermissionValidate(Code=Sys.SysConstLimits.采购返利_创建返利方案)]
        public ActionResult SaveSolution(PrivilegeSolution obj)
        {
            obj.SupplierIds = Request["SupplierIds"];
            var re = PrivilegeOrderService.SaveSolution(obj, Request["StartVal"], Request["EndVal"]);
            return Content(re.ToJson());
        }
        public ActionResult SetProduct(int? id)
        {
            var obj = PrivilegeOrderService.GetObj(id,false);
            var data= SysDataDictService.Find(o => o.DicSN == obj.ModeSN);
            if (obj != null) obj.ModeTitle = data.Title;
            List<DropdownItem> parents = null;
            if(obj.SupplierIds.IsNullOrEmpty())
            {
                parents = ProductCategoryService.GetParentTypes().Select(o => new DropdownItem() { Value = o.CategorySN.ToString(), Text = o.Title }).ToList();
            }
            else
            {
                var sp= obj.SupplierIds.Split(',').ToList();
                var bars = BaseService<ProductMultSupplier>.FindList(o => sp.Contains(o.SupplierId)).Select(o => o.Barcode).Distinct().ToList();
                var categorys = BaseService<VwProduct>.FindList(o => sp.Contains(o.SupplierId) || bars.Contains(o.Barcode)).Select(o => o.CategorySN).Distinct().ToList();
                parents = ProductCategoryService.GetRootSNs(categorys).Select(o => new DropdownItem() { Value = o.CategorySN.ToString(), Text = o.Title }).ToList(); ;
            }
            parents.Insert(0, new DropdownItem("", "请选择"));
            ViewBag.parenttypes = parents.ToJson();
            ViewBag.types= PrivilegeOrderService.LoadTypeDetailJson(id);
            ViewBag.products= PrivilegeOrderService.LoadProductDetailJson(id);
            return View(obj.ModeSN == 46 ? "SetProduct2" : "SetProduct", obj.IsNullThrow());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetProduct(PrivilegeSolution obj)
        {
            var re = PrivilegeOrderService.SaveDesign(obj);
            return Content(re.ToJson());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetProduct2(PrivilegeSolution obj)
        {
            var re = PrivilegeOrderService.SaveDesign2(obj);
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult LoadTypeDetailList(int? id,bool getValue=true)
        {
            var json = PrivilegeOrderService.LoadTypeDetailJson(id, getValue);
            return Content(json);
        }
        [HttpPost]
        public ActionResult LoadProductDetailList(int? id, bool getValue = true)
        {
            var json = PrivilegeOrderService.LoadProductDetailJson(id, getValue);
            return Content(json);
        }
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.采购返利_移除返利方案)]
        public ActionResult DeleteSolution(int[] ids)
        {
            var re = PrivilegeOrderService.DeleteSolution(ids);
            return new JsonNetResult(re);
        }
        //计算
        public ActionResult Calc()
        {
            ViewBag.States = EnumToSelect(typeof(Pharos.Logic.PrivilegeState), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult CalcList()
        {
            var count = 20;
            var list = PrivilegeOrderService.CalcList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult SaveCalc()
        {
            ViewBag.Modes = ListToSelect(SysDataDictService.GetDictionaryList(Logic.DicType.返利模式).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }));
            ViewBag.Solutions = ListToSelect(PrivilegeOrderService.FindList(o=>o.ModeSN!=46).Select(o => new SelectListItem() { Value = o.Id.ToString(), Text = o.Title }),emptyTitle:"请选择");
            return View(new PrivilegeCalc());
        }
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.采购返利_创建返利方案)]
        public ActionResult SaveCalc(PrivilegeCalc obj)
        {
            var re = PrivilegeOrderService.SaveCalc(obj);
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult GetMode(int solId)
        {
            var obj = PrivilegeOrderService.FindById(solId);
            obj.ModeTitle = SysDataDictService.GetDictionaryList(Logic.DicType.返利模式).FirstOrDefault(o => o.DicSN == obj.ModeSN).Title;
            return new JsonNetResult(obj);
        }
        [HttpPost]
        public ActionResult GetCalc(int solId, DateTime start,DateTime end)
        {
            var str = PrivilegeOrderService.GetCalc(solId,start,end);
            return Content(str);
        }
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.采购返利_移除返利方案)]
        public ActionResult DeleteCalc(int[] ids)
        {
            var re = PrivilegeOrderService.DeleteCalc(ids);
            return new JsonNetResult(re);
        }
        public ActionResult Detail(int id)
        {
            return View(PrivilegeOrderService.GetCalc(id));
        }
        public ActionResult Export(int id)
        {
            var obj= PrivilegeOrderService.GetCalc(id);
            var dt = obj.Details.ToDataTable();
            string[] fields = { "IndentOrderId", "Barcode", "PartName", "CategoryTitle", "Unit", "OrderDate", "TakeDate", "TakeNum", "Price", "SubTotal" };
            string[] names = { "订单号", "商品条码", "商品名称", "类别", "单位", "(订货/销售)日期", "到货日期", "收货数量", "进价", "小计" };
            var title = "返利模式:" + obj.PrivilegeModeSNTitle + "供应商名称：" + obj.SupplierTitle + "   返利期间：" + obj.StartDate.GetValueOrDefault().ToString("yyyy-MM-dd") + "至" + obj.EndDate.GetValueOrDefault().ToString("yyyy-MM-dd") + "    返利(金额/数量)：" + obj.PrivilegeNum;
            new ExportExcel() { IsBufferOutput=true,HeaderText=title}.ToExcel(obj.PrivilegeSolutionTitle, dt,fields,names,null);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult SetState(string ids)
        {
            var sid = ids.Split(',').Select(o=>int.Parse(o));
            var list = BaseService<PrivilegeCalc>.FindList(o => sid.Contains(o.Id));
            list.ForEach(o => { o.State=1; });
            var re = BaseService<PrivilegeCalc>.Update(list);
            return new JsonNetResult(re);
        }
    }
}