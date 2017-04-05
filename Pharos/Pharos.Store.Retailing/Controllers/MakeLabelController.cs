﻿using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Sys;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.Store.Retailing.Controllers
{
    public class MakeLabelController : BaseController
    {
        //
        // GET: /MakeLabel/

        public ActionResult Index()
        {
            ViewBag.stores = ListToSelect(WarehouseService.GetAdminList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title, Selected = o.StoreId == CurrentUser.StoreId }));
            return View();
        }
        /// <summary>
        /// 菜单树
        /// </summary>
        /// <returns></returns>
        public ActionResult FindTreeList()
        {
            int count = 0;
            var list = ProductCategoryService.FindPageList(Request.Params, out count, false);
            list.Each(o =>
            {
                o.Childrens.Each(i =>
                {
                    i.OnOff = i.Childrens != null && i.Childrens.Any() ? "closed" : "open";
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
        /// <summary>
        /// 根据菜单选择的类别 加载商品
        /// </summary>
        /// <returns></returns>
        public ActionResult FindProductByCategory(int? category, string storeId, string searchText)
        {
            int count = 0;
            var result = ProductService.GetProductByStore(category.GetValueOrDefault(), storeId, 1, searchText, out count);
            return ToDataGrid(result, count);
        }
        [HttpPost]
        public ActionResult Export(string datas, int? printCount)
        {
            var list = datas.ToObject<List<VwProductModel>>();
            if (printCount > 1)
            {
                list.Each(o => { o.PrintCount = printCount; o.SysPriceStr = string.Format("{0:N}", o.SysPrice); });
            }
            else
            {
                list.Each(o => o.SysPriceStr = string.Format("{0:N}", o.SysPrice));
            }
            var ls = new List<VwProductModel>();
            list.Each(o =>
            {
                for (int i = 1; i <= o.PrintCount; i++)
                {
                    ls.Add(o);
                }
            });
            var dt = ls.ToDataTable<VwProductModel>();
            var fileName = "制作价签";
            var fields = new List<string>() { "ProductCode", "Barcode", "CategoryTitle", "Title", "Size", "BrandTitle", "Factory", "CityTitle", "SysPrice", "SysPriceStr", "SubUnit" };
            var names = new List<string>() { "货号", "条码", "品类", "品名", "规格", "品牌", "厂家", "原产地", "售价", "售价", "单位" };
            new ExportExcel() { IsBufferOutput = true, HeaderText = "" }.ToExcel(fileName, dt, fields.ToArray(), names.ToArray(), null, null, null, true, false);
            return Content(OpResult.Success().ToJson());
        }
    }
}
