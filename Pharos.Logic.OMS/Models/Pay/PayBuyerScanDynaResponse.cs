using Newtonsoft.Json;
using Pharos.Logic.OMS.Entity;
using QCT.Pay.Common;
using System;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 购买者付款扫码（动态二维码）响应Model，（对应融合支付：主扫支付动态二维码）
    /// </summary>
    [Serializable]
    public class PayBuyerScanDynaResponse : PayTradeResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public PayBuyerScanDynaResponse() { }
        /// <summary>
        /// 根据参数构造返回结果
        /// </summary>
        /// <param name="order"></param>
        public PayBuyerScanDynaResponse(TradeOrder order, SxfBuildPayTokenResponse rsp)
            : base(order)
        {
            Return_Code = PayTradeHelper.TransCodeBySxf(rsp.RspCod);
            Return_Msg = rsp.RspMsg;
            Sign_Type = PayConst.DEF_SIGNTYPE;
            Version = PayConst.DEF_VERSION;

            Out_Trade_No = rsp.PayOrderNo;
            Pay_Token = rsp.PayToken;
            QRCode_Url = rsp.ImageUrl;
        }
        /// <summary>
        /// 动态二维码数据，一串支付的 url，请商家自行将该结果生成二维码
        /// </summary>
        [JsonProperty("pay_token")]
        public string Pay_Token { get; set; }
        /// <summary>
        /// 动态二维码的 httpurl
        /// </summary>
        [JsonProperty("qrcode_url")]
        public string QRCode_Url { get; set; }

    }
}
