using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    public class ViewTraderType
    {
        /// <summary>
        /// 分类GUID
        /// </summary>
        public string TraderTypeId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
    }
}
