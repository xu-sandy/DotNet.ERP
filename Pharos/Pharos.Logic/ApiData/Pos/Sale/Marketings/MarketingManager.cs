﻿using Pharos.Infrastructure.Data.Normalize;
using Pharos.Logic.ApiData.Pos.Cache;
using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.Sale.Barcodes;
using Pharos.ObjectModels.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pharos.Logic.ApiData.Pos.Sale.Marketings
{
    /// <summary>
    /// 促销活动管理器
    /// </summary>
    public class MarketingManager
    {
#if(Local!=true)
        ActiveMarketingRuleCache activeMarketingRuleCache = new ActiveMarketingRuleCache();
#endif
#if(Local)
        Pharos.Logic.Cache.ActiveMarketingRuleCache activeMarketingRuleCache = Pharos.Service.Retailing.Marketing.MarketingManager.activeMarketingRuleCache;
#endif

        public MarketingManager(string storeId, int companyId)
        {
            StoreId = storeId;
            CompanyId = companyId;
        }

        public int CompanyId { get; set; }
        public string StoreId { get; set; }
        public string Key { get { return KeyFactory.StoreKeyFactory(CompanyId, StoreId); } }

        /// <summary>
        /// 当前促销规则
        /// </summary>
        public IEnumerable<ActiveMarketingRule> CurrentRules { get { return activeMarketingRuleCache.Get(Key); } }





        /// <summary>
        /// 匹配促销信息
        /// </summary>
        /// <param name="shoppingCart">购物车</param>
        /// <param name="outObj">输出促销匹配结果</param>
        /// <param name="zuHeMinus">组合立减金额</param>
        /// <param name="manYuanMinus">满元立减金额</param>
        /// <returns>总额促销</returns>
        public decimal Match(ShoppingCart shoppingCart, out IEnumerable<MarketingContext> outObj, out decimal zuHeMinus, out decimal manYuanMinus)
        {
            zuHeMinus = 0;//默认
            manYuanMinus = 0;//默认
            var tempRules = CurrentRules;//防止自动更新促销信息时，匹配促销活动方式资源争抢，造成死锁
            List<MarketingContext> result = new List<MarketingContext>();
            decimal totalPreferential = 0;
            if (tempRules == null || tempRules.Count() == 0)
                goto Return;
            var marketingGroups = tempRules.GroupBy(o => o.Type).OrderBy(p => p.Key).ToList();//促销优惠顺序 单品折扣-》买赠促销-》组合促销-》满元促销
            foreach (var child in marketingGroups)
            {
                List<ActiveMarketingRule> marketings;
                if (child.Key == MarketingType.Manyuan)
                {
                    marketings = child.OrderByDescending(o => o.RuleNumber).ToList();//满元同等促销顺序 按最新规则金额为最优
                }
                else
                {
                    marketings = child.OrderByDescending(o => o.CreateRuleDate).ToList();//同等促销顺序 按最新创建日期为最优

                }

                foreach (var item in marketings)
                {
                    var context = item.Match(shoppingCart, StoreId);
                    if (context != MarketingContext.Empty)
                    {
                        switch (context.MarketingRule.MarketingAction.MarketingActionMode)
                        {
                            case MarketingActionMode.DiscountAmount:
                                if (context.MarketingRule.MeteringMode == MeteringMode.GuDingLiang)
                                {
                                    foreach (var product in context.MatchRanges)
                                    {
                                        var num = product.SaleNumber - context.MarketingRule.RuleNumber;
                                        if (num > 0)
                                        {
                                            var barcode = BarcodeFactory.Factory(shoppingCart.MachineInformation.StoreId, shoppingCart.MachineInformation.MachineSn, shoppingCart.MachineInformation.CompanyId, product.MainBarcode, product.Details.SaleStatus, product.Details.MarketingRuleId);
                                            barcode.SaleNumber = num;
                                            barcode.MarketingPrice = barcode.SalePrice;
                                            barcode.Details.CollectionMarketingPrice = product.MarketingPrice;

                                            barcode.Details.Total = barcode.SalePrice * num;
                                            barcode.Details.SaleStatus = SaleStatus.Normal;
                                            barcode.MarketingMarks = new List<ActiveMarketingRule>();
                                            var index = shoppingCart.OrderList.IndexOf(product);
                                            shoppingCart.OrderList.Insert(index + 1, barcode);
                                            product.SaleNumber = context.MarketingRule.RuleNumber;

                                        }
                                        product.MarketingPrice = context.MarketingRule.MarketingAction.DiscountAmount / context.MarketingRule.RuleNumber;
                                        product.Details.CollectionMarketingPrice = product.MarketingPrice;
                                        product.Details.Total = context.MarketingRule.MarketingAction.DiscountAmount;
                                        product.Details.SaleStatus = SaleStatus.Promotion;
                                        product.Details.EnableEditNum = false;
                                    }
                                }
                                break;
                            case MarketingActionMode.RepetitionDiscountAmount:
                                if (context.MarketingRule.MeteringMode == MeteringMode.GuDingLiang)
                                {
                                    foreach (var product in context.MatchRanges)
                                    {
                                        var saleNum = product.SaleNumber;
                                        var num = saleNum % context.MarketingRule.RuleNumber;
                                        var index = shoppingCart.OrderList.IndexOf(product);
                                        product.SaleNumber = context.MarketingRule.RuleNumber;
                                        product.MarketingPrice = context.MarketingRule.MarketingAction.DiscountAmount / context.MarketingRule.RuleNumber;
                                        product.Details.CollectionMarketingPrice = product.MarketingPrice;
                                        product.Details.Total = product.MarketingPrice * product.SaleNumber;
                                        product.Details.SaleStatus = SaleStatus.Promotion;
                                        var saleNumber = saleNum - num - context.MarketingRule.RuleNumber;
                                        while (saleNumber > 0)
                                        {
                                            var barcode = BarcodeFactory.Factory(shoppingCart.MachineInformation.StoreId, shoppingCart.MachineInformation.MachineSn, shoppingCart.MachineInformation.CompanyId, product.MainBarcode, product.Details.SaleStatus, product.Details.MarketingRuleId);

                                            barcode.SaleNumber = context.MarketingRule.RuleNumber;
                                            saleNumber -= context.MarketingRule.RuleNumber;
                                            barcode.MarketingPrice = context.MarketingRule.MarketingAction.DiscountAmount / context.MarketingRule.RuleNumber;
                                            barcode.Details.CollectionMarketingPrice = barcode.MarketingPrice;
                                            barcode.Details.Total = barcode.MarketingPrice * barcode.SaleNumber;

                                            barcode.Details.SaleStatus = SaleStatus.Promotion;
                                            barcode.MarketingMarks = product.MarketingMarks;
                                            shoppingCart.OrderList.Insert(++index, barcode);
                                        }
                                        if (num > 0)
                                        {
                                            var barcode = BarcodeFactory.Factory(shoppingCart.MachineInformation.StoreId, shoppingCart.MachineInformation.MachineSn, shoppingCart.MachineInformation.CompanyId, product.MainBarcode, product.Details.SaleStatus, product.Details.MarketingRuleId);
                                            barcode.SaleNumber = num;
                                            barcode.MarketingPrice = barcode.SalePrice;
                                            barcode.Details.CollectionMarketingPrice = product.MarketingPrice;

                                            barcode.Details.Total = barcode.SalePrice * num;
                                            barcode.Details.SaleStatus = SaleStatus.Normal;
                                            barcode.MarketingMarks = new List<ActiveMarketingRule>();
                                            shoppingCart.OrderList.Insert(++index, barcode);
                                        }
                                    }
                                }
                                break;
                            case MarketingActionMode.Discount:
                                foreach (var product in context.MatchRanges)
                                {
                                    product.MarketingPrice = product.MarketingPrice * (context.MarketingRule.MarketingAction.Discount / 10);
                                    product.Details.CollectionMarketingPrice = product.MarketingPrice;
                                    product.Details.Total = product.MarketingPrice * product.SaleNumber;
                                    product.Details.SaleStatus = SaleStatus.Promotion;
                                }
                                break;
                            case MarketingActionMode.NowCash://组合或者 满元
                                if (context.MarketingRule.IsRepeatMarketing)
                                {
                                    decimal multiple = 1m;
                                    switch (context.MarketingRule.MeteringMode)
                                    {
                                        case MeteringMode.ManYuan:
                                            multiple = (int)(context.ProductTotal / context.MarketingRule.RuleNumber);
                                            manYuanMinus += context.MarketingRule.MarketingAction.Amount * multiple;
                                            break;
                                        case MeteringMode.QiGouLiang:
                                            multiple = (int)(context.ProductCount / context.MarketingRule.RuleNumber);
                                            zuHeMinus += context.MarketingRule.MarketingAction.Amount * multiple;
                                            break;
                                    }

                                    totalPreferential += context.MarketingRule.MarketingAction.Amount * multiple;

                                    foreach (var record in context.MatchRanges)
                                    {
                                        //record.Details.SaleStatus = SaleStatus.Promotion;
                                        record.Details.CollectionMarketingPrice -= context.MarketingRule.MarketingAction.Amount * multiple * (record.Details.CollectionMarketingPrice / context.ProductTotal);
                                    }
                                }
                                else
                                {
                                    totalPreferential += context.MarketingRule.MarketingAction.Amount;
                                    switch (context.MarketingRule.MeteringMode)
                                    {
                                        case MeteringMode.ManYuan:
                                            manYuanMinus += context.MarketingRule.MarketingAction.Amount;
                                            break;
                                        case MeteringMode.QiGouLiang:
                                            zuHeMinus += context.MarketingRule.MarketingAction.Amount;
                                            break;
                                    }
                                    foreach (var record in context.MatchRanges)
                                    {
                                        // record.Details.SaleStatus = SaleStatus.Promotion;
                                        record.Details.CollectionMarketingPrice -= context.MarketingRule.MarketingAction.Amount * (record.Details.CollectionMarketingPrice / context.ProductTotal);
                                    }
                                }
                                break;
                            case MarketingActionMode.Gift:
                                if (context != null && context.MarketingRule != null && context.MarketingRule.MarketingAction != null && context.MarketingRule.MarketingAction.Gifts != null)
                                {
                                    if (context.MarketingRule.MarketingAction.Gifts.Count() == 1)
                                    {
                                        decimal multiple = 1m;

                                        if (context.MarketingRule.IsRepeatMarketing)
                                        {
                                            switch (context.MarketingRule.MeteringMode)
                                            {
                                                case MeteringMode.ManYuan:
                                                    multiple = (int)(context.ProductTotal / context.MarketingRule.RuleNumber);
                                                    break;
                                                case MeteringMode.QiGouLiang:
                                                    multiple = (int)(context.ProductCount / context.MarketingRule.RuleNumber);
                                                    break;
                                            }
                                        }
                                        var saleStatus = SaleStatus.ActivityGifts;
                                        if (context.MarketingRule.MarketingAction.MarketingActionMode == MarketingActionMode.AddMoneyToGive)
                                            saleStatus = SaleStatus.ActivityAddMoneyGifts;

                                        var barcode = shoppingCart.OrderList.FirstOrDefault(o => o.Details.SaleStatus == saleStatus && o.Details.MarketingRuleId == context.MarketingRule.Id);


                                        var gift = context.MarketingRule.MarketingAction.Gifts.FirstOrDefault();
                                        var giftNumber = gift.Value * multiple;
                                        if (barcode != null)
                                        {
                                            barcode.Details.IsActivityGiftsTimeOut = false;
                                            barcode.EnableMarketing = false;
                                            if (barcode.SaleNumber != giftNumber)
                                            {
                                                barcode.SalePrice = context.MarketingRule.MarketingAction.AddMoney;

                                                barcode.MarketingPrice = context.MarketingRule.MarketingAction.AddMoney;
                                                barcode.Details.CollectionMarketingPrice = context.MarketingRule.MarketingAction.AddMoney;
                                                barcode.SaleNumber = giftNumber;
                                                barcode.Details.Total = barcode.MarketingPrice * barcode.SaleNumber; ;
                                                barcode.MarketingMarks = new List<ActiveMarketingRule>();
                                                barcode.EnableMarketing = false;
                                            }
                                        }
                                        else
                                        {
                                            barcode = BarcodeFactory.Factory(shoppingCart.MachineInformation.StoreId, shoppingCart.MachineInformation.MachineSn, shoppingCart.MachineInformation.CompanyId, gift.Key, saleStatus, context.MarketingRule.Id);
                                            barcode.SaleNumber = giftNumber;
                                            barcode.SalePrice = context.MarketingRule.MarketingAction.AddMoney;
                                            barcode.Details.IsActivityGiftsTimeOut = false;
                                            barcode.MarketingMarks = new List<ActiveMarketingRule>();
                                            barcode.EnableMarketing = false;

                                            barcode.MarketingPrice = context.MarketingRule.MarketingAction.AddMoney;
                                            barcode.Details.CollectionMarketingPrice = context.MarketingRule.MarketingAction.AddMoney;
                                            barcode.Details.Total = barcode.MarketingPrice * barcode.SaleNumber;
                                            shoppingCart.OrderList.Insert(shoppingCart.OrderList.Count, barcode);
                                        }

                                        barcode.EnableMarketing = false;


                                    }
                                }
                                break;


                            default:
                                //foreach (var record in context.MatchRanges)
                                //{
                                //    record.Details.SaleStatus = SaleStatus.Promotion;
                                //}
                                break;
                        }
                        result.Add(context);
                    }
                }
            }
        Return:
            outObj = result;
            return totalPreferential;
        }


    }
}
