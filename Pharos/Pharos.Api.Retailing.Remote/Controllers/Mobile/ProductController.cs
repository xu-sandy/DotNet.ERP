using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Pharos.Logic.BLL;
using Pharos.Api.Retailing.Models;
using Pharos.Api.Retailing.Models.Mobile;
using Pharos.Logic.ApiData.Mobile.Exceptions;
using Pharos.Logic.ApiData.Pos.ValueObject;
namespace Pharos.Api.Retailing.Controllers.Mobile
{
    /// <summary>
    /// 商品
    /// </summary>
    [RoutePrefix("api/mobile")]
    public class ProductController : ApiController
    {
        /// <summary>
        /// 获取第一类别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFirstCategory")]
        public object GetFirstCategory([FromBody]BaseApiParams requestParams)
        {
            var list = ProductCategoryService.FindList(o => o.CompanyId == requestParams.CID && o.State == 1 && o.CategoryPSN == 0);
            return list.OrderBy(o => o.OrderNum).Select(o => new {o.CategorySN,o.Title });
        }
        /// <summary>
        /// 获取第二和第三级类别
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCategoryList")]
        public object GetCategoryList([FromBody]ProductRequest requestParams)
        {
            var seconds = ProductCategoryService.FindList(o => o.CompanyId == requestParams.CID && o.CategoryPSN == requestParams.FirstSN && o.State == 1);
            var secondsn = seconds.Select(o => o.CategorySN).ToList();
            var threes = ProductCategoryService.FindList(o => o.CompanyId == requestParams.CID && secondsn.Contains(o.CategoryPSN) && o.State == 1);
            var list = seconds.Union(threes);
            return list.Select(o => new { o.CategorySN,o.CategoryPSN,o.Title});
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetProductList")]
        public PageResult<object> GetProductList([FromBody]ProductListRequest requestParams)
        {
            if(requestParams.BaseList==null)
                throw new MessageException("BaseList为空!");
            return ProductService.GetProductsForAPI(requestParams.BaseList.PageIndex, requestParams.BaseList.PageSize, requestParams.FirstSN, requestParams.SecondSN, requestParams.ThreeSN, requestParams.Barcode);
        }
        /// <summary>
        /// 获取产品详细
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetProductByBarcode")]
        public object GetProductByBarcode([FromBody]ProductRequest requestParams)
        {
            return ProductService.GetProductByBarcode(requestParams.Barcode);
        }
    }
}