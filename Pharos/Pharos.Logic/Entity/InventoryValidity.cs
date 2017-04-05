using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 有效期库存表
    /// </summary>

    public class InventoryValidity
    {
        public int Id { get; set; }
        /// <summary>
        ///  门店Id
        /// </summary>

        public string StoreId { get; set; }
        /// <summary>
        /// 条码
        /// </summary>

        public string Barcode { get; set; }
        /// <summary>
        /// 当前库存量
        /// </summary>

        public decimal StockNumber { get; set; }
        /// <summary>
        ///  生成日期
        /// </summary>

        public DateTime ProducedDate { get; set; }
        /// <summary>
        ///  有效期
        /// </summary>

        public short ExpiryDate { get; set; }
        /// <summary>
        ///  有效单位
        /// </summary>

        public short ExpiryDateUnit { get; set; }
        /// <summary>
        ///  截止保质日期
        /// </summary>

        public DateTime ExpirationDate { get; set; }
        /// <summary>
        /// 生产批次
        /// </summary>

        public string ProductionBatch { get; set; }
        /// <summary>
        ///  配送批次
        /// </summary>

        public string DistributionBatch { get; set; }
    }
}
