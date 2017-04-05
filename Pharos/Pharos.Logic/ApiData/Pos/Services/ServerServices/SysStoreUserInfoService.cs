﻿using Pharos.Logic.BLL;
using Pharos.Logic.DAL;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Sys.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Logic.ApiData.Pos.User;

namespace Pharos.Logic.ApiData.Pos.Services
{
    public class SysStoreUserInfoService : BaseGeneralService<SysStoreUserInfo, EFDbContext>
    {

        public static UserInfo GetStoreUserInfo(string account, string storeId, int companyId)
        {

            var query = CurrentRepository.Entities.Where(o => o.UserCode == account && o.CompanyId == companyId);
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
            if (info != null)
            {
                //160504更新登录时间
                var _queryUser = query.FirstOrDefault();
                _queryUser.LoginDT = DateTime.Now;
                CurrentRepository.Update(_queryUser);
                //160504
                var store = BaseService<Warehouse>.CurrentRepository.Entities.FirstOrDefault(o => o.StoreId == storeId && o.CompanyId == companyId);
                if (store != null)
                    info.StoreName = store.Title;
                else
                    info.StoreName = "未知门店";
            }
            return info;
        }

        public static IEnumerable<UserInfo> GetStoreUsers(string storeId, StoreOperateAuth storeOperateAuth, int companyId)
        {
            var auth = ((int)storeOperateAuth).ToString();
            var query = CurrentRepository.Entities.Where(o => ("|" + o.OperateAuth + "|").Contains("|" + storeId + "," + auth + "|") && o.CompanyId == companyId).Select(o => new UserInfo()
             {
                 FullName = o.FullName,
                 LoginDT = o.LoginDT,
                 LoginPwd = o.LoginPwd,
                 OperateAuth = o.OperateAuth,
                 Sex = o.Sex,
                 UID = o.UID,
                 UserCode = o.UserCode
             }).ToList();
            return query;
        }
    }
}
