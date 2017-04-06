using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    /// <summary>
    /// 公告
    /// </summary>
    [Excel("公告信息")]

    public class NoticeForLocal
    {
        /// <summary>
        /// 公告主题
        /// [长度：100]
        /// [允许为空]
        /// </summary>
        [Excel("公告主题", 1)]
        public string Theme { get; set; }
        [Excel("公告内容", 2)]

        /// <summary>
        /// 公告内容
        /// [长度：1000]
        /// [允许为空]
        /// </summary>
        public string NoticeContent { get; set; }
        [Excel("公告范围", 3)]

        /// <summary>
        /// 公告范围(门店ID 多个ID 以,号间隔）
        /// [长度：100]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public string StoreId { get; set; }
        [Excel("公告状态", 4)]

        /// <summary>
        /// 公告状态（ 0:未发布 1:已发布）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Pharos.Utility.Exclude]
        public short State { get; set; }
        [Excel("创建时间", 5)]

        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// </summary>
        public DateTime CreateDT { get; set; }
        [Excel("创建人", 6)]

        /// <summary>
        /// 创建人UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }
        [Excel("公告截止日期", 7)]

        /// <summary>
        /// 公告截止日期
        /// </summary>
        public DateTime ExpirationDate { get; set; }
    }
}
