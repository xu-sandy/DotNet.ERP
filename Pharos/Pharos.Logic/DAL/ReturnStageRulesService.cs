﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Utility;

namespace Pharos.Logic.BLL
{
    public class ReturnStageRulesService : BaseService<ReturnStageRules>
    {

        public ReturnStageRules GetStageRuleByReturnRuleId(int returnRuleId)
        {
            var result = CurrentRepository.Entities.FirstOrDefault(o => o.CompanyId == CommonService.CompanyId && o.ReturnRuleId == returnRuleId);
            if (result == null)
            {
                result = new ReturnStageRules();
                var returnRuleEntity = BaseService<ReturnRules>.CurrentRepository.Entities.FirstOrDefault(o => o.Id == returnRuleId);
                result.Project = returnRuleEntity.GivenType;
            }
            return result;
        }

        public OpResult UpdateStageRule(ReturnStageRules _stageRule)
        {
            var entity = CurrentRepository.Entities.FirstOrDefault(o => o.ReturnRuleId == _stageRule.ReturnRuleId && o.CompanyId == CommonService.CompanyId);
            if (entity == null)
            {
                return OpResult.Fail("未找到原数据！");
            }
            entity.ReturnDT = _stageRule.ReturnDT;
            entity.State = _stageRule.State;
            entity.Average = _stageRule.Average;
            var result = CurrentRepository.Update(entity);
            if (result)
            {
                return OpResult.Success("操作成功！");
            }
            else
            {
                return OpResult.Fail("请核对数据！");
            }
        }

        public static ReturnStageRules GetStageRuleByReturnRule(int companyId, int integralId)
        {
            return CurrentRepository.Entities.FirstOrDefault(o => o.CompanyId == companyId && o.ReturnRuleId == integralId && o.State == 0);
        }
    }
}
