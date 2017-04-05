﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QCT.Pay.Common.Models
{
    /// <summary>
    /// qct 返回对象
    /// </summary>
    public class QctPayReturn
    {
        public QctPayReturn(string code, string msg,object data=null)
        {
            ReturnCode = code; 
            ReturnMsg = msg;
            Data = data;
        }
        /// <summary>
        /// 返回状态码
        /// </summary>
        [JsonProperty("return_code")]
        public string ReturnCode { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        [JsonProperty("return_msg")]
        public string ReturnMsg { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonProperty("successed")]
        public bool Successed { get; set; }

        /// <summary>
        /// 返回对象
        /// </summary>
        [JsonProperty("data")]
        public object Data { get; set; }

        #region methods
        /// <summary>
        /// 返回成功对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static QctPayReturn Success(string msg = "",object data=null)
        {
            var obj = new QctPayReturn(PayConst.SUCCESS_CODE, msg, data);
            obj.Successed = true;
            return obj;
        }
        /// <summary>
        /// 返回失败对象
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static QctPayReturn Fail(string msg = "参数格式错误")
        {
            var obj = new QctPayReturn(PayConst.FAIL_CODE_40004, msg);
            obj.Successed = false;
            return obj;
        }
        /// <summary>
        /// 返回失败对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static QctPayReturn Fail(string code, string msg = "参数格式错误")
        {
            var obj = new QctPayReturn(code, msg);
            obj.Successed = false;
            return obj;
        }
        #endregion

    }
    /// <summary>
    /// sxf 返回对象
    /// </summary>
    public class SxfPayReturn
    {
        public SxfPayReturn(string code, string msg)
        {
            Result = code; 
            Message = msg;
        }
        /// <summary>
        /// 返回状态码
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        #region methods
        /// <summary>
        /// 返回成功对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static SxfPayReturn Success(string code = PayConst.SXF_SUCCESS_RETURN, string msg = "")
        {
            return new SxfPayReturn(code,msg);
        }
        /// <summary>
        /// 返回失败对象
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static SxfPayReturn Fail(string msg = "参数格式错误")
        {
            var code = PayConst.FAIL_CODE_40004;
            return new SxfPayReturn(code, msg);
        }
        /// <summary>
        /// 返回失败对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static SxfPayReturn Fail(string code, string msg = "参数格式错误")
        {
            return new SxfPayReturn(code, msg);
        }
        #endregion

    }
}
