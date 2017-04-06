using Pharos.Logic.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Pharos.Logic.Mapping
{
    public class BundlingMap : EntityTypeConfiguration<Bundling>
    {
        public BundlingMap()
        {
            this.Property(o => o.SyncItemVersion).IsRowVersion();

        }
    }
}
