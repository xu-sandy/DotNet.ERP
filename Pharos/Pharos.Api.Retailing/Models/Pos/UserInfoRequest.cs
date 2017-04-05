using Pharos.Logic.ApiData.Pos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Api.Retailing.Models.Pos
{
    public class UserInfoRequest : BaseApiParams
    {
        public StoreOperateAuth storeOperateAuth { get; set; }
    }
}