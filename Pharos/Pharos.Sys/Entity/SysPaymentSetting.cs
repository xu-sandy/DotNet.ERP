﻿using Pharos.Utility;
// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：陈雅宾
// 创建时间：2016-01-29
// 描述信息：
// --------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pharos.Sys.Entity
{
    /// <summary>
    /// 用于管理本系统的支付配置信息
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public class SysPaymentSetting : BaseEntity
    {
        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        //[DataMember]
       // public int Id { get; set; }

        /// <summary>
        /// 适用门店（多个以逗号隔开，0表示全部）
        /// [长度：300]
        /// </summary>
        [DataMember]
        public string StoreId { get; set; }

        /// <summary>
        /// 状态（0:停用、1:可用）
        /// </summary>
        [DataMember]
        public short State { get; set; }

        /// <summary>
        /// 支付方式（1:支付宝,2:微信）
        /// </summary>
        [DataMember]
        public short PayType { get; set; }

        /// <summary>
        /// 支付宝：合作身份者ID/微信：mchID
        /// [长度：200]
        /// </summary>
        [DataMember]
        public string PartnerId { get; set; }

        /// <summary>
        /// 支付宝：使用当面付编码/微信：appID
        /// [长度：200]
        /// </summary>
        [DataMember]
        public string AppId { get; set; }

        /// <summary>
        /// 支付宝：交易安全检验码/微信：支付密钥
        /// [长度：200]
        /// </summary>
        [DataMember]
        public string CheckKey { get; set; }

        /// <summary>
        /// 支付通知页面
        /// [长度：200]
        /// </summary>
        [DataMember]
        public string NotifyUrl { get; set; }
        [DataMember]
        public string Memo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 创建人ID
        /// [长度：40]
        /// </summary>
        [DataMember]
        public string CreateUID { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
        public DateTime? AlterDT { get; set; }

        /// <summary>
        /// 修改人ID
        /// [长度：40]
        /// </summary>
        [DataMember]
        public string AlterUID { get; set; }

    }
}
