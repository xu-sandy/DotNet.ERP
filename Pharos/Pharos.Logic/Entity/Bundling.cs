// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：用于管理本系统的商品捆绑促销活动信息（主表： CommodityPromotion）
// --------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 捆绑销售
    /// </summary>
    [Serializable]
    [Excel("捆绑信息")]
    public class Bundling : SyncEntity
    {


        [Excel("促销ID", 1)]
        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CommodityId { get; set; }


        /// <summary>
        /// 新捆绑条码
        /// [长度：30]
        /// </summary>
        [Excel("新捆绑条码", 2)]
        public string NewBarcode { get; set; }
        /// <summary>
        /// 新品名
        /// </summary>
        [Excel("新品名", 3)]
        public string Title { get; set; }
        /// <summary>
        /// 捆绑价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("捆绑价", 4)]
        public decimal BundledPrice { get; set; }


        /// <summary>
        /// 总捆数
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        [Excel("总捆数", 5)]
        public int TotalBundled { get; set; }
        //[Pharos.Utility.Exclude]
        //public virtual CommodityPromotion Promotion { get; set; }
        /// <summary>
        /// 售价小计
        /// </summary>
        [Excel("售价小计", 6)]
        public decimal SysPrices { get; set; }
        /// <summary>
        /// 进价合计
        /// </summary>
        [Excel("进价小计", 7)]
        public decimal BuyPrices { get; set; }

    }
}
