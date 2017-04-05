﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace QCT.Pay.Common
{
    /// <summary>
    /// Sxf 支付订单类型
    /// </summary>
    public enum SxfOrderType : short
    {
        /// <summary>
        /// 普通订单
        /// </summary>
        [Description("普通订单")]
        CommonOrder = 1,
        /// <summary>
        /// 当面收款订单
        /// </summary>
        [Description("当面收款订单")]
        Face2FaceReceiptOrder = 1
    }
    /// <summary>
    /// Sxf 支付订单状态
    /// </summary>
    public enum SxfPayState:short
    {
        /// <summary>
        /// 未支付
        /// </summary>
        [Description("未支付")]
        U = 0,
        /// <summary>
        /// 支付成功
        /// </summary>
        [Description("支付成功")]
        S = 1,
        /// <summary>
        /// 支付失败
        /// </summary>
        [Description("支付失败")]
        F = 2,
        /// <summary>
        /// 已撤销
        /// </summary>
        [Description("已撤销")]
        C = 3,
        /// <summary>
        /// 支付超时
        /// </summary>
        [Description("支付超时")]
        T = 4
    }
    /// <summary>
    /// Sxf 商户退款状态
    /// </summary>
    public enum SxfRefundState : short
    {
        /// <summary>
        /// 退款中（预登记）
        /// </summary>
        [Description("退款中")]
        U = 0,
        /// <summary>
        /// 退款成功
        /// </summary>
        [Description("退款成功")]
        S = 1,
        /// <summary>
        /// 退款失败
        /// </summary>
        [Description("退款失败")]
        F = 2
    }
}
