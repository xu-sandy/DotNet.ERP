using Pharos.Logic.ApiData.Pos.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Pharos.Logic.ApiData.Pos.Sale.Marketings;
using Pharos.Logic.ApiData.Pos.ValueObject;

namespace Pharos.Logic.ApiData.Pos.Sale.Barcodes
{
    public interface IBarcode : IIdentification
    {
        BarcodeType BarcodeType {get;set;}
        /// <summary>
        /// 条码长度
        /// </summary>
        int Count { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        string CurrentString { get; set; }

        ProductStatus ProductStatus { get; }
        /// <summary>
        /// 销售价
        /// </summary>
        decimal SalePrice { get; set; }
        /// <summary>
        /// 促销中间价
        /// </summary>
        decimal MarketingPrice { get; set; }

        /// <summary>
        ///活动拆分
        /// </summary>
        bool IsMarketingSplit { get; set; }
        /// <summary>
        /// 多码识别
        /// </summary>
        bool IsMultiCode { get; set; }

        /// <summary>
        /// 活动拆分规则<数量,金额>
        /// </summary>
        Dictionary<decimal, decimal> MarketingSplitRule { get; set; }

        /// <summary>
        /// 均价
        /// </summary>
        decimal AveragePrice { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        decimal SaleNumber { get; set; }

        /// <summary>
        /// 商品货号
        /// </summary>
        string ProductCode { get; set; }
        /// <summary>
        /// 商品详细信息
        /// </summary>
        ProductDetails Details { get; set; }
        /// <summary>
        /// 验证是否条码是否允许合并
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        bool VerfyEnableCombine(string barcode, SaleStatus status, bool hasEditPrice = false, string recordId = "");

        /// <summary>
        /// 验证是否为同一商品
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        bool SameProduct(string barcode);

        /// <summary>
        /// 是否允许促销
        /// </summary>
        bool EnableMarketing { get; set; }


        /// <summary>
        /// 商品促销标识记录
        /// </summary>
        List<ActiveMarketingRule> MarketingMarks { get; set; }
    }
}
