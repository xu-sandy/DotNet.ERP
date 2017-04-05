using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class MemberIntegralLocalService : BaseLocalService<MemberIntegral>
    {
        public static void Save(MemberIntegral record)
        {
            MemberIntegralLocalService.IsForcedExpired = true;
            var repository = MemberIntegralLocalService.CurrentRepository;
            if (repository.IsExist(o => o.PaySN == record.PaySN))
            {
                return;
            }
            if (record.ActualPrice == 0)
            {
                return;
            }
            repository.Add(record);
        }

        public static int GetMemberIntegral(string sn)
        {
            var integral =CurrentRepository.Entities.FirstOrDefault(o=>o.PaySN == sn);
            if (integral != null)
            {
                return integral.Integral;
            }
            else
                return 0;
        }
    }
}
