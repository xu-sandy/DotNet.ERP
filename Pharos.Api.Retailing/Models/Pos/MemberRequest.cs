using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Pharos.Api.Retailing.Models.Pos
{
    public class MemberRequest
    {
        public MemberSource To { get; set; }

        public string CardNum { get; set; }

        public string Weixin { get; set; }
    }
}