﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    public class PayDetails
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public string PaySn { get; set; }
        /// <summary>
        /// 自定义流水号
        /// </summary>
        public string CustomOrderSn { get; set; }
        /// <summary>
        /// 应收款
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 第三方支付编号
        /// </summary>
        public string ApiOrderSn { get; set; }
        /// <summary>
        /// 储值卡号或者银行卡号（非必填）
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 储值卡密码（非必填）
        /// </summary>
        public string CardPassword { get; set; }
        /// <summary>
        /// 抹零金额
        /// </summary>
        public decimal WipeZero { get; set; }
        /// <summary>
        /// 收到的款
        /// </summary>
        public decimal Receive { get; set; }
        /// <summary>
        /// 找零
        /// </summary>
        public decimal Change { get; set; }
        /// <summary>
        /// 订单支付时间
        /// </summary>
        public DateTime CreateDt { get; set; }
        /// <summary>
        /// 支付方式（多方式支付时使用）
        /// </summary>
        public PayMode Mode { get; set; }
    }
}
