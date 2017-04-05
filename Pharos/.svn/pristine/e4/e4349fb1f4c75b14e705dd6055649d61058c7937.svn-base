using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    [Excel("换货明细")]

    public class SalesReturnsDetailedForLocal
    {
        /// <summary>
        /// 退换货ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("退换货ID", 1)]
        [ExcelField(@"^[\s,\S]{1,40}$###POS机号长度应在1-40位")]
        public string ReturnId { get; set; }

        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("商品条码", 2)]
        [ExcelField(@"^[\s,\S]{1,30}$###商品条码长度应在1-40位")]
        public string Barcode { get; set; }

        /// <summary>
        /// 票据单号 
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("票据单号", 3)]
        [ExcelField(@"^[\s,\S]{1,30}$###票据单号格式不正确或长度超过30位")]
        public string ReceiptsNumber { get; set; }

        /// <summary>
        /// 数量
        /// [长度：10]
        /// 默认值：1
        /// [不允许为空]
        /// </summary>
        [Excel("数量", 4)]
        [ExcelField(@"^[\d+(\.\d+)?]{1,19}$###数量格式不正确或长度超过10位")]
        public decimal Number { get; set; }

        /// <summary>
        /// 单价
        /// [长度：10]
        /// 默认值：0
        /// [不允许为空]
        /// </summary>
        [Excel("单价", 5)]
        [ExcelField(@"^[\d+(\.\d+)?]{1,19}$###单价格式不正确或长度超过10位")]
        public decimal Price { get; set; }

        /// <summary>
        /// 实销价 
        /// 默认值：0
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("实销价", 6)]
        [ExcelField(@"^[\d+(\.\d+)?]{1,30}$###实销价格式不正确或长度超过30位")]
        public decimal TradingPrice { get; set; }
    }
}
