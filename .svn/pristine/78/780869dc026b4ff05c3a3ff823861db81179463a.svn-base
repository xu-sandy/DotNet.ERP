using Pharos.Logic.Entity;
using Pharos.Utility;
using System;
using System.Linq;

namespace Pharos.Logic.BLL
{
    public class CommodityDiscountService:BaseService<Entity.CommodityDiscount>
    {
        /// <summary>
        /// 级联删除相关记录
        /// </summary>
        /// <param name="Ids">主键ID</param>
        /// <returns></returns>
        public static OpResult DeleteById(Int64[] ids)
        {
            var op = new OpResult();
            try
            {
                var repDis = BaseService<CommodityDiscount>.CurrentRepository;
                var discounts = repDis.FindList(o => ids.Contains(o.Id)).ToList();
                if (!discounts.Any())
                {
                    op.Message = "查不到数据"; return op;
                }
                var commids = discounts.Select(o => o.CommodityId).Distinct().ToList();
                var repProm = BaseService<CommodityPromotion>.CurrentRepository;
                foreach (var cid in commids)
                {
                    var count = repDis.QueryEntity.Count(o => o.CommodityId == cid);
                    var dis = discounts.Where(o => o.CommodityId == cid).ToList();
                    if (count == 1)
                    {
                        var prom = repProm.FindById(cid);
                        repProm.Remove(prom, false);
                        repDis.RemoveRange(dis, true);
                    }
                    else
                    {
                        repDis.RemoveRange(dis, true);
                    }
                }
                op.Successed = true;

                Log.WriteInfo(op.Successed ? "成功删除单品折扣" : "删除单品折扣失败");
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError("删除单品折扣异常", ex);
            }
            return op;
        }
    }
}
