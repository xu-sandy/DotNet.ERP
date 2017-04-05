using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using System.Collections.Specialized;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Sys.Entity;
using System.Data;

namespace Pharos.Logic.BLL
{
    public class BreakageGoodsBLL : BaseService<BreakageGoods>
    {
        /// <summary>
        /// 获取报损单信息
        /// </summary>
        /// <param name="nvl">传递条件</param>
        /// <param name="recordCount">返回总行数</param>
        /// <returns>list</returns>
        public static object FindBreakageGoodsList(NameValueCollection nvl, out int recordCount)
        {
            var queryBreakageGoods = BaseService<BreakageGoods>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryBreakageList = BaseService<BreakageList>.CurrentRepository.QueryEntity;
            var queryWarehouse = BaseService<Warehouse>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryUser = UserInfoService.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var querySupplier = BaseService<Supplier>.CurrentRepository.QueryEntity.Where(o => o.BusinessType == 2 && o.CompanyId == CommonService.CompanyId);

            var groupbyBreakageList = from a in queryBreakageList
                                      group a by a.BreakageGoodsId into g
                                      select new
                                      {
                                          g.Key,
                                          BreakagePrice = g.Sum(a => a.BreakagePrice * a.BreakageNumber),
                                          BreakageNumber = g.Sum(a => a.BreakageNumber)
                                      };

            var query = from x in queryBreakageGoods
                        join y in groupbyBreakageList on x.BreakageGoodsId equals y.Key
                        join s in queryWarehouse on x.StoreId equals s.StoreId into temp1
                        from z in temp1.DefaultIfEmpty()
                        join l in queryUser on x.OperatorUID equals l.UID into temp2
                        from m in temp2.DefaultIfEmpty()

                        select new
                        {
                            x.Id,
                            x.BreakageGoodsId,
                            x.StoreId,
                            StoreTitle = z.Title,
                            y.BreakagePrice,
                            y.BreakageNumber,
                            x.CreateDT,
                            x.OperatorUID,
                            Operator = m.FullName,
                            x.State,
                            x.BreakageType,
                            x.VerifyTime
                        };

            var Store = nvl["Store"];
            var State = nvl["State"];
            var StartDate = nvl["StartDate"];
            var EndDate = nvl["EndDate"];
            var searchField = nvl["searchField"];
            var searchText = nvl["searchText"];
            if (!Store.IsNullOrEmpty())
            {
                query = query.Where(o => o.StoreId == Store);
            }
            if (!State.IsNullOrEmpty())
            {
                var breakageStateValue = short.Parse(State);
                query = query.Where(o => o.State == breakageStateValue);
            }
            if (!StartDate.IsNullOrEmpty())
            {
                var st1 = DateTime.Parse(StartDate);
                query = query.Where(o => o.CreateDT >= st1);
            }
            if (!EndDate.IsNullOrEmpty())
            {
                var st2 = DateTime.Parse(EndDate).AddDays(1);
                query = query.Where(o => o.CreateDT < st2);
            }
            if (!searchText.IsNullOrEmpty())
            {
                if (searchField == "barcode")
                    query = query.Where(o => queryBreakageList.Any(i => i.BreakageGoodsId == o.BreakageGoodsId && i.Barcode.Contains(searchText)));
                else if (searchField == "BreakageGoodsId")
                    query = query.Where(o => o.BreakageGoodsId.Contains(searchText));
            }
            recordCount = query.Count();
            return query.ToPageList(nvl).Select(o => new
            {
                o.Id,
                o.StoreId,
                o.StoreTitle,
                BreakagePrice = o.BreakagePrice.ToString("f2"),
                o.BreakageNumber,
                o.CreateDT,
                o.BreakageGoodsId,
                CreateDTStr = o.CreateDT.ToString("yyyy-MM-dd"),
                VerifyDTStr = o.VerifyTime.HasValue ? o.VerifyTime.Value.ToString("yyyy-MM-dd") : "",
                o.OperatorUID,
                o.Operator,
                o.State,
                StateTitle = Enum.GetName(typeof(BreakageState), o.State),
                o.BreakageType,
                BreakageTypeTitle = Enum.GetName(typeof(BreakageType),o.BreakageType)
            });
        }

