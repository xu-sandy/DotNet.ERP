﻿using Newtonsoft.Json;
using Pharos.Logic.OMS.BLL;
using QCT.Pay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS.Models;

namespace Pharos.Logic.OMS
{
    public class PayRules
    {
        #region 计算费率
        /// <summary>
        /// 计算费率
        /// </summary>
        /// <param name="totalAmount">金额(元)</param>
        /// <param name="rate">费率%</param>
        /// <returns></returns>
        public static decimal CalcFee(decimal totalAmount, MerchantChannelModel merchObj)
        {
            var svc = new CommonPayService();
            var payChannelObj = svc.GetPayChannelDetail(merchObj);
            var monthTotalAmt = svc.GetMonthTotalTradeAmt(DateTime.Now, merchObj.MchId);
            decimal rst = payChannelObj.SingleServFeeLowLimit;
            var curMonthTotalAmt = monthTotalAmt + totalAmount;
            if (curMonthTotalAmt > payChannelObj.MonthFreeTradeAmount)
            { //计算费率
                var freeAmt = totalAmount;
                if (monthTotalAmt < payChannelObj.MonthFreeTradeAmount)
                {
                    freeAmt = curMonthTotalAmt - payChannelObj.MonthFreeTradeAmount;
                }
                if (freeAmt > 0)
                {
                    decimal per = 0.01m;
                    rst = freeAmt * payChannelObj.OverServiceRate * per;
                    if (rst < payChannelObj.SingleServFeeLowLimit)
                        rst = payChannelObj.SingleServFeeLowLimit;
                    if (rst > payChannelObj.SingleServFeeUpLimit)
                        rst = payChannelObj.SingleServFeeUpLimit;
                    return rst;
                }
                return rst;
            }
            return rst;
        }
        #endregion

        #region 基础
        /// <summary>
        /// 获取对应表最大编号
        /// </summary>
        /// <param name="tbName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static int GetMaxNo(string tbName, string fieldName)
        {
            var svc = new CommonPayService();
            return svc.GetMaxNo(tbName, fieldName);
        }
        /// <summary>
        /// 根据商户及门店信息获取门店支付静态二维码地址
        /// </summary>
        /// <param name="mch_id"></param>
        /// <param name="store_id"></param>
        /// <returns></returns>
        public static string GetPayQRCode(int mch_id, string store_id) {

            var baseUrl = Pharos.Utility.Config.GetAppSettings("StoreQRCodeUrl");
            string url = string.Format(baseUrl + "?mch_id={0}&store_id={1}", mch_id.ToString(), store_id);
            return url;
        }
        #endregion

        #region 过期-Sign
        ///// <summary>
        ///// 签名（Qct Sxf）
        ///// </summary>
        ///// <param name="signObj"></param>
        ///// <param name="secretKey"></param>
        ///// <returns></returns>
        //public static string Sign(Dictionary<String, Object> signObj, String secretKey)
        //{
        //    StringBuilder signdatasb = new StringBuilder();
        //    foreach (var item in signObj)
        //    {
        //        String key = item.Key;
        //        String value = signObj[key] == null ? "" : signObj[key].ToString();
        //        signdatasb.Append("&").Append(key).Append("=").Append(value);
        //    }

        //    String signdata = signdatasb.ToString().Substring(1) + "&key=" + secretKey;
        //    return PayHelper.GetMD5(signdata);
        //}
        ///// <summary>
        ///// 验证签名的有效性
        ///// </summary>
        ///// <param name="queryStr"></param>
        ///// <returns></returns>
        //public static bool VerifySign(Dictionary<string, object> signObj, string md5Key, string sign)
        //{
        //    var thisSign = Sign(signObj, md5Key);
        //    if (thisSign.Equals(sign, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    else
        //        return false;
        //}
        ///// <summary>
        ///// 验证签名的有效性
        ///// </summary>
        ///// <param name="content"></param>
        ///// <returns></returns>
        //public static bool VerifySign(string content)
        //{
        //    var signObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
        //    var sign = signObj["sign"];
        //    signObj.Remove("sign");
        //    var secretKey = (new CommonPayService()).GetMerchSecretKeyByID(signObj["mch_id"].ToType<int>());
        //    var result = PayRules.VerifySign(signObj, secretKey, sign.ToString());
        //    return result;
        //}
        #endregion
    }
}