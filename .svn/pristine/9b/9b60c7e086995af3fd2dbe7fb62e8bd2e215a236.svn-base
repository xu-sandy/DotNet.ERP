using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService
{
    public class SyncJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //RedisManager.Publish("SyncDatabase", string.Empty);
            StoreManager.PubEvent("SyncDatabase", string.Empty);
        }
    }
}
