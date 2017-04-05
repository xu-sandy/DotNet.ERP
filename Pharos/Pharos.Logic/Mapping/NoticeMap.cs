﻿using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class NoticeMap : EntityTypeConfiguration<Notice>
    {
        public NoticeMap()
        {
            this.ToTable("Notice");
            this.Property(o => o.SyncItemVersion).IsRowVersion();
        }
    }
}
