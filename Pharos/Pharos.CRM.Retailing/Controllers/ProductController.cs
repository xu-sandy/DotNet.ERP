﻿using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Logic;
using Pharos.CRM.Retailing.Models;
namespace Pharos.CRM.Retailing.Controllers
{
    public class ProductController : BaseController
    {
        #region 首页
        public ActionResult Index()
        {
            ViewBag.parenttypes = ListToSelect(ProductCategoryService.GetParentTypes().Select(o => new SelectListItem() { Value = o.CategorySN.ToString(), Text = o.Title }), emptyTitle: "全部");
            //ViewBag.suppliers = ListToSelect(SupplierService.GetList().Where(o => o.BusinessType == 1).Select(o => new SelectListItem() { Value = o.Id, Text = o.Title }), emptyTitle: "全部");
            ViewBag.brands = ListToSelect(ProductBrandService.GetList().Select(o => new SelectListItem() { Value = o.BrandSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }

        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = ProductService.LoadProductList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public void Export()
        {
            int count = 0;
            var list = ProductService.LoadProductList(Request.Params, out count, false);
            List<string> fields = new List<string>() { "SupplierTitle", "ProductCode", "Barcode", "CategoryTitle", "Title", "Size", "BrandTitle", "SubUnit", "ValuationTypeTitle", "BuyPrice", "SysPrice", "StateTitle" };
            List<string> names = new List<string>() { "主供应商", "货号", "商品条码", "品类", "商品名称", "规格", "品牌", "单位", "计价方式", "进价", "系统售价", "产品状态" };
            new ExportExcel() { IsBufferOutput = true, HeaderText = "商品档案信息" }.ToExcel(DateTime.Now.ToString("yyyyMMdd"), list.ToDataTable(), fields.ToArray(), names.ToArray(), null, null);
        }
        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            return new JsonNetResult(ProductService.Delete(ids));
        }
        #endregion
        #region 秤称
        public ActionResult ProductWeight()
        {
            ViewBag.parenttypes = ListToSelect(ProductCategoryService.GetParentTypes().Select(o => new SelectListItem() { Value = o.CategorySN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.brands = ListToSelect(ProductBrandService.GetList().Select(o => new SelectListItem() { Value = o.BrandSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        public ActionResult ProductWeighSet()
        {
            ViewBag.weighTypes= EnumToSelect(typeof(WeighType),selectValue:1).Select(o=>new DropdownItem(o.Value,o.Text,o.Selected)).ToJson();
            return View();
        }
        [HttpPost]
        public ActionResult ProductWeighSet(string inserted, string deleteed, string updateed,string rows)
        {
            var re = ProductService.ProductWeighSet(inserted, deleteed, updateed,rows);
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult ProductWeighSetList()
        {
            var list = ProductService.FindWeighSetList();
            return ToDataGrid(list, list.Count());
        }
        public ActionResult ProductWeighList()
        {
            var selBatch = Request["selBatch"];
            var store= Request["store"];
            var communication =short.Parse(Request["Communication"]);
            ViewBag.oldBars = "";
            return View();
        }
        [HttpPost]
        public ActionResult ProductWeighBatchList()
        {
            int count = 0;
            var list = ProductService.ProductWeighBatchList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult RemoveBatchList(int batchId,string bars)
        {
            return new JsonNetResult(ProductService.RemoveBatchList(batchId, bars));
        }
        [HttpPost]
        public ActionResult GetNumByCatesn(int? catesn, string storeId)
        {
            if (catesn.HasValue)
            {
                var catesns = ProductCategoryService.GetChildSNs(new List<int>() { catesn.Value }, true);
                if (!storeId.IsNullOrEmpty())
                {
                    var entity = WarehouseService.Find(o => o.StoreId == storeId && o.CompanyId==CommonService.CompanyId);
                    var allsns = ProductCategoryService.GetChildSNs(entity.CategorySN.Split(',').Select(o => int.Parse(o)).ToList(),true);
                    catesns = allsns.Where(o => catesns.Contains(o)).ToList();
                }
                return Content(ProductService.FindList(o => catesns.Contains(o.CategorySN) && o.ValuationType == 2 && o.CompanyId == CommonService.CompanyId).Count.ToString());
            }
            return Content("0");
        }
        public ActionResult ExportOption()
        {
            ViewBag.stores = ListToSelect(Pharos.Logic.BLL.WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title,Selected=Request["store"]==o.StoreId }),emptyTitle:"通用");
            var list=ProductService.FindWeighSetList();
            var items = new List<SelectListItem>();
            var weigh = Request["weigh"];
            foreach(var o in list)
            {
                var code= Convert.ToString(o.GetPropertyValue("Code"));
                var title=code+" "+ Convert.ToString(o.GetPropertyValue("Title"));
                items.Add(new SelectListItem() { Value=code,Text=title,Selected=code==weigh});
            }
            ViewBag.weighs = ListToSelect(items);
            return View();
        }
        public ActionResult GetBatchList(string storeId, short communication)
        {
            return new JsonNetResult(ProductService.GetBatchList(storeId, communication));
        }
        public ActionResult ExportWeight()
        {
            var maxRecord = Request["MaxRecord"];
            var store = Request["store"];
            var weigh = Request["Weigh"];
            var msg="";
            if (!weigh.IsNullOrEmpty())
                msg = ProductService.SendDevice(weigh,maxRecord, store);
            else
                msg = ProductService.ExportWeight(maxRecord, store);
            return RedirectAlert("ExportOption", msg);
        }
        #endregion
        #region 新合并商品
        public ActionResult GroupNewIndex(int? id, string barcodes)
        {
            var obj = new ProductRecord();
            obj.ProductGroups = new List<ProductGroup>();
            if (id.HasValue)
            {
                obj = ProductService.FindById(id);
                if (obj.Nature == 1)
                {
                    obj.ProductGroups = ProductService.GetGroups(obj.Barcode);
                }
            }
            return View(obj);
        }
        public ActionResult GroupSaveNew(int? id, string barcodes)
        {
            LoadData();
            var obj = new ProductRecord() { Nature = 1, ValidityWarning = 5, InventoryWarning = 5 ,InventoryWarningMax=1000};
            obj.ProductGroups = new List<ProductGroup>();
            ViewBag.promsg = "";
            if(id.HasValue)
            {
                obj = ProductService.FindById(id);
                if (obj.Nature == 1)
                {
                    obj.ProductGroups = ProductService.GetGroups(obj.Barcode);
                }
                obj.IsRelationship = ProductService.IsRelationship(obj.Barcode);
            }
            if (!barcodes.IsNullOrEmpty())
            {
                var bars = barcodes.TrimEnd(',').Split(',').Distinct().ToList();
                var products = BaseService<VwProduct>.FindList(o => bars.Contains(o.Barcode) && o.CompanyId==CommonService.CompanyId);
                obj.ProductGroups.RemoveAll(o => !bars.Contains(o.GroupBarcode));
                if(!id.HasValue && products.Any())
                {
                    //obj.ValuationType = products[0].ValuationType;
                    obj.GroupCount = products.Min(o => o.StockNums);
                    obj.CategorySN = products[0].CategorySN;
                    obj.SubUnitId = products[0].SubUnitId;
                    obj.ProductCode2 = products[0].ProductCode;
                }
                obj.BuyPrice = products.Sum(o => o.BuyPrice);
                obj.SysPrice = products.Sum(o => o.SysPrice);
                foreach (var o in products)
                {
                    if (!obj.ProductGroups.Any(i => i.GroupBarcode == o.Barcode))
                        obj.ProductGroups.Add(new ProductGroup() { GroupBarcode = o.Barcode, Number = 1, Title = o.Title, BuyPrice = o.BuyPrice, SysPrice = o.SysPrice, ValuationType = o.ValuationType,Unit=o.SubUnit });
                }
            }
            return View(obj.IsNullThrow());
        }
        #endregion
        #region 拆分商品
        public ActionResult SplitSave(int? id)
        {
            LoadData();
            var obj = new ProductRecord();
            if (id.HasValue)
            {
                obj = ProductService.FindById(id);
                obj.OldBarcode = obj.Barcode;
                obj.ProductCode2 = obj.ProductCode;
            }
            return View("SplitSaveNew", obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult SplitSave(ProductRecord obj)
        {
            var re = ProductService.SaveSplit(obj);
            return Content(re.ToJson());
        }
        #endregion
        #region 新建档
        public ActionResult SaveNew(int? id)
        {
            //ViewBag.hasMultPrice = true;
            //if (id.HasValue)
            //{ 
            //    var barMultPrice = BaseService<ProductRecord>.FindById(id).Barcode;
            //    var listMultPrice = ProductService.LoadMultPriceList(barMultPrice).ToJson();
            //    ViewBag.hasMultPrice = (listMultPrice.Count() > 2 ? true : false);
            //}
            ViewBag.omsurl = Pharos.Utility.Config.GetAppSettings("omsurl");
            LoadData();
            SelectListItem city = null;
            var obj = new ProductRecord() { ValidityWarning = 5, InventoryWarning = 5,InventoryWarningMax=1000,ValuationType=1,SaleRate=17,StockRate=17 };
            obj.ProductImages = new List<ProductImage>();
            if (id.HasValue)
            {           
               
                obj = ProductService.FindById(id);
                //var barcode = ProductService.FindById(id).Barcode;
                //var re= ProductService.getIsRelationship(barcode);
                if (obj != null)
                {
                    obj.IsRelationship = ProductService.IsRelationship(obj.Barcode);
                    if (obj.CityId > 0)
                    {
                        var area = BaseService<Area>.FindById(obj.CityId);
                        if (area != null)
                        {
                            var parentArea = BaseService<Area>.FindById(area.AreaPID);
                            var text = area.Title;
                            if (parentArea != null) text = parentArea.Title + "/" + text;
                            city=new SelectListItem() { Value = obj.CityId.ToString(), Text = text, Selected = true };
                        }
                    }
                    ViewBag.haschild= ProductService.IsExist(o => o.OldBarcode == obj.Barcode && o.CompanyId==CommonService.CompanyId);
                    obj.ProductImages= BaseService<ProductImage>.FindList(o => o.CompanyId == obj.CompanyId && o.ProductCode == obj.ProductCode).OrderByDescending(o=>o.CreateDT).ToList();
                }
            }
            ViewBag.provinces = ListToSelect(BaseService<Area>.FindList(o => o.Type == 2).OrderBy(o => o.OrderNum).Select(o => new SelectListItem() { Text = o.Title, Value = o.AreaID.ToString() }), emptyTitle: "请选择");
            ViewBag.city = city;
            for (int i = obj.ProductImages.Count; i < 5; i++)
            {
                obj.ProductImages.Add(new ProductImage());
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult FindTreeList()
        {
            int count = 0;
            var list = ProductCategoryService.FindPageList(Request.Params, out count, false);
            list.Each(o=>{
                o.Childrens.Each(i =>
                {
                    i.OnOff =i.Childrens!=null && i.Childrens.Any()? "closed":"open";
                });
            });
            var trees = new List<ProductCategory>(){
                new ProductCategory(){
                Id = 0,
                Title = "商品分类",
                OnOff="open",
                Childrens = new List<ProductCategory>()
                }
            };
            trees[0].Childrens.AddRange(list);
            return new JsonNetResult(trees);
        }
        public ActionResult GetCity(int pid)
        {
            var list = BaseService<Area>.FindList(o => o.AreaPID == pid).OrderBy(o => o.OrderNum).Select(o => new DropdownItem() { Text = o.Title, Value = o.AreaID.ToString() }).ToList();
            if (list.Any()) list[0].IsSelected = true;
            return new JsonNetResult(list);
        }
        public ActionResult LoadChildList(string barcode)
        {
            var list = ProductService.LoadMultPriceList(barcode);
            return ToDataGrid(list, 0);
        }
        public ActionResult LoadSupplierList(string barcode)
        {
            var list = ProductService.LoadMultSupplierList(barcode);
            return ToDataGrid(list, 0);
        }
        [HttpPost]
        public ActionResult LoadSplitList(string barcode)
        {
            var list =barcode.IsNullOrEmpty()?null:BaseService<VwProduct>.FindList(o=>o.OldBarcode== barcode && o.CompanyId==CommonService.CompanyId);
            return ToDataGrid(list, 0);
        }
        #endregion
        #region 统一保存入口
        /// <summary>
        /// 统一保存入口
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(ProductRecord obj)
        {
            var re = ProductService.SaveOrUpdate(obj);
            return Content(re.ToJson());
        }
        #endregion
        #region 设置各状态
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetState(string ids, short state)
        {
            var sid = ids.Split(',').Select(o => int.Parse(o));
            var list = ProductService.FindList(o => sid.Contains(o.Id));
            list.ForEach(o =>
            {
                if (state == 1)
                    o.State = 1;
                else if (state == 2)
                    o.State = 0;
                else if (state == 3)
                    o.IsReturnSale = 1;
                else if (state == 4)
                    o.IsReturnSale = 0;
                else if (state == 5)
                    o.IsAcceptOrder = 1;
                else if (state == 6)
                    o.IsAcceptOrder = 0;
                else if (state == 7)
                    o.Favorable = 1;
                else if (state == 8)
                    o.Favorable = 0;
            });
            var re = ProductService.Update(list);
            return new JsonNetResult(re);
        }
        #endregion
        #region 联动下拉框
        public ActionResult ParentTypeSelect(int? id, short? showTitle)
        {
            var childtypes = new List<DropdownItem>();
            if (id.HasValue)
                childtypes = ProductCategoryService.GetChildCategorys(id.GetValueOrDefault()).Select(o => new DropdownItem(o.CategorySN.ToString(), o.Title)).ToList();
            if (showTitle == 1)
                childtypes.Insert(0, new DropdownItem("", "子类", true));
            return new JsonNetResult(childtypes);
        }
        public ActionResult ChildTypeSelect(int? id)
        {
            var brands = new List<DropdownItem>();
            if (id.HasValue)
                brands = ProductBrandService.GetListByProduct(id.GetValueOrDefault()).Select(o => new DropdownItem(o.Value.ToString(), o.Key)).ToList();
            brands.Insert(0, new DropdownItem("", "请选择", true));
            return new JsonNetResult(brands);
        }

        public ActionResult BrandSelect(int? subCate, int? brandSn)
        {
            int count = 0;
            var list = ProductService.FindTypePageList(Request.Params, out count);
            return new JsonNetResult(list);
        }
        private void LoadData()
        {
            //ViewBag.parenttypes = ListToSelect(ProductCategoryService.GetParentTypes().Select(o => new SelectListItem() { Value = o.CategorySN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            //ViewBag.childtypes = new List<SelectListItem>();
            ViewBag.suppliers = ListToSelect(SupplierService.GetList().Where(o => o.BusinessType == 1).Select(o => new SelectListItem() { Value = o.Id, Text = o.Title }), emptyTitle: "");
            //ViewBag.bigunits = ListToSelect(SysDataDictService.GetBigUnitCategories().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            ViewBag.subunits = ListToSelect(SysDataDictService.GetSubUnitCategories().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "");
            ViewBag.brands = ListToSelect(ProductBrandService.GetList().Select(o => new SelectListItem() { Value = o.BrandSN.ToString(), Text = o.Title }), emptyTitle: "");
        }
        #endregion
        #region 商品自动完成
        /// <summary>
        /// 输入自动完成商品
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProductInput(string searchName, string supplierID, string zp, string storeId, string order,short? getStockNum)
        {
            if (searchName.IsNullOrEmpty()) return new EmptyResult();
            var express = DynamicallyLinqHelper.Empty<VwProduct>()
                .And(o => (o.Barcode != null && o.Barcode.StartsWith(searchName)) || o.Title.Contains(searchName) || (o.Barcodes!=null && o.Barcodes.Contains(searchName)))
                .And(o => o.IsAcceptOrder == 1, order.IsNullOrEmpty()).And(o=>o.CompanyId==CommonService.CompanyId);
            if(!supplierID.IsNullOrEmpty())
            {
                var sp = supplierID.Split(',').ToList();
                var bars = BaseService<ProductMultSupplier>.FindList(o => sp.Contains(o.SupplierId)).Select(o => o.Barcode).Distinct().ToList();
                express = express.And(o =>(sp.Contains(o.SupplierId) || bars.Contains(o.Barcode)), supplierID.IsNullOrEmpty());
            }
            if (!storeId.IsNullOrEmpty())
            {
                var ware = WarehouseService.Find(o => o.StoreId == storeId && o.CompanyId == CommonService.CompanyId);
                if (ware != null)
                {
                    var categorySNs = ware.CategorySN.Split(',').Select(o => int.Parse(o)).ToList();
                    var childs = ProductCategoryService.GetChildSNs(categorySNs,true);
                    express = express.And(o => childs.Contains(o.CategorySN));
                }
            }
            var list = BaseService<VwProduct>.FindList(express, takeNum: 20);
            list = ProductService.SetAssistBarcode(list);
            ProductService.SetSysPrice<VwProduct>(storeId, list,supplierId:supplierID);
            if (getStockNum == 1) ProductService.SetStockNum(storeId, list);
            return ToDataGrid(list, 20);
        }
        [HttpPost]
        public ActionResult GetProductInputBySupplierId(string supplierID)
        {
            var express = DynamicallyLinqHelper.Empty<VwProduct>().And(o => o.SupplierId == supplierID, supplierID.IsNullOrEmpty());
            var list = BaseService<VwProduct>.FindList(express);
            return ToDataGrid(list, list.Count);
        }
        //flag=1不等于,0等于
        [HttpPost]
        public ActionResult GetBarcodeInput(string searchName, short? nature, short? valuationType,short? flag)
        {
            var express = DynamicallyLinqHelper.Empty<ProductRecord>()
                .And(o=>o.ValuationType==valuationType.Value,!valuationType.HasValue).And(o=>o.Barcode.Contains(searchName) || o.Title.Contains(searchName),searchName.IsNullOrEmpty());
            if (flag == 1)
                express = express.And(o => o.Nature != nature.Value, !nature.HasValue);
            else
                express = express.And(o => o.Nature == nature.Value, !nature.HasValue);
            express = express.And(o => o.CompanyId == CommonService.CompanyId);
            var list = BaseService<ProductRecord>.FindList(express,takeNum:20);
            return ToDataGrid(list, list.Count);
        }
        #endregion       

        #region 品类和商品弹窗
        public ActionResult SelectProduct()
        {
            ViewBag.brands = ListToSelect(ProductBrandService.GetList().Select(o => new SelectListItem() { Value = o.BrandSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        public ActionResult SelectType()
        {
            ViewBag.parenttypes = ListToSelect(ProductCategoryService.GetParentTypes().Select(o => new SelectListItem() { Value = o.CategorySN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            ViewBag.brands = ListToSelect(ProductBrandService.GetList().Select(o => new SelectListItem() { Value = o.BrandSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        public ActionResult SelectType2()
        {
            ViewBag.parenttypes = ListToSelect(ProductCategoryService.GetParentTypes().Select(o => new SelectListItem() { Value = o.CategorySN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            return View();
        }
        [HttpPost]
        public ActionResult FindTypePageList()
        {
            int count = 0;
            var list = ProductService.FindTypePageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        [HttpPost]
        public ActionResult FindTypePageList2()
        {
            int count = 0;
            var list = ProductService.FindTypePageList2(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        #endregion
        #region 调价日志
        public ActionResult LoadChangLogList(string barcode)
        {
            int count = 0;
            var list = ProductService.LoadChangLogList(barcode, out count);
            return ToDataGrid(list, count);
        }
        #endregion
        #region 产品变价
        public ActionResult ChangePriceManager()
        {
            //ViewBag.suppliers = ListToSelect(SupplierService.GetList().Where(o => o.BusinessType == 1).Select(o => new SelectListItem() { Value = o.Id, Text = o.Title }), emptyTitle: "全部");
            ViewBag.stores = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult FindChangePriceList()
        {
            int count = 0;
            var list = ProductService.FindChangePriceList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult ChangePrice(string id)
        {
            ViewBag.users= ListToSelect(new Pharos.Sys.BLL.SysUserInfoBLL().GetUsers(0).Select(o => new SelectListItem() { Value = o.UID, Text = o.FullName }),emptyTitle:"请选择");
            //ViewBag.suppliers = ListToSelect(SupplierService.GetList().Where(o => o.BusinessType == 1).Select(o => new SelectListItem() { Value = o.Id, Text = o.Title }), emptyTitle: "请选择");
            ViewBag.stores = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部",emptyValue:"-1");
            ProductChangePrice obj = new ProductChangePrice() { CreateUID=Sys.CurrentUser.UID};
            if(!id.IsNullOrEmpty())
            {
                obj = BaseService<ProductChangePrice>.FindById(id);
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult ChangePrice(ProductChangePrice obj)
        {
            obj.StoreId = Request["StoreId"];
            var re = ProductService.SaveOrUpdateChangePrice(obj, Request["Inserted"], Request["Deleted"], Request["Updated"]);
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult LoadChangeDetailList(string mid)
        {
            var list = ProductService.LoadChangeDetailList(mid);
            return ToDataGrid(list, 0);
        }
        [HttpPost]
        public ActionResult DeleteChangePrice(int[] ids)
        {
            return new JsonNetResult(ProductService.DeleteChangePrice(ids));
        }
        [HttpPost]
        public ActionResult ChangeState(string ids)
        {
            return new JsonNetResult(ProductService.AuditorChangeState(ids));
        }
        [HttpPost]
        public ActionResult SetFlag(string ids)
        {
            return new JsonNetResult(ProductService.SetInvalid(ids));
        }
        #endregion
        #region 产品批发价
        public ActionResult TradePriceManager()
        {
            ViewBag.suppliers = ListToSelect(SupplierService.GetWholesalerList().Select(o => new SelectListItem() { Value = o.Id, Text = o.FullTitle }), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult FindTradePriceList()
        {
            int count = 0;
            var list = ProductService.FindTradePriceList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult TradePrice(string id)
        {
            ViewBag.suppliers = ListToSelect(SupplierService.GetWholesalerList().Select(o => new SelectListItem() { Value = o.Id, Text = o.FullTitle }), emptyTitle: "全部",emptyValue:"-1");
            ProductTradePrice obj = new ProductTradePrice() { CreateUID = Sys.CurrentUser.UID };
            if (!id.IsNullOrEmpty())
            {
                obj = BaseService<ProductTradePrice>.FindById(id);
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult TradePrice(ProductTradePrice obj)
        {
            obj.Wholesaler = Request["Wholesaler"];
            var re = ProductService.SaveOrUpdateTradePrice(obj, Request["Inserted"], Request["Deleted"], Request["Updated"]);
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult LoadTradeDetailList(string mid)
        {
            var list = ProductService.LoadTradeDetailList(mid);
            return ToDataGrid(list, 0);
        }
        [HttpPost]
        public ActionResult DeleteTradePrice(int[] ids)
        {
            return new JsonNetResult(ProductService.DeleteTradePrice(ids));
        }
        [HttpPost]
        public ActionResult AuditorTradeState(string ids)
        {
            return new JsonNetResult(ProductService.AuditorTradeState(ids));
        }
        [HttpPost]
        public ActionResult SetTradeFlag(string ids)
        {
            var op = new OpResult();
            if (ids.IsNullOrEmpty())
            {
                op.Message = "请选择要处理的项";
            }
            else
            {
                var id = ids.Split(',').Distinct().ToList();
                var list = BaseService<ProductTradePrice>.FindList(o => id.Contains(o.Id));
                list.Each(o => { o.State = 2; });
                op = BaseService<ProductTradePrice>.Update(list);
            }
            return new JsonNetResult(op);
        }
        #endregion
        #region 导入
        public ActionResult Import()
        {
            var obj = BaseService<ImportSet>.Find(o => o.CompanyId == CommonService.CompanyId && o.TableName == "ProductRecord");
            return View(obj ?? new ImportSet());
        }
        [HttpPost]
        public ActionResult Import(ImportSet imp)
        {
            imp.TableName = "ProductRecord";
            imp.CompanyId = CommonService.CompanyId;
            var op = ProductService.Import(imp, Request.Files, Request["FieldName"], Request["ColumnName"]);
            return Content(op.ToJson());
        }
        public ActionResult ImportPrice()
        {
            var obj = BaseService<ImportSet>.Find(o => o.CompanyId == CommonService.CompanyId && o.TableName == "ProductPriceUpdate");
            return View(obj ?? new ImportSet());
        }
        [HttpPost]
        public ActionResult ImportPrice(ImportSet imp)
        {
            imp.TableName = "ProductPriceUpdate";
            imp.CompanyId = CommonService.CompanyId;
            var op = ProductService.ImportPrice(imp, Request.Files, Request["FieldName"], Request["ColumnName"]);
            return Content(op.ToJson());
        }
        #endregion
        #region 当前价格信息
        public ActionResult GetCurrentPrice(string barcode,short type)
        {
            return new JsonNetResult(ProductService.GetCurrentPrice(barcode, type));
        }
        #endregion
    }
}
