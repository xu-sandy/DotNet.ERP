using Pharos.Logic.ApiData.Pos.Sale.Barcodes;
using Pharos.Logic.ApiData.Pos.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Pharos.ObjectModels;

namespace Pharos.Logic.ApiData.Pos.Sale.AfterSale
{
    public class ChangeProduct : IIdentification
    {

        public ChangeProduct()
        {
            CurrentBarcode = new JsonBarcode();
        }
        public ChangeProduct(IBarcode barcode, bool status)
        {
            CurrentBarcode = barcode;
            IsChange = status;
            if (!status)
            {
                ChangeNumber = -barcode.SaleNumber;
            }
            else
            {
                ChangeNumber = barcode.SaleNumber;
            }
            Barcode = barcode.CurrentString;
            Title = barcode.Details.Title;
            ChangePrice = barcode.Details.SystemPrice;
            Total = ChangePrice * ChangeNumber;
            SysPrice = barcode.Details.SystemPrice;
            BuyPrice = barcode.Details.BuyPrice;
            Status = status;
            Unit = barcode.Details.Unit;
        }
        [JsonIgnore]
        internal IBarcode CurrentBarcode { get; set; }
        public ProductType ProductType { get; set; }
        public string Barcode { get; set; }

        public decimal BuyPrice { get; set; }
        public string Unit { get; set; }
        public bool Status { get; set; }
        public string Title { get; set; }
        public decimal ChangeNumber { get; set; }

        public decimal SysPrice { get; set; }
        public decimal ChangePrice { get; set; }

        public SaleStatus SaleStatus { get; set; }

        public decimal Total { get; set; }
        [JsonIgnore]
        public string MainBarcode
        {
            get
            {
                return CurrentBarcode.MainBarcode;
            }
            set
            {
                CurrentBarcode.MainBarcode = value;
            }
        }
        [JsonIgnore]
        public IEnumerable<string> MultiCode
        {
            get
            {
                return CurrentBarcode.MultiCode;

            }
            set
            {
                CurrentBarcode.MultiCode = value;

            }
        }
        [JsonIgnore]

        public bool HasEditPrice
        {
            get
            {
                return CurrentBarcode.HasEditPrice;

            }
            set
            {
                CurrentBarcode.HasEditPrice = value;

            }
        }

        public string RecordId
        {
            get
            {
                return CurrentBarcode.RecordId;

            }
            set
            {
                CurrentBarcode.RecordId = value;

            }
        }

        [JsonIgnore]
        public bool IsChange { get; set; }

    }
}
