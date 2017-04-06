using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public class BundlingList:BaseEntity
    {
        public string CommodityId { get; set; }
        public string Barcode { get; set; }
        public decimal Number { get; set; }

        public decimal? SysPrice { get; set; }
        public decimal? BuyPrice { get; set; }
    }
}
