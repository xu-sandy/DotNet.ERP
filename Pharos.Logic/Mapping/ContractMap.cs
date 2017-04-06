using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class ContractMap : EntityTypeConfiguration<Contract>
    {
        public ContractMap()
        {
            HasMany(o => o.ContractBoths).WithRequired(o => o.Contract).HasForeignKey(o => o.ContractId).WillCascadeOnDelete(true);
            HasMany(o => o.Attachments).WithOptional().HasForeignKey(o => o.ItemId).WillCascadeOnDelete(true);
        }
    }
}
