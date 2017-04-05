﻿using Newtonsoft.Json;
using Pharos.Sys;
using Pharos.Logic;
using Pharos.Logic.BLL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Pharos.Logic.Entity;

namespace Pharos.CRM.Retailing.Controllers
{
    public class WholesalerController : BaseController
    {
        //
        // GET: /Wholesaler/

         public ActionResult Index()
        {
            return View();
        }
        public ActionResult Save(string id)
        {
            ViewBag.types = ListToSelect(SysDataDictService.GetWholesalerTypes().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            var obj = new Logic.Entity.Supplier() { MasterState=1,Designee=Sys.CurrentUser.UID,CreateDT=DateTime.Now};
            if (!id.IsNullOrEmpty())
            {
                obj = WholesalerBLL.FindById(id);
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult Save(Logic.Entity.Supplier obj)
        {
            var re= WholesalerBLL.SaveOrUpdate(obj);
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = WholesalerBLL.FindPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        /// <summary>
        /// 移动到指定类别
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="typeSn"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MoveType(string ids,int typeSn)
        {
            var sids=ids.Split(',');
            var list = WholesalerBLL.FindList(o => sids.Contains(o.Id));
            list.Each(o => { o.ClassifyId = typeSn; });
            return new JsonNetResult(WholesalerBLL.Update(list));
        }
        [HttpPost]
        public ActionResult Delete(string[] ids)
        {
            var list = WholesalerBLL.FindList(o => ids.Contains(o.Id));
            var re = new OpResult();        
            if (OutboundGoodsBLL.IsExist(o => ids.Contains(o.ApplyOrgId)))
                re.Message = "存在下单信息";         
            else
                re = WholesalerBLL.Delete(list);
            return new JsonNetResult(re);
        }

        /// <summary>
        /// 批发商所关联订单信息
        /// </summary>
        /// <param name="sId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadProductList(string sId)
        {
            int count = 0; 
            var nvl = new System.Collections.Specialized.NameValueCollection();
            var list = WholesalerBLL.FindOutboundGoodsList(sId, out count);
            return ToDataGrid(list, count);
        }
        #region 导入
        public ActionResult Import()
        {
            var obj = BaseService<ImportSet>.Find(o => o.CompanyId == CommonService.CompanyId && o.TableName == "Wholesaler");
            return View(obj ?? new ImportSet());
        }
        [HttpPost]
        public ActionResult Import(ImportSet imp)
        {
            imp.TableName = "Wholesaler";
            imp.CompanyId = CommonService.CompanyId;
            var op = SupplierService.Import(imp, Request.Files, Request["FieldName"], Request["ColumnName"],2);
            return Content(op.ToJson());
        }
        #endregion
        //[HttpPost]
        //public ActionResult IsExist(string id, string account)
        //{
        //    if (WholesalerBLL.IsExist(o => o.Id != id && o.MasterAccount == account))
        //        return Content("账号已存在!");
        //    return Content("");
        //}

    }

}