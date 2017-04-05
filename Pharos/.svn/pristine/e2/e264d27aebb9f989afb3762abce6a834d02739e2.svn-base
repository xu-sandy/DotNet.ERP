// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：
// --------------------------------------------------

using Newtonsoft.Json;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 捆绑销售清单： 用于管理本系统的商品捆绑促销清单信息（主表： Bundling）
    /// </summary>
    [Excel("捆绑商品信息")]
    public class BundlingList : BaseEntity
    {
        public Int64 Id { get; set; }
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###促销ID应为Guid")]
        [LocalKey]
        [Excel("促销ID", 1)]

        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CommodityId { get; set; }
        [Excel("商品条码", 2)]
        [ExcelField(@"^[0-9]{1,30}$###商品条码长度应在1-30位且为数字")]

        [LocalKey]
        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }
        [Excel("每捆数量", 3)]
        [ExcelField(@"^[0-9]{1,10}$###每捆数量长度应在1-10位且为整数")]
        /// <summary>
        /// 每捆数量
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public int Number { get; set; }
    }
}
