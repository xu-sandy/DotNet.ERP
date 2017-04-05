using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity;
using Pharos.Logic.ApiData.Pos.User;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services.LocalCeServices
{
    public class SysStoreUserInfoService : BaseGeneralService<SysStoreUserInfo, LocalCeDbContext>
    {
        public static UserInfo GetStoreUserInfo(string account, string storeId, int companyId)
        {

            var query = CurrentRepository.Entities.Where(o => o.UserCode == account);
            var user = query.Select(o => new UserInfo()
            {
                FullName = o.FullName,
                LoginDT = o.LoginDT,
                LoginPwd = o.LoginPwd,
                OperateAuth = o.OperateAuth,
                Sex = o.Sex,
                UID = o.UID,
                UserCode = o.UserCode
            }).ToList();
            var info = user.FirstOrDefault();
            //160504更新时间
            if (info != null)
            {
                var _queryUser = query.FirstOrDefault();
                _queryUser.LoginDT = DateTime.Now;
                CurrentRepository.Update(_queryUser);
                info.StoreName = System.Configuration.ConfigurationManager.AppSettings["StoreName"];
            }
            return info;
        }

        public static IEnumerable<UserInfo> GetStoreManagers(string storeId, StoreOperateAuth storeOperateAuth, int companyId)
        {
            var auth = ((int)storeOperateAuth).ToString(); 
            var storename= System.Configuration.ConfigurationManager.AppSettings["StoreName"];
            var sqlStr = @"select s.*,'" + storename + "' StoreName from SysStoreUserInfo s where '|'+OperateAuth+'|' like '%|" + storeId + "," + auth + "|%'";
            return CurrentRepository._context.Database.SqlQuery<UserInfo>(sqlStr).ToList();
            //var query = CurrentRepository.Entities.Where(o => o.OperateAuth.Contains(storeId + "," + auth)).Select(o => new UserInfo()
            //{
            //    FullName = o.FullName,
            //    LoginDT = o.LoginDT,
            //    LoginPwd = o.LoginPwd,
            //    OperateAuth = o.OperateAuth,
            //    Sex = o.Sex,
            //    UID = o.UID,
            //    UserCode = o.UserCode
            //}).ToList();
            //return result;
        }
    }
}
