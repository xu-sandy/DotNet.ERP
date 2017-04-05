using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.ObjectModels.DTOs
{
    public enum CustomerType
    {
        /// <summary>
        /// 普通消费者
        /// </summary>
        Whole = 1,
        /// <summary>
        /// 内部人员
        /// </summary>
        Insider = 2,
        /// <summary>
        /// VIP
        /// </summary>
        VIP = 3
    }
}
