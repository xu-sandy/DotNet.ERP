using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class SaleOrdersMap : EntityTypeConfiguration<SaleOrders>
    {
        public SaleOrdersMap()
        {
            //this.HasKey(o => o.PaySN);
            //HasMany(o => o.ConsumptionPayments).WithOptional().HasForeignKey(o => o.PaySN);
            //HasMany(o => o.WipeZeros).WithOptional().HasForeignKey(o => o.PaySN);
            //HasMany(o => o.SaleDetails).WithOptional().HasForeignKey(o => o.PaySN);
            this.Property(o => o.SyncItemVersion).IsRowVersion();

        }
    }
}
