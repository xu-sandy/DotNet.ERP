using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.AfterSale
{
    public class BillHistory
    {
        public static BillHistoryInfo GetBillDetails(string storeId, string machineSn, int companyId, string paySn, string deviceSn)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            return dataAdapter.GetBillDetailsHistory(paySn);
        }

        public static IEnumerable<BillListItem> GetBills(string storeId, string machineSn, int companyId,string queryMachineSn, DateTime date, Range range, string deviceSn, string paySn, string cashier)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, deviceSn);
            return dataAdapter.GetBills(date, range, paySn, cashier,queryMachineSn);
        }
    }
}
