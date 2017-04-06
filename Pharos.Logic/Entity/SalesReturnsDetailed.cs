using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    [Excel("换货明细")]
    public class SalesReturnsDetailed 
    {
        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 退换货ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("退换货ID", 1)]
        public string ReturnId { get; set; }
        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("商品条码", 2)]
        public string Barcode { get; set; }
        /// <summary>
        /// 票据单号 
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("票据单号", 3)]
        public string ReceiptsNumber { get; set; }
        /// <summary>
        /// 数量
        /// 默认值：1
        /// [不允许为空]
        /// </summary>
        [Excel("数量", 4)]
        public decimal Number { get; set; }

        /// <summary>
        /// 单价
        /// [长度：10]
        /// 默认值：0
        /// [不允许为空]
        /// </summary>
        [Excel("单价", 5)]
        public decimal Price { get; set; }
        /// <summary>
        /// 实销价 
        /// 默认值：0
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [Excel("实销价", 6)]
        public decimal TradingPrice { get; set; }
        /// <summary>
        /// 原值
        /// </summary>
        [NotMapped]
        public decimal ReNumber { get { return Number; } }
        [NotMapped]
        public string Title { get; set; }
        [NotMapped]
        public decimal BuyPrice { get; set; }
    }
}
