using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    public class WipeZeroForLocal
    {
        /// <summary>
        /// 支付编号
        /// </summary>
        public string PaySN { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Number { get; set; }
    }
}
