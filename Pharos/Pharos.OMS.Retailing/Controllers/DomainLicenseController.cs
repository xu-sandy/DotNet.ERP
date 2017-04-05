﻿using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Entity.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.OMS.BLL;
using System.Data;

namespace Pharos.OMS.Retailing.Controllers
{
    public class DomainLicenseController : BaseController
    {
        [Ninject.Inject]
        //商户资料
        TradersService tradersService { get; set; }

        [Ninject.Inject]
        //BLL商家支付许可档案
        PayLicenseService payLicenseService { get; set; }

        [Ninject.Inject]
        /// <summary>
        /// BLL商户域名信息
        /// </summary>
        RealmService realmService { get; set; }

        [SysPermissionValidate]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(int? id)
        {
            var obj = new Realm
            {

            };
            if (id.HasValue)
            {
                obj = realmService.GetEntityById(Convert.ToInt32(id));
                if (obj.CID > 0)
                {
                    Traders traders = tradersService.GetEntityByWhere(o => o.CID == obj.CID && o.Status == 1);
                    if (traders != null)
                    {
                        obj.TradersFullTitle = traders.FullTitle;
                    }
                }
            }
            //一级域名
            ViewBag.d1 = ListToSelect(tradersService.getDataList(600).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            return View(obj.IsNullThrow());
        }

        [HttpPost]
        public ActionResult Save(int Id, short Category, string Name)
        {
            Realm realm = new Realm();
            if (Id == 0)
            {
                realm.State = -1;
                realm.CreateDT = DateTime.Now;
            }
            else
            {
                realm = realmService.GetEntityById(Id);
                //外部
                if (Category == 2)
                {
                    realm.CID = 0;
                }
            }
            if (!Name.IsNullOrEmpty())
            {
                realm.Name = Name.ToLower();
            }
            string[] s = new string[] { "CID", "Domain1", "Category", "Remark" };
            TryUpdateModel<Realm>(realm,s);
            var op = realmService.Save(realm, Id, Request.Params);
            return new OpActionResult(op);
        }

        /// <summary>
        /// 获取CID
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult GetCidWhere(string keyword)
        {
            var list = payLicenseService.GetCIDWhere(Request.Params);
            return ToDataGrid(list, 0);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpState(string ids, short state)
        {
            return new JsonNetResult(realmService.UpState(ids, state));
        }

        /// <summary>
        /// 添加保留域名前缀
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Retain(string Name)
        {
            Realm realm = new Realm();
            realm.Name = Name.Trim();
            realm.CID = -1;
            realm.State = 1;
            realm.CreateDT = DateTime.Now;
            if (!Name.IsNullOrEmpty())
            {
                realm.Name = Name.ToLower();
            }
            return new JsonNetResult(realmService.AddRetain(realm));
        }

        /// <summary>
        /// 保留域名前缀
        /// </summary>
        /// <returns></returns>
        public ActionResult Retain()
        {
            var list = realmService.GetListByWhere(o=>o.CID==-1).OrderByDescending(o=>o.CreateDT).ToList();
            ViewBag.list = list;
            return View();
        }

        /// <summary>
        /// 删除保留域名
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DeleteRetain(int Id)
        {
            OpResult op = realmService.DeleteByWhere(o=>o.Id==Id);
            return new JsonNetResult(op);
        }

        /// <summary>
        /// 获取商户资料
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public string GetTradersFullTitle(int CID = 0)
        {
            return realmService.getTradersFullTitle(CID);
        }

        public ActionResult FindPageList()
        {
            var count = 0;
            var list = realmService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
    }
}
