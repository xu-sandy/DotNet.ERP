﻿using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.ValueObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos
{
    /// <summary>
    /// 设备设置
    /// </summary>
    public class MachinesSettings
    {
        static MachinesSettings()
        {
#if (Local== true)
            Mode = DataAdapterMode.SQLSERVERCE;
            CachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache");

#endif
#if (Local!= true)
            Mode = DataAdapterMode.SQLSERVER;
            CachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache");

#endif
            OrderSnIdentifierChar = "S";
            try
            {
                ReloadMarketingInterval = Convert.ToInt32(ConfigurationManager.AppSettings["RefreshMakectingInterval"]);//30分钟刷新一次促销
            }
            catch
            {
                ReloadMarketingInterval = 30;
            }
        }
        /// <summary>
        /// 数据源模式（默认启动SQLSERVER）
        /// </summary>
        public static DataAdapterMode Mode { get; set; }

        /// <summary>
        /// 缓存文件路径
        /// </summary>
        public static string CachePath { get; set; }
        /// <summary>
        /// 自动刷新促销活动间隔（分钟）
        /// </summary>
        public static int ReloadMarketingInterval { get; set; }

        /// <summary>
        /// 订单编号起始标识符
        /// </summary>
        public static string OrderSnIdentifierChar { get; set; }
        /// <summary>
        /// 设备注册
        /// </summary>
        /// <param name="storeId">门店ID</param>
        /// <param name="machineSn">POS机编号</param>
        /// <param name="deviceSn">POS机唯一标识</param>
        /// <param name="type">POS机类型（PC = 1,PAD = 2,Mobile = 3）</param>
        public static void RegisterDevice(string storeId, string machineSn, int companyId, string deviceSn, DeviceType type)
        {
            if (!HasRegister(storeId, machineSn, companyId, deviceSn, type, false))
            {
                var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, DataAdapterFactory.DEFUALT);
                dataAdapter.RegisterDevice(deviceSn, type);
            }
        }

        /// <summary>
        /// 查询设备注册
        /// </summary>
        /// <param name="storeId">门店ID</param>
        /// <param name="machineSn">POS机编号</param>
        /// <param name="deviceSn">POS机唯一标识</param>
        /// <param name="type">POS机类型（PC = 1,PAD = 2,Mobile = 3）</param>
        /// <returns></returns>
        public static bool HasRegister(string storeId, string machineSn, int companyId, string deviceSn, DeviceType type, bool verfyState = true)
        {
            try
            {
                var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, DataAdapterFactory.DEFUALT);
                return dataAdapter.HasRegister(deviceSn, type, verfyState);
            }
            catch (Exception ex)
            {
                throw;

            }
        }
    }
}
