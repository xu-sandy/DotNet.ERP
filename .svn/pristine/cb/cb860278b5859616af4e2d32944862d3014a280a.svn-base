using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using Pharos.Logic.DAL;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services
{
    public class ConsumptionPaymentService : BaseGeneralService<Pharos.Logic.Entity.ConsumptionPayment, EFDbContext>
    {

        public static void Save(string paySn, decimal amount, int apiCode,decimal change,decimal receive, string apiOrderSN = null, string cardNo = null, int companyId = 1)
        {
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
              });
        }
    }
}
