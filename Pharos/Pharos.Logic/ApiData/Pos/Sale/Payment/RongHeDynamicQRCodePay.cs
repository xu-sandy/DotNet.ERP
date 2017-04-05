﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Logic.BLL;
using Pharos.Logic.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using Pharos.Infrastructure.Data.Normalize;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    public class RongHeDynamicQRCodePay : BasePay, IBackgroundPayment
    {
        public RongHeDynamicQRCodePay()
            : base(25, PayMode.RongHeDynamicQRCodePay)
        {
        }
        public const string SIGNTYPE = "MD5";//签名类型
        public const string VERSION = "1.0";//调用版本
        public const string CHARSET = "utf-8";//字符集格式

        public const string QCTTRADE_PAY_BUYERSCAN_DYNA = "qct.trade.pay.buyerscandyna";
        public const string QCTTRADE_PAY_MERCHSCAN = "qct.trade.pay.merchscan";
        public const string QCTTRADE_REFUNDAPPLY = "qct.trade.refund.apply";

        public const string QCTTRADE_NOTIFY_PAY = "qct.trade.notify.pay";
        public const string QCTTRADE_NOTIFY_REFUND = "qct.trade.notify.refund";


        //   public const string QCTPAY_CALLBACK = "http://{0}:{1}/api/pay/QuanChengTaoPayCallBack";
        public String Sign(SortedDictionary<String, Object> signObj, String md5Key)
        {
            StringBuilder signdatasb = new StringBuilder();
            foreach (var item in signObj)
            {
                String key = item.Key;
                String value = signObj[key] == null ? "" : signObj[key].ToString();
                signdatasb.Append("&").Append(key).Append("=").Append(value);
            }

            String signdata = signdatasb.ToString().Substring(1) + "&key=" + md5Key;
            return GetMD5(signdata);
        }
        public string GetMD5(string value)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            string sh1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sh1 += data[i].ToString("X2");
            }
            return sh1;
        }
        public SortedDictionary<string, object> GetRongHeDynamicQRCodePayRequestParameter(int companyId, string storeId, string machineSn, string orderSn, decimal amount)
        {
            var payConfig = BaseGeneralService<Pharos.Logic.Entity.PayConfiguration, EFDbContext>.Find(o => o.CompanyId == companyId && o.PayType == 25);

            if (payConfig == null || payConfig.State == 0)
                throw new PosException("支付配置尚未启用，无法授权支付！");
            var storePaymentAuthorization = BaseGeneralService<Pharos.Logic.Entity.StorePaymentAuthorization, EFDbContext>.Find(o => o.CompanyId == companyId && o.PayType == 25 && o.StoreId == storeId);
            if (storePaymentAuthorization == null || storePaymentAuthorization.State == 0)
            {
                throw new PosException("门店支付尚未授权，无法进行支付！");
            }

            var QCTPAY_CALLBACK = ConfigurationManager.AppSettings["QCTPAY_CALLBACK"].ToString();
            if (string.IsNullOrEmpty(QCTPAY_CALLBACK))
            {
                throw new PosException("支付配置不完整，无法完成支付！");
            }
            var reqParams = new SortedDictionary<string, object>();
            reqParams.Add("charset", CHARSET);
            reqParams.Add("method", QCTTRADE_PAY_BUYERSCAN_DYNA);
            reqParams.Add("mch_id", payConfig.PaymentMerchantNumber);
            reqParams.Add("store_id", storePaymentAuthorization.MapPaymentStoreId);
            reqParams.Add("device_id", machineSn);
            reqParams.Add("sign_type", SIGNTYPE);
            reqParams.Add("version", VERSION);
            reqParams.Add("out_trade_no", orderSn);
            reqParams.Add("create_date", DateTime.Now);
            reqParams.Add("total_amount", amount);
            reqParams.Add("pay_notify_url", QCTPAY_CALLBACK);
            reqParams.Add("buyer_mobile", "");
            reqParams.Add("goods_name", payConfig.Description);
            reqParams.Add("goods_desc", "");

            reqParams.Add("sign", Sign(reqParams, payConfig.SecurityKey));
            return reqParams;
        }
        /// <summary>
        /// 接口调用请求
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private object PostPay(string url, SortedDictionary<string, object> obj, string orderSn)
        {
            Stream requestStream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            Stream responseStream = null;

            try
            {
                StringBuilder signdatasb = new StringBuilder();
                foreach (var item in obj)
                {
                    String key = item.Key;
                    String value = obj[key] == null ? "" : obj[key].ToString();
                    signdatasb.Append("&").Append(key).Append("=").Append(value);
                }
                byte[] byteRequest = System.Text.Encoding.UTF8.GetBytes(signdatasb.ToString().Substring(1));

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);

                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.ContentLength = byteRequest.Length;
                httpRequest.Timeout = 120000;
                requestStream = httpRequest.GetRequestStream();
                requestStream.Write(byteRequest, 0, byteRequest.Length);
                requestStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)httpRequest.GetResponse();
                responseStream = response.GetResponseStream();
                //获取服务端返回数据
                sr = new StreamReader(responseStream, Encoding.UTF8);
                var result = sr.ReadToEnd().Trim();
                sr.Close();
                JsonConvert.DeserializeObject(result);
                var rspObj = JsonConvert.DeserializeObject<JObject>(result);
                if (rspObj.Property("return_code").Value.ToString() == "00000")
                {
                    return new { PayToken = rspObj.Property("pay_token").Value.ToString(), PayOrderSn = orderSn };
                    //todo:记录请求成功数据
                }
                else
                {
                    throw new PosException("支付请求失败！");

                }
            }
            catch (WebException ex)
            {
                throw new PosException("支付请求失败，网络错误！");
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
                if (sr != null)
                {
                    sr.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
            }
        }


        public DateTime RequestPayDate { get; set; }

        public object RequestPay()
        {

            var shoppingCart = ShoppingCartFactory.Factory(StoreId, MachineSn, CompanyId, DeviceSn);
            lock (SwiftNumber.LockObject)
            {
                var ordersn = new PayOrderSn(CompanyId, StoreId);
                shoppingCart.PayOrderSn = ordersn.ToString();
                ShoppingCartFactory.ResetCache(shoppingCart, StoreId, MachineSn, CompanyId, DeviceSn);
            }
            var p = GetRongHeDynamicQRCodePayRequestParameter(CompanyId, StoreId, MachineSn, shoppingCart.PayOrderSn, PayDetails.Amount);
            RequestPayDate = DateTime.Now;
            // var RongHeDynamicQRCodePayUrl = ConfigurationManager.AppSettings["RongHeDynamicQRCodePay"].ToString();
            var RongHeDynamicQRCodePayUrl = Path.Combine(ConfigurationManager.AppSettings["RongHeDynamicQRCodePay"].ToString(), "api/pay/pay");

            if (string.IsNullOrEmpty(RongHeDynamicQRCodePayUrl))
            {
                throw new PosException("支付配置不完整，无法完成支付！");
            }
            return PostPay(RongHeDynamicQRCodePayUrl, p, shoppingCart.PayOrderSn);
        }

        public ThirdPartyPaymentStatus GetPayStatus()
        {
            var payConfig = BaseGeneralService<Pharos.Logic.Entity.PayConfiguration, EFDbContext>.Find(o => o.CompanyId == CompanyId && o.PayType == 25);
            var ordersn = ShoppingCartFactory.Factory(StoreId, MachineSn, CompanyId, DeviceSn).PayOrderSn;
            var storePaymentAuthorization = BaseGeneralService<Pharos.Logic.Entity.StorePaymentAuthorization, EFDbContext>.Find(o => o.CompanyId == CompanyId && o.PayType == 25 && o.StoreId == StoreId);
            var mic = int.Parse(payConfig.PaymentMerchantNumber);
            var entity = PayNotifyResultService.Find(o => o.ApiCode == 25 && o.PaySN == ordersn && o.CompanyId == mic);
            if (entity == null)
            {
                return ThirdPartyPaymentStatus.Unknown;
            }
            else if (entity.State == "PAYSUCCESS")
            {
                return ThirdPartyPaymentStatus.Complete;
            }
            else
            {
                return ThirdPartyPaymentStatus.Error;
            }
        }

        public bool ConnectTest()
        {
            return true;
        }
    }
}
