using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Entity.View;

namespace Pharos.Logic.OMS.IDAL
{
    /// <summary>
    /// 商家门店
    /// </summary>
    public interface ITradersStoreRepository : IBaseRepository<TradersStore>
    {
        List<ViewTradersStore> getPageList(int CurrentPage, int PageSize, string strw, out int Count);

        /// <summary>
        /// 获取最大门店编号
        /// </summary>
        ViewMaxStoreNum getMaxStoreNum();
    }
}
