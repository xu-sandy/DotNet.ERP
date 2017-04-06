using Pharos.Logic.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Pharos.Logic.Mapping
{
    public class PosIncomePayoutMap : EntityTypeConfiguration<PosIncomePayout>
    {
        public PosIncomePayoutMap()
        {
            this.Property(o => o.SyncItemVersion).IsRowVersion();

        }
    }
}
