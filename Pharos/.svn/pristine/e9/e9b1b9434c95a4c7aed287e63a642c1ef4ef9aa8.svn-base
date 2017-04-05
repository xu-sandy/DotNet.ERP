using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace QCT.Pay.Common.Helpers
{
    /// <summary>
    /// 数据签名帮助类
    /// </summary>
    public static class PaySignHelper
    {
        #region 数据签名
        /// <summary>
        /// 签名（支持Qct Sxf）
        /// </summary>
        /// <param name="signObj"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static string Sign(this Dictionary<String, Object> signObj, String secretKey)
        {
            StringBuilder signdatasb = new StringBuilder();
            foreach (var item in signObj)
            {
                String key = item.Key;
                String value = signObj[key] == null ? "" : signObj[key].ToString();
                signdatasb.Append("&").Append(key).Append("=").Append(value);
            }

            String signdata = signdatasb.ToString().Substring(1) + "&key=" + secretKey;
            return GetMD5(signdata);
        }
        /// <summary>
        /// 验签（支持Qct Sxf）
        /// </summary>
        /// <param name="signObj"></param>
        /// <param name="secretKey"></param>
        /// <param name="signValue"></param>
        /// <returns></returns>
        public static bool VerifySign(this Dictionary<string, object> signObj, string secretKey, string signField = "sign")
        {
            signField = string.IsNullOrWhiteSpace(signField) ? "sign" : signField;
            var signValue = signObj[signField].ToString();
            signObj.Remove(signField);
            var thisSign = Sign(signObj, secretKey);
            if (thisSign.Equals(signValue, StringComparison.OrdinalIgnoreCase))
                return true;
            else
                return false;
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMD5(string value)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            string str = "";
            for (int i = 0; i < data.Length; i++)
            {
                str += data[i].ToString("x2").ToUpperInvariant();
            }
            return str;
        }
        /// <summary>
        /// 转换为排序字典并添加签名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reqModel"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDicAndSign<T>(this T reqModel, string secretKey, string signField)
        {
            var dicMap = PaySignHelper.ToASCIIDictionary(reqModel);
            return SetSign(dicMap, secretKey, signField);
        }
        /// <summary>
        /// 数据字典对象添加签名属性
        /// </summary>
        /// <param name="dicMap"></param>
        /// <param name="secretKey"></param>
        /// <param name="signField"></param>
        /// <returns></returns>
        public static Dictionary<string, object> SetSign(Dictionary<string, object> dicMap, string secretKey, string signField)
        {
            dicMap.Remove(signField);
            var sign = PaySignHelper.Sign(dicMap, secretKey);
            dicMap.Add(signField, sign);
            return dicMap;
        }
        /// <summary>
        /// 转换请求参数为可签名类型的字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToSignString(this Dictionary<string, object> obj)
        {
            StringBuilder signdatasb = new StringBuilder();
            foreach (var item in obj)
            {
                String key = item.Key;
                var objVal = obj[key];
                if (!(objVal is IEnumerable && objVal.GetType().IsGenericType))
                {
                    String value = objVal == null ? "" : objVal.ToString();
                    signdatasb.Append("&").Append(key).Append("=").Append(value);
                }
            }
            return signdatasb.ToString().Substring(1);
        }
        /// <summary>
        /// 将对象转换为字典类型且根据ASCII进行排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToASCIIDictionary<T>(this T model)
        {
            var dicMap = new Dictionary<string, object>();
            var type = typeof(T);
            foreach (var item in type.GetProperties())
            {
                var attrs = item.GetCustomAttributes(typeof(JsonPropertyAttribute), false);
                var value = item.GetValue(model,null);
                if (!item.PropertyType.IsGenericType)
                {
                    if (attrs.Length > 0)
                    {
                        var key = ((JsonPropertyAttribute)attrs[0]).PropertyName;
                        value = value == null ? "" : value;
                        dicMap.Add(key, value);
                    }
                }
            }
            dicMap = dicMap.OrderBy(s => s.Key, StringComparer.Ordinal).ToDictionary(k => k.Key, v => v.Value);
            return dicMap;
        }
        #endregion
    }
}
