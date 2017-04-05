using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class BillDiscountLocalService : BaseLocalService<WipeZero>
    {
        public static KeyValuePair<string, decimal> GetSalesStatistics(DateTime data, int mode, string machineSn, string uid = "")
        {

            if (mode == 2)
            {
                data = new DateTime(data.Year, data.Month, 1);
            }
            var endDate = (mode == 1 ? data.AddDays(1) : data.AddMonths(1));
            var query = (from a in CurrentRepository.Entities
                         from b in SaleOrdersLocalService.CurrentRepository.Entities
                         where a.PaySN == b.PaySN && b.CreateDT >= data && b.CreateDT <= endDate && (b.MachineSN == machineSn || string.IsNullOrEmpty(machineSn)) && (b.CreateUID == uid || string.IsNullOrEmpty(uid))
                         select a
                            );
            var list = query.ToList();
            return new KeyValuePair<string, decimal>("自动抹零", list.Sum(o => o.Number));
        }
    }
}
