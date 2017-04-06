using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pharos.CRM.Retailing.Controllers
{
    public class BarcodePrintController : ApiController
    {
        [HttpGet]
        public IEnumerable<ProductCategoryDto> GetProductCategory()
        {
            var result = ProductCategoryService.GetAllProductCategory().Select(o => new ProductCategoryDto() { CategoryPSN = o.CategoryPSN, CategorySN = o.CategorySN, Grade = o.Grade, Title = o.Title });
            return result;
        }
        [HttpGet]
        public IEnumerable<StoreDto> GetStores()
        {
            return WarehouseService.FindList(o => o.State == 1).Select(o => new StoreDto() { StoreId = o.StoreId, Title = o.Title });
        }

        [HttpGet]
        public IEnumerable<ProductBrandDto> GetProductBrand()
        {
            return ProductBrandService.FindList(o => o.State == 1).Select(o => new ProductBrandDto() { BrandSN = o.BrandSN, Title = o.Title }).ToList();
        }
        [HttpPost]
        public DataGridPagingResult<IEnumerable<ProductDto>> GetProducts(ProductRequestDto theParams)
        {
            var result = ProductService.GetProducts(theParams.KeyWord, theParams.Store, theParams.ProductBrand, theParams.Categories);
            if (theParams.Categories.Contains(-10))
            {
                var bundlingResult = BundlingService.GetBundings(theParams.Store, theParams.KeyWord, theParams.ProductBrand);
                if (bundlingResult != null && bundlingResult.Count() > 0)
                {
                    result = result.Concat(bundlingResult);
                }
            }

            return new DataGridPagingResult<IEnumerable<ProductDto>>()
            {
                Result = result.Skip((theParams.PageIndex - 1) * theParams.PageSize).Take(theParams.PageSize),
                Total = result.Count()
            };
        }
    }
    public class DataGridPagingResult<R>
    {
        public int Total { get; set; }

        public R Result { get; set; }
    }
    public class ProductRequestDto
    {
        public string KeyWord { get; set; }

        public string Store { get; set; }

        public int ProductBrand { get; set; }

        public List<int> Categories { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class ProductCategoryDto
    {
        /// <summary>
        /// 分类编号（全局唯一） 
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int CategorySN { get; set; }
        /// <summary>
        /// 上级分类SN
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int CategoryPSN { get; set; }
        /// <summary>
        /// 分类层级（1:顶级、2：二级、3:三级、4:四级）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short Grade { get; set; }
        /// <summary>
        /// 分类名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }
    }

    public class StoreDto
    {
        /// <summary>
        /// 商店ID
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// 商店名称
        /// </summary>
        public string Title { get; set; }


    }
    public class ProductBrandDto
    {
        /// <summary>
        /// 品牌编号（全局唯一)
        /// </summary>
        public int BrandSN { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string Title { get; set; }

    }

}