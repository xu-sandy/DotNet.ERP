using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class ProductCategoryDataSyncService : BaseDataSyncService<ProductCategory, ProductCategoryForLocal>
    {
        public override IEnumerable<ProductCategory> Download(string storeId, string entityType)
        {
            var entity = WarehouseService.Find(o => o.StoreId == storeId);
            var CategorySN = new List<int>();
            if (entity != null)
            {
                try
                {
                    CategorySN = entity.CategorySN.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(o => Convert.ToInt32(o)).ToList();
                }
                catch
                {
                }
            }
            var sources = CurrentRepository.FindList(o => CategorySN.Contains(o.CategorySN) && o.Grade == 1).ToList();
            sources = sources.Concat(GetCategory(CategorySN, 2)).ToList();
            return sources;
        }

        private IEnumerable<ProductCategory> GetCategory(List<int> categories, int deep)
        {
            var result = new List<ProductCategory>();
            try
            {
                result = CurrentRepository.FindList(o => categories.Contains(o.CategoryPSN) && o.Grade == deep).ToList();
                var ids = result.Select(o => o.CategorySN).ToList();

                if (ids.Count == 0)
                {
                    return result;
                }
                else
                {
                    var arr = GetCategory(ids, ++deep);
                    result = result.Concat(arr).ToList();
                }
            }
            catch { }
            return result;
        }

    }
}
