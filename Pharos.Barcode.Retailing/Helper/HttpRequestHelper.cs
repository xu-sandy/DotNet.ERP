using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.Barcode.Retailing.Helper
{
    public class HttpRequestHelper
    {
        public void GetRequst<TResult>(string url, IDictionary<string, string> datas = null, Action<ErrorMessage> errorCallback = null, Action<TResult> successCallback = null)
        {
            if (datas != null && datas.Count > 0)
            {
                if (url.IndexOf("?") == -1)
                {
                    url += "?";
                }
                string paramInfos = string.Empty;
                foreach (var item in datas)
                {
                    paramInfos += string.Format("&{0}={1}", item.Key, item.Value);
                }
                if (url.IndexOf("&") > 0)
                {
                    url += paramInfos;
                }
                else
                {
                    url += paramInfos.Substring(1);
                }
            }
            Request<object, TResult>(HttpRequestMethod.Get, url, null, errorCallback, successCallback);
        }

        public void PostRequst<T, TResult>(string url, T datas, Action<ErrorMessage> errorCallback = null, Action<TResult> successCallback = null)
        {
            Request<T, TResult>(HttpRequestMethod.Post, url, datas, errorCallback, successCallback);
        }
        private void Request<T, TResult>(HttpRequestMethod method, string url, T datas, Action<ErrorMessage> errorCallback, Action<TResult> successCallback)
        {

            if (string.IsNullOrWhiteSpace(url))
            {
                if (errorCallback != null)
                    errorCallback(new ErrorMessage());
                return;
            }
            try
            {
                var httpClient = new HttpClient();
                Task<HttpResponseMessage> result = null;
                switch (method)
                {
                    case HttpRequestMethod.Get:
                        result = httpClient.GetAsync(url);
                        break;
                    case HttpRequestMethod.Post:
                        result = httpClient.PostAsJsonAsync(url, datas);
                        break;
                }
                if (result == null)
                {
                    if (errorCallback != null)
                        errorCallback(new ErrorMessage());
                    return;
                }
                result.ContinueWith((o) =>
                {

                    if (o.IsFaulted || !o.Result.IsSuccessStatusCode)
                    {
                        if (errorCallback != null)
                            errorCallback(new ErrorMessage());
                    }
                    o.Result.Content.ReadAsAsync<TResult>().ContinueWith((p) =>
                    {
                        if (p.IsFaulted)
                        {
                            if (errorCallback != null)
                                errorCallback(new ErrorMessage());
                        }
                        if (successCallback != null)
                            successCallback(p.Result);
                    });
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
    public enum HttpRequestMethod
    {
        Get,
        Post
    }

    public class ErrorMessage
    {

    }

}
