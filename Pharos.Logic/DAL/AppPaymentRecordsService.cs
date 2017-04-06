using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;

namespace Pharos.Logic.DAL
{
    public class AppPaymentRecordsService : BaseService<AppPaymentRecords>
    {
        public static List<AppPaymentRecords> FindPayOrderHistory(int companyId, string storeId, DateTime? beginDate, DateTime? endDate, int status)
        {
            var baseQuery = CurrentRepository.Entities.Where(o => o.CompanyId == companyId && o.StoreId == storeId);
            if (beginDate.HasValue)
            {
                baseQuery = baseQuery.Where(o => o.CreateDate > beginDate);
            }
            if (endDate.HasValue)
            {
                baseQuery = baseQuery.Where(o => o.CreateDate < endDate);
            }
            if (status != -1)
            {
                baseQuery = baseQuery.Where(o => o.State == status);
            }
            return baseQuery.ToList();
        }
    }
}
