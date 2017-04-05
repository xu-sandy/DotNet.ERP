﻿using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Logic.ApiData.Pos.Sale;
using Pharos.Logic.ApiData.Pos.Sale.Marketings;
using Pharos.Logic.ApiData.Pos.Services.LocalCeServices;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.ObjectModels.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Pharos.Logic.ApiData.Pos.DataAdapter
{
    /// <summary>
    /// 本地数据源适配器( for sql server Compact)
    /// </summary>
    public class SqlServerCEDataAdapter : IDataAdapter
    {
        public string StoreId { get; set; }
        public string DeviceSn { get; set; }

        public string MachineSN { get; set; }

        public int CompanyId { get; set; }
        /// <summary>
        /// 是否在练习模式下
        /// </summary>
        public bool IsSalesclerkTest { get; set; }


        public bool Enable
        {
            get { return SysStoreUserInfoService.CurrentRepository._context.Database.Exists(); }
        }


        public ValueObject.UserInfo GetUser(string account)
        {
            var result = SysStoreUserInfoService.GetStoreUserInfo(account, StoreId, CompanyId);
            return result;
        }

        public ProductInfo GetProductInfoByBarcode(string barcode, bool isFindWeigh = false)
        {
            return ProductRecordService.GetProductRecod(StoreId, barcode, CompanyId, isFindWeigh);
        }
        public ValueObject.PageResult<InventoryResult> CheckedInventory(IEnumerable<int> categorySns, string keyword, decimal price, int pageSize, int pageIndex)
        {
            //改为从产品档案来
            return ProductRecordService.CheckedInventory(StoreId, CompanyId, categorySns, keyword, price, pageSize, pageIndex);
        }

        public IEnumerable<ValueObject.UserInfo> GetStoreUsers(User.StoreOperateAuth storeOperateAuth)
        {
            return SysStoreUserInfoService.GetStoreManagers(StoreId, storeOperateAuth, CompanyId);
        }

        public ValueObject.MemberInfo GetMemberInfo(string phone, string cardNo, string uid)
        {
            return MembersService.GetMemberInfo(StoreId, CompanyId, phone, uid, cardNo);
        }

        public IEnumerable<CategoryDAO> GetStoreCategory()
        {
            return ProductCategorySerivce.GetStoreCategories(StoreId, CompanyId);
        }

        //public ValueObject.PageResult<InventoryResult> CheckedPrice(IEnumerable<int> categorySns, decimal from, decimal to, int pageSize, int pageIndex)
        //{
        //    //改为从产品档案来
        //    return ProductRecordService.CheckedPrice(StoreId, CompanyId, categorySns, from, to, pageSize, pageIndex);
        //}
        public ProductInfo GetProductInfoFromBundlingByBarcode(string barcode)
        {
            return BundlingService.GetProductInfoFromBundlingByBarcode(StoreId, barcode, CompanyId);
        }


        public void PosIncomePayout(string uid, decimal money, User.PosIncomePayoutMode mode)
        {
            PosIncomePayoutService.Save(StoreId, MachineSN, uid, money, mode, CompanyId, IsSalesclerkTest);
        }

        public ValueObject.BillHistoryInfo GetBillDetailsHistory(string paySn)
        {
            return SaleOrdersService.GetBillDetailsHistory(StoreId, MachineSN, paySn, CompanyId, IsSalesclerkTest);
        }

        public IEnumerable<ValueObject.ReasonItem> GetReason(int mode)
        {
            int psn = mode == 1 ? 7 : 8;
            var result = Pharos.Logic.BLL.BaseGeneralService<SysDataDictionary, LocalCeDbContext>.CurrentRepository.FindList(o => o.CompanyId == CompanyId && o.DicPSN == psn).Select(o => new ReasonItem()
            {
                DicSN = o.DicSN,
                Title = o.Title
            }).ToList();
            return result;
        }

        public DateTime RefundAll(int reason, string customOrderSn, decimal amount, string uid, string apiCodes, string newOrderSn, string newCustomOrderSn, DateTime createdt)
        {
            var order = SaleOrdersService.CurrentRepository.Entities.FirstOrDefault(o => o.CompanyId == CompanyId && o.CustomOrderSn == customOrderSn && o.StoreId == StoreId && o.State == 0);
            var version = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            var newOrder = new SaleOrders()
            {
                StoreId = order.StoreId,//门店id
                MachineSN = order.MachineSN,//pos机号
                PaySN = newOrderSn,//订单流水号
                CustomOrderSn = newCustomOrderSn,//订单流水号
                ReFundOrderCustomOrderSn = order.CustomOrderSn,//原订单流水号
                TotalAmount = -order.TotalAmount,//优惠后金额
                PreferentialPrice = order.PreferentialPrice,//优惠金额
                ApiCode = apiCodes,//支付方式  TODO:
                CreateUID = uid,//收银员工号
                ReturnOrderUID = uid,
                ReturnDT = createdt,
                CreateDT = createdt,
                CompanyId = order.CompanyId,
                ProductCount = -order.ProductCount,
                Reason = reason,
                Type = 3,
                State = 1,
                MemberId = order.MemberId,
                ActivityId = order.ActivityId,
                Salesman = order.Salesman,
                Receive = -order.Receive,
                IsTest = IsSalesclerkTest,
                InInventory = 0,
                IsProcess = false,
                OrderDiscount = order.OrderDiscount,
                SyncItemId = Guid.NewGuid(),
                SyncItemVersion = version
            };
            var saleOrdersDetails = SaleDetailService.FindList(o => o.PaySN == order.PaySN).ToList();
            foreach (var item in saleOrdersDetails)
            {
                SaleDetail _saleDetail = new SaleDetail()
                {
                    PaySN = newOrderSn,
                    ProductCode = item.ProductCode,
                    AveragePrice = item.AveragePrice,//均价
                    CompanyId = CompanyId,
                    Total = -item.Total,
                    Barcode = item.Barcode,//商品条码
                    PurchaseNumber = -item.PurchaseNumber,//销售数量
                    BuyPrice = item.BuyPrice, //系统进价
                    SysPrice = item.SysPrice,//系统售价
                    ActualPrice = item.ActualPrice,//销售价
                    Title = item.Title,
                    ScanBarcode = item.ScanBarcode,
                    SalesClassifyId = item.SalesClassifyId,//销售分类id
                    SyncItemId = Guid.NewGuid(),
                    SyncItemVersion = version
                };
                SaleDetailService.CurrentRepository.Add(_saleDetail, false);
            }
            SaleOrdersService.CurrentRepository.Add(newOrder);
            //RedisManager.Publish("SyncDatabase", "SalePackage");
            StoreManager.PubEvent("SyncDatabase", "SalePackage");
            return createdt;
        }
        public void RecordPayment(string paySn, decimal amount, int apiCode, decimal change, decimal receive, string apiOrderSN = null, string cardNo = null)
        {
            ConsumptionPaymentService.Save(paySn, amount, apiCode, change, receive, apiOrderSN, cardNo, CompanyId);
        }

        public ApiSetting GetApiSettings(int apiCode)
        {
            return ApiLibraryService.GetApiSettings(apiCode);
        }

        public IEnumerable<ValueObject.BillListItem> GetBills(DateTime date, Sale.Range range, string paySn, string cashier, string qmachineSn)
        {
            return SaleOrdersService.GetBills(StoreId, qmachineSn, date, CompanyId, range, IsSalesclerkTest, paySn, cashier);
        }

        public void RegisterDevice(string deviceSn, ValueObject.DeviceType type)
        {
            DeviceRegInfoService.RegisterDevice(StoreId, MachineSN, deviceSn, type, CompanyId);
        }

        public bool HasRegister(string deviceSn, ValueObject.DeviceType type, bool verfyState = true)
        {
            return DeviceRegInfoService.HasRegister(StoreId, MachineSN, deviceSn, type, CompanyId, verfyState);
        }

        public IEnumerable<ValueObject.Announcement> Announcements()
        {
            return NoticeService.Announcements(StoreId, MachineSN, CompanyId);
        }
        public IEnumerable<ValueObject.Activity> Activities()
        {
            return NoticeService.Activities(StoreId, MachineSN, CompanyId);
        }
        public DateTime ChangeOrRefund(Sale.AfterSale.OrderChangeRefundSale changeList, int reason, MachineInformation machineInformation, string newPaySn, decimal amount, decimal receive, string uid, string apiCodes, DateTime saveTime, string saleman, string oldOrderSn)
        {
            try
            {

                var oldOrder = (from a in SaleOrdersService.CurrentRepository.Entities.Where(o => o.StoreId == machineInformation.StoreId && o.CompanyId == machineInformation.CompanyId && o.CustomOrderSn == oldOrderSn)
                                join b in SaleDetailService.CurrentRepository.Entities
                                on a.PaySN equals b.PaySN
                                select new
                                {
                                    a,
                                    b
                                });

                var version = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

                SaleOrders _saleOrder = new SaleOrders()
                {
                    PaySN = changeList.PaySn,
                    CustomOrderSn = changeList.CustomOrderSn,
                    MachineSN = machineInformation.MachineSn,
                    StoreId = machineInformation.StoreId,
                    TotalAmount = amount,
                    Receive = receive,
                    PreferentialPrice = 0,
                    ApiCode = apiCodes,
                    CreateUID = machineInformation.CashierUid,
                    CreateDT = saveTime,
                    ReturnDT = saveTime,
                    CompanyId = machineInformation.CompanyId,
                    ProductCount = changeList.RecordCount,
                    Type = (short)changeList.Mode,
                    State = 0,
                    MemberId = null,
                    IsTest = IsSalesclerkTest,
                    InInventory = 0,
                    IsProcess = false,
                    Reason = reason,
                    SyncItemId = Guid.NewGuid(),
                    SyncItemVersion = version,
                    Salesman = saleman,
                    ReFundOrderCustomOrderSn = oldOrderSn
                };
                SaleOrdersService.CurrentRepository.Add(_saleOrder, false);
                foreach (var item in changeList.ChangingList)
                {
                    var buyPrice = 0m;
                    if (oldOrder != null)
                    {
                        foreach (var itm in oldOrder)
                        {
                            if ((short)changeList.Mode == 2)
                            {
                                if (itm.b.Barcode == item.CurrentBarcode.MainBarcode)
                                {
                                    buyPrice = itm.b.BuyPrice;
                                }
                            }
                            else
                            {
                                if (itm.b.PurchaseNumber > 0)
                                {
                                    var isWeight = false;
                                    if (itm.b.Barcode.Length == 13 && itm.b.ScanBarcode.Substring(0, 2) == "27")
                                    {
                                        isWeight = true;
                                    }
                                    var productInfo = GetProductInfoByBarcode(itm.b.ScanBarcode, isWeight);
                                    if (productInfo != null)
                                    {
                                        buyPrice = productInfo.BuyPrice;
                                    }
                                }
                                else
                                {
                                    if (itm.b.Barcode == item.CurrentBarcode.MainBarcode)
                                    {
                                        buyPrice = itm.b.BuyPrice;
                                    }
                                }
                            }
                        }
                    }
                    SaleDetail _saleDetail = new SaleDetail()
                    {
                        PaySN = changeList.PaySn,
                        ScanBarcode = item.CurrentBarcode.CurrentString,
                        ProductCode = item.CurrentBarcode.ProductCode,
                        AveragePrice = item.ChangePrice,//均价
                        CompanyId = machineInformation.CompanyId,
                        Total = item.ChangePrice * item.ChangeNumber,
                        Barcode = item.CurrentBarcode.MainBarcode,
                        PurchaseNumber = item.ChangeNumber,
                        BuyPrice = buyPrice,//进价
                        SysPrice = item.SysPrice,
                        ActualPrice = item.ChangePrice,
                        SalesClassifyId = (int)item.SaleStatus,
                        Title = item.CurrentBarcode.Details.Title,
                        SyncItemId = Guid.NewGuid(),
                        SyncItemVersion = version
                    };
                    SaleDetailService.CurrentRepository.Add(_saleDetail, false);
                }
                SaleOrdersService.CurrentRepository.Update(_saleOrder);
                //RedisManager.Publish("SyncDatabase", "SalePackage");
                StoreManager.PubEvent("SyncDatabase", "SalePackage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return saveTime;
        }



        public IEnumerable<KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>> GetMarketingRules()
        {
            List<KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>> result = new List<KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>>();
            result = GetDiscountForMarketing(result);
            result = GetFreeGiftPurchaseForMarketing(result);
            result = GetCollectionPromotionForMarketing(result);

            return result;
        }

        public decimal GetMarketingRecordNumber(string marketingId, MarketingQuotaMode mode)
        {
            var result = 0m;
            try
            {
                switch (mode)
                {
                    case MarketingQuotaMode.EveryDay:
                        var start = DateTime.Now.Date;
                        result = MarketingRecordService.CurrentRepository.Entities.Where(o => o.StoreId == StoreId && o.CompanyId == CompanyId && o.MarketingId == marketingId && o.CreateDT >= start).Sum(o => o.Number);
                        break;
                    case MarketingQuotaMode.TotalQuota:
                        result = MarketingRecordService.CurrentRepository.Entities.Where(o => o.StoreId == StoreId && o.CompanyId == CompanyId && o.MarketingId == marketingId).Sum(o => o.Number);
                        break;
                }
            }
            catch
            {
            }
            return result;
        }

        public void SaveMarketingRecord(string marketingId, decimal number)
        {
            var version = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

            MarketingRecordService.CurrentRepository.Add(new SalesRecord()
            {
                MarketingId = marketingId,
                CompanyId = CompanyId,
                CreateDT = DateTime.Now,
                Number = number,
                StoreId = StoreId,
                SyncItemId = Guid.NewGuid(),
                SyncItemVersion = version
            });
            //RedisManager.Publish("SyncDatabase", "SalesRecord");
            StoreManager.PubEvent("SyncDatabase", "SalesRecord");
        }

        public void SaveOrder(Sale.ShoppingCart shoppingCart, string apiCodes, DateTime saveTime)
        {
            try
            {

                //SaleOrders SaleDetail SaleDetailsTotal 
                var saleOrders = shoppingCart.GetSaleStatistics();//销售清单
                var version = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                SaleOrders _saleOrder = new SaleOrders()
                {
                    StoreId = shoppingCart.MachineInformation.StoreId,//门店id
                    MachineSN = shoppingCart.MachineInformation.MachineSn,//pos机号
                    PaySN = shoppingCart.OrderSN,//订单流水号
                    CustomOrderSn = shoppingCart.CustomOrderSN,//订单流水号
                    TotalAmount = saleOrders.Receivable,//优惠后金额
                    PreferentialPrice = saleOrders.Preferential,//优惠金额
                    ApiCode = apiCodes,//支付方式  TODO:
                    CreateUID = shoppingCart.MachineInformation.CashierUid,//收银员工号
                    CreateDT = saveTime,
                    CompanyId = CompanyId,
                    ProductCount = saleOrders.Num,
                    Type = 0,
                    State = 0,
                    MemberId = shoppingCart.MemberId,
                    ActivityId = shoppingCart.ActivityId,
                    Salesman = shoppingCart.SaleMan,
                    Receive = shoppingCart.WipeZeroAfter,
                    IsTest = IsSalesclerkTest,
                    InInventory = 0,
                    IsProcess = false,
                    OrderDiscount = shoppingCart.OrderDiscount,
                    SyncItemId = Guid.NewGuid(),
                    SyncItemVersion = version
                };
                SaleOrdersService.CurrentRepository.Add(_saleOrder, false);

                var saleOrdersDetails = shoppingCart.GetOrdeList();//订单信息
                foreach (var item in saleOrdersDetails)
                {
                    SaleDetail _saleDetail = new SaleDetail()
                    {
                        PaySN = _saleOrder.PaySN,
                        ProductCode = item.ProductCode,
                        AveragePrice = item.AveragePrice,//均价
                        CompanyId = CompanyId,
                        Total = item.Details.Total,
                        Barcode = item.MainBarcode,//商品条码
                        PurchaseNumber = item.SaleNumber,//销售数量
                        BuyPrice = item.Details.BuyPrice, //系统进价
                        SysPrice = item.Details.SystemPrice,//系统售价
                        ActualPrice = item.MarketingPrice,//销售价
                        Title = item.Details.Title,
                        ScanBarcode = item.CurrentString,
                        SalesClassifyId = (int)item.Details.SaleStatus,//销售分类id
                        SyncItemId = Guid.NewGuid(),
                        SyncItemVersion = version
                    };
                    SaleDetailService.CurrentRepository.Add(_saleDetail, false);
                }
                SaleOrdersService.CurrentRepository.Update(_saleOrder);
                //  RedisManager.Publish("SyncDatabase", "SalePackage");
                StoreManager.PubEvent("SyncDatabase", "SalePackage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ValueObject.DayReportResult DayMonthReport(DateTime from, DateTime to, Sale.Range range)
        {
            DayReportResult result = new DayReportResult();
            result.Summary = new List<DayReportDetailItem>();
            result.SalesmanRecords = new List<SalesmanDayReportResult>();
            var machineParams = range == Range.Local ? MachineSN : "";
            var users = SaleOrdersService.DayMonthReport(from, to, ref result, StoreId, machineParams, CompanyId, IsSalesclerkTest);
            SaleDetailService.DayMonthReport(from, to, ref result, StoreId, machineParams, CompanyId, IsSalesclerkTest, users);
            PosIncomePayoutService.DayMonthReport(from, to, ref result, StoreId, machineParams, CompanyId, IsSalesclerkTest);
            return result;
        }




        public void AddWipeZero(int companyId, string paySn, decimal money)
        {
            WipeZero _wipe = new WipeZero()
            {
                CompanyId = companyId,
                PaySN = paySn,
                Number = money,
                SyncItemId = Guid.NewGuid(),
                SyncItemVersion = BitConverter.GetBytes((long)1)
            };
            WipeZeroService.CurrentRepository.Add(_wipe);
        }


        /// <summary>
        /// 获取单品折扣信息
        /// </summary>
        /// <param name="collections"></param>
        /// <returns></returns>
        private List<KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>> GetDiscountForMarketing(List<KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>> collections)
        {
            //Discount 不匹配称重商品
            var today = DateTime.Now.Date;
            var discountQuery = (from a in CommodityDiscountService.CurrentRepository.Entities
                                 from b in CommodityPromotionService.CurrentRepository.Entities
                                 where
                                 a.CommodityId == b.Id && b.State != 2
                                 && b.EndDate >= today
                                 && b.PromotionType == 1
                                 select new { a, b }).ToList();
            foreach (var item in discountQuery)
            {
                var marketingTimelinessLimit = GetMarketingTimelinessLimit(item.b);
                var marketingRules = GetMarketingRule(item.b);
                marketingRules.Type = MarketingType.Danpinzhekou;
                var barcodeRange = new List<string>();
                if (!string.IsNullOrEmpty(item.a.Barcode))
                {
                    barcodeRange.Add(item.a.Barcode);
                }
                else if (item.a.CategorySN != -1)
                {
                    var ranges = GetProductRanges(item.a.CategorySN, item.a.CategoryGrade ?? 0);
                    barcodeRange.AddRange(ranges);
                }
                else
                {
                    goto Continue;
                }
                marketingRules.BarcodeRange = barcodeRange;
                marketingRules.RuleNumber = item.a.MinPurchaseNum;
                marketingRules.MarketingAction = new MarketingAction()
                {
                    AddMoney = 0m,
                    Discount = item.a.DiscountRate.GetValueOrDefault(),
                    DiscountAmount = item.a.DiscountPrice ?? 0m,
                    Gifts = null,
                    MarketingActionNumber = 0m,
                    Repeatable = true
                };
                switch (item.a.Way)
                {
                    case 1:
                        marketingRules.MeteringMode = MeteringMode.GuDingLiang;
                        marketingRules.MarketingAction.MarketingActionMode = MarketingActionMode.RepetitionDiscountAmount;
                        break;
                    case 2:
                        marketingRules.MeteringMode = MeteringMode.QiGouLiang;
                        marketingRules.MarketingAction.MarketingActionMode = MarketingActionMode.Discount;
                        break;
                    case 3:
                        marketingRules.MeteringMode = MeteringMode.GuDingLiang;
                        marketingRules.MarketingAction.MarketingActionMode = MarketingActionMode.DiscountAmount;
                        break;
                    default:
                        goto Continue;
                }
                collections.Add(new KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>(marketingTimelinessLimit, marketingRules));
            Continue:
                continue;
            }
            return collections;
        }
        /// <summary>
        /// 获取时效信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private MarketingTimelinessLimit GetMarketingTimelinessLimit(CommodityPromotion entity)
        {
            //时效限制参数
            var timeRanges = new List<KeyValuePair<string, string>>();
            if (entity.Timeliness == 1)
            {
                timeRanges = ParseTimeRange(timeRanges, entity.StartAging1, entity.EndAging1);
                timeRanges = ParseTimeRange(timeRanges, entity.StartAging2, entity.EndAging2);
                timeRanges = ParseTimeRange(timeRanges, entity.StartAging3, entity.EndAging3);
            }
            var timelinessLimit = new MarketingTimelinessLimit()
            {
                StartTime = entity.StartDate ?? default(DateTime),
                OverTime = (entity.EndDate ?? default(DateTime)).AddDays(1),
                TimeRanges = timeRanges
            };
            return timelinessLimit;
        }
        /// <summary>
        /// 解析验证时间范围
        /// </summary>
        /// <param name="timeRanges"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> ParseTimeRange(List<KeyValuePair<string, string>> timeRanges, string startTime, string endTime)
        {

            if (ParseTimeRange(startTime, endTime))
            {
                timeRanges.Add(new KeyValuePair<string, string>(startTime, endTime));
            }
            return timeRanges;
        }
        /// <summary>
        /// 解析时间范围
        /// </summary>
        /// <param name="timeRanges"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private bool ParseTimeRange(string startTime, string endTime)
        {
            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                try
                {
                    var start = DateTime.Parse(startTime);
                    var end = DateTime.Parse(endTime);
                    return true;
                }
                catch (Exception ex)
                {
                }
            }
            return false;

        }

        /// <summary>
        /// 买赠促销信息
        /// </summary>
        /// <param name="collections"></param>
        /// <returns></returns>
        private List<KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>> GetFreeGiftPurchaseForMarketing(List<KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>> collections)
        {
            var today = DateTime.Now.Date;
            var discountQuery = (from a in FreeGiftPurchaseService.CurrentRepository.Entities
                                 from b in CommodityPromotionService.CurrentRepository.Entities
                                 where
                                 a.CommodityId == b.Id
                                 && b.State != 2
                                 && b.EndDate >= today
                                 && b.PromotionType == 4
                                 select new { a, b }).ToList();
            foreach (var item in discountQuery)
            {
                var marketingTimelinessLimit = GetMarketingTimelinessLimit(item.b);
                var marketingRules = GetMarketingRule(item.b);
                marketingRules.Type = MarketingType.Maizeng;
                marketingRules.Id = item.a.GiftId;
                marketingRules.RestrictionBuyCount = item.a.RestrictionBuyNum;
                marketingRules.MarketingQuotaMode = item.a.RestrictionBuyNum == 0 ? MarketingQuotaMode.NotLimit : MarketingQuotaMode.EveryDay;
                var barcodeRange = new List<string>();
                if (item.a.GiftType == 1 && !string.IsNullOrEmpty(item.a.BarcodeOrCategorySN))
                {
                    barcodeRange.Add(item.a.BarcodeOrCategorySN);
                }
                else if (item.a.GiftType == 2 && !string.IsNullOrEmpty(item.a.BarcodeOrCategorySN))
                {
                    try
                    {
                        var categorySN = int.Parse(item.a.BarcodeOrCategorySN);
                        var ranges = GetProductRanges(categorySN, item.a.CategoryGrade ?? 0);
                        barcodeRange.AddRange(ranges);
                    }
                    catch
                    {
                        goto Continue;
                    }
                }
                else
                {
                    goto Continue;
                }
                marketingRules.BarcodeRange = barcodeRange;
                marketingRules.MeteringMode = MeteringMode.QiGouLiang;
                marketingRules.RuleNumber = item.a.MinPurchaseNum;


                var gifts = FreeGiftPurchaseListService.CurrentRepository.Entities.Where(o => o.GiftId == item.a.GiftId).ToList();
                var giftRanges = new List<KeyValuePair<string, decimal>>();
                foreach (var gift in gifts)
                {
                    if (string.IsNullOrEmpty(gift.BarcodeOrCategorySN))
                    {
                        goto Continue;
                    }
                    switch (gift.GiftType)
                    {
                        case 1:
                            giftRanges.Add(new KeyValuePair<string, decimal>(gift.BarcodeOrCategorySN, gift.GiftNumber));
                            break;
                        case 2:
                            try
                            {
                                var categorySN = int.Parse(gift.BarcodeOrCategorySN);
                                var ranges = GetProductRanges(categorySN, gift.CategoryGrade ?? 0).Select(o => new KeyValuePair<string, decimal>(o, gift.GiftNumber));
                                giftRanges.AddRange(ranges);
                            }
                            catch
                            {
                                goto Continue;
                            }
                            break;
                    }
                }
                marketingRules.MarketingAction = new MarketingAction()
                {
                    AddMoney = 0m,
                    Discount = 0m,
                    MarketingActionMode = MarketingActionMode.Gift,
                    Gifts = giftRanges,
                    DiscountAmount = 0m,
                    MarketingActionNumber = item.a.RestrictionBuyNum,
                    Repeatable = false
                };


                collections.Add(new KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>(marketingTimelinessLimit, marketingRules));
            Continue:
                continue;
            }
            return collections;
        }
        /// <summary>
        /// 获取促销规则的主表信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ActiveMarketingRule GetMarketingRule(CommodityPromotion entity)
        {
            var marketingRules = new ActiveMarketingRule()
            {
                Id = entity.Id,
                RestrictionBuyCount = entity.RestrictionBuyNum,
                MarketingQuotaMode = entity.RestrictionBuyNum == 0 ? MarketingQuotaMode.NotLimit : MarketingQuotaMode.EveryDay,
                CreateRuleDate = entity.CreateDT,
                Enable = true,
                IsRepeatMarketing = false
            };
            switch (entity.CustomerObj)
            {
                case 0:
                    marketingRules.CustomerType = Pharos.ObjectModels.DTOs.CustomerType.Whole;
                    break;
                case 1:
                    marketingRules.CustomerType = Pharos.ObjectModels.DTOs.CustomerType.Insider;
                    break;
                case 2:
                    marketingRules.CustomerType = Pharos.ObjectModels.DTOs.CustomerType.VIP;
                    break;
                default:
                    marketingRules.CustomerType = Pharos.ObjectModels.DTOs.CustomerType.Whole;
                    break;
            }
            return marketingRules;
        }

        public IEnumerable<string> GetProductRanges(int category, int depth)
        {
            var lastDepthStoreCategories = ProductCategorySerivce.GetLastDepthStoreCategories(StoreId, category, depth, CompanyId);
            var categories = lastDepthStoreCategories.Select(o => o.CategorySN).ToList();
            var ranges = ProductRecordService.CurrentRepository.Entities.Where(o => o.CompanyId == CompanyId && categories.Contains(o.CategorySN)).Select(o => o.Barcode).ToList();
            return ranges;
        }
        public IEnumerable<string> GetProductRanges(int category, int depth, int brandSn)
        {
            var lastDepthStoreCategories = ProductCategorySerivce.GetLastDepthStoreCategories(StoreId, category, depth, CompanyId);
            var categories = lastDepthStoreCategories.Select(o => o.CategorySN).ToList();
            var ranges = ProductRecordService.CurrentRepository.Entities.Where(o => o.CompanyId == CompanyId && categories.Contains(o.CategorySN) && o.BrandSN == brandSn).Select(o => o.Barcode).ToList();
            return ranges;
        }
        /// <summary>
        /// 集合促销（即 满元、组合）
        /// </summary>
        /// <param name="collections"></param>
        /// <returns></returns>
        private List<KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>> GetCollectionPromotionForMarketing(List<KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>> collections)
        {

            var today = DateTime.Now.Date;
            var discountQuery = (from a in PromotionBlendService.CurrentRepository.Entities
                                 from b in CommodityPromotionService.CurrentRepository.Entities
                                 where
                                 a.CommodityId == b.Id
                                 && b.State != 2
                                 && b.EndDate >= today
                                 && (b.PromotionType == 3 || b.PromotionType == 5)
                                 select new { a, b }).ToList();
            foreach (var item in discountQuery)
            {
                try
                {
                    var marketingTimelinessLimit = GetMarketingTimelinessLimit(item.b);
                    var marketingRules = GetMarketingRule(item.b);
                    marketingRules.IsRepeatMarketing = Convert.ToBoolean(item.a.AllowedAccumulate);
                    marketingRules.Type = item.a.RuleType == 1 ? MarketingType.Zuhe : MarketingType.Manyuan;
                    var promotionBlendList = PromotionBlendListService.CurrentRepository.Entities.Where(o => o.CommodityId == item.a.CommodityId).ToList();
                    var barcodeRanges = new List<string>();
                    var giftRanges = new List<KeyValuePair<string, decimal>>();
                    var ignoreBarcodeRanges = new List<string>();
                    foreach (var child in promotionBlendList)
                    {
                        switch (child.BlendType)
                        {
                            case 1:
                                barcodeRanges.Add(child.BarcodeOrCategorySN);
                                break;
                            case 2:
                                if (child.BrandSN != 0)
                                {
                                    var productList = GetProductRanges(Convert.ToInt32(child.BarcodeOrCategorySN), child.CategoryGrade ?? 3, child.BrandSN);
                                    barcodeRanges.AddRange(productList);
                                }
                                else
                                {
                                    var productList = GetProductRanges(Convert.ToInt32(child.BarcodeOrCategorySN), child.CategoryGrade ?? 3);
                                    barcodeRanges.AddRange(productList);
                                }
                                break;
                            case 3:
                                giftRanges.Add(new KeyValuePair<string, decimal>(child.BarcodeOrCategorySN, 1));
                                break;
                            case 4:
                                if (child.BrandSN != 0)
                                {
                                    var productList = GetProductRanges(Convert.ToInt32(child.BarcodeOrCategorySN), child.CategoryGrade ?? 3, child.BrandSN);
                                    giftRanges.AddRange(productList.Select(o => new KeyValuePair<string, decimal>(o, 1)).ToList());
                                }
                                else
                                {
                                    var productList = GetProductRanges(Convert.ToInt32(child.BarcodeOrCategorySN), child.CategoryGrade ?? 3);
                                    giftRanges.AddRange(productList.Select(o => new KeyValuePair<string, decimal>(o, 1)).ToList());
                                }
                                break;
                            case 5:
                                ignoreBarcodeRanges.Add(child.BarcodeOrCategorySN);
                                break;
                            case 6:
                                if (child.BrandSN != 0)
                                {
                                    var productList = GetProductRanges(Convert.ToInt32(child.BarcodeOrCategorySN), child.CategoryGrade ?? 3, child.BrandSN);
                                    ignoreBarcodeRanges.AddRange(productList);
                                }
                                else
                                {
                                    var productList = GetProductRanges(Convert.ToInt32(child.BarcodeOrCategorySN), child.CategoryGrade ?? 3);
                                    ignoreBarcodeRanges.AddRange(productList);
                                }
                                break;

                        }
                    }
                    marketingRules.BarcodeRange = barcodeRanges;
                    marketingRules.IgnoreBarcodeRange = ignoreBarcodeRanges;
                    marketingRules.RuleNumber = item.a.FullNumber;
                    marketingRules.MarketingAction = new MarketingAction()
                    {
                        AddMoney = 0m,
                        Discount = 0m,
                        MarketingActionMode = MarketingActionMode.Gift,
                        DiscountAmount = 0m,
                        MarketingActionNumber = 0m,
                        Repeatable = Convert.ToBoolean(item.a.AllowedAccumulate)
                    };
                    switch (item.a.PromotionType)
                    {
                        case 1:
                            marketingRules.MarketingAction.MarketingActionMode = MarketingActionMode.NowCash;
                            marketingRules.MarketingAction.Amount = item.a.DiscountOrPrice;

                            break;
                        case 2:
                            marketingRules.MarketingAction.MarketingActionMode = MarketingActionMode.CashCoupon;
                            marketingRules.MarketingAction.Amount = item.a.DiscountOrPrice;
                            break;
                        case 3:
                            marketingRules.MarketingAction.MarketingActionMode = MarketingActionMode.Discount;
                            marketingRules.MarketingAction.Discount = item.a.DiscountOrPrice;
                            break;
                        case 4:
                            marketingRules.MarketingAction.MarketingActionMode = MarketingActionMode.AddMoneyToGive;
                            break;
                        case 5:
                            marketingRules.MarketingAction.MarketingActionMode = MarketingActionMode.AddMoneyToGive;
                            var products = ProductRecordService.CheckedPrice(StoreId, CompanyId, null, item.a.PriceRange, 0);
                            var ranges = products.Select(o => o.Barcode).Distinct().Select(o => new KeyValuePair<string, decimal>(o, 1)).ToList();
                            marketingRules.MarketingAction.Gifts = ranges;
                            break;
                    }



                    switch (item.a.RuleType)
                    {
                        case 1:
                            marketingRules.MeteringMode = MeteringMode.QiGouLiang;
                            break;
                        case 2:
                            marketingRules.MeteringMode = MeteringMode.ManYuan;
                            break;
                    }


                    collections.Add(new KeyValuePair<MarketingTimelinessLimit, ActiveMarketingRule>(marketingTimelinessLimit, marketingRules));
                }
                catch (Exception ex)
                {
                    goto Continue;
                }
            Continue:
                continue;
            }
            return collections;
        }
        public IEnumerable<WarehouseInformations> GetWarehouseInformations()
        {
            return new WarehouseInformations[1] { new WarehouseInformations() { CompanyId = Convert.ToInt32(ConfigurationManager.AppSettings["CompanyId"]), StoreId = ConfigurationManager.AppSettings["StoreId"] } };
        }

        public string GetStoreName()
        {
            throw new PosException("500", string.Format("请确认是否处于远程模式，并重新设置!"));
            //   return ConfigurationManager.AppSettings["StoreName"];
        }

        public ChangeOrRefundReturnOrderData IsHasCustomerOrderSn(string customerOrderSn)
        {
            ChangeOrRefundReturnOrderData _data = new ChangeOrRefundReturnOrderData();
            //Pharos.Logic.ApiData.Pos.ValueObject
            //Pharos.Logic.ApiData.Pos.ValueObject
            SaleManInfo _saleman = new SaleManInfo();
            List<ChangingList> _datalist = new List<ChangingList>();
            _data.OldOrderList = _datalist;
            _data.SaleMan = _saleman;
            var order = SaleOrdersService.CurrentRepository.Entities.Where(o => o.CustomOrderSn == customerOrderSn && o.CompanyId == CompanyId && o.StoreId == StoreId).FirstOrDefault();
            if (order != null)
            {
                var detail = SaleDetailService.CurrentRepository.Entities.Where(o => o.PaySN == order.PaySN).ToList();
                if (!string.IsNullOrEmpty(order.Salesman))
                {
                    var saleman = SysStoreUserInfoService.CurrentRepository.Entities.Where(o => o.UID == order.Salesman).FirstOrDefault();
                    _saleman.SaleManCode = saleman.UserCode;
                    _saleman.SaleManName = saleman.FullName;
                }
                foreach (var item in detail)
                {
                    ChangingList _list = new ChangingList();
                    _list.Barcode = item.ScanBarcode;
                    _list.ChangeNumber = item.PurchaseNumber;
                    _data.OldOrderList.Add(_list);
                }
            }
            return _data;
        }
        public IEnumerable<object> GetAreas(int pid)
        {
            var items = AreaService.CurrentRepository.Entities.Where(o => o.AreaPID == pid).ToList();
            return items;
        }
        public void AddMember(MemberDto member)
        {
            var info = Pharos.Logic.ApiData.Pos.User.Salesclerk.GetMachineInfo(StoreId, MachineSN, CompanyId, DeviceSn);
            if (info == null)
            {
                throw new PosException("无法获取操作员信息！");
            }
            var members = MembersService.CurrentRepository.FindList(o => o.Email == member.Email || o.Weixin == member.Weixin || o.MobilePhone == member.MobilePhone || o.Zhifubao == member.Zhifubao).ToList();
            if (members.Count > 0)
            {
                string exitItems = string.Empty;
                if (members.Exists(o => o.MobilePhone == member.MobilePhone))
                    exitItems += string.Format("手机号【{0}】已存在.", member.MobilePhone);
                if (members.Exists(o => o.Weixin == member.Weixin) && !string.IsNullOrEmpty(member.Weixin))
                    exitItems += string.Format("微信号【{0}】已存在.", member.Weixin);
                if (members.Exists(o => o.Email == member.Email) && !string.IsNullOrEmpty(member.Email))
                    exitItems += string.Format("Email【{0}】已存在.", member.Email);
                if (members.Exists(o => o.Zhifubao == member.Zhifubao) && !string.IsNullOrEmpty(member.Zhifubao))
                    exitItems += string.Format("支付宝【{0}】已存在.", member.Zhifubao);
                throw new PosException(string.Format("会员已存在，{0}", exitItems));
            }


            var version = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

            MembersService.CurrentRepository.Add(new Members()
            {
                Address = member.Address,
                Birthday = member.Birthday,
                CompanyId = CompanyId,
                CreateDT = DateTime.Now,
                CreateUID = info.CashierUid,
                CurrentCityId = member.CurrentCityId,
                Email = member.Email,
                MemberNo = member.MemberNo,
                Weixin = member.Weixin,
                Zhifubao = member.Zhifubao,
                MobilePhone = member.MobilePhone,
                StoreId = StoreId,
                Status = 1,
                MemberId = Guid.NewGuid().ToString("N"),
                RealName = member.RealName,
                Sex = member.Sex ? (short)1 : (short)0,
                CurrentCountyId = member.CurrentCountyId,
                CurrentProvinceId = member.ProvinceID,
                SyncItemId = Guid.NewGuid(),
                SyncItemVersion = version
            });
            StoreManager.PubEvent("SyncDatabase", "Member");
        }


        public Logic.Entity.MembershipCard GetStoredValueCardInfo(string cardNo)
        {
            throw new NotImplementedException();
        }

        public void SaveToStoredValueCard(string cardNo, decimal amount, decimal balance)
        {
            throw new NotImplementedException();
        }
    }
}
