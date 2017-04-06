using Pharos.Logic.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL
{
    public class FreeGiftService : BaseService<FreeGiftPurchase>
    {
        /// <summary>
        /// 级联删除相关数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OpResult DeleteById(string[] ids)
        {
            var op = new OpResult();
            try
            {
                var promots = CommodityPromotionService.FindList(o => ids.Contains(o.Id));
                CommodityPromotionService.CurrentRepository.RemoveRange(promots, false);
                var blends = FreeGiftService.FindList(o => ids.Contains(o.CommodityId));
                FreeGiftService.CurrentRepository.RemoveRange(blends, false);
                var gifIds = blends.Select(o => o.GiftId).Distinct().ToList();
                var repBlend = BaseService<FreeGiftPurchaseList>.CurrentRepository;
                var blendList = repBlend.FindList(o => gifIds.Contains(o.GiftId)).ToList();
                repBlend.RemoveRange(blendList, true);
                op.Successed = true;
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
    }
}
