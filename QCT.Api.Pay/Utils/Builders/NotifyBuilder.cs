﻿using Pharos.Logic.OMS.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS;
using QCT.Pay.Common.Models;
using QCT.Pay.Common.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QCT.Pay.Common;

namespace QCT.Api.Pay.Utils
{
    /// <summary>
    /// 创建生成退款通知基础
    /// </summary>
    /// <typeparam name="TReqModel"></typeparam>
    public class NotifyBuilder<TReqModel>:BasePayBuilder
    {
        /// <summary>
        /// 创建生成退款通知
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public virtual SxfPayReturn Build(TReqModel reqModel)
        {
            return SxfPayReturn.Fail();
        }
        /// <summary>
        /// 回发通知给商家
        /// </summary>
        /// <param name="method"></param>
        /// <param name="dic"></param>
        /// <param name="cid"></param>
        /// <param name="notifyUrl"></param>
        /// <returns></returns>
        public SxfPayReturn SendPost(string method, Dictionary<string, object> dic, int cid, string notifyUrl)
        {
            var secretKey = (new PaySignService()).GetMerchSecretKeyByCID(cid);
            if (secretKey.IsNullOrEmpty())
            {
                LogEngine.WriteError(string.Format("发起后台通知请求错误：找不到SecretKey，接口参数名：{0}，通知URL：{1}，发送参数：{2}", method, notifyUrl, dic.ToJson()), null, LogModule.支付交易);
                return SxfPayReturn.Fail(msg: "数据接收失败");
            }
            else
            {
                //Qct签名
                dic = PaySignHelper.SetSign(dic, secretKey, "sign");
                try
                {
                    //给商家发送结果通知
                    var postResult = PayHelper.SendPost(notifyUrl, dic);
                    if (postResult.Successed)
                    {
                        var resultObj = JsonConvert.DeserializeObject<JObject>(postResult.Data.ToString());

                        if (resultObj.Property("return_code", true) == PayConst.SUCCESS_CODE)
                        {
                            return SxfPayReturn.Success();
                        }
                        else
                        {
                            return SxfPayReturn.Fail(msg: "数据接收失败");
                        }
                    }
                    else
                    {
                        return SxfPayReturn.Fail(msg: postResult.ReturnMsg);
                    }
                }
                catch (Exception ex)
                {
                    LogEngine.WriteError(string.Format("发起后台通知请求异常：商户通知Url无响应，接口参数名：{0}，通知URL：{1}]", method, notifyUrl), ex, LogModule.支付交易);
                    return SxfPayReturn.Fail(PayConst.FAIL_CODE_40004, "");
                }
            }
        }
    }
}