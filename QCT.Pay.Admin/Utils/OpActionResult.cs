using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QCT.Pay.Admin
{
    public class OpActionResult:ContentResult
    {
        public OpActionResult(Pharos.Utility.OpResult op)
        {
            op = op ?? new Pharos.Utility.OpResult();
            Content = op.ToJson();
        }
    }
}