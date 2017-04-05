﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class MemberInfo
    {
        public string MemberId { get; set; }
        /// <summary>
        /// 手机号（全局唯一）
        /// [长度：11]
        /// </summary>
        public string MobilePhone { get; set; }
        /// <summary>
        /// 会员卡号（全局唯一）
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        public string MemberCardNum { get; set; }
        /// <summary>
        /// 会员姓名
        /// [长度：20]
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 可用积分
        /// [长度：10]
        /// [默认值：((0))]
        /// </summary>
        public decimal UsableIntegral { get; set; }
        /// <summary>
        /// 会员类型
        /// </summary>
        public Pharos.ObjectModels.DTOs.CustomerType Type { get; set; }


        public decimal UsableAmount { get; set; }

        public short Sex { get; set; }



        public string ZhiFuBao { get; set; }


        public string WeiXin { get; set; }


        public string Email { get; set; }

        public IEnumerable<MemberCard> MemberCards { get; set; }
        public string Birthday { get; set; }
        public string Address { get; set; }

        public string RecordId { get; set; }
    }

    public class MemberCard
    {
        public string CardNo { get; set; }
        public decimal Amount { get; set; }
        public string State { get; set; }
    }
}
