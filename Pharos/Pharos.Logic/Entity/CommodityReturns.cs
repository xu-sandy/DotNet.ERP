// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：陈雅宾
// 创建时间：2015-09-22
// 描述信息：：用于管理本系统的库存退货信息
// --------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 库存退货信息
    /// </summary>
    [Serializable]
    public partial class CommodityReturns:BaseEntity
    {
        /// <summary>
        /// 记录ID
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
        /// 出货仓ID
        /// [长度：3]
        /// [不允许为空]
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// 经办人ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }

        /// <summary>
        /// 登记时间
        /// [不允许为空]
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 已完成时间
        /// </summary>
        public DateTime? VerifyTime { get; set; }
        /// <summary>
        /// 状态（ 0:未处理、1:处理中、 2:已处理）
        /// [不允许为空]
        /// </summary>
        public short State { get; set; }

        /// <summary>
        /// 退货供应商ID
        /// [长度：40]
        /// </summary>
        public string SupplierID { get; set; }
    }
}
