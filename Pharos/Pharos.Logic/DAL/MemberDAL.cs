using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Sys.Entity;
using Pharos.Utility.Helpers;
namespace Pharos.Logic.DAL
{
    internal class MemberDAL
    {
        public bool CheckMessage(Members obj, int companyId, ref string msg)
        {
            using (EFDbContext db = new EFDbContext())
            {
                if (!obj.MobilePhone.IsNullOrEmpty() && db.Set<Members>().Any(o => o.MobilePhone == obj.MobilePhone && o.Id != obj.Id && o.CompanyId == companyId))
                {
                    msg = "已存在该手机号!";
                }
                else if (!obj.Email.IsNullOrEmpty() && db.Set<Members>().Any(o => o.Email == obj.Email && o.Id != obj.Id && o.CompanyId == companyId))
                {
                    msg = "已存在该邮箱!";
                }
                else if (!obj.QQ.IsNullOrEmpty() && db.Set<Members>().Any(o => o.QQ == obj.QQ && o.Id != obj.Id && o.CompanyId == companyId))
                {
                    msg = "已存在该QQ号!";
                }
                else if (!obj.Zhifubao.IsNullOrEmpty() && db.Set<Members>().Any(o => o.Zhifubao == obj.Zhifubao && o.Id != obj.Id && o.CompanyId == companyId))
                {
                    msg = "已存在该支付宝号!";
                }
                else if (!obj.Weixin.IsNullOrEmpty() && db.Set<Members>().Any(o => o.Weixin == obj.Weixin && o.Id != obj.Id && o.CompanyId == companyId))
                {
                    msg = "已存在该微信号!";
                }
                else if (!obj.ReferrerUID.IsNullOrEmpty() && !BaseService<SysStoreUserInfo>.CurrentRepository.Entities.Any(o => o.UserCode == obj.ReferrerUID))
                {
                    msg = "推荐人工号不存在!";
                }
            }
            return string.IsNullOrEmpty(msg);
        }
    }
}
