using Pharos.Logic;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Sys;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Pharos.Store.Retailing.Controllers
{
    public class ProductTypeController : BaseController
    {
        //
        // GET: /ProductType/

        public ActionResult Index()
        {
            ViewBag.parenttypes = ListToSelect(ProductCategoryService.GetParentTypes().Select(o => new SelectListItem() { Value = o.CategorySN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.states = EnumToSelect(typeof(EnableState), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var ls = ProductCategoryService.FindPageList(Request.Params, out count);
            return ToDataGrid(ls, ls.Count);
        }

        public ActionResult Save(int? id, int? CategoryPSN, string CategoryPSNTitle, string CategoryPSN_0)
        {
            ViewBag.parenttypes = ListToSelect(ProductCategoryService.GetParentTypes().Select(o => new SelectListItem() { Value = o.CategorySN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            var obj = new ProductCategory() { State = 1 };
            SelectListItem category = null;
            if (id.HasValue)
            {
                obj = ProductCategoryService.GetObj(id.Value);
                category = new SelectListItem { Value = obj.CategoryPSN.ToString(), Text = obj.CategoryPSNTitle, Selected = true };
            }
            else if (CategoryPSN.HasValue)
                category = new SelectListItem() { Value = CategoryPSN.ToString(), Text = CategoryPSNTitle, Selected = true };
            ViewBag.category = category;
            ViewData["SelectPSN"] = CategoryPSN_0;
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.产品种类_编辑种类)]
        public ActionResult Save(ProductCategory obj)
        {
            var re = new OpResult();
            obj.CompanyId = CommonService.CompanyId;
            if (ProductCategoryService.IsExist(o => o.CompanyId == obj.CompanyId && o.CategoryPSN == obj.CategoryPSN && o.Title == obj.Title && o.Id != obj.Id))
                re.Message = "已存在该名称";
            else if (obj.Id == 0)
            {
                var max = ProductCategoryService.MaxSn;
                obj.CategorySN = max;
                obj.Grade = GetGrade;
                obj.CategoryCode = ProductCategoryService.MaxCode(obj.CategoryPSN);
                re = ProductCategoryService.Add(obj);
                #region 操作日志
                var msg = LogEngine.CompareModelToLog<ProductCategory>(LogModule.种类管理, obj);
                new LogEngine().WriteInsert(msg, LogModule.种类管理);
                #endregion
            }
            else
            {
                var supp = ProductCategoryService.FindById(obj.Id);
                obj.ToCopyProperty(supp);
                //supp.Grade = GetGrade;
                var oProCate = new ProductCategory();
                ExtendHelper.CopyProperty<ProductCategory>(oProCate, supp);
                re = ProductCategoryService.Update(supp);
                #region 操作日志
                var msg = LogEngine.CompareModelToLog<ProductCategory>(LogModule.种类管理, supp, oProCate);
                new LogEngine().WriteUpdate(msg, LogModule.种类管理);
                #endregion
            }
            if (re.Successed)
            {
                var stores = string.Join(",", WarehouseService.GetList().Select(o => o.StoreId));
                //Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = CommonService.CompanyId, StoreId = stores, Target = "ProductCategory" });
            }
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            var list = ProductCategoryService.FindList(o => ids.Contains(o.Id));
            var re = ProductCategoryService.Delete(list);
            #region 操作日志
            foreach (var item in list)
            {
                var msg = LogEngine.CompareModelToLog<ProductCategory>(LogModule.种类管理, null, item);
                new LogEngine().WriteDelete(msg, LogModule.种类管理);
            }
            #endregion
            return new JsonNetResult(re);
        }
        [HttpPost]
        public ActionResult SetState(string ids, short state)
        {
            var sid = ids.Split(',').Select(o => Int64.Parse(o));
            var list = ProductCategoryService.FindList(o => sid.Contains(o.Id));
            list.ForEach(o => { o.State = state; });
            var re = ProductCategoryService.Update(list);
            if (re.Successed)
            {
                var stores = string.Join(",", WarehouseService.GetList().Select(o => o.StoreId));
                // Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = CommonService.CompanyId, StoreId = stores, Target = "ProductCategory" });
            }
            return new JsonNetResult(re);
        }
        #region 类别联动选择
        public ActionResult ParentCategory(bool showAll = false, string emptyTitle = "请选择")
        {
            var parenttypes = ProductCategoryService.GetParentCategorys(showAll).Select(o => new DropdownItem(o.CategorySN.ToString(), o.Title)).ToList();
            parenttypes.Insert(0, new DropdownItem("", Server.UrlDecode(emptyTitle), true));
            return new JsonNetResult(parenttypes);
        }
        public ActionResult ChildCategory(int? psn, string emptyTitle = "请选择")
        {
            var childtypes = new List<DropdownItem>();
            if (psn.HasValue)
                childtypes = ProductCategoryService.GetChildCategorys(psn.Value).Select(o => new DropdownItem(o.CategorySN.ToString(), o.Title)).ToList();
            if (!emptyTitle.IsNullOrEmpty())
                childtypes.Insert(0, new DropdownItem("", Server.UrlDecode(emptyTitle), true));
            return new JsonNetResult(childtypes);
        }
        #endregion
        short GetGrade
        {
            get
            {
                var val = Request.Form["CategoryPSN_0"];
                if (val.IsNullOrEmpty()) val = Request["CategoryPSN_0"];
                if (val.IsNullOrEmpty()) return 1;
                var vals = val.Split(',');
                short grade = 0;
                for (int i = vals.Length; i > 0; i--)
                {
                    if (!vals[i - 1].IsNullOrEmpty())
                    {
                        grade = (short)(i + 1); break;
                    }
                }
                return grade;
            }
        }

        /// <summary>
        /// 产品类别下拉多选树控件数据源
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FindTreeListToMultiSelect()
        {
            int count = 0;
            var list = ProductCategoryService.FindPageList(Request.Params, out count, false);
            list.Each(o =>
            {
                o.OnOff = "closed";
                o.Childrens.Each(i =>
                {
                    i.OnOff = i.Childrens != null && i.Childrens.Any() ? "closed" : "open";
                });
            });
            var trees = new List<ProductCategory>(){
                new ProductCategory(){
                Id = 0,
                Title = "全部",
                OnOff="open",
                Childrens = new List<ProductCategory>()
                }
            };
            trees[0].Childrens.AddRange(list);
            return new JsonNetResult(trees);
        }
        [HttpPost]
        public ActionResult AddCategoryTitle(string title)
        {
            var op = new OpResult() { Successed = true };
            if (!title.IsNullOrEmpty())
            {
                short i = 1;
                int psn = 0;
                var list = new List<ProductCategory>();
                foreach (var text in title.Split('/'))
                {
                    if (text.IsNullOrEmpty()) continue;
                    var obj = Pharos.Logic.BLL.ProductCategoryService.Find(o => o.CompanyId == CommonService.CompanyId && o.Grade == i && o.Title == text);
                    if (obj == null)
                    {
                        obj = new ProductCategory() { CompanyId = CommonService.CompanyId, Grade = i, CategoryPSN = psn, CategorySN = ProductCategoryService.MaxSn, Title = text, CategoryCode = ProductCategoryService.MaxCode(psn) };
                        if (ProductCategoryService.Add(obj).Successed)
                        {
                            psn = obj.CategorySN;
                            i++;
                            op.Message = obj.CategorySN.ToString();
                        }
                        else
                        {
                            op.Message = "";
                            break;
                        }
                    }
                }
            }
            return Content(op.ToJson());
        }
    }
}
