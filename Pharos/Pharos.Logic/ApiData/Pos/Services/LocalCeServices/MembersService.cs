﻿using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services.LocalCeServices
{
    public class MembersService : BaseGeneralService<Members, LocalCeDbContext>
    {
        public static MemberInfo GetMemberInfo(string storeId, int companyId, string phone, string uid, string cardNo)
        {
            var memberInfo = new MemberInfo();
            var member = new Members();
            if (!string.IsNullOrEmpty(phone))
            {
                member = CurrentRepository.Find(o => o.MobilePhone == phone && o.CompanyId == companyId);
            }
            else if (!string.IsNullOrEmpty(cardNo))
            {
                var carno = MembershipCardService.CurrentRepository.Find(o => o.CardSN == cardNo);
                member = CurrentRepository.Find(o => o.CompanyId == companyId && o.MemberId == carno.MemberId);
            }

            if (member != null)
            {
                memberInfo.MemberId = member.MemberId;
                memberInfo.MobilePhone = member.MobilePhone;
                memberInfo.RealName = member.RealName;
                memberInfo.MemberCardNum = cardNo;
                memberInfo.UsableIntegral = member.UsableIntegral;
                return memberInfo;
            }
            throw new Pos.Exceptions.PosException("未找到会员信息！");
        }
    }
}
