using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Sys.Entity
{
    public class CRM_DeviceInfo
    {
        public int StoreId { get; set; }
        public int Id { get; set; }
        public short Type { get; set; }
        public string DeviceSN { get; set; }
        public int MachineSN { get; set; }
        public string Store { get; set; }
        public DateTime CreateDT { get; set; }
        public short State { get; set; }
        public string Auditor { get; set; }
        public string Memo { get; set; }
    }
}
