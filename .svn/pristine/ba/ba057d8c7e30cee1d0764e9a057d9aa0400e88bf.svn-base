using Pharos.Logic.MemberDomain.Interfaces;
using Pharos.Logic.MemberDomain.QuanChengTaoProviders.IntegralRounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.MemberDomain.QuanChengTaoProviders
{
    public class IntegralRoundFactory
    {
        public IRound CreateRounder(int companyId)
        {
            IRound rounder = null;
            switch (companyId)
            {
                default:
                    rounder = new DefaultIntegralRound();
                    break;
            }
            return rounder;
        }
    }
}
