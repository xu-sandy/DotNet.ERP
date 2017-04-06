using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Pharos.Logic.ApiData.Mobile.Services;

namespace Pharos.Api.Retailing.Controllers.Mobile
{
    /// <summary>
    /// 供应商
    /// </summary>
    [RoutePrefix("api/mobile")]
    public class SupplierController : ApiController
    {
        /// <summary>
        /// 订单配送清单
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderDeliveryList")]
        public object GetOrderDeliveryList([FromBody]JObject requestParams)
        {
            var supplierId=requestParams.Property("supplierId",true);
            var date=requestParams.Property("date",true);
            return SupplierService.GetOrderDeliveryList(supplierId, date);
        }
        /// <summary>
        /// 新订单列表
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderNewList")]
        public object GetOrderNewList([FromBody]JObject requestParams)
        {
            var supplierId = requestParams.Property("supplierId", true);
            return SupplierService.GetOrderNewList(supplierId);
        }
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderDeliveryDetail")]
        public object GetOrderDeliveryDetail([FromBody]JObject requestParams)
        {
            var orderId = requestParams.Property("orderId", true);
            var userCode = requestParams.Property("supplierId", true);
            return OrderService.GetOrderDetail(orderId, userCode,3);
        }
        /// <summary>
        /// 订单配送当天清单
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderDeliveryDayList")]
        public object GetOrderDeliveryDayList([FromBody]JObject requestParams)
        {
            var supplierId = requestParams.Property("supplierId", true);
            var date = requestParams.Property("date", true);
            return SupplierService.GetOrderDeliveryDayList(supplierId, date);
        }
        /// <summary>
        /// 确定配送
        /// </summary>
        /// <param name="requestParams">{}</param>
        [HttpPost]
        [Route("OrderDelivery")]
        public void OrderDelivery([FromBody]JObject requestParams)
        {
            var orderId = requestParams.Property("orderId", true);
            var barcode = requestParams.Property("barcode", true);
            var number = requestParams.Property("number", true);
            SupplierService.OrderDelivery(orderId, barcode,number);
        }

    }
}