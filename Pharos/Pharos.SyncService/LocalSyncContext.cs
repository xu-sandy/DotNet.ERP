﻿using Pharos.SyncService.LocalDataServices;
using Pharos.SyncService.SyncEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.SyncService
{
    public class LocalSyncContext : ISyncContext
    {
        public LocalSyncContext()
        {
            Register(typeof(SysStoreUserInfo), new SysStoreUserInfoSyncLocalService());
            Register(typeof(ApiLibrary), new ApiLibrarySyncLocalService());
            Register(typeof(Notice), new NoticeSyncLocalService());
            Register(typeof(DeviceRegInfo), new DeviceRegInfoSyncLocalService());
            Register(typeof(Area), new AreaSyncLocalService());
            Register(typeof(PosIncomePayout), new PosIncomePayoutSyncLocalService());
            Register(typeof(Member), new MembersSyncLocalService());
            Register(typeof(MemberIntegral), new MemberIntegralSyncLocalService());
            Register(typeof(ProductCategory), new ProductCategorySyncLocalService());
            Register(typeof(ProductRecord), new ProductRecordSyncLocalService());
            Register(typeof(SysDataDictionary), new SysDataDictionarySyncLocalService());
            Register(typeof(SalesRecord), new SalesRecordSyncLocalService());
            Register(typeof(MembershipCard), new MembershipCardSyncLocalService());
            Register("CommodityBundlingPackage", new CommodityBundlingPackageLocalService());
            Register("CommodityDiscountPackage", new CommodityDiscountPackageLocalService());
            Register("CommodityFreeGiftPackage", new CommodityFreeGiftPurchasePackageLocalService());
            Register("CommodityBlendPackage", new CommodityPromotionBlendPackageLocalService());
            Register("MemberIntegralSetPackage", new MemberIntegralSetPackageSyncLocalService());
            Register("SalePackage", new SalePackageSyncLocalService());

        }

        private void Register(string syncDataObjectType, ISyncDataService dataService)
        {
            dataServiceDict.Add(syncDataObjectType, dataService);
        }
        Dictionary<string, ISyncDataService> dataServiceDict = new Dictionary<string, ISyncDataService>();

        private void Register(Type syncDataObjectType, ISyncDataService dataService)
        {
            dataServiceDict.Add(syncDataObjectType.ToString(), dataService);
        }
        public IDictionary<string, ISyncDataService> ServiceMappings
        {
            get { return dataServiceDict; }
        }
        public bool Contains(string key)
        {
            return dataServiceDict.ContainsKey(key);
        }

        public ISyncDataService GetDataService(string key)
        {
            return dataServiceDict[key];
        }
    }
}
