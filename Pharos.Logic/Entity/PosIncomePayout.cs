// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有门店POS机的出入款信息
// --------------------------------------------------

using System;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// POS出入款信息
    /// </summary>
    [Serializable]

    [Excel("POS出入款信息")]
    public class PosIncomePayout : SyncEntity
    {



        /// <summary>
        /// 门店ID
        /// [长度：3]
        /// [不允许为空]
        /// </summary>
        [Excel("门店ID", 1)]
        public string StoreId { get; set; }


        /// <summary>
        /// POS机号
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        [Excel("POS机号", 2)]
        public string MachineSN { get; set; }


        /// <summary>
        /// 收银员UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("收银员", 3)]
        public string CreateUID { get; set; }


        /// <summary>
        /// 类型（0:出款、1:入款） 
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("类型", 4)]
        public short Type { get; set; }


        /// <summary>
        /// 金额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("金额", 5)]
        public decimal Amount { get; set; }


        /// <summary>
        /// 时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [Excel("时间", 6)]
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 是否练习模式数据
        /// [不允许为空]
        /// [默认值：(0)]
        /// </summary>
        [Excel("时间", 6)]
        public bool IsTest { get; set; }

    }
}