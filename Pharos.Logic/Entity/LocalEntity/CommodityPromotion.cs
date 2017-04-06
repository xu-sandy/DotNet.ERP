// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有商品促销信息
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Logic.Entity;
using Pharos.Utility;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 商品促销
    /// </summary>
    [Excel("主促销信息")]
    public partial class CommodityPromotion : BaseEntity
    {
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###促销ID应为Guid")]
        [Excel("商品促销Id", 1)]
        [LocalKey]
        public string Id { get; set; }
        /// <summary>
        /// 门店 ID（多个 ID 以,号间隔）
        /// [长度：2000]
        /// [不允许为空]
        /// </summary>
        [Excel("门店", 2)]
        [ExcelField(@"^[0-9]{1,2}([\,][0-9]{1,2})*$###门店ID长度应在1-2位且为数字,多个 ID 以,号间隔")]
        public string StoreId { get; set; }
        [ExcelField(@"^[0,1,2]$###适用客户群值范围（ 0:不限、 1:内部、2:VIP）")]

        /// <summary>
        /// 适用客户群（ 0:不限、 1:内部、2:VIP）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("适用客户群", 3)]
        public short CustomerObj { get; set; }
        [ExcelField(@"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$###活动起始日期格式为yyyy-MM-dd HH:mm:ss")]

        /// <summary>
        /// 活动起始日期
        /// [长度：10]
        /// [不允许为空]
        /// </summary>

        [Excel("活动起始日期", 4)]
        public DateTime StartDate { get; set; }
        [ExcelField(@"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$###活动结束日期格式为yyyy-MM-dd HH:mm:ss")]

        /// <summary>
        /// 活动结束日期
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("活动结束日期", 5)]
        public DateTime EndDate { get; set; }
        [ExcelField(@"^[0,1]$###活动时效值范围（ 0:不限、 1:指定时效）")]

        /// <summary>
        /// 活动时效（ 0:不限、 1:指定时效）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("活动时效", 6)]
        public short Timeliness { get; set; }
        [ExcelField(@"^((20|21|22|23|[0-1]?\d):[0-5]?\d(:[0-5]?\d{0,1}){0,1})*$###时效1开始格式为HH:mm")]

        /// <summary>
        /// 时效 1（开始）
        /// [长度：5]
        /// </summary>
        [Excel("时效1开始", 7)]
        public string StartAging1 { get; set; }
        [ExcelField(@"^((20|21|22|23|[0-1]?\d):[0-5]?\d(:[0-5]?\d{0,1}){0,1})*$###时效1结束格式为HH:mm")]

        /// <summary>
        /// 时效 1（结束）
        /// [长度：5]
        /// </summary>
        [Excel("时效1结束", 8)]
        public string EndAging1 { get; set; }
        [ExcelField(@"^((20|21|22|23|[0-1]?\d):[0-5]?\d(:[0-5]?\d{0,1}){0,1})*$###时效2开始格式为HH:mm")]

        /// <summary>
        /// 时效 2（开始）
        /// [长度：5]
        /// </summary>
        [Excel("时效2开始", 9)]
        public string StartAging2 { get; set; }
        [ExcelField(@"^((20|21|22|23|[0-1]?\d):[0-5]?\d(:[0-5]?\d{0,1}){0,1})*$###时效2结束格式为HH:mm")]

        /// <summary>
        /// 时效 2（结束）
        /// [长度：5]
        /// </summary>
        [Excel("时效2结束", 10)]
        public string EndAging2 { get; set; }
        [ExcelField(@"^((20|21|22|23|[0-1]?\d):[0-5]?\d(:[0-5]?\d{0,1}){0,1})*$###时效3开始格式为HH:mm")]

        /// <summary>
        /// 时效 3（开始）
        /// [长度：5]
        /// </summary>
        [Excel("时效3开始", 11)]
        public string StartAging3 { get; set; }
        [ExcelField(@"^((20|21|22|23|[0-1]?\d):[0-5]?\d(:[0-5]?\d{0,1}){0,1})*$###时效3结束格式为HH:mm")]

        /// <summary>
        /// 时效 3（结束）
        /// [长度：5]
        /// </summary>
        [Excel("时效3结束", 12)]
        public string EndAging3 { get; set; }
        [ExcelField(@"^[1,2,3,4,5]$###促销方式值范围（ 1:单品折扣、 2:捆绑促销、 3:组合促销、4:买赠促销、 5:满元促销）")]

        /// <summary>
        /// 促销方式（ 1:单品折扣、 2:捆绑促销、 3:组合促销、4:买赠促销、 5:满元促销）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("促销方式", 13)]
        public short PromotionType { get; set; }
        [ExcelField(@"^[0-9]{1,5}$###每天限购次数长度应在1-5位且为数字")]


        /// <summary>
        /// 每天限购次数（ 0:不限）
        /// [长度：5]
        /// [默认值：((0))]
        /// </summary>
        [Excel("每天限购次数", 14)]
        public short RestrictionBuyNum { get; set; }
        [ExcelField(@"^[0,1,2]$###活动状态值范围（ 0:未开始、 1:活动中、 2:已过期）")]

        /// <summary>
        /// 活动状态（ 0:未开始、 1:活动中、 2:已过期）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("活动状态", 15)]
        public short State { get; set; }
        [ExcelField(@"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$###创建时间格式为yyyy-MM-dd HH:mm:ss")]

        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [Excel("创建时间", 16)]
        public DateTime CreateDT { get; set; }
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###创建人UID应为Guid")]

        /// <summary>
        /// 创建人 UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("创建人", 17)]
        public string CreateUID { get; set; }

    }
}
