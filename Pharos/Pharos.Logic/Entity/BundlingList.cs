// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：
// --------------------------------------------------

using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using Pharos.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 捆绑销售清单： 用于管理本系统的商品捆绑促销清单信息（主表： Bundling）
    /// </summary>
    [Serializable]
    [Excel("捆绑商品信息")]
    public partial class BundlingList 
    {
        public int Id { get; set; }

        [Pharos.Utility.Exclude]
        public byte[] SyncItemVersion { get; set; }
		[Pharos.Utility.Exclude]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid SyncItemId { get; set; }
        [Excel("促销ID", 1)]
        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CommodityId { get; set; }

        [Excel("商品条码", 2)]
        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }

        public decimal? SysPrice { get; set; }
        public decimal? BuyPrice { get; set; }

        [Excel("每捆数量", 3)]
        /// <summary>
        /// 每捆数量
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public decimal Number { get; set; }

    }
}
