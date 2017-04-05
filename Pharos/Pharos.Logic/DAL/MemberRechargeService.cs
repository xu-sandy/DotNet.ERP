﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Sys.Entity;
using Pharos.Utility.Helpers;

namespace Pharos.Logic.BLL
{
    public class MemberRechargeService : BaseService<MemberRecharge>
    {
        /// <summary>
        /// get datagrid datas by where 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="keyWold"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public object FindRechargePageList(DateTime? beginDate, DateTime? endDate, string keyWold, int keywordType, int selectType, out int count, ref object footer)
        {
            var query = from a in CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId && o.IsTest == false && o.Type == selectType)
                        join b in BaseService<MembershipCard>.CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId)
                        on a.CardId equals b.CardSN into m
                        from lgu in m.DefaultIfEmpty()
                        join c in BaseService<Members>.CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId)
                        on lgu.MemberId equals c.MemberId into n
                        from ac in n.DefaultIfEmpty()
                        join l in BaseService<SysUserInfo>.CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId)
                        on a.CreateUID equals l.UID into d
                        from def in d.DefaultIfEmpty()
                        select new { a, lgu, ac, d };


            if (beginDate.HasValue)
            {
                query = query.Where(o => o.a.CreateDT > beginDate);
            }
            if (endDate.HasValue)
            {
                endDate = endDate.Value.AddDays(1);
                query = query.Where(o => o.a.CreateDT < endDate.Value);
            }
            if (keywordType == 1)
            {//会员信息 
                if (!string.IsNullOrEmpty(keyWold))
                {
                    query = query.Where(o => o.ac.MemberNo.Contains(keyWold) || o.ac.MobilePhone.Contains(keyWold) || o.ac.RealName.Contains(keyWold));
                }
            }
            else if (keywordType == 2)
            {//卡号
                if (!string.IsNullOrEmpty(keyWold))
                {
                    query = query.Where(o => o.lgu.CardSN.Contains(keyWold));
                }
            }

            count = query.Count();
            query = query.OrderByDescending(o => o.a.CreateDT);
            if (count > 0)
            {
                var rechargeTotal = query.Sum(o => o.a.RechargeAmount);
                var totalTitle = "累计充值：";
                if (selectType == 2)
                {
                    totalTitle = "累计反结算：";
                }
                footer = new List<object> { new { BeforChargeMonunt = totalTitle, RechargeAmount = rechargeTotal } };
                //exits data
                return query.Select(o => new
                {
                    //cardsn user money rechangemoney giftmoney balance createdate type memo douser
                    Id = o.a.Id,
                    CardSN = o.lgu.CardSN,
                    MemberFullName = o.ac.RealName,
                    BeforChargeMonunt = o.a.BeforeAmount,
                    RechargeAmount = o.a.RechargeAmount,
                    GivenAmount = o.a.GivenAmount,
                    PresentExp = o.a.PresentExp,
                    Blance = o.a.AfterAmount,
                    CreateDT = o.a.CreateDT,
                    CreateUID = o.d.FirstOrDefault() == null ? "" : o.d.FirstOrDefault().FullName,
                    Memo = o.a.Memo
                }).ToPageList();
            }
            else
            {
                return null;
            }
        }
    }
}
