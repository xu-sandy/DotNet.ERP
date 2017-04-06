using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Utility;

namespace Pharos.Logic.BLL
{
    public class ReturnStageRulesBLL
    {
        private readonly ReturnStageRulesService _service = new ReturnStageRulesService();
        /// <summary>
        /// 根据赠送规则找分期记录
        /// </summary>
        /// <param name="returnRuleId"></param>
        /// <returns></returns>
        public ReturnStageRules GetStageRuleByReturnRuleId(int returnRuleId)
        {
            return _service.GetStageRuleByReturnRuleId(returnRuleId);
        }
        /// <summary>
        /// 新增或编辑一条分期记录
        /// </summary>
        /// <returns></returns>
        public OpResult CreateOrUpdateStageRule(ReturnStageRules _stageRule)
        {
            var entity = _service.GetStageRuleByReturnRuleId(_stageRule.ReturnRuleId);
            if (entity.Id == 0)
            {//新建
                _stageRule.CompanyId = CommonService.CompanyId;
                _stageRule.CreateDT = DateTime.Now;
                _stageRule.CreateUID = Sys.CurrentUser.UID;
                return BaseService<ReturnStageRules>.Add(_stageRule);
            }
            else
            { //编辑
                return _service.UpdateStageRule(_stageRule);
            }
        }
    }
}
