using Pharos.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Barcodes
{
    public abstract class OrderMiddleware
    {
        public virtual bool VerfyProduct(IIdentification newBarcode, IIdentification oldBarcode)
        {
            if (newBarcode.RecordId == oldBarcode.RecordId)
            {
                return true;
            }
            if (string.IsNullOrEmpty(newBarcode.RecordId) && SameBarcode(oldBarcode, newBarcode.MainBarcode) && !oldBarcode.HasEditPrice && oldBarcode.ProductType != ProductType.Weigh)
            {
                return true;
            }
            return false;
        }

        public bool SameBarcode(IIdentification oldBarcode, string barcode)
        {
            return (oldBarcode.MainBarcode == barcode || (oldBarcode.MultiCode != null && oldBarcode.MultiCode.Contains(barcode)));
        }
    }
}
