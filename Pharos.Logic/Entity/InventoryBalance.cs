using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 日库存结余表
    /// </summary>
    public class InventoryBalance:BaseEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ///  门店Id
        /// </summary>

        public string StoreId { get; set; }
        /// <summary>
        ///  条码
        /// </summary>

        public string Barcode { get; set; }
        /// <summary>
        ///  数量
        /// </summary>

        public decimal Number { get; set; }
        /// <summary>
        ///  库存结余时间
        /// </summary>

        public DateTime BalanceDate { get; set; }

        /// <summary>
        /// 周期（默认0，系统自动；0;实时结余、1:每天结余）
        /// </summary>
        public bool Period { get; set; }


        public decimal SaleAveragePrice { get; set; }
        public decimal SaleAmount { get; set; }
        public decimal StockAmount { get; set; }

    }
}
