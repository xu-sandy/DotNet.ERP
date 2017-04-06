using Pharos.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.PosModels
{
    public enum StateToImg
    {
        /// <summary>
        /// 正常销售商品
        /// </summary>
        [EnumTitle(47, "")]
        Normal = 47,
        /// <summary>
        /// 捆绑
        /// </summary>
        [EnumTitle(48, @"Images\PosWindow\icon-bind.png")]
        Bunding = 48,
        /// <summary>
        /// 多种促销
        /// </summary>
        [EnumTitle(50, @"Images\PosWindow\icon-group.png")]
        MarkingGroup = 50,
        /// <summary>
        /// 单品折扣
        /// </summary>
        [EnumTitle(51, @"Images\PosWindow\icon-privilege.png")]
        Discount = 51,
        /// <summary>
        /// 非单品折扣、捆绑商品的促销
        /// </summary>
        [EnumTitle(52, @"Images\PosWindow\icon-sales.png")]
        Marking = 52,
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
