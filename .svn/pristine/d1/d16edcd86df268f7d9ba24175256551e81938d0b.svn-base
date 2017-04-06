using Newtonsoft.Json;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.LocalEntity
{
    public class PosChecking : BaseEntity, ICanUploadEntity
    {
        public Int64 Id { get; set; }

        public string Project { get; set; }

        public decimal Total { get; set; }
        [JsonIgnore]

        public bool IsUpload { get; set; }

        public string MachineSN { get; set; }

        public string CreateUID { get; set; }
        public string StoreId { get; set; }
        public int OrderId { get; set; }
        public DateTime CreateDT { get; set; }
    }
}
