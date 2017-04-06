using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class PosCheckingLocalService : BaseLocalService<PosChecking>
    {
        public static DateTime GetLastPosChecking()
        {
            if (CurrentRepository.Entities.Count() > 0)
            {
                return CurrentRepository.Entities.Max(o => o.CreateDT);
            }
            return DateTime.MinValue;
        }

        public static IEnumerable<KeyValuePair<string, decimal>> GetPosAccountCheck(string machineId, DateTime date, string uid, string storeId)
        {
            var endDateTime = date.AddDays(1);
            var query = (from a in CurrentRepository.Entities
                         where a.CreateDT > date && a.CreateDT < endDateTime && a.MachineSN == machineId && a.CreateUID == uid && storeId == a.StoreId
                         group a by a.Project into g
                         select g
                 ).ToList();
            List<dynamic> result = new List<dynamic>();
            query.ForEach(o =>
                {
                    var total = 0m;
                    switch (o.Key)
                    {
                        case "剩余现金":
                            var entity = o.OrderByDescending(p => p.CreateDT).FirstOrDefault();
                            if (entity != null)
                                total = entity.Total;
                            break;
                        default:
                            total = o.Sum(p => p.Total);
                            break;
                    }
                    result.Add(new { Key = o.Key, Total = total, OrderId = o.FirstOrDefault().OrderId });
                });
            return result.OrderBy(o => o.OrderId).ToDictionary(o => ((string)o.Key), p => ((decimal)p.Total));
        }
    }
}
