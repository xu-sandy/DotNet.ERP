using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Xml;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Pharos.Utility;
using qrcode = Pharos.Component.qrcode;
using Pharos.Utility.Helpers;
using ThoughtWorks.QRCode.Codec;
using Pharos.Logic;
using Pharos.Logic.BLL;


namespace Pharos.CRM.Retailing.Controllers
{
    public class QrcodeController : BaseController
    {

        #region 扫码即时到帐
        public ActionResult QrcodePay(string paySN, decimal totalPrice)
        {
            var op = new OpResult();
            if (paySN.IsNullOrEmpty())
                op.Message = "订单编号不能为空！";
            else
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //动作
                string method = "add";
                //创建商品二维码
                //业务类型
                string biz_type = "10";
                //json数据
                string biz_data = GetBizData(paySN, totalPrice);

                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("service", "alipay.mobile.qrcode.manage");
                sParaTemp.Add("partner", qrcode._1.Config.Partner);
                sParaTemp.Add("_input_charset", qrcode._1.Config.Input_charset.ToLower());
                sParaTemp.Add("timestamp", timestamp);
                sParaTemp.Add("method", method);
                sParaTemp.Add("biz_type", biz_type);
                sParaTemp.Add("biz_data", biz_data);
                string _sign = "";
                foreach (var item in sParaTemp.OrderBy(s => s.Key))
                {
                    _sign = appendParam(_sign, item.Key, item.Value);
                }
                //签名
                string sign = qrcode._1.AlipayMD5.Sign(_sign, qrcode._1.Config.Key, qrcode._1.Config.Input_charset.ToLower());
                sParaTemp.Add("sign_type", qrcode._1.Config.Sign_type);
                sParaTemp.Add("sign", sign);
                //建立请求
                string sHtmlText = qrcode._1.Submit.BuildRequest(sParaTemp);

                //请在这里加上商户的业务逻辑程序代码
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(sHtmlText);

                    XmlNode root = xmlDoc.DocumentElement;//返回的根节点
                    string is_success = root.SelectSingleNode("is_success").InnerText;
                    if (is_success == "T")
                    {
                        string result_code = root.SelectSingleNode("/alipay/response/alipay/result_code").InnerText;
                        if (result_code == "SUCCESS")
                        {
                            string codeUrl = root.SelectSingleNode("/alipay/response/alipay/qrcode").InnerText;//二维码地址
                            //string qrcode_img_url = root.SelectSingleNode("/alipay/response/alipay/qrcode_img_url").InnerText;//二维码图片地址
                            var createUrl = Url.Action("GenerateQrcode", new { codeUrl = codeUrl });
                            //op.Message = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("CreateQRCode", new { url = createUrl, paySN = paySN, price = totalPrice });
                            op.Message = Request.Url.Scheme + "://" + Request.Url.Authority + createUrl;
                            op.Successed = true;
                        }
                        else
                        {
                            op.Message = "业务处理失败！";
                        }
                    }
                    else
                    {
                        op.Message = "请求失败！";
                    }
                }
                catch
                {
                    op.Message = "系统异常！";
                }
            }
            return new JsonNetResult(op);
        }
        /// <summary>
        /// 获取json数据
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        private string GetBizData(string OrderNo, decimal price)
        {
            StringBuilder sbJson = new StringBuilder();
            //有些数据不必传
            sbJson.Append("{");
            sbJson.AppendFormat("\"need_address\":\"{0}\",", "F");//是否需要收货地址：T需要，F不需要
            sbJson.AppendFormat("\"trade_type\":\"{0}\",", "1");//交易类型，1 即时到账
            sbJson.AppendFormat("\"notify_url\":\"{0}\",", qrcode._1.Config.Notify_url);
            //sbJson.Append("\"ext_info\":{\"pay_timeout\":\"5\"},");
            string goods = "\"goods_info\":{\"id\":\"" + OrderNo + "\",\"name\":\"东本-订单编号" + OrderNo + "\",\"price\":\"" + price + "\",\"expiry_date\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|" + DateTime.Now.AddMinutes(10).ToString("yyyy-MM-dd HH:mm:ss") + "\"}";
            sbJson.Append(goods);
            sbJson.Append("}");
            return sbJson.ToString();
        }
        private string appendParam(string returnStr, string paramId, string paramValue)
        {
            if (returnStr != "")
            {
                if (paramValue != "")
                {
                    returnStr += "&" + paramId + "=" + paramValue;
                }
            }
            else
            {
                if (paramValue != "")
                {
                    returnStr = paramId + "=" + paramValue;
                }
            }
            return returnStr;
        }
        private SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            var sArray = new SortedDictionary<string, string>();
            var coll = Request.Form;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            return sArray;
        }
        #endregion

        #region 当面付
        IAopClient client = new DefaultAopClient(qrcode._2.Config.serverUrl, qrcode._2.Config.appId, qrcode._2.Config.merchant_private_key, "", qrcode._2.Config.version,
            qrcode._2.Config.sign_type, qrcode._2.Config.alipay_public_key, qrcode._2.Config.charset);
        public ActionResult QrcodeRSAPay(string bizContent)
        {
            var op = new OpResult();
            AlipayTradePrecreateRequest payRequst = new AlipayTradePrecreateRequest();
            payRequst.BizContent = bizContent;

            //需要异步通知的时候，需要是指接收异步通知的地址。
            payRequst.SetNotifyUrl(qrcode._1.Config.Notify_url);
            Dictionary<string, string> paramsDict = (Dictionary<string, string>)payRequst.GetParameters();
            AlipayTradePrecreateResponse payResponse = client.Execute(payRequst);

            //以下返回结果的处理供参考。
            //payResponse.QrCode即二维码对于的链接
            //将链接用二维码工具生成二维码打印出来，顾客可以用支付宝钱包扫码支付。
            string result = payResponse.Body;
            if (payResponse != null)
            {
                switch (payResponse.Code)
                {
                    case qrcode._2.ResultCode.SUCCESS:
                        var createUrl = Url.Action("GenerateQrcode", new { codeUrl = payResponse.QrCode });
                        op.Message = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("CreateQRCode", new { url = createUrl, paySN = payResponse.OutTradeNo });
                        op.Successed = true;
                        break;
                    case qrcode._2.ResultCode.FAIL:
                        StringBuilder sb2 = new StringBuilder();
                        sb2.Append("{\"out_trade_no\":\"" + payResponse.OutTradeNo + "\"}");
                        Cancel(sb2.ToString());
                        op.Message = "预下单失败";
                        break;
                }
            }
            return new JsonNetResult(op);
        }
        private AlipayTradeCancelResponse Cancel(string biz_content)
        {
            AlipayTradeCancelRequest cancelRequest = new AlipayTradeCancelRequest();
            cancelRequest.BizContent = biz_content;
            AlipayTradeCancelResponse cancelResponse = client.Execute(cancelRequest);
            if (null != cancelResponse)
            {
                if (cancelResponse.Code == qrcode._2.ResultCode.FAIL && cancelResponse.RetryFlag == "Y")
                {
                    //if (cancelResponse.Body.Contains("\"retry_flag\":\"Y\""))		
                    //cancelOrderRetry(biz_content);
                    // 新开一个线程重试撤销
                    ParameterizedThreadStart ParStart = new ParameterizedThreadStart(cancelOrderRetry);
                    Thread myThread = new Thread(ParStart);
                    object o = biz_content;
                    myThread.Start(o);
                }
            }
            return cancelResponse;
        }

        private void cancelOrderRetry(object o)
        {
            int retryCount = 50;

            for (int i = 0; i < retryCount; ++i)
            {
                Thread.Sleep(10000);
                AlipayTradeCancelRequest cancelRequest = new AlipayTradeCancelRequest();
                cancelRequest.BizContent = o.ToString();
                // Dictionary<string, string> paramsDict = (Dictionary<string, string>)cancelRequest.GetParameters();
                AlipayTradeCancelResponse cancelResponse = client.Execute(cancelRequest);

                if (null != cancelResponse)
                {
                    if (cancelResponse.Code == qrcode._2.ResultCode.FAIL)
                    {
                        //if (cancelResponse.Body.Contains("\"retry_flag\":\"N\""))		
                        if (cancelResponse.RetryFlag == "N")
                        {
                            break;
                        }
                    }
                    if ((cancelResponse.Code == qrcode._2.ResultCode.SUCCESS))
                    {
                        break;
                    }
                }

                if (i == retryCount - 1)
                {
                    // 处理到最后一次，还是未撤销成功，需要在商户数据库中对此单最标记，人工介入处理

                }

            }
        }

        #endregion
        #region 微信支付
        public ActionResult WxQrcodePay(string paySN, decimal totalPrice)
        {
            var data = new qrcode.wx.WxPayData();
            data.SetValue("body", "东本-订单编号" + paySN);//商品描述
            data.SetValue("attach", paySN);//附加数据
            data.SetValue("out_trade_no", qrcode.wx.WxPayApi.GenerateOutTradeNo());//随机字符串
            data.SetValue("total_fee", Convert.ToInt32(totalPrice * 100));//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", "");//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", paySN);//商品ID
            var op = new OpResult();
            try
            {
                var result = qrcode.wx.WxPayApi.UnifiedOrder(data);//调用统一下单接口

                if (Convert.ToString(result.GetValue("result_code")) == "SUCCESS" && Convert.ToString(result.GetValue("return_code")) == "SUCCESS")
                {
                    string createUrl = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
                    createUrl = Url.Action("GenerateQrcode", new { codeUrl = createUrl });
                    //op.Message = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("CreateQRCode", new { url = createUrl, paySN = paySN, price = totalPrice });
                    op.Message = Request.Url.Scheme + "://" + Request.Url.Authority + createUrl;
                    op.Successed = true;
                }
                else if (Convert.ToString(result.GetValue("return_code")) == "SUCCESS")
                    op.Message = result.GetValue("err_code_des").ToString();
                else
                    op.Message = Convert.ToString(result.GetValue("return_msg"));

            }
            catch (Exception ex)
            {
                qrcode.Log.Error(this.GetType().Name, ex.Message);
                op.Message = ex.Message;
                new Sys.LogEngine().WriteError(ex);
            }
            return new JsonNetResult(op);
        }
        #endregion
        #region 生成二维码
        public void GenerateQrcode(string codeUrl)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 7;

            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(codeUrl, Encoding.Default);
            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            //输出二维码图片
            Response.BinaryWrite(ms.GetBuffer());
            Response.End();
        }
        public ActionResult CreateQRCode(string url, string paySN, decimal? price)
        {
            ViewBag.URL = url;
            return View();
        }
        #endregion
        #region 获取支付状态

        public int GetStatus(int apiCode, string paySn)
        {
            var entity = PayNotifyResultService.Find(o => o.ApiCode == apiCode && o.PaySN == paySn);
            if (entity == null)
            {
                return 0;
            }
            else if (entity.State == "Success")
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        #endregion 获取支付状态
        public bool OnlineTest()
        {
            return true;
        }
        #region 客户端连接测试不可去掉

        #endregion 客户端连接测试不可去掉
    }
}
