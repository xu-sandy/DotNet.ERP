using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 返赠方案表
    /// </summary>
    public class ReturnRules
    {
        /// <summary>
        /// 记录 ID 
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 公司CID
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        public string MemberLevelId { get; set; }
        /// <summary>
        /// 适配器(来自字典)
        /// </summary>
        public int Adapters { get; set; }
        /// <summary>
        /// 数值1
        /// </summary>
        public decimal Number1 { get; set; }
        /// <summary>
        /// <,<=,=(来自字典)
        /// </summary>
        public int LeftSign { get; set; }
        /// <summary>
        /// <=,<,=(来自字典)
        /// </summary>
        public int? RightSign { get; set; }
        /// <summary>
        /// 数值2
        /// </summary>
        public decimal? Number2 { get; set; }
        /// <summary>
        /// 计量模式（eg:金额，次数。。）
        /// </summary>
        public int Mode { get; set; }
        /// <summary>
        /// 适配限定条件值
        /// </summary>
        public string LimitItems { get; set; }
        /// <summary>
        /// 返赠运算方式(0：按固定值；1：按固定比例；2：动态配置)
        /// </summary>
        public short OperationType { get; set; }
        /// <summary>
        /// 返赠具体值
        /// </summary>
        public string Expression { get; set; }
        /// <summary>
        /// 返赠类型(0:返现；1：返积分)
        /// </summary>
        public short GivenType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime ExpiryStart { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? ExpiryEnd { get; set; }
        /// <summary>
        /// 时效类型（0:全天；1：周；2：时段）
        /// </summary>
        public short AgingType { get; set; }
        /// <summary>
        /// 时段
        /// </summary>
        public string StartTime1 { get; set; }
        /// <summary>
        /// 时段
        /// </summary>
        public string EndTime1 { get; set; }
        /// <summary>
        /// 时段
        /// </summary>
        public string StartTime2 { get; set; }

        /// /// <summary>
        /// 时段
        /// </summary>
        public string EndTime2 { get; set; }
        /// <summary>
        /// 时段
        /// </summary>
        public string StartTime3 { get; set; }
        /// <summary>
        /// 时段
        /// </summary>
        public string EndTime3 { get; set; }
        /// <summary>
        /// 限量
        /// </summary>
        public int? LimitNum { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 状态（0：未开始；1：活动中；2：已过期）
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUID { get; set; }
    }
}
