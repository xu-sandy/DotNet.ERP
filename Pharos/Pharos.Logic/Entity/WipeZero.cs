// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有商品促销信息
// --------------------------------------------------

using System;
using System.Runtime.Serialization;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 商品促销
    /// </summary>
    [Serializable]

    public class WipeZero : SyncEntity
    {
        /// <summary>
        /// 支付编号
        /// </summary>
        public string PaySN { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Number { get; set; }

        //public int CompanyId { get; set; }

    }
}
