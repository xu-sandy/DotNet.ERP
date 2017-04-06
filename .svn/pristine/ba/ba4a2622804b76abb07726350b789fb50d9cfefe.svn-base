using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Pharos.Utility
{
    /// <summary>
    /// 序列化为JSON格式
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        public JsonSerializerSettings Settings { get; private set; }

        public JsonNetResult()
            : this(null, JsonRequestBehavior.AllowGet, null, null)
        {
        }
        public JsonNetResult(Object data) : this(data, JsonRequestBehavior.AllowGet, null, null) { }
        public JsonNetResult(Object data, String contentType) : this(data, JsonRequestBehavior.AllowGet, contentType, null) { }

        public JsonNetResult(Object data, JsonRequestBehavior behavior, String contentType, Encoding encoding)
        {
            this.Data = data;
            this.ContentType = contentType;
            this.ContentEncoding = encoding;
            this.JsonRequestBehavior = behavior;
            Settings = new JsonSerializerSettings
            {
                //解决.Net MVC EntityFramework Json 序列化循环引用问题.
                //这句是解决问题的关键,也就是json.net官方给出的解决配置选项.                 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON GET is not allowed");
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;
            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;
            var timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            Settings.Converters.Add(timeConverter);
            var scriptSerializer = JsonSerializer.Create(this.Settings);
            using (JsonTextWriter writer = new JsonTextWriter(response.Output))
            {
                scriptSerializer.Serialize(writer, this.Data);
                writer.Flush();
            }
        }
    }
    public class JsonShortDate : IsoDateTimeConverter
    {
        public JsonShortDate():base()
        {
            this.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}