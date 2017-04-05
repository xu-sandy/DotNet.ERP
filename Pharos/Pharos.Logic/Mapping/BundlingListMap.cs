using Pharos.Logic.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Pharos.Logic.Mapping
{
    public class BundlingListMap : EntityTypeConfiguration<BundlingList>
    {
        public BundlingListMap()
        {

            this.Property(o => o.SyncItemVersion).IsRowVersion();

        }
    }
}
