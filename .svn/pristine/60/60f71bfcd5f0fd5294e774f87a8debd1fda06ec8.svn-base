// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有会员的消费积分明细信息（主表：SaleOrders） 
// --------------------------------------------------

using Newtonsoft.Json;
using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.LocalServices;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;

namespace Pharos.Logic.LocalEntity
{
    [Excel("会员积分明细")]

    /// <summary>
    /// 会员积分明细
    /// </summary>
    public class MemberIntegral : BaseEntity, ICanUploadEntity
    {
        public Int64 Id { get; set; }
        [Excel("流水号", 1)]

        [LocalKey]
        /// <summary>
        /// 流水号
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string PaySN { get; set; }
        [Excel("会员卡号", 2)]

        [LocalKey]
        /// <summary>
        /// 会员卡号
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        public string MemberId { get; set; }
        [Excel("消费金额", 3)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###消费金额格式错误")]

        /// <summary>
        /// 消费金额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal ActualPrice { get; set; }
        [Excel("兑换积分", 4)]

        /// <summary>
        /// 兑换积分
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public int Integral { get; set; }
        [Excel("消费时间", 5)]

        /// <summary>
        /// 消费时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }
        [JsonIgnore]

        public bool IsUpload { get; set; }
    }
}
