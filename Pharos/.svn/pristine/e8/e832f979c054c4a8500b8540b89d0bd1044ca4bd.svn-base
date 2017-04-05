// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有商品销售明细信息（主表：SaleOrders） 
// --------------------------------------------------

using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.LocalServices;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;
using Newtonsoft.Json;

namespace Pharos.Logic.LocalEntity
{
    [Excel("销售明细信息")]
    /// <summary>
    /// 销售明细信息
    /// </summary>
    public class SaleDetailsTotal : BaseEntity, ICanUploadEntity
    {
        [JsonIgnore]
        public Int64 Id { get; set; }
        [Excel("流水号", 1)]

        [LocalKey]

        /// <summary>
        /// 流水号 
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string PaySN { get; set; }
        [LocalKey]
        [Excel("商品条码", 2)]

        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }
        [Excel("购买数量", 3)]

        /// <summary>
        /// 总额
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public decimal Total { get; set; }
        [Excel("系统进价", 4)]


        [JsonIgnore]

        public bool IsUpload { get; set; }

        [JsonIgnore]

        public DateTime CreateDT { get; set; }
    }
}
