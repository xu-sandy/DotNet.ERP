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
    public class SaleDetail : BaseEntity, ICanUploadEntity
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
        /// 购买数量
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public decimal PurchaseNumber { get; set; }
        [Excel("系统进价", 4)]

        /// <summary>
        /// 系统进价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal BuyPrice { get; set; }
        [Excel("系统售价", 5)]

        /// <summary>
        /// 系统售价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal SysPrice { get; set; }
        [Excel("交易价", 6)]

        /// <summary>
        /// 交易价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal ActualPrice { get; set; }
        [Excel("销售分类ID", 7)]

        /// <summary>
        /// 销售分类ID（来自数据字典） 
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int SalesClassifyId { get; set; }
        [Excel("备注", 8)]

        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        public string Memo { get; set; }

        [JsonIgnore]

        public bool IsUpload { get; set; }

        [JsonIgnore]

        public DateTime CreateDT { get; set; }
    }
}
