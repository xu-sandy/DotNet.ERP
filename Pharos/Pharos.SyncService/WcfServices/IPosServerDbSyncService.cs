using Microsoft.Synchronization;
using Pharos.SyncService.SyncEntities;
using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Pharos.SyncService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IPosServerDbSyncService”。
    [ServiceContract(SessionMode = SessionMode.Required)]
    [ServiceKnownType(typeof(SyncIdFormatGroup))]
    [ServiceKnownType(typeof(SyncId))]
    [ServiceKnownType(typeof(SysStoreUserInfo))]
    [ServiceKnownType(typeof(ApiLibrary))]
    [ServiceKnownType(typeof(Notice))]
    [ServiceKnownType(typeof(DeviceRegInfo))]
    [ServiceKnownType(typeof(PosIncomePayout))]
    [ServiceKnownType(typeof(Member))]
    [ServiceKnownType(typeof(MemberIntegral))]
    [ServiceKnownType(typeof(ProductCategory))]
    [ServiceKnownType(typeof(ProductRecord))]
    [ServiceKnownType(typeof(SysDataDictionary))]
    [ServiceKnownType(typeof(SalesRecord))]
    [ServiceKnownType(typeof(Package))]
    [ServiceKnownType(typeof(SaleOrders))]
    [ServiceKnownType(typeof(SaleDetail))]
    [ServiceKnownType(typeof(WipeZero))]
    [ServiceKnownType(typeof(ConsumptionPayment))]
    [ServiceKnownType(typeof(Bundling))]
    [ServiceKnownType(typeof(Area))]
    [ServiceKnownType(typeof(MembershipCard))]
    [ServiceKnownType(typeof(BundlingList))]
    [ServiceKnownType(typeof(CommodityDiscount))]
    [ServiceKnownType(typeof(CommodityPromotion))]
    [ServiceKnownType(typeof(FreeGiftPurchase))]
    [ServiceKnownType(typeof(FreeGiftPurchaseList))]
    [ServiceKnownType(typeof(MemberIntegralSet))]
    [ServiceKnownType(typeof(MemberIntegralSetList))]
    [ServiceKnownType(typeof(PromotionBlend))]
    [ServiceKnownType(typeof(PromotionBlendList))]
    public interface IPosServerDbSyncService
    {
        [OperationContract(
            IsInitiating = true,
            IsTerminating = false)]
        void CreateProviderForSyncSession(int companyId, string storeId, string syncServiceName);

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false)]
        SyncIdFormatGroup GetIdFormats();

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false)]
        void BeginSession();

        [OperationContract(
            IsInitiating = false,
            IsTerminating = true)]
        void EndSession();

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false)]
        ChangeBatch GetChangeBatch(
            uint batchSize,
            SyncKnowledge destinationKnowledge,
            out CachedChangeDataRetriever changeDataRetriever);

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false)]
        FullEnumerationChangeBatch GetFullEnumerationChangeBatch(
            uint batchSize,
            SyncId lowerEnumerationBound,
            SyncKnowledge knowledgeForDataRetrieval,
            out CachedChangeDataRetriever changeDataRetriever);

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false)]
        void GetSyncBatchParameters(
            out uint batchSize,
            out SyncKnowledge knowledge);

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false)]
        byte[] ProcessChangeBatch(
            ConflictResolutionPolicy resolutionPolicy,
            ChangeBatch sourceChanges,
            CachedChangeDataRetriever changeDataRetriever,
            byte[] changeApplierInfo);

        [OperationContract(
            IsInitiating = false,
            IsTerminating = false)]
        byte[] ProcessFullEnumerationChangeBatch(
            ConflictResolutionPolicy resolutionPolicy,
            FullEnumerationChangeBatch sourceChanges,
            CachedChangeDataRetriever changeDataRetriever,
            byte[] changeApplierInfo);

        #region For demo purpose, not required for RCA pattern
        [OperationContract(
            IsInitiating = false,
            IsTerminating = false)]
        void CleanupTombstones(TimeSpan timespan);
        #endregion
    }
}