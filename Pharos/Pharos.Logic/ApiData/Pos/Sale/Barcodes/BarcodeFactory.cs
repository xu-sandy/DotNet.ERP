using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Logic.ApiData.Pos.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Barcodes
{
    public class BarcodeFactory
    {
        public static IBarcode Factory(string storeId, string machineSn, int companyId, string barcodeString, SaleStatus saleStatus = SaleStatus.Normal, string giftPromotionId = "")
        {
            IBarcode barcodeObj = null;
            if (string.IsNullOrWhiteSpace(barcodeString))
            {
                throw new BarcodeException("条码不能为空！");
            }
            switch (barcodeString.Length)
            {

                case 12:
                    {
                        var codeType = barcodeString.Substring(0, 2);
                        if (codeType == "04")
                        {
                            try
                            {
                                barcodeObj = new BundlingBarcode(storeId, machineSn, companyId, barcodeString, saleStatus, giftPromotionId);
                            }
                            catch (SaleException)
                            {
                                goto LCustomBarcode;
                            }
                        }
                        else
                        {
                            goto LCustomBarcode;
                        }
                    }
                    break;
                case 13:
                    {
                        var codeType = barcodeString.Substring(0, 2);

                        if (codeType == "27")
                        {
                            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, storeId, machineSn, companyId, DataAdapterFactory.DEFUALT);
                            var productInfo = dataAdapter.GetProductInfoByBarcode(barcodeString.Substring(2, 5), true);
                            if (productInfo != null)
                            {
                                barcodeObj = new WeighBarcode(storeId, machineSn, companyId, barcodeString, saleStatus, giftPromotionId);
                            }
                            else
                            {
                                barcodeObj = new StandardBarcode(storeId, machineSn, companyId, barcodeString, saleStatus, giftPromotionId);
                            }
                        }
                        else
                            barcodeObj = new StandardBarcode(storeId, machineSn, companyId, barcodeString, saleStatus, giftPromotionId);
                    }
                    break;
                //case 18:
                //    barcodeObj = new WeighBarcode(storeId, machineSn, companyId, barcodeString, saleStatus, giftId, giftPromotionId);
                //    break;

                default:
                LCustomBarcode:
                    barcodeObj = new CustomBarcode(storeId, machineSn, companyId, barcodeString, saleStatus, giftPromotionId);
                    break;

            }
            switch (barcodeObj.Details.SaleStatus)
            {
                case SaleStatus.ActivityGifts:
                case SaleStatus.POSGift:
                case SaleStatus.ActivityAddMoneyGifts:
                    barcodeObj.EnableMarketing = false;
                    barcodeObj.Details.EnableEditPrice = false;
                    break;

                default:
                    barcodeObj.EnableMarketing = true;
                    break;
            }
            return barcodeObj;
        }

        //public static IBarcode Factory(IBarcode barcode)
        //{
        //    IBarcode barcodeObj = null;

        //    switch (barcode.Count)
        //    {
        //        case 10:
        //            barcodeObj = new CustomBarcode(barcode);
        //            break;
        //        case 12:
        //            barcodeObj = new BundlingBarcode(barcode);
        //            break;
        //        case 13:
        //            barcodeObj = new StandardBarcode(barcode);
        //            break;
        //        case 18:
        //            barcodeObj = new WeighBarcode(barcode);
        //            break;
        //    }
        //    return barcodeObj;
        //}
    }
    public static class BarcodeExtensions
    {
        public static T InitEntity<T>(this T _entity, IBarcode data)
            where T : IBarcode
        {
            if (_entity == null)
                throw new StackOverflowException("初始化对象entity不能为空！");
            if (data == null)
                throw new StackOverflowException("初始化对象data不能为空！");

            var type = typeof(T);
            var dataType = data.GetType();
            var properties = type.GetProperties();
            var dataProperties = dataType.GetProperties().ToList();
            foreach (var item in properties)
            {
                var dataProperty = dataProperties.FirstOrDefault(o => o.Name == item.Name && o.CanRead);
                if (item.CanWrite && dataProperty != null)
                    item.SetValue(_entity, dataProperty.GetValue(data, null), null);
            }
            return _entity;
        }

    }
}
