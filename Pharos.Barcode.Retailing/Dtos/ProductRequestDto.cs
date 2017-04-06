using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Barcode.Retailing.Dtos
{
    public class ProductRequestDto
    {
        public string KeyWord { get; set; }

        public string Store { get; set; }

        public int ProductBrand { get; set; }

        public List<int> Categories { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
