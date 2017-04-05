﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class AppPaymentRecords
    {

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 商户标识
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 交易单号
        /// </summary>
        public string OrderSN { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 实收
        /// </summary>
        public decimal Received { get; set; }
        /// <summary>
        /// 支付接口
        /// </summary>
        public int ApiCode { get; set; }
        /// <summary>
        /// 支付渠道
        /// </summary>
        public string PayChannel { get; set; }
        /// <summary>
        /// 状态（0:未支付；1支付成功；2：支付失败；3：已撤销；4：支付超时）
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// CreateUser
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// ReturnData
        /// </summary>
        public string ReturnData { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserCode { get; set; }
    }
}
