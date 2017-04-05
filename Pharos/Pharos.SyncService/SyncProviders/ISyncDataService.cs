﻿using Microsoft.Synchronization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.SyncService
{
    public interface ISyncDataService
    {
        SyncDirectionOrder SyncDirectionOrder { get; }
        IEnumerable<ISyncDataObject> GetSyncObjects(int companyId, string storeId);

        ISyncDataObject GetItem(Guid guid, int companyId, string storeId);
        byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId);

        byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId);
        void DeleteItem(Guid syncItemId, int companyId, string storeId);
        ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId);

        IDictionary<SyncId, ISyncDataObject> GetItems(IEnumerable<SyncId> ids, int companyId, string storeId);

        TimeSpan SyncInterval { get; }
        string Name { get; }
    }
}
