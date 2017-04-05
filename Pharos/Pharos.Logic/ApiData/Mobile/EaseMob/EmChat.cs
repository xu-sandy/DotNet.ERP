﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;


//企业管理员账号: cloudchen
//企业管理员密码: 

//appkey: cloudchen-demo#chatdemo
//client_id: YXA6keTK8J3aEeW-UPP3ZCzwGg
//client_secret: YXA6XMlVOMZAio3qvftmuMc-LB4otsk

namespace Pharos.Logic.ApiData.Mobile.EaseMob
{
    public class EmChat
    {
        private string reqUrlFormat { get; set; }
        private string clientID { get; set; }
        private string clientSecret { get; set; }
        private string appName { get; set; }
        private string orgName { get; set; }
        private string token { get; set; }
        private string easeMobUrl { get { return string.Format(reqUrlFormat, orgName, appName); } }
        public string AppName { get { return appName; } }
        public string OrgName { get { return orgName; } }
        /// <summary>
        /// 构造函数，自定义登录参数
        /// </summary>
        /// <param name="reqUrlFormat">环信IM服务器请求资源Url</param>
        /// <param name="easeAppClientID">client_id</param>
        /// <param name="easeAppClientSecret">client_secret</param>
        /// <param name="easeAppName">应用标识之应用名称</param>
        /// <param name="easeAppOrgName">应用标识之登录账号</param>
        public EmChat(string reqUrlFormat, string easeAppClientID, string easeAppClientSecret, string easeAppOrgName, string easeAppName)
        {
            this.reqUrlFormat = reqUrlFormat;
            this.clientID = easeAppClientID;
            this.clientSecret = easeAppClientSecret;
            this.appName = easeAppName;
            this.orgName = easeAppOrgName;
            this.token = QueryToken();
        }
        //测试用
        public EmChat(string reqUrlFormat, string easeAppClientID, string easeAppClientSecret, string easeAppOrgName, string easeAppName, string token)
        {
            this.reqUrlFormat = reqUrlFormat;
            this.clientID = easeAppClientID;
            this.clientSecret = easeAppClientSecret;
            this.appName = easeAppName;
            this.orgName = easeAppOrgName;
            this.token = token;
        }
        /// <summary>
        /// 使用app的client_id 和 client_secret登陆并获取授权token
        /// </summary>
        /// <returns>授权令牌Token。若解析响应的JSON异常，则返回响应的字符串形式</returns>
        string QueryToken()
        {
            if (string.IsNullOrEmpty(clientID) || string.IsNullOrEmpty(clientSecret)) { return string.Empty; }
            string cacheKey = clientID + clientSecret;
            if (System.Web.HttpRuntime.Cache.Get(cacheKey) != null &&
                System.Web.HttpRuntime.Cache.Get(cacheKey).ToString().Length > 0)
            {
                return System.Web.HttpRuntime.Cache.Get(cacheKey).ToString();
            }

            string postUrl = easeMobUrl + "token";
            StringBuilder _build = new StringBuilder();
            _build.Append("{");
            _build.AppendFormat("\"grant_type\": \"client_credentials\",\"client_id\": \"{0}\",\"client_secret\": \"{1}\"", clientID, clientSecret);
            _build.Append("}");

            string postResultStr = ReqUrl(postUrl, "POST", _build.ToString(), string.Empty).ResponseBody;
            string token = string.Empty;
            int expireSeconds = 0;
            try
            {
                JObject jo = JObject.Parse(postResultStr);
                token = jo.GetValue("access_token").ToString();
                int.TryParse(jo.GetValue("expires_in").ToString(), out expireSeconds);
                //设置缓存
                if (!string.IsNullOrEmpty(token) && token.Length > 0 && expireSeconds > 0)
                {
                    System.Web.HttpRuntime.Cache.Insert(cacheKey, token, null, DateTime.Now.AddSeconds(expireSeconds), System.TimeSpan.Zero);
                }
            }
            catch { return postResultStr; }
            return token;
        }
        /// <summary>
        /// 根据提供的参数构造并发送请求
        /// </summary>
        /// <param name="reqUrl">请求Url</param>
        /// <param name="method">请求方式</param>
        /// <param name="paramData">请求体参数</param>
        /// <param name="token">授权令牌Token</param>
        /// <returns>响应的字符串形式。发生异常时，返回异常的字符串形式</returns>
        public ResponseResult ReqUrl(string reqUrl, string method, string paramData, string token)
        {
            ResponseResult response = new ResponseResult();
            try
            {
                HttpWebRequest request = WebRequest.Create(reqUrl) as HttpWebRequest;
                request.Method = method.ToUpperInvariant();

                if (!string.IsNullOrEmpty(token) && token.Length > 1) { request.Headers.Add("Authorization", "Bearer " + token); }
                if (request.Method.ToString() != "GET" && !string.IsNullOrEmpty(paramData) && paramData.Length > 0)
                {
                    request.ContentType = "application/json";
                    byte[] buffer = Encoding.UTF8.GetBytes(paramData);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }

                using (HttpWebResponse resp = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    {
                        return new ResponseResult(resp.StatusCode, stream.ReadToEnd());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 根据提供的参数构造并发送请求
        /// </summary>
        /// <param name="reqUrl">请求Url</param>
        /// <param name="method">请求方式</param>
        /// <param name="paramData">请求体参数(JSON格式字符串)</param>
        /// <param name="token">授权令牌Token</param>
        /// <param name="extraHeaderCollection">额外的HTTP表头键值对集合</param>
        /// <returns>响应的字符串形式。发生异常时，返回异常的字符串形式</returns>
        public string ReqUrl(string reqUrl, string method, string paramData, string token, Dictionary<string, string> extraHeaderCollection)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(reqUrl) as HttpWebRequest;
                request.Method = method.ToUpperInvariant();

                if (!string.IsNullOrEmpty(token) && token.Length > 1) { request.Headers.Add("Authorization", "Bearer " + token); }
                if (extraHeaderCollection != null && extraHeaderCollection.Any())
                {
                    foreach (var item in extraHeaderCollection)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }
                if (request.Method.ToString() != "GET" && !string.IsNullOrEmpty(paramData) && paramData.Length > 0)
                {
                    request.ContentType = "application/json";
                    byte[] buffer = Encoding.UTF8.GetBytes(paramData);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }

                using (HttpWebResponse resp = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string result = stream.ReadToEnd();
                        return result;
                    }
                }
            }
            catch (Exception ex) { return ex.ToString(); }
        }

        //============================
        //接口限流说明: 同一个IP每秒最多可调用30次, 超过的部分会返回503错误, 所以在调用程序中, 如果碰到了这样的错误, 需要稍微暂停一下并且重试。
        //如果该限流控制不满足需求，请联系商务经理开放更高的权限。
        //============================

        #region 环信即时通讯云 REST API 资源

        #region 账户管理
        /// <summary>
        /// 创建用户
        /// （可以批量创建，建议在20-60之间）
        /// </summary>
        /// <param name="postData">创建账号JSON数组--可以一个，也可以多个。username和password是必须的, nickname是可选的，这个nickname用于IOS推送</param>
        /// <returns>创建成功的用户JSON字符串</returns>
        internal ResponseResult AccountCreate(string postData) { try { return ReqUrl(easeMobUrl + "users", "POST", postData, token); } catch (Exception) { throw; } }

        /// <summary>
        /// 获取指定用户详情
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <returns>会员详情JSON字符串</returns>
        internal ResponseResult AccountGet(string userName) { try { return ReqUrl(easeMobUrl + "users/" + userName, "GET", string.Empty, token); } catch (Exception) { throw; } }

        /// <summary>
        /// 批量获取用户详情，指定获取数量
        /// </summary>
        /// <param name="count">获取数量。默认返回最早创建的10个用户。</param>
        /// <returns>指定数量的用户详情</returns>
        internal ResponseResult AccountGet(int count = 10) { return ReqUrl(easeMobUrl + "users?limit=" + count, "GET", string.Empty, token); }

        /// <summary>
        /// 批量获取用户详情，指定获取数量，带分页
        /// </summary>
        /// <param name="count">获取数量</param>
        /// <param name="nextCursor">指向下一页数据的游标。没有时需要赋值为空</param>
        /// <returns>指定数量的用户详情，并返回指向下一页数据的游标</returns>
        //internal string AccountGet(int count, ref string nextCursor)
        //{
        //    string postResultStr = string.Empty;
        //    if (string.IsNullOrEmpty(nextCursor))
        //        postResultStr = AccountGet(count);
        //    else
        //        postResultStr = ReqUrl(easeMobUrl + "users?limit=" + count + "&cursor=" + nextCursor, "GET", string.Empty, token);
        //    try
        //    {
        //        JObject jo = JObject.Parse(postResultStr);
        //        nextCursor = jo.GetValue("cursor").ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        nextCursor = string.Empty;
        //        return ex.ToString();
        //    }
        //    return postResultStr;
        //}

        /// <summary>
        /// 删除用户
        /// （注意: 删除一个用户会删除以该用户为群主的所有群组）
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <returns>成功返回删除的会员JSON详细信息，失败直接返回：系统错误信息</returns>
        internal ResponseResult AccountDel(string userName) { try { return ReqUrl(easeMobUrl + "users/" + userName, "DELETE", string.Empty, token); } catch (Exception) { throw; } }

        /// <summary>
        /// 批量删除用户，指定数量，并不指定某个对象
        /// （建议在100-500之间）
        /// </summary>
        /// <param name="count">删除数量</param>
        /// <returns>成功返回删除的会员JSON详细信息，失败直接返回：系统错误信息</returns>
        internal ResponseResult AccountDel(int count) { return ReqUrl(easeMobUrl + "users" + "?limit=" + count, "DELETE", string.Empty, token); }

        /// <summary>
        /// 批量删除用户详情，指定数量，带分页
        /// </summary>
        /// <param name="count">删除数量</param>
        /// <param name="nextCursor">指向下一页数据的游标。没有时需要赋值为空</param>
        /// <returns>指定数量的用户详情，并返回指向下一页数据的游标</returns>
        //internal string AccountDel(int count, ref string nextCursor)
        //{
        //    string postResultStr = string.Empty;
        //    if (string.IsNullOrEmpty(nextCursor))
        //        postResultStr = AccountDel(count);
        //    else
        //        postResultStr = ReqUrl(easeMobUrl + "users?limit=" + count + "&cursor=" + nextCursor, "DELETE", string.Empty, token);
        //    try
        //    {
        //        JObject jo = JObject.Parse(postResultStr);
        //        nextCursor = jo.GetValue("cursor").ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }
        //    return postResultStr;
        //}

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>重置结果JSON(如：{ "action" : "set user password",  "timestamp" : 1404802674401,  "duration" : 90})</returns>
        internal ResponseResult AccountResetPwd(string userName, string newPassword) { return ReqUrl(easeMobUrl + "users/" + userName + "/password", "PUT", "{\"newpassword\" : \"" + newPassword + "\"}", token); }

        /// <summary>
        /// 修改用户昵称
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <param name="newNickname">新昵称</param>
        /// <returns>修改结果JSON字符串</returns>
        internal ResponseResult AccountModifyNickname(string userName, string newNickname) { return ReqUrl(easeMobUrl + "users/" + userName, "PUT", "{\"nickname\" : \"" + newNickname + "\"}", token); }

        /// <summary>
        /// 禁用一个用户账号
        /// </summary>
        /// <param name="username">用户账号</param>
        /// <returns>操作结果JSON字符串</returns>
        internal ResponseResult AccountDeactivate(string username) { try { return ReqUrl(easeMobUrl + "users/" + username + "/deactivate", "POST", string.Empty, token); } catch (Exception) { throw; } }

        /// <summary>
        /// 解禁一个用户账号
        /// </summary>
        /// <param name="username">用户账号</param>
        /// <returns>操作结果JSON字符串</returns>
        internal ResponseResult AccountActivate(string username) { try { return ReqUrl(easeMobUrl + "users/" + username + "/activate", "POST", string.Empty, token); } catch (Exception) { throw; } }
        #endregion

        #region 用户管理（好友、黑名单、在线状态）
        /// <summary>
        /// 给一个用户添加好友
        /// （两者必须是在同一App下的用户）
        /// </summary>
        /// <param name="ownerUsername">要添加好友的用户名</param>
        /// <param name="friendUsername">被添加的用户名</param>
        /// <returns>添加结果JSON字符串</returns>
        public ResponseResult UserAddFriend(string ownerUsername, string friendUsername) { return ReqUrl(easeMobUrl + "users/" + ownerUsername + "/contacts/users/" + friendUsername, "POST", string.Empty, token); }

        /// <summary>
        /// 从一个用户的好友列表中删除好友
        /// （两者必须是在同一App下的用户）
        /// </summary>
        /// <param name="ownerUsername">要删除好友的用户名</param>
        /// <param name="friendUsername">被删除的用户名</param>
        /// <returns>删除结果JSON字符串</returns>
        public ResponseResult UserDelFriend(string ownerUsername, string friendUsername) { return ReqUrl(easeMobUrl + "users/" + ownerUsername + "/contacts/users/" + friendUsername, "DELETE", string.Empty, token); }

        /// <summary>
        /// 查看某个用户的好友列表信息
        /// </summary>
        /// <param name="ownerUsername">要查看的用户名</param>
        /// <returns>好友列表信息</returns>
        public ResponseResult UserDelFriend(string ownerUsername) { return ReqUrl(easeMobUrl + "users/" + ownerUsername + "/contacts/users", "GET", string.Empty, token); }

        /// <summary>
        /// 获取一个用户的黑名单
        /// </summary>
        /// <param name="ownerUsername">要查看的用户名</param>
        /// <returns>黑名单列表信息</returns>
        public ResponseResult UserBlacklist(string ownerUsername) { return ReqUrl(easeMobUrl + "users/" + ownerUsername + "/blocks/users", "GET", string.Empty, token); }

        /// <summary>
        /// 往一个用户的黑名单中加人
        /// </summary>
        /// <param name="ownerUsername">要操作的用户名</param>
        /// <param name="blockUsernames">黑名单用户名列表</param>
        /// <returns>操作结果JSON字符串</returns>
        //public string UserAddToBlacklist(string ownerUsername, string[] blockUsernames) { return ReqUrl(easeMobUrl + "users/" + ownerUsername + "/blocks/users", "POST", "{\"usernames\" : " + blockUsernames + "}", token); }

        /// <summary>
        /// 从一个用户的黑名单中减人
        /// </summary>
        /// <param name="ownerUsername">要操作的用户名</param>
        /// <param name="blockedUsername">要从黑名单中减去的用户名</param>
        /// <returns>操作结果JSON字符串</returns>
        public ResponseResult UserDelFromBlacklist(string ownerUsername, string blockedUsername) { return ReqUrl(easeMobUrl + "users/" + ownerUsername + "/blocks/users/" + blockedUsername, "DELETE", string.Empty, token); }

        /// <summary>
        /// 查看一个用户的在线状态
        /// </summary>
        /// <param name="username">要查看的用户名</param>
        /// <returns>用户的在线状态JSON字符串</returns>
        public ResponseResult UserStatus(string username) { return ReqUrl(easeMobUrl + "users/" + username + "/status", "GET", string.Empty, token); }

        /// <summary>
        /// 获取一个用户的离线消息数
        /// </summary>
        /// <param name="username">要查看的用户名</param>
        /// <returns>用户的离线消息数</returns>
        public ResponseResult UserOfflineMsgCount(string username) { return ReqUrl(easeMobUrl + "users/" + username + "/offline_msg_count", "GET", string.Empty, token); }

        /// <summary>
        /// 查询某条离线消息状态
        /// </summary>
        /// <param name="username">要查看的用户名</param>
        /// <param name="messageId">离线消息Id</param>
        /// <returns>该条离线消息的状态</returns>
        public ResponseResult UserOfflineMsgStatus(string username, string messageId) { return ReqUrl(easeMobUrl + "users/" + username + "/offline_msg_status/" + messageId, "GET", string.Empty, token); }

        /// <summary>
        /// 强制用户下线
        /// </summary>
        /// <param name="username">用户账号</param>
        /// <returns>操作结果JSON字符串</returns>
        public ResponseResult UserDisconnect(string username) { return ReqUrl(easeMobUrl + "users/" + username + "/disconnect", "GET", string.Empty, token); }


        #endregion

        #region 聊天记录
        /// <summary>
        /// 获取聊天记录
        /// </summary>
        /// <returns>聊天记录(json),默认返回10条记录</returns>
        public ResponseResult ChatMessages() { return ReqUrl(easeMobUrl + "chatmessages", "GET", string.Empty, token); }
        public ResponseResult ChatMessages(string sqlStr) { return ReqUrl(easeMobUrl + "chatmessages?ql=" + sqlStr.Replace(" ", "+"), "GET", string.Empty, token); }
        public ResponseResult ChatMessages(int count) { return ReqUrl(easeMobUrl + "chatmessages?limit=" + count, "GET", string.Empty, token); }
        //public string ChatMessages(int count, ref string nextCursor)
        //{
        //    string postResultStr = string.Empty;
        //    if (string.IsNullOrEmpty(nextCursor))
        //        postResultStr = ChatMessages(count);
        //    else
        //        postResultStr = ReqUrl(easeMobUrl + "chatmessages?limit=" + count + "&cursor=" + nextCursor, "GET", string.Empty, token);
        //    try
        //    {
        //        JObject jo = JObject.Parse(postResultStr);
        //        nextCursor = jo.GetValue("cursor").ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }
        //    return postResultStr;
        //}
        #endregion

        #region 发送消息
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="requestBody">请求携带参数</param>
        /// <returns></returns>
        internal ResponseResult SendMessages(string requestBody) { return ReqUrl(easeMobUrl + "messages", "POST", requestBody, token); }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="targetType">消息接收方类型。（users 给用户发消息, chatgroups 给群发消息, chatrooms 给聊天室发消息）</param>
        /// <param name="targets">消息接收方标识，用户名/群组Id/聊天室Id。（数组长度建议不大于20）</param>
        /// <param name="messageType">消息类型。（txt 文本、img 图片、audio 语音、video 视频、cmd 透传）</param>
        /// <param name="message">消息内容</param>
        /// <param name="from">消息发送方。为空时请求失败。</param>
        /// <returns>发送结果JSON字符串</returns>
        internal ResponseResult SendMessages(string targetType, string[] targets, string messageType, string message, string from = "admin")
        {
            StringBuilder requestBody = new StringBuilder();
            requestBody.Append("{");
            requestBody.AppendFormat("\"target_type\": \"{0}\",", targetType);
            requestBody.AppendFormat("\"target\": \"{0}\",", JsonConvert.SerializeObject(targets));
            requestBody.Append("\"msg\": {");
            requestBody.AppendFormat("\"type\": \"{0}\",", messageType);
            requestBody.AppendFormat("\"msg\": \"{0}\"", message);
            requestBody.Append("},");
            requestBody.AppendFormat("\"from\": \"{0}\"", from);
            requestBody.Append("}");
            return SendMessages(requestBody.ToString());
        }

        internal ResponseResult SendMessages(MsgObject msgObject)
        {
            return SendMessages(JsonConvert.SerializeObject(msgObject));
        }
        #endregion

        #region 上传下载文件
        /// <summary>
        /// 上传消息附件
        /// </summary>
        /// <param name="requestBody">请求携带参数。需要使用 multipart/form-data 形式</param>
        /// <param name="restrictAccess">是否限制访问。限制的话需要有附件相应的 secret 才能访问</param>
        /// <returns></returns>
        public string UploadChatFiles(string requestBody, bool restrictAccess = false) { return ReqUrl(easeMobUrl + "chatfiles", "POST", requestBody, token, restrictAccess ? new Dictionary<string, string> { { "restrict-access", "true" }} : null); }

        public string DownloadChatFiles(string uuid) { return ReqUrl(easeMobUrl + "chatfiles/" + uuid, "GET", string.Empty, token, new Dictionary<string, string> { { "Accept", "application/octet-stream" } }); }
        public string DownloadChatFiles(string uuid, string secret) { return ReqUrl(easeMobUrl + "chatfiles/" + uuid, "GET", string.Empty, token, new Dictionary<string, string> { { "share-secret", secret }, { "Accept", "application/octet-stream" } }); }
        public string DownloadChatFilesThumbnail(string uuid) { return ReqUrl(easeMobUrl + "chatfiles/" + uuid, "GET", string.Empty, token, new Dictionary<string, string> { { "Accept", "application/octet-stream" }, { "thumbnail", "true" } }); }
        #endregion


        #endregion
    }

