using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class ProductCategoryLocalService : BaseLocalService<ProductCategory>
    {
        public static List<ProductCategory> GetSubCategorySelectItem(int psn)
        {
            return ProductCategoryLocalService.FindList(o => o.CategoryPSN == psn).OrderBy(o => o.OrderNum).ToList();
        }

        public static List<int> FindAllParent(int sn, int deep = -1)
        {
            var list = new List<int>();
            var result = CurrentRepository.Find(o => o.CategorySN == sn && (o.Grade == deep || deep == -1));
            if (result != null)
            {
                if (deep == -1)
                    deep = result.Grade;
                list.Add(result.CategoryPSN);
                if (result.CategoryPSN != 0 && deep > 1)
                {
                    list = list.Concat(FindAllParent(result.CategoryPSN, --deep)).ToList();
                }
            }
            return list;
        }

        public static List<int> FindAllChild(int sn, int deep = -1)
        {
            var categories = CurrentRepository.FindList(o => o.CategoryPSN == sn && (o.Grade == deep || deep == -1));
            var temp = categories.Select(o => o.CategorySN).ToList();
            var result = temp;
            foreach (var item in categories)
            {
                result = result.Concat(FindAllChild(item.CategorySN, ++item.Grade)).ToList();
            }
            return result;
        }

    }
}
