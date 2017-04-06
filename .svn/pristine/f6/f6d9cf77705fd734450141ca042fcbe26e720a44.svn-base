using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
namespace QCT.Pay.Admin.Controllers
{
    public class ProductController : BaseController
    {
        #region 私有对象
        [Ninject.Inject]
        ProductService ProductService { get; set; }
        [Ninject.Inject]
        ProductCategoryService ProductCategoryService { get; set; }
        [Ninject.Inject]
        BrandService BrandService { get; set; }
        [Ninject.Inject]
        DictionaryService DictionaryService { get; set; }
        [Ninject.Inject]
        ImportSetService ImportSetService { get; set; }
        #endregion
        #region 首页
        public ActionResult Index()
        {
            ViewBag.parenttypes = ListToSelect(ProductCategoryService.GetFirstList().Select(o => new SelectListItem() { Value = o.CategorySN.ToString(), Text = o.Title }), emptyTitle: "全部");
            //ViewBag.suppliers = ListToSelect(SupplierService.GetList().Where(o => o.BusinessType == 1).Select(o => new SelectListItem() { Value = o.Id, Text = o.Title }), emptyTitle: "全部");
            ViewBag.brands = ListToSelect(BrandService.GetList().Select(o => new SelectListItem() { Value = o.BrandSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }

        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = ProductService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public void Export()
        {
            int count = 0;
            var list = ProductService.GetPageList(Request.Params, out count);
            List<string> fields = new List<string>() { "SupplierTitle", "ProductCode", "Barcode", "CategoryTitle", "Title", "Size", "BrandTitle", "SubUnit", "ValuationTypeTitle", "BuyPrice", "SysPrice", "StateTitle" };
            List<string> names = new List<string>() { "主供应商", "货号", "商品条码", "品类", "商品名称", "规格", "品牌", "单位", "计价方式", "进价", "系统售价", "产品状态" };
            new ExportExcel() { IsBufferOutput = true, HeaderText = "商品档案信息" }.ToExcel(DateTime.Now.ToString("yyyyMMdd"), list.ToDataTable(), fields.ToArray(), names.ToArray(), null, null);
        }
        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            return new JsonNetResult(ProductService.Deletes(ids));
        }
        #endregion
        #region 新建档
        public ActionResult Save(int? id)
        {
            var obj = new ProductRecord() { Source=1};
            if (id.HasValue)
            {           
                obj = ProductService.GetOne(id);
            }
            ViewBag.subunits = ListToSelect(DictionaryService.GetSubUnitCategories().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "");
            ViewBag.brands = ListToSelect(BrandService.GetList().Select(o => new SelectListItem() { Value = o.BrandSN.ToString(), Text = o.Title }), emptyTitle: "");
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult FindTreeList()
        {
            int count = 0;
            var list = ProductCategoryService.GetPageList(Request.Params, out count);
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
            return new OpActionResult(re);
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
            var re = ProductService.SetState(ids,state);
            return new OpActionResult(re);
        }
        #endregion
        
        #region 导入
        public ActionResult Import()
        {
            var obj = ImportSetService.GetOne("ProductRecord");
            return View(obj ?? new ImportSet());
        }
        [HttpPost]
        public ActionResult Import(ImportSet imp)
        {
            imp.TableName = "ProductRecord";
            var op = ProductService.Import(imp, Request.Files, Request["FieldName"], Request["ColumnName"]);
            return Content(op.ToJson());
        }
        #endregion
        
    }
}
