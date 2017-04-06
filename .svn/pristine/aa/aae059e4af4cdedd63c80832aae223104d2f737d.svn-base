using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class PromotionBlendList : SyncDataObject
    {
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
        public short BlendType { get; set; }


        /// <summary>
        /// 品牌
        /// </summary>
        public int BrandSN { get; set; }


        /// <summary>
        /// 条码或系列ID
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string BarcodeOrCategorySN { get; set; }
        /// <summary>
        /// 类别层级
        /// </summary>
        public short? CategoryGrade { get; set; }
    }
}
