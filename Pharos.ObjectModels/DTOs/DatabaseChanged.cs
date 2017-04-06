using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.ObjectModels.DTOs
{
    public class DatabaseChanged
    {
        public int CompanyId { get; set; }
        public string StoreId { get; set; }
        public string Target { get; set; }
    }
}
