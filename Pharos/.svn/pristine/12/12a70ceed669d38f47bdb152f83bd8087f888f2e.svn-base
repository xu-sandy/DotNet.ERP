using Pharos.Logic.ApiData.Pos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.ApiData.Pos.Cache;
using Pharos.Logic.ApiData.Pos.Sale;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.ObjectModels.DTOs;
using Pharos.Infrastructure.Data.Normalize;
using Pharos.Logic.Entity;

namespace Pharos.Logic.ApiData.Pos.DataAdapter
{
    class MemoryCacheDataAdapter : IDataAdapter
    {
        public string StoreId { get; set; }

        public string MachineSN { get; set; }
        public string DeviceSn { get; set; }

        public int CompanyId { get; set; }

        public bool IsSalesclerkTest { get; set; }
        public bool Enable
        {
            get { return true; }
        }
        /// <summary>
        /// 根据条码查找商品信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public ProductInfo GetProductInfoByBarcode(string barcode, bool isFindWeigh = false)
        {
            var result = default(ProductInfo);
            //先从内存中查找数据
            string key = KeyFactory.ProductKeyFactory(CompanyId, StoreId, barcode);
            if (DataAdapterFactory.ProductCache.ContainsKey(key))
            {
                result = DataAdapterFactory.ProductCache.Get(key);

                //缓存中存在该数据，直接返回
                return result;
            }
            else
            {
                //缓存中不存在符合条件的数据从数据库中查找
                var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
                result = dataAdapter.GetProductInfoByBarcode(barcode, isFindWeigh);
                //加到缓存中
                if (result != null)
                    DataAdapterFactory.ProductCache.Set(key, result);


                return result;
            }
        }

        public ProductInfo GetProductInfoFromBundlingByBarcode(string barcode)
        {
            var result = default(ProductInfo);
            //先从内存中查找数据
            string key = KeyFactory.ProductKeyFactory(CompanyId, StoreId, barcode);
            result = DataAdapterFactory.ProductCache.Get(key);
            if (result != null)
            {
                //缓存中存在该数据，直接返回
                return result;
            }
            else
            {
                //缓存中不存在符合条件的数据从数据库中查找
                var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);

                result = dataAdapter.GetProductInfoFromBundlingByBarcode(barcode);
                //加到缓存中
                if (result != null)
                    DataAdapterFactory.ProductCache.Set(key, result);
                return result;
            }
        }

