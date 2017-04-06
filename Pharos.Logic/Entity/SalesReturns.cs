// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有门店的商品售后退换信息
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices;
using System;
using System.Runtime.Serialization;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 售后退换信息
    /// </summary>
    [Serializable]

    [Excel("售后退换信息")]
    public class SalesReturns:BaseEntity
    {
        

        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }
        

        /// <summary>
        /// 退换方式（0:退货、1:换货、2、整单退出、3、修改订单）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("退换方式", 1)]
        public short ReturnType { get; set; }
        

        /// <summary>
        /// 退换货ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("退换货ID", 2)]
        public string ReturnId { get; set; }
        

        /// <summary>
        /// 新单据号
        /// </summary>
        [Excel("新单据号", 4)]
        public string NewPaySN { get; set; }
        

        /// <summary>
        /// 门店ID
        /// [长度：3]
        /// [不允许为空]
        /// </summary>
        [Excel("门店ID", 3)]
        public string StoreId { get; set; }
        

        /// <summary>
        /// 退换理由ID（来自数据字典）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("退换理由ID", 5)]
        public int ReasonId { get; set; }
        

        /// <summary>
        /// 退换单价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        [Excel("退换差价", 6)]
        public decimal ReturnPrice { get; set; }
        

        /// <summary>
        /// 退换时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [Excel("退换时间", 7)]
        public DateTime CreateDT { get; set; }
        

        /// <summary>
        /// 经办人UID 
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("经办人UID", 8)]
        public string CreateUID { get; set; }
        

        /// <summary>
        /// 状态 - 1、处理中（网购预留）；2、已完成
        /// </summary>
        [Excel("状态", 9)]
        public short State { get; set; }

        /// <summary>
        /// POS机号
        /// 后台退换标识为“-1”
        /// </summary>
        public string MachineSN { get; set; }

        /// <summary>
        /// 来源（0:POS,1:SERVER）
        /// </summary>
        public int Source { get; set; }
    }
}
