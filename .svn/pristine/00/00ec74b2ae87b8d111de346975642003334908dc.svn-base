using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class SaleMarketingMap : EntityTypeConfiguration<Pharos.Logic.Entity.SalesRecord>
    {
        public SaleMarketingMap() 
        {
            this.Property(o => o.SyncItemVersion).IsRowVersion();

        }


    }
}
