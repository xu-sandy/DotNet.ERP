﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.ApiParams
{
    public class SetOldCustomerOrderSnRequset:BaseApiParams
    {
        public string OldCustomerOrderSN { get; set; }
        public int Mode { get; set; }


    }
}
