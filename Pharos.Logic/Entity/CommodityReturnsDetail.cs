// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：陈雅宾
// 创建时间：2015-09-22
// 描述信息：：用于管理本系统的库存退货详细信息
// --------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 库存退货明细信息
    /// </summary>
    [Serializable]
    public partial class CommodityReturnsDetail
    {
        /// <summary>
        /// 明细ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 退货单号
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string ReturnId { get; set; }

        /// <summary>
        /// 条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 退货数量
        /// [不允许为空]
        /// </summary>
        public decimal ReturnNum { get; set; }

        /// <summary>
        /// 系统售价
        /// [不允许为空]
        /// </summary>
        public decimal SysPrice { get; set; }

        /// <summary>
        /// 备注
        /// [长度：200]
        /// [允许为空]
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 状态（ 0:未处理、1:处理中、 2:已处理）
        /// [不允许为空]
        /// </summary>
        public short State { get; set; }

        /// <summary>
        /// 进价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal BuyPrice { get; set; }

    }
}
