using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class FreeGiftPurchaseDataSyncService : BaseDataSyncService<FreeGiftPurchase, FreeGiftPurchaseForLocal>
    {
        public override IEnumerable<FreeGiftPurchase> Download(string storeId, string entityType)
        {
            var ids = CommodityPromotionService.GetEffectiveId(storeId).ToList();
            return CurrentRepository.FindList(o => ids.Contains(o.CommodityId)).ToList();
        }

        public static IEnumerable<string> GetEffectiveId(string storeId) 
        {
            var ids = CommodityPromotionService.GetEffectiveId(storeId).ToList();
            return CurrentRepository.FindList(o => ids.Contains(o.CommodityId)).Select(o => o.GiftId).ToList();
        }
    }
}
