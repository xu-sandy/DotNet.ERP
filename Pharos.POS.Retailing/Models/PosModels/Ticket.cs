using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.PosModels
{
    public class Ticket
    {
        public TicketType TicketType { get; set; }

        public List<object> PayList { get; set; }

    }

    public enum TicketType
    {
        Sale = 0,
        Refund = 1,
        Changed = 2,
        RefundOrder = 3
    }
}
