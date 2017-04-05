﻿using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
namespace Pharos.OMS.Retailing.Controllers
{
    public class ProductDataVerController : BaseController
    {
        #region 私有对象
        [Ninject.Inject]
        ProductVerService ProductVerService { get; set; }
        [Ninject.Inject]
        ProductDataVerService ProductDataVerService { get; set; }
        [Ninject.Inject]
        ProductModelVerService ProductModelVerService { get; set; }
        #endregion
        #region 首页
        [SysPermissionValidate]
        public ActionResult Index()
        {
            ViewBag.products= ListToSelect(ProductVerService.GetList(1,2).Select(o => new SelectListItem() { Text ="（"+o.ProductId+"）"+ o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "全部");
            return View();
        }

        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = ProductDataVerService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }

        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            return new JsonNetResult(ProductDataVerService.Deletes(ids));
        }
        #endregion
        
        #region 发布
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="verId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Publish(string verId, short state)
        {
            var re = ProductDataVerService.Publish(verId, state);
            return new OpActionResult(re);
        }
        #endregion
        [HttpPost]
        public ActionResult Copy(string verId)
        {
            var re = ProductDataVerService.Copy(verId);
            return new OpActionResult(re);
        }
        [HttpPost]
        public ActionResult GetMenus(int productId)
        {
            var obj= ProductModelVerService.GetOfficialLast(productId);
            var list=new List<DropdownItem>();
            if (obj != null)
            {
                list = obj.ProductMenuLimits.Where(o => o.PMenuId <= 0).Select(o => new DropdownItem() { Text = o.Title, Value = o.MenuId.ToString() }).ToList();
                //if (list.Any()) list[0].IsSelected = true;
            }
            return new JsonNetResult(new {modelId=obj.ModuleId, list=list });
        }
        public ActionResult Save(string verId)
        {
            string modelId = "";
            var obj = ProductDataVerService.Get(verId);
            if(obj!=null)
            {
                ViewBag.products = ListToSelect(ProductVerService.GetList().Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "请选择", selectValue: obj.ProductId);
                ViewBag.state = obj.VerStatusTitle + "，" + obj.StatusTitle;
                ViewBag.status = obj.VerCode == 0 ? "--" : "v" + obj.VerCode.ToString("f1");
                modelId = obj.ModuleId;
            }
            else
            {
                ViewBag.products = ListToSelect(ProductDataVerService.GetProductVers().Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "请选择");
                ViewBag.state = "未发布，未生效";
                ViewBag.status = "--";
            }
            var pmenus = new List<DropdownItem>();
            if(!modelId.IsNullOrEmpty())
            {
                var model= ProductModelVerService.Get(modelId);
                if (model != null)
                    pmenus.AddRange(model.ProductMenuLimits.Where(o => o.PMenuId <= 0).OrderBy(o => o.SortOrder).Select(o => new DropdownItem() { Text = o.Title, Value = o.MenuId.ToString() }));
            }
            ViewBag.pmenus = pmenus.ToJson();
            ViewBag.modelId = modelId;
            return View();
        }
        [HttpPost]
        public ActionResult DataList(string verId)
        {
            var list = ProductDataVerService.DataList(verId);
            return ToDataGrid(list,0);
        }

        [HttpPost]
        public ActionResult SaveData(int id, string runSql)
        {
            var data = new ProductDataSql()
            {
                Id=id,
                RunSql = runSql
            };
            var re = ProductDataVerService.SaveData(data,0);
            return new OpActionResult(re);
        }
        [HttpPost]
        public ActionResult AppendData(int menuId, string runSql, string verId, int productId, string modelId)
        {
            var data = new ProductDataSql()
            {
                MenuId=menuId,
                DataId=verId,
                RunSql = runSql
            };
            var re = ProductDataVerService.SaveData(data, productId);
            return new OpActionResult(re);
        }
        [HttpPost]
        public void RemoveData(int id)
        {
            ProductDataVerService.RemoveData(id);
        }
        
        [HttpPost]
        public void MoveItem(short mode, int sn, string verId)
        {
            ProductDataVerService.MoveItem(mode, sn, verId);
        }

        public ActionResult SeeData(int id)
        {
            var obj= ProductDataVerService.SeeData(id);
            var sql = "";
            if (obj != null) sql = obj.RunSql;
            ViewBag.sql = sql;
            return View();
        }
    }
}
