﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{

    [Serializable]
    public class CommodityDiscount : SyncDataObject
    {
        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CommodityId { get; set; }
        public int CompanyId { get; set; }


        /// <summary>
        /// 单品条码
        /// [长度：30]
        /// </summary>
        public string Barcode { get; set; }


        /// <summary>
        /// 商品系列 ID
        /// [长度：10]
        /// </summary>
        public int CategorySN { get; set; }


        /// <summary>
        /// 折扣率（ %）
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((100))]
        /// </summary>
        public decimal? DiscountRate { get; set; }


        /// <summary>
        /// 折后价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal? DiscountPrice { get; set; }


        /// <summary>
        /// 起购量
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public decimal MinPurchaseNum { get; set; }
        /// <summary>
        /// 折扣方式 1-固定量,2-起购量
        /// </summary>
        public short Way { get; set; }
        /// <summary>
        /// 类别层级
        /// </summary>
        public short? CategoryGrade { get; set; }
    }
}
