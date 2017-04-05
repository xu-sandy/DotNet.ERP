﻿using Pharos.Logic.ApiData.Mobile.EaseMob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using Pharos.Sys.BLL;

namespace Pharos.Logic.ApiData.Mobile.Services
{
    public class ChatService
    {
        private static EmChat emChat;

        static ChatService()
        {
            var setting = new SysWebSettingBLL().GetWebSetting();
            if(setting!=null && !setting.EMReqUrlBase.IsNullOrEmpty())
                emChat = new EmChat(setting.EMReqUrlBase, setting.EMAppId, setting.EMAppSecret, setting.EMAppOrg, setting.EMAppName);
        }
        public static EmChat EmChat { get { return emChat; } }
        /// <summary>
        /// 创建一个环信用户
        /// </summary>
        /// <param name="imUserName">环信用户账号</param>
        /// <param name="emPassword">环信密码</param>
        /// <param name="emNickName">环信昵称，（可选）用于IOS推送</param>
        /// <returns>OpResult</returns>
        public static OpResult AccountCreate(string imUserName, string emPassword, string emNickName)
        {
            StringBuilder _build = new StringBuilder();
            _build.Append("{");
            _build.AppendFormat("\"username\": \"{0}\",\"password\": \"{1}\"", imUserName, emPassword);
            if (!string.IsNullOrEmpty(emNickName))
                _build.AppendFormat(",\"nickname\": \"{0}\"", emNickName);
            _build.Append("}");
            try
            {
                var response = emChat.AccountCreate(_build.ToString());
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return OpResult.Fail(message: "注册环信IM用户失败！HTTP错误状态码：" + response.StatusCode, data: response.ResponseBody);
                }
                return OpResult.Success(data: response.ResponseBody);
            }
            catch (Exception ex)
            {
                new Sys.LogEngine().WriteError(ex);
                return OpResult.Fail("注册环信IM用户失败！");
            }
        }


        /// <summary>
        /// 删除一个指定环信用户
        /// </summary>
        /// <param name="imUserName">环信用户账号</param>
        /// <returns>OpResult</returns>
        public static OpResult AccountDel(string imUserName)
        {
            try
            {
                var response = emChat.AccountDel(imUserName);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return OpResult.Fail(message: "删除环信IM用户失败！HTTP错误状态码：" + response.StatusCode, data: response.ResponseBody);
                }
                return OpResult.Success(data: response.ResponseBody);
            }
            catch (Exception ex)
            {
                new Sys.LogEngine().WriteError(ex);
            }
            return OpResult.Fail("删除环信IM用户失败！");
        }
        /// <summary>
        /// 禁用环信用户账号
        /// </summary>
        /// <param name="imUserName">环信用户账号</param>
        /// <returns></returns>
        public static ResponseResult AccountDeactivate(string imUserName) { return emChat.AccountDeactivate(imUserName); }
        /// <summary>
        /// 启用环信用户账户
        /// </summary>
        /// <param name="imUserName">环信用户账号</param>
        /// <returns></returns>
        public static ResponseResult AccountActivate(string imUserName) { return emChat.AccountActivate(imUserName); }

        /// <summary>
        /// 获取用户对应的IM用户通行证，若没有则向IM服务器注册
        /// </summary>
        /// <param name="uid">用户UID</param>
        /// <returns></returns>
        public static string GetIMPassport(string uid)
        {
            var userAccount = Logic.BLL.UserInfoService.Find(a => a.UID.Equals(uid, StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrWhiteSpace(userAccount.HuanXinUUID))
            {//不存在环信IM账号时，创建
                var emCreateResult = ChatService.AccountCreate(uid, uid, userAccount.FullName);
                if (emCreateResult.Data != null)
                {
                    JObject jo = JObject.Parse(emCreateResult.Data as string);
                    if (emCreateResult.Successed)
                    {//创建IM账号成功，存入数据库
                        JArray ja = JArray.Parse(jo.GetValue("entities").ToString());
                        var uuid = ja[0]["uuid"].ToString();
                        userAccount.HuanXinUUID = uuid;
                        Logic.BLL.UserInfoService.Update(userAccount);

                        //发送“有新用户”透传消息
                        //var imUser = ChatService.PaserToIMUser(uid);
                        //ChatService.SendIMUserUpdateCmdMessage(imUser);
                    }
                    else
                    {//创建IM账号失败，根据返回判断是否已在IM服务器上注册
                        if (jo.GetValue("error").ToString() == "duplicate_unique_property_exists")
                        {
                            var existedAccountResp = emChat.AccountGet(uid);
                            JObject existedJo = JObject.Parse(existedAccountResp.ResponseBody);
                            JArray ja = JArray.Parse(jo.GetValue("entities").ToString());
                            var uuid = ja[0]["uuid"].ToString();
                            userAccount.HuanXinUUID = uuid;
                            Logic.BLL.UserInfoService.Update(userAccount);
                        }
                    }
                }
            }
            return userAccount.UID;
        }

        /// <summary>
        /// 当修改用户的IM信息时，给其他联系人发送透传消息通知更新当前用户的信息
        /// </summary>
        /// <returns></returns>
        public static void SendIMUserUpdateCmdMessage(IMUser imUser)
        {
            UpdateIMUserCmdMagBody cmdMagBody = new UpdateIMUserCmdMagBody(imUser);
            var otherUsers = Logic.BLL.UserInfoService.FindList(a => !a.UID.Equals(imUser.IMUserName, StringComparison.OrdinalIgnoreCase) && a.Status == 1 && a.HuanXinUUID.Length > 0)
                .Select(a => a.UID).ToArray();
            if (otherUsers.Any())
            {
                MsgObject mgsObj = new MsgObject(MsgTargetType.users, otherUsers, cmdMagBody, null);
                emChat.SendMessages(mgsObj);
            }
        }

        public static IMUser PaserToIMUser(string uid)
        {
            var user = Pharos.Logic.BLL.UserInfoService.GetUserInfoForIM(uid);
            var userType = user.GetType();
            return new ChatService.IMUser()
            {
                IMUserName = userType.GetProperty("IMUserName").GetValue(user, null) as string,
                Department = userType.GetProperty("Department").GetValue(user, null) as string,
                FullName = userType.GetProperty("FullName").GetValue(user, null) as string,
                Mobile = userType.GetProperty("Mobile").GetValue(user, null) as string,
                PhotoUrl = userType.GetProperty("PhotoUrl").GetValue(user, null) as string,
                Sex = userType.GetProperty("Sex").GetValue(user, null) as string,
                Signature = userType.GetProperty("Signature").GetValue(user, null) as string,
                UserCode = userType.GetProperty("UserCode").GetValue(user, null) as string
            };
        }

        public class IMUser
        {
            public string IMUserName { get; set; }
            public string IMPassword { get; set; }
            public string PhotoUrl { get; set; }
            public string FullName { get; set; }
            public string UserCode { get; set; }
            public string Sex { get; set; }
            public string Department { get; set; }
            public string Signature { get; set; }
            public string Mobile { get; set; }
        }
        public class UpdateIMUserCmdMagBody : CuntomCmdMsgBody
        {
            public IMUser IMUser { get; set; }
            public UpdateIMUserCmdMagBody(IMUser IMUser)
            {
                this.IMUser = IMUser;
            }
        }
    }
}
