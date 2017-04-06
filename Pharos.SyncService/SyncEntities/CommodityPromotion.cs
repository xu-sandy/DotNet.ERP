using Pharos.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    public class CommodityPromotion : SyncDataObject
    {

        /// <summary>
        /// 促销 ID
        /// [主键：√]
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///门店 ID（多个 ID 以,号间隔）
        /// ///[长度：2000]
        ///[不允许为空]
        ///</summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 适用客户群（ 0:不限、 1:内部、2:VIP）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short CustomerObj { get; set; }


        /// <summary>
        /// 活动起始日期
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public DateTime? StartDate { get; set; }

        public int CompanyId { get; set; }
        /// <summary>
        /// 活动结束日期
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public DateTime? EndDate { get; set; }


        /// <summary>
        /// 活动时效（ 0:不限、 1:指定时效）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short Timeliness { get; set; }

        /// <summary>
        /// 时效 1（开始）
        /// [长度：5]
        /// </summary>
        public string StartAging1 { get; set; }


        /// <summary>
        /// 时效 1（结束）
        /// [长度：5]
        /// </summary>
        public string EndAging1 { get; set; }


        /// <summary>
        /// 时效 2（开始）
        /// [长度：5]
        /// </summary>
        public string StartAging2 { get; set; }


        /// <summary>
        /// 时效 2（结束）
        /// [长度：5]
        /// </summary>
        public string EndAging2 { get; set; }


        /// <summary>
        /// 时效 3（开始）
        /// [长度：5]
        /// </summary>
        public string StartAging3 { get; set; }


        /// <summary>
        /// 时效 3（结束）
        /// [长度：5]
        /// </summary>
        public string EndAging3 { get; set; }


        /// <summary>
        /// 促销方式（ 1:单品折扣、 2:捆绑促销、 3:组合促销、4:买赠促销、 5:满元促销）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short PromotionType { get; set; }


        /// <summary>
        /// 每天限购次数（ 0:不限）
        /// [长度：5]
        /// [默认值：((0))]
        /// </summary>
        public short RestrictionBuyNum { get; set; }


        /// <summary>
        /// 活动状态（ 0:未开始、 1:活动中、 2:已过期）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short State { get; set; }


        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }


        /// <summary>
        /// 创建人 UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }

    }
}
