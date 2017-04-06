using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.BLL.LocalServices;
using Pharos.Logic.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class BundlingDataSyncService : BaseDataSyncService<Bundling, BundlingForLocal>
    {
        public override IEnumerable<Bundling> Download(string storeId, string entityType)
        {
            var ids = CommodityPromotionService.GetEffectiveId(storeId).ToList();
            return CurrentRepository.FindList(o => ids.Contains(o.CommodityId)).ToList();
        }
    }
}
