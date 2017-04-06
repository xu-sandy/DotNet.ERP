using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Pharos.Logic.MemberDomain.Interfaces
{
    /// <summary>
    /// 积分规则
    /// 【余雄文】
    /// </summary>
    public interface IIntegralRule<TScene> : IIntegralRule
        where TScene : IScene, new()
    {
        Expression<Func<TScene, bool>> VerifyExpression { get; set; }
        Expression<Func<TScene, decimal>> IntegralExpression { get; set; }
    }
    public interface IIntegralRule
    {
        /// <summary>
        /// 计量模式
        /// </summary>
        int MeteringMode { get; set; }

        string Id { get; set; }
    }
}
