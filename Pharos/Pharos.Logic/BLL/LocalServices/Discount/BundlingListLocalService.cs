using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class BundlingListLocalService : BaseLocalService<BundlingList>
    {

        public static IEnumerable<BundlingProductDao> GetBundlingList(string commodityId)
        {
            var query = from a in CurrentRepository.QueryEntity
                        join b in ProductInfoLocalService.CurrentRepository.QueryEntity on a.Barcode equals b.Barcode
                        where a.CommodityId == commodityId
                        select new BundlingProductDao()
                        {
                            Title = b.Title,
                            Number = a.Number,
                            SysPrice = b.SysPrice
                        };

            return query.ToList();
        }


    }
    public class BundlingProductDao
    {
        public string Title { get; set; }
        public int Number { get; set; }
        public decimal SysPrice { get; set; }
    }

}
