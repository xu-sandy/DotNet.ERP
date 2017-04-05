using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class WeighingSet:BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public WeighType Type { get; set; }
        public string Serie { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
    }
}
