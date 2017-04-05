using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class DeviceRegInfoMap : EntityTypeConfiguration<DeviceRegInfo>
    {
        public DeviceRegInfoMap() 
        {
            this.Property(o => o.SyncItemVersion).IsRowVersion();

        }
    }
}
