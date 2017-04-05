﻿// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：蔡少发
// 创建时间：2016-11-25
// 描述信息：当前登录用户信息（门店专用）
// --------------------------------------------------

using System;
using System.Web;
using System.Collections.Generic;
using Pharos.Utility;
using Pharos.Sys.Entity;
using Pharos.Utility.Caching;
using Pharos.Sys.BLL;

namespace Pharos.Sys
{
    /// <summary>
    /// 当前登录用户信息（门店专用）
    /// </summary>
    public class CurrentStoreUser
    {
        private LogEngine log = new LogEngine();

        /// <summary>
        /// 是否已登录
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                return !string.IsNullOrEmpty(UID);
            }
        }

        /// <summary>
        /// 是否登录到门店系统
        /// </summary>
        public static bool IsStore
        {
            get
            {
                return !string.IsNullOrEmpty(StoreId) ? Convert.ToInt32(StoreId) > 0 : false;
            }
        }
                
        /// <summary>
        /// 统一登录写入cookie
        /// </summary>
        /// <param name="user">Entity.SysUserInfo 用户信息类</param>
        /// <param name="remember">记住用户和密码，默认false</param>
        public void StoreLogin(SysStoreUserInfo user,string store, bool remember = false)
        {
            Dictionary<string, string> kv = new Dictionary<string, string>();

            kv.Add(key_cid, user.CompanyId.ToString());
            kv.Add(key_uid, user.UID);
            kv.Add(key_uname, user.LoginName);
            kv.Add(key_fname, HttpUtility.UrlEncode(user.FullName));

            //kv.Add(key_branchId, user.BranchId.ToString());
            //kv.Add(key_bumenId, user.BumenId.ToString());
            //kv.Add(key_photo, user.PhotoUrl);

            kv.Add(key_storeId, HttpUtility.UrlEncode(store));
            //kv.Add(key_roleId, user.RoleIds);

            Cookies.Remove("storeremuc");
            if (remember)
            {
                //kv.Add("_pwd", user.LoginPwd);
                Cookies.Set("storeremuc", kv, 100, true);//防止退出删除
            }
            //StoreId { get { return "d4ab4325c87c4863b1419b6862d8454c"; } }

            SettingLimits(user.UID);

            //Cookies.Set(uc, kv);
            Cookies.Set(Url.CurDomain, "", uc, kv, 1, false);

            log.WriteLogin(string.Format("用户（{0}，{1}）成功登录门店系统！", user.LoginName, user.FullName), LogModule.其他);
        }


        private const string uc = "storeuc";
        private const string key_pwd = "_storepwd";

        /// <summary>
        /// 安全退出
        /// </summary>
        public static void Exit()
        {
            if (IsLogin)
            {
                new LogEngine().WriteLogout(string.Format("用户（{0}，{1}）成功退出门店系统！", UserName, FullName),LogModule.其他);

                HttpCookie hc = new HttpCookie(uc);
                hc.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(hc);
            }
        }

        public static string PWD
        {
            get
            {
                return Cookies.Get(uc, key_pwd);
            }
        }

        /// <summary>
        /// todo: 该 usercode 过后需要移除掉
        /// </summary>
        public static int UserCode
        {
            get
            {
                return 1001;
            }
        }

        /// <summary>
        /// 用户UID
        /// </summary>
        public static string UID
        {
            get
            {
                return Cookies.Get(uc, key_uid);
            }
        }
        private const string key_uid = "_u";

        /// <summary>
        ///  CID
        /// </summary>
        public static string CID
        {
            get
            {
                return Cookies.Get(uc, key_cid);
            }
        }
        private const string key_cid = "_s_cid";

        /// <summary>
        /// 登录名称
        /// </summary>
        public static string UserName
        {
            get
            {
                return HttpUtility.UrlDecode(Cookies.Get(uc, key_uname));
            }
        }
        private const string key_uname = "_s_uname";

        /// <summary>
        /// 用户姓名
        /// </summary>
        public static string FullName
        {
            get
            {
                return HttpUtility.UrlDecode(Cookies.Get(uc, key_fname));
            }
        }
        private const string key_fname = "_s_fname";

        /// <summary>
        /// 隶属机构ID
        /// </summary>
        public static int BranchId
        {
            get
            {
                string id = Cookies.Get(uc, key_branchId);
                return !string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : -1;
            }
        }
        private const string key_branchId = "_s_branchId";

        /// <summary>
        /// 隶属部门ID
        /// </summary>
        public static int BumenId
        {
            get
            {
                string id = Cookies.Get(uc, key_bumenId);
                return !string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : -1;
            }
        }
        private const string key_bumenId = "_s_bumenId";

        /// <summary>
        /// 头像URL
        /// </summary>
        public static string Photo
        {
            get
            {
                return Cookies.Get(uc, key_photo);
            }
        }
        private const string key_photo = "_s_photo";

        /// <summary>
        /// 所在门店ID
        /// </summary>
        public static string StoreId
        {
            get
            {
                var val= Cookies.Get(uc, key_storeId);
                if (string.IsNullOrWhiteSpace(val)) return "";
                val = HttpUtility.UrlDecode(val);
                return val.Split('~')[0];
            }
        }
        private const string key_storeId = "_storeId";
        /// <summary>
        /// 所在门店ID
        /// </summary>
        public static string StoreName
        {
            get
            {
                var val = Cookies.Get(uc, key_storeId);
                if (string.IsNullOrWhiteSpace(val)) return "";
                val = HttpUtility.UrlDecode(val);
                return val.Split('~')[1];
            }
        }
        /// <summary>
        /// 角色ID
        /// </summary>
        public static string RoleIds
        {
            get
            {
                return Cookies.Get(uc, key_roleId);
            }
        }
        private const string key_roleId = "_s_roleId";

        /// <summary>
        /// 用户权限缓存数据
        /// </summary>
        public static Dictionary<int, int> LimitIds
        {
            get
            {
                var cacheManager = new MemoryCacheManager();
                var objs = cacheManager.Get<Dictionary<int, int>>(UID + "_" + key_limitId);
                return objs;
            }
        }
        private const string key_limitId = "_s_limitId";
        /// <summary>
        /// 判断用户是否有对应的访问权限
        /// </summary>
        /// <param name="limitId"></param>
        /// <returns></returns>
        public static bool HasPermiss(int limitId)
        {
            var result = false;
            //取缓存的权限
            //var objs = LimitIds;
            var roleBLL = new SysRoleBLL();//测试
            Dictionary<int, int> objs = null;
            if (string.IsNullOrWhiteSpace(CurrentStoreUser.RoleIds))
                objs = roleBLL.GetRoleLimitsByUId(CurrentStoreUser.UID);//测试
            else
                objs = roleBLL.GetRoleLimitsByRoleId(CurrentStoreUser.RoleIds);//门店根据角色

            if (objs == null || objs.Count<=0) {
                //重新初始化当前登录用户权限对象并设置全局缓存
                //SettingLimits(CurrentUser.UID);
                objs = roleBLL.GetRoleLimitsByUId(CurrentStoreUser.UID);
            }
            if (objs != null)
            {
                var curLimits = (Dictionary<int, int>)objs;
                //判断当前用户的权限是否包含数据权限
                if (curLimits.ContainsKey(limitId))
                {
                    result = true;
                    return result;
                }
            }
            return result;
        }
        /// <summary>
        /// 根据用户UID初始化用户权限对象并设置全局缓存
        /// </summary>
        /// <param name="uid"></param>
        private static void SettingLimits(string uid)
        {
            var ukey = uid + "_" + key_limitId;
            //取权限
            var roleBLL = new SysRoleBLL();
            var objs = roleBLL.GetRoleLimitsByUId(uid);

            //设置缓存
            var cacheManager = new MemoryCacheManager();
            if (cacheManager.IsSet(ukey))
            {
                cacheManager.Remove(ukey);
            }
            cacheManager.Set(ukey, objs, 1440);
        }
        public static dynamic Company
        {
            get { return DataCache.Get("CompanyAuthorize_" + SysCommonRules.CompanyId); }
            set { DataCache.Set("CompanyAuthorize_" + SysCommonRules.CompanyId, value,30); }
        }
    }
}
