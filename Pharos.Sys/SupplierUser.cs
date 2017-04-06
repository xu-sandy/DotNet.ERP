// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-08-08
// 描述信息：当前登录用户信息（适用于供应商）
// --------------------------------------------------

using System;
using System.Web;
using System.Collections.Generic;
using Pharos.Utility;

namespace Pharos.Sys
{
    /// <summary>
    /// 当前登录用户信息（适用于供应商）
    /// </summary>
    public class SupplierUser
    {
        /// <summary>
        /// 是否已登录
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                return !string.IsNullOrEmpty(SupplierId);
            }
        }

        /// <summary>
        /// 统一登录写入cookie
        /// </summary>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="supplierName">供应商简称</param>
        /// <param name="account">供应商登录帐号</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="remember">记住用户和密码，默认false</param>
        public void Login(string supplierId, string supplierName, string account, string pwd, bool remember = false)
        {
            Dictionary<string, string> kv = new Dictionary<string, string>();

            kv.Add(key_uid, supplierId);
            kv.Add(key_uname, account);
            kv.Add(key_fname, HttpUtility.UrlEncode(supplierName));
            kv.Add(key_rememberMo, remember ? "1" : "0");

            Cookies.Remove("remuc");
            if (remember)
            {
                Cookies.Set("remuc", kv, 30, true);
            }
            //StoreId { get { return "d4ab4325c87c4863b1419b6862d8454c"; } }

            Cookies.Set(uc, kv);
        }
        private const string uc = "sc";

        /// <summary>
        /// 安全退出
        /// </summary>
        public static void Exit()
        {
            if (IsLogin)
            {
                HttpCookie hc = new HttpCookie(uc);
                hc.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(hc);
            }
        }
        
        /// <summary>
        /// 供应商ID
        /// </summary>
        public static string SupplierId
        {
            get
            {
                return Cookies.Get(uc, key_uid);
            }
        }
        private const string key_uid = "_u";

        /// <summary>
        /// 登录帐号
        /// </summary>
        public static string UserName
        {
            get
            {
                return Cookies.Get(uc, key_uname);
            }
        }
        private const string key_uname = "_uname";

        /// <summary>
        /// 商家简称
        /// </summary>
        public static string SupplierName
        {
            get
            {
                return HttpUtility.UrlDecode(Cookies.Get(uc, key_fname));
            }
        }
        private const string key_fname = "_fname";

        /// <summary>
        /// 登录密码
        /// </summary>
        public static string UserPwd
        {
            get
            {
                return Cookies.Get(uc, key_upwd);
            }
        }
        private const string key_upwd = "_pwd";

        /// <summary>
        /// 记住我
        /// </summary>
        public static bool RememberMe
        {
            get
            {
                var val = Cookies.Get(uc, key_rememberMo);
                if (string.IsNullOrWhiteSpace(val)) return false;
                return Convert.ToInt32(val) > 0;
            }
        }
        private const string key_rememberMo = "_remember";
    }
}
