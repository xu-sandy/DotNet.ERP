﻿// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：蔡少发
// 创建时间：2016-09-03
// 描述信息：
// --------------------------------------------------

using System;

namespace Pharos.Logic.OMS.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Devices
    {
        /// <summary>
        /// 记录 ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 设备档案 GUID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 设备分类（取字典表）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short CategoryId { get; set; }

        /// <summary>
        /// 设备名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 品牌
        /// [长度：50]
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 型号
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 状态(0-停用 1-可用)
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：(1)]
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(Pharos.Utility.JsonShortDate))]
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 创建人 UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }
    }
}
