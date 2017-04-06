using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using Pharos.Logic.Entity;

namespace Pharos.Logic.Mapping
{
    public class CommodityReturnsDetailMap:EntityTypeConfiguration<CommodityReturnsDetail>
    {
        public CommodityReturnsDetailMap()
        {
            this.ToTable("CommodityReturnsDetail");
        }
    }
}
