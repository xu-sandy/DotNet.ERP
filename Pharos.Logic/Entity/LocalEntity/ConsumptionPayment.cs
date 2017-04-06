// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有商品销售结算方式信息（主表：SaleOrders）
// --------------------------------------------------

using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;

namespace Pharos.Logic.LocalEntity
{
    [Excel("消费支付方式")]
    /// <summary>
    /// 消费支付方式
    /// </summary>
    public class ConsumptionPayment : BaseEntity, ICanUploadEntity
    {
        public Int64 Id { get; set; }
        [Excel("流水号", 1)]

        [LocalKey]
        /// <summary>
        /// 流水号
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string PaySN { get; set; }
        [LocalKey]
        [Excel("支付方式 ID", 2)]

        /// <summary>
        /// 支付方式 ID
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int ApiCode { get; set; }
        [Excel("交易单号", 3)]

        /// <summary>
        /// 交易单号（由支付宝或微信返回）
        /// [长度：50]
        /// </summary>
        public string ApiOrderSN { get; set; }
        [Excel("金额", 4)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###金额格式错误")]

        /// <summary>
        /// 金额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal Amount { get; set; }
        [Excel("备注", 5)]
        [ExcelField(@"^[\s,\S]{0,200}$###备注不能超过200个字符")]

        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        public string Memo { get; set; }
        [Excel("支付状态", 6)]
        [ExcelField(@"^[0,1]$###支付状态值范围（0：未支付、1：已支付）")]

        /// <summary>
        /// 支付状态（0：未支付、1：已支付）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short State { get; set; }
        [Excel("卡号",7)]
        [ExcelField(@"^[0-9]{5,200}$###卡号应为数字序列")]

        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }

        public bool IsUpload { get; set; }


        public DateTime CreateDT { get; set; }
    }
}
