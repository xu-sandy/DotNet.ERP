using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Pharos.Logic.Entity
{
    //ReturnProject
    public class ReturnProject
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// guid 用于关联
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 公司CID
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// ExpiryStart
        /// </summary>
        public DateTime ExpiryStart { get; set; }
        /// <summary>
        /// 有效期-截止(空为永久)
        /// </summary>
        public DateTime ExpiryEnd { get; set; }
        /// <summary>
        /// 返赠时效类型（0：每天；1：周；2：时效）
        /// </summary>
        public int AgingType { get; set; }
        /// <summary>
        /// 适用门店
        /// </summary>
        public string StoreIds { get; set; }
        /// <summary>
        /// 绑定会员等级id
        /// </summary>
        public string MemberLevelId { get; set; }
        /// <summary>
        /// 适配器(来自字典)
        /// </summary>
        public int Adapters { get; set; }
        /// <summary>
        /// 适配限定条件值
        /// </summary>
        public string LimitItems { get; set; }
        /// <summary>
        /// 返赠运算方式(0：按固定值；1：按固定比例；2：动态配置)
        /// </summary>
        public int OperationType { get; set; }
        /// <summary>
        /// 活动状态（0：为开始；1：活动中；2：已过期）
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 开始时间段 1（
        /// </summary>
        public string StartTime1 { get; set; }
        /// <summary>
        /// 结束时间段 1 
        /// </summary>
        public string EndTime1 { get; set; }
        /// <summary>
        /// 开始时间段 2
        /// </summary>
        public string StartTime2 { get; set; }
        /// <summary>
        /// 结束时间段 2
        /// </summary>
        public string EndTime2 { get; set; }
        /// <summary>
        /// 开始时间段 3 
        /// </summary>
        public string StartTime3 { get; set; }
        /// <summary>
        /// 结束时间段 3 
        /// </summary>
        public string EndTime3 { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUID { get; set; }

    }
}