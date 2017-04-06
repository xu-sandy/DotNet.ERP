using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity;
using Pharos.Logic.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services.LocalCeServices
{
    public class ConsumptionPaymentService : BaseGeneralService<ConsumptionPayment, LocalCeDbContext>
    {
        public static void Save(string paySn, decimal amount, int apiCode, decimal change, decimal receive, string apiOrderSN = null, string cardNo = null, int companyId = 1)
        {
            var version = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

            CurrentRepository.Add(new ConsumptionPayment()
            {
                Amount = amount,
                ApiCode = apiCode,
                PaySN = paySn,
                State = 1,
                ApiOrderSN = apiOrderSN,
                CardNo = cardNo,
                CompanyId = companyId,
                Change = change,
                Received = receive,
                SyncItemId = Guid.NewGuid(),
                SyncItemVersion = version
            });
        }
    }
}
