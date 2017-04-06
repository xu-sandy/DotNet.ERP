using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class ConsumptionPaymentDataSyncService : BaseDataSyncService<ConsumptionPayment, ConsumptionPaymentForLocal>
    {
        public override IEnumerable<ConsumptionPayment> Download(string storeId, string entityType)
        {
            ConsumptionPaymentDataSyncService.IsForcedExpired = true;

            var expirationDate = DateTime.Now.Date.AddYears(-1);

            var query = (from a in CurrentRepository.Entities
                         from b in SaleOrdersDataSyncService.CurrentRepository.Entities
                         where a.PaySN == b.PaySN && b.StoreId == storeId && b.CreateDT > expirationDate
                         select a).ToList();
            return query;
        }
        public override bool UpLoad(IEnumerable<ConsumptionPayment> datas, string storeId)
        {
            var serverRepository = CurrentRepository;

            if (datas == null)
            {
                return false;
            }
            //删除重复数据
            var paySNs = CurrentRepository.Entities.Select(o => o.PaySN).ToList();
            datas = datas.Where(o => !paySNs.Exists(p => p == o.PaySN));

            var tempDatas = datas.Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<ConsumptionPayment>(o));
            serverRepository.AddRange(tempDatas.ToList());
            return true;
        }
    }
}