        public ValueObject.UserInfo GetUser(string account)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetUser(account);
        }

        public IEnumerable<ValueObject.UserInfo> GetStoreUsers(User.StoreOperateAuth storeOperateAuth)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetStoreUsers(storeOperateAuth);
        }

        public ValueObject.PageResult<InventoryResult> CheckedInventory(IEnumerable<int> categorySns, string keyword, decimal price, int pageSize, int pageIndex)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.CheckedInventory(categorySns, keyword, price, pageSize, pageIndex);
        }


        public ValueObject.MemberInfo GetMemberInfo(string phone, string cardNo, string uid)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetMemberInfo(phone, cardNo, uid);
        }

        public IEnumerable<CategoryDAO> GetStoreCategory()
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetStoreCategory();
        }

        public void PosIncomePayout(string uid, decimal money, User.PosIncomePayoutMode mode)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            dataAdapter.PosIncomePayout(uid, money, mode);
        }

        public ValueObject.BillHistoryInfo GetBillDetailsHistory(string paySn)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetBillDetailsHistory(paySn);
        }

        public IEnumerable<ValueObject.ReasonItem> GetReason(int mode)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetReason(mode);
        }

        //public void Refund(List<ValueObject.ChangeRefundItem> refundList, int reason, string paySn, decimal amount, string uid)
        //{
        //    var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId);
        //    dataAdapter.Refund(refundList, reason, paySn, amount, uid);
        //}

        public DateTime RefundAll(int reason, string paySn, decimal amount, string uid, string apicodes, string newOrderSn, string newCustomOrderSn, DateTime createdt)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.RefundAll(reason, paySn, amount, uid, apicodes, newOrderSn, newCustomOrderSn, createdt);
        }

        public DateTime ChangeOrRefund(Sale.AfterSale.OrderChangeRefundSale changeList, int reason, MachineInformation machineInformation, string newPaySn, decimal amount, decimal receive, string uid, string apiCodes, DateTime saveTime, string saleman, string oldOrderSn)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.ChangeOrRefund(changeList, reason, machineInformation, newPaySn, amount, receive, uid, apiCodes, saveTime, saleman, oldOrderSn);
        }

        public void RecordPayment(string paySn, decimal amount, int apiCode, decimal change, decimal receive, string apiOrderSN = null, string cardNo = null)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            dataAdapter.RecordPayment(paySn, amount, apiCode, change, receive, apiOrderSN, cardNo);
        }

        public ApiSetting GetApiSettings(int apiCode)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetApiSettings(apiCode);
        }

        public IEnumerable<ValueObject.BillListItem> GetBills(DateTime date, Range range, string paySn, string cashier, string qmachineSn)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetBills(date, range, paySn, cashier, qmachineSn);
        }

        public void RegisterDevice(string deviceSn, ValueObject.DeviceType type)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            dataAdapter.RegisterDevice(deviceSn, type);
        }

        public bool HasRegister(string deviceSn, ValueObject.DeviceType type, bool verfyState = true)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.HasRegister(deviceSn, type, verfyState);
        }

        public IEnumerable<ValueObject.Announcement> Announcements()
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.Announcements();
        }

        public decimal GetMarketingRecordNumber(string marketingId, Pharos.ObjectModels.DTOs.MarketingQuotaMode mode)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetMarketingRecordNumber(marketingId, mode);
        }

        public void SaveMarketingRecord(string marketingId, decimal number)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            dataAdapter.SaveMarketingRecord(marketingId, number);
        }

        public void SaveOrder(Sale.ShoppingCart shoppingCart, string apiCodes, DateTime saveTime)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            dataAdapter.SaveOrder(shoppingCart, apiCodes, saveTime);
            var saleOrders = shoppingCart.GetSaleStatistics();//销售清单

            //var orderEvent = new OrderCompletedEvent()
            //{
            //    CompanyId = CompanyId,
            //    MachineSn = MachineSN,
            //    StoreId = StoreId,
            //    OrderAmount = saleOrders.Receivable,
            //    OrderProductCount = saleOrders.Num,
            //    OrderSn = shoppingCart.OrderSN,
            //    OrderReceiveAmount = shoppingCart.WipeZeroAfter,
            //    OrderDetails = shoppingCart.GetOrdeList().Select(o =>
            //        new OrderDetail()
            //        {
            //            AveragePrice = o.AveragePrice,
            //            ActualPrice = o.MarketingPrice,
            //            Barcode = o.MainBarcode,
            //            PurchaseNumber = o.SaleNumber,
            //            BuyPrice = o.Details.BuyPrice,
            //            ProductCode = o.ProductCode,
            //            SalesClassifyId = (int)o.Details.SaleStatus,
            //            ScanBarcode = o.CurrentString,
            //            SysPrice = o.Details.SystemPrice,
            //            Title = o.Details.Title,
            //            Total = o.Details.Total,
            //            Category = o.Details.Category,
            //            Size = o.Details.Size
            //        })
            //};
            //DataAdapterFactory._EventAggregator.Publish<OrderCompletedEvent>(orderEvent);
        }

        public ValueObject.DayReportResult DayMonthReport(DateTime from, DateTime to, Sale.Range range)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.DayMonthReport(from, to, range);
        }

        public IEnumerable<ValueObject.WarehouseInformations> GetWarehouseInformations()
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetWarehouseInformations();
        }

        public IEnumerable<ValueObject.Activity> Activities()
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.Activities();
        }


        public void AddWipeZero(int companyId, string paySn, decimal money)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            dataAdapter.AddWipeZero(companyId, paySn, money);
        }

        public string GetStoreName()
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetStoreName();
        }



        public ChangeOrRefundReturnOrderData IsHasCustomerOrderSn(string customerOrderSn)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.IsHasCustomerOrderSn(customerOrderSn);
        }


        public IEnumerable<object> GetAreas(int pid)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetAreas(pid);
        }
        public void AddMember(MemberDto member)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            dataAdapter.AddMember(member);
        }


        public MembershipCard GetStoredValueCardInfo(string cardNo)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetStoredValueCardInfo(cardNo);
        }

        public void SaveToStoredValueCard(string cardNo, decimal amount, decimal balance)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            dataAdapter.SaveToStoredValueCard(cardNo, amount, balance);
        }


        public IEnumerable<StoredValueCardPayDetailsItem> GetStoredValueCardPayDetails(string cardNo, DateTime start, DateTime end)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetStoredValueCardPayDetails(cardNo, start, end);
        }


        public string GetLastMemberNo()
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
            return dataAdapter.GetLastMemberNo();
        }
        public StoredValueCardRechargeResult StoredValueCardRecharge(int type,string cardNo, decimal amount)
        {
            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, StoreId, MachineSN, CompanyId, DeviceSn);
           return  dataAdapter.StoredValueCardRecharge(type,cardNo, amount);
        }

    }
}
