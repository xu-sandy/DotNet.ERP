using Microsoft.Synchronization;
using Pharos.SyncService.SyncProviders;
using System;
using System.ServiceModel;
using System.Collections;
using System.Collections.Generic;

namespace Pharos.SyncService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“PosServerDbSyncService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 PosServerDbSyncService.svc 或 PosServerDbSyncService.svc.cs，然后开始调试。
    [ServiceBehavior(
       ConcurrencyMode = ConcurrencyMode.Single,
       InstanceContextMode = InstanceContextMode.PerSession,
       IncludeExceptionDetailInFaults = true)]
    public class PosServerDbSyncService : IPosServerDbSyncService
    {
        private string _storeId;
        private int _companyId;
        string syncServiceName;

        private PosDbSyncProvider FindProvider(bool isNew = false)
        {
            var syncProvider = SyncProviderFactory.Factory(_companyId, _storeId, syncServiceName, isNew);
            return syncProvider;
        }

        public SyncIdFormatGroup GetIdFormats()
        {
            return FindProvider().IdFormats;
        }

        public void CreateProviderForSyncSession(int companyId, string storeId, string syncServiceName)
        {
            _companyId = companyId;
            _storeId = storeId;
            this.syncServiceName = syncServiceName;
            FindProvider(true);
        }

        public void BeginSession()
        {
            FindProvider().BeginSession();
        }

        public void EndSession()
        {
            FindProvider().EndSession();
        }

        public ChangeBatch GetChangeBatch(
            uint batchSize,
            SyncKnowledge destinationKnowledge, 
            out CachedChangeDataRetriever changeDataRetriever)
        {
            object dataRetriever;

            ChangeBatch changeBatch = FindProvider().GetChangeBatch(
                batchSize,
                destinationKnowledge,
                out dataRetriever);

            changeDataRetriever = new CachedChangeDataRetriever(
                dataRetriever as IChangeDataRetriever,
                changeBatch);

            return changeBatch;
        }

        public FullEnumerationChangeBatch GetFullEnumerationChangeBatch(
            uint batchSize,
            SyncId lowerEnumerationBound,
            SyncKnowledge knowledgeForDataRetrieval,
            out CachedChangeDataRetriever changeDataRetriever)
        {
            object dataRetriever;

            FullEnumerationChangeBatch changeBatch = FindProvider().GetFullEnumerationChangeBatch(
                batchSize,
                lowerEnumerationBound,
                knowledgeForDataRetrieval,
                out dataRetriever);

            changeDataRetriever = new CachedChangeDataRetriever(
                dataRetriever as IChangeDataRetriever,
                changeBatch);

            return changeBatch;
        }

        public void GetSyncBatchParameters(
            out uint batchSize,
            out SyncKnowledge knowledge)
        {
            FindProvider().GetSyncBatchParameters(
                out batchSize,
                out knowledge);
        }

        public byte[] ProcessChangeBatch(
            ConflictResolutionPolicy resolutionPolicy,
            ChangeBatch sourceChanges,
            CachedChangeDataRetriever changeDataRetriever,
            byte[] changeApplierInfo)
        {
            return FindProvider().ProcessRemoteChangeBatch(
                resolutionPolicy,
                sourceChanges,
                changeDataRetriever,
                changeApplierInfo);
        }

        public byte[] ProcessFullEnumerationChangeBatch(
            ConflictResolutionPolicy resolutionPolicy,
            FullEnumerationChangeBatch sourceChanges,
            CachedChangeDataRetriever changeDataRetriever,
            byte[] changeApplierInfo)
        {
            return FindProvider().ProcessRemoteFullEnumerationChangeBatch(
                resolutionPolicy,
                sourceChanges,
                changeDataRetriever,
                changeApplierInfo);
        }

        #region For demo purpose, not required for RCA pattern
        public void CleanupTombstones(TimeSpan timespan)
        {
            FindProvider().CleanupTombstones(timespan);
        }

        #endregion
    }
}
