using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncProviders
{
    public class SyncDataServiceGroupFactory
    {
        public static IEnumerable<ISyncDataServiceGroup> Factory(ISyncContext dataService)
        {
            List<ISyncDataServiceGroup> result = new List<ISyncDataServiceGroup>();
            var groups = dataService.ServiceMappings.GroupBy(o => o.Value.SyncDirectionOrder, o => o).ToList();
            foreach (var item in groups)
            {
                var serviceGroup = new SyncDataServiceGroup();
                serviceGroup.SyncDirectionOrder = item.Key;
                serviceGroup.dataServiceDict = item.ToDictionary(o => o.Key, o => o.Value);
                result.Add(serviceGroup);
            }
            return result;
        }
    }
}
