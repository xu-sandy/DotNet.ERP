﻿using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.DataAdapter
{
    /// <summary>
    /// 本地数据源适配器（for local sqlite）
    /// </summary>
    public class SqliteDataAdapter : IDataAdapter
    {
        public string StoreId { get; set; }

        public string MachineSN { get; set; }

        public bool Enable
        {
            get { throw new NotImplementedException(); }
        }

        public ValueObject.ProductInfo GetProductInfoByBarcode(string barcode)
        {
            throw new NotImplementedException();
        }

        public ValueObject.ProductInfo GetProductInfoByProductCode(string productCode)
        {
            throw new NotImplementedException();
        }

        public ValueObject.ProductInfo GetProductInfoFromBundlingByBarcode(string barcode)
        {
            throw new NotImplementedException();
        }

        public ValueObject.UserInfo GetUser(string account)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValueObject.UserInfo> GetStoreManagers(User.StoreOperateAuth storeOperateAuth)
        {
            throw new NotImplementedException();
        }

        public ValueObject.PageResult<ValueObject.InventoryResult> CheckedInventory(IEnumerable<int> categorySns, string keyword, int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public ValueObject.PageResult<ValueObject.InventoryResult> CheckedPrice(IEnumerable<int> categorySns, decimal from, decimal to, int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public string GetMemberId(string phone, string uid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValueObject.CategoryDAO> GetStoreCategory()
        {
            throw new NotImplementedException();
        }

        public void PosIncomePayout(string uid, decimal money, User.PosIncomePayoutMode mode)
        {
            throw new NotImplementedException();
        }

        public ValueObject.BillHistoryInfo GetBillDetailsHistory(string paySn)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValueObject.ReasonItem> GetReason(int mode)
        {
            throw new NotImplementedException();
        }

        public void Refund(List<ValueObject.ChangeRefundItem> refundList, int reason, string paySn, decimal amount, string uid)
        {
            throw new NotImplementedException();
        }

        public void RefundAll(int reason, string paySn, decimal amount, string uid)
        {
            throw new NotImplementedException();
        }

        public void Change(List<ValueObject.ChangeRefundItem> changeList, int reason, string paySn, string newPaySn, decimal amount, string uid, string apiCodes)
        {
            throw new NotImplementedException();
        }

        public void RecordPayment(string paySn, decimal amount, int apiCode, string apiOrderSN = null, string cardNo = null)
        {
            throw new NotImplementedException();
        }

        public ApiLibrary GetApiSettings(int apiCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValueObject.BillListItem> GetBills(DateTime date)
        {
            throw new NotImplementedException();
        }

        public void RegisterDevice(string deviceSn, ValueObject.DeviceType type)
        {
            throw new NotImplementedException();
        }

        public bool HasRegister(string deviceSn, ValueObject.DeviceType type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValueObject.Announcement> Announcements()
        {
            throw new NotImplementedException();
        }
    }
}
