﻿using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class VwOrderMap : EntityTypeConfiguration<VwOrder>
    {
        public VwOrderMap()
        {
            this.ToTable("Vw_Order");
        }
    }
}
