﻿using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Barcodes
{
    /// <summary>
    /// 用于获取Json反序列化结果
    /// </summary>
    public class JsonBarcode : IBarcode
    {
        public int Count { get; set; }
        public bool IsMultiCode { get; set; }

        public string CurrentString { get; set; }
        public IEnumerable<string> MultiCode { get; set; }

        public ProductType ProductType { get; set; }
        public bool HasEditPrice { get; set; }
        public BarcodeType BarcodeType { get; set; }

        public decimal SalePrice { get; set; }
        public decimal AveragePrice { get; set; }

        public decimal SaleNumber { get; set; }

        public string ProductCode { get; set; }

        public bool EnableMarketing { get; set; }
        public ProductDetails Details { get; set; }
        public string MainBarcode { get; set; }

        public List<Marketings.ActiveMarketingRule> MarketingMarks { get; set; }

        public bool VerfyEnableCombine(string barcode, SaleStatus status, string giftId, string giftPromotionId)
        {
            return false;
        }

        public bool VerfyEnableCombine(IBarcode barcode)
        {
            return false;
        }




        public decimal MarketingPrice { get; set; }

        public bool IsMarketingSplit { get; set; }

        public Dictionary<decimal, decimal> MarketingSplitRule { get; set; }


        public bool VerfyEnableCombine(string barcode, SaleStatus status, bool hasEditPrice = false, string recordId = "")
        {
            return false;
        }

        public bool SameProduct(string barcode)
        {
            return false;
        }

        public string RecordId { get; set; }


        public ProductStatus ProductStatus { get; set; }
    }
}
