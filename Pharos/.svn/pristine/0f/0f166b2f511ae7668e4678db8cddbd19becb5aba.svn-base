using Pharos.Infrastructure.Data.Normalize;
using Pharos.MessageTransferAgenClient.DomainEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.ObjectModels.Events
{
    public class SyncMemberNoEvent : SwiftNumberDto, IEvent
    {

        public SyncMemberNoEvent()
        {
            ID = Guid.NewGuid();
            TimeStamp = DateTime.Now;
        }

        public System.Guid ID { get; set; }

        public System.DateTime TimeStamp { get; set; }
        public string StoreId { get; set; }
        public int CompanyId { get; set; }
    }
}
