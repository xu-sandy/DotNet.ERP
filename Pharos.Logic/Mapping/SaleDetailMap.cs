using Pharos.Logic.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Pharos.Logic.Mapping
{
    public class SaleDetailMap : EntityTypeConfiguration<SaleDetail>
    {
        public SaleDetailMap()
        {
            this.Property(o => o.SyncItemVersion).IsRowVersion();

        }
    }
}
