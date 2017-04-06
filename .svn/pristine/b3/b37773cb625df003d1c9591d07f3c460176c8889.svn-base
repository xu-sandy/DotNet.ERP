// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：CYB
// 创建时间：2016-09-05
// 描述信息：生成优惠券信息
// --------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 用于管理本系统的所有生成优惠券信息 
    /// </summary>
    [Serializable]
    public class CouponCardDetail
    {
        /// <summary>
        /// 记录 ID 
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 公司CID
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 制卡批次 
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string BatchSN { get; set; }

        /// <summary>
        /// 券号 
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string TicketNo { get; set; }

        /// <summary>
        /// 防伪码
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string SecurityCode { get; set; }

        /// <summary>
        /// 创建时间
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 创建人
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }

        /// <summary>
        /// 状态（0：未派发；1：未使用[派发申领后]；2：已使用[门店回收后]；3：已过期；）
        /// [不允许为空]
        /// [默认值：0)]
        /// [不允许为空]
        /// </summary>
        public short State { get; set; }

        /// <summary>
        /// 派发申领时间
        /// [允许为空]
        /// </summary>
        public DateTime? ReceiveDT { get; set; }

        /// <summary>
        /// 派发申领人
        /// [长度：40]
        /// [允许为空]
        /// </summary>
        public string Recipients { get; set; }

        /// <summary>
        /// 来源，即优惠券使用门店
        /// [长度：3]
        /// [允许为空]
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 使用时间
        /// [允许为空]
        /// </summary>
        public DateTime? UseDT { get; set; }

        /// <summary>
        /// 使用人
        /// [长度：40]
        /// [允许为空]
        /// </summary>
        public string UsePerson { get; set; }

    }
}
