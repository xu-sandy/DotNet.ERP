using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class ReceiptsMap : EntityTypeConfiguration<Receipts>
    {
        public ReceiptsMap()
        {
            HasMany(u => u.Attachments).WithOptional().HasForeignKey(r => r.ItemId).WillCascadeOnDelete(true);
        }
    }
}
