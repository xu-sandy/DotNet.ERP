using Pharos.Sys.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Pharos.Logic.Mapping
{
    public class SysDataDictMap : EntityTypeConfiguration<SysDataDictionary>
    {
        public SysDataDictMap()
        {
            this.ToTable("SysDataDictionary");
        }
    }
}
