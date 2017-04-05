using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale
{
    /// <summary>
    /// 销售状态
    /// </summary>
    public enum SaleStatus
    {
        /// <summary>
        /// 正常销售商品
        /// </summary>
        Normal = 47,
        /// <summary>
        /// 促销
        /// </summary>
        Promotion = 48,
        /// <summary>
        /// 临时赠品
        /// </summary>
        POSGift = 49,
        /// <summary>
        /// 促销活动赠品
        /// </summary>
        ActivityGifts = 161,
        /// <summary>
        /// 促销活动加购商品
        /// </summary>
        ActivityAddMoneyGifts = 162,
        /// <summary>
        /// 换货
        /// </summary>
        Changing = 159,
        /// <summary>
        /// 退货
        /// </summary>
        Refunding = 160
    }
}
