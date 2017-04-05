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
    /// 创建生成退款订单
    /// </summary>
    public class OrderBuilderForRefund : OrderBuilder<RefundApplyRequest, RefundApplyResponse>
    {
        /// <summary>
        /// 创建生产退款订单
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public override QctPayReturn Build(RefundApplyRequest reqModel)
        {
            WithReqModel(reqModel);
            WithOutTradeNo(reqModel.Out_Refund_No);
            var canObj = CanBuilder();
            if (!canObj.Successed)
                return canObj;
            else
            {
                ReqModel.ResetRfdNotifyUrl(MerchModel.RfdNotifyUrl);
                var tradeOrder = new TradeOrder(ReqModel, MerchStoreModel)
                {
                    TradeNo = OrderHelper.GetMaxTradeNo(),
                    OutTradeNo = ReqModel.Out_Refund_No,
                    OldOutTradeNo = ReqModel.Out_Trade_No,
                    TotalAmount = ReqModel.Refund_Amount,
                    SourceType = MerchModel.SourceType,
                    ApiNo = MerchModel.ApiNo,
                    State = (short)RefundState.RefundIng,
                    RfdNotifyUrl = ReqModel.Refund_Notify_Url,
                    OrderType3 = ((short)SxfOrderType.CommonOrder),
                    TradeType = (short)QctTradeType.Expense,
                    FeeType = (short)PayFeeType.RMB,
                    BuyerMobile = ""
                };
                var result = PaySvc.SaveTradeOrder(tradeOrder);
                if (result)
                {
                    //构建Sxf请求参数，签名，发起请求
                    var sxfReqModel = new SxfRefundApplyRequest(tradeOrder, PayConfig.SxfRefundNotifyUrl);
                    var rstObj = SendPost<SxfRefundApplyRequest, SxfRefundApplyResponse>(MerchStoreModel.ApiUrl, sxfReqModel);
                    if (rstObj.Successed)
                    {
                        //处理返回成功结果，保存退款结果，后进行Qct签名并返回结果回发给商户，fishtodo:暂忽略验证Sxf返回的响应结果签名
                        var sxfRspModel = (SxfRefundApplyResponse)rstObj.Data;
                        //保存退款结果
                        var rfdApplyRsp = new RefundApplyResponse(tradeOrder, sxfRspModel)
                        {
                            Out_Trade_No = sxfReqModel.OldOutTradeNo
                        };
                        return QctPayReturn.Success(data:PaySignHelper.ToDicAndSign(rfdApplyRsp, MerchModel.SecretKey, "sign"));
                    }
                    else
                        return rstObj;
                }
                else
                {
                    return ResultFail(msg: "订单提交失败", logMsg: string.Format("退款请求失败：{0}，异常信息：{1}", reqModel.ToJson(), tradeOrder.ToJson()));
                }
            }
        }
    }
}