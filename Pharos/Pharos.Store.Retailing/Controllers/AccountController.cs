﻿using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using Pharos.Utility;
using Pharos.Logic;
using Pharos.Logic.BLL;
using Pharos.Utility.Helpers;
using System;
using Pharos.Sys.BLL;
using Pharos.Sys;
using Pharos.Store.Retailing;
using Pharos.Store.Retailing.Models;
using Pharos.Sys.Entity;

namespace Pharos.Store.Retailing.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            //if (CurrentUser.IsLogin)
            //{
            //    //已登录，则直接进入主界面
            //    return Redirect(Url.Action("Index", "Home"));
            //}
            //1为单商户版本，其他值为多商户版本
            string ver = Pharos.Utility.Config.GetAppSettings("ver");
            //单商户版本
            if (ver == "1")
            {
                var user = new UserLogin();
                if (Cookies.IsExist("remuc"))
                {
                    user.UserName = Cookies.Get("remuc", "_uname");
                    user.UserPwd = Cookies.Get("remuc", "_pwd");
                    user.RememberMe = true;
                }
                return View(user);
            }
            //多商户版本
            else
            {
                return Logins();
            }


        }

        [HttpPost]
        public ActionResult Login(UserLogin user)
        {

            //1为单商户版本，其他值为多商户版本
            string ver = Pharos.Utility.Config.GetAppSettings("ver");
            //单商户版本
            if (ver == "1")
            {
                if (!ModelState.IsValid) return View(user);
                var op = Authorize.LoinValidator(CommonService.CompanyId);
                if (!op.Successed)
                {
                    ViewBag.msg = op.Message;
                    return View(user);
                }

                var pwd = Pharos.Utility.Security.MD5_Encrypt(user.UserPwd);
                var obj = BaseService<SysStoreUserInfo>.Find(o => o.CompanyId == CommonService.CompanyId && o.LoginName == user.UserName && o.LoginPwd == pwd && o.Status == 1);
                if (obj == null)
                {
                    ViewBag.msg = "帐户或密码输入不正确!";
                    return View(user);
                }
                new Sys.CurrentStoreUser().StoreLogin(obj, Sys.SysCommonRules.CurrentStore, user.RememberMe);
                return Redirect(Url.Action("Index", "Home"));
            }
            //多商户版本
            else
            {
                return Logins(user);
            }

        }

        #region 个人信息
        public ActionResult UserInfo()
        {
            var userBll = new Pharos.Sys.BLL.SysUserInfoBLL();
            var model = userBll.GetModelByUID(Sys.CurrentStoreUser.UID);
            List<SelectListItem> storeRoles = null;
            if (!Sys.CurrentStoreUser.StoreId.IsNullOrEmpty())
            {
                var user = userBll.GetStoreUser(Sys.CurrentStoreUser.StoreId, null, 0).FirstOrDefault(o => o.UID == Sys.CurrentStoreUser.UID);
                if (user != null && !user.OperateAuth.IsNullOrEmpty())
                {
                    var auths = user.OperateAuth.Split('|');
                    storeRoles = new List<SelectListItem>();
                    foreach (var o in auths)
                    {
                        if (o.IsNullOrEmpty()) continue;
                        if (o.StartsWith(Sys.CurrentStoreUser.StoreId + ","))
                        {
                            var role = o.Substring(o.LastIndexOf(',') + 1);
                            storeRoles.Add(new SelectListItem()
                            {
                                Value = role,
                                Text = role == "1" ? "店长" : role == "2" ? "营业员" : role == "3" ? "收银员" : "数据维护",
                                Selected = true
                            });
                        }
                    }
                }
            }
            //ViewBag.BumenTitle = new Pharos.Sys.BLL.SysDepartmentBLL().GetModelByDepId(model.BumenId).Title;
            //ViewBag.PositionTitle = new Pharos.Sys.BLL.SysDataDictionaryBLL().GetExtModelByDicSN(model.PositionId).Title;
            var roleBLL = new Pharos.Sys.BLL.SysRoleBLL();
            var roleIdArray = model.RoleIds.Split(',');
            var roleTitle = string.Empty;
            var roleStr = string.Empty;
            foreach (var item in roleIdArray)
            {
                roleTitle = roleBLL.GetModelByRoleId(int.Parse(item)).Title;
                if (string.IsNullOrEmpty(roleStr)) roleStr = roleTitle;
                else roleStr += "、" + roleTitle;
            }
            ViewBag.RoleStr = roleStr;
            ViewBag.StoreRoles = storeRoles;
            return View(model);
        }
        [HttpPost]
        public ActionResult UserInfo(int Id, string LoginPwd)
        {
            var userBLL = new Pharos.Sys.BLL.SysUserInfoBLL();
            var model = userBLL.GetStoreUserInfoByUId(CurrentStoreUser.UID);
            model.LoginPwd =Utility.Security.MD5_Encrypt( LoginPwd);
            var result = userBLL.UpdateStoreUser(model);
            return Content(result.ToJson());
        }
        #endregion

        #region 多商户登录
        //多商户登录
        public ActionResult Logins()
        {
            //cid是否只读
            int isReadOnly = 0;
            //登录logo
            string logo = "/Content/mythemes/default/images/login3/logo.png";
            //电话
            string phone = Pharos.Utility.Config.GetAppSettings("phone");

            var user = new UserLogin();

            //二级域名
            string d = "";

            //二级域名
            string dom = "";
            if (!RouteData.Values["dom"].IsNullOrEmpty())
            {
                dom = RouteData.Values["dom"].ToString();
            }
            //一级域名
            string d1 = "";
            if (!RouteData.Values["d1"].IsNullOrEmpty())
            {
                d1 = RouteData.Values["d1"].ToString();
            }
            //顶级域名
            string d0 = "";
            if (!RouteData.Values["d0"].IsNullOrEmpty())
            {
                d0 = RouteData.Values["d0"].ToString();
            }

            if (!d0.IsNullOrEmpty())
            {
                if (!dom.IsNullOrEmpty())
                {
                    d = dom;
                }
            }

            //localhost访问、ip访问
            if ((dom.ToLower().Trim() == "localhost") || (dom.IsNullOrEmpty() && d1.IsNullOrEmpty() && d0.IsNullOrEmpty()))
            {
                //获取cookie
                user = setUserLogin(0, user);
            }
            //域名访问
            else
            {
                //API的CID
                int cID = Authorize.getCID(d);

                //请求API发生错误
                if (cID == -2)
                {
                    return error("");
                }
                //输入的二级域名是空
                else if (cID == -1)
                {
                    //return noBusiness();
                    Response.Redirect("http://www." + dom + "." + d1);
                    return null;
                }
                //输入的域名不存在商户
                else if (cID == 0)
                {
                    return noBusiness();
                }
                //输入的域名是保留二级域名
                else if (cID == -3)
                {
                    //在crm里面
                    if (d.ToLower() == "erp")
                    {
                        //获取cookie
                        user = setUserLogin(0, user);
                    }
                    //不在crm里面
                    else
                    {
                        return noBusiness();
                    }
                }
                //输入的域名存在商户
                else if (cID > 0)
                {
                    var obj = UserInfoService.Find(o => o.CompanyId == cID);
                    //CID在目前项目不存在
                    if (obj == null)
                    {
                        return noUser(cID.ToString());
                    }
                    else
                    {
                        isReadOnly = 1;
                        user.CID = cID;
                        SysWebSettingBLL bll = new SysWebSettingBLL();
                        string lg = bll.getLogo(cID);
                        if (!lg.IsNullOrEmpty())
                        {
                            logo = lg;
                        }
                        //获取cookie
                        user = setUserLogin(cID, user);
                    }
                }
            }

            //cid是否只读
            ViewBag.isR = isReadOnly;
            //登录logo
            ViewBag.logo = logo;
            //电话
            ViewBag.phone = phone;

            return View("Logins", user);
        }
        //多商户登录
        [HttpPost]
        public ActionResult Logins(UserLogin user)
        {
            //cid是否只读
            ViewBag.isR = user.isReadOnly;
            //登录logo
            ViewBag.logo = user.logo;
            //电话
            string phone = Pharos.Utility.Config.GetAppSettings("phone");
            ViewBag.phone = phone;

            if (!ModelState.IsValid) return View(user);
            var op = Authorize.LoinValidator(CommonService.CompanyId);
            if (!op.Successed)
            {
                ViewBag.msg = op.Message;
                return View(user);
            }

            var pwd = Pharos.Utility.Security.MD5_Encrypt(user.UserPwd);
            var obj = BaseService<SysStoreUserInfo>.Find(o => o.LoginName == user.UserName && o.LoginPwd == pwd && o.Status == 1 && o.CompanyId == user.CID);
            if (obj == null)
            {
                ViewBag.msg = "商户号或帐户或密码输入不正确!";
                return View(user);
            }
            new Sys.CurrentStoreUser().StoreLogin(obj, Sys.SysCommonRules.CurrentStore, user.RememberMe);
            return Redirect(Url.Action("Index", "Home"));
        }
        //发生错误
        public ActionResult error(string msg)
        {
            if (msg.IsNullOrEmpty())
            {
                ViewBag.Message = "发生错误，请稍后再访问";
            }
            else
            {
                ViewBag.Message = Pharos.Utility.DESEncrypt.Decrypt(msg);
            }
            return View("error");
        }
        public ActionResult noBusiness()
        {
            ViewBag.Message = "无法访问，请检查网址是否正确或页面是否存在！";
            return View("noBusiness");
        }
        public ActionResult noUser(string cid)
        {
            ViewBag.cid = cid;
            return View("noUser");
        }


        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="cID"></param>
        /// <param name="u"></param>
        public UserLogin setUserLogin(int cID, UserLogin user)
        {
            if (Cookies.IsExist("remuc"))
            {
                //cookie的CID
                string cid = Cookies.Get("remuc", "_cid");
                if (cid.IsNullOrEmpty())
                {
                    cid = "0";
                }

                //输入的域名存在商户
                if (cID > 0)
                {
                    //API的CID等于cookie的CID
                    if (cid == cID.ToString())
                    {
                        user.CID = Convert.ToInt32(cid);
                        user.UserName = Cookies.Get("remuc", "_uname");
                        user.UserPwd = Cookies.Get("remuc", "_pwd");
                        user.RememberMe = true;
                    }
                }
                //localhost访问、ip访问、保留二级域名访问
                else
                {
                    user.CID = Convert.ToInt32(cid);
                    user.UserName = Cookies.Get("remuc", "_uname");
                    user.UserPwd = Cookies.Get("remuc", "_pwd");
                    user.RememberMe = true;
                }

            }
            return user;
        }

        #endregion


    }

}