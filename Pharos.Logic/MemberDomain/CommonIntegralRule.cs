using Pharos.Logic.MemberDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Pharos.Logic.MemberDomain
{
    public class CommonIntegralRule<TScene> : IIntegralRule<TScene>
        where TScene : IScene, new()
    {
        public string LimitItems { get; set; }
        public string Id { get; set; }
        public int MeteringMode { get; set; }
        public Expression<Func<TScene, bool>> VerifyExpression { get; set; }
        public Expression<Func<TScene, decimal>> IntegralExpression { get; set; }

    }
}
