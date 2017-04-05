﻿using Pharos.Logic.MemberDomain.Exceptions;
using Pharos.Logic.MemberDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.MemberDomain.QuanChengTaoProviders
{
    /// <summary>
    /// 全城淘积分规则工厂
    /// 【余雄文】
    /// </summary>
    public class QuanChengTaoIntegralRuleFactory : IIntegralRuleFactory
    {
        public QuanChengTaoIntegralRuleFactory(int companyId)
        {
            CompanyId = companyId;
        }
        public int CompanyId { get; private set; }
        public IEnumerable<int> GetIntegralRuleProviderIds(object info = null)
        {
            return Pharos.Logic.BLL.ReturnRulesService.GetIntegralRuleProviderIds((int)info);
        }

        public IEnumerable<IIntegralRuleProvider> CreateRuleProviders(IIntegralRuleProviderLoader ruleProviderLoader)
        {
            IEnumerable<IIntegralRuleProvider> outRules = null;
            var providerIds = GetIntegralRuleProviderIds(CompanyId);
            if (!ruleProviderLoader.TryLoadIntegralRuleProviders(providerIds, out outRules))
            {
                throw new IntegralRuleProviderLoadException("加载积分规则处理程序失败！");
            }
            return outRules;
        }
    }
}
