using Pharos.Logic.ApiData.Pos.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class OrderItem
    {
        public bool IsMultiCode { get; set; }
        public string Barcode { get; set; }
        public string Title { get; set; }
        public string Size { get; set; }

        public string Brand { get; set; }

        public string Category { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public decimal ActualPrice { get; set; }

        public decimal Number { get; set; }
        public bool EnableEditNum { get; set; }
        public bool EnableEditPrice { get; set; }
        public SaleStatus Status { get; set; }
        public decimal Total { get; set; }

        public bool HasEditPrice { get; set; }
        public string RecordId { get; set; }

        public ProductStatus ProductStatus { get; set; }
    }
}
