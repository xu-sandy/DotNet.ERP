// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-07-10
// 描述信息：用于管理本系统的公告信息
// --------------------------------------------------

using Pharos.Utility;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 公告
    /// </summary>
    [Excel("公告信息")]

    public class Notice : BaseEntity
    {
        public Int64 Id { get; set; }

        /// <summary>
        /// 公告主题
        /// [长度：100]
        /// [允许为空]
        /// </summary>
        [Excel("公告主题", 1)]
        [ExcelField(@"^[\s,\S]{1,100}$###公告主题为必填且不超过100字符")]
        public string Theme { get; set; }
        [Excel("公告内容", 2)]
        [ExcelField(@"^[\s,\S]{1,1000}$###公告主题为不超过1000字符")]

        /// <summary>
        /// 公告内容
        /// [长度：1000]
        /// [允许为空]
        /// </summary>
        public string NoticeContent { get; set; }
        [Excel("公告范围", 3)]
        [ExcelField(@"^[0-9]{1,2}([,][0-9]{1,2})*$###门店ID长度应在1-2位且为数字")]

        /// <summary>
        /// 公告范围(门店ID 多个ID 以,号间隔）
        /// [长度：100]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public string StoreId { get; set; }
        [Excel("公告状态", 4)]
        [ExcelField(@"^[0,1]$###公告状态值范围（ 0:未发布 1:已发布）")]

        /// <summary>
        /// 公告状态（ 0:未发布 1:已发布）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short State { get; set; }
        [Excel("创建时间", 5)]
        [ExcelField(@"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$###创建时间格式为yyyy-MM-dd HH:mm:ss")]


        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// </summary>
        public DateTime CreateDT { get; set; }
        [Excel("创建人", 6)]
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###创建人UID应为Guid")]

        /// <summary>
        /// 创建人UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }
        [Excel("公告截止日期", 7)]
        [ExcelField(@"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$###公告截止日期格式为yyyy-MM-dd HH:mm:ss")]

        /// <summary>
        /// 公告截止日期
        /// </summary>
        public DateTime ExpirationDate { get; set; }
    }
}
