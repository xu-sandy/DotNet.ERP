// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-07-24
// 描述信息：用于管理本系统的所有在库商品信息
// --------------------------------------------------

using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    /// <summary>
    /// 商品库信息
    /// </summary>
    [Excel("商品库")]
    public partial class CommodityForLocal
    {
        [Excel("商品条码", 2)]
        
        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }

        [Excel("在库数量", 3)]


        /// <summary>
        /// 在库数量
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal StockNumber { get; set; }

    }
}
