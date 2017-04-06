using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService
{
    public interface ISyncDataObject
    {
        Guid SyncItemId { get; set; }

        byte[] SyncItemVersion { get; set; }

        string EntityType { get; set; }
    }
}
