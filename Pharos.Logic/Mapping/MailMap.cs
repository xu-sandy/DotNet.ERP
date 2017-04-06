using Pharos.Logic.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Pharos.Logic.Mapping
{
    public class MailSenderMap : EntityTypeConfiguration<SysMailSender>
    {
        public MailSenderMap()
        {
            HasMany(o => o.Attachments).WithOptional().HasForeignKey(o => o.ItemId).WillCascadeOnDelete(true); ;
        }
    }
    public class MailReceiveMap : EntityTypeConfiguration<SysMailReceive>
    {
        public MailReceiveMap()
        {
            HasMany(o => o.Attachments).WithOptional().HasForeignKey(o => o.ItemId).WillCascadeOnDelete(true); ;
        }
    }
}
