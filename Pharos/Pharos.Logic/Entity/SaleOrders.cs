// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有商品销售单信息
// --------------------------------------------------

using System;
using Pharos.Utility;
using System.ComponentModel.DataAnnotations;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 销售单信息
    /// </summary>
    [Serializable]

    [Excel("销售单信息")]
    public partial class SaleOrders : SyncEntity
    {

        /// <summary>
        /// 流水号（全局唯一）
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [Key]
        [Excel("流水号", 1)]
        public string PaySN { get; set; }


        /// <summary>
        /// 门店ID
        /// [长度：3]
        /// [不允许为空]
        /// </summary>
        [Excel("门店ID", 2)]
        public string StoreId { get; set; }


        /// <summary>
        /// POS机号
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        [Excel("POS机号", 3)]
        public string MachineSN { get; set; }



        /// <summary>
        /// 金额合计（优惠后)
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("金额合计", 4)]
        public decimal TotalAmount { get; set; }


        /// <summary>
        /// 优惠合计
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        [Excel("优惠合计", 5)]
        public decimal PreferentialPrice { get; set; }


        /// <summary>
        /// 支付方式ID（多个ID以,号间隔）
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        [Excel("支付方式ID", 6)]
        public string ApiCode { get; set; }


        /// <summary>
        /// 交易时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [Excel("交易时间", 7)]
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 退单日期
        /// </summary>
        public DateTime? ReturnDT { get; set; }
        /// <summary>
        /// 收银员UID 
        /// [长度：40]
        /// </summary>
        [Excel("收银员UID", 8)]
        public string CreateUID { get; set; }

        public string ReturnOrderUID { get; set; }

        /// <summary>
        /// 导购员UID
        /// [长度：40]
        /// </summary>
        [Excel("导购员UID", 9)]
        public string Salesman { get; set; }


        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        [Excel("备注", 10)]
        public string Memo { get; set; }

        /// <summary>
        /// 账单类型(0：正常销售；1：换货；2：退货) 默认值：0
        /// </summary>
        [Excel("账单类型", 11)]
        public short Type { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        [Excel("会员ID", 12)]
        public string MemberId { get; set; }

        /// <summary>
        /// 状态（默认：0，0：正常，1：退整单）
        /// </summary>
        public int State { get; set; }


        /// <summary>
        /// 退换ID
        /// （多个以逗号连接）
        /// </summary>
        public string ReturnId { get; set; }

        /// <summary>
        /// 数据源版本记录
        /// </summary>
        //public byte[] RowVersion { get; set; }

        public int ActivityId { get; set; }

        public decimal ProductCount { get; set; }
        /// <summary>
        /// 抹零运算后金额
        /// </summary>
        public decimal Receive { get; set; }
        /// <summary>
        /// 是否练习模式数据
        /// </summary>
        public bool IsTest { get; set; }
        /// <summary>
        /// 订单在库状态（0：未处理，1、已处理销售（包好销售、退货、换货新生成的订单）单、2、已退单处理的销售单）
        /// </summary>
        public short InInventory { get; set; }
        /// <summary>
        /// 记录是否已处理（0:未处理，1：已处理）
        /// </summary>
        public bool IsProcess { get; set; }

        public int? Reason { get; set; }
        /// <summary>
        /// 客户自定义流水号
        /// </summary>
        public string CustomOrderSn { get; set; }
        /// <summary>
        /// 退单原单流水号
        /// </summary>
        public string ReFundOrderCustomOrderSn { get; set; }
        /// <summary>
        /// 整单让利
        /// </summary>
        public decimal OrderDiscount { get; set; }

    }
}
