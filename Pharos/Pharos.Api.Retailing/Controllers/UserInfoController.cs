﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Pharos.Logic.ApiData.Mobile.Exceptions;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Utility.Helpers;
using Pharos.Logic.ApiData.Mobile.Services;
namespace Pharos.Api.Retailing.Controllers
{
    public class UserInfoController : ApiController
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        public object POST([FromBody]JObject requestParams)
        {
            var usercode = requestParams.Property("account", true);
            if (string.IsNullOrWhiteSpace(usercode))
                throw new PosException("登陆帐号为空!");
            var user = Pharos.Logic.BLL.UserInfoService.GetUserAndSup(Sys.SysCommonRules.CompanyId,usercode);
            if (user == null)
                throw new PosException("登陆帐号不存在!");
            return user;
        }
    }
    public class UpdatePasswordController : ApiController
    {
        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        public void POST([FromBody]JObject requestParams)
        {
            var account = requestParams.Property("account", true);
            var oldpassword = requestParams.Property("oldpassword", true);
            var newpassword = requestParams.Property("newpassword", true);
            Pharos.Logic.BLL.UserInfoService.UpdatePassword(account, oldpassword, newpassword);
        }
    }
    public class HandSignController : ApiController
    {
        /// <summary>
        /// 手势密码设置 
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        public void POST([FromBody]JObject requestParams)
        {
            var account = requestParams.Property("account", true);
            var oldpassword = requestParams.Property("oldpassword", true);
            var newpassword = requestParams.Property("newpassword", true);
            Pharos.Logic.BLL.UserInfoService.UpdateHandsign(account, oldpassword, newpassword);
        }
        /// <summary>
        /// 手势密码是否开启
        /// </summary>
        ///  <param name="account"></param>
        ///  <param name="cid"></param>
        /// <returns></returns>
        public int GET(string account,int cid)
        {
            bool isopen = false;
            var hand = Pharos.Logic.BLL.UserInfoService.GetHandsign(account, "", ref isopen);
            return Convert.ToInt32(isopen);
        }
    }
    public class HandSignOffController : ApiController
    {
        /// <summary>
        /// 手势密码设置 
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        public void POST([FromBody]JObject requestParams)
        {
            var account = requestParams.Property("account", true);
            var password = requestParams.Property("password", true);
            Pharos.Logic.BLL.UserInfoService.UpdateHandsignOff(account, password);
        }
    }
    public class GetHandSignController : ApiController
    {
        /// <summary>
        /// 获取手势密码
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        public object POST([FromBody]JObject requestParams)
        {
            var account = requestParams.Property("account", true);
            var password = requestParams.Property("password", true);
            bool isopen=false;
            var hand = Pharos.Logic.BLL.UserInfoService.GetHandsign(account, password, ref isopen);
            if (hand.IsNullOrEmpty()) throw new MessageException("帐号不存在或未设置手势密码!");
            return hand;
        }
    }

    public class SetMobileController : ApiController
    {
        /// <summary>
        /// 设置手机号码
        /// </summary>
        /// <param name="requestParams">请求参数{ "account":"xxx", "mobile":"xxx" }</param>
        /// <returns></returns>
        public void POST([FromBody]JObject requestParams)
        {
            var account = requestParams.Property("account", true);
            var mobile = requestParams.Property("mobile", true);
            Pharos.Logic.BLL.UserInfoService.SetMobile(account, mobile);

            //发送更新透传消息
            //var resultData = result.Data as Sys.Entity.SysUserInfo;
            //var imUser = ChatService.PaserToIMUser(resultData.UID);
            //ChatService.SendIMUserUpdateCmdMessage(imUser);
        }
    }
    public class SetSignatureController : ApiController
    {
        /// <summary>
        /// 设置个性签名
        /// </summary>
        /// <param name="requestParams">请求参数{ "account":"xxx", "signature":"xxx" }</param>
        /// <returns></returns>
        public void POST([FromBody]JObject requestParams)
        {
            var account = requestParams.Property("account", true);
            var signature = requestParams.Property("signature", true);
            Pharos.Logic.BLL.UserInfoService.SetSignature(account, signature);

            //发送更新透传消息
            //var resultData = result.Data as Sys.Entity.SysUserInfo;
            //var imUser = ChatService.PaserToIMUser(resultData.UID);
            //ChatService.SendIMUserUpdateCmdMessage(imUser);
        }
    }
}