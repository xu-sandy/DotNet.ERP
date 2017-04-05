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
    public class ProductDictionaryVerController : BaseController
    {
        #region 私有对象
        [Ninject.Inject]
        ProductVerService ProductVerService { get; set; }
        [Ninject.Inject]
        ProductDictionaryVerService ProductDictionaryVerService { get; set; }
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
            var list = ProductDictionaryVerService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }

        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            return new JsonNetResult(ProductDictionaryVerService.Deletes(ids));
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
            var re = ProductDictionaryVerService.Publish(verId, state);
            return new OpActionResult(re);
        }
        #endregion
        [HttpPost]
        public ActionResult Copy(string verId)
        {
            var re = ProductDictionaryVerService.Copy(verId);
            return new OpActionResult(re);
        }

        public ActionResult Save(string verId)
        {
            var obj = ProductDictionaryVerService.Get(verId);
            if(obj!=null)
            {
                ViewBag.products = ListToSelect(ProductVerService.GetList().Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "请选择", selectValue: obj.ProductId);
                ViewBag.state = obj.VerStatusTitle + "，" + obj.StatusTitle;
                ViewBag.status = obj.VerCode == 0 ? "--" : "v" + obj.VerCode.ToString("f1");
            }
            else
            {
                ViewBag.products = ListToSelect(ProductDictionaryVerService.GetProductVers().Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "请选择");
                ViewBag.state = "未发布，未生效";
                ViewBag.status = "--";
            }
            return View();
        }
        [HttpPost]
        public ActionResult DataList(string verId,int? psn)
        {
            var list = ProductDictionaryVerService.DataList(verId,psn);
            return ToDataGrid(list,0);
        }

        public ActionResult SaveData(int? psn, string verId,short status=1)
        {
            if (psn.HasValue)
            {
                var pm = ProductDictionaryVerService.GetData(verId, psn.Value);
                if (pm != null) ViewBag.Parent = pm.Title;
            }
            return View();
        }
        [HttpPost]
        public ActionResult SaveData(int id,string title,bool hasChild=false)
        {
            var data = new ProductDictionaryData()
            {
                Id=id,
                Title = title,
                HasChild= hasChild
            };
            var re = ProductDictionaryVerService.SaveData(data,0);
            return new OpActionResult(re);
        }
        [HttpPost]
        public ActionResult AppendData(string title,short status, string verId, int productId,bool hasChild=false,int psn=0)
        {
            var data = new ProductDictionaryData()
            {
                Title=title,
                DictId=verId,
                Status=status,
                HasChild=hasChild,
                DicPSN=psn
            };
            var re = ProductDictionaryVerService.SaveData(data, productId);
            return new OpActionResult(re);
        }
        [HttpPost]
        public void RemoveData(int sn, string verId)
        {
            ProductDictionaryVerService.RemoveData(sn, verId);
        }
        [HttpPost]
        public void SetState(int sn,short state, string verId)
        {
            ProductDictionaryVerService.SetState(sn,state, verId);
        }
        [HttpPost]
        public void MoveItem(short mode, int sn, string verId)
        {
            ProductDictionaryVerService.MoveItem(mode, sn, verId);
        }

        
    }
}