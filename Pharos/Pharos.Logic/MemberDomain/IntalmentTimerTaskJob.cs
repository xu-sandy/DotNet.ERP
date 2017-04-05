﻿using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.Logic.InstalmentDomain
{
    public class IntalmentTimerTaskJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Task.Factory.StartNew(() => { RefreshReturnRules(); });
            Task.Factory.StartNew(() => { IntalmentTimeOut(); });
            Task.Factory.StartNew(() => { RefreshMembershipCardState(); });
        }
        /// <summary>
        /// 积分分期
        /// </summary>
        private void IntalmentTimeOut()
        {
            var date = DateTime.Now.Date;
            var list = BaseService<InstalmentRecord>.FindList(o => o.InstalmentDT <= date && o.State == 0);
            var memberInstalments = list.GroupBy(o => o.MemberId);
            var memberids = memberInstalments.Select(o => o.Key).ToList();
            var members = MembersService.FindList(o => memberids.Any(p => p == o.MemberId));
            foreach (var item in members)
            {
                var memberDatas = memberInstalments.FirstOrDefault(o => o.Key == item.MemberId);
                item.UsableIntegral += memberDatas.Sum(o => o.Integral);
                foreach (var instalment in memberDatas)
                {
                    instalment.State = 1;
                }
            }
            MembersService.CurrentRepository._context.SaveChanges();
        }
        /// <summary>
        /// 定时刷新返赠规则
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private void RefreshReturnRules()
        {
            var datas = ReturnRulesService.GetAllActivingReturnRule();
            if (datas == null || datas.Count() == 0)
            {
                return;
            }
            foreach (var item in datas)
            {
                if (item.State == 0 && item.ExpiryStart < DateTime.Now
                    && ((item.ExpiryEnd.HasValue && DateTime.Now < item.ExpiryEnd) || (!item.ExpiryEnd.HasValue))
                    )
                {//未开始 
                    ReturnRulesService.UpdateReturnRulesState(1, item.Id.ToString());
                }
                else if (item.State == 1 && item.ExpiryEnd.HasValue && item.ExpiryEnd <= DateTime.Now)
                { //活动中
                    ReturnRulesService.UpdateReturnRulesState(2, item.Id.ToString());
                }
            }
        }
        /// <summary>
        /// 刷新会员卡状态
        /// </summary>
        private void RefreshMembershipCardState()
        {
            var datas = MembershipCardService.GetMembershipListByState();
            if (datas == null || datas.Count() == 0)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in datas)
            {
                var currentDate = DateTime.Now;
                if (currentDate > item.ExpiryEnd)
                {
                    sb.Append("," + item.Id);
                }
            }

            MembershipCardService.UpdateState(5, sb.ToString());//设置过期
        }
    }
}
