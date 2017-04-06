using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class CommodityDataSyncService : BaseDataSyncService<Commodity, CommodityForLocal>
    {
        public override IEnumerable<Commodity> Download(string storeId, string entityType)
        {

            var result = CurrentRepository._context.Database.SqlQuery<InventoryResult>("exec CheckedInventory @p0,@p1,@p2", null, storeId, null).Select(o => new Commodity()
            {
                Barcode = o.Barcode,
                StockNumber = o.Inventory

            });
            return result;
        }
    }
}
