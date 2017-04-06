using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pharos.Component.qrcode;
using Pharos.Component.qrcode.wx;

namespace Pharos.Pay.Retailing
{
    public partial class PayFor2 : System.Web.UI.Page
    {
        public string wxJsApiParam { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                var result = GetUnifiedOrderResult();
                wxJsApiParam= GetJsApiParameters(result);
            }
        }

        WxPayData GetUnifiedOrderResult()
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", "test");
            data.SetValue("attach", "test");
            data.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo());
            data.SetValue("notify_url", "http://demo.xmpharos.com/WXNotify.aspx");
            data.SetValue("total_fee", 1);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", "test");
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", "oAZCps75h8Jyj1a0uyIktHiGHbho");
            data.SetValue("spbill_create_ip", WxPayConfig.IP);//终端ip	  	    
            data.SetValue("appid", "wxbeb2e5a46ca7bf69");//公众账号ID
            data.SetValue("mch_id", "1240541002");//商户号
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串,放置顺序
            //签名
            data.SetValue("sign", data.MakeSign("e10adc3849ba56abbe56e056f2588888"));
            string xml = data.ToXml();
            var start = DateTime.Now;
            Log.Debug("WxPayApi", "UnfiedOrder request : " + xml);
            string response = HttpService.Post(xml, url, false, 10);
            Log.Debug("WxPayApi", "UnfiedOrder response : " + response);
            WxPayData result = new WxPayData();
            result.FromXml(response);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new Exception("UnifiedOrder response error!");
            }
            return result;
        }
        string GetJsApiParameters(WxPayData unifiedOrderResult)
        {
            Log.Debug(this.GetType().ToString(), "JsApiPay::GetJsApiParam is processing...");

            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign("e10adc3849ba56abbe56e056f2588888"));

            string parameters = jsApiParam.ToJson();

            Log.Debug(this.GetType().ToString(), "Get jsApiParam : " + parameters);
            return parameters;
        }
    }
}