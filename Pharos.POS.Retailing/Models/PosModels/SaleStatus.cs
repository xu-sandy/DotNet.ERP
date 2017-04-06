using Pharos.Wpf.Models;

namespace Pharos.POS.Retailing.Models.PosModels
{
    public enum SaleStatus
    {
        /// <summary>
        /// 正常销售商品
        /// </summary>
        [EnumTitle(47, "")]
        Normal = 47,
        /// <summary>
        /// 促销
        /// </summary>
        [EnumTitle(48, @"Images\PosWindow\icon-sales.png")]
        Promotion = 48,
        /// <summary>
        /// 临时赠品
        /// </summary>
        [EnumTitle(49, @"Images\PosWindow\icon-present.png")]
        POSGift = 49,
        /// <summary>
        /// 促销活动赠品
        /// </summary>
        [EnumTitle(161, @"Images\PosWindow\icon-present.png")]
        ActivityGifts = 161,
        /// <summary>
        /// 促销活动加购商品
        /// </summary>
        [EnumTitle(162, @"Images\PosWindow\icon-add.png")]
        ActivityAddMoneyGifts = 162
    }
}
