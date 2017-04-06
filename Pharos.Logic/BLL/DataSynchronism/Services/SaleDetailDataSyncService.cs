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
    public class SaleDetailDataSyncService : BaseDataSyncService<SaleDetail, SaleDetailForLocal>
    {

        public override IEnumerable<SaleDetail> Download(string storeId, string entityType)
        {
            SaleDetailDataSyncService.IsForcedExpired = true;

            var expirationDate = DateTime.Now.Date.AddYears(-1);

            var query = (from a in CurrentRepository.Entities
                         from b in SaleOrdersDataSyncService.CurrentRepository.Entities
                         where a.PaySN == b.PaySN && b.StoreId == storeId && b.CreateDT > expirationDate
                         select a).ToList();
            return query;
        }


        public override bool UpLoad(IEnumerable<SaleDetail> datas, string storeId)
        {
            //删除重复数据
            {
                if (datas == null)
                {
                    return false;
                }
                var paySNs = CurrentRepository.Entities.Select(o => o.PaySN).Distinct().ToList();
                datas = datas.Where(o => !paySNs.Exists(p => p == o.PaySN));

            }
            //更新库存
            {
                foreach (var item in datas)
                {
                    var orders = JsonConvert.DeserializeObject<IEnumerable<SaleOrders>>(JsonConvert.SerializeObject(UpdateFormDatas.Datas["Pharos.Logic.LocalEntity.SaleOrders"]));
                    var order = orders.FirstOrDefault(o => o.PaySN == item.PaySN);
                    if (order == null)
                    {
                        return false;
                    }
                    //更新捆绑商品库存
                    if (BundlingService.IsExist(o => o.NewBarcode == item.Barcode))
                    {
                        var query = (from a in BundlingService.CurrentRepository.Entities
                                     from b in BundlingListService.CurrentRepository.Entities
                                     where a.CommodityId == b.CommodityId && a.NewBarcode == item.Barcode
                                     select b);
                        var items = query.ToList();
                        foreach (var i in items)
                        {
                            if (!UpdateProduct(item.PurchaseNumber, i.Barcode, storeId, item.PaySN, item.Barcode, order.CreateUID))
                            {
                                return false;
                            }
                        }
                    }
                    else//更新产品档案库存
                    {
                        if (!UpdateProduct(item.PurchaseNumber, item.Barcode, storeId, item.PaySN, item.Barcode, order.CreateUID))
                        {
                            return false;
                        }
                    }
                }
            }

            //更新销售清单
            {
                var serverRepository = CurrentRepository;
                var tempDatas = datas.Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<SaleDetail>(o));
                serverRepository.AddRange(tempDatas.ToList());
            }
            return true;
        }

        private bool UpdateProduct(decimal purchaseNumber, string barcode, string storeId, string paySn, string saleBarcode, string uid)
        {
            var info = ProductService.Find(o => o.Barcode == barcode || o.ProductCode == barcode);
            var result = true;

            switch (info.Nature)
            {
                case 2://拆分
                    {
                        var childProductInventory = BaseService<Inventory>.CurrentRepository.QueryEntity.FirstOrDefault(o => o.StockNumber > 0 && o.StoreId == storeId && o.Barcode == info.Barcode);

                        if (childProductInventory != null)
                        {
                            var stockNum = childProductInventory.StockNumber;
                            if (stockNum >= purchaseNumber)
                            {
                                result = UpdateSingleProduct(purchaseNumber, info.Barcode, storeId, paySn, saleBarcode, 1, uid);
                            }
                            else if (stockNum < purchaseNumber)
                            {
                                result = UpdateSingleProduct(stockNum, info.Barcode, storeId, paySn, saleBarcode, 1, uid);
                                if (!result)
                                {
                                    break;
                                }
                                result = SplitSingleProduct(info, purchaseNumber - stockNum, info.Barcode, storeId, paySn, saleBarcode, uid);
                            }
                        }
                        else
                        {
                            result = SplitSingleProduct(info, purchaseNumber, info.Barcode, storeId, paySn, saleBarcode, uid);
                        }
                        break;

                    }
                case 1://组合
                    {
                        var barcodes = BaseService<ProductGroup>.CurrentRepository.Entities.Where(o => o.Barcode == info.Barcode).ToList();
                        foreach (var item in barcodes)
                        {
                            if (!UpdateSingleProduct(purchaseNumber * item.Number, item.GroupBarcode, storeId, paySn, saleBarcode, 3, uid))
                            {
                                result = false;
                                break;
                            }
                        }
                        break;
                    }
                case 0://单品
                    {
                        if (!UpdateSingleProduct(purchaseNumber, info.Barcode, storeId, paySn, saleBarcode, 1, uid))
                        {
                            result = false;
                        }
                        break;
                    }
            }
            return result;
        }
        public bool SplitSingleProduct(ProductRecord product, decimal purchaseNumber, string barcode, string storeId, string paySn, string saleBarcode, string uid)
        {

            var oldProductNum = Math.Ceiling(purchaseNumber / (product.SaleNum ?? 1m));
            var result = UpdateSingleProduct(oldProductNum, product.OldBarcode, storeId, paySn, saleBarcode, 2, uid);
            if (!result)
            {
                return false;
            }

            var InventoryDict = ProductService.FindList(o => o.OldBarcode == product.OldBarcode).ToDictionary(o => o.Barcode, o => o.SaleNum ?? 1 * oldProductNum);
            InventoryDict[barcode] -= purchaseNumber;

            foreach (var item in InventoryDict)
            {
                var childInventory = BaseService<Inventory>.CurrentRepository.Find(o => o.Barcode == item.Key && o.StoreId == storeId);
                if (childInventory != null)
                {
                    childInventory.StockNumber = childInventory.StockNumber + item.Value;
                    BaseService<Inventory>.CurrentRepository.Update(childInventory);
                }
                else
                {
                    childInventory = new Inventory()
                    {
                        StoreId = storeId,
                        StockNumber = item.Value,
                        Barcode = item.Key
                    };
                    BaseService<Inventory>.CurrentRepository.Add(childInventory);
                }
                if (item.Key == barcode)
                {
                    BaseService<SaleInventoryHistory>.CurrentRepository.Add(new SaleInventoryHistory()
                    {
                        InventoryId = childInventory.Id,
                        CreateDt = DateTime.Now,
                        Mode = 5,
                        Number = (item.Value + purchaseNumber),
                        SaleBarcode = saleBarcode,
                        StorageBarcode = item.Key,
                        PaySn = paySn,
                        CreateUid = uid,
                        StoreId = storeId
                    }, false);
                    BaseService<SaleInventoryHistory>.CurrentRepository.Add(new SaleInventoryHistory()
                    {
                        InventoryId = childInventory.Id,
                        CreateDt = DateTime.Now,
                        Mode = 1,
                        Number = purchaseNumber,
                        SaleBarcode = saleBarcode,
                        StorageBarcode = item.Key,
                        PaySn = paySn,
                        CreateUid = uid,
                        StoreId = storeId
                    }, false);
                }
                else
                {
                    BaseService<SaleInventoryHistory>.CurrentRepository.Add(new SaleInventoryHistory()
                    {
                        InventoryId = childInventory.Id,
                        CreateDt = DateTime.Now,
                        Mode = 5,
                        Number = item.Value,
                        SaleBarcode = saleBarcode,
                        StorageBarcode = item.Key,
                        PaySn = paySn,
                        CreateUid = uid,
                        StoreId = storeId
                    }, false);
                }
            }

            BaseService<Inventory>.CurrentRepository.Update(new Inventory());
            return true;
        }
        private bool UpdateSingleProduct(decimal purchaseNumber, string barcode, string storeId, string paySn, string saleBarcode, int mode, string uid)
        {
            try
            {
                Inventory Inventory = BaseService<Inventory>.CurrentRepository.QueryEntity.FirstOrDefault(o => o.StoreId == storeId && o.Barcode == barcode);
                if (Inventory != null) //存在库存记录
                {
                    Inventory.StockNumber -= purchaseNumber;
                    BaseService<SaleInventoryHistory>.CurrentRepository.Add(new SaleInventoryHistory()
                    {
                        InventoryId = Inventory.Id,
                        CreateDt = DateTime.Now,
                        Mode = mode,
                        Number = purchaseNumber,
                        SaleBarcode = saleBarcode,
                        StorageBarcode = barcode,
                        PaySn = paySn,
                        CreateUid = uid,
                        StoreId = storeId

                    }, false);
                    purchaseNumber = 0;

                }
                else //不存在库存记录
                {
                    var entity = new Inventory()
                    {
                        StoreId = storeId,
                        StockNumber = -purchaseNumber,
                        Barcode = barcode
                    };
                    BaseService<Inventory>.CurrentRepository.Add(entity, false);
                    BaseService<SaleInventoryHistory>.CurrentRepository.Add(new SaleInventoryHistory()
                    {
                        InventoryId = entity.Id,
                        CreateDt = DateTime.Now,
                        Mode = mode,
                        Number = purchaseNumber,
                        SaleBarcode = saleBarcode,
                        StorageBarcode = barcode,
                        PaySn = paySn,
                        CreateUid = uid,
                        StoreId = storeId,

                    }, false);
                    purchaseNumber = 0;
                }
                BaseService<Inventory>.CurrentRepository.Update(new Inventory());//激发更新
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
