﻿using Pharos.POS.Retailing.Models.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    /// <summary>
    /// 销售清单api返回结果
    /// </summary>
    public class ApiSaleReturn
    {
        public Statistics Statistics { get; set; }
        public IEnumerable<object> Gifts { get; set; }
        public ObservableCollection<Product> BuyList { get; set; }
    }

    public class Statistics
    {
        public decimal Total { get; set; }
        public decimal ManJianPreferential { get; set; }
        public decimal Preferential { get; set; }
        public decimal Receivable { get; set; }
        public int Num { get; set; }
        public string OrderSn { get; set; }

        /// <summary>
        /// 满件立减金额
        /// </summary>
        public decimal ZuHePreferential { get; set; }
        /// <summary>
        /// 满元立减金额
        /// </summary>
        public decimal ManYuanPreferential { get; set; }

    }
    public class GiftList
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

    public class Gifts
    {
        public string PromotionActivity { get; set; }
        public int GiftNumber { get; set; }
        public int ToGiftNumber { get; set; }
        public decimal Price { get; set; }
        public string GiftId { get; set; }
        public string GiftPromotionId { get; set; }
        public int Mode { get; set; }
        public int Amount { get; set; }
        public IEnumerable<GiftList> GiftList { get; set; }
    }
}
