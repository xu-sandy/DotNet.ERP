﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Entity.View;

namespace Pharos.Logic.OMS.IDAL
{
    /// <summary>
    /// 商家结算账户
    /// </summary>
    public interface IBankAccountRepository : IBaseRepository<BankAccount>
    {
        List<ViewBankAccount> getPageList(int CurrentPage, int PageSize, string strw, out int Count);
    }
}
