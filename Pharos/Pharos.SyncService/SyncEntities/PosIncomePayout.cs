using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class PosIncomePayout : SyncDataObject
    {
        /// <summary>
        /// 门店ID
        /// [长度：3]
        /// [不允许为空]
        /// </summary>
        public string StoreId { get; set; }


        /// <summary>
        /// POS机号
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        public string MachineSN { get; set; }


        /// <summary>
        /// 收银员UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }


        /// <summary>
        /// 类型（0:出款、1:入款） 
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short Type { get; set; }


        /// <summary>
        /// 金额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal Amount { get; set; }


        /// <summary>
        /// 时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }

        public int CompanyId { get; set; }
        /// <summary>
        /// 是否练习模式数据
        /// [不允许为空]
        /// [默认值：(0)]
        /// </summary>
        public bool IsTest { get; set; }
    }
}
