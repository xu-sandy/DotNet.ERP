using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Utility;

namespace Pharos.Logic.BLL
{
    public class IntegralRuleBLL
    {
        IntegralRuleService _service = new IntegralRuleService();
        public OpResult CreateIntegralRule(IntegralRule rule)
        {
            if (rule.Id == 0)
            {
                rule.CreateDT = DateTime.Now;
                rule.CreateUID = Sys.CurrentUser.UID;
                rule.CompanyId = CommonService.CompanyId;
                if (string.IsNullOrEmpty(rule.ExpiryStart))
                {
                    rule.ExpiryStart = DateTime.Now.ToString("yyyy-MMM-dd");
                }
                //将原有同等级规则设为失效
                var _orule = _service.GetActivingIntergralRuleByMemberLevel(rule.UseUsers);
                if (_orule != null && _orule.Count() > 0)
                {
                    foreach (var item in _orule)
                    {
                        _service.UpdateIntegralRule(item.Id.ToString(), 1);
                    }
                }
                //保存数据
                return _service.CreateIntegralRule(rule);
            }
            else
            {
                return _service.UpdateIntegralRule(rule);
            }
        }

        public object FindIntegralRuleList(short ruleTypeId, string value, out int count)
        {
            return _service.FindIntegralRuleList(ruleTypeId, value, out count);
        }

        public IntegralRule GetIntegralRuleById(int id)
        {
            return _service.GetIntegralRuleById(id);
        }

        public OpResult UpdateIntegralRule(string ids, short state)
        {
            return _service.UpdateIntegralRule(ids, state);
        }

        public IntegralRule FindIntegralRuleById(int id)
        {
            return IntegralRuleService.FindById(id);
        }

        public IEnumerable<IntegralRule> GetActivingIntergralRuleByMemberLevel(string memberLevel)
        {
            return _service.GetActivingIntergralRuleByMemberLevel(memberLevel);
        }
    }
}
