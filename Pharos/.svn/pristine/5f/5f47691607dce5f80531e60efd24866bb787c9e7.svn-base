using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.ApiData.Pos.Extensions;
using Pharos.Logic.BLL;
using Pharos.Logic.DAL;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Pharos.ObjectModels.DTOs;

namespace Pharos.Logic.ApiData.Pos.Services
{
    public class WarehouseService : BaseGeneralService<Warehouse, EFDbContext>
    {
        public static PageResult<InventoryResult> CheckedInventory(string storeId, int companyId, IEnumerable<int> categorySns, string keyword, decimal price, int pageSize, int pageIndex)
        {
            try
            {
                string categorySnsStr = null;
                if (categorySns != null)
                    categorySnsStr = string.Join(",", categorySns.Select(o => o.ToString()));
                var result = CurrentRepository._context.Database.SqlQuery<InventoryResult>("exec CheckedInventory @p0,@p1,@p2,@p3,@p4", categorySnsStr, storeId, keyword, price, companyId).ToPageList(pageSize, pageIndex);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static IEnumerable<InventoryResult> CheckedPrice(string storeId, int companyId, IEnumerable<int> categorySns, decimal from, decimal to)
        {
            try
            {
                string categorySnsStr = null;
                if (categorySns != null)
                    categorySnsStr = string.Join(",", categorySns.Select(o => o.ToString()));
                var result = CurrentRepository._context.Database.SqlQuery<InventoryResult>("exec CheckedPrice @p0,@p1,@p2,@p3,@p4", categorySnsStr, storeId, from, to, companyId).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
