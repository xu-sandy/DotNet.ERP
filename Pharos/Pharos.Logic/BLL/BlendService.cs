using Pharos.Logic.Entity;
using Pharos.Utility;
using System;
using System.Linq;

namespace Pharos.Logic.BLL
{
    public class BlendService:BaseService<PromotionBlend>
    {
        /// <summary>
        /// 级联删除相关记录
        /// </summary>
        /// <param name="ids">主键ID</param>
        /// <returns></returns>
        public static OpResult DeleteById(string[] ids)
        {
            var op = new OpResult();
            try
            {
                var promots = CommodityPromotionService.FindList(o => ids.Contains(o.Id));
                CommodityPromotionService.CurrentRepository.RemoveRange(promots, false);
                var blends = BlendService.FindList(o => ids.Contains(o.CommodityId));
                BlendService.CurrentRepository.RemoveRange(blends, false);
                var repBlend = BaseService<PromotionBlendList>.CurrentRepository;
                var blendList = repBlend.FindList(o => ids.Contains(o.CommodityId)).ToList();
                repBlend.RemoveRange(blendList, true);
                op.Successed = true;

                Log.WriteInfo(op.Successed ? "成功删除组合促销" : "删除组合促销失败");
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError("删除组合促销异常", ex);
            }
            return op;
        }
    }
}
