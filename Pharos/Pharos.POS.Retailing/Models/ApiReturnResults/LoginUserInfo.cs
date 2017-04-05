﻿using System.Collections.Generic;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    public class LoginUserInfo
    {
        /// <summary>
        /// 用户编号（员 工编 号，全 局唯一  ）
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 用户权限
        /// </summary>
        public Dictionary<string, List<int>> OperateAuth { get; set; }
    }
}
