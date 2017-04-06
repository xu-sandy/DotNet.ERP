﻿using Pharos.CRM.Retailing.Models;
using Pharos.Logic.ApiData.Pos;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL.DataSynchronism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pharos.CRM.Retailing.Controllers
{
    public class DataSynchronismController : ApiController
    {
        [HttpPost]
        public bool TestConnect()
        {
            return true;
        }

        [HttpPost]
        public bool UpLoad([FromBody]UpdateFormData info)
        {
            if (info != null)
            {
                return DataSyncContext.UpLoadAll(info);
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public UpdateFormData Download([FromBody]UpdateFormData info)
        {
            if (info != null)
            {
                try
                {
                    return DataSyncContext.DownloadAll(info);
                }
                catch (Exception)
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 设备注册
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [HttpPost]
        public void RegisterDevice([FromBody] DeviceRequest requestParams)
        {
            MachinesSettings.RegisterDevice(requestParams.StoreId, requestParams.MachineSn,-1, requestParams.DeviceSn, requestParams.Type);
        }
    }
}