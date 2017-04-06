using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public class SaleDetail : BaseEntity
    {
        public decimal Total { get; set; }

        public decimal AveragePrice { get; set; }

        public string ProductCode { get; set; }
        /// <summary>
        /// 流水号 
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string PaySN { get; set; }


        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }
        public string ScanBarcode { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 购买数量
        /// [不允许为空]
        /// </summary>
        public decimal PurchaseNumber { get; set; }


        /// <summary>
        /// 系统进价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal BuyPrice { get; set; }


        /// <summary>
        /// 系统售价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal SysPrice { get; set; }


        /// <summary>
        /// 交易价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal ActualPrice { get; set; }


        /// <summary>
        /// 销售分类ID（来自数据字典） 
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int SalesClassifyId { get; set; }


        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        public string Memo { get; set; }
    }
}
