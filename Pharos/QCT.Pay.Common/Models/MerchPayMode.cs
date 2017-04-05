﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QCT.Pay.Common;
using Newtonsoft.Json;

namespace QCT.Pay.Common.Models
{
    /// <summary>
    /// 商户支付列表
    /// </summary>
    [Serializable]
    public class MerchPayMode
    {
        /// <summary>
        /// 商户号
        /// </summary>
        [JsonProperty("mch_id")]
        public int CID { get; set; }
        /// <summary>
        /// 商户支付列表
        /// </summary>
        [JsonProperty("items")]
        public List<MerchPayModeItem> Items { get; set; }
    }
    /// <summary>
    /// 支付方式Model
    /// </summary>
    [Serializable]
    public class MerchPayModeItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 渠道编号
        /// </summary>
        [JsonProperty("channel_no")]
        public int ChannelNo { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        [JsonProperty("pay_mode")]
        public int PayMode { get; set; }
        /// <summary>
        /// 支付方式状态
        /// </summary>
        [JsonProperty("state")]
        public DataItemState State { get; set; }

    }

    public class MerchStorePayModel
    {
        /// <summary>
        /// 商户号
        /// </summary>
        [JsonProperty("mch_id")]
        public int CID { get; set; }
        /// <summary>
        /// 商户支付列表
        /// </summary>
        [JsonProperty("items")]
        public List<MerchStorePayItem> Items { get; set; }
    }
    public class MerchStorePayItem
    {
        /// <summary>
        /// 门店号
        /// </summary>
        [JsonProperty("store_id")]
        public string StoreId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty("state")]
        public DataItemState State { get; set; }
    }
}