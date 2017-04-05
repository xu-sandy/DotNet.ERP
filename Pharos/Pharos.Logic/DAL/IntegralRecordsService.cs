using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Logic.Entity.Views;
using Pharos.Sys.Entity;
using Pharos.Utility.Helpers;


namespace Pharos.Logic.BLL
{
    public class IntegralRecordsService : BaseService<IntegralRecords>
    {

        public List<IntegralRecordViewModel> GetIntegralRecordPageList(NameValueCollection nvc, out int count)
        {
            var dicquery = BaseService<SysDataDictionary>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);

            var query = (from integral in CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId)
                         join rule in BaseService<ReturnRules>.CurrentRepository.QueryEntity
                             //on integral.IntegralRuleId equals rule.Id
                         on new { a = integral.IntegralRuleId, companyid = integral.CompanyId } equals new { a = rule.Id, companyid = rule.CompanyId }
                         join member in BaseService<Members>.CurrentRepository.QueryEntity
                             //on integral.MemberId equals member.MemberId
                         on new { a = integral.MemberId, companyid = integral.CompanyId } equals new { a = member.MemberId, companyid = member.CompanyId }
                         join u in BaseService<SysUserInfo>.CurrentRepository.QueryEntity
                             //on integral.OperatorUid equals u.UID into users
                         on new { a = integral.OperatorUid, companyid = integral.CompanyId } equals new { a = u.UID, companyid = u.CompanyId } into users
                         from userinfo in users.DefaultIfEmpty()
                         join dic in dicquery
                         on rule.OperationType equals dic.DicSN
                         join dicadapter in dicquery
                         on rule.Adapters equals dicadapter.DicSN
                         join givetype in dicquery
                         on rule.GivenType equals givetype.DicSN
                         select new IntegralRecordViewModel
                         {
                             Id = integral.Id,
                             Adapter = dicadapter.Title,
                             CID = rule.CompanyId,
                             LeftSign = rule.LeftSign,
                             ReftSign = rule.RightSign,
                             Condition = "{{left}}" + rule.Number1 + "," + (rule.Number2 == null ? "" : "{{right}}" + rule.Number2) + dic.Title + givetype.Title + rule.Expression,
                             CreateDT = integral.CreateDt,
                             Intergal = integral.Integral,
                             Member = member.RealName + (member.MobilePhone == "" ? "" : "(" + member.MobilePhone + ")"),
                             Memo = "",
                             OrderSn = (integral.SourceType == 1 ? BaseService<SaleOrders>.CurrentRepository.QueryEntity.FirstOrDefault(o => o.PaySN == integral.Source).CustomOrderSn :
                                       integral.SourceType == 2 ? BaseService<MemberRecharge>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId).ToList().FirstOrDefault(o => o.Id.ToString() == integral.Source).RechargeSN : ""),
                             ReturnType = BaseService<InstalmentRecord>.CurrentRepository.QueryEntity.Where(o => o.IntegralRecordId == integral.Id).Count() > 0 ? "分期" : "即时",
                             State = "",
                             Store = "",
                             Type = "",
                             Unit = dic.Title,
                             User = userinfo.FullName
                         });
            count = query.Count();
            //积分记录  返赠方案 字典 会员 用户
            var pageIndex = 1;
            var pageSize = 0;
            if (!nvc["page"].IsNullOrEmpty())
                pageIndex = int.Parse(nvc["page"]);
            if (!nvc["rows"].IsNullOrEmpty())
                pageSize = int.Parse(nvc["rows"]);
            var result = query.OrderByDescending(o => o.CreateDT).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            foreach (var item in result)
            {
                if (item.LeftSign > 0)
                {
                    var leftSignStr = Pharos.Logic.MemberDomain.QuanChengTaoProviders.Extensions.EnumExtensions.GetEnumDescription(item.LeftSign, typeof(Pharos.Logic.MemberDomain.QuanChengTaoProviders.LogicalOperatorType));
                    item.Condition = item.Condition.Replace("{{left}}", leftSignStr);
                }
                if (item.ReftSign.HasValue)
                {
                    var rightSignStr = Pharos.Logic.MemberDomain.QuanChengTaoProviders.Extensions.EnumExtensions.GetEnumDescription((int)item.ReftSign, typeof(Pharos.Logic.MemberDomain.QuanChengTaoProviders.LogicalOperatorType));
                    item.Condition = item.Condition.Replace("{{right}}", rightSignStr);
                }
            }
            return result;
        }

        public object GetIntegralRecordDetailPageList(string id, out int count)
        {
            var query = BaseService<InstalmentRecord>.CurrentRepository.QueryEntity.Where(o => o.IntegralRecordId == id).OrderByDescending(o => o.InstalmentDT);
            count = query.Count();
            var result = query.ToPageList();
            return result;
        }
    }
}
