﻿using Pharos.Logic.ApiData.Pos.Sale.Members;

namespace Pharos.Api.Retailing.Models.Pos
{
    /// <summary>
    /// 设置会员参数
    /// </summary>
    public class SetMemberRequest : BaseApiParams
    {
        /// <summary>
        /// 去向(当前默认为0[内部])
        /// </summary>
        public MembersSourseMode To { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityId { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }

    }


    public class SetActivityRequest : BaseApiParams
    {
        public int ActivityId { get; set; }

    }
}