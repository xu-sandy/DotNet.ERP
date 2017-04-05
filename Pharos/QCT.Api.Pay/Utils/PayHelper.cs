﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Results;
using QCT.Pay.Common.Helpers;
using QCT.Pay.Common.Models;
using QCT.Pay.Common;
using Pharos.Logic.OMS.BLL.Pay;

namespace QCT.Api.Pay.Utils
{
    /// <summary>
    /// 支付帮助类
    /// </summary>
    public static class PayHelper
    {
        /// <summary>
        /// 发送Post支付请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static QctPayReturn SendPost(this string url, Dictionary<string, object> reqParams)
        {
            var result = PayHelper.SendPost(url, reqParams.ToSignString());
            return result;
        }
        /// <summary>
        /// 发送Post支付请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramsStr"></param>
        /// <returns></returns>
        public static QctPayReturn SendPost(this string url, string paramsStr)
        {
            string reqUrl = url + "?" + paramsStr;
            PayLogServer.WriteInfo(string.Format("发送交易请求：{0}", reqUrl));
            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.Timeout = 45000;
                byte[] byteRequest = System.Text.Encoding.UTF8.GetBytes(paramsStr);
                httpRequest.ContentLength = byteRequest.Length;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(byteRequest, 0, byteRequest.Length);
                requestStream.Close();

                //获取服务端返回
                var response = (HttpWebResponse)httpRequest.GetResponse();
                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                var result = sr.ReadToEnd().Trim();
                sr.Close();
                result = HttpUtility.UrlDecode(result, Encoding.UTF8);
                return QctPayReturn.Success(data: result);
            }
            catch (Exception ex)
            {
                var logEng = new LogEngine();
                logEng.WriteError(string.Format("发送交易请求异常：{0}，请求地址：{1}", ex.Message, reqUrl), ex, LogModule.支付交易);
                return QctPayReturn.Fail(code: PayConst.FAIL_CODE_20000, msg: "订单请求失败，服务器繁忙！");
            }
        }
    }
}