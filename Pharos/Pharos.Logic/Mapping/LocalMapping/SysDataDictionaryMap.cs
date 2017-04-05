using Pharos.Logic.LocalEntity;
using System.Data.Entity.ModelConfiguration;

namespace Pharos.Logic.Mapping.LocalMapping
{
    public class SysDataDictionaryMap : EntityTypeConfiguration<SysDataDictionary>
    {
        public SysDataDictionaryMap()
        {
            this.ToTable("SysDataDictionary");
        }
    }
}
