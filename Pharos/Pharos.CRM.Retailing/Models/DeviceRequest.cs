using Pharos.Logic.ApiData.Pos.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.CRM.Retailing.Models
{
    public class DeviceRequest
    {
        public string StoreId { get; set; }
        public string MachineSn { get; set; }
        public DeviceType Type { get; set; }
        public string DeviceSn { get; set; }

    }
}