using Pharos.Logic.ApiData.Pos.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.ApiData.Pos;
using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.ObjectModels.DTOs;

namespace Pharos.Logic.ApiData.Pos.Sale
{
    /// <summary>
    /// 仓库管理器
    /// </summary>
    public class WarehouseManager
    {
        public PageResult<InventoryResult> CheckedInventory(string storeId, string machineSn, int companyId, string deviceSn, IEnumerable<int> categorySns, string keyword, decimal price, int pageSize, int pageIndex)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            var result = dataAdapter.CheckedInventory(categorySns, keyword, price, pageSize, pageIndex);
            return result;
        }

        /// <summary>
        /// 获取公告
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="machineSn"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IEnumerable<Announcement> Announcements(string storeId, string machineSn, int companyId, string deviceSn)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            var result = dataAdapter.Announcements();
            return result;
        }

        /// <summary>
        /// 获取活动
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="machineSn"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IEnumerable<Activity> Activities(string storeId, string machineSn, int companyId, string deviceSn)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            var result = dataAdapter.Activities();
            return result;
        }


        public DayReportResult DayMonthReport(string storeId, string machineSn, int companyId, DateTime from, DateTime to, Range range, string deviceSn)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            var result = dataAdapter.DayMonthReport(from, to, range);
            return result;
        }
    }
}
