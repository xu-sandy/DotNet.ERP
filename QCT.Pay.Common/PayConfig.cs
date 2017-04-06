using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

using QCT.Pay.Common;
using System.Collections;
using System.Configuration;

namespace QCT.Pay.Common
{
    /// <summary>
    /// 商户支付帮助类
    /// </summary>
    public class PayConfig
    {
        private static string _sxfPayNotifyUrl = string.Empty;
        private static string _sxfRefundNotifyUrl = string.Empty;
        private static string _saveTestLog = string.Empty;
        /// <summary>
        /// 是否保存测试日志
        /// </summary>
        public static bool IsSaveTestLog {
            get { 
                if(string.IsNullOrWhiteSpace(_saveTestLog))
                    _saveTestLog = ConfigurationManager.AppSettings["config_savetestlog"].ToString();
                if(_saveTestLog=="true")
                    return true;
                else
                    return false;
            }
        }

        #region sxf url
        /// <summary>
        /// 第三方随心付支付结果通知URL
        /// </summary>
        public static string SxfPayNotifyUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_sxfPayNotifyUrl))
                {
                    _sxfPayNotifyUrl = ConfigurationManager.AppSettings["sxfurl_notify_pay"].ToString();
                }
                return _sxfPayNotifyUrl;
            }
        }
        /// <summary>
        /// 第三方随心付退款结果通知URL
        /// </summary>
        public static string SxfRefundNotifyUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_sxfRefundNotifyUrl))
                {
                    _sxfRefundNotifyUrl = ConfigurationManager.AppSettings["sxfurl_notify_refund"].ToString();
                }
                return _sxfRefundNotifyUrl;
            }
        }
        #endregion
    }
}