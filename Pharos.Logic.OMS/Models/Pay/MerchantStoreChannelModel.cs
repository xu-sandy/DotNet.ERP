using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 商户门店信息Model
    /// </summary>
    [Serializable]
    public class MerchantStoreChannelModel : MerchantChannelModel
    {
        /// <summary>
        /// 
        /// </summary>
        public MerchantStoreChannelModel() { }
        /// <summary>
        /// 构造商户基本信息
        /// </summary>
        /// <param name="model"></param>
        public MerchantStoreChannelModel(MerchantChannelModel model)
        {
            MchId = model.MchId;
            SecretKey = model.SecretKey;
            MerchId3 = model.MerchId3;
            SecretKey3 = model.SecretKey3;
            SourceType = model.SourceType;
            ChannelNo = model.ChannelNo;
            PayNotifyUrl = model.PayNotifyUrl;
            RfdNotifyUrl = model.RfdNotifyUrl;
            ApiNo = model.ApiNo;
            ApiUrl = model.ApiUrl;
            Method = model.Method;
            ChannelPayMode = model.ChannelPayMode;
            OptType = model.OptType;
        }
        /// <summary>
        /// 商户门店ID
        /// </summary>
        public string SID { get; set; }
        /// <summary>
        /// 商户第三方门店ID
        /// </summary>
        public string StoreId3 { get; set; }
    }
}
