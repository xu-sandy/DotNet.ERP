using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public class Notice:BaseEntity
    {
        /// <summary>
        /// 公告主题
        /// [长度：100]
        /// [允许为空]
        /// </summary>
        public string Theme { get; set; }
        /// <summary>
        /// 公告内容
        /// [长度：1000]
        /// [允许为空]
        /// </summary>
        public string NoticeContent { get; set; }
        /// <summary>
        /// 公告范围(门店ID 多个ID 以,号间隔）
        /// [长度：100]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 公告状态（ 0:未发布 1:已发布）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Pharos.Utility.Exclude]
        public short State { get; set; }
        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 创建人UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }
        /// <summary>
        /// 公告截止日期
        /// </summary>
        public DateTime ExpirationDate { get; set; }
        /// <summary>
        /// 公告开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }

        public string Url { get; set; }
        /// <summary>
        /// 类型（1-公告；2-活动）
        /// </summary>
        public short Type { get; set; }

    }
}
