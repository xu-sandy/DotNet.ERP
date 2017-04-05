﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.DAL;
using Pharos.Logic.Entity;

namespace Pharos.Logic.BLL
{
    public class ScaleHistoryBLL
    {
        private readonly ScaleHistoryService service = new ScaleHistoryService();

        public List<string> GetAllHistoryByStore()
        {
            var store = Sys.SysCommonRules.CurrentStore;
            return ScaleHistoryService.FindList((o) => o.CompanyId == CommonService.CompanyId && o.Store == store).OrderByDescending(o => o.CreateDt).Select(o => o.BatchSN).Distinct().ToList();
        }
    }
}
