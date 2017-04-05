using Pharos.Logic.ApiData.Pos.DataAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Category
{
    /// <summary>
    /// 分类树
    /// </summary>
    public class CategoryTree
    {
        /// <summary>
        /// 获取分类树
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="machineSn"></param>
        /// <returns></returns>
        public Category GetCategoryTree(string storeId, string machineSn, int companyId, string deviceSn)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            var categories = dataAdapter.GetStoreCategory();
            var result = Category.CategoryTreeFactory(categories, true, "全部");
            return result;
        }
    }
}
