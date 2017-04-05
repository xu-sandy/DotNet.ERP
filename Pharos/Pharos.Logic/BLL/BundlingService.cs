using Pharos.Logic.Entity;
using Pharos.Utility;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Pharos.Logic.BLL
{
    public class BundlingService : BaseService<Bundling>
    {
        /// <summary>
        /// 级联删除相关记录
        /// </summary>
        /// <param name="Ids">主键ID</param>
        /// <returns></returns>
        public static OpResult DeleteById(string[] ids)
        {
            var op = new OpResult();
            try
            {
                var repBund = BaseService<Bundling>.CurrentRepository;
                var bundings = repBund.FindList(o => ids.Contains(o.NewBarcode)).ToList();
                if (bundings == null)
                {
                    op.Message = "查不到数据"; return op;
                }
                var commids = bundings.Select(o => o.CommodityId).Distinct().ToList();
                var repBundlist = BaseService<BundlingList>.CurrentRepository;
                var bundlinglist = repBundlist.FindList(o => commids.Contains(o.CommodityId)).ToList();
                var repProm = BaseService<CommodityPromotion>.CurrentRepository;
                var prom = repProm.FindList(o => commids.Contains(o.Id)).ToList();
                repProm.RemoveRange(prom, false);
                repBund.RemoveRange(bundings, false);
                repBundlist.RemoveRange(bundlinglist, true);
                op.Successed = true;

                Log.WriteInfo(op.Successed ? "成功删除捆绑销售" : "删除捆绑销售失败");
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError("删除捆绑销售异常", ex);
            }
            return op;
        }

        public static IEnumerable<ProductDto> GetBundings(string store, string keyWord, int productBrand)
        {
            var date = DateTime.Now.Date;
            var query = (from a in CurrentRepository.Entities
                         from b in BundlingListService.CurrentRepository.Entities
                         from c in ProductService.CurrentRepository.Entities
                         from d in CommodityPromotionService.CurrentRepository.Entities
                         where a.CommodityId == b.CommodityId && b.Barcode == c.Barcode && a.CommodityId == d.Id && d.EndDate > date
                         && ("," + d.StoreId + ",").Contains("," + store + ",")
                         && (c.BrandSN == productBrand || productBrand == -1)
                         && (a.NewBarcode.Contains(keyWord) || b.Barcode.Contains(keyWord) || c.ProductCode.Contains(keyWord) || c.Title.Contains(keyWord) || string.IsNullOrEmpty(keyWord))
                         group new { a, b, c, d } by a.NewBarcode into g
                         select g);
            var result = query.ToList();
            return result.Select(o => new ProductDto()
            {
                IsWeigh = false,
                Barcode = o.Key,
                Category = "捆绑促销商品",
                SysPrice = o.FirstOrDefault().a.BundledPrice,
                Title = string.Join(Environment.NewLine, o.Select(p => "[" + p.c.ProductCode + "]" + p.c.Title + (string.IsNullOrEmpty(p.c.Size) ? "" : "," + p.c.Size) + ",×" + p.b.Number)),
                ExportTitle = string.Join(",", o.Select(p => p.c.Title)),
                Unit = "件"
            });

        }
    }
}
