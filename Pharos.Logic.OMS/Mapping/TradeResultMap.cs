using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Mapping
{
    public class TradeResultMap : EntityTypeConfiguration<TradeResult>
    {
        public TradeResultMap()
        {
            ToTable("TradeResults");
        }
    }
}
