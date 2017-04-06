using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public class Bundling : BaseEntity
    {
        public string CommodityId { get; set; }
        public string NewBarcode { get; set; }
        public string Title { get; set; }
        public decimal BundledPrice { get; set; }
        public int TotalBundled { get; set; }
        public decimal SysPrices { get; set; }
        public decimal BuyPrices { get; set; }
    }
}
