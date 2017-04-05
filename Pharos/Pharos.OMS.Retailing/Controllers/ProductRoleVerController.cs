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
    public class ProductRoleVerController : BaseController
    {
        #region 私有对象
        [Ninject.Inject]
        ProductVerService ProductVerService { get; set; }
        [Ninject.Inject]
        ProductRoleVerService ProductRoleVerService { get; set; }
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
            var list = ProductRoleVerService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }

        public ActionResult Save(string verId)
        {
            if (!verId.IsNullOrEmpty())
            {
                var obj = ProductRoleVerService.Get(verId);
                ViewBag.products = ListToSelect(ProductVerService.GetList().Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "请选择", selectValue: obj.ProductId);
                ViewBag.state = obj.VerStatusTitle + "，" + obj.StatusTitle;
                ViewBag.status = obj.VerCode == 0 ? "--" : "v" + obj.VerCode.ToString("f1");
                if (!obj.ModuleId.IsNullOrEmpty())
                    ViewBag.usecode="v"+ ProductModelVerService.Get(obj.ModuleId).VerCode.ToString("f1");
                var model= ProductModelVerService.GetOfficialLast(obj.ProductId);
                ViewBag.newcode =model==null?"": "v" + model.VerCode.ToString("f1");
            }
            else
            {
                ViewBag.products = ListToSelect(ProductRoleVerService.GetProductVers().Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "请选择");
                ViewBag.state = "未发布，未生效";
                ViewBag.status = "--";
            }
            return View();
        }
        [HttpPost]
        public ActionResult Save(string roleVerId, string moduleId, int? productId, string title)
        {
            var re= ProductRoleVerService.SaveRole(roleVerId, moduleId, productId, title);
            return new OpActionResult(re);
        }
        [HttpPost]
        public ActionResult GetModelLast(int? productId)
        {
            var model= ProductModelVerService.GetOfficialLast(productId.GetValueOrDefault());
            return Content(model == null ? "" : "v" + model.VerCode.ToString("f1"));
        }
        [HttpPost]
        public ActionResult SaveLimits(string verId, int roleId, string menuIds, string limitIds)
        {
            var re = ProductRoleVerService.SaveLimits(verId, roleId, menuIds, limitIds);
            return new OpActionResult(re);
        }
        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            return new JsonNetResult(ProductRoleVerService.Deletes(ids));
        }
        [HttpPost]
        public ActionResult FindTreeList(string verId)
        {
            var list = ProductRoleVerService.FindTreeList(verId);
            return new JsonNetResult(list);
        }
        [HttpPost]
        public ActionResult FindMenuList(int? productId, int? roleId, string verId, short? detail)
        {
            if (productId.HasValue && roleId.HasValue)
            {
                var list = ProductRoleVerService.FindMenuList(productId.Value, roleId.Value, verId, detail==1);
                return new JsonNetResult(list);
            }
            return null;
        }
        [HttpPost]
        public void MoveMenuItem(short mode, int menuId,int roleId, string verId)
        {
            ProductRoleVerService.MoveMenuItem(mode, menuId, roleId,verId);
        }
        [HttpPost]
        public ActionResult Copy(string verId)
        {
            var re = ProductRoleVerService.Copy(verId);
            return new OpActionResult(re);
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
        public ActionResult Publish(string verId, short state)
        {
            var re = ProductRoleVerService.Publish(verId, state);
            return new OpActionResult(re);
        }
        #endregion
        public ActionResult RoleIndex()
        {
            return View();
        }
        public ActionResult RoleList(string verId)
        {
            var list = ProductRoleVerService.RoleList(verId);
            return ToDataGrid(list, 0);
        }
        public ActionResult SaveRole(int? id,string verId)
        {
            var role = new ProductRole() { RoleVerId = verId };
            if (id.HasValue)
                role = ProductRoleVerService.GetRole(id.Value);
            return View(role);
        }
        [HttpPost]
        public ActionResult SaveRole(ProductRole obj)
        {
            return new OpActionResult(ProductRoleVerService.SaveRole(obj));
        }
        [HttpPost]
        public ActionResult RemoveRole(int[] ids)
        {
            return new OpActionResult(ProductRoleVerService.RemoveRole(ids));
        }
    }
}
