﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class MemberIntegralSet : SyncDataObject
    {
        public int CompanyId { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 适用对象,1-内部,2-VIP
        /// </summary>
        public string CustomerObj { get; set; }
        /// <summary>
        /// 是否包含促销
        /// </summary>
        public bool Promotion { get; set; }
        /// <summary>
        /// 积分比例
        /// </summary>
        public short Scale { get; set; }
        /// <summary>
        /// 设定类型(1-消费积分,2-商品积分)
        /// </summary>
        public short Nature { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorUID { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatorTime { get; set; }

      //  public List<MemberIntegralSetList> ProductList { get; set; }
    }
}
