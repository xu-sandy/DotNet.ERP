﻿using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data;
using System.Linq;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.DAL;

namespace Pharos.Logic.BLL
{
    /// <summary>
    /// 商品促销主表
    /// </summary>
    public class CommodityPromotionService : BaseService<CommodityPromotion>
    {
        static readonly PromotionDAL _promotionDal = new PromotionDAL();
        #region 单品销售
        /// <summary>
        /// 单品折扣列表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="recordCount"></param>
        /// <param name="isFilterStore">是否过滤门店</param>
        /// <returns></returns>
        public static object DanPingPageList(NameValueCollection nvl, out int recordCount, bool isFilterStore = false)
        {
            var parentType = nvl["parentType"];//父类
            var childType = nvl["childType"].IsNullOrEmpty() ? -1 : int.Parse(nvl["childType"]);//子类
            var state = nvl["state"].IsNullOrEmpty() ? -1 : short.Parse(nvl["state"]);//状态
            var searchText = nvl["searchText"].Trim();//查找文本
            //int? big = null, mid = null, sub = null;
            //CommonService.GetCategory(nvl["parentType_0"], ref big, ref mid, ref sub);
            var childs = new List<int>();
            if (!parentType.IsNullOrEmpty())
            {
                var big = int.Parse(parentType);
                childs = ProductCategoryService.GetChildSNs(new List<int>() { big }, true);
            }
            var express = DynamicallyLinqHelper.Empty<VwProduct>().And(o => childs.Contains(o.CategorySN), !childs.Any())
                .And(o => o.ProductCode.Contains(searchText) || o.Barcode.Contains(searchText) || o.Title.Contains(searchText), searchText.IsNullOrEmpty())
                .And(o => o.CompanyId == CommonService.CompanyId);
            var exp = DynamicallyLinqHelper.Empty<CommodityPromotion>().And(o => o.State == state, state == -1).And(o => o.CompanyId == CommonService.CompanyId);
            if (isFilterStore)
            {
                exp = exp.And(o => (o.StoreId.Contains("-1") || ("," + o.StoreId + ",").Contains("," + Sys.SysCommonRules.CurrentStore + ",")));
            }
            var queryDiscount = BaseService<CommodityDiscount>.CurrentRepository.QueryEntity;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity;
            var queryPromotion = CurrentRepository.QueryEntity.Where(exp);
            queryProduct = queryProduct.Where(express);

            var query = from x in queryPromotion
                        join y in queryDiscount on x.Id equals y.CommodityId
                        join z in queryProduct on y.Barcode equals z.Barcode
                        select new
                        {
                            x.CompanyId,
                            x.CreateDT,
                            x.StartDate,
                            x.EndDate,
                            x.CustomerObj,
                            x.State,
                            y.Id,
                            y.CommodityId,
                            y.DiscountPrice,
                            y.DiscountRate,
                            y.MinPurchaseNum,
                            y.Way,
                            pro = z,
                            x.StoreId
                        };

            if (!Sys.CurrentUser.StoreId.IsNullOrEmpty())
                query = query.Where(o => ("," + o.StoreId + ",").Contains("," + Sys.CurrentUser.StoreId + ",") || o.StoreId.Contains("-1"));
            recordCount = query.Count();
            var nl = new NameValueCollection() { nvl };
            nl["sort"] = "CreateDT";
            var list = query.ToPageList(nl);
            ProductService.SetSysPrice(Sys.CurrentUser.StoreId, list.Select(o => (VwProduct)o.pro).ToList());
            return list.Select(o => new
            {
                BetWeen = DateDesc(o.StartDate, o.EndDate),
                Customer = Enum.GetName(typeof(CustomerType), o.CustomerObj),
                StateTitle = Enum.GetName(typeof(SaleState), o.State),
                o.CreateDT,
                o.Id,
                o.State,
                o.CommodityId,
                o.DiscountPrice,
                o.DiscountRate,
                o.MinPurchaseNum,
                WayDesc = ((DiscountWay)Enum.Parse(typeof(DiscountWay), o.Way.ToString())).GetEnumDescription(),
                o.pro.Barcode,
                o.pro.ProductCode,
                o.pro.Title,
                o.pro.BrandSN,
                o.pro.SysPrice,
                o.pro.BrandTitle,
                o.pro.CategoryTitle
            });
        }
        /// <summary>
        /// 保存或修改
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="times">时间段</param>
        /// <returns></returns>
        public static OpResult DanPingSaveOrUpdate(CommodityPromotion obj, string times)
        {
            var re = new OpResult();
            try
            {
                if (!times.IsNullOrEmpty())
                {
                    var ts = times.Split(',');
                    obj.StartAging1 = ts[0];
                    obj.EndAging1 = ts[1];
                    obj.StartAging2 = ts[2];
                    obj.EndAging2 = ts[3];
                    obj.StartAging3 = ts[4];
                    obj.EndAging3 = ts[5];
                }
                var discountList = new List<CommodityDiscount>();
                if (!string.IsNullOrWhiteSpace(obj.Inserted))
                {
                    var adds = obj.Inserted.ToObject<List<CommodityDiscount>>();
                    if (adds.Any())
                        discountList.AddRange(adds.Where(o => !string.IsNullOrWhiteSpace(o.Barcode)));
                }
                if (!obj.Id.IsNullOrEmpty() && obj.State == 2)//过期则新增
                {
                    var res = CurrentRepository.QueryEntity.Include(o => o.CommodityDiscounts).FirstOrDefault(o => o.Id == obj.Id);
                    foreach (var dis in res.CommodityDiscounts)
                    {
                        var todis = new CommodityDiscount();
                        dis.ToCopyProperty(todis);
                        //todis.Id=0;
                        todis.CommodityId = "";
                        discountList.Add(todis);
                    }
                    if (!string.IsNullOrWhiteSpace(obj.Updated))
                    {
                        var ups = obj.Updated.ToObject<List<CommodityDiscount>>();
                        ups.Each(o =>
                        {
                            var dis = discountList.FirstOrDefault(i => i.Id == o.Id);
                            if (dis != null)
                            {
                                o.ToCopyProperty(dis);
                            }
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(obj.Deleted))
                    {
                        var dels = obj.Deleted.ToObject<List<CommodityDiscount>>();
                        if (dels.Any())
                        {
                            foreach (var d in dels)
                                discountList.RemoveAll(i => i.Id == d.Id);
                        }
                    }
                    obj.Id = null;
                }
                discountList.Each(o => { o.CategorySN = -1; o.CompanyId = CommonService.CompanyId; });
                if (obj.Id.IsNullOrEmpty())
                {
                    var query = from x in CurrentRepository.Entities
                                join y in CommodityDiscountService.CurrentRepository.Entities on x.Id equals y.CommodityId
                                where x.State != 2 && x.EndDate >= obj.StartDate
                                select new
                                {
                                    x.EndDate,
                                    x.State,
                                    y.Barcode,
                                    y.MinPurchaseNum,
                                    y.Way
                                };
                    var barcodes = discountList.Select(o => o.Barcode).ToList();
                    var pros = query.Where(o => barcodes.Contains(o.Barcode)).ToList();
                    foreach (var discount in discountList)
                    {
                        if (pros.Any(o => o.Barcode == discount.Barcode && o.MinPurchaseNum == discount.MinPurchaseNum))
                        {
                            re.Message = "商品条码[" + discount.Barcode + "]同个数量不能在同个时间段内促销";
                            return re;
                        }
                        if (pros.Any(o => o.Barcode == discount.Barcode && o.Way != discount.Way))
                        {
                            re.Message = "商品条码[" + discount.Barcode + "]存在不同的折扣方式";
                            return re;
                        }
                    }
                    obj.CommodityDiscounts = discountList;
                    var list = obj.CommodityDiscounts.GroupBy(o => o.Barcode, o => o.MinPurchaseNum).ToList();
                    var list2 = obj.CommodityDiscounts.GroupBy(o => new { o.Barcode, o.MinPurchaseNum });
                    var ls = list2.Select(o => new { o.Key, Cout = o.Count() }).ToList();
                    var lt = discountList.Distinct(new Eqcomp());
                    var bars = discountList.GroupBy(o => o.Barcode).Where(o => o.Count() > 1).Select(o => o.Key).ToList();
                    var ways = discountList.Where(o => bars.Contains(o.Barcode)).GroupBy(o => o.Way);
                    if (ways.Count() > 1)
                    {
                        re.Message = "同个商品折扣方式需一致";
                        return re;
                    }
                    if (lt.Count() != discountList.Count)
                    {
                        re.Message = "同个商品购买量不能一致";
                        return re;
                    }
                    var msg = _promotionDal.PromotionValidMsg("1", obj.StartDate.GetValueOrDefault(), barcodes, null);
                    if (!msg.IsNullOrEmpty())
                    {
                        re.Message = msg;
                        return re;
                    }
                    obj.Id = Pharos.Logic.CommonRules.GUID;
                    obj.CreateDT = DateTime.Now;
                    obj.CreateUID = Sys.CurrentUser.UID;
                    obj.PromotionType = 1;
                    obj.State = (short)obj.SaleState;
                    obj.CompanyId = CommonService.CompanyId;
                    re = Add(obj);
                }
                else
                {
                    var res = CurrentRepository.QueryEntity.Include(o => o.CommodityDiscounts).FirstOrDefault(o => o.Id == obj.Id);
                    obj.ToCopyProperty(res);
                    res.CompanyId = CommonService.CompanyId;
                    res.State = (short)obj.SaleState;
                    res.CommodityDiscounts.AddRange(discountList);
                    if (!string.IsNullOrWhiteSpace(obj.Updated))
                    {
                        var ups = obj.Updated.ToObject<List<CommodityDiscount>>();
                        ups.Each(o =>
                        {
                            var dis = res.CommodityDiscounts.FirstOrDefault(i => i.Id == o.Id);
                            if (dis != null)
                            {
                                o.ToCopyProperty(dis);
                                dis.CompanyId = res.CompanyId;
                            }
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(obj.Deleted))
                    {
                        var dels = obj.Deleted.ToObject<List<CommodityDiscount>>();
                        if (dels.Any())
                        {
                            var ids = dels.Select(o => o.Id).ToList();
                            BaseService<CommodityDiscount>.CurrentRepository.RemoveRange(res.CommodityDiscounts.Where(o => ids.Contains(o.Id)).ToList(), false);
                            res.CommodityDiscounts.RemoveAll(o => ids.Contains(o.Id));
                        }
                    }
                    var lt = res.CommodityDiscounts.Distinct(new Eqcomp());
                    if (lt.Count() != res.CommodityDiscounts.Count)
                    {
                        re.Message = "同个商品数量不能一致";
                        return re;
                    }
                    re = Update(res);
                }
                if (re.Successed)
                {
                    var companyId = Sys.SysCommonRules.CompanyId;
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        System.Threading.Thread.Sleep(1000 * 30);
                        Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = companyId, StoreId = obj.StoreId, Target = "CommodityDiscountPackage" });
                    });
                }
                Log.WriteInfo(re.Successed ? "成功保存单品折扣" : "保存单品折扣失败");
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                Log.WriteError("保存单品折扣异常", ex);
            }
            return re;
        }
        /// <summary>
        /// 用于修改回显列表数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="recordCount">返回记录数</param>
        /// <returns></returns>
        public static object FindDanPingDetailsById(string id, out int recordCount)
        {
            recordCount = 0;
            if (id.IsNullOrEmpty()) return null;
            var queryDiscount = BaseService<CommodityDiscount>.CurrentRepository.QueryEntity;
            queryDiscount = queryDiscount.Where(o => o.CommodityId == id);
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity;
            var query = from y in queryDiscount
                        join z in queryProduct on new { y.CompanyId, y.Barcode } equals new { z.CompanyId, z.Barcode }

