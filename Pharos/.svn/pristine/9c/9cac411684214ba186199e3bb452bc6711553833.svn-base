using Pharos.Logic.ApiData.Pos.Sale.AfterSale;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Api.Retailing.Models.Pos
{
    public class ChangeRefundRequest:BaseApiParams
    {
        public string Barcode { get; set; }
        public decimal ChangeNumber { get; set; }
        public decimal ChangePrice { get; set; }
        public bool Status { get; set; }

        public AfterSaleMode Mode { get; set; }

        public string RecordId { get; set; }

        public ProductType ProductType { get; set; }
    }
}