using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class FreeGiftPurchaseList : SyncDataObject
    {
        public string GiftId { get; set; }
        public short GiftType { get; set; }
        public short GiftNumber { get; set; }
        public string BarcodeOrCategorySN { get; set; }
        public short? CategoryGrade { get; set; }
    }
}
