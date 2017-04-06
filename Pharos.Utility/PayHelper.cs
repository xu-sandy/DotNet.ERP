using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Pharos.Utility
{
    public class PayHelper
    {
        public static String Sign(SortedDictionary<String, Object> signObj, String md5Key)
        {
            StringBuilder signdatasb = new StringBuilder();
            foreach (var item in signObj)
            {
                String key = item.Key;
                String value = signObj[key] == null ? "" : signObj[key].ToString();
                signdatasb.Append("&").Append(key).Append("=").Append(value);
            }

            String signdata = signdatasb.ToString().Substring(1) + "&key=" + md5Key;
            return GetMD5(signdata);
        }
        public static string GetMD5(string value)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            string sh1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sh1 += data[i].ToString("x2").ToUpperInvariant();
            }
            return sh1;
        }
        public static SortedDictionary<string, object> Trans2SortedDic<T>(T model)
        {
            var sortDic = new SortedDictionary<string, object>();
            var type = typeof(T);
            foreach (var item in type.GetProperties())
            {
                var attrs = item.GetCustomAttributes(typeof(JsonPropertyAttribute), false);
                foreach (JsonPropertyAttribute attr in attrs)
                {
                    var key = attr.PropertyName;
                    var value = item.GetValue(model);
                    value = value == null ? "" : value;
                    sortDic.Add(key, value);
                }
            }
            return sortDic;
        }
    }
}