        /// <summary>
        /// 级联删除报损单记录
        /// </summary>
        /// <param name="Ids">BreakageGoods主键ID</param>
        /// <returns>执行结果</returns>
        public static OpResult DeleteBreakageGoodById(string[] ids)
        {
            var op = new OpResult();
            try
            {
                var queryBreakageGoods = BaseService<BreakageGoods>.CurrentRepository;
                var queryBreakageList = BaseService<BreakageList>.CurrentRepository;
                var breakageGoods = queryBreakageGoods.FindList(o => ids.Contains(o.Id.ToString())).ToList();
                if (breakageGoods.Any(o => o.State == 1))
                {
                    var goods = breakageGoods.Where(o => o.State == 1).FirstOrDefault();
                    op.Message = string.Format("报损单：{0}为已审状态，无法删除", goods.BreakageGoodsId);
                    return op;
                }
                var breakageGoodIds = breakageGoods.Select(o => o.BreakageGoodsId).ToList();
                var breakageGoodLists = new List<BreakageList>();
                foreach (var breakageGoodsId in breakageGoodIds)
                {
                    var breakageGoodList = queryBreakageList.FindList(o => o.BreakageGoodsId == breakageGoodsId);
                    if (breakageGoodList.Count() > 0)
                        breakageGoodLists.AddRange(breakageGoodList);
                }
                queryBreakageList.RemoveRange(breakageGoodLists, false);
                queryBreakageGoods.RemoveRange(breakageGoods, true);

                op.Successed = true;
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError("删除失败", ex);
            }
            return op;
        }

        /// <summary>
        /// 保存报损清单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static OpResult SaveOrUpdate(BreakageGoods obj)
        {
            var re = new OpResult();
            try
            {
                var details = new List<BreakageList>();
                var breakageGoodsId = Logic.CommonRules.BreakageSN;
                if (!string.IsNullOrWhiteSpace(obj.Rows))
                {
                    var adds = obj.Rows.ToObject<List<BreakageList>>();
                    if (adds.Any())
                        details.AddRange(adds.Where(o => !string.IsNullOrWhiteSpace(o.Barcode)));
                }
                //判断出库的商品是否有库存记录
                //var stockNums = CommodityService.GetStockNumsByStoreId(obj.StoreId);
                //foreach (var d in details)
                //{
                //    var stockNum = stockNums.FirstOrDefault(o => o.Key == d.Barcode);
                //    if (stockNum.Value == 0 && string.IsNullOrEmpty(stockNum.Key))
                //    {
                //        re.Message = string.Format("条码：{0} 不存在库存记录，无法报损", d.Barcode);
                //        return re;
                //    }
                //}
                if (obj.Id == 0)
                {//新增
                    obj.BreakageGoodsId = breakageGoodsId;
                    obj.State = 0;
                    obj.CompanyId = CommonService.CompanyId;
                    foreach (var d in details)
                    {
                        d.BreakageGoodsId = breakageGoodsId;
                    }
                    BaseService<BreakageList>.CurrentRepository.AddRange(details, false);
                    re = Add(obj, true);
                }
                else
                {//编辑
                    var breakage = FindById(obj.Id);
                    breakage.StoreId = obj.StoreId;
                    breakage.OperatorUID = obj.OperatorUID;
                    breakage.CreateDT = obj.CreateDT;
                    breakage.Memo = obj.Memo;
                    foreach (var d in details)
                    {
                        d.BreakageGoodsId = breakage.BreakageGoodsId;
                    }
                    var oldBreakageList = BaseService<BreakageList>.CurrentRepository.FindList(o => o.BreakageGoodsId == breakage.BreakageGoodsId).ToList();
                    BaseService<BreakageList>.CurrentRepository.RemoveRange(oldBreakageList, false);
                    BaseService<BreakageList>.CurrentRepository.AddRange(details, false);
                    re = Update(breakage, true);


                }
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                Log.WriteError(ex);
            }
            return re;
        }
        /// <summary>
        /// 设置报损单为已审状态
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public static OpResult SetBreakageGoodsStateToChecked(string Ids)
        {
            var re = new OpResult();
            try
            {
                var ids = Ids.Split(',').Select(o => o.ToString());
                var goods = BaseService<BreakageGoods>.FindList(o => ids.Contains(o.Id.ToString()));
                var breakageGoodsIds = goods.Select(o => o.BreakageGoodsId).ToList();
                var list = BaseService<BreakageList>.FindList(o => breakageGoodsIds.Contains(o.BreakageGoodsId));
                if (goods.Any(o => o.State == 1))
                {
                    var goodsChecked = goods.Where(o => o.State == 1).FirstOrDefault();
                    re.Message = string.Format("报损单：{0} 已为已审状态", goodsChecked.BreakageGoodsId);
                    return re;
                }
                var invents = new List<InventoryRecord>();
                var query = from a in goods
                            join b in list on a.BreakageGoodsId equals b.BreakageGoodsId
                            from c in ProductService.CurrentRepository.QueryEntity
                            where b.Barcode == c.Barcode || ("," + c.Barcodes + ",").Contains("," + b.Barcode + ",")
                            group new { a, b, c } by new { a.StoreId, c.Barcode } into g
                            select new
                            {
                                g.Key.StoreId,
                                g.Key.Barcode,
                                ReturnNum = g.Sum(o => o.b.BreakageNumber)
                            };
                var datas = query.ToList();
                datas.Each(o =>
                {
                    invents.Add(new InventoryRecord() { Barcode = o.Barcode, StoreId = o.StoreId, Number = o.ReturnNum, Source = 16 });
                });
                goods.ForEach(o => { o.State = 1; o.VerifyTime = DateTime.Now; });
                list.ForEach(o => { o.State = 1; });
                re= BaseService<BreakageGoods>.Update(goods);
                if(re.Successed)
                    InventoryRecordService.SaveLog(invents);
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
            }
            return re;
        }

