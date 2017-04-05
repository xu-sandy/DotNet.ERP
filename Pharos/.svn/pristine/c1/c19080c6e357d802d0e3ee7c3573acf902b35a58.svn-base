// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有商品销售明细信息（主表：SaleOrders） 
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices;
using System;
using System.Runtime.Serialization;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 销售明细信息总计附加项
    /// </summary>
    [Serializable]


    [Excel("销售明细信息总计附加项")]
    public class SaleDetailsTotal
    {
        

        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }
        

        /// <summary>
        /// 流水号 
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [Excel("流水号", 1)]
        public string PaySN { get; set; }
        

        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("商品条码", 2)]
        public string Barcode { get; set; }
        

        /// <summary>
        /// 金额
        /// [不允许为空]
        /// </summary>
        [Excel("金额", 3)]
        public decimal Total { get; set; }
    }
}
