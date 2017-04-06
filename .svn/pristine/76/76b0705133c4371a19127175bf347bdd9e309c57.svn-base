// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-07-24
// 描述信息：用于管理本系统的所有在库商品信息
// --------------------------------------------------

using System;
using System.Runtime.Serialization;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 商品库信息
    /// </summary>
    [Serializable]
    [Excel("商品库")]
    public partial class Commodity
    {
        

        /// <summary>
        /// 记录 ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        [Excel("门店ID", 1)]
        /// <summary>
        /// 门店 ID
        /// [长度：3]
        /// </summary>
        public string StoreId { get; set; }

        [Excel("商品条码",2)]
        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 生产日期
        /// [长度：10]
        /// </summary>
        public string ProducedDate { get; set; }

        /// <summary>
        /// 有效期（天数）
        /// [长度：5]
        /// [默认值：((-1))]
        /// </summary>
        public short? ExpiryDate { get; set; }

        /// <summary>
        /// 有效期单位（ 1:天、2:月、 3:年）
        /// [长度：5]
        /// [默认值：((1))]
        /// </summary>
        public short? ExpiryDateUnit { get; set; }

        /// <summary>
        /// 截止保质日期
        /// [长度：10]
        /// </summary>
        public string ExpirationDate { get; set; }

        /// <summary>
        /// 生产批次
        /// [长度：30]
        /// </summary>
        public string ProductionBatch { get; set; }
        [Excel("配送批次", 3)]
        /// <summary>
        /// 配送批次
        /// [长度：30]
        /// </summary>
        public string DistributionBatch { get; set; }

        [Excel("在库数量",4)]
        /// <summary>
        /// 在库数量
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal StockNumber { get; set; }
        /// <summary>
        /// 0-单品1－赠品
        /// </summary>
        public short Nature { get; set; }
    }
}
