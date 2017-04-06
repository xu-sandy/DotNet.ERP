using Newtonsoft.Json;
using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using QCT.Pay.Common.Helpers;
using Newtonsoft.Json.Linq;
using QCT.Pay.Common;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// 支付签名Service
    /// </summary>
    public class PaySignService
    {
        IBaseRepository<TradersPaySecretKey> TPSKRepost { get; set; }
        /// <summary>
        /// 签名验证（支持Qct、Sxf）
        /// </summary>
        /// <param name="signDic"></param>
        /// <param name="signField"></param>
        /// <returns></returns>
        public bool VerifySign(Dictionary<string, object> signDic,string signField)
        {
            var secretKey = string.Empty;
            if (signField == PayConst.SXF_SIGNATUREFIELD)
            {
                secretKey = GetMerchSecretKey3ByMerchID(signDic["merchantId"].ToString());
            }
            else
            {
                secretKey = GetMerchSecretKeyByCID(signDic["mch_id"].ToType<int>());
            }

            var result = PaySignHelper.VerifySign(signDic, secretKey, signField);
            return result;
        }

        /// <summary>
        /// 根据商户ID获取商户Key
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public string GetMerchSecretKeyByCID(int cid)
        {
            TPSKRepost = new BaseRepository<TradersPaySecretKey>();
            var query = TPSKRepost.GetQuery(o => o.CID == cid && o.State == (short)TraderPayCchannelState.Enabled);
            var obj = query.FirstOrDefault();
            if (obj != null)
                return obj.SecretKey;
            else
                return string.Empty;
        }
        /// <summary>
        /// 根据第三方商户ID获取第三方商户SecretKey
        /// </summary>
        /// <param name="merchId"></param>
        /// <returns></returns>
        public string GetMerchSecretKey3ByMerchID(string merchId)
        {
            TPSKRepost = new BaseRepository<TradersPaySecretKey>();
            var query = TPSKRepost.GetQuery(o => o.MchId3 == merchId && o.State == (short)TraderPayCchannelState.Enabled);
            var obj = query.FirstOrDefault();
            if (obj != null)
                return obj.SecretKey3;
            else
                return string.Empty;
        }
    }
}
