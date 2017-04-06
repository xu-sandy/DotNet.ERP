// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：用于管理本系统的商品买赠促销活动信息（主表： Bundling）
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 买赠促销
    /// </summary>
    [Excel("买赠信息")]
    public class FreeGiftPurchase : BaseEntity
    {
        public Int64 Id { get; set; }

        [LocalKey]
        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###促销ID应为Guid")]
        [Excel("促销ID", 1)]
        public string CommodityId { get; set; }
        [LocalKey]
        /// <summary>
        /// 买赠ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###买赠ID应为Guid")]
        [Excel("买赠ID", 2)]
        public string GiftId { get; set; }
        [ExcelField(@"^[0-9]{1,10}$###起购量为不大于10位数字且不为空")]

        /// <summary>
        /// 起购量
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        [Excel("起购量", 3)]
        public int MinPurchaseNum { get; set; }

        /// <summary>
        /// 每天限购次数（ 0:不限）
        /// [长度：5]
        /// [默认值：((0))]
        /// </summary>
        [Excel("每天限购次数", 4)]
        [ExcelField(@"^[0-9]{1,5}$###每天限购次数为不大于5位数字且不为空")]

        public short RestrictionBuyNum { get; set; }
        [ExcelField(@"^[0-9]{1,13}$###条码或系列ID应为1~13数字")]
        /// <summary>
        /// 条码或系列ID
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("条码或系列ID", 5)]
        public string BarcodeOrCategorySN { get; set; }

    }
}
