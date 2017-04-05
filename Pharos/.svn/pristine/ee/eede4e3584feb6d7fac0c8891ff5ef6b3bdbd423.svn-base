// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：用于管理本系统的商品折扣信息（主表： CommodityPromotion）
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 单品折扣
    /// </summary>
    [Excel("单品折扣")]
    public class CommodityDiscount : BaseEntity
    {
        public Int64 Id { get; set; }

        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [LocalKey]
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###促销ID应为Guid")]

        [Excel("促销ID", 1)]
        public string CommodityId { get; set; }
        [LocalKey]
        [Excel("单品条码", 2)]
        [ExcelField(@"^[0-9]{1,13}$###单品条码长度应在1-13位且为数字")]

        /// <summary>
        /// 单品条码（多个以,号间隔）
        /// [长度：30]
        /// </summary>
        public string Barcode { get; set; }
        [Excel("商品系列", 3)]
        [ExcelField(@"^[0-9,-]{1}[0-9]{0,13}$###商品系列应在1-13位且为数字")]

        /// <summary>
        /// 商品系列 ID（多个以,号间隔）
        /// [长度：10]
        /// </summary>
        public int CategorySN { get; set; }
        [Excel("折扣率", 4)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###商品系列应在1-13位且为浮点数")]
        /// <summary>
        /// 折扣率（ %）
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((100))]
        /// </summary>
        public decimal DiscountRate { get; set; }
        [Excel("折后价", 5)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###折后价格式错误")]

        /// <summary>
        /// 折后价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal? DiscountPrice { get; set; }
        [Excel("起购量", 6)]
        [ExcelField(@"^[0-9]{1,10}$###单品条码长度应在1-10位且为数字")]
        /// <summary>
        /// 起购量
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public int MinPurchaseNum { get; set; }
        [Excel("折扣方式", 6)]
        [ExcelField(@"^[1,2]$###折扣方式为数字,1-固定量,2-起购量")]
        public int Way { get; set; }
    }
}
