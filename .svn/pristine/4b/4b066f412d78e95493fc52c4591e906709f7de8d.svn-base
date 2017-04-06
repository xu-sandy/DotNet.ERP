using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Pharos.MessageAgent.Extensions
{
    public static class HttpRequestExtensions
    {
        #region HttpRequest
        public static string PostJson(this Uri url, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return PostString(url, json);
        }
        public static string PostString(this Uri url, string JSONData)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
            return PostBytes(url, bytes);
        }
        public static string PostBytes(this Uri url, byte[] datas)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentLength = datas.Length;
            request.ContentType = "text/json";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(datas, 0, datas.Length);

            //声明一个HttpWebRequest请求  
            request.Timeout = 90000;
            //设置连接超时时间  
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;

            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            return strResult;
        }
        #endregion HttpRequest
    }
}
