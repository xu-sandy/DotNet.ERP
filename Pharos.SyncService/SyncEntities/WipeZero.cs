using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class WipeZero : SyncDataObject
    {
        /// <summary>
        /// 支付编号
        /// </summary>
        public string PaySN { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Number { get; set; }

        public int CompanyId { get; set; }
    }
}
