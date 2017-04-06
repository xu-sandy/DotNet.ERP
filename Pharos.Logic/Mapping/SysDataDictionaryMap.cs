using Pharos.Logic.Entity;
using Pharos.Sys.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Pharos.Logic.Mapping
{
    public class SysDataDictionaryMap : EntityTypeConfiguration<SysDataDictionary>
    {
        public SysDataDictionaryMap()
        {
            this.ToTable("SysDataDictionary");
            this.Property(o => o.SyncItemVersion).IsRowVersion();
        }
    }
}
