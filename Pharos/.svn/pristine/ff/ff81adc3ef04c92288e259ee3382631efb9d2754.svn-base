using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.SyncService.Helpers;
using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.DAL;
using Pharos.SyncService.SyncEntities;
using Pharos.SyncService.Exceptions;

namespace Pharos.SyncService.RemoteDataServices
{
    public class MembershipCardSyncRemoteService : ISyncDataService
    {
        public TimeSpan SyncInterval
        {
            get
            {
                return new TimeSpan(0, 30, 0);
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
                    var result = db.MembershipCards.Where(o => o.CompanyId == companyId).Select(o => new SyncDataObject() { SyncItemId = o.SyncItemId, SyncItemVersion = o.SyncItemVersion }).ToList();
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
                var entity = db.MembershipCards.Where(o => o.SyncItemId == guid && o.CompanyId == companyId).FirstOrDefault();
                if (entity != null)
                    return new Pharos.SyncService.SyncEntities.MembershipCard().InitEntity(entity);
                return null;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.MembershipCards.Where(o => syncids.Contains(o.SyncItemId)).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => (ISyncDataObject)new Pharos.SyncService.SyncEntities.MembershipCard().InitEntity(o));
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            throw new SyncException("MembershipCard表不允许远程创建！");
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            throw new SyncException("MembershipCard表不允许远程更新！");
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            throw new SyncException("MembershipCard表不允许远程删除！");
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
