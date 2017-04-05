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
    public class ProductModelVerController : BaseController
    {
        #region 私有对象
        [Ninject.Inject]
        ProductVerService ProductVerService { get; set; }
        [Ninject.Inject]
        ProductModelVerService ProductModelVerService { get; set; }
        #endregion
        #region 首页
        [SysPermissionValidate]
        public ActionResult Index()
        {
            ViewBag.products = ListToSelect(ProductVerService.GetList(1, 2).Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "全部");
            return View();
        }

        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = ProductModelVerService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }

        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            return new JsonNetResult(ProductModelVerService.Deletes(ids));
        }
        #endregion
        
        #region 发布
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Publish(string modelId, short state)
        {
            var re = ProductModelVerService.Publish(modelId, state);
            return new OpActionResult(re);
        }
        #endregion
        [HttpPost]
        public ActionResult Copy(string modelId)
        {
            var re = ProductModelVerService.Copy(modelId);
            return new OpActionResult(re);
        }

        #region 菜单
        public ActionResult Menus(string modelId)
        {
            var obj = ProductModelVerService.Get(modelId);

            if(obj!=null)
            {
                ViewBag.products = ListToSelect(ProductVerService.GetList().Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "请选择", selectValue: obj.ProductId);
                ViewBag.state = obj.VerStatusTitle + "，" + obj.StatusTitle;
                ViewBag.status = obj.VerCode == 0 ? "--" : "v" + obj.VerCode.ToString("f1");
            }
            else
            {
                ViewBag.products = ListToSelect(ProductModelVerService.GetProductVers().Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "请选择");
                ViewBag.state = "未发布，未生效";
                ViewBag.status = "--";
            }
            return View();
        }
        [HttpPost]
        public ActionResult MenuList(string modelId)
        {
            var list = ProductModelVerService.MenuList(modelId);
            return ToDataGrid(list,0);
        }

        public ActionResult SaveMenu(int? menuid, int productid, int? pmenuId, string modelId)
        {
            var obj = new ProductMenuLimit() { ProductId = productid,PMenuId=pmenuId.GetValueOrDefault(),ModuleId=modelId,Type=1,Status=true };
            if (menuid.HasValue)
            {
                obj = ProductModelVerService.GetMenu(menuid.Value, modelId);
            }
            if(pmenuId.HasValue)
            {
                var pm = ProductModelVerService.GetMenu(pmenuId.Value, modelId);
                if (pm != null) ViewBag.ParentMenu = pm.Title;
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult SaveMenu(ProductMenuLimit menu)
        {
            var re = ProductModelVerService.SaveMenu(menu);
            return new OpActionResult(re);
        }
        [HttpPost]
        public void RemoveMenu(int menuId, string modelId)
        {
            ProductModelVerService.RemoveMenu(menuId, modelId);
        }
        [HttpPost]
        public void MoveMenuItem(short mode, int menuId, string modelId)
        {
            if (mode == 1 || mode == 2)
                ProductModelVerService.MoveMenuItem(mode, menuId, modelId);
            else
                ProductModelVerService.MoveUpMenuItem(mode, menuId, modelId);
        }
        #endregion

        #region 权限
        public ActionResult Limits(string modelId)
        {
            var obj= ProductModelVerService.Get(modelId);
            obj.IsNullThrow();
            ViewBag.products = ListToSelect(ProductVerService.GetList().Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "请选择", selectValue: obj.ProductId);
            ViewBag.state = obj.VerStatusTitle + "，" + obj.StatusTitle;
            ViewBag.status = obj.VerCode == 0 ? "--" : "v" + obj.VerCode.ToString("f1");
            return View();
        }
        [HttpPost]
        public ActionResult LimitList(string modelId)
        {
            var list = ProductModelVerService.LimitList(modelId);
            return ToDataGrid(list, 0);
        }

        public ActionResult SaveLimit(int? limitid, int? productid, int? pmenuId, string modelId)
        {
            var obj = new ProductMenuLimit() { ProductId = productid.GetValueOrDefault(), PMenuId = pmenuId.GetValueOrDefault(),Type=3, Status = true,ModuleId=modelId };
            if (limitid.HasValue)
            {
                obj = ProductModelVerService.GetLimit(limitid.Value, modelId);
            }
            if (pmenuId.HasValue)
            {
                var pm = ProductModelVerService.GetMenu(pmenuId.Value, modelId);
                if (pm != null) ViewBag.ParentMenu = pm.Title;
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult SaveLimit(ProductMenuLimit menu)
        {
            var re = ProductModelVerService.SaveMenu(menu);
            return new OpActionResult(re);
        }
        [HttpPost]
        public void RemoveLimit(int limitId, string modelId)
        {
            ProductModelVerService.RemoveMenu(limitId, modelId);
        }
        [HttpPost]
        public void MoveLimitItem(short mode, int limitId, string modelId)
        {
            ProductModelVerService.MoveLimitItem(mode, limitId, modelId);
        }
        [HttpPost]
        public void SetLimitState(short mode, int limitId, string modelId)
        {
            ProductModelVerService.SetLimitState(mode, limitId, modelId);
        }
        #endregion
    }
}
