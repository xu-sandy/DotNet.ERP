﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Api.Retailing.Models.Pos
{
    public class SetOldCustomerOrderSnRequset : BaseApiParams
    {
        public string OldCustomerOrderSN { get; set; }

        public Logic.ApiData.Pos.Sale.AfterSale.AfterSaleMode Mode { get; set; }
    }
}