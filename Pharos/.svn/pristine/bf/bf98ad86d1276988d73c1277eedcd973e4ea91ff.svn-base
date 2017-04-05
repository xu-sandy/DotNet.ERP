using Pharos.Logic.Entity;
using Pharos.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL
{
    public class OrderDistributionService : BaseService<OrderDistribution>
    {
        /// <summary>
        /// 待收货
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<NoticeAbouctOrderDistributionDao> GetReceivedOrder()
        {
            var storeId = CurrentUser.StoreId;
            var query = (from a in CurrentRepository.Entities
                         from c in OrderService.CurrentRepository.Entities
                         from d in WarehouseService.CurrentRepository.Entities
                         where a.IndentOrderId == c.IndentOrderId && c.StoreId == d.StoreId && a.State == 4 && c.StoreId == storeId
                         select new NoticeAbouctOrderDistributionDao()
                         {
                             IndentOrderId = c.IndentOrderId,
                             Store = d.Title,
                             DistributionBatch = a.DistributionBatch,
                         }).Distinct();
            return query.ToList();
        }
    }
    public class NoticeAbouctOrderDistributionDao
    {

        public string IndentOrderId { get; set; }

        public string Store { get; set; }

        public string DistributionBatch { get; set; }
    }
}
