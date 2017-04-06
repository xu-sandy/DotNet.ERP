using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    [Excel("组合促销商品")]

    /// <summary>
    /// 组合促销清单
    /// </summary>
    public class PromotionBlendListForLocal
    {

        [Excel("促销ID", 1)]
        /// <summary>
        /// 促销ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CommodityId { get; set; }


        /// <summary>
        /// 商品分类（1:组合单品、2:组合系列、3:赠送单品、4:赠送系列、5:不参与促销单品、6:不参与促销系列）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("商品分类", 2)]
        public short BlendType { get; set; }


        /// <summary>
        /// 品牌
        /// </summary>
        [Excel("品牌", 3)]
        public int BrandSN { get; set; }


        /// <summary>
        /// 条码或系列ID
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("条码或系列ID", 4)]
        public string BarcodeOrCategorySN { get; set; }
    }
}
