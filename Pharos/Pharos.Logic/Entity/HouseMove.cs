﻿// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：陈雅宾
// 创建时间：2015-12-25
// 描述信息：用于管理本系统的所有店内的货品调拨基本信息
// --------------------------------------------------
using Newtonsoft.Json;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 货品调拨信息
    /// </summary>
    [Serializable]
    public partial class HouseMove:BaseEntity
    {
        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 调拨单号 ID
        /// [不允许为空]
        /// </summary>
        public string MoveId { get; set; }

        /// <summary>
        /// 调出分店ID
        /// [长度：3]
        /// [不允许为空]
        /// </summary>
        public string OutStoreId { get; set; }

        /// <summary>
        /// 调入分店ID
        /// [长度：3]
        /// [不允许为空]
        /// </summary>
        public string InStoreId { get; set; }
        
        /// <summary>
        /// 申请时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [JsonConverter(typeof(JsonShortDate))]
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 申请人UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }

        /// <summary>
        /// 配送人UID
        /// [长度：40]
        /// </summary>
        public string DeliveryUID { get; set; }

        /// <summary>
        /// 调出时间
        /// [长度：23，小数位数：3]
        /// [允许为空]
        /// </summary>
        [JsonConverter(typeof(JsonShortDate))]
        public DateTime? DeliveryDT { get; set; }

        /// <summary>
        /// 收货人UID
        /// [长度：40]
        /// </summary>
        public string ActualUID { get; set; }

        /// <summary>
        /// 收货时间
        /// [长度：23，小数位数：3]
        /// </summary>
        [JsonConverter(typeof(JsonShortDate))]
        public DateTime? ActualDT { get; set; }

        /// <summary>
        /// 状态（1:调拨中、2:已配送、3:已撤回、4:已收货）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public short State { get; set; }

        /// <summary>
        /// 退回请求原因（来自字典）
        /// [长度：10]
        /// [允许为空]
        /// </summary>
        public int? ReasonId { get; set; }


    }
}