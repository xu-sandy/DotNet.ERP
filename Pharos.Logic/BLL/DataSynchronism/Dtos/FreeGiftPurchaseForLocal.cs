using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    /// <summary>
    /// 买赠促销
    /// </summary>
    [Excel("买赠信息")]
    public class FreeGiftPurchaseForLocal
    {
        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Pharos.Utility.Exclude]
        [Excel("促销ID", 1)]
        public string CommodityId { get; set; }
        
        /// <summary>
        /// 买赠ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Pharos.Utility.Exclude]
        [Excel("买赠ID", 2)]
        public string GiftId { get; set; }

        /// <summary>
        /// 起购量
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        [Excel("起购量", 3)]
        public int MinPurchaseNum { get; set; }

        /// <summary>
        /// 每天限购次数（ 0:不限）
        /// [长度：5]
        /// [默认值：((0))]
        /// </summary>
        [Excel("每天限购次数", 4)]
        public short RestrictionBuyNum { get; set; }

        /// <summary>
        /// 条码或系列ID
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Pharos.Utility.Exclude]
        [Excel("条码或系列ID", 5)]
        public string BarcodeOrCategorySN { get; set; }

    }
}
