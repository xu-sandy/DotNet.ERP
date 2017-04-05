﻿using Pharos.SyncService.SyncEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.SyncService.Helpers;
using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.DAL;

namespace Pharos.SyncService.RemoteDataServices
{
    public class SalesRecordSyncRemoteService : ISyncDataService
    {
        public TimeSpan SyncInterval
        {
            get
            {
                return new TimeSpan(0, 0, 1);
            }
        }
        public string Name
        {
            get
            {
                return this.GetType().ToString();
            }

        }
        public IEnumerable<ISyncDataObject> GetSyncObjects(int companyId, string storeId)
        {
            try
            {
                using (var db = SyncDbContextFactory.Factory<EFDbContext>())
                {
                    var result = db.SalesRecords.Where(o => o.CompanyId == companyId && o.StoreId == storeId).Select(o => new SyncDataObject() { SyncItemId = o.SyncItemId, SyncItemVersion = o.SyncItemVersion }).ToList();
                    return result;
                }
            }
            catch
            {
                return new List<ISyncDataObject>();
            }
        }

        public ISyncDataObject GetItem(Guid guid, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var entity = db.SalesRecords.Where(o => o.SyncItemId == guid).FirstOrDefault();
                if (entity != null)
                    return new Pharos.SyncService.SyncEntities.SalesRecord().InitEntity(entity);
                return null;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.SalesRecords.Where(o => syncids.Contains(o.SyncItemId)).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => (ISyncDataObject)new Pharos.SyncService.SyncEntities.SalesRecord().InitEntity(o));
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            var temp = data as Pharos.SyncService.SyncEntities.SalesRecord;
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                Pharos.Logic.Entity.SalesRecord entity;
                if (!db.SalesRecords.Any(o => o.SyncItemId == guid))
                {
                    entity = new Pharos.Logic.Entity.SalesRecord();
                    entity.InitEntity(temp);
                    db.SalesRecords.Add(entity);
                    db.SaveChanges();
                }
                else
                {
                    entity = db.SalesRecords.FirstOrDefault(o => o.SyncItemId == guid);
                }
                return entity.SyncItemVersion;
            }
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            var temp = mergedData as Pharos.SyncService.SyncEntities.SalesRecord;
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var dbEntity = db.SalesRecords.FirstOrDefault(o => o.SyncItemId == guid);
                if (dbEntity == null)
                    throw new SyncService.Exceptions.SyncException("未能找到更新的项！");
                dbEntity.InitEntity(temp);
                db.SaveChanges();
                var newEntity = db.SalesRecords.FirstOrDefault(o => o.SyncItemId == guid);
                return newEntity.SyncItemVersion;
            }
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var dbEntity = db.SalesRecords.FirstOrDefault(o => o.SyncItemId == syncItemId);
                db.SalesRecords.Remove(dbEntity);
                db.SaveChanges();
            }
        }

        public ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId)
        {
            return syncDataObject1;
        }
        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.UploadAndDownload; }
        }
    }
}
