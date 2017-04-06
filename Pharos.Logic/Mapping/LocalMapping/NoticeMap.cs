using Pharos.Logic.LocalEntity;
using System.Data.Entity.ModelConfiguration;

namespace Pharos.Logic.Mapping.LocalMapping
{
    public class NoticeMap : EntityTypeConfiguration<Notice>
    {
        public NoticeMap()
        {
            this.ToTable("Notice");
        }
    }
}
