using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Pharos.Component.qrcode.wx
{
    public class ResultNotify
    {
        private static WxPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = HttpContext.Current.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            Log.Info("ResultNotify", "Receive data from WeChat : " + builder.ToString());

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (Exception ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                var xml = res.ToXml();
                Log.Error("ResultNotify", "Sign check error : " + xml);
                HttpContext.Current.Response.Write(xml);
                HttpContext.Current.Response.End();
            }

            Log.Info("ResultNotify", "Check sign success");
            return data;
        }
        public static bool ProcessNotify(ref string xml, ref WxPayData notifyData)
        {
            notifyData = GetNotifyData();
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                xml = res.ToXml();
                Log.Error("ResultNotify", "The Pay result is error : " + xml);
                return false;
            }
            //查询订单，判断订单真实性
            if (!QueryOrder(notifyData))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                xml = res.ToXml();
                Log.Error("ResultNotify", "Order query failure : " + xml);
                return false;
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                xml = res.ToXml();
                Log.Info("ResultNotify", "order query success : " + xml);
                return true;
            }
        }

        //查询订单
        private static bool QueryOrder(WxPayData notifyData)
        {
            string transaction_id = notifyData.GetValue("transaction_id").ToString();
            string appid = notifyData.GetValue("appid").ToString();
            string mch_id = notifyData.GetValue("mch_id").ToString();
            string nonce_str = notifyData.GetValue("nonce_str").ToString();

            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            req.SetValue("appid", appid);
            req.SetValue("mch_id", mch_id);
            req.SetValue("nonce_str", nonce_str);
            req.SetValue("sign", req.MakeSign());
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return true;
            }
        }
        public static bool TestQuery(string transaction_id, string appid, string mch_id, string sign, string nonce_str)
        {
            WxPayData notifyData = new WxPayData();
            notifyData.SetValue("transaction_id", transaction_id);
            notifyData.SetValue("appid", appid);
            notifyData.SetValue("mch_id", mch_id);
            notifyData.SetValue("sign", sign);
            notifyData.SetValue("nonce_str", nonce_str);
            return QueryOrder(notifyData);
        }
    }
}