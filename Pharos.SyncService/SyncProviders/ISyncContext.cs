using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.SyncService
{
    public interface ISyncContext
    {
        IDictionary<string, ISyncDataService> ServiceMappings { get; }

        bool Contains(string key);

        ISyncDataService GetDataService(string key);
    }
}
