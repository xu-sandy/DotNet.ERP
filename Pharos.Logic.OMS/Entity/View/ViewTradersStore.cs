﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    /// <summary>
    /// 商家门店
    /// </summary>
    public class ViewTradersStore
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 指派人
        /// </summary>
        public string Assign
        {
            get;
            set;
        }

        /// <summary>
        /// 门店状态
        /// </summary>
        public short State
        {
            get;
            set;
        }

        /// <summary>
        /// 系统商户号
        /// </summary>
        public int CID
        {
            get;
            set;
        }

        /// <summary>
        /// 系统门店编号
        /// </summary>
        public string StoreNum
        {
            get;
            set;
        }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string StoreName
        {
            get;
            set;
        }

        /// <summary>
        /// 门店主账号
        /// </summary>
        public string LoginName
        {
            get;
            set;
        }

        /// <summary>
        /// 第三方支付平台门店编号
        /// </summary>
        public string StoreNum3
        {
            get;
            set;
        }

        /// <summary>
        /// 门店二维码
        /// </summary>
        public string QRCode
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatePerson
        {
            get;
            set;
        }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDT
        {
            get;
            set;
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditPerson
        {
            get;
            set;
        }
    }
}