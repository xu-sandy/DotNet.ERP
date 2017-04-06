// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：用于管理本系统的商品随机组合促销活动信息和整单满元促销信息（主表：Bundling）
// --------------------------------------------------

using System;
using System.Runtime.Serialization;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 组合促销
    /// </summary>
    [Serializable]

    public class PromotionBlend : SyncEntity
    {
        /// <summary>
        /// 规则类别（1:组合促销、2:满元促销）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Pharos.Utility.Exclude]
        public short RuleType { get; set; }


        /// <summary>
        /// 促销ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Pharos.Utility.Exclude]
        public string CommodityId { get; set; }


        /// <summary>
        /// 促销形式（1:现金返回、2:代金券、3:折扣、4:加购按具体商品赠送、5:加购按单价范围赠送）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short PromotionType { get; set; }


        /// <summary>
        /// 满件数或满N元
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((2))]
        /// </summary>
        public decimal FullNumber { get; set; }


        /// <summary>
        /// 折扣或多少元
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal DiscountOrPrice { get; set; }


        /// <summary>
        /// 单价范围（0:不限）
        /// [长度：19，小数位数：4]
        /// [默认值：((0))]
        /// </summary>
        public decimal PriceRange { get; set; }


        /// <summary>
        /// 允许累加赠送（0:不允许、1:允许）
        /// [长度：5]
        /// [默认值：((1))]
        /// </summary>
        public short AllowedAccumulate { get; set; }

    }
}
