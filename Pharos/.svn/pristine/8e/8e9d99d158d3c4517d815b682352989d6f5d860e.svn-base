using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public abstract class BaseEntity
    {
        public virtual int CompanyId { get; set; }
    }

    public abstract class SyncEntity : BaseEntity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        [Pharos.Utility.Exclude]
        public byte[] SyncItemVersion { get; set; }
        Guid _SyncItemId = Guid.NewGuid();
        [Pharos.Utility.Exclude]
        public Guid SyncItemId { get { return _SyncItemId; } set { _SyncItemId = value; } }
    }
}
