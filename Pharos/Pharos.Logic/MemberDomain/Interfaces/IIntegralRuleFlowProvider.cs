﻿using System.Collections.Generic;

namespace Pharos.Logic.MemberDomain.Interfaces
{
    /// <summary>
    /// 积分提供程序完整流程操作
    /// 【余雄文】
    /// </summary>
    public interface IIntegralRuleFlowProvider
    {
        /// <summary>
        /// 积分提供程序完整流程操作
        /// </summary>
        /// <param name="ruleProvider">规则提供程序</param>
        IDictionary<IIntegralRule, decimal> DoFlow<TMember>(object channelMessage, IIntegralRuleProvider ruleProvider, TMember member);

    }
}
