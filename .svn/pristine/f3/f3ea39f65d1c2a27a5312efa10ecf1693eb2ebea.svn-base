// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-07-24
// 描述信息：用于管理本系统的所有在库商品信息
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 商品库信息
    /// </summary>
    [Excel("商品库")]
    public partial class Commodity : BaseEntity
    {
        public Int64 Id { get; set; }
        [Excel("商品条码", 2)]
        [ExcelField(@"^[0-9]{1,30}$###商品条码长度应在1-30位且为数字")]
        [LocalKey]
        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }

        [Excel("在库数量", 3)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###在库数量格式错误")]

        /// <summary>
        /// 在库数量
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal StockNumber { get; set; }
    }
}
