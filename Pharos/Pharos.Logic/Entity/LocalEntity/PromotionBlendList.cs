// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：用于管理本系统的商品随机组合促销活动信息和整单满元促销信息（主表：PromotionBlend）
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;

namespace Pharos.Logic.LocalEntity
{
    [Excel("组合促销商品")]

    /// <summary>
    /// 组合促销清单
    /// </summary>
    public class PromotionBlendList : BaseEntity
    {
        public Int64 Id { get; set; }
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###促销ID应为Guid")]

        [Excel("促销ID", 1)]
        /// <summary>
        /// 促销ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CommodityId { get; set; }

        [ExcelField(@"^[1,2,3,4,5,6]$###商品分类值范围（1:组合单品、2:组合系列、3:赠送单品、4:赠送系列、5:不参与促销单品、6:不参与促销系列）")]

        /// <summary>
        /// 商品分类（1:组合单品、2:组合系列、3:赠送单品、4:赠送系列、5:不参与促销单品、6:不参与促销系列）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("商品分类", 2)]
        public short BlendType { get; set; }
        [ExcelField(@"^[0-9]{1,20}$###品牌应为整数")]


        /// <summary>
        /// 品牌
        /// </summary>
        [Excel("品牌", 3)]
        public int BrandSN { get; set; }
        [ExcelField(@"^[0-9]{1,13}$###条码或系列ID应为1~13数字")]


        /// <summary>
        /// 条码或系列ID
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("条码或系列ID", 4)]
        public string BarcodeOrCategorySN { get; set; }
    }
}
