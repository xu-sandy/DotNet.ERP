using Pharos.Logic.ApiData.Pos.Sale.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Api.Retailing.Models.Pos
{
    public class PayWay
    {
        /// <summary>
        /// 类型
        /// </summary>
        public PayMode Type { get; set; }
        /// <summary>
        /// 应收款 
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 收款
        /// </summary>
        public decimal Receive { get; set; }
        /// <summary>
        /// 找零
        /// </summary>
        public decimal Change { get; set; }

        /// <summary>
        /// 卡号或扫码编号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 抹零金额
        /// </summary>
        public decimal WipeZero { get; set; }

        public string PayOrderSn { get; set; }
    }
}