        /// <summary>
        /// 获得报损单单明细
        /// </summary>
        /// <param name="inboundGoodsId"></param>
        /// <returns></returns>
        public static object GetBreakageListByBreakageGoodId(string breakageGoodsId, out int recordCount)
        {
            var queryBreakageListList = BaseService<BreakageList>.CurrentRepository.QueryEntity;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o=>o.CompanyId==CommonService.CompanyId);
            var queryData = BaseService<SysDataDictionary>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var query = from x in queryBreakageListList
                        from y in queryProduct
                        where x.BreakageGoodsId == breakageGoodsId && (y.Barcode == x.Barcode || y.Barcodes.Contains(x.Barcode)) 
                        select new
                        {
                            y.ProductCode,
                            y.Title,
                            x.Barcode,
                            y.SubUnitId,
                            x.BreakagePrice,
                            Price = y.SysPrice,
                            x.BreakageNumber,
                            Subtotal = x.BreakagePrice * x.BreakageNumber,
                            x.Id,
                            SubUnit=y.SubUnit
                        };

            recordCount = query.Count();
            var sql = query.ToString();
            return query.ToPageList();
        }

        #region 导入
        public static OpResult BreakageImport(ImportSet obj, System.Web.HttpFileCollectionBase httpFiles, string fieldName, string columnName)
        {
            var op = new OpResult();
            var errLs = new List<string>();
            int count = 0;
            var list = new List<OutboundList>();
            try
            {
                Dictionary<string, char> fieldCols = null;
                DataTable dt = null;
                op = ImportSetService.ImportSet(obj, httpFiles, fieldName, columnName, ref fieldCols, ref dt);
                if (!op.Successed) return op;
                var storeId = System.Web.HttpContext.Current.Request["StoreId"];
                var products = new List<VwProduct>();
                if (!storeId.IsNullOrEmpty())
                {
                    var ware = WarehouseService.Find(o => o.StoreId == storeId && o.CompanyId == CommonService.CompanyId);
                    if (ware != null)
                    {
                        var categorySNs = ware.CategorySN.Split(',').Select(o => int.Parse(o)).ToList();
                        var childs = ProductCategoryService.GetChildSNs(categorySNs);
                        var pros = BaseService<VwProduct>.FindList(o => o.CompanyId == CommonService.CompanyId && childs.Contains(o.CategorySN));
                        products.AddRange(pros);
                    }
                }
                var barcodeIdx = fieldCols.GetValue("Barcode").ToType<int>() - 65;
                var breakPriceIdx = fieldCols.GetValue("BreakagePrice").ToType<int>() - 65;
                var numberIdx = fieldCols.GetValue("BreakageNumber").ToType<int>() - 65;
                count = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var pos = i + obj.MinRow;
                    var dr = dt.Rows[i];
                    var barcode = dr.GetValue(barcodeIdx).ToString().Trim();
                    if (barcode.IsNullOrEmpty())
                    {
                        errLs.Add("行号[" + pos + "]条码为空!");
                        continue;
                    }
                    var pro = products.FirstOrDefault(o => o.Barcode == barcode || ("," + o.Barcodes + ",").Contains("," + barcode + ","));
                    if (pro == null)
                    {
                        errLs.Add("行号[" + pos + "]该门店下无此条码!");
                        continue;
                    }
                    var outPrice = dr.GetValue(breakPriceIdx).ToType<decimal?>();
                    if (!outPrice.HasValue)
                    {
                        errLs.Add("行号[" + pos + "]该条码单价为空!");
                        continue;
                    }
                    var number = dr.GetValue(numberIdx).ToType<decimal?>();
                    if (!number.HasValue)
                    {
                        errLs.Add("行号[" + pos + "]该条码数量为空!");
                        continue;
                    }

                    list.Add(new OutboundList()
                    {
                        Barcode = barcode,
                        ProductTitle = pro.Title,
                        BuyPrice = pro.BuyPrice,
                        OutboundNumber = number.Value,
                        Unit = pro.SubUnit,
                        SysPrice = pro.SysPrice,
                        OutPrice = outPrice.Value,
                    });
                }
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                op.Successed = false;
                Log.WriteError(ex);
                errLs.Add("导入出现异常!");
            }
            return CommonService.GenerateImportHtml(errLs, count, data: list, isSuccess: false);
        }
        #endregion
    }
}
