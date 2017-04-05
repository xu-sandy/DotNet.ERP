﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.ApiData.Pos.Sale.Barcodes;
using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.ApiData.Pos.Sale.Payment;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.ObjectModels;
using Newtonsoft.Json;

namespace Pharos.Logic.ApiData.Pos.Sale.AfterSale
{
    public class OrderChangeRefundSale : OrderMiddleware
    {
        public OrderChangeRefundSale()
        {
            ChangingList = new List<ChangeProduct>();
        }
        public AfterSaleMode Mode { get; set; }
        public List<ChangeProduct> ChangingList { get; set; }

        public string PaySn { get; internal set; }
        public string CustomOrderSn { get { return CustomOrderSnObject.ToString(); } }
        public Pharos.Infrastructure.Data.Normalize.PaySn CustomOrderSnObject { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        public int RecordCount
        {
            get
            {
                var products = ChangingList.Where(o => o.ProductType != ProductType.Weigh);
                var weighProducts = ChangingList.Where(o => o.ProductType == ProductType.Weigh);
                var count = Convert.ToInt32(products.Sum(o => o.ChangeNumber));
                count += weighProducts.Count();
                return count;
            }
        }

        public decimal Difference
        {
            get
            {
                return ChangingList.Sum(o => o.Total);
            }
        }
        public override bool VerfyProduct(IIdentification newBarcode, IIdentification oldBarcode)
        {
            if (base.VerfyProduct(newBarcode, oldBarcode))
            {
                var newP = (newBarcode as ChangeProduct);
                var oldP = (oldBarcode as ChangeProduct);
                if (string.IsNullOrEmpty(newBarcode.RecordId))
                {
                    return newP.IsChange == oldP.IsChange;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 增加退换货商品
        /// </summary>
        /// <param name="barcodeStr"></param>
        /// <param name="storeId"></param>
        /// <param name="machineSn"></param>
        /// <param name="companyToken"></param>
        /// <returns></returns>
        public object Add(string barcodeStr, string storeId, string machineSn, int companyToken, bool status)
        {
            var changingItem = ChangingList.Where(o => VerfyProduct(new ChangeProduct() { IsChange = status, MainBarcode = barcodeStr, }, o)).FirstOrDefault();

            if (changingItem == null)
            {
                var barcode = BarcodeFactory.Factory(storeId, machineSn, companyToken, barcodeStr);
                ChangingList.Add(new ChangeProduct(barcode, status));
            }
            else
            {
                if (status)
                { changingItem.ChangeNumber++; }
                else
                { changingItem.ChangeNumber--; }

                changingItem.Total = changingItem.ChangeNumber * changingItem.ChangePrice;
            }
            return new { ChangingList, Difference, PaySn = CustomOrderSn };
        }

        public object Edit(string barcodeStr, string storeId, string machineSn, int companyToken, decimal num, decimal price, string recordId, ProductType productType)
        {
            ChangeProduct changingItem;
            try
            {
                changingItem = ChangingList.Where(o => VerfyProduct(new ChangeProduct() { MainBarcode = barcodeStr, ProductType = productType, RecordId = recordId ?? "errorrecordid" }, o)).FirstOrDefault();
            }
            catch
            {
                goto ThrowException;
            }
            if (changingItem.Barcode != barcodeStr)
            {
                goto ThrowException;
            }
            if (changingItem != null)
            {
                changingItem.ChangeNumber = num;
                if (Math.Abs(changingItem.ChangePrice - price) >= 0.01m)
                {
                    changingItem.ChangePrice = price;
                    changingItem.HasEditPrice = true;
                }
                changingItem.Total = changingItem.ChangePrice * changingItem.ChangeNumber;
            }
            return new { ChangingList, Difference, PaySn = CustomOrderSn };
        ThrowException:
            throw new PosException("指定修改行失败！");
        }

        public object Remove(string barcodeStr, string storeId, string machineSn, int companyToken, string recordId, ProductType productType)
        {
            ChangeProduct changingItem;
            try
            {
                changingItem = ChangingList.Where(o => VerfyProduct(new ChangeProduct() { MainBarcode = barcodeStr, ProductType = productType, RecordId = recordId ?? "errorrecordid" }, o)).FirstOrDefault();
            }
            catch
            {
                goto ThrowException;
            }
            if (changingItem.Barcode != barcodeStr)
            {
                goto ThrowException;
            }

            ChangingList.Remove(changingItem);
            return new { ChangingList, Difference, PaySn = CustomOrderSn };
        ThrowException:
            throw new PosException("指定修改行失败！");
        }

        public DateTime SaveRecord(string storeId, string machineSn, int companyId, int reason, decimal amount, decimal receive, IPay pay, string deviceSn)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            var shoppingcart = ShoppingCartFactory.Factory(storeId, machineSn, companyId, deviceSn);
            var uid = shoppingcart.MachineInformation.CashierUid;
            string apiCodes = pay != null ? pay.ApiCodes : "-1";
            DateTime saveTime = pay != null ? pay.PayDetails.CreateDt : DateTime.Now;
            var orderTime = dataAdapter.ChangeOrRefund(this, reason, shoppingcart.MachineInformation, this.CustomOrderSn, amount, receive, uid, apiCodes, saveTime, this.SaleMan, this.OldOrderSn);
            new Pharos.Infrastructure.Data.Normalize.PaySn(companyId, storeId, machineSn).NextSerialNumber();
            return orderTime;
        }
        public static IEnumerable<ReasonItem> GetChangeReason(string storeId, string machineSn, int companyId, string deviceSn)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            return dataAdapter.GetReason(1);
        }
        public static string GetRefundAllCustomOrderSn(string storeId, string machineSn, int companyId)
        {
            Pharos.Infrastructure.Data.Normalize.PaySn orderSn = new Infrastructure.Data.Normalize.PaySn(companyId, storeId, machineSn);
            return orderSn.ToString();

        }
        public static DateTime RefundAll(string storeId, string machineSn, int companyId, int reason, string paySn, decimal amount, string deviceSn, IPay pay, DateTime createdt)
        {
            var customOrderSn = new Pharos.Infrastructure.Data.Normalize.PaySn(companyId, storeId, machineSn);
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            var shoppingcart = ShoppingCartFactory.Factory(storeId, machineSn, companyId, deviceSn);
            var uid = shoppingcart.MachineInformation.CashierUid;
            var datetime = dataAdapter.RefundAll(reason, paySn, amount, uid, pay.ApiCodes, pay.PayDetails.PaySn, customOrderSn.ToString(), createdt);
            customOrderSn.NextSerialNumber();
            return datetime;
        }

        public static IEnumerable<ReasonItem> GetRefundReason(string storeId, string machineSn, int companyId, string deviceSn)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            return dataAdapter.GetReason(2);
        }
        public SaleManInfo SetSaleMan(string saleMan, string storeId, string machineSn, int companyId, string deviceSn)
        {
            if (saleMan == "0")
            {
                SaleMan = string.Empty;
                return new SaleManInfo();
            }
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            var saleManInfo = dataAdapter.GetUser(saleMan);
            if (saleManInfo != null)
            {
                SaleMan = saleManInfo.UID;
                //return string.Format("[{0};{1}]", saleManInfo.UserCode, saleManInfo.FullName);
                return new SaleManInfo() { SaleManCode = saleManInfo.UserCode, SaleManName = saleManInfo.FullName };
            }
            else
            {
                throw new PosException("606", "导购员不存在！");
            }
        }
        /// <summary>
        /// 退换货时设置原单号
        /// </summary>
        /// <param name="oldOrderSn"></param>
        /// <returns></returns>
        public ChangeOrRefundReturnOrderData SetOldOrderSN(string oldOrderSn, string storeId, string machineSn, int companyId, string deviceSn)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            var orderInfos = dataAdapter.IsHasCustomerOrderSn(oldOrderSn);
            //if (!order)
            //{
            //    return false;
            //}
            OldOrderSn = oldOrderSn;//设置原订单号
            return orderInfos;

        }
        public string SaleMan { get; set; }

        public string OldOrderSn { get; set; }
    }
}