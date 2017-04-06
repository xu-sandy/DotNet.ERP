using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.OMS.Retailing
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