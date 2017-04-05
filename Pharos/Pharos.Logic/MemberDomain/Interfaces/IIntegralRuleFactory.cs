﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.MemberDomain.Interfaces
{
    /// <summary>
    /// 积分规则工厂
    /// 【余雄文】
    /// </summary>
    public interface IIntegralRuleFactory
    {
        /// <summary>
        /// 获取生效的积分规则提供程序ID
        /// </summary>
        /// <param name="info">获取生效积分规则提供程序ID可能参数</param>
        /// <returns>生效的积分提供程序id</returns>
        IEnumerable<int> GetIntegralRuleProviderIds(object parameter = null);
        /// <summary>
        /// 创建规则提供程序
        /// </summary>
        /// <param name="ruleProviderLoader">规则提供程序加载器</param>
        /// <returns>当前所有的积分规则提供程序</returns>
        IEnumerable<IIntegralRuleProvider> CreateRuleProviders(IIntegralRuleProviderLoader ruleProviderLoader);
    }
}
