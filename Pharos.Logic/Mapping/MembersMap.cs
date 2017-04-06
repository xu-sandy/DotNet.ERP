using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class MembersMap : EntityTypeConfiguration<Members>
    {
        public MembersMap()
        {
            this.Property(o => o.SyncItemVersion).IsRowVersion();

        }
    }
}
