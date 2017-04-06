using Pharos.Logic.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Logic.Entity;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic;
using Pharos.Sys;
namespace Pharos.CRM.Retailing.Controllers
{
    public class BrandController : BaseController
    {
        //
        // GET: /Brand/
        LogEngine logEngine = new LogEngine();

        public ActionResult Index()
        {
            return View();
        }

        //品牌管理
        public ActionResult BrandManagement()
        {
            ViewBag.types = ListToSelect(SysDataDictService.GetBrandClassify().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }

        public ActionResult FindPageList(string title, int? classfyId)
        {
            var count = 0;
            var list = new ProductBrandBLL().GetBrandListBySearch(title, classfyId, out count);
            return ToDataGrid(list, count);
        }
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.品牌管理_移除品牌)]
        public ActionResult Delete(int[] ids)
        {
            var list = ProductBrandService.FindList(o => ids.Contains(o.Id));
            var brands = list.Select(o => o.BrandSN).ToList();
            var re = new OpResult();
            if (ProductService.IsExist(o => o.BrandSN.HasValue && brands.Contains(o.BrandSN.Value)))
                re.Message = "商品存在品牌关联";
            else
                re = ProductBrandService.Delete(list);
            #region 16-04-05 操作日志
            try
            {
                foreach (var item in list)
                {
                    var logMsg = LogEngine.CompareModelToLog<ProductBrand>(LogModule.品牌管理, null, item);
                    logEngine.WriteDelete(logMsg, LogModule.品牌管理);
                }
            }
            catch
            {

            }

            #endregion

            return new JsonNetResult(re);
        }
        //新增品牌
        public ActionResult AddBrand(int? id)
        {
            ViewBag.types = ListToSelect(SysDataDictService.GetBrandClassify().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            var obj = new ProductBrand();
            if (id.HasValue)
            {
                obj = ProductBrandService.FindById(id.Value);
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.品牌管理_编辑品牌)]
        public ActionResult AddBrand(ProductBrand obj)
        {
            var re = new OpResult();
            obj.CompanyId = CommonService.CompanyId;
            if (ProductBrandService.IsExist(o => o.CompanyId==obj.CompanyId && o.Title == obj.Title && o.Id != obj.Id))
                re.Message = "已存在该名称";
            else if (obj.Id == 0)
            {
                obj.BrandSN = ProductBrandService.MaxSN;
                obj.State = 1;
                re = ProductBrandService.Add(obj);
                #region 操作日志
                try
                {
                    var logMsg = LogEngine.CompareModelToLog<ProductBrand>(LogModule.品牌管理, obj);
                    logEngine.WriteInsert(logMsg, LogModule.品牌管理);
                }
                catch
                {
                }
                #endregion
            }
            else
            {
                var supp = ProductBrandService.FindById(obj.Id);
                obj.ToCopyProperty(supp);
                re = ProductBrandService.Update(supp);
                #region 操作日志
                try
                {
                    var logMsg = LogEngine.CompareModelToLog<ProductBrand>(LogModule.品牌管理, obj, supp);
                    logEngine.WriteUpdate(logMsg, LogModule.品牌管理);
                }
                catch
                {
                }
                #endregion
            }
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult SetState(string ids, short state)
        {
            var sId = ids.Split(',').Select(o => int.Parse(o));
            var olist = ProductBrandService.FindList(o => sId.Contains(o.Id));
            var list = new List<ProductBrand>();
            foreach (var item in olist)
            {
                ProductBrand _band = new ProductBrand()
                {
                    Id = item.Id,
                    ClassifyId = item.ClassifyId,
                    CompanyId = item.CompanyId,
                    BrandSN = item.BrandSN,
                    JianPin = item.JianPin,
                    Num = item.Num,
                    State = item.State,
                    Title = item.Title
                };
                //_band.ToCopyProperty(item);
                list.Add(_band);
            }
            list.ToCopyProperty(olist);
            list.ForEach(o => { o.State = state; });
            #region 操作日志
            if (olist != null)
            {
                for (int i = 0; i < olist.Count(); i++)
                {
                    try
                    {
                        var logMsg = LogEngine.CompareModelToLog<ProductBrand>(LogModule.品牌管理, list[i], olist[i]);
                        logEngine.WriteUpdate(logMsg, LogModule.品牌管理);
                    }
                    catch
                    {
                    }
                }
            }
            #endregion
            olist.ForEach(o => { o.State = state; });
            var re = ProductBrandService.Update(olist);
            return new JsonNetResult(re);
        }
        [HttpPost]
        public ActionResult GetBrandInput(string searchName)
        {
            if (searchName.IsNullOrEmpty()) return new EmptyResult();
            var list = ProductBrandService.FindList(o => o.CompanyId == CommonService.CompanyId && (o.JianPin != null && o.JianPin.StartsWith(searchName)) || o.Title.Contains(searchName));
            return ToDataGrid(list, 10);
        }
        public ActionResult Import()
        {
            var obj = BaseService<ImportSet>.Find(o => o.CompanyId == CommonService.CompanyId && o.TableName == "ProductBrand");
            return View(obj ?? new ImportSet());
        }
        [HttpPost]
        public ActionResult Import(ImportSet imp)
        {
            imp.TableName = "ProductBrand";
            imp.CompanyId = CommonService.CompanyId;
            var op = ProductBrandService.Import(imp, Request.Files, Request["FieldName"], Request["ColumnName"]);
            return Content(op.ToJson());
        }
        [HttpPost]
        public ActionResult AddBrandTitle(string title)
        {
            var re = new OpResult();
            var obj = new ProductBrand();
            obj.CompanyId = CommonService.CompanyId;
            var source= ProductBrandService.Find(o =>o.CompanyId==obj.CompanyId && o.Title == title);
            if (source != null)
                re = OpResult.Success(source.BrandSN.ToString());
            else
            {
                obj.Title = title;
                obj.BrandSN = ProductBrandService.MaxSN;
                obj.State = 1;
                re = ProductBrandService.Add(obj);
                if(re.Successed)
                    re.Message = obj.BrandSN.ToString();
                #region 操作日志
                try
                {
                    var logMsg = LogEngine.CompareModelToLog<ProductBrand>(LogModule.品牌管理, obj);
                    logEngine.WriteInsert(logMsg, LogModule.品牌管理);
                }
                catch
                {
                }
                #endregion
            }
            return Content(re.ToJson());
        }
    }
}
