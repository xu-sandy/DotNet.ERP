using System.Data.Entity.ModelConfiguration;

namespace Pharos.Logic.Mapping
{
    public class SysStoreUserInfoMap : EntityTypeConfiguration<Sys.Entity.SysStoreUserInfo>
    {
        public SysStoreUserInfoMap()
        {
            //HasMany(u => u.UserRoles).WithOptional(ur => ur.User).HasForeignKey(r => r.UserID);
            this.Property(o => o.SyncItemVersion).IsRowVersion();
        }
    }
}
