// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：用于管理本系统的商品买赠促销活动信息（主表： Bundling）
// --------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 买赠促销
    /// </summary>
    [Serializable]
    [Excel("买赠信息")]
    public class FreeGiftPurchase : SyncEntity
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
        /// 分类（1:单品、2:系列）
        /// </summary>
        public short GiftType { get; set; }
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
        public decimal MinPurchaseNum { get; set; }


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
        /// <summary>
        /// 品牌
        /// </summary>
        public int? BrandSN { get; set; }

        [NotMapped]
        public string Barcodes { get; set; }
        [NotMapped]
        public string Barcode { get; set; }
        [NotMapped]
        public string CategorySN { get; set; }


        //public virtual List<FreeGiftPurchaseList> FreeGiftPurchaseDetails { get; set; }
        /// <summary>
        /// 类别层级
        /// </summary>
        public short? CategoryGrade { get; set; }

    }
}
