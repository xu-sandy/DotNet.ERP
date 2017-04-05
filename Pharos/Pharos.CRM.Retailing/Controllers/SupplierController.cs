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
    public class SupplierController : BaseController
    {
        //
        // GET: /Supplier/

        public ActionResult Index()
        {
            ViewBag.states = EnumToSelect(typeof(ContractState), emptyTitle: "全部");
            return View();
        }
        public ActionResult Save(string id)
        {
            ViewBag.types = ListToSelect(SysDataDictService.GetSupplierTypes().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            var obj = new Logic.Entity.Supplier() { MasterState=1,Designee=Sys.CurrentUser.UID,CreateDT=DateTime.Now};
            if (!id.IsNullOrEmpty())
            {
                obj = SupplierService.FindById(id);
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.供应商_编辑供应商)]
        public ActionResult Save(Logic.Entity.Supplier obj)
        {
            var re= SupplierService.SaveOrUpdate(obj);
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = SupplierService.FindPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        /// <summary>
        /// 移动到指定类别
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="typeSn"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.供应商_编辑分类)]
        public ActionResult MoveType(string ids, int typeSn)
        {
            var sids=ids.Split(',');
            var list = SupplierService.FindList(o => sids.Contains(o.Id));
            list.Each(o => { o.ClassifyId = typeSn; });
            return new JsonNetResult(SupplierService.Update(list));
        }
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.供应商_移除供应商)]
        public ActionResult Delete(string[] ids)
        {
            var list = SupplierService.FindList(o => ids.Contains(o.Id));
            var contracts= ContractBLL.FindList(o => ids.Contains(o.SupplierId));
             var re= new OpResult();
             if (contracts.Any())
                 re.Message = "存在以下合同号关联:" + string.Join(",", contracts.Select(o => o.ContractSN));
             else if (ProductService.IsExist(o => ids.Contains(o.SupplierId)))
                 re.Message = "存在商品关联";
             else if (OrderService.IsExist(o => ids.Contains(o.SupplierID)))
                 re.Message = "存在订单关联";
             else
                 re = SupplierService.Delete(list);
            return new JsonNetResult(re);
        }
        /// <summary>
        /// 供应商所关联商品信息
        /// </summary>
        /// <param name="sId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadProductList(string sId)
        {
            int count = 0; ;
            var nvl = new System.Collections.Specialized.NameValueCollection();
            nvl.Add("supplierId", sId);
            //var list = ProductService.LoadProductList(nvl, out count);
            var list = SupplierService.GetProductsBySupplierId(sId,out count);
            return ToDataGrid(list, count);
        }
        /// <summary>
        /// 供应商所关联合同信息
        /// </summary>
        /// <param name="sId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadContractList(string sId)
        {
            int count=0;
            var list = ContractSerivce.LoadContractList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        [HttpPost]
        public ActionResult IsExist(string id, string account)
        {
            var result= SupplierService.IsExist(o => o.Id != id && o.MasterAccount == account && o.CompanyId==CommonService.CompanyId);
            if (!result)
            {
                if(!account.IsNullOrEmpty())
                {
                    if(!id.IsNullOrEmpty())
                    {
                        var supp = SupplierService.FindById(id);
                        if (supp.MasterAccount != account && UserInfoService.IsExist(o => o.LoginName == account && o.CompanyId == CommonService.CompanyId))
                        {
                            result = true;
                        }
                    }
                    else if (UserInfoService.IsExist(o => o.LoginName == account && o.CompanyId == CommonService.CompanyId))
                    {
                        result = true;
                    }
                }
                
            }
            return Content(result?"false":"true");
        }
        #region 导入
        public ActionResult Import()
        {
            var obj = BaseService<ImportSet>.Find(o => o.CompanyId == CommonService.CompanyId && o.TableName == "Supplier");
            return View(obj ?? new ImportSet());
        }
        [HttpPost]
        public ActionResult Import(ImportSet imp)
        {
            imp.TableName = "Supplier";
            imp.CompanyId = CommonService.CompanyId;
            var op = SupplierService.Import(imp, Request.Files, Request["FieldName"], Request["ColumnName"],1);
            return Content(op.ToJson());
        }
        #endregion
    }
}
