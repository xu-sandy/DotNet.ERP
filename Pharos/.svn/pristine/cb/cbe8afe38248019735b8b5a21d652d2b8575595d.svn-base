using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using System.Net;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Models;
using Pharos.Logic.OMS.BLL;
using QCT.Api.Pay.Utils;
using QCT.Pay.Common;
using QCT.Pay.Common.Models;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Pharos.Logic.OMS.BLL.Pay;
using System.IO;
using System.Text;
using System.Web;
 

namespace QCT.Api.Pay.Controllers
{
    /// <summary>
    /// QCT 支付相关
    /// </summary>
    [RoutePrefix("Pay")]
    public class PayController : BasePayController
    {
        // GET: /Pay/
        #region Ninject Properties
        /// <summary>
        /// 支付交易Service
        /// </summary>
        [Ninject.Inject]
        PayService PaySvc { get; set; }

        #endregion

        #region Test
        public void Test()
        {
            //var url = "http://localhost:23760/api/pay/SxfPayNotify";
            //var paramsStr = "";
            //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            //httpRequest.Method = "POST";
            //httpRequest.ContentType = "application/x-www-form-urlencoded";
            //httpRequest.Timeout = 45000;
            //byte[] byteRequest = System.Text.Encoding.UTF8.GetBytes(paramsStr);
            //httpRequest.ContentLength = byteRequest.Length;
            //Stream requestStream = httpRequest.GetRequestStream();
            //requestStream.Write(byteRequest, 0, byteRequest.Length);
            //requestStream.Close();

            ////获取服务端返回
            //var response = (HttpWebResponse)httpRequest.GetResponse();
            ////获取服务端返回数据
            //StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            //var result = sr.ReadToEnd().Trim();
            //sr.Close();
            //result = HttpUtility.UrlDecode(result, Encoding.UTF8);
        }
        #endregion

        #region Qct Pay
        /// <summary>
        /// 购买者付款扫码（动态二维码）
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        [HttpPost]
        [SignVerify]
        public object Pay(PayBuyerScanDynaRequest reqModel)
        {
            //（对应融合支付：主扫支付动态）
            var result = QctPayReturn.Fail(msg: "参数格式错误");
            OrderBuilder<PayBuyerScanDynaRequest, PayBuyerScanDynaResponse> buyerOrder = new OrderBuilderForBuyerScanDyna();
            result = buyerOrder.Build(reqModel);
            return buyerOrder.Result(result);
        }
        /// <summary>
        /// 商家收款扫码
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        [HttpPost]
        [SignVerify]
        public object ScanPay(PayMerchScanRequest reqModel)
        {
            //（对应融合支付：被扫支付）
            var result = QctPayReturn.Fail(msg: "参数格式错误");
            OrderBuilder<PayMerchScanRequest, PayMerchScanResponse> merchOrder = new OrderBuilderForPayMerchScan();
            result = merchOrder.Build(reqModel);
            return merchOrder.Result(result);
        }
        /// <summary>
        /// 退款申请
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        [HttpPost]
        [SignVerify]
        public object Refund(RefundApplyRequest reqModel)
        {
            var result = QctPayReturn.Fail(msg: "参数格式错误");
            OrderBuilder<RefundApplyRequest, RefundApplyResponse> rfdOrder = new OrderBuilderForRefund();
            result = rfdOrder.Build(reqModel);
            return rfdOrder.Result(result);
        }

        #region old pay
        ///// <summary>
        ///// Qct支付接口调用（第三方随心付SxfPay）
        ///// </summary>
        ///// <param name="reqPms">请求参数</param>
        ///// <returns></returns>
        //[HttpPost]
        //[VerifySign]
        //public object QctPay([FromBody]JObject reqPms)
        //{
        //    var result = QctPayReturn.Fail(msg: "参数格式错误");
        //    String rstStrSxf = String.Empty;
        //    var sign = String.Empty;
        //    try
        //    {
        //        String method = reqPms.Property("method", true);
        //        {
        //            switch (method)
        //            {
        //                case PayConst.QCTTRADE_PAY_BUYERSCAN_DYNA: //购买者付款扫码（动态二维码）（对应融合支付：主扫支付动态）

        //                    var buyerOrderModel = reqPms.ToObject<PayBuyerScanDynaRequest>();
        //                    OrderBuilder<PayBuyerScanDynaRequest, PayBuyerScanDynaResponse> buyerOrder = new OrderBuilderForBuyerScanDyna();
        //                    result = buyerOrder.Build(buyerOrderModel);
        //                    return buyerOrder.Result(result);

