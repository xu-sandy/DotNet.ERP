// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有商品销售单信息
// --------------------------------------------------

using Newtonsoft.Json;
using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.LocalServices;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;
using System.Collections.Generic;

namespace Pharos.Logic.LocalEntity
{
    [Excel("销售单信息")]

    /// <summary>
    /// 销售单信息
    /// </summary>
    public class SaleOrders : BaseEntity, ICanUploadEntity, IEqualityComparer<SaleOrders>
    {
        public Int64 Id { get; set; }

        [LocalKey]
        [Excel("流水号", 1)]
        /// <summary>
        /// 流水号（全局唯一）
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string PaySN { get; set; }
        [ExcelField(@"^[0-9]{1,2}$###门店ID长度应在1-2位且为数字")]
        [Excel("门店ID", 2)]
        /// <summary>
        /// 门店ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string StoreId { get; set; }

        [Excel("POS机号", 3)]

        /// <summary>
        /// POS机号
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        public string MachineSN { get; set; }


        [Excel("金额合计", 4)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###金额合计格式错误")]

        /// <summary>
        /// 金额合计（优惠前)
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal TotalAmount { get; set; }
        [Excel("优惠合计", 5)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###优惠合计格式错误")]

        /// <summary>
        /// 优惠合计
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal PreferentialPrice { get; set; }
        [Excel("支付方式ID", 6)]

        /// <summary>
        /// 支付方式ID（多个ID以,号间隔）
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        public string ApiCode { get; set; }
        [Excel("交易时间", 7)]

        /// <summary>
        /// 交易时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }
        [Excel("收银员UID", 8)]

        /// <summary>
        /// 收银员UID 
        /// [长度：40]
        /// </summary>
        public string CreateUID { get; set; }
        [Excel("导购员UID", 9)]

        /// <summary>
        /// 导购员UID
        /// [长度：40]
        /// </summary>
        public string Salesman { get; set; }
        [Excel("备注", 10)]
        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        public string Memo { get; set; }
        [Excel("账单类型", 11)]
        /// <summary>
        /// 账单类型(0：正常销售；1：换货)
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public string MemberId { get; set; }
        [JsonIgnore]
        public bool IsUpload { get; set; }

        public int State { get; set; }

        public string ReturnId { get; set; }

        public bool Equals(SaleOrders x, SaleOrders y)
        {
            return x.PaySN == y.PaySN;
        }

        public int GetHashCode(SaleOrders obj)
        {
            return obj.PaySN.GetHashCode();
        }
    }
}
