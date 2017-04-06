using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity
{
    public class Plans
    {
        public string Id { get; set; }
        /// <summary>
        /// 计划类型（取字典表）
        /// </summary>
        public short Type { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 详细计划
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 总结
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 上级领导(多个以逗号隔开)
        /// </summary>
        public string LeaderUID { get; set; }
        /// <summary>
        /// 执行人UID
        /// </summary>
        public string AssignerUID { get; set; }
        /// <summary>
        /// 执行状态（取字典）
        /// </summary>
        public short Status { get; set; }
        public DateTime CreateDT { get; set; }
        public string CreateUID { get; set; }


        public List<Attachment> Attachments { get; set; }
        public List<Reply> Replys { get; set; }
    }
}
