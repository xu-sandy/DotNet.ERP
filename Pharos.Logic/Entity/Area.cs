using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class Area
    {
        public int AreaID { get; set; }
        public int AreaPID { get; set; }
        public string Title { get; set; }
        public byte Type { get; set; }
        public string JianPin { get; set; }
        public string QuanPin { get; set; }
        public string AreaSN { get; set; }
        public string PostCode { get; set; }
        public int OrderNum { get; set; }

        [Pharos.Utility.Exclude]
        public byte[] SyncItemVersion { get; set; }
        Guid _SyncItemId = Guid.NewGuid();
        [Pharos.Utility.Exclude]
        public Guid SyncItemId { get { return _SyncItemId; } set { _SyncItemId = value; } }
    }
}
