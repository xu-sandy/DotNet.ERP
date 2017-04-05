using Newtonsoft.Json;
using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment;
using Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment.InstalmentItems;
using Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment.InstalmentParameters;
using Pharos.Logic.MemberDomain.Interfaces;
using Pharos.ObjectModels.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.MemberDomain.QuanChengTaoProviders.SubscribeMemberEvents
{
    public class SaleOrderEventHandler : IEventHandler
    {
        public void Handler()
        {
            RedisManager.Subscribe("OrderCompleted", (channel, msg) =>
            {
                var orderCompleteEvent = JsonConvert.DeserializeObject<OrderCompletedEvent>(msg);
                QuanChengTaoIntegralRuleService.OrderIntegral(orderCompleteEvent);
            });
        }
    }
}
