using Pharos.Logic.Entity.LocalEntity;
using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharos.Logic.BLL.LocalServices
{
    public class ProductInfoLocalService : BaseLocalService<ProductInfo>
    {
        /// <summary>
        /// 查库存
        /// </summary>
        /// <param name="storeId">商店ID</param>
        /// <param name="productCode">货号</param>
        /// <param name="barcode">条码</param>
        /// <param name="productName">产品名称</param>
        /// <param name="bigCategorySN">品类</param>
        /// <param name="subCategorySN">子类</param>
        /// <returns>商品列表</returns>
        public static IList<CheckInventoryDAO> FindCommodity(string storeId, string code, string productName, List<int> categoryList)
        {

            var query = (from a in CurrentRepository.QueryEntity
                         join d in ProductCategoryLocalService.CurrentRepository.QueryEntity on a.CategorySN equals d.CategorySN
                         where (a.Barcode.Contains(code) || a.ProductCode.Contains(code) || string.IsNullOrEmpty(code)) && a.State == 1 && (a.Title.Contains(productName) || string.IsNullOrEmpty(productName)) && a.ValuationType == 1
                         select new CheckInventoryDAO()
                         {
                             ValuationType = a.ValuationType,
                             Barcode = a.Barcode,
                             SysPrice = a.SysPrice,
                             ProductName = a.Title + (string.IsNullOrEmpty(a.Size) ? "" : " " + a.Size),
                             ProductCode = a.ProductCode,
                             CategoryName = d.Title,
                             SubCategorySN = d.CategorySN,
                             OldBarcode = a.OldBarcode,
                             SaleNum = a.SaleNum,
                             Nature = a.Nature,
                             StockNumber = a.StockNumber
                         }).ToList();
            return query.Where(o => (categoryList.Exists(p => p == o.SubCategorySN) || categoryList.FirstOrDefault() == -1)).ToList();
        }

        /// <summary>
        /// 查找商品
        /// </summary>
        /// <param name="form"></param>
        /// <param name="to"></param>
        /// <returns>商品列表</returns>
        public static IList<CheckPricesDAO> FindProductList(decimal form, decimal to, List<int> sns)
        {
            var query = (from a in CurrentRepository.QueryEntity
                         join b in ProductCategoryLocalService.CurrentRepository.QueryEntity on a.CategorySN equals b.CategorySN
                         // join c in ProductCategoryLocalService.CurrentRepository.QueryEntity on a.SubCategorySN equals c.CategorySN
                         join e in SysDataDictionaryLocalService.CurrentRepository.QueryEntity on a.SubUnitId equals e.DicSN
                         where a.SysPrice >= form && a.SysPrice <= to && a.State == 1 && a.ValuationType == 1
                         select new CheckPricesDAO()
                         {
                             Barcode = a.Barcode,
                             SysPrice = a.SysPrice,
                             ProductName = a.Title + (string.IsNullOrEmpty(a.Size) ? "" : " " + a.Size),
                             ProductCode = a.ProductCode,
                             //BigCategoryName = c.Title,
                             SubCategoryName = b.Title,
                             CategorySN = a.CategorySN,
                             Unit = e.Title
                         }).ToList();

            return query.Where(o => (sns.Exists(p => p == o.CategorySN) || sns.FirstOrDefault() == -1)).ToList();
        }

        public static List<ProductInfo> GetSeries(int CategorySN)
        {
            var ids = ProductCategoryLocalService.FindAllChild(CategorySN);
            ids.Add(CategorySN);
            var result = CurrentRepository.FindList(o => ids.Contains(o.CategorySN)).ToList();
            return result;
        }

    }

    public class CheckPricesDAO
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string SubCategoryName { get; set; }

        public string Barcode { get; set; }

        public decimal SysPrice { get; set; }

        public int CategorySN { get; set; }

        public string Unit { get; set; }

        public int ValuationType { get; set; }
    }

    public class CheckInventoryDAO
    {
        public int Nature { get; set; }


        public short ValuationType { get; set; }
        public string ProductCode { get; set; }
        /// <summary>
        /// 对应条码
        /// [长度：30]
        /// </summary>
        public string OldBarcode { get; set; }
        /// <summary>
        /// 可售数量
        /// </summary>
        public decimal? SaleNum { get; set; }
        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public string Barcode { get; set; }

        public decimal SysPrice { get; set; }

        public decimal StockNumber { get; set; }

        public int SubCategorySN { get; set; }
    }
}
