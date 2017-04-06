﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Pharos.Api.Retailing.Models;
#if(Local!= true)
using Pharos.Component.qrcode;
using Pharos.Utility.Helpers;
using Newtonsoft.Json.Linq;
#endif
namespace Pharos.Api.Retailing
{
    public class ResponseFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Request.RequestUri.PathAndQuery.Contains("api/mobile/TradeNotify")) 
            {
                base.OnActionExecuted(actionExecutedContext);
                return;
            }
            if (actionExecutedContext.Response == null) return;
            var content = actionExecutedContext.Response.Content;
            var x = content == null ? null : content.ReadAsAsync<object>().Result;
            var result = new ApiRetrunResult<object>()
            {
                Code = "200",
                Result = x
            };
            var code = (System.Net.HttpStatusCode)Enum.Parse(typeof(System.Net.HttpStatusCode), result.Code, true);
            var response = actionExecutedContext.Request.CreateResponse<ApiRetrunResult<object>>(code, result, "application/json");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            //response.Content = new StringContent(string.Concat(result), Encoding.UTF8, "application/xml");
            ////var response = actionExecutedContext.Request.CreateResponse<string>(code, result.ToJson(), "application/json");
            //if (string.Equals(actionExecutedContext.ActionContext.ControllerContext.RouteData.Values["controller"].ToString(),"Login",StringComparison.CurrentCultureIgnoreCase))
            //{
            //    //var cookie = new CookieHeaderValue("session","hello!");
            //    //cookie.Expires = DateTimeOffset.Now.AddDays(1);
            //    //cookie.Domain = actionExecutedContext.Request.RequestUri.Host;
            //    //cookie.Path = "/";
            //    //response.Headers.AddCookies(new CookieHeaderValue[] { cookie } );
            //}
            actionExecutedContext.Response = response;
        }
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
#if(Local!= true)
                string content = "";
                if (actionContext.Request.Method == HttpMethod.Get)
                    content = actionContext.Request.RequestUri.Query;
                else
                {
                    if (actionContext.Request.Content.IsMimeMultipartContent("form-data"))
                    {
                        var provider = new MultipartMemoryStreamProvider();
                        System.Threading.Tasks.Task.Factory.StartNew(() =>
                        {
                            provider = actionContext.Request.Content.ReadAsMultipartAsync(provider).Result;
                        },
                        System.Threading.CancellationToken.None,
                        System.Threading.Tasks.TaskCreationOptions.LongRunning, // guarantees separate thread
                        System.Threading.Tasks.TaskScheduler.Default).Wait();
                        foreach (var pro in provider.Contents)
                        {
                            var title = pro.Headers.ContentDisposition.Name.Replace("\"", string.Empty);
                            if (pro.Headers.ContentType == null)
                                content += "," + title + "=" + pro.ReadAsStringAsync().Result;
                            else
                                content += ",附件名称:" + title;
                        }
                        content = content.TrimStart(',');
                    }
                    else
                        content =actionContext.Request.Content.ReadAsStringAsync().Result;//content-type=json

                    //var nvl= actionContext.Request.Content.ReadAsFormDataAsync().Result;
                    //var byts= actionContext.Request.Content.ReadAsByteArrayAsync().Result;
                    //var st= actionContext.Request.Content.ReadAsStreamAsync().Result;
                    if (content.IsNullOrEmpty() && actionContext.ActionArguments.Any())
                    {
                        content = actionContext.ActionArguments.Values.ToJson();
                        object obj=null;
                        if(actionContext.ActionArguments.TryGetValue("requestParams", out obj))
                        {
                            
                            var cid =(obj is JObject)?((JObject)obj).Property("CID",true): obj.GetPropertyValue("CID");
                            if (!cid.IsNullOrEmpty())
                                System.Web.HttpContext.Current.Items["CID"] = cid;
                        }
                    }
                }
                if (!content.IsNullOrEmpty())
                {
                    var parms = System.Web.HttpUtility.UrlDecode(content, Encoding.UTF8);
                    if (parms.Length > 5)
                    {
                        var json = " 参数:" + parms;
                        Log.Debug(actionContext.Request.RequestUri.AbsolutePath, json);
                    }
                }
#endif
            }
            catch { }
            base.OnActionExecuting(actionContext);
        }
    }
}