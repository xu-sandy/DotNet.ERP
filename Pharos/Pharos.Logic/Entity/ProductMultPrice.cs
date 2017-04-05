using Pharos.Sys.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 一品多价
    /// </summary>
    public class ProductMultPrice : SyncEntity
    {
        //[OperationLog("ID", false)]
        //public int Id { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        [OperationLog("条码", false)]
        public string Barcode { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        [OperationLog("门店", false)]
        public string StoreId { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        [OperationLog("售价", false)]
        public decimal? Price { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        [OperationLog("供应商", false)]
        public string SupplierId { get; set; }
        /// <summary>
        /// 进价
        /// </summary>
        [OperationLog("进价", false)]
        public decimal? BuyPrice { get; set; }
    }
}
