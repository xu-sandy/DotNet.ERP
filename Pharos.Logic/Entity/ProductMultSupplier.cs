using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 一品多商
    /// </summary>
    public class ProductMultSupplier : SyncEntity
    {
        //public int Id { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierId { get; set; }
        /// <summary>
        /// 进价
        /// </summary>
        public decimal? BuyPrice { get; set; }
    }
}
