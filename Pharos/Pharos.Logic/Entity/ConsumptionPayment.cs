// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有商品销售结算方式信息（主表：SaleOrders）
// --------------------------------------------------

using System;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 消费支付方式
    /// </summary>
    [Serializable]
    [Excel("消费支付方式")]
    public class ConsumptionPayment : SyncEntity
    {


        ///// <summary>
        ///// 记录 ID
        ///// [主键：√]
        ///// [长度：10]
        ///// [不允许为空]
        ///// </summary>
        //public int Id { get; set; }


        /// <summary>
        /// 流水号
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [Excel("流水号", 1)]
        public string PaySN { get; set; }


        /// <summary>
        /// 支付方式 ID
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("支付方式 ID", 2)]
        public int ApiCode { get; set; }


        /// <summary>
        /// 交易单号（由支付宝或微信返回）
        /// [长度：50]
        /// </summary>
        [Excel("交易单号", 3)]
        public string ApiOrderSN { get; set; }


        /// <summary>
        /// 金额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        [Excel("金额", 4)]
        public decimal Amount { get; set; }


        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        [Excel("备注", 5)]
        public string Memo { get; set; }


        /// <summary>
        /// 支付状态（0：未支付、1：已支付）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("支付状态", 6)]
        public short State { get; set; }

        public string CardNo { get; set; }

        public decimal Change { get; set; }

        public decimal Received { get; set; }

    }
}