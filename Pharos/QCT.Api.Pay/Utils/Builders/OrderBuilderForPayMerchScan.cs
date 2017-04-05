﻿using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Models;
using QCT.Pay.Common;
using QCT.Pay.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pharos.Utility.Helpers;
using QCT.Pay.Common.Models;
using Pharos.Logic.OMS.BLL;

namespace QCT.Api.Pay.Utils
{
    /// <summary>
    /// 创建生成支付订单，商家收款扫码，（对应融合支付：被扫支付）
    /// </summary>
    public class OrderBuilderForPayMerchScan : OrderBuilder<PayMerchScanRequest, PayMerchScanResponse>
    {
        /// <summary>
        /// 创建生成支付订单，商家收款扫码，（对应融合支付：被扫支付）
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public override QctPayReturn Build(PayMerchScanRequest reqModel)
        {
            WithReqModel(reqModel);
            WithOutTradeNo(reqModel.Out_Trade_No);
            var canObj = CanBuilder();
            if (!canObj.Successed)
                return canObj;
            else
            {
                var tradeOrder = new TradeOrder(ReqModel, MerchStoreModel, OrderHelper.GetMaxTradeNo());
                tradeOrder.BuyerPayToken = ReqModel.Buyer_Pay_Token;
                var result = PaySvc.SaveTradeOrder(tradeOrder);
                if (result)
                {
                    //构建Sxf请求参数，签名，发起请求
                    var sxfReqModel = new SxfScanPayRequest(tradeOrder, PayConfig.SxfPayNotifyUrl);
                    var rstObj = SendPost<SxfScanPayRequest, SxfScanPayResponse>(MerchStoreModel.ApiUrl, sxfReqModel);
                    if (rstObj.Successed)
                    {
                        //处理返回成功结果，保存商家收款扫码结果，后进行Qct签名并返回结果回发给商户，fishtodo:暂忽略验证Sxf返回的响应结果签名
                        var sxfRspModel = (SxfScanPayResponse)rstObj.Data;
                        PaySvc.SaveMerchScanResult(sxfRspModel, out tradeOrder);
                        var merchScanRsp = new PayMerchScanResponse(tradeOrder, sxfRspModel);
                        return QctPayReturn.Success(data:PaySignHelper.ToDicAndSign(merchScanRsp, MerchModel.SecretKey, "sign"));
                    }
                    else
                        return rstObj;
                }
                else
                {
                    return ResultFail(msg: "订单提交失败", logMsg: string.Format("商家收款扫码请求失败：{0}，异常信息：{1}", reqModel.ToJson(), tradeOrder.ToJson()));
                }
            }
        }
    }
}