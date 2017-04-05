﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.MemberDomain.Exceptions
{
    /// <summary>
    /// 积分规则提供程序加载异常
    /// 【余雄文】
    /// </summary>
    public class IntegralRuleProviderLoadException : System.Exception
    {
        public IntegralRuleProviderLoadException(string msg)
            : base(msg)
        {
        }
        public IntegralRuleProviderLoadException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

    }
}
