using Pharos.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.PosModels
{
    public enum OperatingStatus
    {
        /// <summary>
        /// 正常销售
        /// </summary>
        [EnumTitle(0, "收银")]
        Normal = 0,

        /// <summary>
        /// 测试（练习）
        /// </summary>
        [EnumTitle(1, "练习")]
        Test = 1
    }
}
