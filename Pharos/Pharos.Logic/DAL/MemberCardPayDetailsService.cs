﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Logic.Entity.Views;
using Pharos.Utility.Helpers;


namespace Pharos.Logic.DAL
{
    /// <summary>
    /// 会员卡消费明细CRM-DAL
    /// </summary>
    public class MemberCardPayDetailsService : BaseService<Vw_MembershipCardPayDetails>
    {
        //支付信息   订单  卡类型 
        public List<MemberCardPayDetailsViewModel> GetMemberCardPayDetailsByPageList(string cardType, DateTime? beginDate, DateTime? endDate, int? storeIds, string cardNo, out int count, ref object footer)
        {
            if (endDate.HasValue)
            {
                endDate = (DateTime)endDate.Value.AddDays(1);
            }

            var where = DynamicallyLinqHelper.Empty<Vw_MembershipCardPayDetails>().And(o => o.CompanyId == CommonService.CompanyId)
                               .And(o => o.CreateDT >= beginDate, !beginDate.HasValue)
                               .And(o => o.CreateDT < endDate, !endDate.HasValue)
                               .And(o => o.CardTypeId == cardType, cardType.IsNullOrEmpty())
                               .And(o => o.StoreId == storeIds, !storeIds.HasValue)
                               .And(o => o.CardSN == cardNo, cardNo.IsNullOrEmpty());
            var result = CurrentRepository.Entities.Where(where).Select(o => new MemberCardPayDetailsViewModel()
            {
                Id = o.Id,
                OrderSN = o.OrderSN,
                CardSN = o.CardSN,
                CardType = o.CardType,
                CreateDT = o.CreateDT,
                Price = o.Price,
                StoreTitle = o.StoreTitle
            });
            var payTotal = 0m;
            if (result.Count() > 0)
            {
                payTotal = result.Sum(o => o.Price);
            }
            footer = new List<object> { new { CardSN = "累计消费：", Price = payTotal } };
            count = result.Count();
            return result.ToPageList();
        }
    }
}
