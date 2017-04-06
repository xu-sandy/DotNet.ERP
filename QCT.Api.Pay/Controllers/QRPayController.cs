using Newtonsoft.Json.Linq;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Models;
using QCT.Api.Pay.Utils;
using QCT.Pay.Common;
using QCT.Pay.Common.Helpers;
using QCT.Pay.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;

namespace QCT.Api.Pay.Controllers
{
    public class QRPayController : Controller
    {
        //
        // GET: /QRPay/
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult QROrder(int mch_id = 0, string store_id = "")
        {
            var paySvc = new PayService();
            var smodel = paySvc.GetMerchStore(mch_id, store_id);
            return View(smodel);
        }
        /// <summary>
        /// 台卡支付提交 fishtodo:待增加返回错误页面
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        [HttpPost]
        public object SubmitQROrder(PayBuyerScanStaticRequest reqModel)
        {
            var errMsg = PayTradeHelper.TryValidateObject(reqModel);
            if (string.IsNullOrWhiteSpace(errMsg))
            {
                //取得商户当前系统时间
                var orderDate = DateTime.Now;
                //商户订单号
                String orderId = OrderHelper.GetMaxOutOrderNo();
                //商户订单日期

                var reqObj = new PayBuyerScanDynaRequest();
                reqObj.Mch_Id = reqModel.Mch_Id;
                reqObj.Store_Id = reqModel.Store_Id;
                reqObj.Device_Id = "";
                reqObj.Method = PayConst.QCTTRADE_PAY_QRORDER;
                reqObj.Charset = PayConst.DEF_CHARSET;
                reqObj.Sign_Type = PayConst.DEF_SIGNTYPE;
                reqObj.Version = PayConst.DEF_VERSION;
                reqObj.Out_Trade_No = orderId;
                reqObj.Create_Date = orderDate;
                reqObj.Total_Amount = reqModel.Total_Amount;
                reqObj.Pay_Notify_Url = "";
                reqObj.Buyer_Mobile = "";
                reqObj.Goods_Name = "购物消费";
                reqObj.Goods_Desc = reqModel.Goods_Desc;
                reqObj.Sign = "nosignrequest";

                OrderBuilder<PayBuyerScanDynaRequest, PayBuyerScanDynaResponse> buyerOrder = new OrderBuilderForBuyerScanDyna();
                var result = buyerOrder.Build(reqObj);
                if (!result.Successed)
                {
                    return View("PayError");
                }
                else
                {
                    Response.Redirect(buyerOrder.RspModel.Pay_Token);
                    return View();
                    //return Json(buyerOrder.RspModel.Pay_Token);
                }
            }
            else
            {
                return View("PayError", QctPayReturn.Fail(msg:errMsg));
            }
        }
    }
}