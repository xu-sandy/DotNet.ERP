﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pharos.Component.qrcode.wx;
using qrcode = Pharos.Component.qrcode;
namespace Pharos.Pay.PayResult
{
    public partial class WXNotify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            qrcode.Log.Info(this.GetType().Name, "微信进入通知页面");
            WxPayData notifyData = null;
            string xml = "";
            try
            {
                if (qrcode.wx.ResultNotify.ProcessNotify(ref xml, ref notifyData))
                {
                    qrcode.Log.Info(this.GetType().Name, notifyData.ToXml());
                    var fee = notifyData.GetValue("cash_fee").ToString();
                    var paysn = notifyData.GetValue("attach").ToString();
                    var trade_no = notifyData.GetValue("transaction_id").ToString();
                    if (string.IsNullOrWhiteSpace(fee) || fee == "0")
                        fee = notifyData.GetValue("total_fee").ToString();
                    decimal tofee = 0;
                    decimal.TryParse(fee, out tofee);
                    int companyId = 0;
                    if (paysn.Contains("_"))
                    {
                        companyId = int.Parse(paysn.Split('_')[0]);
                        paysn = paysn.Split('_')[1];
                    }
                    NotifyDAL.AddOne(new PayNotifyResult()
                    {
                        PaySN = paysn,
                        CashFee = tofee / 100,
                        ApiCode = 13,
                        TradeNo = trade_no,
                        State = "Success",
                        CompanyId=companyId
                    });
                }
                else
                {

                    var fee = notifyData.GetValue("cash_fee").ToString();
                    var paysn = notifyData.GetValue("attach").ToString();
                    var trade_no = notifyData.GetValue("transaction_id").ToString();
                    if (string.IsNullOrWhiteSpace(fee) || fee == "0")
                        fee = notifyData.GetValue("total_fee").ToString();
                    decimal tofee = 0;
                    decimal.TryParse(fee, out tofee);
                    var state = Convert.ToString(notifyData.GetValue("err_code_des"));
                    if (string.IsNullOrWhiteSpace(state))
                        state = Convert.ToString(notifyData.GetValue("return_msg"));
                    if (string.IsNullOrWhiteSpace(state))
                        state = "Success";
                    int companyId = 0;
                    if (paysn.Contains("_"))
                    {
                        companyId = int.Parse(paysn.Split('_')[0]);
                        paysn = paysn.Split('_')[1];
                    }
                    NotifyDAL.AddOne(new PayNotifyResult()
                    {
                        PaySN = paysn,
                        CashFee = tofee / 100,
                        ApiCode = 13,
                        TradeNo = trade_no,
                        State = state,
                        CompanyId = companyId
                    });

                }
            }
            catch (Exception ex)
            {
                qrcode.Log.Error(this.GetType().Name, "支付失败异常!" + ex.Message+ex.StackTrace);
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                xml = res.ToXml();
            }
            Response.Write(xml);
        }
    }
}