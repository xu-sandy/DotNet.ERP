using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Pharos.Utility
{
    public class HttpClient
    {
        /// <summary>
        /// jsonPost
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string parms, string contentType = "application/json")
        {
            byte[] data = Encoding.UTF8.GetBytes(parms);//使用UTF8
            return HttpPost(url, data, contentType);//json方式上传
        }
        public static string HttpPost(string url, byte[] data, string contentType)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                if (!string.IsNullOrWhiteSpace(contentType))
                    request.ContentType = contentType;
                request.ContentLength = data.Length;
                request.Headers.Add("size", data.Length.ToString());
                var reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
                var response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                var rt = sr.ReadToEnd().Trim();
                sr.Close();
                return rt;
            }
            catch (Exception ex)
            {
                return "404";
            }
        }
        /// <summary>
        /// 支付排序POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string PayPost(string url, SortedDictionary<string, object> obj)
        {
            StringBuilder signdatasb = new StringBuilder();
            foreach (var item in obj)
            {
                String key = item.Key;
                String value = obj[key] == null ? "" : obj[key].ToString();
                signdatasb.Append("&").Append(key).Append("=").Append(value);
            }
            byte[] byteRequest = System.Text.Encoding.UTF8.GetBytes(signdatasb.ToString().Substring(1));

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.ContentLength = byteRequest.Length;
            httpRequest.Timeout = 40000;
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(byteRequest, 0, byteRequest.Length);
            requestStream.Close();

            //获取服务端返回
            var response = (HttpWebResponse)httpRequest.GetResponse();
            //获取服务端返回数据
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var result = sr.ReadToEnd().Trim();
            sr.Close();
            return result;
        }

        public static string HttpGet(string url, string parms)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + parms);
                request.Method = "GET";
                request.ContentType = "text/json";
                var response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                var rt = sr.ReadToEnd().Trim();
                sr.Close();
                return rt;
            }
            catch (Exception ex)
            {
                return "error";
            }
        }
    }
}
