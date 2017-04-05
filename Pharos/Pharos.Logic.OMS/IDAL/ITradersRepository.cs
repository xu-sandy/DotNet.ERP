﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Entity.View;

namespace Pharos.Logic.OMS.IDAL
{
    public interface ITradersRepository : IBaseRepository<Traders>
    {
        List<ViewTrader> getPageList(int CurrentPage, int PageSize, string strw, out int Count);

        /// <summary>
        /// 获取部门ID
        /// </summary>
        List<ViewDepart> getDepartID(int DeptId = 0);
    }
}
