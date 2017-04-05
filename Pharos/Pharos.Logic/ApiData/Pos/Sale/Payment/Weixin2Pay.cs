﻿using Newtonsoft.Json;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    public class Weixin2Pay : BasePay, IThirdPartyPaymentWithoutWait
    {
        public Weixin2Pay()
            : base(18, PayMode.ScanWeixin)
        {
        }
        public object RequestPay()
        {
            var request = (HttpWebRequest)WebRequest.Create(System.IO.Path.Combine(ApiUrl, "Qrcode/WxMicroPay"));

            string msg = string.Format("storeId={2}&paySN={0}&totalPrice={1:f2}&autoCode={3}&CompanyToken={4}", base.PayDetails.PaySn, base.PayDetails.Amount, base.PayDetails.StoreId, base.PayDetails.CardNo, base.PayDetails.CompanyToken);//门店号，条码，流水号，金额
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            request.ContentLength = data.Length;
            var reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
            try
            {
                //获取服务端返回
                var response = (HttpWebResponse)request.GetResponse();
                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                var result = sr.ReadToEnd().Trim();
                sr.Close();
                var op = JsonConvert.DeserializeObject<OpResult>(result);
                if (op.Successed)
                {
                    base.PayDetails.ApiOrderSn = op.Message;
                    base.SetComplete();
                    return base.PayDetails.CreateDt;
                }
                else
                    throw new PosException(string.Format("支付请求失败！{0}", op.Message));

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool ConnectTest()
        {
            try
            {
                string url = System.IO.Path.Combine(ApiUrl, "Qrcode/OnlineTest");
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Get";
                //获取服务端返回
                var response = (HttpWebResponse)request.GetResponse();
                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                var content = sr.ReadToEnd().Trim();
                sr.Close();
                var result = Convert.ToBoolean(content);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override void RefreshStatus(string storeId, int companyToken)
        {
            base.RefreshStatus(storeId, companyToken);
            if (base.Enable)
            {
                base.Enable = ConnectTest();
            }
        }
    }
}
