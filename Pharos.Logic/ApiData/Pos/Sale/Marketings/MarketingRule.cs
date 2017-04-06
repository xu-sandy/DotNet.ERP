﻿using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.Sale.Barcodes;
using Pharos.Logic.ApiData.Pos.Sale.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.ObjectModels.DTOs;

namespace Pharos.Logic.ApiData.Pos.Sale.Marketings
{
    /// <summary>
    /// 促销活动限制规则
    /// </summary>
    public partial class ActiveMarketingRule : Pharos.ObjectModels.DTOs.MarketingRule
    {

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="shoppingCart">购物车</param>
        /// <returns>促销活动上下文</returns>
        internal MarketingContext Match(ShoppingCart shoppingCart, string storeId)
        {
            MarketingContext result = MarketingContext.Empty;
            this.IsTimeOut = false;

            //判断规则是否可用
            if (!Enable)
            {
                goto Return;
            }


            var defualtDataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, shoppingCart.MachineInformation.StoreId, shoppingCart.MachineInformation.MachineSn, shoppingCart.MachineInformation.CompanyId, DataAdapterFactory.DEFUALT);
            //处理限购量
            if (MarketingQuotaMode != MarketingQuotaMode.NotLimit)
            {
                var soldNumber = defualtDataAdapter.GetMarketingRecordNumber(Id, MarketingQuotaMode);
                var enableContinue = RestrictionBuyCount > soldNumber;
                if (!enableContinue)
                {
                    goto Return;
                }
            }
            var memberType = shoppingCart.MemberInfo == null ? Pharos.ObjectModels.DTOs.CustomerType.Whole : shoppingCart.MemberInfo.Type;
            //处理会员、内部员工、普通消费
            switch (CustomerType)
            {
                case Pharos.ObjectModels.DTOs.CustomerType.Whole:
                    break;
                case Pharos.ObjectModels.DTOs.CustomerType.Insider:
                    if (CustomerType != memberType)
                    {
                        goto Return;
                    }
                    break;
                case Pharos.ObjectModels.DTOs.CustomerType.VIP:
                    if (memberType == Pharos.ObjectModels.DTOs.CustomerType.VIP || memberType == Pharos.ObjectModels.DTOs.CustomerType.Insider)
                    {
                        goto Return;
                    }
                    break;
                default:
                    goto Return;
            }



            //处理默认值
            if (BarcodeRange == null)
            {
                BarcodeRange = new List<string>();
            }
            if (IgnoreBarcodeRange == null)
            {
                IgnoreBarcodeRange = new List<string>();
            }

            //配购物车
            var matchRanges = new List<IBarcode>();
            var orderList = shoppingCart.OrderList;
            foreach (var barcode in orderList)
            {

                if (
                    barcode.EnableMarketing &&
                    (
                        BarcodeRange.Contains(barcode.MainBarcode)
                        || (BarcodeRange.Count() == 0 && IgnoreBarcodeRange.Count() > 0 && MeteringMode != MeteringMode.ManYuan)
                        || MeteringMode == MeteringMode.ManYuan
                    )
                    && !IgnoreBarcodeRange.Contains(barcode.MainBarcode)
                    && Filter(barcode)
                   )
                {
                    matchRanges.Add(barcode);
                }
            }



            //匹配数量
            var totalCount = matchRanges.Sum(o => o.SaleNumber);
            var total = matchRanges.Sum(o => o.SaleNumber * o.MarketingPrice);
            var enableMarketing = false;

            switch (MeteringMode)
            {
                case Pharos.ObjectModels.DTOs.MeteringMode.GuDingLiang://固定量
                    enableMarketing = RuleNumber <= totalCount;
                    break;
                case Pharos.ObjectModels.DTOs.MeteringMode.QiGouLiang://起购量
                    enableMarketing = RuleNumber <= totalCount;
                    break;
                case Pharos.ObjectModels.DTOs.MeteringMode.ManYuan://满元
                    enableMarketing = RuleNumber <= total;
                    break;
            }
            if (MarketingType.Danpinzhekou == this.Type && enableMarketing)//单品折扣 以规则数量最大者为最优，起购量与固定量为互斥条件，不能同时作用在一个商品上
            {
                foreach (var match in matchRanges)
                {
                    foreach (var marketing in match.MarketingMarks)
                    {
                        if (marketing.Type == this.Type && marketing.Id != this.Id)
                        {
                            enableMarketing = marketing.RuleNumber < this.RuleNumber;//固定量促销始终只能匹配一个促销
                            if (enableMarketing)
                            {
                                marketing.IsTimeOut = true;
                            }
                            break;
                        }
                    }
                }
            }


            if (enableMarketing)//返回初始匹配结果
            {
                result = new MarketingContext();
                result.MarketingRule = this;
                result.MatchRanges = matchRanges;
                result.ProductCount = totalCount;
                result.ProductTotal = total;
                foreach (var match in matchRanges) //记录已匹配的促销
                {
                    match.MarketingMarks.RemoveAll(o => o.IsTimeOut);
                    match.MarketingMarks.Add(this);
                }
            }
        Return:
            return result;
        }
        /// <summary>
        /// 过滤已优惠的条码
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        internal bool Filter(IBarcode barcode)
        {
            return !barcode.MarketingMarks.Exists(o => o.Type == this.Type);
        }

        internal void SetMarketingRecordNumber(string storeId, string machineSn, int companyId)
        {
            if (MarketingQuotaMode != Pharos.ObjectModels.DTOs.MarketingQuotaMode.NotLimit)
            {
                var defualtDataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, DataAdapterFactory.DEFUALT);
                defualtDataAdapter.SaveMarketingRecord(Id, 1m);
            }
        }
    }
}