// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：用于管理本系统的商品随机组合促销活动信息和整单满元促销信息（主表：Bundling）
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 组合促销
    /// </summary>
    [Excel("组合信息")]
    public class PromotionBlend : BaseEntity
    {
        public Int64 Id { get; set; }

        /// <summary>
        /// 规则类别（1:组合促销、2:满元促销）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [LocalKey]
        [Excel("规则类别", 1)]
        [ExcelField(@"^[1,2]$###规则类别值范围（1:组合促销、2:满元促销）")]

        public short RuleType { get; set; }

        /// <summary>
        /// 促销ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [LocalKey]
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###促销ID应为Guid")]
        [Excel("促销ID", 2)]
        public string CommodityId { get; set; }
        [ExcelField(@"^[1,2,3,4,5]$###促销形式值范围（1:现金返回、2:代金券、3:折扣、4:加购按具体商品赠送、5:加购按单价范围赠送）")]

        /// <summary>
        /// 促销形式（1:现金返回、2:代金券、3:折扣、4:加购按具体商品赠送、5:加购按单价范围赠送）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("促销形式", 3)]
        public short PromotionType { get; set; }

        /// <summary>
        /// 满件数或满N元
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((2))]
        /// </summary>
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###满件数格式错误")]
        [Excel("满件数", 4)]
        public decimal FullNumber { get; set; }

        /// <summary>
        /// 折扣或多少元
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        [Excel("折扣或多少元", 5)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###折扣或多少元格式错误")]
        public decimal DiscountOrPrice { get; set; }

        /// <summary>
        /// 单价范围（0:不限）
        /// [长度：19，小数位数：4]
        /// [默认值：((0))]
        /// </summary>
        [Excel("单价范围", 6)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###单价范围格式错误")]
        public decimal PriceRange { get; set; }

        /// <summary>
        /// 允许累加赠送（0:不允许、1:允许）
        /// [长度：5]
        /// [默认值：((1))]
        /// </summary>
        [Excel("允许累加赠送", 7)]
        [ExcelField(@"^[0,1]*$###允许累加赠送值范围（0:不允许、1:允许）")]
        public short AllowedAccumulate { get; set; }
    }
}
