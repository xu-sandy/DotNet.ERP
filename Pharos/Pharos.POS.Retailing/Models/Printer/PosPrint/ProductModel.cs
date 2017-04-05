using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Pharos.POS.Retailing.Models.Printer
{

    public class ProductModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Num { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public bool IsPromotion { get; set; }
    }
}
