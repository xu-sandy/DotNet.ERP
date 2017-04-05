using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity.Views
{
    public class MakeingMembershipCardViewModel
    {
        public int Id { get; set; }
        public string CardName { get; set; }
        public int NumberStart { get; set; }
        public int NumberEnd { get; set; }
        public int State { get; set; }
        public DateTime ExpiryStart { get; set; }
        public DateTime? ExpiryEnd { get; set; }
        public string SecurityCode { get; set; }
        public DateTime CreateDT { get; set; }
        public string CreateUID { get; set; }
        public short CardType { get; set; }
        public string AllRechange { get; set; }
        public decimal MinRecharge { get; set; }
        public string BatchSN { get; set; }
        public int MakeNumber { get; set; }
        public int LeadCount { get; set; }
    }
}
