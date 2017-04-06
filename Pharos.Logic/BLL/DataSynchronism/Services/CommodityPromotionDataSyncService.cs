using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.DataSynchronism.Dtos;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    /// <summary>
    /// 商品促销主表
    /// </summary>
    public class CommodityPromotionDataSyncService : BaseDataSyncService<CommodityPromotion, CommodityPromotionForLocal>
    {
        public override IEnumerable<CommodityPromotion> Download(string storeId, string entityType)
        {
            var date = DateTime.Now.Date;
            return CurrentRepository.FindList(o => o.State != 2 && o.EndDate >= date).ToList();
        }
        public override IEnumerable<dynamic> Export(string storeId, string entityType)
        {
            var date = DateTime.Now.Date;
            var list = CurrentRepository.FindList(o => o.State != 2 && o.EndDate >= date);
            list.Each(o => { o.StoreId = o.StoreId.TrimStart(','); });
            return list;
        }
    }
}
