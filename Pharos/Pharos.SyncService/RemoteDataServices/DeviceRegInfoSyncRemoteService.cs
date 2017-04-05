﻿using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.DAL;
using Pharos.SyncService.SyncEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.SyncService.Helpers;
using Pharos.SyncService.Exceptions;

namespace Pharos.SyncService.RemoteDataServices
{
    public class DeviceRegInfoSyncRemoteService : ISyncDataService
    {
        public TimeSpan SyncInterval
        {
            get
            {
                return new TimeSpan(0, 0, 0, 500);
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
                    var result = db.DeviceRegInfos.Where(o => o.CompanyId == companyId && o.StoreId == storeId).Select(o => new SyncDataObject() { SyncItemId = o.SyncItemId, SyncItemVersion = o.SyncItemVersion }).ToList();
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
                var entity = db.DeviceRegInfos.Where(o => o.SyncItemId == guid).FirstOrDefault();
                if (entity != null)
                    return new Pharos.SyncService.SyncEntities.DeviceRegInfo().InitEntity(entity);
                return null;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.DeviceRegInfos.Where(o => syncids.Contains(o.SyncItemId)).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => (ISyncDataObject)new Pharos.SyncService.SyncEntities.DeviceRegInfo().InitEntity(o));
            }
        }

        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            var temp = data as DeviceRegInfo;

            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                Pharos.Logic.Entity.DeviceRegInfo entity;
                if (!db.DeviceRegInfos.Any(o => o.SyncItemId == guid))
                {
                    entity = new Pharos.Logic.Entity.DeviceRegInfo();
                    entity.InitEntity(temp, false);
                    db.DeviceRegInfos.Add(entity);
                    db.SaveChanges();
                }
                else
                {
                    entity = db.DeviceRegInfos.FirstOrDefault(o => o.SyncItemId == guid);
                }
                return entity.SyncItemVersion;
            }
            //  throw new Exception("创建DeviceRegInfo");

        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            //    var temp = mergedData as DeviceRegInfo;
            //    using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            //    {
            //        var dbEntity = db.DeviceRegInfos.FirstOrDefault(o => o.SyncItemId == guid && o.CompanyId == companyId);
            //        dbEntity.InitEntity(temp, false);
            //        db.SaveChanges();
            //        var newEntity = db.DeviceRegInfos.FirstOrDefault(o => o.SyncItemId == guid);
            //        return newEntity.SyncItemVersion;
            //    }
            throw new SyncException("DeviceRegInfo表不允许远程修改！");

        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            throw new SyncException("DeviceRegInfo表不允许远程删除！");
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
