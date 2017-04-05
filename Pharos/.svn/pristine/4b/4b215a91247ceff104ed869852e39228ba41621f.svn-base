using Newtonsoft.Json;
using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class SaleOrdersDataSyncService : BaseDataSyncService<SaleOrders, SaleOrdersForLocal>
    {
        public override IEnumerable<SaleOrders> Download(string storeId, string entityType)
        {
            SaleOrdersDataSyncService.IsForcedExpired = true;
            var expirationDate = DateTime.Now.Date.AddYears(-1);
            var result = CurrentRepository.Entities.Where(o => o.StoreId == storeId && o.CreateDT > expirationDate).ToList();
            return result;
        }

        public override bool UpLoad(IEnumerable<SaleOrders> datas, string storeId)
        {
            try
            {
                var serverRepository = CurrentRepository;

                if (datas == null)
                {
                    return false;
                }
                var paySNs = CurrentRepository.Entities.ToDictionary(o => o.PaySN, o => o.State).ToList();
                //整单退出记录
                var removeDatas = datas.Where(o => o.State == 1 && paySNs.Exists(p => p.Key == o.PaySN && p.Value != 1)).ToList();
                foreach (var item in removeDatas)
                {
                    var order = SaleOrdersDataSyncService.CurrentRepository.Find(o => o.PaySN == item.PaySN);
                    order.ReturnId = item.ReturnId;
                    order.State = item.State;
                    SaleOrdersDataSyncService.Update(order);
                    var ReturnsDetails = JsonConvert.DeserializeObject<IEnumerable<SalesReturnsDetailed>>(JsonConvert.SerializeObject(UpdateFormDatas.Datas["Pharos.Logic.LocalEntity.SalesReturnsDetailed"]));
                  var details  = ReturnsDetails.Where(o => o.ReturnId == item.ReturnId);
                    //var details = SaleDetailDataSyncService.CurrentRepository.Entities.Where(o => o.PaySN == item.PaySN).ToList();
                    foreach (var i in details)
                    {

                        if (BundlingService.IsExist(o => o.NewBarcode == i.Barcode))
                        {
                            var query = (from a in BundlingService.CurrentRepository.Entities
                                         from b in BundlingListService.CurrentRepository.Entities
                                         where a.CommodityId == b.CommodityId && a.NewBarcode == i.Barcode
                                         select b);
                            var items = query.ToList();
                            foreach (var p in items)
                            {
                                var info = ProductRecordDataSyncService.CurrentRepository.Entities.FirstOrDefault(o => o.Barcode == p.Barcode);
                                UpdateProduct(p.Barcode, i.Number * p.Number, item.PaySN, i.Barcode, storeId, order.CreateUID);
                            }
                        }
                        else
                        {
                            UpdateProduct(i.Barcode, i.Number, item.PaySN, i.Barcode, storeId, order.CreateUID);
                        }
                    }

                }





                //删除重复数据
                datas = datas.Where(o => !paySNs.Exists(p => p.Key == o.PaySN));

                var tempDatas = datas.Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<SaleOrders>(o));
                serverRepository.AddRange(tempDatas.ToList());




                return true;
            }
            catch
            {
                return false;
            }
        }

        private void UpdateProduct(string barcode, decimal purchaseNumber, string paySn, string saleBarcode, string storeId, string uid)
        {
            var product = ProductRecordDataSyncService.CurrentRepository.Entities.FirstOrDefault(o => o.Barcode == barcode || o.ProductCode == barcode);

            if (product.Nature != 1)
            {
                var last = BaseService<Inventory>.CurrentRepository.Entities.FirstOrDefault(o => o.Barcode == product.Barcode && o.StoreId == storeId);
                last.StockNumber += purchaseNumber;
                BaseService<Inventory>.Update(last);
                BaseService<SaleInventoryHistory>.CurrentRepository.Add(new SaleInventoryHistory()
                {
                    InventoryId = last.Id,
                    CreateDt = DateTime.Now,
                    Mode = 4,
                    Number = purchaseNumber,
                    SaleBarcode = saleBarcode,
                    StorageBarcode = last.Barcode,
                    PaySn = paySn,
                    CreateUid = uid,
                    StoreId = storeId
                });
            }
            else if (product.Nature == 1)
            {
                var barcodes = BaseService<ProductGroup>.CurrentRepository.Entities.Where(o => o.Barcode == product.Barcode).ToList();
                foreach (var p in barcodes)
                {
                    var last = BaseService<Inventory>.CurrentRepository.Entities.FirstOrDefault(o => o.Barcode == p.GroupBarcode && o.StoreId == storeId);
                    last.StockNumber += (purchaseNumber * p.Number);
                    BaseService<Inventory>.Update(last);
                    BaseService<SaleInventoryHistory>.CurrentRepository.Add(new SaleInventoryHistory()
                    {
                        InventoryId = last.Id,
                        CreateDt = DateTime.Now,
                        Mode = 4,
                        Number = (purchaseNumber * p.Number),
                        SaleBarcode = saleBarcode,
                        StorageBarcode = last.Barcode,
                        PaySn = paySn,
                        CreateUid = uid,
                        StoreId = storeId
                    });
                }
            }
        }
    }
}

