using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL
{
    /// <summary>
    /// 库存操作 业务实现
    /// </summary>
    public class InventoryService : BaseService<Inventory>
    {
        #region 销售库存操作
        /// <summary>
        /// 售后管理 更新库存
        /// </summary>
        /// <param name="barcode">商品条码</param>
        /// <param name="number">库存变化数量</param>
        /// <param name="storeId">库存变化门店Id</param>
        /// <param name="paySN">对应的销售单流水号</param>
        /// <param name="mode">库存变化来源操作的模式（1、销售量消减；2、父商品拆分；3、组合商品消减；4、整单退出库存回撤，5、拆分子商品入库,6、退货库存回撤，7、修改订单库存回撤,8、修改订单新增商品消减）</param>
        /// <param name="addOrSubtract">添入库存（true）或是扣除库存（false）</param>
        /// <param name="isSave">是否保存到数据库。默认：false</param>
        public static void UpdateStock(string barcode, decimal number, string storeId, string paySN, int mode, bool addOrSubtract, bool isSave = false)
        {
            if (BundlingService.IsExist(o => o.NewBarcode == barcode))
            {//更新捆绑商品库存
                var bundingList = (from a in BundlingService.CurrentRepository.Entities
                                   from b in BundlingListService.CurrentRepository.Entities
                                   where a.CommodityId == b.CommodityId && a.NewBarcode == barcode && a.CompanyId==CommonService.CompanyId
                                   select b).ToList();
                if (addOrSubtract)
                {
                    foreach (var bundingItem in bundingList)
                    {
                        AddProductStock(barcode, number * bundingItem.Number, paySN, barcode, storeId, mode);
                    }
                }
                else
                {
                    foreach (var bundingItem in bundingList)
                    {
                        SubtractProductStock(number, bundingItem.Barcode, storeId, paySN, barcode, mode);
                    }
                }
            }
            else
            {//更新产品档案库存
                if (addOrSubtract)
                {
                    AddProductStock(barcode, number, paySN, barcode, storeId, mode);
                }
                else
                {
                    SubtractProductStock(number, barcode, storeId, paySN, barcode, mode);
                }
            }
            if (isSave)
                BaseService<Inventory>.CurrentRepository.Update(new Inventory());
        }
        /// <summary>
        /// 扣除产品库存
        /// </summary>
        /// <param name="purchaseNumber">库存变动数量</param>
        /// <param name="barcode">产品条码，实际库存变动的产品</param>
        /// <param name="storeId">门店Id</param>
        /// <param name="paySn">订单流水号</param>
        /// <param name="saleBarcode">销售条码</param>
        /// <param name="mode">库存变化来源操作的模式（1、销售量消减；2、父商品拆分；3、组合商品消减；4、整单退出库存回撤，5、拆分子商品入库,6、退货库存回撤，7、修改订单库存回撤,8、修改订单新增商品消减）</param>
        /// <param name="isSave">是否保存到数据库。默认：false</param>
        public static void SubtractProductStock(decimal purchaseNumber, string barcode, string storeId, string paySn, string saleBarcode, int mode, bool isSave = false)
        {
            try
            {
                var info = ProductService.Find(o =>o.CompanyId==CommonService.CompanyId && o.Barcode == barcode || o.ProductCode == barcode);
                switch (info.Nature)
                {
                    case 2://拆分
                        {
                            SubtractSplitChildStock(info, purchaseNumber, storeId, paySn, saleBarcode, mode);
                            break;
                        }
                    case 1://组合
                        {
                            var barcodes = BaseService<ProductGroup>.CurrentRepository.Entities.Where(o => o.Barcode == info.Barcode).ToList();
                            foreach (var item in barcodes)
                            {
                                SubtractSingleProductStock(purchaseNumber * item.Number, item.GroupBarcode, storeId, paySn, saleBarcode, 3);
                            }
                            break;
                        }
                    case 0://单品
                        {
                            SubtractSingleProductStock(purchaseNumber, info.Barcode, storeId, paySn, saleBarcode, mode);
                            break;
                        }
                }
                if (isSave)
                    BaseService<Inventory>.CurrentRepository.Update(new Inventory());
            }
            catch (Exception ex)
            {
                Log.WriteError("添入产品库存操作失败！", ex);
            }
        }
        /// <summary>
        /// 扣除 拆分子产品 库存
        /// </summary>
        /// <param name="childProduct">拆分子产品条码，实际库存变动的产品</param>
        /// <param name="purchaseNumber">库存变动数量</param>
        /// <param name="storeId">门店Id</param>
        /// <param name="paySn">订单流水号</param>
        /// <param name="saleBarcode">销售条码</param>
        /// <param name="mode">库存变化来源操作的模式（1、销售量消减；2、父商品拆分；3、组合商品消减；4、整单退出库存回撤，5、拆分子商品入库,6、退货库存回撤，7、修改订单库存回撤,8、修改订单新增商品消减）</param>
        /// <param name="isSave">是否保存到数据库。默认：false</param>
        private static void SubtractSplitChildStock(ProductRecord childProduct, decimal purchaseNumber, string storeId, string paySn, string saleBarcode, int mode, bool isSave = false)
        {
            try
            {
                var thisChildInventory = BaseService<Inventory>.CurrentRepository.QueryEntity.FirstOrDefault(o =>o.CompanyId==CommonService.CompanyId && o.Barcode == childProduct.Barcode && o.StoreId == storeId);
                if (thisChildInventory != null)
                {   //存在库存
                    if (purchaseNumber <= thisChildInventory.StockNumber)
                    {   //足够扣，视作单品来扣
                        SubtractSingleProductStock(purchaseNumber, thisChildInventory.Barcode, storeId, paySn, saleBarcode, mode);
                    }
                    else
                    {   //不足够扣
                        var subtractParentNum = Math.Ceiling((purchaseNumber - thisChildInventory.StockNumber) / (childProduct.SaleNum ?? 1m));
                        //扣除父产品库存
                        SubtractSplitParentStock(subtractParentNum, childProduct.OldBarcode, storeId, paySn, saleBarcode);
                        //最多递归一次，故此次作为事务一部分而不SaveChanges
                        SubtractSplitChildStock(childProduct, purchaseNumber, storeId, paySn, saleBarcode, mode);
                    }
                }
                else
                {   //不存在库存（不应该有此情况，售卖前应先入了库，但此处考虑后台操作产品未必有库存）
                    var subtractParentNum = Math.Ceiling((purchaseNumber - thisChildInventory.StockNumber) / (childProduct.SaleNum ?? 1m));
                    //扣除父产品库存
                    SubtractSplitParentStock(subtractParentNum, childProduct.OldBarcode, storeId, paySn, saleBarcode);
                    //最多递归一次，故此次作为事务一部分而不SaveChanges
                    SubtractSplitChildStock(childProduct, purchaseNumber, storeId, paySn, saleBarcode, mode);
                }
                if (isSave)
                    BaseService<Inventory>.CurrentRepository.Update(new Inventory());

            }
            catch (Exception ex)
            {
                Log.WriteError("扣除拆分子产品库存操作失败！", ex);
            }
        }
        /// <summary>
        /// 扣除 拆分父产品 库存，并添入相应拆分子产品库存
        /// </summary>
        /// <param name="purchaseNumber">库存变动数量</param>
        /// <param name="barcode">拆分父产品条码，实际库存变动的产品</param>
        /// <param name="storeId">门店Id</param>
        /// <param name="paySn">订单流水号</param>
        /// <param name="saleBarcode">销售条码</param>
        private static void SubtractSplitParentStock(decimal purchaseNumber, string barcode, string storeId, string paySn, string saleBarcode)
        {
            //扣除父产品库存
            SubtractSingleProductStock(purchaseNumber, barcode, storeId, paySn, saleBarcode, 2);
            var childrenAddInventoryDic = ProductService.FindList(o =>o.CompanyId==CommonService.CompanyId && o.OldBarcode == barcode).ToDictionary(o => o.Barcode, o => (o.SaleNum ?? 1) * purchaseNumber);
            foreach (var item in childrenAddInventoryDic)
            {
                var childInventory = BaseService<Inventory>.CurrentRepository.QueryEntity.FirstOrDefault(o => o.CompanyId == CommonService.CompanyId && o.Barcode == item.Key && o.StoreId == storeId);
                if (childInventory != null)
                {
                    childInventory.StockNumber += item.Value;
                    BaseService<Inventory>.CurrentRepository.Update(childInventory, false);
                }
                else
                {
                    BaseService<Inventory>.CurrentRepository.Add(new Inventory()
                    {
                        Barcode = item.Key,
                        StockNumber = item.Value,
                        StoreId = storeId,
                        CompanyId=CommonService.CompanyId
                    }, false);
                }
            }
        }

        /// <summary>
        /// 扣除 单品 库存
        /// </summary>
        /// <param name="purchaseNumber">库存变动数量</param>
        /// <param name="barcode">产品条码，实际库存变动的产品</param>
        /// <param name="storeId">门店Id</param>
        /// <param name="paySn">订单流水号</param>
        /// <param name="saleBarcode">销售条码</param>
        /// <param name="mode">库存变化来源操作的模式（1、销售量消减；2、父商品拆分；3、组合商品消减；4、整单退出库存回撤，5、拆分子商品入库,6、退货库存回撤，7、修改订单库存回撤,8、修改订单新增商品消减）</param>
        /// <param name="isSave">是否保存到数据库。默认：false</param>
        private static void SubtractSingleProductStock(decimal purchaseNumber, string barcode, string storeId, string paySn, string saleBarcode, int mode, bool isSave = false)
        {
            try
            {
                Inventory inventory = BaseService<Inventory>.CurrentRepository.QueryEntity.FirstOrDefault(o =>o.CompanyId==CommonService.CompanyId && o.StoreId == storeId && o.Barcode == barcode);//？不应该只取大于零的库存
                if (inventory != null)
                {   //存在库存记录
                    inventory.StockNumber -= purchaseNumber;
                    BaseService<SaleInventoryHistory>.CurrentRepository.Add(new SaleInventoryHistory()
                    {
                        InventoryId = inventory.Id,
                        CreateDt = DateTime.Now,
                        Mode = mode,
                        Number = purchaseNumber,
                        SaleBarcode = saleBarcode,
                        StorageBarcode = barcode,
                        PaySn = paySn,
                        CreateUid = Sys.CurrentUser.UID,
                        StoreId = storeId,
                        CompanyId=CommonService.CompanyId
                    }, false);
                }
                else
                {   //不存在库存记录
                    var entity = new Inventory()
                    {
                        StoreId = storeId,
                        StockNumber = -purchaseNumber,
                        Barcode = barcode,
                        CompanyId = CommonService.CompanyId
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
                        CreateUid = Sys.CurrentUser.UID,
                        StoreId = storeId,
                        CompanyId = entity.CompanyId
                    }, false);
                }
                purchaseNumber = 0;
                if (isSave)
                    BaseService<Inventory>.CurrentRepository.Update(new Inventory());
            }
            catch (Exception ex)
            {
                Log.WriteError("扣除单品库存操作失败！", ex);
            }
        }
        /// <summary>
        /// 添入产品库存
        /// </summary>
        /// <param name="barcode">产品条码，实际库存变动的产品</param>
        /// <param name="purchaseNumber">库存变动数量</param>
        /// <param name="paySn">订单流水号</param>
        /// <param name="saleBarcode">销售条码</param>
        /// <param name="storeId">门店Id</param>
        /// <param name="mode">库存变化来源操作的模式（1、销售量消减；2、父商品拆分；3、组合商品消减；4、整单退出库存回撤，5、拆分子商品入库,6、退货库存回撤，7、修改订单库存回撤,8、修改订单新增商品消减）</param>
        /// <param name="isSave">是否保存到数据库。默认：false</param>
        public static void AddProductStock(string barcode, decimal purchaseNumber, string paySn, string saleBarcode, string storeId, int mode, bool isSave = false)
        {
            try
            {
                var product = ProductService.CurrentRepository.Entities.FirstOrDefault(o =>o.CompanyId==CommonService.CompanyId &&(o.Barcode == barcode || o.ProductCode == barcode));
                if (product.Nature != 1)
                {   //单品、拆分子产品直接添入
                    AddSingleProductStock(product.Barcode, purchaseNumber, paySn, saleBarcode, storeId, mode);
                }
                else if (product.Nature == 1)
                {   //组合产品 入成分子产品的库存，每个子产品视作单品
                    var elementBarcodes = BaseService<ProductGroup>.CurrentRepository.Entities.Where(o => o.Barcode == product.Barcode).ToList();
                    foreach (var elementBarcode in elementBarcodes)
                    {
                        AddSingleProductStock(elementBarcode.GroupBarcode, purchaseNumber * elementBarcode.Number, paySn, saleBarcode, storeId, mode);
                    }
                }
                if (isSave)
                    BaseService<Inventory>.CurrentRepository.Update(new Inventory());
            }
            catch (Exception ex)
            {
                Log.WriteError("添入产品库存操作失败！", ex);
            }
        }
        /// <summary>
        /// 添入单品/拆分子产品库存
        /// </summary>
        /// <param name="barcode">产品条码，实际库存变动的产品</param>
        /// <param name="purchaseNumber">库存变动数量</param>
        /// <param name="paySn">订单流水号</param>
        /// <param name="saleBarcode">销售条码</param>
        /// <param name="storeId">门店Id</param>
        /// <param name="mode">库存变化来源操作的模式（1、销售量消减；2、父商品拆分；3、组合商品消减；4、整单退出库存回撤，5、拆分子商品入库,6、退货库存回撤，7、修改订单库存回撤,8、修改订单新增商品消减）</param>
        /// <param name="isSave">是否保存到数据库。默认：false</param>
        private static void AddSingleProductStock(string barcode, decimal purchaseNumber, string paySn, string saleBarcode, string storeId, int mode, bool isSave = false)
        {
            try
            {
                var inventory = BaseService<Inventory>.CurrentRepository.Entities.FirstOrDefault(o =>o.CompanyId==CommonService.CompanyId &&(o.Barcode == barcode && o.StoreId == storeId));
                if (inventory != null)
                {   //存在库存记录
                    inventory.StockNumber += purchaseNumber;
                    BaseService<SaleInventoryHistory>.CurrentRepository.Add(new SaleInventoryHistory()
                    {
                        InventoryId = inventory.Id,
                        CreateDt = DateTime.Now,
                        Mode = mode,
                        Number = purchaseNumber,
                        SaleBarcode = saleBarcode,
                        StorageBarcode = barcode,
                        PaySn = paySn,
                        CreateUid = Sys.CurrentUser.UID,
                        StoreId = storeId,
                        CompanyId=CommonService.CompanyId
                    }, false);
                }
                else
                {   //不存在库存记录（不是很必要，因为在售卖前应该先入了库存）
                    var entity = new Inventory()
                    {
                        Barcode = barcode,
                        StockNumber = purchaseNumber,
                        StoreId = storeId,
                        CompanyId=CommonService.CompanyId
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
                        CreateUid = Sys.CurrentUser.UID,
                        StoreId = storeId,
                        CompanyId=entity.CompanyId
                    }, false);
                }
                if (isSave)
                    BaseService<Inventory>.CurrentRepository.Update(new Inventory());
            }
            catch (Exception ex)
            {
                Log.WriteError("添入单品/拆分子产品库存操作失败！", ex);
            }
        }
        #endregion
    }
}
