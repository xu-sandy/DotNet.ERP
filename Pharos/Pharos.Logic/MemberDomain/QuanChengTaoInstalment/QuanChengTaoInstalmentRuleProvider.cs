﻿using Pharos.Logic.BLL;
using Pharos.Logic.InstalmentDomain.Interfaces;
using Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment.InstalmentParameters;
using Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment.InstalmentRules;

namespace Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment
{
    public class QuanChengTaoInstalmentRuleProvider : BaseInstalmentRuleProvider<QuanChengTaoInstalmentRule, QuanChengTaoIntegralInstalment>
    {
        public override IInstalmentRule<QuanChengTaoIntegralInstalment> LoadRule(QuanChengTaoIntegralInstalment parameter)
        {
            var entity = ReturnStageRulesService.GetStageRuleByReturnRule(parameter.CompanyId, parameter.IntegralRuleId);
            if (entity == null)
            { return null; }
            var rule = new QuanChengTaoInstalmentRule() { RuleId = entity.Id.ToString(), Average = entity.Average, ReturnDT = entity.ReturnDT, ReturnType = entity.ReturnType };
            return rule;
        }

    }
}
