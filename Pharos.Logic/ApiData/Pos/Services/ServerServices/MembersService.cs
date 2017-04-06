using Pharos.Logic.ApiData.Pos.Sale.Members;
using Pharos.Logic.ApiData.Pos.Services.ServerServices;
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
    public class MembersService : BaseGeneralService<Pharos.Logic.Entity.Members, EFDbContext>
    {
        public static MemberInfo GetMemberInfo(string storeId, int companyId, string phone, string uid, string cardNo)
        {
            var memberInfo = new MemberInfo();
            Members member = null;
            if (!string.IsNullOrEmpty(phone))
            {
                member = CurrentRepository.Find(o => o.MobilePhone == phone && o.CompanyId == companyId);
                if (member != null)
                    cardNo = "";
            }
            if (member == null && !string.IsNullOrEmpty(cardNo))
            {
                var carno = Pharos.Logic.ApiData.Pos.Services.ServerServices.MembershipCardService.CurrentRepository.Find(o => o.CardSN == cardNo && o.CompanyId == companyId);
                if (carno == null)
                {
                    goto NOTFOUND;
                }
                memberInfo.UsableAmount = carno.Balance;
                cardNo = carno.CardSN;
                member = CurrentRepository.Find(o => o.CompanyId == companyId && o.MemberId == carno.MemberId);
            }
            if (member == null && !string.IsNullOrEmpty(uid))
            {
                member = CurrentRepository.Find(o => o.CompanyId == companyId && o.MemberId == uid);
            }
            if (member != null)
            {
                var cards = Pharos.Logic.ApiData.Pos.Services.ServerServices.MembershipCardService.CurrentRepository.FindList(o => o.MemberId == member.MemberId && o.CompanyId == companyId).ToList();
                if (cards != null && cards.Count() > 0)
                {
                    memberInfo.MemberCards = cards.OrderByDescending(o => o.LeadTime).ThenByDescending(o => o.CreateDT).Select(o => new MemberCard()
                       {
                           CardNo = o.CardSN,
                           Amount = o.Balance,
                           State = GetState(o.State)
                       }).ToList();
                    if (string.IsNullOrEmpty(cardNo))
                    {
                        var card = cards.FirstOrDefault(o => o.State == 1);
                        cardNo = card.CardSN;
                        memberInfo.UsableAmount = card.Balance;
                    }
                }
            }
            memberInfo.Sex = -1;
            if (member != null)
            {
                memberInfo.MemberId = member.MemberNo;
                memberInfo.RecordId = member.MemberId;
                memberInfo.Type = member.Insider ? ObjectModels.DTOs.CustomerType.Insider : ObjectModels.DTOs.CustomerType.VIP;
                memberInfo.MobilePhone = member.MobilePhone;
                memberInfo.RealName = member.RealName;
                memberInfo.UsableIntegral = member.UsableIntegral;
                memberInfo.Birthday = member.Birthday;
                memberInfo.Email = member.Email;
                memberInfo.Sex = member.Sex;
                memberInfo.WeiXin = member.Weixin;
                memberInfo.ZhiFuBao = member.Zhifubao;
                var province = AreaService.CurrentRepository.Entities.FirstOrDefault(o => o.AreaID == member.CurrentProvinceId);
                var city = AreaService.CurrentRepository.Entities.FirstOrDefault(o => o.AreaID == member.CurrentCityId);
                var county = AreaService.CurrentRepository.Entities.FirstOrDefault(o => o.AreaID == member.CurrentCountyId);

                memberInfo.Address = "";
                if (province != null && !string.IsNullOrEmpty(province.Title))
                {
                    memberInfo.Address += province.Title;
                }
                if (city != null && !string.IsNullOrEmpty(city.Title))
                {
                    memberInfo.Address += city.Title;
                }
                if (county != null && !string.IsNullOrEmpty(county.Title))
                {
                    memberInfo.Address += county.Title;
                }
                memberInfo.Address += member.Address;
            }
            memberInfo.MemberCardNum = cardNo;

            return memberInfo;

        NOTFOUND:
            throw new Pos.Exceptions.PosException("未找到会员信息！");
        }
        private static string GetState(int state)
        {
            //0:未激活；1：正常；2 已挂失；3：已作废；4 已退卡
            switch (state)
            {
                case 0:
                    return "未激活";
                case 1:
                    return "正常";
                case 2:
                    return "已挂失";
                case 3:
                    return "已作废";
                case 4:
                    return "已退卡";
                default:
                    return "未知状态";
            }
        }
    }

}
