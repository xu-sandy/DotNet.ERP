using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Utility;

namespace Pharos.Logic.BLL
{
    public class ReturnRulesBLL
    {
        private readonly ReturnRulesService _service = new ReturnRulesService();
        public object FindReturnRulesList(out int count)
        {
            return _service.FindReturnRulesList(out count);
        }
        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public OpResult UpdateReturnRulesState(int state, string ids)
        {
            return ReturnRulesService.UpdateReturnRulesState(state, ids);
        }
        /// <summary>
        /// 新增一条规则
        /// </summary>
        /// <param name="_rule"></param>
        /// <returns></returns>
        public OpResult CreateReturnRule(ReturnRules _rule)
        {
            _rule.CompanyId = CommonService.CompanyId;
            _rule.CreateDT = DateTime.Now;
            _rule.CreateUID = Sys.CurrentUser.UID;
            return BaseService<ReturnRules>.Add(_rule);
        }
        /// <summary>
        /// 修改返赠方案
        /// </summary>
        /// <param name="_rule"></param>
        /// <returns></returns>
        public OpResult UpdateReturnRule(ReturnRules _rule)
        {
            return _service.UpdateReturnRule(_rule);
        }
        /// <summary>
        /// 根据id查找1条规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReturnRules FindReturnRuleById(int id)
        {
            return BaseService<ReturnRules>.FindById(id);
        }
    }
}
