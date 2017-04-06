using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    /// <summary>
    /// 买赠赠品清单
    /// </summary>
    [Excel("买赠赠品信息")]
    public class FreeGiftPurchaseListForLocal
    {
        /// <summary>
        /// 买赠ID
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("买赠ID", 1)]
        public string GiftId { get; set; }
        
        /// <summary>
        /// 赠品分类（1:单品、2:系列）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("赠品分类", 2)]
        public short GiftType { get; set; }

        /// <summary>
        /// 赠送件数
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        [Excel("赠送件数", 3)]
        public short GiftNumber { get; set; }
        
        /// <summary>
        /// 条码或系列ID
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("条码或系列", 4)]
        public string BarcodeOrCategorySN { get; set; }
    }
}
