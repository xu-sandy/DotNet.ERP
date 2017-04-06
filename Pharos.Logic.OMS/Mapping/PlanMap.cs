using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Mapping
{
    public class PlanMap : EntityTypeConfiguration<Plans>
    {
        public PlanMap()
        {
            //Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasMany(o => o.Attachments).WithOptional().HasForeignKey(o => o.ItemId).WillCascadeOnDelete(true);
            HasMany(o => o.Replys).WithOptional().HasForeignKey(o => o.PlanId);
        }
    }
}
