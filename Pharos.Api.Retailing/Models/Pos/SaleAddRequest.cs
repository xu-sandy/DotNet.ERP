using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Api.Retailing.Models.Pos
{
    public class SaleAddRequest : BaseApiParams
    {
        public string Barcode { get; set; }

        public Logic.ApiData.Pos.Sale.SaleStatus Status { get; set; }
    }
}