    #region 封装类

    public class ResponseResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseBody { get; set; }

        public ResponseResult()
            : this(HttpStatusCode.OK)
        {
        }
        public ResponseResult(HttpStatusCode statusCode)
            : this(statusCode, string.Empty)
        {
        }
        public ResponseResult(HttpStatusCode statusCode, string responseBody)
        {
            this.StatusCode = statusCode;
            this.ResponseBody = responseBody;
        }
    }

    /// <summary>
    /// 消息对象封装
    /// </summary>
    public class MsgObject
    {
        /// <summary>
        /// 接收方类型
        /// </summary>
        [JsonIgnore]
        public MsgTargetType TargetType { get; set; }
        /// <summary>
        /// 接收方类型字符串
        /// </summary>
        [JsonProperty("target_type")]
        private string _targetTypeStr { get { return TargetType.ToString(); } }
        /// <summary>
        /// 接收方数组
        /// </summary>
        [JsonProperty("target")]
        public string[] Target { get; set; }
        /// <summary>
        /// 发送方
        /// </summary>
        [JsonProperty("from")]
        public string From { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        [JsonProperty("msg")]
        public MsgBody MsgBody { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        [JsonProperty("ext")]
        public object Ext { get; set; }

        public MsgObject(MsgTargetType targetType, string[] target, string from, MsgBody msgBody, object ext = null)
        {
            this.TargetType = targetType;
            this.Target = target;
            this.From = from;
            this.MsgBody = msgBody;
            this.Ext = ext ?? new { };
        }
        public MsgObject(MsgTargetType targetType, string[] target, MsgBody msgBody, object ext = null)
            : this(targetType, target, "admin", msgBody, ext)
        {
        }
    }
    /// <summary>
    /// 消息内容基类
    /// </summary>
    public abstract class MsgBody
    {
        [JsonIgnore]
        public virtual MsgType MsgType { get; set; }
        [JsonProperty("type")]
        protected string MagTypeStr { get { return MsgType.ToString(); } }
    }
    public class TxtMsgBody : MsgBody
    {
        public override MsgType MsgType
        {
            get { return MsgType.txt; }
        }
        [JsonProperty("msg")]
        public string Msg { get; set; }
    }
    public class ImgMsgBody : MsgBody
    {
        public override MsgType MsgType
        {
            get { return MsgType.img; }
        }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("filename")]
        public string FileName { get; set; }
        [JsonProperty("secret")]
        public string Secret { get; set; }
        /// <summary>
        /// 尺寸：{ "width":400, "heignt": 720}
        /// </summary>
        [JsonProperty("size")]
        public object Size { get; set; }
    }
    public class AudioMsgBody : MsgBody
    {
        public override MsgType MsgType
        {
            get { return MsgType.audio; }
        }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("filename")]
        public string FileName { get; set; }
        [JsonProperty("secret")]
        public string Secret { get; set; }
        [JsonProperty("length")]
        public int Length { get; set; }
    }
    public class VideoMsgBody : MsgBody
    {
        public override MsgType MsgType
        {
            get { return MsgType.video; }
        }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("filename")]
        public string FileName { get; set; }
        [JsonProperty("secret")]
        public string Secret { get; set; }
        [JsonProperty("length")]
        public int Length { get; set; }
        [JsonProperty("file_length")]
        public int FileSize { get; set; }
        [JsonProperty("thumb")]
        public string Thumb { get; set; }
        [JsonProperty("thumb_secret")]
        public string ThumbSecret { get; set; }
    }
    public class CuntomCmdMsgBody : MsgBody
    {
        public override MsgType MsgType
        {
            get { return MsgType.cmd; }
        }
    }
    /// <summary>
    /// 消息接收方类型
    /// </summary>
    public enum MsgTargetType
    {
        /// <summary>
        /// 用户
        /// </summary>
        users,
        /// <summary>
        /// 群组
        /// </summary>
        chatgroups,
        /// <summary>
        /// 聊天室
        /// </summary>
        chatrooms
    }
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 文本
        /// </summary>
        txt,
        /// <summary>
        /// 图片
        /// </summary>
        img,
        /// <summary>
        /// 语音
        /// </summary>
        audio,
        /// <summary>
        /// 视频
        /// </summary>
        video,
        /// <summary>
        /// 透传
        /// </summary>
        cmd
    }
    #endregion
}
