using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    [Excel("销售明细信息")]
    /// <summary>
    /// 销售明细信息
    /// </summary>
    public class SaleDetailForLocal
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
        /// 购买数量
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [ExcelField(@"^[\d+(\.\d+)?]{1,10}$###购买数量格式不正确或长度超过10位或为空")]
        public decimal PurchaseNumber { get; set; }

        /// <summary>
        /// 系统进价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("系统进价", 4)]
        [ExcelField(@"^[\d+(\.\d+)?]{1,10}$###系统进价格式不正确或长度超过19位或为空")]
        public decimal BuyPrice { get; set; }

        /// <summary>
        /// 系统售价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("系统售价", 5)]
        [ExcelField(@"^[\d+(\.\d+)?]{1,10}$###系统售价格式不正确或长度超过19位或为空")]
        public decimal SysPrice { get; set; }

        /// <summary>
        /// 交易价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("交易价", 6)]
        [ExcelField(@"^[\d+(\.\d+)?]{1,10}$###交易价格式不正确或长度超过19位或为空")]
        public decimal ActualPrice { get; set; }

        /// <summary>
        /// 销售分类ID（来自数据字典） 
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("销售分类ID", 7)]
        [ExcelField(@"^[0-9]{1,3}$###销售分类格式不正确或长度超过10位或为空")]
        public int SalesClassifyId { get; set; }

        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        [Excel("备注", 8)]
        [ExcelField(@"^[\s,\S]{0,200}$###备注长度应在1-200位")]
        public string Memo { get; set; }
    }
}
