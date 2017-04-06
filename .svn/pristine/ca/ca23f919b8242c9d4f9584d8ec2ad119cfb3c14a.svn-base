using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pharos.Infrastructure.Net.Http.RestClient
{
    public class RestRequest<R> : IDisposable
   where R : class
    {
        private WebRequest _request = null;
        private HttpWebRequest request = null;
        private HttpWebResponse response = null;
        Stream requestStream = null;
        StreamWriter requestWriter = null;
        Stream responseStream = null;
        StreamReader responseReader = null;
        public RestRequest(RequestSetting setting)
        {
            Setting = setting;
            var queryString = GetQueryString(setting);
            UriBuilder ub = new UriBuilder(setting.Schema, setting.Host, setting.Port, setting.Path, queryString);
            Uri = ub.Uri;
            if (Uri.Scheme.ToLower() == "https")
            {
                ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidate;
            }
            _request = WebRequest.Create(Uri);
            request = _request as HttpWebRequest;
            _request.Method = setting.Method;
            foreach (var item in setting.Headers)
            {
                _request.Headers.Add(item);
            }
            _request.ContentType = setting.ContentType;
            _request.Timeout = setting.Timeout;
            if (request != null)
            {
                SetRequsetCookie(null);
            }
        }
        public RequestSetting Setting { get; private set; }
        public Uri Uri { get; private set; }
        public void SetRequsetCookie(CookieContainer cookie)
        {
            if (cookie != null)
            {
                request.CookieContainer = cookie;
            }
            else
            {
                request.CookieContainer = new CookieContainer();
            }
        }
        public CookieContainer GetResponeCookie()
        {
            if (response != null)
            {
                var result = new CookieContainer();
                if (response.Cookies.Count == 0)
                {
                    string cookies = response.Headers["Set-Cookie"];
                    if (!string.IsNullOrEmpty(cookies))
                    {
                        result.Add(parseCookies(cookies));
                    }
                }
                else
                {
                    result.Add(response.Cookies);
                }
                return result;
            }
            return null;
        }

        protected virtual string GetQueryString(RequestSetting setting)
        {
            string result = string.Empty;
            if (setting.UriParameters != null && setting.UriParameters.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("?");
                foreach (var item in setting.UriParameters)
                {
                    sb.AppendFormat("{0}={1}&", item.Key, item.Value);
                }
                result = sb.ToString();
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            if (error == SslPolicyErrors.None)
                return true;
            return false;
        }
        private CookieCollection parseCookies(string cookieHeader)
        {
            CookieCollection cc = new CookieCollection();
            var cookies = cookieHeader.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            Cookie cookie = null;
            for (int k = 0, len = cookies.Length; k < len; k++)
            {
                if (cookies[k].TrimStart().StartsWith("expires="))
                {
                    var arr = cookies[k].Trim().Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (cookie != null && arr.Length > 1)
                    {
                        try
                        {
                            cookie.Expires = DateTime.Parse(arr[1]);
                        }
                        catch (Exception ex)
                        {
                            cookie.Expires = DateTime.Now.AddDays(30);
                        }
                    }
                }
                else if (cookies[k].TrimStart().StartsWith("path="))
                {
                    var arr = cookies[k].Trim().Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (cookie != null && arr.Length > 1)
                    {
                        cookie.Path = arr[1];
                    }
                }
                else
                {
                    if (cookie == null)
                    {
                        cookie = new Cookie();
                        cookie.Domain = Uri.Host;
                        cc.Add(cookie);
                    }
                    var cookieInfo = cookies[k].Trim().Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (cookieInfo.Length > 0)
                    {
                        cookie.Name = cookieInfo[0];
                    }
                    if (cookieInfo.Length > 1)
                    {
                        cookie.Value = cookieInfo[1];
                    }
                }
            }
            return cc;
        }
        public virtual R Execute()
        {
            if (_request.Method != "GET")
            {
                requestStream = _request.GetRequestStream();
                requestWriter = new StreamWriter(requestStream);
                requestWriter.Write(Setting.Content);
                requestWriter.Close();
                requestWriter.Dispose();
                requestWriter = null;
            }
            response = _request.GetResponse() as HttpWebResponse;
            responseStream = response.GetResponseStream();
            responseReader = new StreamReader(responseStream);
            var rawResponse = responseReader.ReadToEnd();
            return JsonConvert.DeserializeObject<R>(rawResponse) as R;
        }

        public void Dispose()
        {
            if (requestStream != null)
            {
                if (requestWriter != null)
                {
                    requestWriter.Close();
                }
                else
                {
                    requestStream.Close();
                }
                requestStream.Dispose();
                requestStream = null;
            }
            if (responseStream != null)
            {
                if (responseReader != null)
                {
                    responseReader.Close();
                    responseReader.Dispose();
                    responseReader = null;
                }
                else
                {
                    responseStream.Close();
                    responseStream.Dispose();
                    responseStream = null;
                }
            }
            if (response != null)
            {
                response.Close();
            }
        }
    }
}
