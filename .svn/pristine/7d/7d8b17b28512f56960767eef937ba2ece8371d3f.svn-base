// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-07-29
// 描述信息：Cookies 引擎
// --------------------------------------------------

namespace Pharos.Utility
{
    using System;
    using System.Web;
    using System.Collections.Generic;

    /// <summary>
    /// Cookies 引擎
    /// </summary>
    public class Cookies
    {
        #region 检测 Cookies 是否存在

        /// <summary>
        /// 检测 Cookies 是否存在
        /// </summary>
        /// <param name="CookiesName">Cookies Name</param>
        /// <returns>True:是 / False:否</returns>
        public static bool IsExist(string CookiesName)
        {
            return ((HttpContext.Current.Request.Cookies[CookiesName] != null)
                    && (HttpContext.Current.Request.Cookies[CookiesName].Value != null)
                    && (HttpContext.Current.Request.Cookies[CookiesName].Value.ToString().Trim().Length > 0));
        }

        #endregion

        #region 写入或更新 Cookies

        /// <summary>
        /// 逐个 写入或更新 Cookies
        /// </summary>
        /// <param name="name">键名</param>
        /// <param name="value">键值</param>
        public static void Set(string name, string value)
        {
            HttpContext.Current.Response.Cookies[name].Value = value;
        }

        /// <summary>
        /// 逐个 写入或更新 Cookies
        /// </summary>
        /// <param name="name">键名</param>
        /// <param name="value">键值</param>
        /// <param name="days">有效天数（-32767至32767，须启用 expires 方有效）</param>
        /// <param name="expires">启用有效天数（True:启用, false:禁用）</param>
        public static void Set(string name, string value, short days, bool expires)
        {
            HttpContext.Current.Response.Cookies[name].Value = value;
            if (expires == true)
            {
                HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(days);
            }
        }

        /// <summary>
        /// 逐个 写入或更新 Cookies
        /// </summary>
        /// <param name="path">存储路径，如 @"/"</param>
        /// <param name="name">键名</param>
        /// <param name="value">键值</param>
        /// <param name="days">有效天数（-32767至32767，须启用 expires 方有效）</param>
        /// <param name="expires">启用有效天数（True:启用, false:禁用）</param>
        public static void Set(string path, string name, string value, short days, bool expires)
        {
            HttpContext.Current.Response.Cookies[name].Path = path;
            HttpContext.Current.Response.Cookies[name].Value = value;

            if (expires == true)
            {
                HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(days);
            }
        }

        /// <summary>
        /// 逐个 写入或更新 Cookies
        /// </summary>
        /// <param name="domain">域名，如 "caisf.net"</param>
        /// <param name="path">存储路径，如 @"/"</param>
        /// <param name="name">键名</param>
        /// <param name="value">键值</param>
        /// <param name="days">有效天数（-32767至32767，须启用 expires 方有效）</param>
        /// <param name="expires">启用有效天数（True:启用, false:禁用）</param>
        public static void Set(string domain, string path, string name, string value, short days, bool expires)
        {
            HttpContext.Current.Response.Cookies[name].Path = path;

            if ((Url.CurDomain.ToLower() != "localhost") && (string.IsNullOrEmpty(domain) != true))
            {
                HttpContext.Current.Response.Cookies[name].Domain = domain;
            }

            HttpContext.Current.Response.Cookies[name].Value = value;
            if (expires == true)
            {
                HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(days);
            }
        }


        /// <summary>
        /// 写入或更新 Cookies 集合
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="keyValue">键、值</param>
        public static void Set(string name, Dictionary<string, string> keyValue)
        {
            Set(null, null, name, keyValue, 1, false);
        }

        /// <summary>
        /// 写入或更新 Cookies 集合
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="keyValue">键、值</param>
        /// <param name="days">有效天数（-32767至32767，须启用 expires 方有效）</param>
        /// <param name="expires">启用有效天数（True:启用, false:禁用）</param>
        public static void Set(string name, Dictionary<string, string> keyValue, short days, bool expires)
        {
            Set(null, null, name, keyValue, days, expires);
        }

        /// <summary>
        /// 写入或更新 Cookies 集合
        /// </summary>
        /// <param name="domain">域名，如 "caisf.net"</param>
        /// <param name="path">存储路径，如 @"/"</param>
        /// <param name="name">名称</param>
        /// <param name="keyValue">键、值</param>
        /// <param name="days">有效天数（-32767至32767，须启用 expires 方有效）</param>
        /// <param name="expires">启用有效天数（True:启用, false:禁用）</param>
        public static void Set(string domain, string path, string name, Dictionary<string, string> keyValue, short days, bool expires)
        {
            HttpCookie hc = new HttpCookie(name);

            if (string.IsNullOrEmpty(path) == true) { hc.Path = @"/"; } else { hc.Path = path; }
            if ((string.IsNullOrEmpty(domain) != true) && (Url.CurDomain.ToLower() != "localhost")) { hc.Domain = domain; }

            foreach (KeyValuePair<string, string> kv in keyValue)
            {
                hc.Values[kv.Key] = kv.Value;
            }

            if (expires == true) { hc.Expires = DateTime.Now.AddDays(days); }
            HttpContext.Current.Response.Cookies.Add(hc);
        }


        /// <summary>
        /// 往 Cookies集合中 插入某个键值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void InsertCookiesKey(string name, string key, string value)
        {
            if (IsExist(name) == true)
            {
                HttpCookie hc = HttpContext.Current.Request.Cookies[name];
                hc.Values[key].Insert(0, value);

                HttpContext.Current.Response.Cookies.Add(hc);
            }
        }

