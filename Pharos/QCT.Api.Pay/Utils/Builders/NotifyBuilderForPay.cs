﻿using Pharos.Logic.OMS;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Models;
using QCT.Pay.Common;
using QCT.Pay.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Pharos.Utility.Helpers;
using Newtonsoft.Json;
using System.Collections;
using QCT.Pay.Common.Helpers;

namespace QCT.Api.Pay.Utils
{
    /// <summary>
    /// 创建生成支付通知类
    /// </summary>
    public class NotifyBuilderForPay : NotifyBuilder<SxfPayNotifyRequest>
    {
        /// <summary>
        /// 创建生成支付通知
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public override SxfPayReturn Build(SxfPayNotifyRequest reqModel)
        {
            TradeOrder tradeOrder = null;
            var tradeResult = new TradeResult(reqModel);
            //保存通知结果并更改TradeOrder状态
            var isSucc = PaySvc.SaveMchTradeResult(tradeResult, out tradeOrder);
            if (!isSucc)
                return SxfPayReturn.Fail(msg: "数据接收失败");
            else
            {
                try
                {
                    var payNotify = new NotifyPayRequest(tradeOrder, tradeResult);
                    var payNotifyDic = PaySignHelper.ToASCIIDictionary(payNotify);
                    return SendPost(PayConst.QCTTRADE_NOTIFY_PAY, payNotifyDic, tradeOrder.CID, tradeOrder.PayNotifyUrl);
                }
                catch (Exception ex)
                {
                    LogEngine.WriteError(string.Format("发起支付后台通知请求异常：商户ID：{0}，门店ID：{1}，返回参数：{2}", reqModel.MerchantId, reqModel.ShopId, reqModel.ToJson()), null, LogModule.支付交易);
                    return SxfPayReturn.Fail(msg: "数据接收失败");
                }
            }
        }
    }
}