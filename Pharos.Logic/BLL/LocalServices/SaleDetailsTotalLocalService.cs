using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class SaleDetailsTotalLocalService : BaseLocalService<SaleDetailsTotal>
    {
        public static void Save(List<SaleDetailsTotal> billList)
        {
            SaleDetailsTotalLocalService.IsForcedExpired = true;
            var repository = SaleDetailsTotalLocalService.CurrentRepository;
            var first = billList.FirstOrDefault();
            if (first == null)
                return;
            if (repository.IsExist(o => o.PaySN == first.PaySN))
            {
                return;
            }
            repository.AddRange(billList);
        }

    }
}
