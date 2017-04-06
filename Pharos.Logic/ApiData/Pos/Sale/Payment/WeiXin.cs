using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    public class WeiXin : BasePay
    {
        public WeiXin()
            : base(21, PayMode.WeChat)
        {
        }
    }
}
