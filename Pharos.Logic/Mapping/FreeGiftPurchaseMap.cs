using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class FreeGiftPurchaseMap : EntityTypeConfiguration<FreeGiftPurchase>
    {
        public FreeGiftPurchaseMap()
        {
            //HasMany(o => o.FreeGiftPurchaseDetails).WithOptional().HasForeignKey(o => o.GiftId);
            this.Property(o => o.SyncItemVersion).IsRowVersion();
        }
    }
}
