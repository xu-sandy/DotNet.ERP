using Pharos.Logic.MemberDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.MemberDomain.QuanChengTaoProviders.IntegralRounds
{
    public class DefaultIntegralRound : IRound
    {
        public decimal DoRound(decimal integral, IScene scene, IIntegralRule rule)
        {
            return ((int)(integral * 10m)) / 10M;
        }
    }
}
