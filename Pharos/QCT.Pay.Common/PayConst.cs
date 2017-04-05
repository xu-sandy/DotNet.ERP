﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QCT.Pay.Common
{
    public class PayConst
    {
        #region qct
        /// <summary>
        /// 支付成功返回给商户的成功状态值，这里返回00000 是因为被SxfPay限定死了
        /// </summary>
        public const string SUCCESS_CODE = "00000";
        /// <summary>
        /// 业务处理失败
        /// </summary>
        public const string FAIL_CODE_40004 = "40004";
        /// <summary>
        /// 服务不可用，问题原因：1 服务暂不可用（业务系统不可用）2 服务暂不可用（网关自身的未知错误），解决方案：稍后重试
        /// </summary>
        public const string FAIL_CODE_20000 = "20000";
        /// <summary>
        /// 授权权限不足
        /// </summary>
        public const string FAIL_CODE_20001 = "20001";
        /// <summary>
        /// 缺少必选参数或非法的参数
        /// </summary>
        public const string FAIL_CODE_40001 = "40001";

        public const string DEF_CHARSET = "utf-8";
        public const string DEF_SIGNTYPE = "MD5";
        public const string DEF_VERSION = "1.0";
        #endregion

        #region QctPay Methods
        public const string QCTTRADE_PAY_BUYERSCAN_DYNA = "qct.trade.pay.buyerscandyna";
        public const string QCTTRADE_PAY_QRORDER = "qct.trade.pay.qrorder";
        public const string QCTTRADE_PAY_MERCHSCAN = "qct.trade.pay.merchscan";
        public const string QCTTRADE_REFUNDAPPLY = "qct.trade.refund.apply";

        public const string QCTTRADE_NOTIFY_PAY = "qct.trade.notify.pay";
        public const string QCTTRADE_NOTIFY_REFUND = "qct.trade.notify.refund";

        public const string QCTTRADE_QUERY_PAY = "qct.trade.query.pay";
        public const string QCTTRADE_QUERY_REFUND = "qct.trade.query.refund";
        public const string QCTTRADE_QUERY_PAYBATCH = "qct.trade.query.paybatch";
        #endregion

        #region SxfPay Methods Type
        public const string SXF_TYPE_BUILDPAYTOKEN = "buildPayToken";
        public const string SXF_TYPE_SCANPAY = "scanPay";
        public const string SXF_TYPE_RFDAPPLY = "rfdApply";
        public const string SXF_TYPE_PAYNOTIFY = "payNotify";
        public const string SXF_TYPE_RFDNOTIFY = "rfdNotify";
        public const string SXF_TYPE_PAYORDERQUERY = "payOrderQuery";
        public const string SXF_TYPE_RFDORDERQUERY = "rfdOrderQuery";
        public const string SXF_TYPE_PAYORDERPAGEQUERY = "payOrderPageQuery";
        #endregion

        #region sxf
        /// <summary>
        /// SxfPay接收通知返回成功状态值
        /// </summary>
        public const string SXF_SUCCESS_RETURN = "SUCCESS";
        /// <summary>
        /// SxfPay请求成功返回Code值
        /// </summary>
        public const string SXF_SUCCESS_CODE = "00000";
        /// <summary>
        /// 默认字符集值
        /// </summary>
        public const string SXF_DEF_CHARSET = "02";
        /// <summary>
        /// 默认签名类型
        /// </summary>
        public const string SXF_DEF_SIGNTYPE = "MD5";
        public const string SXF_SIGNATUREFIELD = "signature";

        public const string SXF_DEF_ORDERTYPE = "1";
        public const string SXF_DEF_VERSION = "1.0";
        #endregion

        /// <summary>
        /// 从元兑换为以分为单位的兑换率
        /// </summary>
        public const decimal YUAN_2_CENT_RATE = 100;
        public const decimal CENT_2_YUAN_RATE = 0.01m;
    }
}