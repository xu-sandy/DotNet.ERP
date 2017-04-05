using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    /// <summary>
    /// 审核日志
    /// </summary>
    public class ViewApproveLog
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
        private DateTime _CreateTime;

        /// <summary>
        /// 描述说明
        /// [长度：1000]
        /// [不允许为空]
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        private string _Description;

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string OperatorFullName { get; set; }
    }
}
