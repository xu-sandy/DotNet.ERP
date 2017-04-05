﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class SaleOrders : SyncDataObject
    {
        /// <summary>
        /// 流水号（全局唯一）
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string PaySN { get; set; }


        /// <summary>
        /// 门店ID
        /// [长度：3]
        /// [不允许为空]
        /// </summary>
        public string StoreId { get; set; }

        public int CompanyId { get; set; }
        /// <summary>
        /// POS机号
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        public string MachineSN { get; set; }



        /// <summary>
        /// 金额合计（优惠后)
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal TotalAmount { get; set; }


        /// <summary>
        /// 优惠合计
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal PreferentialPrice { get; set; }


        /// <summary>
        /// 支付方式ID（多个ID以,号间隔）
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        public string ApiCode { get; set; }


        /// <summary>
        /// 交易时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 退单日期
        /// </summary>
        public DateTime? ReturnDT { get; set; }
        /// <summary>
        /// 收银员UID 
        /// [长度：40]
        /// </summary>
        public string CreateUID { get; set; }


        /// <summary>
        /// 导购员UID
        /// [长度：40]
        /// </summary>
        public string Salesman { get; set; }


        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 账单类型(0：正常销售；1：换货；2：退货) 默认值：0
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
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

        public string ReturnOrderUID { get; set; }

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
