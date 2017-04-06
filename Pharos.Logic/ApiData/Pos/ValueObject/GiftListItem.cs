using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class GiftListItem
    {
        public string Barcode { get; set; }
        public string Title { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public decimal SysPrice { get; set; }
        public decimal Inventory { get; set; }

    }
}
