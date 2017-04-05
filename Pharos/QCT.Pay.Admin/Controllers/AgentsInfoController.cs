﻿using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.EntityExtend;
using Pharos.Logic.OMS.Entity.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.OMS.BLL;

namespace QCT.Pay.Admin.Controllers
{
    public class AgentsInfoController : BaseController
    {

        //代理商档案BLL
        [Ninject.Inject]
        AgentsInfoService agentsInfoService { get; set; }

        //商户资料BLL
        [Ninject.Inject]
        TradersService tradersService { get; set; }

        //结算账户信息BLL
        [Ninject.Inject]
        BankCardInfoService bankCardInfoService { get; set; }

        //代理商支付渠道BLL
        [Ninject.Inject]
        AgentPayService agentPayService { get; set; }

        //代理商账号
        [Ninject.Inject]
        AgentsUsersService agentsUsersService { get; set; }

        public ActionResult FindPageList()
        {
            var count = 0;
            var list = agentsInfoService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }

        public ActionResult Index()
        {
            //日期
            ViewBag.dt = DateTime.Now.ToString("yyyy-MM-dd");
            var UserList = tradersService.getUserList().Select(o => new SelectListItem() { Value = o.UserId, Text = o.FullName });
            //指派人
            ViewBag.Ass = ListToSelect(UserList, emptyTitle: "请选择");
            //设定指派人
            ViewBag.upAss = ListToSelect(UserList, emptyTitle: "将所选项归类到指派人");

            return View();
        }

        /// <summary>
        /// 获取有效-起始日期、有效-终止日期
        /// </summary>
        /// <returns></returns>
        public string getStartEndDT(int DicSN)
        {
            if (DicSN != 0)
            {
                SysDataDictionary data = agentsInfoService.getData(DicSN);
                if (data != null)
                {
                    int year = Convert.ToInt32(data.Title.Replace("年",""));
                    string StartEndDT = "";
                    DateTime dtNow = DateTime.Now;
                    StartEndDT = dtNow.ToString("yyyy-MM-dd");
                    StartEndDT = StartEndDT + "|" + dtNow.AddYears(year).AddDays(1).ToString("yyyy-MM-dd");
                    return StartEndDT;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

        }

        public ActionResult Save(int? id)
        {
            //证件照
            string img = "";
            //结算账户
            BankCardInfo bankCardInfo = new BankCardInfo();
            //支付渠道
            AgentPay agentPay = new AgentPay();
            //是否存在下级代理商（0不存在，1是存在）
            string readOnly = "0";
            int _id = id == null ? 0 : Convert.ToInt32(id);
            bool isE = agentsInfoService.isExistPAgentsId(_id);
            if (isE)
            {
                readOnly = "1";
            }

            var obj = new AgentsInfo
            {
                
            };
            if (id.HasValue)
            {
                obj = agentsInfoService.GetOne(id.Value);
                img = Tool.getIdCardPhotoPath(obj.AgentsId) + obj.IdCardPhoto;
                bankCardInfo = bankCardInfoService.GetOne(obj.AgentsId);
                agentPay = agentPayService.GetOne(obj.AgentsId);
            }

            //有效期
            ViewBag.ValidityY = ListToSelect(tradersService.getDataList(450).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            //是否存在下级代理商（0不存在，1是存在）
            ViewBag.ReadOnly = readOnly;
            //证件照
            ViewBag.img = img;
            //结算账户
            ViewBag.BankCardInfo = bankCardInfo;
            //支付渠道
            ViewBag.AgentPay = agentPay;
            //交易支付通道
            ViewBag.PayC = ListToSelect(agentsInfoService.GetPayApiList().Select(o => new SelectListItem() { Value = o.ApiNo.ToString(), Text = o.ApiName }), emptyTitle: "请选择");
            //指派人
            ViewBag.user = ListToSelect(tradersService.getUserList().Select(o => new SelectListItem() { Value = o.UserId, Text = o.FullName}), emptyTitle: "请选择");
            //代理商类型
            ViewBag.ty = ListToSelect(tradersService.getDataList(457).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            return View(obj.IsNullThrow());
        }

        [HttpPost]
        public ActionResult Save(AgentsInfo agentsInfo)
        {
            HttpFileCollectionBase files= Request.Files;
            HttpPostedFileBase PostedFileBase=files[0];

            var op = agentsInfoService.Save(agentsInfo, PostedFileBase,Request.Params);
            return new OpActionResult(op);
        }

        /// <summary>
        /// 获取代理商编号
        /// </summary>
        /// <returns></returns>
        public ActionResult getAgentsIdWhere()
        {
            var list = agentsInfoService.GetListWhere(Request.Params);
            return ToDataGrid(list, 0);
        }

        public ActionResult Verification(int id)
        {
            int AgentsId = 0;
            AgentsInfo agentsInfo = agentsInfoService.GetOne(id);
            if (agentsInfo != null)
            {
                AgentsId = agentsInfo.AgentsId;
            }
            var op = agentsInfoService.Verification(Request.Params,AgentsId);
            return new OpActionResult(op);
        }

        //选择交易支付通道
        public string SelPayApi(int ApiNo)
        {
            PayApi payApi = new PayApi();
            if (ApiNo > 0)
            {
                payApi = agentsInfoService.GetPayApiList(o=>o.ApiNo==ApiNo).FirstOrDefault();
            }
            return payApi.ToJson();
        }

        /// <summary>
        /// 设定指派人
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult upAssignUid(string ids, string AssignUid)
        {
            return new JsonNetResult(agentsInfoService.upAssignUid(ids, AssignUid));
        }


        /// <summary>
        /// 终止所选代理商
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StopAgents(string ids)
        {
            return new JsonNetResult(agentsInfoService.StopAgents(ids));
        }

        /// <summary>
        /// 代理商是否存在未到期
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string isExpires(string ids)
        {
            string s = "0";
            if (agentsInfoService.isExpires(ids))
            {
                s = "1";
            }
            return s;
        }


        public ActionResult Renewal(string ids)
        {
            ViewBag.ids = ids;
            //续签年
            ViewBag.ValidityY = ListToSelect(tradersService.getDataList(450).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            return View();
        }

        /// <summary>
        /// 续签代理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Renewal()
        {
            return new JsonNetResult(agentsInfoService.Renewal(Request.Params));
        }

        /// <summary>
        /// 设定主账号
        /// </summary>
        /// <returns></returns>
        public ActionResult SettingUser(string ids)
        {
            string FullName = "";
            string LoginName = "";
            string usersFullName = "";
            AgentsInfo agentsInfo = agentsInfoService.GetOne(ids);
            if (agentsInfo != null)
            {
                FullName = agentsInfo.FullName;
                AgentsUsers agentsUsers = agentsUsersService.GetAgentsUsersList(o => o.AgentsId == agentsInfo.AgentsId && o.AgentType == 1).FirstOrDefault();
                if (agentsUsers != null)
                {
                    LoginName = agentsUsers.LoginName;
                    usersFullName = agentsUsers.FullName;
                }
            }

            //代理商全称
            ViewBag.FullName = FullName;
            //主登录账号
            ViewBag.LoginName = LoginName;
            //姓名
            ViewBag.usersFullName = usersFullName;
            return View();
        }

        /// <summary>
        /// 设定主账号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SettingUser()
        {
            return new JsonNetResult(agentsInfoService.SettingUser(Request.Params));
        }
    }
}
