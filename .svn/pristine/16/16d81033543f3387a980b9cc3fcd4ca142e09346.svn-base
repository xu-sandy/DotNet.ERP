using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Infrastructure.Data.Normalize
{
    public class MemberNo : SwiftNumber
    {
        public MemberNo(int companyId, string storeId, string lastNum)
            : base(string.Format("MemberNo{0}_{1}", companyId, storeId), SwiftNumberMode.Normal)
        {
            CompanyId = companyId;
            StoreId = storeId;
            int old = 0;
            if (!string.IsNullOrEmpty(lastNum) && int.TryParse(lastNum, out old) && old >= GetNumber())
            {
                Reset(old + 1);
            }
        }
        public string StoreId { get; set; }
        public int CompanyId { get; set; }
        public override string ToString()
        {
            return GetNumber().ToString("000000");
        }
    }

    public class MemberNoDto : SwiftNumberDto
    {
        public string StoreId { get; set; }
        public int CompanyId { get; set; }
    }
}
