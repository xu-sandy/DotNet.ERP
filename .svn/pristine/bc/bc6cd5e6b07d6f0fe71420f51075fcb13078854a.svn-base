using Pharos.Logic.MemberDomain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Pharos.Logic.MemberDomain.QuanChengTaoProviders
{
    public class QuanChengTaoIntegralRuleFlowProvider : IIntegralRuleFlowProvider
    {
        public QuanChengTaoIntegralRuleFlowProvider(int companyId, IRound rounder)
        {
            CompanyId = companyId;
            Rounder = rounder;
        }

        public int CompanyId { get; private set; }
        public IRound Rounder { get; private set; }
        public IDictionary<IIntegralRule, decimal> DoFlow<TMember>(object channelMessage, IIntegralRuleProvider ruleProvider, TMember member)
        {
            ruleProvider.RuleRound = Rounder;
            var allRules = ruleProvider.GetRules(CompanyId);
            var result = new Dictionary<IIntegralRule, decimal>();
            foreach (var rule in allRules)
            {
                var scene = ruleProvider.GetScene(channelMessage, rule, member);
                if (scene == null)
                    continue;
                var efficientRule = ruleProvider.VerifyRule(rule, scene);
                if (efficientRule == null)
                    continue;
                var integral = ruleProvider.RunExpression(efficientRule, scene);
                result.Add(integral.Key, integral.Value);
            }
            return result;
        }
    }
}
