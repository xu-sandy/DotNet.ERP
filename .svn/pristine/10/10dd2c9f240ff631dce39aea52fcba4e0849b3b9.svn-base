using Newtonsoft.Json;
using Pharos.Logic.OMS.Entity;
using QCT.Pay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 退款申请返回
    /// </summary>
    public class RefundApplyResponse : BaseTradeResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public RefundApplyResponse() { }
        /// <summary>
        /// 构造退款申请响应商户基本信息
        /// </summary>
        /// <param name="order"></param>
        public RefundApplyResponse(TradeOrder order, SxfRefundApplyResponse rsp)
            : base(order)
        {
            Return_Code = PayTradeHelper.TransCodeBySxf(rsp.RspCod);
            Return_Msg = rsp.RspMsg;
            Sign_Type = PayConst.DEF_SIGNTYPE;
            Version = PayConst.DEF_VERSION;

            Refund_Status = (PayTradeHelper.Convert2EnumString<RefundState>(PayTradeHelper.Convert2EnumValue<SxfRefundState>(rsp.RefundResult))).ToUpper();
            Out_Refund_No = rsp.OutRefundNo;
            Refund_Amount = rsp.RefundAmount;
        }

        /// <summary>
        /// 商户退款订单号，每笔退款订单的唯一标识，商户需保持该字段在系统内唯一
        /// </summary>
        [JsonProperty("out_refund_no")]
        public string Out_Refund_No { get; set; }
        /// <summary>
        /// 原商户支付订单号
        /// </summary>
        [JsonProperty("out_trade_no")]
        public string Out_Trade_No { get; set; }

        /// <summary>
        /// 退款金额，以元为单位
        /// </summary>
        [JsonProperty("refund_amount")]
        public decimal Refund_Amount { get; set; }
        /// <summary>
        /// 退款结果，商家如果收到此字段值为"REFUNDING"，说明该退款订单还处于退款中，请等待退款结果后台通知，以确认是否退款成功
        /// REFUNDING：退款中;REFUNDSUCCESS：退款成功;REFUNDFAIL：退款失败
        /// </summary>
        [JsonProperty("refund_status")]
        public string Refund_Status { get; set; }
    }
}
