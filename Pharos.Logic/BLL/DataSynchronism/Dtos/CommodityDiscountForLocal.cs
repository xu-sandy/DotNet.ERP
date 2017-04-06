using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    /// <summary>
    /// 单品折扣
    /// </summary>
    [Excel("单品折扣")]
    public class CommodityDiscountForLocal
    {
        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Pharos.Utility.Exclude]
        

        [Excel("促销ID", 1)]
        public string CommodityId { get; set; }
        
        [Excel("单品条码", 2)]

        /// <summary>
        /// 单品条码（多个以,号间隔）
        /// [长度：30]
        /// </summary>
        public string Barcode { get; set; }
        [Excel("商品系列", 3)]

        /// <summary>
        /// 商品系列 ID（多个以,号间隔）
        /// [长度：10]
        /// </summary>
        public int CategorySN { get; set; }
        [Excel("折扣率", 4)]

        /// <summary>
        /// 折扣率（ %）
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((100))]
        /// </summary>
        public decimal DiscountRate { get; set; }
        [Excel("折后价", 5)]

        /// <summary>
        /// 折后价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal? DiscountPrice { get; set; }
        [Excel("起购量", 6)]

        /// <summary>
        /// 起购量
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public int MinPurchaseNum { get; set; }

        public int Way { get; set; }

    }
}
