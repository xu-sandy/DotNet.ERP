﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Pharos.Api.Retailing.Models
{
    public class BaseApiParams
    {
        /// <summary>
        /// 门店ID
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// POS编号
        /// </summary>
        public string MachineSn { get; set; }

        /// <summary>
        /// 公司授权码
        /// </summary>
        public int CID { get; set; }
        /// <summary>
        /// 机器编码
        /// </summary>
        public string DeviceSn { get; set; }
    }
}
