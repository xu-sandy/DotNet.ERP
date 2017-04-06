using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Api.Retailing.Models.Pos
{
    public class SaleEnableMarketingRequest : BaseApiParams
    {
        public bool EnableRangeMarketings { get; set; }
    }
}