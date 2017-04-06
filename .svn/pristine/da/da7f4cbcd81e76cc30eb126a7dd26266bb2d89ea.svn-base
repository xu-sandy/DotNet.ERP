using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    public class ViewVisitTrack
    {
        /// <summary>
        /// 记录 ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// CID
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int CID { get; set; }

        /// <summary>
        /// 回访小结
        /// [长度：1000]
        /// [不允许为空]
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 回访时间(yyyyMM-dd)
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public string VisitDT { get; set; }

        /// <summary>
        /// 记录时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 修改时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime UpdateDT { get; set; }

        /// <summary>
        /// 操作人 UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }

        /// <summary>
        /// 记录人
        /// </summary>
        public string FullName { get; set; }
    }
}
