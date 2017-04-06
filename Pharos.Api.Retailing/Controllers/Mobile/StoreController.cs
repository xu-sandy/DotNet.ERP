using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Pharos.Api.Retailing.Models;
using Pharos.Logic.BLL;

namespace Pharos.Api.Retailing.Controllers.Mobile
{
    [RoutePrefix("api/mobile")]
    public class StoreController : ApiController
    {
        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetStoreList")]
        public object GetStoreList(int cid)
        {
            //var cid = Request.GetRouteData().Values["CID"];
            //var cid= Request.GetQueryNameValuePairs().FirstOrDefault(o => o.Key == "CC").Value;
            return WarehouseService.GetList().Select(o=>new{o.StoreId,o.Title,o.Address,o.CategorySN}).ToList();
        }
        /// <summary>
        /// 获取收银员列表
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCashierList")]
        public object GetCashierList(int cid,string storeId)
        {
            return SaleOrdersService.GetCashiers(storeId);
        }
        /// <summary>
        /// 获取导购员列表
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSalerList")]
        public object GetSalerList(int cid, string storeId)
        {
            return SaleOrdersService.GetSalers(storeId);
        }
        /// <summary>
        /// 获取销售类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSalerTypeList")]
        public object GetSalerTypeList()
        {
            return CommonService.EnumToSelect(typeof(Pharos.Logic.SaleType));
        }
    }
}