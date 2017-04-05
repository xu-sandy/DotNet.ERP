using Pharos.Logic.InstalmentDomain.Interfaces;
using Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment.InstalmentItems;
using Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment.InstalmentParameters;
using System;
using System.Collections.Generic;

namespace Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment.InstalmentRules
{
    public class QuanChengTaoInstalmentRule : IInstalmentRule<QuanChengTaoIntegralInstalment>
    {
        public string RuleId { get; set; }
        /// <summary>
        /// 返还时间
        /// </summary>
        public string ReturnDT { get; set; }
        /// <summary>
        /// 返还类型（0：按月返）
        /// </summary>
        public short ReturnType { get; set; }
        /// <summary>
        /// 每期返还金额
        /// </summary>
        public decimal Average { get; set; }
        public IEnumerable<IInstalmentItem> GetInstalmentItems(QuanChengTaoIntegralInstalment parameter)
        {
            var instalmentList = new List<IInstalmentItem>();
            var integral = parameter.Integral;
            var index = 0;
            while (integral > 0)
            {
                var date = DateTime.Now.AddMonths(index + 1);
                date = new DateTime(date.Year, date.Month, Convert.ToInt32(ReturnDT));
                if (integral > Average)
                {
                    instalmentList.Add(new QuanChengTaoInstalmentItem() { IntegralRecordId = parameter.IntegralRecordId, InstalmentNumber = Average, InstalmentRuleId = RuleId, InstalmentDT = date });
                    integral = integral - Average;
                }
                else
                {
                    instalmentList.Add(new QuanChengTaoInstalmentItem() { IntegralRecordId = parameter.IntegralRecordId, InstalmentNumber = integral, InstalmentRuleId = RuleId, InstalmentDT = date });
                    integral = 0;
                }
                index++;
            }
            return instalmentList;
        }
    }
}
