﻿using System.Collections.Generic;

namespace Pharos.Api.Retailing.Models.Pos
{
    public class InventoryRequest : BaseApiParams
    {
        public IEnumerable<int> CategorySns { get; set; }


        public string Keyword { get; set; }

        public decimal Price { get; set; }
    }
}