        /// <summary>
        /// 更新 Cookies集合中 的某个键的值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void UpdateCookiesKey(string name, string key, string value)
        {
            if (IsExist(name) == true)
            {
                HttpCookie hc = HttpContext.Current.Request.Cookies[name];
                hc.Values[key] = value;

                HttpContext.Current.Response.Cookies.Add(hc);
            }
        }

        #endregion

        #region 读取 Cookies

        /// <summary>
        /// 读取 单个Cookies
        /// </summary>
        /// <param name="name">名称</param>
        public static string Get(string name)
        {
            if (IsExist(name))
            {
                return HttpContext.Current.Request.Cookies[name].Value.Trim();
            }

            return string.Empty;
        }

        /// <summary>
        /// 读取 Cookies集合
        /// </summary>
        /// <param name="name">集合名</param>
        /// <param name="key">键</param>
        public static string Get(string name, string key)
        {
            if (IsExist(name) == true)
            {
                HttpCookie hc = HttpContext.Current.Request.Cookies[name];

                try
                {
                    return hc.Values[key].Trim();
                }
                catch
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        #endregion

        #region 设置 Cookies为过期

        /// <summary>
        /// 逐个 设置Cookies过期
        /// </summary>
        /// <param name="name">名称</param>
        public static void SetExpires(string name)
        {
            HttpContext.Current.Response.Cookies[name].Value = string.Empty;
            HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(-1);

            HttpCookie hc = new HttpCookie(name);
            hc.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(hc);
        }

        /// <summary>
        /// 逐个 设置Cookies过期
        /// </summary>
        /// <param name="path">所在路径，如 @"/"</param>
        /// <param name="name">名称</param>
        public static void SetExpires(string path, string name)
        {
            HttpContext.Current.Response.Cookies[name].Path = path;
            HttpContext.Current.Response.Cookies[name].Value = string.Empty;
            HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(-1);

            HttpCookie hc = new HttpCookie(name);
            hc.Path = path;
            hc.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(hc);
        }

        /// <summary>
        /// 逐个 设置Cookies过期
        /// </summary>
        /// <param name="domain">域名，如 "caisf.net"</param>
        /// <param name="path">所在路径，如 @"/"</param>
        /// <param name="name">名称</param>
        public static void SetExpires(string domain, string path, string name)
        {
            HttpContext.Current.Response.Cookies[name].Path = path;
            HttpCookie hc = new HttpCookie(name);

            if (Url.CurDomain.ToLower() != "localhost")
            {
                HttpContext.Current.Response.Cookies[name].Domain = domain;
                hc.Domain = domain;
            }

            HttpContext.Current.Response.Cookies[name].Value = string.Empty;
            HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(-1);

            hc.Path = path;
            hc.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(hc);
        }

        /// <summary>
        /// 设置指定主域下的所有Cookies过期
        /// </summary>
        /// <param name="domain">域名，如 "caisf.net"</param>
        public static void SetExpiresAll(string domain)
        {
            int c = HttpContext.Current.Request.Cookies.Count;

            for (int i = 0; i < c; i++)
            {
                HttpCookie hc = HttpContext.Current.Request.Cookies[i];

                if (Url.CurDomain.ToLower() != "localhost") { hc.Domain = domain; }

                hc.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(hc);
            }
        }

        /// <summary>
        /// 设置指定主域下的所有Cookies过期
        /// </summary>
        /// <param name="domain">域名，如 "caisf.net"</param>
        /// <param name="path">所在路径，如 @"/"</param>
        public static void SetExpiresAll(string domain, string path)
        {
            int c = HttpContext.Current.Request.Cookies.Count;

            for (int i = 0; i < c; i++)
            {
                HttpCookie hc = HttpContext.Current.Request.Cookies[i];
                hc.Path = path;

                if (Url.CurDomain.ToLower() != "localhost") { hc.Domain = domain; }

                hc.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(hc);
            }
        }

        /// <summary>
        /// 设置所有Cookies过期
        /// </summary>
        public static void SetExpiresAll()
        {
            int c = HttpContext.Current.Request.Cookies.Count;

            for (int i = 0; i < c; i++)
            {
                HttpContext.Current.Response.Cookies[i].Value = null;
                HttpContext.Current.Response.Cookies[i].Expires = DateTime.Now.AddDays(-1);
            }
        }

        #endregion

        #region 移除 Cookies

        /// <summary>
        /// 移除Cookies
        /// </summary>
        /// <param name="name">名称</param>
        public static void Remove(string name)
        {
            RemoveKey(name, null);
        }

        /// <summary>
        /// 移除Cookies的子键
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="key">键</param>
        public static void RemoveKey(string name, string key)
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies[name];

            if (hc != null)
            {
                if (!string.IsNullOrEmpty(key) && hc.HasKeys)
                {
                    hc.Values.Remove(key);
                }
                else
                {
                    hc.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(hc);
                }
            }
        }

        /// <summary>
        /// 移除Cookies整个集合
        /// </summary>
        /// <param name="name">名称</param>
        public static void RemoveAllKey(string name)
        {
            if (IsExist(name) == true)
            {
                HttpCookie hc = HttpContext.Current.Request.Cookies[name];

                try
                {
                    int count = hc.Values.Count;
                    hc.Name.Remove(0, count);
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion
    }
}