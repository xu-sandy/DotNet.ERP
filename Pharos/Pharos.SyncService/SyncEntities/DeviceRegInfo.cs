﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class DeviceRegInfo : SyncDataObject
    {
        public int CompanyId { get; set; }

        /// <summary>
        /// 设备类型（1:大POS机、2:PAD、3:Mobile） 
        /// </summary>
        public short Type { get; set; }
        /// <summary>
        /// 设备编码（全局唯一） 
        /// </summary>
        public string DeviceSN { get; set; }
        /// <summary>
        /// POS机号（全局唯一）
        /// </summary>
        public string MachineSN { get; set; }
        /// <summary>
        /// 门店ID（全局唯一）
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 注册时间 
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 状态（0:禁用、1:可用）
        /// </summary>
        public short State { get; set; }
        /// <summary>
        /// 审核人UID 
        /// </summary>
        public string AuditorUID { get; set; }

        /// <summary>
        /// 备注
        /// [长度：200]
        /// [允许为空]
        /// </summary>
        public string Memo { get; set; }

    }
}
