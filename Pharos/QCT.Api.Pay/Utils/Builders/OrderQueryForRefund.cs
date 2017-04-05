﻿using Newtonsoft.Json;
using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Models;
using QCT.Pay.Common.Helpers;
using QCT.Pay.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pharos.Utility.Helpers;

namespace QCT.Api.Pay.Utils
{
    public class OrderQueryForRefund : OrderBuilder<RefundQueryRequest, RefundQueryResponse>
    {
        public override object Query(RefundQueryRequest reqModel)
        {
            try
            {
                var canObj = CanAccess();
                if (!canObj.Successed)
                    return canObj;
                var sxfReq = new SxfRefundQueryRequest(reqModel, MerchStoreModel);
                //sxf签名并请求
                var sxfResult = PayHelper.SendPost(MerchStoreModel.ApiUrl, PaySignHelper.ToDicAndSign(sxfReq, MerchModel.SecretKey3, "signature"));
                if (sxfResult.Successed)
                {
                    //处理返回结果
                    var sxfResultObj = JsonConvert.DeserializeObject<SxfRefundQueryResponse>(sxfResult.Data.ToString());
                    var result = sxfResultObj.ToRefundQueryResponse(MerchStoreModel);
                    //Qct签名
                    var rstRsp = PaySignHelper.ToDicAndSign(result, MerchModel.SecretKey, "sign");
                    return rstRsp;
                }
                else
                {
                    return sxfResult;
                }
            }
            catch (Exception ex)
            {
                LogEngine.WriteError(string.Format("退款订单查询请求异常：{0}，请求参数：{1}", ex.Message, reqModel.ToJson()), null, LogModule.支付交易);
                return QctPayReturn.Fail();
            }
        }
    }
}