        //                case PayConst.QCTTRADE_PAY_MERCHSCAN://商家收款扫码（对应融合支付：被扫支付）

        //                    var merchOrderModel = reqPms.ToObject<PayMerchScanRequest>();
        //                    OrderBuilder<PayMerchScanRequest, PayMerchScanResponse> merchOrder = new OrderBuilderForPayMerchScan();
        //                    result = merchOrder.Build(merchOrderModel);
        //                    return merchOrder.Result(result);

        //                case PayConst.QCTTRADE_REFUNDAPPLY://退款申请

        //                    var rfdOrderModel = reqPms.ToObject<RefundApplyRequest>();
        //                    OrderBuilder<RefundApplyRequest, RefundApplyResponse> rfdOrder = new OrderBuilderForRefund();
        //                    result = rfdOrder.Build(rfdOrderModel);
        //                    return rfdOrder.Result(result);

        //                default:
        //                    return QctPayReturn.Fail(msg: "[method]参数错误");
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LogEngine.WriteError("[QctPay参数Json解析失败]" + ex.Message, ex, LogModule.支付交易);
        //        result = QctPayReturn.Fail(msg: "参数格式错误");
        //        return result;
        //    }
        //}
        #endregion

        #endregion

        #region Qct PayNotify
        /// <summary>
        /// 支付结果后台通知
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        [HttpPost]
        [SignVerify(SignField="signature")]
        public object SxfPayNotify(SxfPayNotifyRequest reqModel)
        {
            try
            {
                if (reqModel.Type == PayConst.SXF_TYPE_PAYNOTIFY)
                {
                    NotifyBuilder<SxfPayNotifyRequest> payNotify = new NotifyBuilderForPay();
                    return payNotify.Build(reqModel);
                }
                else 
                    return SxfPayReturn.Fail(msg: "通知参数type值错误");
            }
            catch (WebException ex)
            {
                LogEngine.WriteError(string.Format("支付通知异常：{0}，请求参数：{1}", ex.Message, reqModel.ToJson()), ex, LogModule.支付交易);
                return SxfPayReturn.Fail(msg: "通知格式错误");
            }
        }
        /// <summary>
        /// 退款结果后台通知
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        [HttpPost]
        [SignVerify(SignField = "signature")]
        public object SxfRefundNotify(SxfRefundNotifyRequest reqModel)
        {
            try
            {
                //退款通知日志记录
                PayLogServer.WriteInfo(string.Format("退款通知：{0}", reqModel.ToJson()));

                if (reqModel.Type == PayConst.SXF_TYPE_RFDNOTIFY)
                {
                    NotifyBuilder<SxfRefundNotifyRequest> rfdNotify = new NotifyBuilderForRefund();
                    return rfdNotify.Build(reqModel);
                }
                else
                    return SxfPayReturn.Fail(msg: "通知参数type值错误");
            }
            catch (WebException ex)
            {
                LogEngine.WriteError(string.Format("退款通知异常：{0}，请求参数：{1}", ex.Message, reqModel.ToJson()), ex, LogModule.支付交易);
                return SxfPayReturn.Fail(msg: "通知格式错误");
            }
        }

        #endregion

        #region Qct PayQuery
        /// <summary>
        /// 单笔支付订单查询
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        [HttpPost]
        [SignVerify]        
        public object PayQuery(PayQueryRequest reqModel)
        {
            OrderBuilder<PayQueryRequest, PayQueryResponse> orderQuery = new OrderQueryForPay();
            var result = orderQuery.Query(reqModel);
            return Json(result);
        }

        /// <summary>
        /// 单笔退款订单查询
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        [HttpPost]
        [SignVerify]
        public object RefundQuery(RefundQueryRequest reqModel)
        {
            OrderBuilder<RefundQueryRequest, RefundQueryResponse> orderQuery = new OrderQueryForRefund();
            var result = orderQuery.Query(reqModel);
            return Json(result);
        }
        /// <summary>
        /// 支付订分页单查询
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        [HttpPost]
        [SignVerify]
        public object PayBatchQuery(PayBatchQueryRequest reqModel)
        {
            OrderBuilder<PayBatchQueryRequest, PayBatchQueryResponse> orderQuery = new OrderQueryForPayBatch();
            var result = orderQuery.Query(reqModel);
            return Json(result);
        }
        #endregion
    }
}
