﻿using Pharos.Logic.ApiData.Pos.DataAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class LoginParams
    {
        public string Account { get; set; }

        public string Password { get; set; }

        public string StoreId { get; set; }

        public string MachineSn { get; set; }

        public int CompanyId { get; set; }
    }
}
