// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：用于管理本系统的商品捆绑促销活动信息（主表： CommodityPromotion）
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 捆绑销售
    /// </summary>
    [Excel("捆绑信息")]

    public class Bundling : BaseEntity
    {
        public Int64 Id { get; set; }
        [Excel("促销ID", 1)]
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###促销ID应为Guid")]
        [LocalKey]
        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CommodityId { get; set; }
        [Excel("新捆绑条码", 2)]
        [ExcelField(@"^[0-9]{1,30}$###新捆绑条码长度应在1-30位且为数字")]
        [LocalKey]
        /// <summary>
        /// 新捆绑条码
        /// [长度：30]
        /// </summary>
        public string NewBarcode { get; set; }
        [Excel("捆绑价", 3)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###捆绑价格式错误")]
        /// <summary>
        /// 捆绑价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal BundledPrice { get; set; }
        [ExcelField(@"^[0-9]{1,10}$###总捆数长度应在1-10位且为整数")]
        [Excel("总捆数", 4)]
        /// <summary>
        /// 总捆数
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public int TotalBundled { get; set; }
    }
}
