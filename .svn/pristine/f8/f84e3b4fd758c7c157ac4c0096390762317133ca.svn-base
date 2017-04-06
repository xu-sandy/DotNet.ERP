// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：用于管理本系统的 商品 买赠促销活动的赠品信息 （主表： FreeGiftPurchase）
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 买赠赠品清单
    /// </summary>
    [Excel("买赠赠品信息")]
    public class FreeGiftPurchaseList : BaseEntity
    {
        public Int64 Id { get; set; }

        [LocalKey]
        /// <summary>
        /// 买赠ID
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("买赠ID", 1)]
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###买赠ID应为Guid")]
        public string GiftId { get; set; }
        [LocalKey]
        /// <summary>
        /// 赠品分类（1:单品、2:系列）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("赠品分类", 2)]
        [ExcelField(@"^[1,2]$###赠品分类值范围（1:单品、2:系列）")]
        public short GiftType { get; set; }

        /// <summary>
        /// 赠送件数
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        [Excel("赠送件数", 3)]
        [ExcelField(@"^[0-9]{1,5}$###赠送件数为不大于5位数字且不为空")]

        public short GiftNumber { get; set; }
        [LocalKey]
        /// <summary>
        /// 条码或系列ID
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [ExcelField(@"^[0-9]{1,13}$###条码或系列ID应为1~13数字")]

        [Excel("条码或系列", 4)]
        public string BarcodeOrCategorySN { get; set; }
    }
}
