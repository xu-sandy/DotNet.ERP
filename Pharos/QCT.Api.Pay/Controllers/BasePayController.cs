using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Models;
using QCT.Pay.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using QCT.Pay.Common.Helpers;

namespace QCT.Api.Pay.Controllers
{
    public class BasePayController : ApiController
    {
        /// <summary>
        /// 日志记录引擎
        /// </summary>
        [Ninject.Inject]
        protected LogEngine LogEngine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="secretKey"></param>
        /// <param name="signField"></param>
        /// <returns></returns>
        internal JsonResult<Dictionary<string, object>> ToSignJson<T>(T result, string secretKey, string signField)
        {
            var rstRsp = result.ToDicAndSign(secretKey, signField);
            return Json(rstRsp);
        }
    }
}