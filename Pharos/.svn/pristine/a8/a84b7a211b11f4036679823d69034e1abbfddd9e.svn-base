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
    /// 订单
    /// </summary>
    [RoutePrefix("api/mobile")]
    public class OrderController : ApiController
    {
        /// <summary>
        /// 订单明细
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderDetail")]
        public object GetOrderDetail([FromBody]JObject requestParams)
        {
            var orderId = requestParams.Property("orderId", true);
            var userCode = requestParams.Property("userCode", true);
            return OrderService.GetOrderDetail(orderId, userCode,2);
        }
        /// <summary>
        /// 订单审批列表
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderApproveList")]
        public object GetOrderApproveList([FromBody]JObject requestParams)
        {
            var date = requestParams.Property("date", true);
            var pageIndex = requestParams.Property("pageIndex", true);
            var pageSize = requestParams.Property("pageSize", true);
            return OrderService.GetOrderApproveList(date, pageIndex, pageSize);
        }
        /// <summary>
        /// 新审批列表
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderApproveNewList")]
        public object GetOrderApproveNewList([FromBody]JObject requestParams)
        {
            var userCode = requestParams.Property("userCode", true);
            return OrderService.GetOrderApproveNewList(userCode);
        }
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderApproveDayList")]
        public object GetOrderApproveDayList([FromBody]JObject requestParams)
        {
            var date = requestParams.Property("date", true);
            return OrderService.GetOrderApproveDayList(date);
        }
        /// <summary>
        /// 订单批准
        /// </summary>
        /// <param name="requestParams">{}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("OrderApprove")]
        public void OrderApprove([FromBody]JObject requestParams)
        {
            var orderId = requestParams.Property("orderId", true);
            OrderService.Approve(orderId);
        }
    }
}