using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Sys
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum SysUserState : short
    {
        正常 = 1,
        锁定 = 2,
        注销 = 3,
    }
    public enum SysStoreRole : short { 
        店长=1,
        营业员=2,
        收银员=3,
        数据维护=4
    }
}
