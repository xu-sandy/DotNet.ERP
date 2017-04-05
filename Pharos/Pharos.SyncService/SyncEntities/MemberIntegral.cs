using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class MemberIntegral : SyncDataObject
    {
        public string PaySN { get; set; }
        public string MemberId { get; set; }
        public decimal ActualPrice { get; set; }
        public int Integral { get; set; }
        public DateTime CreateDT { get; set; }
        public int CompanyId { get; set; }

    }
}