                        select new
                        {
                            y.Id,
                            y.DiscountPrice,
                            y.DiscountRate,
                            y.MinPurchaseNum,
                            y.CategorySN,
                            z.Barcode,
                            z.ProductCode,
                            z.Title,
                            z.BrandSN,
                            z.SysPrice,
                            z.SubUnitId,
                            StockNums = z.AcceptNums - z.PurchaseNumbers,
                            z.SubCategoryTitle,
                            z.SubUnit,
                            y.Way,
                            z.ValuationType
                        };
            var list = query.ToList().Select(o => new
            {
                o.Id,
                o.DiscountPrice,
                o.DiscountRate,
                o.MinPurchaseNum,
                o.CategorySN,
                o.Barcode,
                o.ProductCode,
                o.Title,
                o.BrandSN,
                o.SysPrice,
                o.SubUnitId,
                o.StockNums,
                o.SubCategoryTitle,
                o.SubUnit,
                o.Way,
                o.ValuationType,
                Way2 = ((DiscountWay)Enum.Parse(typeof(DiscountWay), o.Way.ToString())).GetEnumDescription(),
            });
            recordCount = list.Count();
            return list;
        }
        #endregion
        #region 捆绑销售
        /// <summary>
        /// 捆销销售列表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static object KubangPageList(NameValueCollection nvl, out int recordCount)
        {
            var state = nvl["state"].IsNullOrEmpty() ? -1 : short.Parse(nvl["state"]);
            var searchText = nvl["searchText"].Trim();
            /*var exp = DynamicallyLinqHelper.Empty<CommodityPromotion>().And(o => o.State == state, state == -1);
            var queryBundling = BaseService<Bundling>.CurrentRepository.QueryEntity;
            var queryBundlings = BaseService<BundlingList>.CurrentRepository.QueryEntity;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity;
            var queryPromotion = CurrentRepository.QueryEntity.Where(exp);
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var query = from x in queryPromotion
                        join y in queryBundling on x.Id equals y.CommodityId
                        join z in queryBundlings on y.CommodityId equals z.CommodityId
                        join h in queryProduct on z.Barcode equals h.Barcode
                        orderby y.NewBarcode
                        select new
                        {
                            x.CreateDT,
                            x.StartDate,
                            x.EndDate,
                            x.CustomerObj,
                            x.State,
                            y.CommodityId,
                            y.NewBarcode,
                            y.BundledPrice,
                            y.TotalBundled,
                            pro=h,
                            z.Id,
                            z.Number,
                            NewBarcode2 = y.NewBarcode,
                            x.StoreId
                        };
            if (!Sys.CurrentUser.StoreId.IsNullOrEmpty())
                query = query.Where(o => ("," + o.StoreId + ",").Contains("," + Sys.CurrentUser.StoreId + ","));
            if (!searchText.IsNullOrEmpty())
                query = query.Where(o => o.NewBarcode.StartsWith(searchText) || o.pro.ProductCode.StartsWith(searchText) || o.pro.Barcode.StartsWith(searchText) || o.pro.Title.Contains(searchText));
            recordCount = query.Count();
            var list = query.ToPageList();*/
            var nl = new NameValueCollection() { nvl };
            nl.Add("storeId", Sys.CurrentUser.StoreId);
            nl.Add("companyid", CommonService.CompanyId.ToString());
            var dt = _promotionDal.KubangPageData(nl, out recordCount);
            ProductService.SetSysPrice(Sys.CurrentUser.StoreId, dt);
            //watch.Stop();
            //System.Diagnostics.Debug.WriteLine(watch.ElapsedMilliseconds);
            return dt;
        }
        /// <summary>
        /// 保存或修改
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="times">时间段</param>
        /// <returns></returns>
        public static OpResult KubangSaveOrUpdate(PromotionBundling obj, string times)
        {
            var re = new OpResult();
            try
            {
                if (!times.IsNullOrEmpty())
                {
                    var ts = times.Split(',');
                    obj.StartAging1 = ts[0];
                    obj.EndAging1 = ts[1];
                    obj.StartAging2 = ts[2];
                    obj.EndAging2 = ts[3];
                    obj.StartAging3 = ts[4];
                    obj.EndAging3 = ts[5];
                }
                var bundlist = new List<BundlingList>();
                if (!string.IsNullOrWhiteSpace(obj.Inserted))
                {
                    var adds = obj.Inserted.ToObject<List<BundlingList>>();
                    if (adds.Any())
                        bundlist.AddRange(adds.Where(o => !string.IsNullOrWhiteSpace(o.Barcode)));
                }
                if (!obj.Id.IsNullOrEmpty() && obj.State == 2)//过期则新增
                {
                    var res = CurrentRepository.QueryEntity.Include(o => o.BundlingDetails).FirstOrDefault(o => o.Id == obj.Id);
                    foreach (var dis in res.BundlingDetails)
                    {
                        var todis = new BundlingList();
                        dis.ToCopyProperty(todis);
                        todis.Id = 0;
                        todis.CommodityId = null;
                        bundlist.Add(todis);
                    }
                    if (!string.IsNullOrWhiteSpace(obj.Updated))
                    {
                        var ups = obj.Updated.ToObject<List<BundlingList>>();
                        ups.Each(o =>
                        {
                            var dis = bundlist.FirstOrDefault(i => i.Barcode == o.Barcode);
                            if (dis != null)
                            {
                                o.ToCopyProperty(dis);
                            }
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(obj.Deleted))
                    {
                        var dels = obj.Deleted.ToObject<List<BundlingList>>();
                        if (dels.Any())
                        {
                            foreach (var d in dels)
                                bundlist.RemoveAll(o => o.Barcode == d.Barcode);
                        }
                    }
                    obj.Id = null;
                }
                if (!obj.GenerateCode && !obj.NewBarcode.IsNullOrEmpty() && BundlingService.IsExist(o => o.NewBarcode == obj.NewBarcode && o.CommodityId != obj.Id))
                {
                    re.Message = "捆绑条码已存在"; return re;
                }
                if (obj.Id.IsNullOrEmpty())
                {
                    var msg = _promotionDal.PromotionValidMsg("2", obj.StartDate.GetValueOrDefault(), bundlist.Select(o => o.Barcode), null);
                    if (!msg.IsNullOrEmpty())
                    {
                        re.Message = msg;
                        return re;
                    }
                    var prom = new CommodityPromotion();
                    var bundling = new Bundling();
                    prom.BundlingDetails = bundlist;
                    obj.ToCopyProperty(prom);
                    obj.ToCopyProperty(bundling);
                    bundling.CompanyId = CommonService.CompanyId;
                    bundling.SysPrices = bundlist.Sum(o => o.SysPrice.GetValueOrDefault());
                    bundling.BuyPrices = bundlist.Sum(o => o.BuyPrice.GetValueOrDefault());
                    if (obj.GenerateCode)
                    {
                        bundling.NewBarcode = GenerateBundlingBar(bundlist.Count().ToString("00") + bundlist[0].ProductCode);
                    }
                    prom.Id = bundling.CommodityId = Pharos.Logic.CommonRules.GUID;
                    prom.CreateDT = DateTime.Now;
                    prom.CreateUID = Sys.CurrentUser.UID;
                    prom.Bundlings = new List<Bundling>() { bundling };
                    prom.PromotionType = 2;
                    prom.State = (short)prom.SaleState;
                    prom.CompanyId = bundling.CompanyId;
                    re = Add(prom);
                    if (re.Successed)
                    {
                        Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = Sys.SysCommonRules.CompanyId, StoreId = obj.StoreId, Target = "CommodityBundlingPackage" });
                    }
                }
                else
                {
                    var res = CurrentRepository.QueryEntity.Include(o => o.BundlingDetails).Include(o => o.Bundlings).FirstOrDefault(o => o.Id == obj.Id);
                    obj.ToCopyProperty(res);
                    res.CompanyId = CommonService.CompanyId;
                    res.State = (short)obj.SaleState;
                    res.BundlingDetails.AddRange(bundlist);
                    var bund = res.Bundlings.FirstOrDefault();
                    obj.ToCopyProperty(bund);
                    bund.CompanyId = res.CompanyId;
                    var refeshItems = new List<object>();
                    if (!string.IsNullOrWhiteSpace(obj.Updated))
                    {
                        var ups = obj.Updated.ToObject<List<BundlingList>>();
                        ups.Each(o =>
                        {
                            var dis = res.BundlingDetails.FirstOrDefault(i => i.Id == o.Id);
                            if (dis != null)
                            {
                                o.ToCopyProperty(dis);
                            }
                        });
                        bundlist.AddRange(ups);
                        if (ups.Any())
                            refeshItems.Add(new { StoreId = obj.StoreId, Barcode = bund.NewBarcode, ProductCode = "", ProductType = 2 });
                    }
                    if (bundlist.Any())
                    {
                        bund.SysPrices = bundlist.Sum(o => o.SysPrice.GetValueOrDefault());
                        bund.BuyPrices = bundlist.Sum(o => o.BuyPrice.GetValueOrDefault());
                    }
                    if (!string.IsNullOrWhiteSpace(obj.Deleted))
                    {
                        var dels = obj.Deleted.ToObject<List<BundingTrans>>();
                        if (dels.Any())
                        {
                            var ids = dels.Select(o => o.Id).ToList();
                            var deletelist = res.BundlingDetails.Where(o => ids.Contains(o.Id)).ToList();
                            BaseService<BundlingList>.CurrentRepository.RemoveRange(deletelist, false);
                            res.BundlingDetails.RemoveAll(o => ids.Contains(o.Id));
                        }
                    }
                    re = Update(res);
                    if (re.Successed)
                    {
                        Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = Sys.SysCommonRules.CompanyId, StoreId = obj.StoreId, Target = "CommodityBundlingPackage" });
                        Pharos.Infrastructure.Data.Redis.RedisManager.Publish("RefreshProductCache", new List<Pharos.ObjectModels.DTOs.MemoryCacheRefreshQuery>() { new Pharos.ObjectModels.DTOs.MemoryCacheRefreshQuery() { Barcode = bund.NewBarcode, CompanyId = res.CompanyId, StoreId = res.StoreId, ProductType = Pharos.ObjectModels.ProductType.Bundling } });
                    }
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
        /// 用于修改回显数据
        /// </summary>
        /// <param name="commodityId">主键</param>
        /// <returns></returns>
        public static PromotionBundling FindKuBangById(string commodityId)
        {
            var queryBundling = BaseService<Bundling>.CurrentRepository.QueryEntity;
            var queryBundlings = BaseService<BundlingList>.CurrentRepository.QueryEntity;
            var queryPromotion = CurrentRepository.QueryEntity;
            var query = from x in queryPromotion
                        from y in queryBundling
                        where x.Id == y.CommodityId && y.CommodityId == commodityId
                        select new { x, y };
            var obj = query.FirstOrDefault();
            var target = new PromotionBundling();
            obj.x.ToCopyProperty(target);
            obj.y.ToCopyProperty(target);
            target.State = obj.x.State;
            return target;
        }
        /// <summary>
        /// 用于修改回显列表数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static object FindKubangDetailsById(string id, out int recordCount)
        {
            recordCount = 0;
            if (id.IsNullOrEmpty()) return null;
            var queryDetail = BaseService<BundlingList>.CurrentRepository.QueryEntity;
            queryDetail = queryDetail.Where(o => o.CommodityId == id);
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var query = from y in queryDetail
                        from z in queryProduct
                        where y.Barcode == z.Barcode || ("," + z.Barcodes + ",").Contains("," + y.Barcode + ",")
                        select new
                        {
                            y.Id,
                            y.Number,
                            y.CommodityId,
                            z.Barcode,
                            z.ProductCode,
                            z.Title,
                            z.BrandSN,
                            y.SysPrice,
                            z.SubUnitId,
                            z.BrandTitle,
                            y.BuyPrice,
                            StockNums = z.AcceptNums - z.PurchaseNumbers,
                            z.SubCategoryTitle,
                            z.SubUnit
                        };
            var list = query.ToList();
            recordCount = list.Count;
            return list;
        }
        #endregion
        #region 组合销售
        /// <summary>
        /// 组合销售列表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static object ZuhePageList(NameValueCollection nvl, out int recordCount, bool isFilterStore = false)
        {
            var state = nvl["state"].IsNullOrEmpty() ? -1 : short.Parse(nvl["state"]);
            var searchText = nvl["searchText"].Trim();
            var exp = DynamicallyLinqHelper.Empty<CommodityPromotion>().And(o => o.State == state, state == -1).And(o => o.CompanyId == CommonService.CompanyId);
            if (isFilterStore)
            {
                exp = exp.And(o => (o.StoreId.Contains("-1") || ("," + o.StoreId + ",").Contains("," + Sys.SysCommonRules.CurrentStore + ",")));
            }
            var queryBlend = BaseService<PromotionBlend>.CurrentRepository.QueryEntity;
            var queryBlends = BaseService<PromotionBlendList>.CurrentRepository.QueryEntity;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryCategory = BaseService<ProductCategory>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryPromotion = CurrentRepository.QueryEntity.Where(exp);

            var query = from x in queryPromotion
                        join y in queryBlend on x.Id equals y.CommodityId
                        where y.RuleType == 1
                        select new
                        {
                            x.Id,
                            x.CreateDT,
                            x,
                            y,
                            x.StoreId
                        };
            if (!Sys.CurrentUser.StoreId.IsNullOrEmpty())
                query = query.Where(o => ("," + o.StoreId + ",").Contains("," + Sys.CurrentUser.StoreId + ",") || o.StoreId.Contains("-1"));
            if (!searchText.IsNullOrEmpty())
            {
                query = from x in query
                        let o = from h in queryProduct
                                join z in queryBlends on x.Id equals z.CommodityId
                                where (z.BarcodeOrCategorySN == h.Barcode || ("," + h.Barcodes + ",").Contains("" + z.BarcodeOrCategorySN + ",")) &&
                                    (h.Barcode.Contains(searchText) || h.Title.Contains(searchText) || ("," + h.Barcodes + ",").Contains("," + z.BarcodeOrCategorySN + ","))
                                select h
                        where o.Any()
                        select x;
            }
            recordCount = query.Count();
            var sql = query.ToString();
            var nl = new NameValueCollection() { nvl };
            nl["sort"] = "CreateDT";
            var list = query.ToPageList(nl);
            if (!list.Any()) return list;
            var commIds = list.Select(o => o.Id).Distinct().ToList();
            var blends = queryBlends.Where(o => commIds.Contains(o.CommodityId)).ToList();
            var barcodes = blends.Where(o => o.BlendType == 3 || o.BlendType == 1).Select(o => o.BarcodeOrCategorySN).Distinct().ToList();
            var categoryIds = blends.Where(o => o.BlendType == 4 || o.BlendType == 2).Select(o => int.Parse(o.BarcodeOrCategorySN)).Distinct().ToList();
            var products = queryProduct.Where(o => barcodes.Contains(o.Barcode)).ToList();
            var categorys = queryCategory.Where(o => categoryIds.Contains(o.CategorySN)).ToList();
            return list.Select(o => new
            {
                BetWeen = DateDesc(o.x.StartDate, o.x.EndDate, o.x.Timeliness),
                Customer = Enum.GetName(typeof(CustomerType), o.x.CustomerObj),
                StateTitle = Enum.GetName(typeof(SaleState), o.x.State),
                o.x.RestrictionBuyNum,
                o.x.Id,
                o.x.State,
                Product = CommonDetail(o.y, blends, products, categorys),
                Category = Detail(o.y, blends, products, categorys, false),
                SellWay = SellWay(o.y),
                o.x.CreateDT,
                o.x.CompanyId
            });
        }
        /// <summary>
        /// 保存或修改
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="times">时间段</param>
        /// <returns></returns>
        public static OpResult ZuheSaveOrUpdate(PromotBlend obj, string times)
        {
            var re = new OpResult();
            try
            {
                if (!times.IsNullOrEmpty())
                {
                    var ts = times.Split(',');
                    obj.StartAging1 = ts[0];
                    obj.EndAging1 = ts[1];
                    obj.StartAging2 = ts[2];
                    obj.EndAging2 = ts[3];
                    obj.StartAging3 = ts[4];
                    obj.EndAging3 = ts[5];
                }
                var blendDetails = new List<PromotionBlendList>();
                if (!string.IsNullOrWhiteSpace(obj.InsertTypeed))
                {
                    var adds = obj.InsertTypeed.ToObject<List<TransObject>>().Where(o => o.CategorySN > 0);
                    if (adds.Any())
                    {
                        blendDetails.AddRange(adds.Select(o => new PromotionBlendList()
                        {
                            BarcodeOrCategorySN = Convert.ToString(o.CategorySN),
                            BrandSN = o.BrandSN.GetValueOrDefault(),
                            BlendType = 2,
                            CategoryGrade = o.CategoryGrade,
                            Id = 0
                        }));
                    }
                }
                if (!string.IsNullOrWhiteSpace(obj.InsertProducted))
                {
                    var barcodes = obj.InsertProducted.Split(',');
                    if (barcodes.Any())
                    {
                        blendDetails.AddRange(barcodes.Select(o => new PromotionBlendList()
                        {
                            BarcodeOrCategorySN = o,
                            BlendType = 1,
                            Id = 0
                        }));
                    }
                }
                if (!string.IsNullOrWhiteSpace(obj.InsertNoTypeed))
                {
                    var adds = obj.InsertNoTypeed.ToObject<List<TransObject>>().Where(o => o.CategorySN > 0);
                    if (adds.Any())
                    {
                        blendDetails.AddRange(adds.Select(o => new PromotionBlendList()
                        {
                            BarcodeOrCategorySN = Convert.ToString(o.CategorySN),
                            BrandSN = o.BrandSN.GetValueOrDefault(),
                            CategoryGrade = o.CategoryGrade,
                            BlendType = 4,
                            Id = 0
                        }));
                    }
                }
                if (!string.IsNullOrWhiteSpace(obj.InsertNoProducted))
                {
                    var barcodes = obj.InsertNoProducted.Split(',');
                    if (barcodes.Any())
                    {
                        blendDetails.AddRange(barcodes.Select(o => new PromotionBlendList()
                        {
                            BarcodeOrCategorySN = o.Split(':')[0],
                            Number = o.Split(':')[1].ToType<short?>() ?? 1,
                            BlendType = 3,
                            Id = 0
                        }));
                    }
                }
                if (!obj.Id.IsNullOrEmpty() && obj.State == 2)//过期则新增
                {
                    var res = CurrentRepository.QueryEntity.Include(o => o.BlendDetails).FirstOrDefault(o => o.Id == obj.Id);
                    foreach (var dis in res.BlendDetails)
                    {
                        var todis = new PromotionBlendList();
                        dis.ToCopyProperty(todis);
                        todis.Id = 0;
                        todis.CommodityId = null;
                        blendDetails.Add(todis);
                    }
                    if (!string.IsNullOrWhiteSpace(obj.DeleteProducted))
                    {
                        var dels = obj.DeleteProducted.ToObject<List<TransObject>>();
                        if (dels.Any())
                        {
                            foreach (var d in dels)
                                blendDetails.RemoveAll(o => o.BarcodeOrCategorySN == d.Barcode && o.BlendType == 1);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(obj.DeleteTypeed))
                    {
                        var dels = obj.DeleteTypeed.ToObject<List<TransObject>>();
                        if (dels.Any())
                        {
                            var ids = dels.Select(o => int.Parse(o.Id)).ToList();
                            blendDetails.RemoveAll(o => ids.Contains(o.Id) && o.BlendType == 2);
                        }
                    }
                    if (obj.PromotionType2 == 4)
                    {
                        if (!string.IsNullOrWhiteSpace(obj.DeleteNoTypeed))
                        {
                            var dels = obj.DeleteNoTypeed.ToObject<List<TransObject>>();
                            if (dels.Any())
                            {
                                var ids = dels.Select(o => int.Parse(o.Id)).ToList();
                                blendDetails.RemoveAll(o => ids.Contains(o.Id) && o.BlendType == 4);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(obj.DeleteNoProducted))
                        {
                            var dels = obj.DeleteNoProducted.ToObject<List<TransObject>>();
                            if (dels.Any())
                            {
                                foreach (var d in dels)
                                    blendDetails.RemoveAll(o => o.BarcodeOrCategorySN == d.Barcode && o.BlendType == 3);
                            }
                        }
                    }
                    else
                        blendDetails.RemoveAll(o => o.BlendType == 3 || o.BlendType == 4);
                    obj.Id = null;
                }
                var msg = _promotionDal.PromotionValidMsg("3", obj.StartDate.GetValueOrDefault(), blendDetails.Where(o => o.BlendType == 1).Select(o => o.BarcodeOrCategorySN), blendDetails.Where(o => o.BlendType == 2).Select(o => o.BarcodeOrCategorySN));
                if (!msg.IsNullOrEmpty())
                {
                    re.Message = msg;
                    return re;
                }
                var prom = new CommodityPromotion();
                var blend = new PromotionBlend();
                if (obj.Id.IsNullOrEmpty())
                {
                    obj.ToCopyProperty(prom);
                    obj.ToCopyProperty(blend);
                    prom.Id = blend.CommodityId = Pharos.Logic.CommonRules.GUID;
                    prom.PromotionType = 3;
                    prom.CreateDT = DateTime.Now;
                    prom.CreateUID = Sys.CurrentUser.UID;
                    blend.RuleType = 1;
                    blend.PromotionType = obj.PromotionType2;
                    blend.CompanyId = CommonService.CompanyId;
                    prom.Blends = new List<PromotionBlend>() { blend };
                    prom.BlendDetails = blendDetails;
                    prom.State = (short)prom.SaleState;
                    prom.CompanyId = blend.CompanyId;
                    re = Add(prom);
                }
                else
                {
                    prom = CurrentRepository.QueryEntity.Include(o => o.BlendDetails).Include(o => o.Blends).FirstOrDefault(o => o.Id == obj.Id);
                    obj.ToCopyProperty(prom);
                    prom.CompanyId = CommonService.CompanyId;
                    prom.State = (short)obj.SaleState;
                    blend = prom.Blends.FirstOrDefault();
                    obj.ToCopyProperty(blend);
                    blend.CompanyId = prom.CompanyId;
                    prom.BlendDetails.AddRange(blendDetails);
                    if (!string.IsNullOrWhiteSpace(obj.DeleteProducted))
                    {
                        var dels = obj.DeleteProducted.ToObject<List<TransObject>>();
                        if (dels.Any())
                        {
                            var ids = dels.Select(o => long.Parse(o.Id)).ToList();
                            BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(prom.BlendDetails.Where(o => ids.Contains(o.Id)).ToList(), false);
                            prom.BlendDetails.RemoveAll(o => ids.Contains(o.Id));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(obj.DeleteTypeed))
                    {
                        var dels = obj.DeleteTypeed.ToObject<List<TransObject>>();
                        if (dels.Any())
                        {
                            var ids = dels.Select(o => int.Parse(o.Id)).ToList();
                            var dts = prom.BlendDetails.Where(o => ids.Contains(o.Id) && o.CommodityId == obj.Id).ToList();
                            BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(dts, false);
                            prom.BlendDetails.RemoveAll(o => ids.Contains(o.Id) && o.CommodityId == obj.Id);
                        }
                    }
                    if (obj.PromotionType2 == 4)
                    {
                        if (!string.IsNullOrWhiteSpace(obj.DeleteNoProducted))
                        {
                            var dels = obj.DeleteNoProducted.ToObject<List<TransObject>>();
                            if (dels.Any())
                            {
                                var ids = dels.Select(o => long.Parse(o.Id)).ToList();
                                BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(prom.BlendDetails.Where(o => ids.Contains(o.Id)).ToList(), false);
                                prom.BlendDetails.RemoveAll(o => ids.Contains(o.Id));
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(obj.DeleteNoTypeed))
                        {
                            var dels = obj.DeleteNoTypeed.ToObject<List<TransObject>>();
                            if (dels.Any())
                            {
                                var ids = dels.Select(o => int.Parse(o.Id)).ToList();
                                var dts = prom.BlendDetails.Where(o => ids.Contains(o.Id) && o.CommodityId == obj.Id).ToList();
                                BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(dts, false);
                                prom.BlendDetails.RemoveAll(o => ids.Contains(o.Id) && o.CommodityId == obj.Id);
                            }
                        }
                        BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(prom.BlendDetails.Where(o => o.Id > 0 && o.BlendType == 3).ToList(), false);
                    }
                    else
                    {
                        BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(prom.BlendDetails.Where(o => o.BlendType == 3 || o.BlendType == 4).ToList(), false);
                    }
                    blend.PromotionType = obj.PromotionType2;
                    re = Update(prom);
                }
                if (re.Successed)
                {
                    Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = Sys.SysCommonRules.CompanyId, StoreId = obj.StoreId, Target = "CommodityBlendPackage" });
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
        /// 用于修改回显数据
        /// </summary>
        /// <param name="commodityId">主键</param>
        /// <returns></returns>
        public static PromotBlend FindZuheById(string commodityId)
        {
            var queryBlend = BaseService<PromotionBlend>.CurrentRepository.QueryEntity;
            var queryPromotion = CurrentRepository.QueryEntity;
            var query = from x in queryPromotion
                        from y in queryBlend
                        where x.Id == y.CommodityId && y.CommodityId == commodityId
                        select new { x, y };
            var obj = query.FirstOrDefault();
            var target = new PromotBlend();
            obj.x.ToCopyProperty(target);
            obj.y.ToCopyProperty(target);
            target.PromotionType2 = obj.y.PromotionType;
            target.SellWay = (target.PromotionType2 == 1 || target.PromotionType2 == 2) ? 1 : target.PromotionType2 == 3 ? 2 : 3;
            target.State = obj.x.State;
            return target;
        }
        /// <summary>
        /// 用于修改,回显商品和系列
        /// </summary>
        /// <param name="id">CommodityId主键</param>
        /// <param name="type">1:组合单品、2:组合系列、3:赠送单品、4:赠送系列、5:不参与促销单品、6:不参与促销系列</param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static object FindZuheDetailsById(string id, short type, out int recordCount)
        {
            recordCount = 0;
            if (id.IsNullOrEmpty()) return null;
            var queryDetail = BaseService<PromotionBlendList>.CurrentRepository.QueryEntity;
            queryDetail = queryDetail.Where(o => o.CommodityId == id);
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            if (type == 1 || type == 3 || type == 5)
            {
                var query = from y in queryDetail
                            from z in queryProduct
                            where (y.BarcodeOrCategorySN == z.Barcode || ("," + z.Barcodes + ",").Contains("," + y.BarcodeOrCategorySN + ",")) && y.BlendType == type
                            select new
                            {
                                y.Id,
                                y.Number,
                                z
                            };
                var list = query.ToList();
                recordCount = list.Count;
                return list.Select(o => new
                {
                    o.Id,
                    o.Number,
                    o.z.ProductCode,
                    o.z.Barcode,
                    o.z.Title,
                    o.z.CategoryTitle,
                    o.z.SubUnit,
                    o.z.SysPrice,
                    o.z.StockNums
                });
            }
            else
            {
                var dal = new Pharos.Logic.DAL.BlendListDAL();
                var dt = dal.GetBlendCategoryList(id, type);
                var list = dt.AsEnumerable().Select(dr => new
                {
                    Id = dr["Id"],
                    StrId = dr["CategorySN"] + "~" + dr["BrandSN"],
                    CategorySN = dr["CategorySN"],
                    SubCategoryTitle = dr["SubCategoryTitle"],
                    MidCategoryTitle = dr["MidCategoryTitle"],
                    BigCategoryTitle = dr["BigCategoryTitle"],
                    BrandSN = dr["BrandSN"],
                    BrandTitle = dr["BrandTitle"],
                    StockNums = dr["StockNums"],
                    CategoryGrade = dr["CategoryGrade"]
                }).ToList();
                recordCount = list.Count;
                return list;
            }

        }
        #endregion
        #region 满元销售
        /// <summary>
        /// 满元销售列表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static object ManYuanPageList(NameValueCollection nvl, out int recordCount, bool isFilterStore = false)
        {
            var state = nvl["state"].IsNullOrEmpty() ? -1 : short.Parse(nvl["state"]);
            var searchText = nvl["searchText"].Trim();
            var exp = DynamicallyLinqHelper.Empty<CommodityPromotion>().And(o => o.State == state, state == -1).And(o => o.CompanyId == CommonService.CompanyId);
            if (isFilterStore)
            {
                exp = exp.And(o => (o.StoreId.Contains("-1") || ("," + o.StoreId + ",").Contains("," + Sys.SysCommonRules.CurrentStore + ",")));
            }
            var queryBlend = BaseService<PromotionBlend>.CurrentRepository.QueryEntity;
            var queryBlends = BaseService<PromotionBlendList>.CurrentRepository.QueryEntity;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryCategory = BaseService<ProductCategory>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryPromotion = CurrentRepository.QueryEntity.Where(exp);
            var query = from x in queryPromotion
                        join y in queryBlend on x.Id equals y.CommodityId
                        where y.RuleType == 2
                        select new
                        {
                            x.Id,
                            x.CreateDT,
                            x,
                            y,
                            x.StoreId
                        };
            if (!Sys.CurrentUser.StoreId.IsNullOrEmpty())
                query = query.Where(o => ("," + o.StoreId + ",").Contains("," + Sys.CurrentUser.StoreId + ",") || o.StoreId.Contains("-1"));
            if (!searchText.IsNullOrEmpty())
            {
                query = from x in query
                        let o = from h in queryProduct
                                join z in queryBlends on x.Id equals z.CommodityId
                                where (z.BarcodeOrCategorySN == h.Barcode || ("," + h.Barcodes + ",").Contains("" + z.BarcodeOrCategorySN + ",")) &&
                                    (h.Barcode.Contains(searchText) || h.Title.Contains(searchText) || ("," + h.Barcodes + ",").Contains("," + z.BarcodeOrCategorySN + ","))
                                select h
                        where o.Any()
                        select x;
            }
            recordCount = query.Count();
            var nl = new NameValueCollection() { nvl };
            nl["sort"] = "CreateDT";
            var list = query.ToPageList(nl);
            if (!list.Any()) return list;
            var commIds = list.Select(o => o.Id).Distinct().ToList();
            var blends = queryBlends.Where(o => commIds.Contains(o.CommodityId)).ToList();
            var barcodes = blends.Where(o => o.BlendType == 3 || o.BlendType == 5).Select(o => o.BarcodeOrCategorySN).Distinct().ToList();
            var categoryIds = blends.Where(o => o.BlendType == 4 || o.BlendType == 6).Select(o => int.Parse(o.BarcodeOrCategorySN)).Distinct().ToList();
            var products = queryProduct.Where(o => barcodes.Contains(o.Barcode)).ToList();
            var categorys = queryCategory.Where(o => categoryIds.Contains(o.CategorySN)).ToList();
            return list.Select(o => new
            {
                BetWeen = DateDesc(o.x.StartDate, o.x.EndDate, o.x.Timeliness),
                Customer = Enum.GetName(typeof(CustomerType), o.x.CustomerObj),
                StateTitle = Enum.GetName(typeof(SaleState), o.x.State),
                o.x.RestrictionBuyNum,
                o.x.Id,
                o.y.FullNumber,
                o.x.State,
                join = CommonDetail(o.y, blends, products, categorys),
                nojoin = Detail(o.y, blends, products, categorys, true),
                SellWay = SellWay(o.y),
                o.CreateDT,
                o.x.CompanyId
            });
        }
        /// <summary>
        /// 保存或修改
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="times">时间段</param>
        /// <returns></returns>
        public static OpResult ManYuanSaveOrUpdate(PromotBlend obj, string times)
        {
            var re = new OpResult();
            try
            {
                if (!times.IsNullOrEmpty())
                {
                    var ts = times.Split(',');
                    obj.StartAging1 = ts[0];
                    obj.EndAging1 = ts[1];
                    obj.StartAging2 = ts[2];
                    obj.EndAging2 = ts[3];
                    obj.StartAging3 = ts[4];
                    obj.EndAging3 = ts[5];
                }
                var blendDetails = new List<PromotionBlendList>();
                if (!string.IsNullOrWhiteSpace(obj.InsertTypeed))
                {
                    var adds = obj.InsertTypeed.ToObject<List<TransObject>>().Where(o => o.CategorySN > 0);
                    if (adds.Any())
                    {
                        blendDetails.AddRange(adds.Select(o => new PromotionBlendList()
                        {
                            BarcodeOrCategorySN = Convert.ToString(o.CategorySN),
                            BrandSN = o.BrandSN.GetValueOrDefault(),
                            CategoryGrade = o.CategoryGrade,
                            BlendType = 4,
                            Id = 0
                        }));
                    }
                }
                if (!string.IsNullOrWhiteSpace(obj.InsertProducted))
                {
                    //var adds = obj.InsertProducted.ToObject<List<TransObject>>().Where(o => !string.IsNullOrWhiteSpace(o.Barcode));
                    var barcodes = obj.InsertProducted.Split(',');
                    if (barcodes.Any())
                    {
                        blendDetails.AddRange(barcodes.Select(o => new PromotionBlendList()
                        {
                            BarcodeOrCategorySN = o.Split(':')[0],
                            Number = o.Split(':')[1].ToType<short?>() ?? 1,
                            BlendType = 3,
                            Id = 0
                        }));
                    }
                }
                if (!string.IsNullOrWhiteSpace(obj.InsertNoTypeed))
                {
                    var adds = obj.InsertNoTypeed.ToObject<List<TransObject>>().Where(o => o.CategorySN > 0);
                    if (adds.Any())
                    {
                        blendDetails.AddRange(adds.Select(o => new PromotionBlendList()
                        {
                            BarcodeOrCategorySN = Convert.ToString(o.CategorySN),
                            BrandSN = o.BrandSN.GetValueOrDefault(),
                            CategoryGrade = o.CategoryGrade,
                            BlendType = 6,
                            Id = 0
                        }));
                    }
                }
                if (!string.IsNullOrWhiteSpace(obj.InsertNoProducted))
                {
                    //var adds = obj.InsertNoProducted.ToObject<List<TransObject>>().Where(o => !string.IsNullOrWhiteSpace(o.Barcode));
                    var barcodes = obj.InsertNoProducted.Split(',');
                    if (barcodes.Any())
                    {
                        blendDetails.AddRange(barcodes.Select(o => new PromotionBlendList()
                        {
                            BarcodeOrCategorySN = o,
                            BlendType = 5,
                            Id = 0
                        }));
                    }
                }
                if (!obj.Id.IsNullOrEmpty() && obj.State == 2)//过期则新增
                {
                    var res = CurrentRepository.QueryEntity.Include(o => o.BlendDetails).FirstOrDefault(o => o.Id == obj.Id);
                    foreach (var dis in res.BlendDetails)
                    {
                        var todis = new PromotionBlendList();
                        dis.ToCopyProperty(todis);
                        todis.Id = 0;
                        todis.CommodityId = null;
                        blendDetails.Add(todis);
                    }
                    if (!string.IsNullOrWhiteSpace(obj.DeleteTypeed))
                    {
                        var dels = obj.DeleteTypeed.ToObject<List<TransObject>>();
                        if (dels.Any())
                        {
                            var ids = dels.Select(o => int.Parse(o.Id)).ToList();
                            blendDetails.RemoveAll(o => ids.Contains(o.Id) && o.BlendType == 4);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(obj.DeleteProducted))
                    {
                        var dels = obj.DeleteProducted.ToObject<List<TransObject>>();
                        if (dels.Any())
                        {
                            foreach (var d in dels)
                                blendDetails.RemoveAll(o => o.BarcodeOrCategorySN == d.Barcode && o.BlendType == 3);
                        }
                    }
                    if (obj.PromotionType2 == 4)
                    {
                        if (!string.IsNullOrWhiteSpace(obj.DeleteNoTypeed))
                        {
                            var dels = obj.DeleteNoTypeed.ToObject<List<TransObject>>();
                            if (dels.Any())
                            {
                                var ids = dels.Select(o => int.Parse(o.Id)).ToList();
                                blendDetails.RemoveAll(o => ids.Contains(o.Id) && o.BlendType == 6);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(obj.DeleteNoProducted))
                        {
                            var dels = obj.DeleteNoProducted.ToObject<List<TransObject>>();
                            if (dels.Any())
                            {
                                foreach (var d in dels)
                                    blendDetails.RemoveAll(o => o.BarcodeOrCategorySN == d.Barcode && o.BlendType == 5);
                            }
                        }
                    }
                    else
                        blendDetails.RemoveAll(o => o.BlendType == 3 || o.BlendType == 4);
                    obj.Id = null;
                }
                var prom = new CommodityPromotion();
                var blend = new PromotionBlend();
                if (obj.Id.IsNullOrEmpty())
                {
                    obj.ToCopyProperty(prom);
                    obj.ToCopyProperty(blend);
                    prom.Id = blend.CommodityId = Pharos.Logic.CommonRules.GUID;
                    prom.PromotionType = 5;
                    prom.CreateDT = DateTime.Now;
                    prom.CreateUID = Sys.CurrentUser.UID;
                    blend.RuleType = 2;
                    blend.PromotionType = obj.PromotionType2;
                    blend.CompanyId = CommonService.CompanyId;
                    prom.Blends = new List<PromotionBlend>() { blend };
                    prom.BlendDetails = blendDetails;
                    prom.State = (short)prom.SaleState;
                    prom.CompanyId = blend.CompanyId;
                    re = Add(prom);
                }
                else
                {
                    prom = CurrentRepository.QueryEntity.Include(o => o.BlendDetails).Include(o => o.Blends).FirstOrDefault(o => o.Id == obj.Id);
                    obj.ToCopyProperty(prom);
                    prom.CompanyId = CommonService.CompanyId;
                    prom.State = (short)obj.SaleState;
                    blend = prom.Blends.FirstOrDefault();
                    obj.ToCopyProperty(blend);
                    blend.CompanyId = prom.CompanyId;
                    prom.BlendDetails.AddRange(blendDetails);
                    if (!string.IsNullOrWhiteSpace(obj.DeleteNoProducted))
                    {
                        var dels = obj.DeleteNoProducted.ToObject<List<TransObject>>();
                        if (dels.Any())
                        {
                            var ids = dels.Select(o => long.Parse(o.Id)).ToList();
                            BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(prom.BlendDetails.Where(o => ids.Contains(o.Id)).ToList(), false);
                            prom.BlendDetails.RemoveAll(o => ids.Contains(o.Id));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(obj.DeleteNoTypeed))
                    {
                        var dels = obj.DeleteNoTypeed.ToObject<List<TransObject>>();
                        if (dels.Any())
                        {
                            var ids = dels.Select(o => int.Parse(o.Id)).ToList();
                            var dts = prom.BlendDetails.Where(o => ids.Contains(o.Id) && o.CommodityId == obj.Id).ToList();
                            BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(dts, false);
                            prom.BlendDetails.RemoveAll(o => ids.Contains(o.Id) && o.CommodityId == obj.Id);
                        }
                    }
                    if (obj.PromotionType2 == 4)
                    {
                        if (!string.IsNullOrWhiteSpace(obj.DeleteProducted))
                        {
                            var dels = obj.DeleteProducted.ToObject<List<TransObject>>();
                            if (dels.Any())
                            {
                                var ids = dels.Select(o => long.Parse(o.Id)).ToList();
                                BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(prom.BlendDetails.Where(o => ids.Contains(o.Id)).ToList(), false);
                                prom.BlendDetails.RemoveAll(o => ids.Contains(o.Id));
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(obj.DeleteTypeed))
                        {
                            var dels = obj.DeleteTypeed.ToObject<List<TransObject>>();
                            if (dels.Any())
                            {
                                var ids = dels.Select(o => int.Parse(o.Id)).ToList();
                                var dts = prom.BlendDetails.Where(o => ids.Contains(o.Id) && o.CommodityId == obj.Id).ToList();
                                BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(dts, false);
                                prom.BlendDetails.RemoveAll(o => ids.Contains(o.Id) && o.CommodityId == obj.Id);
                            }
                        }
                        BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(prom.BlendDetails.Where(o => o.Id > 0 && o.BlendType == 3).ToList(), false);
                    }
                    else
                    {
                        BaseService<PromotionBlendList>.CurrentRepository.RemoveRange(prom.BlendDetails.Where(o => o.BlendType == 3 || o.BlendType == 4).ToList(), false);
                    }
                    blend.PromotionType = obj.PromotionType2;
                    re = Update(prom);
                }
                if (re.Successed)
                {
                    Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = Sys.SysCommonRules.CompanyId, StoreId = obj.StoreId, Target = "CommodityBlendPackage" });
                }
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                Log.WriteError(ex);
            }
            return re;
        }
        #endregion
        #region 买赠销售
        /// <summary>
        /// 买赠销售列表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static object MaiZengPageList(NameValueCollection nvl, out int recordCount, bool isFilterStore = false)
        {
            var state = nvl["state"].IsNullOrEmpty() ? -1 : short.Parse(nvl["state"]);
            var searchText = nvl["searchText"].Trim();
            var exp = DynamicallyLinqHelper.Empty<CommodityPromotion>().And(o => o.State == state, state == -1).And(o => o.CompanyId == CommonService.CompanyId);
            if (isFilterStore)
            {
                exp = exp.And(o => (o.StoreId.Contains("-1") || ("," + o.StoreId + ",").Contains("," + Sys.SysCommonRules.CurrentStore + ",")));
            }
            var queryFree = BaseService<FreeGiftPurchase>.CurrentRepository.QueryEntity;
            var queryFrees = BaseService<FreeGiftPurchaseList>.CurrentRepository.QueryEntity;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryCategory = BaseService<ProductCategory>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryPromotion = CurrentRepository.QueryEntity.Where(exp);
            var query = from x in queryPromotion
                        from y in queryFree
                        where x.Id == y.CommodityId
                        select new
                        {
                            x.Id,
                            x.StartDate,
                            x.CreateDT,
                            x.EndDate,
                            x.CustomerObj,
                            x.State,
                            x.Timeliness,
                            y.RestrictionBuyNum,
                            y.MinPurchaseNum,
                            y.GiftId,
                            y.GiftType,
                            y.BarcodeOrCategorySN,
                            x.StoreId,
                            x.CompanyId
                        };
            if (!Sys.CurrentUser.StoreId.IsNullOrEmpty())
                query = query.Where(o => ("," + o.StoreId + ",").Contains("," + Sys.CurrentUser.StoreId + ",") || o.StoreId.Contains("-1"));
            if (!searchText.IsNullOrEmpty())
            {
                query = from x in query
                        let o = from h in queryProduct
                                where (x.BarcodeOrCategorySN == h.Barcode || ("," + h.Barcodes + ",").Contains("" + x.BarcodeOrCategorySN + ",")) &&
                                    (h.Barcode.Contains(searchText) || h.Title.Contains(searchText) || ("," + h.Barcodes + ",").Contains("," + x.BarcodeOrCategorySN + ","))
                                select h
                        where o.Any()
                        select x;
            }
            recordCount = query.Count();
            var nl = new NameValueCollection() { nvl };
            nl["sort"] = "CreateDT";
            var list = query.ToPageList(nl);
            if (!list.Any()) return list;
            var giftIds = list.Select(o => o.GiftId).Distinct().ToList();
            var barcodes = list.Where(o => o.GiftType == 1).Select(o => o.BarcodeOrCategorySN).ToList();
            var fress = queryFrees.Where(o => giftIds.Contains(o.GiftId)).ToList();
            barcodes.AddRange(fress.Where(o => o.GiftType == 1).Select(o => o.BarcodeOrCategorySN));
            barcodes = barcodes.Distinct().ToList();
            var products = queryProduct.Where(o => barcodes.Contains(o.Barcode)).ToList();
            //var categorys = queryCategory.Where(o => categoryIds.Contains(o.CategorySN)).ToList();
            return list.Select(o => new
            {
                o.Id,
                o.CreateDT,
                o.State,
                BetWeen = DateDesc(o.StartDate, o.EndDate, o.Timeliness),
                Customer = Enum.GetName(typeof(CustomerType), o.CustomerObj),
                StateTitle = Enum.GetName(typeof(SaleState), o.State),
                o.RestrictionBuyNum,
                o.MinPurchaseNum,
                o.GiftType,
                PromotionRange = Detail(o.GiftType, o.BarcodeOrCategorySN, products),
                ProductGive = Detail(o.GiftId, fress, products),
                o.CompanyId,
            }).OrderByDescending(o => o.CreateDT).ThenBy(o => o.GiftType);
        }
        /// <summary>
        /// 保存或修改
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="times">时间段</param>
        /// <returns></returns>
        public static OpResult MaiZengSaveOrUpdate(CommodityPromotion obj, string times)
        {
            var re = new OpResult();
            try
            {
                if (!times.IsNullOrEmpty())
                {
                    var ts = times.Split(',');
                    obj.StartAging1 = ts[0];
                    obj.EndAging1 = ts[1];
                    obj.StartAging2 = ts[2];
                    obj.EndAging2 = ts[3];
                    obj.StartAging3 = ts[4];
                    obj.EndAging3 = ts[5];
                }
                var frees = new List<FreeGiftPurchase>();
                var compid = CommonService.CompanyId;
                if (!obj.Inserted.IsNullOrEmpty())
                {
                    var adds = obj.Inserted.ToObject<List<FreeGiftPurchase>>();
                    if (adds.Any())
                    {
                        var add = adds.Where(o => !string.IsNullOrWhiteSpace(o.Barcode));
                        add.Each(o => { o.GiftType = 1; o.BarcodeOrCategorySN = o.Barcode; o.CompanyId = compid; });
                        frees.AddRange(add);
                    }
                }
                if (!obj.InsertTypeed.IsNullOrEmpty())
                {
                    var adds = obj.InsertTypeed.ToObject<List<FreeGiftPurchase>>();
                    if (adds.Any())
                    {
                        var add = adds.Where(o => !string.IsNullOrWhiteSpace(o.CategorySN));
                        add.Each(o => { o.GiftType = 2; o.BarcodeOrCategorySN = o.CategorySN; o.CompanyId = compid; });
                        frees.AddRange(add);
                    }
                }
                if (frees.Where(o => o.GiftType == 1).Distinct(new FreeGiftCompare()).Count() != frees.Where(o => o.GiftType == 1).Count())
                {
                    re.Message = "存在重复商品和起购量"; return re;
                }
                if (!obj.Id.IsNullOrEmpty() && obj.State == 2)//过期则新增
                {
                    var res = CurrentRepository.QueryEntity.Include(o => o.FreeGiftPurchases).FirstOrDefault(o => o.Id == obj.Id);
                    var gids = res.FreeGiftPurchases.Select(o => o.GiftId).ToList();
                    var gifts = BaseService<FreeGiftPurchaseList>.FindList(o => gids.Contains(o.GiftId));
                    foreach (var dis in res.FreeGiftPurchases)
                    {
                        var todis = new FreeGiftPurchase();
                        todis.Barcodes = string.Join(",", gifts.Where(o => o.GiftId == dis.GiftId).Select(o => o.BarcodeOrCategorySN+":"+o.GiftNumber));
                        todis.Barcode = todis.BarcodeOrCategorySN = dis.BarcodeOrCategorySN;
                        todis.GiftType = dis.GiftType;
                        todis.MinPurchaseNum = dis.MinPurchaseNum;
                        todis.RestrictionBuyNum = dis.RestrictionBuyNum;
                        todis.CategoryGrade = dis.CategoryGrade;
                        todis.CompanyId = compid;
                        frees.Add(todis);
                    }
                    if (!string.IsNullOrWhiteSpace(obj.Updated))
                    {
                        var ups = obj.Updated.ToObject<List<FreeGiftPurchase>>();
                        ups.Each(o =>
                        {
                            var dis = frees.FirstOrDefault(i => i.Barcode == o.Barcode && o.GiftType == 1);
                            if (dis != null)
                            {
                                o.ToCopyProperty(dis);
                                dis.CompanyId = compid;
                            }
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(obj.UpdateTyped))
                    {
                        var ups = obj.UpdateTyped.ToObject<List<FreeGiftPurchase>>();
                        ups.Each(o =>
                        {
                            var dis = frees.FirstOrDefault(i => i.BarcodeOrCategorySN == o.CategorySN && i.BrandSN == o.BrandSN && o.GiftType == 2);
                            if (dis != null)
                            {
                                o.ToCopyProperty(dis);
                                dis.CompanyId = compid;
                            }
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(obj.Deleted))
                    {
                        var dels = obj.Deleted.ToObject<List<FreeGiftPurchase>>();
                        if (dels.Any())
                        {
                            var barcodes = dels.Select(o => o.Barcode);
                            frees.RemoveAll(o => barcodes.Contains(o.Barcode) && o.GiftType == 1);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(obj.DeleteTyped))
                    {
                        var dels = obj.DeleteTyped.ToObject<List<FreeGiftPurchase>>();
                        if (dels.Any())
                        {
                            dels.Each(o =>
                            {
                                frees.RemoveAll(i => i.BarcodeOrCategorySN == o.CategorySN && i.BrandSN == o.BrandSN && o.GiftType == 2);
                            });
                        }
                    }
                    obj.Id = null;
                }
                foreach (var detail in frees)
                {
                    detail.GiftId = Pharos.Logic.CommonRules.GUID;
                    if (detail.Barcodes.IsNullOrEmpty()) continue;
                    var bs = detail.Barcodes.Split(',');
                    var freeDetails = new List<FreeGiftPurchaseList>();
                    foreach (var b in bs)
                    {
                        if (b.IsNullOrEmpty()) continue;
                        freeDetails.Add(new FreeGiftPurchaseList()
                        {
                            GiftNumber = b.Split(':')[1].ToType<short?>() ?? 1,
                            GiftType = 1,
                            GiftId = detail.GiftId,
                            BarcodeOrCategorySN = b.Split(':')[0]
                        });
                    }
                    BaseService<FreeGiftPurchaseList>.CurrentRepository.AddRange(freeDetails, false);
                }
                if (obj.Id.IsNullOrEmpty())
                {
                    var query = from x in CurrentRepository.Entities
                                join y in FreeGiftPurchaseService.CurrentRepository.Entities on x.Id equals y.CommodityId
                                where x.State != 2 && x.EndDate >= obj.StartDate
                                select new
                                {
                                    x.EndDate,
                                    x.State,
                                    y.BarcodeOrCategorySN,
                                    y.MinPurchaseNum
                                };
                    var barcodes = frees.Where(o => o.GiftType == 1).Select(o => o.Barcode).ToList();
                    var categorysn = frees.Where(o => o.GiftType == 2).Select(o => o.CategorySN).ToList();
                    var pros = query.Where(o => barcodes.Contains(o.BarcodeOrCategorySN)).ToList();
                    foreach (var fre in frees.Where(o => o.GiftType == 1))
                    {
                        if (pros.Any(o => o.BarcodeOrCategorySN == fre.Barcode && o.MinPurchaseNum == fre.MinPurchaseNum))
                        {
                            re.Message = "同个商品同个数量不能在同个时间段内促销";
                            return re;
                        }
                    }
                    var msg = _promotionDal.PromotionValidMsg("4", obj.StartDate.GetValueOrDefault(), barcodes, categorysn);
                    if (!msg.IsNullOrEmpty())
                    {
                        re.Message = msg;
                        return re;
                    }

                    obj.Id = Pharos.Logic.CommonRules.GUID;
                    obj.CreateDT = DateTime.Now;
                    obj.CreateUID = Sys.CurrentUser.UID;
                    obj.FreeGiftPurchases = frees;
                    obj.PromotionType = 4;
                    obj.State = (short)obj.SaleState;
                    obj.CompanyId = compid;
                    re = Add(obj);
                }
                else
                {
                    var prom = CurrentRepository.QueryEntity.Include(o => o.FreeGiftPurchases).FirstOrDefault(o => o.Id == obj.Id);
                    obj.ToCopyProperty(prom);
                    prom.CompanyId = CommonService.CompanyId;
                    prom.State = (short)obj.SaleState;
                    prom.FreeGiftPurchases.AddRange(frees);
                    if (!string.IsNullOrWhiteSpace(obj.Deleted))
                    {
                        var dels = obj.Deleted.ToObject<List<FreeGiftPurchase>>();
                        if (dels.Any())
                        {
                            var ids = dels.Select(o => o.Id).ToList();
                            BaseService<FreeGiftPurchase>.CurrentRepository.RemoveRange(prom.FreeGiftPurchases.Where(o => ids.Contains(o.Id)).ToList(), false);
                            prom.FreeGiftPurchases.RemoveAll(o => ids.Contains(o.Id));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(obj.DeleteTyped))
                    {
                        var dels = obj.DeleteTyped.ToObject<List<FreeGiftPurchase>>();
                        if (dels.Any())
                        {
                            var ids = dels.Select(o => o.Id).ToList();
                            BaseService<FreeGiftPurchase>.CurrentRepository.RemoveRange(prom.FreeGiftPurchases.Where(o => ids.Contains(o.Id)).ToList(), false);
                            prom.FreeGiftPurchases.RemoveAll(o => ids.Contains(o.Id));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(obj.Updated))
                    {
                        var ups = obj.Updated.ToObject<List<FreeGiftPurchase>>();
                        ups.Each(o =>
                        {
                            var dis = prom.FreeGiftPurchases.FirstOrDefault(i => i.Id == o.Id);
                            if (dis != null)
                            {
                                o.ToCopyProperty(dis);
                                o.CompanyId = prom.CompanyId;
                            }
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(obj.UpdateTyped))
                    {
                        var ups = obj.UpdateTyped.ToObject<List<FreeGiftPurchase>>();
                        ups.Each(o =>
                        {
                            var dis = prom.FreeGiftPurchases.FirstOrDefault(i => i.Id == o.Id);
                            if (dis != null)
                            {
                                o.ToCopyProperty(dis);
                                o.CompanyId = prom.CompanyId;
                            }
                        });
                    }
                    re = Update(prom);
                }
                if (re.Successed)
                {
                    Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = Sys.SysCommonRules.CompanyId, StoreId = obj.StoreId, Target = "CommodityFreeGiftPackage" });
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
        /// 用于修改回显列表数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static object FindManZengDetailsById(string id, short type, out int recordCount)
        {
            recordCount = 0;
            if (id.IsNullOrEmpty()) return null;
            var queryDetail = BaseService<FreeGiftPurchaseList>.CurrentRepository.QueryEntity;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            if (type == 1)
            {
                var queryFree = BaseService<FreeGiftPurchase>.CurrentRepository.QueryEntity;
                var query = from x in queryFree
                            from y in queryProduct
                            where (x.BarcodeOrCategorySN == y.Barcode || ("," + y.Barcodes + ",").Contains("," + x.BarcodeOrCategorySN + ",")) && x.GiftType == 1 && x.CommodityId == id
                            select new
                            {
                                x.Id,
                                x.MinPurchaseNum,
                                x.RestrictionBuyNum,
                                x.GiftId,
                                x.GiftType,
                                x.BrandSN,
                                x.CategoryGrade,
                                y
                            };
                var list = query.ToList();
                recordCount = list.Count;
                var gitIds = list.Select(o => o.GiftId).ToList();
                var details = queryDetail.Where(o => gitIds.Contains(o.GiftId)).ToList();
                var barcodes = details.Where(o => o.GiftType == 1).Select(o => o.BarcodeOrCategorySN).Distinct().ToList();
                var products = queryProduct.Where(o => barcodes.Contains(o.Barcode)).ToList();
                return list.Select(o => new
                {
                    o.Id,
                    o.MinPurchaseNum,
                    o.RestrictionBuyNum,
                    o.GiftId,
                    o.y.Barcode,
                    o.y.ProductCode,
                    o.y.Title,
                    o.BrandSN,
                    o.y.SysPrice,
                    o.y.SubUnitId,
                    o.GiftType,
                    StockNums = o.y.AcceptNums - o.y.PurchaseNumbers,
                    o.y.CategoryTitle,
                    o.y.SubUnit,
                    o.CategoryGrade,
                    Detail = Detail(o.GiftId, details, products),
                    Barcodes = Barcodes(details)
                });
            }
            else
            {
                var dal = new Pharos.Logic.DAL.BlendListDAL();
                var dt = dal.GetFreeGiftCategoryList(id);
                var gitIds = dt.AsEnumerable().Select(o => o["GiftId"].ToString()).ToList();
                var details = queryDetail.Where(o => gitIds.Contains(o.GiftId)).ToList();
                var barcodes = details.Where(o => o.GiftType == 1).Select(o => o.BarcodeOrCategorySN).Distinct().ToList();
                var products = queryProduct.Where(o => barcodes.Contains(o.Barcode)).ToList();
                var list = dt.AsEnumerable().Select(dr => new
                {
                    Id = dr["Id"],
                    GiftId = dr["GiftId"],
                    StrId = dr["CategorySN"] + "~" + dr["BrandSN"],
                    CategorySN = dr["CategorySN"],
                    SubCategoryTitle = dr["SubCategoryTitle"],
                    MidCategoryTitle = dr["MidCategoryTitle"],
                    BigCategoryTitle = dr["BigCategoryTitle"],
                    BrandSN = dr["BrandSN"],
                    BrandTitle = dr["BrandTitle"],
                    StockNums = dr["StockNums"],
                    MinPurchaseNum = dr["MinPurchaseNum"],
                    RestrictionBuyNum = dr["RestrictionBuyNum"],
                    Detail = Detail(dr["GiftId"].ToString(), details, products),
                    Barcodes = Barcodes(details),
                    GiftType = dr["GiftType"],
                    CategoryGrade = dr["CategoryGrade"]
                }).ToList();
                recordCount = list.Count;
                return list;
            }
        }
        #endregion
        #region 辅助方法
        static string GenerateBundlingBar(string prefix)
        {
            //var maxCode = BaseService<Bundling>.CurrentRepository.QueryEntity.Where(o=>o.NewBarcode.StartsWith(prefix)).Max(o => o.NewBarcode);
            //int code = 0;
            //if (!maxCode.IsNullOrEmpty())
            //    code = int.Parse(maxCode.Substring(maxCode.Length - 5));
            //return prefix+(code + 1).ToString("00000");
            return _promotionDal.GetNewBarcode();
        }
        static string CommonDetail(PromotionBlend blend, List<PromotionBlendList> blends, List<VwProduct> products, List<ProductCategory> categorys)
        {
            var str = "";
            if (blend.PromotionType == 4)
            {
                var barcodes = blends.Where(o => o.CommodityId == blend.CommodityId && o.BlendType == 3).Select(o => o.BarcodeOrCategorySN).ToList();
                var categorySNs = blends.Where(o => o.CommodityId == blend.CommodityId && o.BlendType == 4).Select(o => int.Parse(o.BarcodeOrCategorySN)).ToList();
                products = products.Where(o => barcodes.Contains(o.Barcode)).ToList();
                categorys = categorys.Where(o => categorySNs.Contains(o.CategorySN)).ToList();
                if (products.Any() || categorys.Any())
                {
                    str = ""; //"<div style='color:orange'>" + (products.Count > 1 || categorys.Any() ? "随机" : "") + "赠1件</div>";
                    products.Each(o =>
                    {
                        str += o.Barcode + " " + o.BrandTitle + " " + o.Title +
                            "(赠" + (blends.Where(i => i.CommodityId == blend.CommodityId && i.BarcodeOrCategorySN == o.Barcode).Select(i => i.Number).FirstOrDefault() ?? 1) + "件)<br>";
                    });
                    categorys.Each(o =>
                    {
                        str += o.Title + "系列<br>";
                    });
                }
            }
            else if (blend.PromotionType == 5)
            {
                str = "<div style='color:orange'>随机赠" + blend.PriceRange.ToString("f2") + "元以下商品1件</div>";
            }
            return str;

        }
        static string Detail(PromotionBlend blend, List<PromotionBlendList> blends, List<VwProduct> products, List<ProductCategory> categorys, bool manyuan)
        {
            string str = "";
            var barcodes = blends.Where(o => o.CommodityId == blend.CommodityId && o.BlendType == 1).Select(o => o.BarcodeOrCategorySN).ToList();
            var categorySNs = blends.Where(o => o.CommodityId == blend.CommodityId && o.BlendType == 2).Select(o => int.Parse(o.BarcodeOrCategorySN)).ToList();
            if (manyuan)
            {
                barcodes = blends.Where(o => o.CommodityId == blend.CommodityId && o.BlendType == 5).Select(o => o.BarcodeOrCategorySN).ToList();
                categorySNs = blends.Where(o => o.CommodityId == blend.CommodityId && o.BlendType == 6).Select(o => int.Parse(o.BarcodeOrCategorySN)).ToList();
            }
            products = products.Where(o => barcodes.Contains(o.Barcode)).ToList();
            categorys = categorys.Where(o => categorySNs.Contains(o.CategorySN)).ToList();
            if (products.Any() || categorys.Any())
            {
                products.Each(o =>
                {
                    str += o.Barcode + " " + o.BrandTitle + " " + o.Title + "<br>";
                });
                categorys.Each(o =>
                {
                    str += o.Title + "系列<br>";
                });
            }
            return str;
        }
        static string Detail(string giftId, List<FreeGiftPurchaseList> frees, List<VwProduct> details)
        {
            var str = "";
            var barcodes = frees.Where(o => o.GiftId == giftId && o.GiftType == 1).Select(o => o.BarcodeOrCategorySN).ToList();
            details = details.Where(o => barcodes.Contains(o.Barcode)).ToList();
            if (details.Any())
            {
                str = "";// "<div style='color:orange'>" + (details.Count == 1 ? "" : "随机") + "赠1件</div>";
                details.Each(o =>
                {
                    str += o.Barcode + " " + o.BrandTitle + " " + o.Title +
                        "(赠" + frees.Where(i => i.GiftId == giftId && i.BarcodeOrCategorySN == o.Barcode).Select(i => i.GiftNumber).FirstOrDefault() + "件)<br>";
                });
            }
            return str;

        }
        static string Barcodes(List<FreeGiftPurchaseList> frees)
        {
            var str = "";
            frees.Each(o =>
            {
                str += o.BarcodeOrCategorySN + ":" + o.GiftNumber + ",";
            });
            return str.TrimEnd(',');

        }
        static string Detail(short type, string barcodeOrCategorySN, List<VwProduct> details)
        {
            var str = "";
            if (type == 1)
            {
                var product = details.FirstOrDefault(o => o.Barcode == barcodeOrCategorySN);
                if (product != null)
                    str = product.Barcode + " " + product.BrandTitle + " " + product.Title;
            }
            else if (type == 2)
            {
                var item = ProductCategoryService.GetTitleDesc(int.Parse(barcodeOrCategorySN));
                if (item != null)
                    str = item.Text;
            }
            return str;

        }
        static string SellWay(PromotionBlend blend)
        {
            var desc = "";
            if (blend.RuleType == 1)
                desc = "满" + blend.FullNumber.ToString("f0") + "件,";
            else if (blend.RuleType == 2)
                desc = "满" + blend.FullNumber.ToString("f2") + "元,";
            if (blend.PromotionType == 1)
                desc += "减" + blend.DiscountOrPrice.ToString("f2") + "元 ";
            else if (blend.PromotionType == 2)
                desc += "减" + blend.DiscountOrPrice.ToString("f2") + "元 代金券";
            else if (blend.PromotionType == 3)
                desc += "打" + blend.DiscountOrPrice.ToString("f1") + "折";
            else if (blend.PromotionType == 4)
                desc += blend.DiscountOrPrice == 0 ? "送(指定)赠品" : "加" + blend.DiscountOrPrice.ToString("f2") + "元送(指定)赠品";
            else if (blend.PromotionType == 5)
                desc += "加" + blend.DiscountOrPrice.ToString("f2") + "元送(" + blend.PriceRange.ToString("f2") + "元以下)赠品";
            return desc;
        }
        static string DateDesc(DateTime? start, DateTime? end, short timeLines)
        {
            var str = (start.HasValue && end.HasValue) ? start.Value.ToString("yyyy-MM-dd") + "至" + end.Value.ToString("yyyy-MM-dd") + "<br/>(" + (end.Value.Subtract(start.Value).TotalDays + 1) + "天{0})" : "";
            str = string.Format(str, timeLines == 1 ? ",按时段" : "");
            return str;
        }
        static string DateDesc(DateTime? start, DateTime? end)
        {
            return start.GetValueOrDefault().ToString("yyyy-MM-dd") + "至" + end.GetValueOrDefault().ToString("yyyy-MM-dd");
        }
        class Eqcomp : IEqualityComparer<CommodityDiscount>
        {

            public bool Equals(CommodityDiscount x, CommodityDiscount y)
            {
                return x.Barcode == y.Barcode && x.MinPurchaseNum == y.MinPurchaseNum;
            }

            public int GetHashCode(CommodityDiscount obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
        class TransObject
        {
            public string Id { get; set; }
            public string Barcode { get; set; }
            public int? CategorySN { get; set; }
            public int? BrandSN { get; set; }
            public short? CategoryGrade { get; set; }
        }
        class BundingTrans
        {
            public Int64 Id { get; set; }
            public string Barcode { get; set; }
            public int Number { get; set; }
        }
        class FreeGiftCompare : IEqualityComparer<FreeGiftPurchase>
        {

            public bool Equals(FreeGiftPurchase x, FreeGiftPurchase y)
            {
                return x.Barcode == y.Barcode && x.MinPurchaseNum == y.MinPurchaseNum;
            }

            public int GetHashCode(FreeGiftPurchase obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
        #endregion

        #region 获取同步数据
        public static IEnumerable<string> GetEffectiveId(string storeId)
        {
            var date = DateTime.Now.Date;
            return CurrentRepository.FindList(o => o.EndDate >= date && o.State != 2 && ("," + o.StoreId + ",").Contains("," + storeId + ",")).Select(o => o.Id).ToList();
        }
        #endregion
        #region 获取最新活动信息
        /// <summary>
        /// 获得最新的活动
        /// </summary>
        /// <param name="takeNum">需要取出的条数</param>
        /// <returns></returns>
        public static List<CommodityPromotion> GetNewestActivity(int takeNum)
        {
            var query = CurrentRepository.QueryEntity.Where(c => c.CompanyId == CommonService.CompanyId && (c.State == 0 || c.State == 1));
            if (!Sys.CurrentUser.StoreId.IsNullOrEmpty())
                query = query.Where(o => ("," + o.StoreId + ",").Contains("," + Sys.CurrentUser.StoreId + ","));
            var i = query.Count();
            return query.OrderByDescending(c => c.StartDate).Take(takeNum).ToList();
        }
        #endregion
    }
}
