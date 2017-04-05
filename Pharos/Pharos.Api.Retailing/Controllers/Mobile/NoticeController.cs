﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Pharos.Logic.BLL;

namespace Pharos.Api.Retailing.Controllers.Mobile
{
    [RoutePrefix("api/mobile")]
    public class NoticeController : ApiController
    {
        /// <summary>
        /// 门店公告列表
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetStoreNoticeList")]
        public object GetStoreNoticeList([FromBody]JObject requestParams)
        {
            string userCode = requestParams.Property("userCode", true);
            return NoticeService.GetNoticeList(userCode);
        }
        /// <summary>
        /// 门店未读公告条数
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetStoreNoticeNum")]
        public object GetStoreNoticeNum([FromBody]JObject requestParams)
        {
            string userCode = requestParams.Property("userCode", true);
            return NoticeService.GetNoticeNum(userCode);
        }
        /// <summary>
        /// 订单未审核数
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderNotApproveNum")]
        public object GetOrderNotApproveNum([FromBody]JObject requestParams)
        {
            string userCode = requestParams.Property("userCode", true);
            return Pharos.Logic.ApiData.Mobile.Services.OrderService.GetOrderApproveNewList(userCode).Count();
        }
        /// <summary>
        /// 供应商未配送数
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSupplierNotDeliveyNum")]
        public object GetSupplierNotDeliveyNum([FromBody]JObject requestParams)
        {
            string supplierId = requestParams.Property("userCode", true);
            return Pharos.Logic.ApiData.Mobile.Services.SupplierService.GetOrderNewList(supplierId).Count();
        }

    }
}