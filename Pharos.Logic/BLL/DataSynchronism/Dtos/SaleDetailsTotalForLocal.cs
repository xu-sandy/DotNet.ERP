using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    [Excel("销售明细信息总计")]
    /// <summary>
    /// 销售明细信息
    /// </summary>
    public class SaleDetailsTotalForLocal
    {
        /// <summary>
        /// 流水号 
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [Excel("流水号", 1)]
        [ExcelField(@"^[\s,\S]{1,50}$###POS机号长度应在1-50位或为空")]
        public string PaySN { get; set; }

        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("商品条码", 2)]
        [ExcelField(@"^[\s,\S]{1,50}$###商品条码长度应在1-50位或为空")]
        public string Barcode { get; set; }
        [Excel("购买数量", 3)]

        /// <summary>
        /// 金额
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [ExcelField(@"^[\d+(\.\d+)?]{1,10}$###金额格式不正确或长度超过10位或为空")]
        public decimal Total { get; set; }
    }
}
