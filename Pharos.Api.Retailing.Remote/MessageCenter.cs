using Pharos.Infrastructure.Data.Normalize;
using Pharos.Logic.ApiData.Pos;
using Pharos.Logic.ApiData.Pos.Cache;
using Pharos.MessageTransferAgenClient.DomainEvent;
using Pharos.ObjectModels.Events;

namespace Pharos.Api.Retailing
{
    public class MessageCenter
    {
        public static void Sub()
        {
            new EventAggregator().Subscribe<SyncMemberNoEvent>("SyncMemberNoEventForServer", (o) =>
            {
                MemberNo memberNo = new MemberNo(o.CompanyId, o.StoreId);
                if (memberNo.GetNumber() < o.Number)
                {
                    memberNo.Reset(o.Number);
                }
            });

            new EventAggregator().Subscribe<SyncOnlineEvent>("SyncOnlineEventForServer", (o) =>
            {
                var cache = new OnlineCache();
                var key = KeyFactory.MachineKeyFactory(o.CompanyToken, o.StoreId, o.MachineSn, o.DeviceSn);
                cache.Set(key, new MachineInformation()
                {
                    CashierName = o.CashierName,
                    CashierOperateAuth = o.CashierOperateAuth,
                    CashierUid = o.CashierUid,
                    CashierUserCode = o.CashierUserCode,
                    CompanyToken = o.CompanyToken,
                    DeviceSn = o.DeviceSn,
                    InTestMode = o.InTestMode,
                    MachineSn = o.MachineSn,
                    StoreId = o.StoreId,
                    StoreName = o.StoreName
                });
            });

            new EventAggregator().Subscribe<SyncSerialNumberEvent>("SyncSerialNumberEventForServer", (o) =>
            {
                PaySn customOrderSn = new PaySn(o.CompanyToken, o.StoreId, o.MachineSn);
                if (customOrderSn.GetNumber() < o.Number)
                {
                    customOrderSn.ResetSerialNumber(o.Number);
                }
            });

        }
    }
}