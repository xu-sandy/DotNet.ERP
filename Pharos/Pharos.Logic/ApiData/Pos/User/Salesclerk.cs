﻿using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.ApiData.Pos.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.ApiData.Pos.Sale;
using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.Cache;
using Pharos.Infrastructure.Data.Normalize;
using Pharos.Logic.ApiData.Pos.Security;
using Pharos.Infrastructure.Data.Redis;
using Newtonsoft.Json;

namespace Pharos.Logic.ApiData.Pos.User
{
    /// <summary>
    /// 店员权限及登录验证
    /// </summary>
    public static class Salesclerk
    {
      public static  OnlineCache onlineCache = new OnlineCache();

        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <param name="userParams">登录参数</param>
        /// <returns></returns>
        public static LoginResult Login(LoginParams userParams, string deviceSn, bool inTestMode = false, bool isLock = false)
        {

            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, userParams.StoreId, userParams.MachineSn, userParams.CompanyId, deviceSn);
            var userInfo = dataAdapter.GetUser(userParams.Account);
            if (userInfo == null)
            {
                throw new LoginExecption("401", "账号错误！");
            }
            if (userInfo.LoginPwd != MD5.MD5Encrypt(userParams.Password))
            {
                throw new LoginExecption("401", "密码错误！");
            }
            //本店角色（1:店长、2:营业员、3:收银员、4:数据维护），格式：门店ID,角色ID|门店ID,角色ID
            if (!inTestMode && !(VerfyOperateAuth(userInfo, dataAdapter.StoreId, StoreOperateAuth.Cashier) || VerfyOperateAuth(userInfo, dataAdapter.StoreId, StoreOperateAuth.DataManager)))
            {
                throw new LoginExecption("402", "非销售员或数据维护员不允许登录销售！");
            }
            var key = KeyFactory.MachineKeyFactory(userParams.CompanyId, userParams.StoreId, userParams.MachineSn, deviceSn);
            var machineInfo = new MachineInformation()
            {
                CashierName = userInfo.FullName,
                CashierOperateAuth = userInfo.OperateAuth,
                CashierUid = userInfo.UID,
                CashierUserCode = userInfo.UserCode,
                StoreName = userInfo.StoreName,
                StoreId = userParams.StoreId,
                MachineSn = userParams.MachineSn,
                CompanyId = userParams.CompanyId,
                InTestMode = inTestMode,
                DeviceSn = deviceSn
            };
            onlineCache.Set(key, machineInfo);
#if(Local!=true)
            RedisManager.Publish("SyncOnlineCache", JsonConvert.SerializeObject(machineInfo));
#endif
#if(Local)
            StoreManager.PubEvent("SyncOnlineCache", JsonConvert.SerializeObject(machineInfo));
#endif
            if (!isLock)
                ShoppingCartFactory.Factory(userParams.StoreId, userParams.MachineSn, userParams.CompanyId, deviceSn, true);
            return new LoginResult()
            {
                FullName = userInfo.FullName,
                OperateAuth = userInfo.OperateAuth,
                UserCode = userInfo.UserCode,
                StoreName = userInfo.StoreName
            };
        }

        internal static MachineInformation GetMachineInfo(string storeId, string machineSn, int companyToken, string deviceSn)
        {

            var key = KeyFactory.MachineKeyFactory(companyToken, storeId, machineSn, deviceSn);

            return onlineCache.Get(key);
        }
        private static bool VerfyOperateAuth(UserInfo userInfo, string storeId, StoreOperateAuth storeOperateAuth)
        {
            return ("|" + userInfo.OperateAuth + "|").Contains(string.Format("|{0},{1}|", storeId, (int)storeOperateAuth));
        }
        /// <summary>
        /// 获取满足条件的用户信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="machineSn"></param>
        /// <param name="companyId"></param>
        /// <param name="storeOperateAuth"></param>
        /// <returns></returns>
        public static IEnumerable<UserInfo> GetAuthUsers(string storeId, string machineSn, int companyId, StoreOperateAuth storeOperateAuth)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, DataAdapterFactory.DEFUALT);
            var users = dataAdapter.GetStoreUsers(storeOperateAuth);
            return users;
        }
        public static bool VerfyStoreManagerOperateAuth(string storeId, string machineSn, int companyId, string password)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, DataAdapterFactory.DEFUALT);
            var storeManagers = dataAdapter.GetStoreUsers(StoreOperateAuth.ShopManager);
            foreach (var item in storeManagers)
            {
                if (VerfyOperateAuth(item, storeId, StoreOperateAuth.ShopManager) && item.LoginPwd == MD5.MD5Encrypt(password))
                {
                    return true;
                }
            }
            return false;
        }

        public static void PosIncomePayout(string storeId, string machineSn, int companyId, string userCode, string password, decimal money, PosIncomePayoutMode mode)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, DataAdapterFactory.DEFUALT);
            var userInfo = dataAdapter.GetUser(userCode);
            if (userInfo == null)
            {
                throw new LoginExecption("401", "账号错误！");
            }
            if (userInfo.LoginPwd != MD5.MD5Encrypt(password))
            {
                throw new LoginExecption("401", "密码错误！");
            }
            dataAdapter.PosIncomePayout(userInfo.UID, money, mode);
        }
    }
}
