﻿using Pharos.Api.Retailing.Models.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pharos.Api.Retailing.Controllers
{
    [RoutePrefix("api/mobile")]
    public class CommonController : ApiController
    {
        /// <summary>
        /// 获取该帐户下所有门店
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetStoreIdByUser")]
        public object GetStoreIdByUser([FromBody]StoreUserRequest requestParams)
        {
            return Pharos.Logic.BLL.UserInfoService.GetStoreIdByUser(requestParams.LoginName);
        }
    }
}
