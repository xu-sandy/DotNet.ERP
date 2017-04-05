﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable()]
    public class Notice : SyncDataObject
    {
        public int CompanyId { get; set; }
        public string Theme { get; set; }
        public string NoticeContent { get; set; }
        public short Type { get; set; }
        public string CreateUID { get; set; }
        public short State { get; set; }
        public DateTime CreateDT { get; set; }
        public string StoreId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime BeginDate { get; set; }
        public string Url { get; set; }
    }
}
