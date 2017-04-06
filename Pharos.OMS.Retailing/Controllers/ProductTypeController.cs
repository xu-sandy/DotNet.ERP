using Pharos.Logic;
using Pharos.Logic.OMS;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Pharos.OMS.Retailing.Controllers
{
    public class ProductTypeController : BaseController
    {
        [Ninject.Inject]
        ProductCategoryService ProductCategoryService { get; set; }
        public ActionResult Index()
        {
            ViewBag.parenttypes = ListToSelect(ProductCategoryService.GetFirstList().Select(o => new SelectListItem() { Value = o.CategorySN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.states = EnumToSelect(typeof(EnableState), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var ls = ProductCategoryService.GetPageList(Request.Params, out count);
            return ToDataGrid(ls, ls.Count());
        }

        public ActionResult Save(int? id, int? CategoryPSN, string CategoryPSNTitle, string CategoryPSN_0)
        {
            ViewBag.parenttypes = ListToSelect(ProductCategoryService.GetFirstList().Select(o => new SelectListItem() { Value = o.CategorySN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            var obj = new ProductCategory() { State = 1,Source=1 };
            SelectListItem category = null;
            if (id.HasValue)
            {
                obj = ProductCategoryService.GetOne(id.Value);
                category = new SelectListItem { Value = obj.CategoryPSN.ToString(), Text = obj.CategoryPSNTitle, Selected = true };
            }
            else if (CategoryPSN.HasValue)
                category = new SelectListItem() { Value = CategoryPSN.ToString(), Text = CategoryPSNTitle, Selected = true };
            ViewBag.category = category;
            ViewData["SelectPSN"] = CategoryPSN_0;
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult Save(ProductCategory obj)
        {
            var re = ProductCategoryService.SaveOrUpdate(obj);
            return new OpActionResult(re);
        }
        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            var re = ProductCategoryService.Deletes(ids);
            return new OpActionResult(re);
        }
        [HttpPost]
        public ActionResult SetState(string ids, short state)
        {
            var re = ProductCategoryService.SetState(ids,state);
            return new OpActionResult(re);
        }
        #region 类别联动选择
        public ActionResult ParentCategory(bool showAll = false, string emptyTitle = "请选择")
        {
            var parenttypes = ProductCategoryService.GetFirstList().Select(o => new DropdownItem(o.CategorySN.ToString(), o.Title)).ToList();
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
        
        
    }
}
