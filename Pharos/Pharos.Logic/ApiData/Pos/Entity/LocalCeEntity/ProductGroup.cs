using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public class ProductGroup : BaseEntity
    {
        public string Barcode { get; set; }
        public string GroupBarcode { get; set; }
        public decimal Number { get; set; }
        public DateTime CreateDT { get; set; }

        public string CreateUID { get; set; }
    }
}
