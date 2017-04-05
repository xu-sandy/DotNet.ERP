using Newtonsoft.Json;
using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.MemberDomain.Interfaces;
using Pharos.ObjectModels.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.MemberDomain.QuanChengTaoProviders.EventHandlers
{
    public class RechargeEventHandler : IEventHandler
    {
        public void Handler()
        {
            RedisManager.Subscribe("RechargeCompleted", (channel, msg) =>
            {
                var rechargeCompleteEvent = JsonConvert.DeserializeObject<RechargeCompletedEvent>(msg);
                QuanChengTaoIntegralRuleService.RechargeIntegral(rechargeCompleteEvent);
            });
        }
    }
}
