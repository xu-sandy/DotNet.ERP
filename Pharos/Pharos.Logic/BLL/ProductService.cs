﻿using Pharos.Logic.Entity;
using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.DAL;
using System.IO;
using Pharos.Sys.Entity;
using System.Web;
using Pharos.Logic.WeighDevice;
using Pharos.Sys;
using Pharos.Logic.ApiData.Mobile.Exceptions;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.WeighDevice.ScaleEntity;
using System.Text.RegularExpressions;
namespace Pharos.Logic.BLL
{
    public class ProductService : BaseService<ProductRecord>
    {
        /// <summary>
        /// 用于POS端启动加速
        /// </summary>
        public static void InitForStart()
        {
            CurrentRepository.IsExist(o => o.ProductCode == "");
        }
        /// <summary>
        /// 用于datagrid列表
        /// </summary>
        /// <param name="nvl">传递条件</param>
        /// <param name="recordCount">返回总行数</param>
        /// <returns>list</returns>
        public static List<VwProduct> LoadProductList(NameValueCollection nvl, out int recordCount, bool isPage = true)
        {
            var supplierId = nvl["supplierId"];//供应商
            var childType = nvl["childType"];//子类
            var category = nvl["parentType"];//选中分类
            var state = nvl["state"];//状态
            var searchText = nvl["searchText"];//查找文本
            var startDate = nvl["startDate"];//
            var endDate = nvl["endDate"];
            var customer = nvl["customer"];//客户类型
            var store = nvl["store"];//门店
            var isExclude = nvl["isExclude"];//排除列表中已有的条码
            var barcodes = nvl["barcodes"];
            var promtype = nvl["promtype"];
            var order = nvl["order"];
            var zp = nvl["zp"];
            var brandsn = nvl["brandsn"];
            var valuationType = nvl["ValuationType"];
            var nature = nvl["nature"];
            var query = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            if (!supplierId.IsNullOrEmpty())
            {
                var sp = supplierId.Split(',').ToList();
                var bars = BaseService<ProductMultSupplier>.FindList(o => sp.Contains(o.SupplierId) && o.CompanyId == CommonService.CompanyId).Select(o => o.Barcode).Distinct().ToList();
                query = query.Where(o => (sp.Contains(o.SupplierId) || bars.Contains(o.Barcode)));
            }
            //if (!parentType.IsNullOrEmpty())
            //{
            //    var pt = int.Parse(parentType);
            //    query = query.Where(o => o.BigCategorySN == pt);
            //}
            //if (!childType.IsNullOrEmpty())
            //{
            //    var ct = int.Parse(childType);
            //    query = query.Where(o => o.SubCategorySN == ct);
            //}

            if (!category.IsNullOrEmpty())
            {
                var bigs = category.Split(',').Select(o => int.Parse(o)).ToList();
                var childs = ProductCategoryService.GetChildSNs(bigs, true);
                query = query.Where(o => childs.Contains(o.CategorySN));
            }
            if (!state.IsNullOrEmpty())
            {
                var ct = short.Parse(state);
                query = query.Where(o => o.State == ct);
            }
            if (!searchText.IsNullOrEmpty())
            {
                searchText = searchText.Trim();
                query = query.Where(o => o.ProductCode.Contains(searchText) || o.Barcode.Contains(searchText) || o.Title.Contains(searchText) || ("," + o.Barcodes + ",").Contains("," + searchText + ","));
            }
            if (!barcodes.IsNullOrEmpty() && isExclude == "1")
            {
                var bas = barcodes.Split(',');
                query = query.Where(o => !bas.Contains(o.Barcode));
            }
            if (!store.IsNullOrEmpty())
            {
                var ware = WarehouseService.Find(o => o.CompanyId == CommonService.CompanyId && o.StoreId == store);
                if (ware != null)
                {
                    var categorySNs = ware.CategorySN.Split(',').Select(o => int.Parse(o)).ToList();
                    var childs = ProductCategoryService.GetChildSNs(categorySNs, true);
                    query = query.Where(o => childs.Contains(o.CategorySN));
                }
            }
            if (order == "1")
                query = query.Where(o => o.IsAcceptOrder == 1 && o.Nature != 2);
            if (!promtype.IsNullOrEmpty())//限制所有促销
                query = query.Where(o => o.ValuationType == 1);
            else if (!valuationType.IsNullOrEmpty())
            {
                var vt = short.Parse(valuationType);
                query = query.Where(o => o.ValuationType == vt);
            }
            if (!brandsn.IsNullOrEmpty())
            {
                var sn = brandsn.Split(',').Select(o => o.IsNullOrEmpty() ? new Nullable<int>() : int.Parse(o)).ToList();
                query = query.Where(o => sn.Contains(o.BrandSN));
            }
            if (!startDate.IsNullOrEmpty() && !endDate.IsNullOrEmpty() && !customer.IsNullOrEmpty() && !store.IsNullOrEmpty())//促销过滤条件
            {
                var start = DateTime.Parse(startDate);
                var end = DateTime.Parse(endDate);
                var cus = short.Parse(customer);
                var stores = store.TrimEnd(',');
                var express = DynamicallyLinqHelper.Empty<CommodityPromotion>().And(o => o.StartDate >= start && o.StartDate <= end, startDate.IsNullOrEmpty())
                    .And(o => o.EndDate >= start && o.EndDate <= end, endDate.IsNullOrEmpty()).And(o => o.StartDate <= start && o.EndDate >= end)
                    .And(o => o.CustomerObj == cus, customer.IsNullOrEmpty());

                var queryPromot = BaseService<CommodityPromotion>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
                queryPromot = queryPromot.Where(o => ((o.StartDate >= start && o.StartDate <= end) || (o.EndDate >= start && o.EndDate <= end)
                    || (o.StartDate <= start && o.EndDate >= end)) && o.CustomerObj == cus && ("," + o.StoreId + ",").Contains("," + stores + ","));
                if (promtype == "1")//单品
                {
                    var queryDiscount = BaseService<CommodityDiscount>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
                    query = from q in query
                            where !(from x in queryPromot
                                    from y in queryDiscount
                                    where x.Id == y.CommodityId
                                    select y.Barcode).Contains(q.Barcode)
                            select q;
                }
                else if (promtype == "2")//捆绑
                {
                    var queryBundings = BaseService<BundlingList>.CurrentRepository.QueryEntity;
                    query = from q in query
                            where !(from x in queryPromot
                                    from y in queryBundings
                                    where x.Id == y.CommodityId
                                    select y.Barcode).Contains(q.Barcode)
                            select q;
                }
                else if (promtype == "4")//买赠
                {
                    var queryFrees = BaseService<FreeGiftPurchase>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
                    query = from q in query
                            where !(from x in queryPromot
                                    from y in queryFrees
                                    where x.Id == y.CommodityId
                                    select y.BarcodeOrCategorySN).Contains(q.Barcode)
                            select q;
                }
            }
            if (promtype == "4")
                query = query.Where(o => o.Nature == 0);
            if (!nature.IsNullOrEmpty())
            {
                var nt = short.Parse(nature);
                query = query.Where(o => o.Nature == nt);
            }
            recordCount = query.Count();
            var list = isPage ? query.ToPageList() : query.ToList();
            SetSysPrice<VwProduct>(store, list, supplierId: supplierId);
            return list;
        }
        /// <summary>
        /// 弹框选择类别
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static object FindTypePageList(NameValueCollection nvl, out int count)
        {
            var query = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var bigCate = nvl["BigCategorySN"].IsNullOrEmpty() ? -1 : int.Parse(nvl["BigCategorySN"]);
            var subCate = nvl["SubCategorySN"].IsNullOrEmpty() ? -1 : int.Parse(nvl["SubCategorySN"]);
            var brandSn = nvl["BrandSN"].IsNullOrEmpty() ? -1 : int.Parse(nvl["BrandSN"]);
            var supplierId = nvl["supplierId"];
            int? big = null, mid = null, sub = null;
            //CommonService.GetCategory(nvl["parentType_0"], ref big, ref mid, ref sub);
            var category = nvl["parentType"].IsNullOrEmpty() ? -1 : int.Parse(nvl["parentType"]);
            var express = DynamicallyLinqHelper.Empty<VwProduct>()
                .And(o => o.BrandSN == brandSn, brandSn == -1);
            query = query.Where(express);
            if (!supplierId.IsNullOrEmpty())
            {
                var sp = supplierId.Split(',').ToList();
                var bars = BaseService<ProductMultSupplier>.FindList(o => sp.Contains(o.SupplierId)).Select(o => o.Barcode).Distinct().ToList();
                query = query.Where(o => (sp.Contains(o.SupplierId) || bars.Contains(o.Barcode)));
            }
            if (category != -1)
            {
                var childs = ProductCategoryService.GetChildSNs(new List<int>() { category }, true);
                query = query.Where(o => childs.Contains(o.CategorySN));
            }
            var q = from x in query
                    group x by new { x.CategorySN, x.MidCategoryTitle, x.SubCategoryTitle, x.BigCategoryTitle } into g
                    //where g.Key.MidCategoryTitle != null && g.Key.MidCategoryTitle.Length > 0
                    select new
                    {
                        g.Key.BigCategoryTitle,
                        g.Key.MidCategoryTitle,
                        g.Key.SubCategoryTitle,
                        g.Key.CategorySN,
                        BrandSN = 0,
                        BrandTitle = "",
                        AcceptNums = g.Sum(o => o.AcceptNums),
                        PurchaseNumbers = g.Sum(o => o.PurchaseNumbers),
                    };
            var list = q.ToList();
            count = list.Count;
            return list.Select(o => new
            {
                Id = o.CategorySN + "~" + o.BrandSN,
                o.BigCategoryTitle,
                o.MidCategoryTitle,
                o.SubCategoryTitle,
                o.BrandTitle,
                o.BrandSN,
                o.CategorySN,
                StockNums = o.AcceptNums - o.PurchaseNumbers,
                CategoryGrade = !o.SubCategoryTitle.IsNullOrEmpty() ? 3 : !o.MidCategoryTitle.IsNullOrEmpty() ? 2 : 1
            });
        }
        public static object FindTypePageList2(NameValueCollection nvl, out int count)
        {
            var query = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var bigCate = nvl["BigCategorySN"].IsNullOrEmpty() ? -1 : int.Parse(nvl["BigCategorySN"]);
            var subCate = nvl["SubCategorySN"].IsNullOrEmpty() ? -1 : int.Parse(nvl["SubCategorySN"]);
            var supplierId = nvl["supplierId"];
            var category = nvl["parentType"].IsNullOrEmpty() ? -1 : int.Parse(nvl["parentType"]);
            if (!supplierId.IsNullOrEmpty())
            {
                var sp = supplierId.Split(',').ToList();
                var bars = BaseService<ProductMultSupplier>.FindList(o => sp.Contains(o.SupplierId)).Select(o => o.Barcode).Distinct().ToList();
                query = query.Where(o => (sp.Contains(o.SupplierId) || bars.Contains(o.Barcode)));
            }
            if (category != -1)
            {
                var childs = ProductCategoryService.GetChildSNs(new List<int>() { category }, true);
                query = query.Where(o => childs.Contains(o.CategorySN));
            }
            var q = from x in query
                    group x by new { x.CategorySN, x.MidCategoryTitle, x.SubCategoryTitle, x.BigCategoryTitle } into g
                    where g.Key.MidCategoryTitle != null && g.Key.MidCategoryTitle.Length > 0
                    select new
                    {
                        g.Key.BigCategoryTitle,
                        g.Key.MidCategoryTitle,
                        g.Key.SubCategoryTitle,
                        g.Key.CategorySN,
                        AcceptNums = g.Sum(o => o.AcceptNums),
                        PurchaseNumbers = g.Sum(o => o.PurchaseNumbers),
                    };
            count = q.Count();
            var list = q.ToPageList();
            return list.Select(o => new
            {
                Id = o.CategorySN,
                o.BigCategoryTitle,
                o.MidCategoryTitle,
                o.SubCategoryTitle,
                o.CategorySN,
                StockNums = o.AcceptNums - o.PurchaseNumbers
            });
        }
        public static ProductRecord FindProduct(string barcode)
        {
            return ProductService.Find(o => o.Barcode == barcode && o.CompanyId == CommonService.CompanyId);
        }
        public static OpResult Delete(int[] ids)
        {
            var list = FindList(o => ids.Contains(o.Id));
            var gps = new List<string>();
            var op = new OpResult();
            foreach (var pro in list)
            {
                if (IsRelationship(pro.Barcode))
                {
                    op.Message = "条码[" + pro.Barcode + "]存在业务关系不允许删除!";
                    return op;
                }
                else if (!pro.Barcodes.IsNullOrEmpty())
                {
                    foreach (var o in pro.Barcodes.Split(','))
                    {
                        if (IsRelationship(o))
                        {
                            op.Message = "条码[" + o + "]存在业务关系不允许删除!";
                            return op;
                        }
                    }
                }
                if (pro.Nature == 1)
                {
                    gps.Add(pro.Barcode);
                }
            }
            if (gps.Any())
            {
                var bars = BaseService<ProductGroup>.FindList(o => gps.Contains(o.Barcode)).Select(o => o.GroupBarcode).ToList();
                var brs = FindList(o => bars.Contains(o.Barcode));
                brs.Each(o => o.IsRelationship = false);
                Update(brs, false);
            }
            #region 操作日志
            foreach (var item in list)
            {
                var msg = Pharos.Sys.LogEngine.CompareModelToLog<ProductRecord>(Sys.LogModule.档案管理, null, item);
                new Pharos.Sys.LogEngine().WriteDelete(msg, Sys.LogModule.档案管理);
            }
            #endregion
            op = Delete(list);
            if (op.Successed)
            {
                var barcodes = list.Where(o => o.Nature == 1).Select(o => o.Barcode).ToList();
                if (barcodes.Any())
                {
                    var pgs = BaseService<ProductGroup>.FindList(o => barcodes.Contains(o.Barcode));
                    BaseService<ProductGroup>.Delete(pgs);
                }
                var stores = string.Join(",", WarehouseService.GetList().Select(o => o.StoreId));
                Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = CommonService.CompanyId, StoreId = stores, Target = "ProductRecord" });
            }
            return op;
        }
        public static List<ProductRecord> InitLocalProductInfo(string storeId)
        {
            var result = (from a in CurrentRepository.QueryEntity
                          join b in CommodityService.CurrentRepository.QueryEntity on a.Barcode equals b.Barcode
                          join c in SysDataDictService.CurrentRepository.QueryEntity on a.BigUnitId equals c.DicSN
                          join d in SysDataDictService.CurrentRepository.QueryEntity on a.SubUnitId equals d.DicSN
                          where b.StoreId == storeId && a.CompanyId == CommonService.CompanyId
                          select new { A = a, B = b, C = c, D = d }
                           ).ToList();
            return result.Select(o => o.A.InitForLocalDb(o.B.StockNumber, o.D.Title, o.C.Title)).ToList();
        }
        static object objLock = new object();
        public static int GetNatureMaxCode(string prefix, short nature)
        {
            lock (objLock)
            {
                var maxCode = CurrentRepository.QueryEntity.Where(o => o.Nature == nature && o.CompanyId == CommonService.CompanyId && o.Barcode.StartsWith(prefix)).Max(o => o.Barcode);
                int code = 0;
                if (!maxCode.IsNullOrEmpty())
                    code = int.Parse(maxCode.Substring(maxCode.Length - 5));
                return code + 1;
            }
        }
        public static List<ProductGroup> GetGroups(string barcode)
        {
            var query = from a in BaseService<VwProduct>.CurrentRepository.Entities
                        join b in BaseService<ProductGroup>.CurrentRepository.Entities on a.Barcode equals b.GroupBarcode
                        where b.Barcode == barcode && b.CompanyId == a.CompanyId && a.CompanyId == CommonService.CompanyId
                        select new
                        {
                            a.Title,
                            b.Id,
                            b.Number,
                            b.GroupBarcode,
                            b.Barcode,
                            a.ValuationType,
                            a.SysPrice,
                            a.BuyPrice,
                            a.SubUnit
                        };
            var list = query.AsEnumerable().Select(o => new ProductGroup() { Barcode = o.Barcode, GroupBarcode = o.GroupBarcode, Id = o.Id, Number = o.Number, Title = o.Title, ValuationType = o.ValuationType, SysPrice = o.SysPrice, BuyPrice = o.BuyPrice, Unit = o.SubUnit }).ToList();
            return list;
        }
        static readonly ProductDAL productDal = new ProductDAL();
        public static OpResult SaveOrUpdate(ProductRecord obj)
        {
            var re = ValidCategory(obj.CategorySN);
            if (!re.Successed) return re;
            re.Successed = false;
            string newBar = null;
            if (!obj.Barcode.IsNullOrEmpty() && obj.Barcode.Length == 12 && obj.Barcode.StartsWith("04"))
            {
                re.Message = "该号段被定义为捆绑码，请确认条码是否有误！";
                return re;
            }
            if (!obj.Barcodes.IsNullOrEmpty())
            {
                obj.Barcodes = obj.Barcodes.Replace("，", ",").TrimEnd(',');
                var bars = obj.Barcodes.Split(',');
                foreach(var b in bars)
                {
                    if(b.Length==12 && b.StartsWith("04"))
                    {
                        re.Message = "该号段被定义为捆绑码，请确认条码是否有误！";
                        return re;
                    }
                }
                var list = ProductService.FindList(o => o.CompanyId == CommonService.CompanyId && bars.Contains(o.Barcode) && o.Id != obj.Id);
                if (list.Any())
                {
                    re.Message = "一品多码存在重复条码[" + (string.Join(",", list.Select(o => o.Barcode))) + "]";
                    re.Successed = false;
                    return re;
                }
                list = ProductService.FindList(o => o.CompanyId == CommonService.CompanyId && !(o.Barcodes == null || o.Barcodes == "") && o.Id != obj.Id);
                if (list.Any())
                {
                    foreach (var bar in bars)
                    {
                        if (bar.IsNullOrEmpty()) continue;
                        if (list.Any(o => ("," + o.Barcodes + ",").Contains("," + bar + ",")))
                        {
                            re.Message = "一品多码存在重复定义条码[" + bar + "]";
                            re.Successed = false;
                            return re;
                        }
                    }
                }
            }
            if (!obj.BarcodeMult.IsNullOrEmpty())
            {
                obj.BarcodeMult = obj.BarcodeMult.Replace("，", ",").TrimEnd(',');
                var bars = obj.BarcodeMult.Split(',');
                var list = ProductService.FindList(o => o.CompanyId == CommonService.CompanyId && bars.Contains(o.Barcode));
                string msg = "", msg2 = "";
                bars.Each(o =>
                {
                    var b = list.FirstOrDefault(i => i.Barcode == o);
                    if (b == null)
                        msg += "," + o;
                    else if (b.Nature != 0)
                        msg2 += "," + o;
                });
                if (!msg2.IsNullOrEmpty())
                {
                    re.Message = "多条码串存在非单品条码[" + msg2.Substring(1) + "]";
                    re.Successed = false;
                    return re;
                }
                if (!msg.IsNullOrEmpty())
                {
                    re.Message = "多条码串不存在条码[" + msg.Substring(1) + "]";
                    re.Successed = false;
                    return re;
                }
                if (list.GroupBy(o => o.CategorySN).Count() > 1 || !list.Any(o => o.CategorySN == obj.CategorySN))
                {
                    re.Message = "多条码串存在不同的分类";
                    re.Successed = false;
                    return re;
                }
                if (list.GroupBy(o => o.SysPrice).Count() > 1 || !list.Any(o => o.SysPrice == obj.SysPrice))
                {
                    re.Message = "多条码串存在不同的售价";
                    re.Successed = false;
                    return re;
                }
            }
            if ((!obj.BarcodeMult.IsNullOrEmpty() || !obj.Barcodes.IsNullOrEmpty()) && obj.Barcode.IsNullOrEmpty())
            {
                newBar = productDal.GenerateNewBarcode(CommonService.CompanyId, obj.CategorySN, obj.ValuationType);
                if (("," + obj.Barcodes + ",").Contains("," + newBar + ","))
                {
                    re.Message = "一品多码存在重复条码[" + newBar + "]";
                    re.Successed = false;
                    return re;
                }
            }
            var insertPrice = new List<ProductMultPrice>();
            var updatePrice = new List<ProductMultPrice>();
            var deletePrice = new List<ProductMultPrice>();
            var insertSupp = new List<ProductMultSupplier>();
            var updateSupp = new List<ProductMultSupplier>();
            var deleteSupp = new List<ProductMultSupplier>();
            var groups = new List<ProductGroup>();
            var changPriceLogs = new List<ChangePriceLog>();
            var batchno = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            if (!obj.PriceInserted.IsNullOrEmpty())
            {
                insertPrice = obj.PriceInserted.ToObject<List<ProductMultPrice>>().Where(o => !o.StoreId.IsNullOrEmpty()).ToList();
                insertPrice.Each(o =>
                {
                    o.Barcode = obj.Barcode;
                    o.CompanyId = CommonService.CompanyId;
                    changPriceLogs.Add(new ChangePriceLog()
                    {
                        BatchNo = batchno,
                        CreateDT = DateTime.Now,
                        CreateUID = Sys.CurrentUser.UID,
                        NewPrice = o.Price.GetValueOrDefault(),
                        OldPrice = obj.SysPrice,
                        OperType = 1,
                        StoreId = o.StoreId,
                        BusinessType = 5,
                        CompanyId = o.CompanyId
                    });
                });
            }
            if (!obj.PriceUpdateed.IsNullOrEmpty())
            {
                updatePrice = obj.PriceUpdateed.ToObject<List<ProductMultPrice>>();
            }
            if (!obj.PriceDeleteed.IsNullOrEmpty())
            {
                deletePrice = obj.PriceDeleteed.ToObject<List<ProductMultPrice>>();
            }
            if (!obj.SuppInserted.IsNullOrEmpty())
            {
                insertSupp = obj.SuppInserted.ToObject<List<ProductMultSupplier>>().Where(o => !o.SupplierId.IsNullOrEmpty()).ToList();
                insertSupp.Each(o => { o.Barcode = obj.Barcode; o.CompanyId = CommonService.CompanyId; });
            }
            if (!obj.SuppUpdateed.IsNullOrEmpty())
            {
                updateSupp = obj.SuppUpdateed.ToObject<List<ProductMultSupplier>>();
            }
            if (!obj.SuppDeleteed.IsNullOrEmpty())
            {
                deleteSupp = obj.SuppDeleteed.ToObject<List<ProductMultSupplier>>();
            }
            var groupBarcode = System.Web.HttpContext.Current.Request["GroupBarcode"];
            var groupNumber = System.Web.HttpContext.Current.Request["Number"];
            if (!groupBarcode.IsNullOrEmpty() && !groupNumber.IsNullOrEmpty())
            {
                var gps = groupBarcode.Split(',');
                var gpnums = groupNumber.Split(',');
                for (int i = 0; i < gps.Length; i++)
                {
                    groups.Add(new ProductGroup() { Barcode = obj.Barcode, GroupBarcode = gps[i], Number = decimal.Parse(gpnums[i]), CreateDT = DateTime.Now, CreateUID = Sys.CurrentUser.UID });
                }
                //if (groups.Count == 1)
                //{
                //    re.Message = "组合商品必需1件以上";
                //    re.Successed = false;
                //    return re;
                //}
            }
            obj.SupplierId = obj.SupplierId.IsNullOrEmpty() ? "-1" : obj.SupplierId;
            if (!obj.Barcode.IsNullOrEmpty() && ProductService.IsExist(o => o.CompanyId == CommonService.CompanyId && (o.Barcode == obj.Barcode || ("," + o.Barcodes + ",").Contains("," + obj.Barcode + ","))
                && o.Id != obj.Id))
                return OpResult.Fail("条形码已存在");

            #region 上传图像
            string path = "";
            int size = 200;
            var ps = Config.GetAppSettings("productimage");
            if (!ps.IsNullOrEmpty()) size = int.Parse(ps);
            string fullPath = FileHelper.SaveAttachPath(ref path, "productimgs");
            var images = new List<ProductImage>();
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                var file1 = HttpContext.Current.Request.Files[i];
                if (file1.ContentLength <= 0) continue;

                if (file1.ContentLength > size * 1024)
                {
                    return OpResult.Fail("图片不能大于" + size + "K!");
                }
                var extname = System.IO.Path.GetExtension(file1.FileName);
                if (string.Equals(extname, ".jpg", StringComparison.CurrentCultureIgnoreCase)
                    || string.Equals(extname, ".jpeg", StringComparison.CurrentCultureIgnoreCase)
                    || string.Equals(extname, ".gif", StringComparison.CurrentCultureIgnoreCase)
                    || string.Equals(extname, ".bmp", StringComparison.CurrentCultureIgnoreCase)
                    || string.Equals(extname, ".png", StringComparison.CurrentCultureIgnoreCase))
                {
                    string fileName = Guid.NewGuid().ToString("n") + extname;
                    string fulename = fullPath + fileName;
                    file1.SaveAs(fulename);
                    int w = 120, h = 120;
                    ps = Pharos.Utility.Config.GetAppSettings("productthumbnail");
                    if (!ps.IsNullOrEmpty())
                    {
                        var whs = ps.Split('*');
                        if (whs.Length == 2)
                        {
                            w = int.Parse(whs[0]);
                            h = int.Parse(whs[1]);
                        }
                        else
                            w = int.Parse(whs[0]);
                    }
                    Pharos.Utility.Helpers.ImageHelper.MakeThumbnail(fulename, fulename.Insert(fulename.LastIndexOf("."), "_s"), w, h, "W");
                    images.Add(new ProductImage()
                    {
                        ImagePath = path + fileName,
                        Status = obj.ProductImages[i].Status,
                        ProductCode = obj.ProductCode,
                        CompanyId = CommonService.CompanyId,
                        CreateDT = DateTime.Now,
                        CreateUID = CurrentUser.UID,
                        Id = obj.ProductImages[i].Id
                    });
                }
                else
                {
                    return OpResult.Fail("请选择图片格式!");
                }
            }
            #endregion
            if (obj.Id == 0)
            {
                obj.CompanyId = CommonService.CompanyId;
                if (obj.Nature == 1)
                {
                    var prefix = groups.Count().ToString("00") + obj.ProductCode2;
                    obj.Barcode = obj.Barcode.IsNullOrEmpty() ? prefix + ProductService.GetNatureMaxCode(prefix, 1).ToString("00000") : obj.Barcode;
                    groups.Each(o => { o.Barcode = obj.Barcode; o.CompanyId = obj.CompanyId; });
                    obj.IsAcceptOrder = obj.Favorable = obj.IsReturnSale = obj.ExpiryUnit = 1;
                    obj.Expiry = 0;
                    obj.ValuationType = 1;
                }
                else if (obj.Barcode.IsNullOrEmpty())
                {
                    obj.Barcode = newBar ?? productDal.GenerateNewBarcode(CommonService.CompanyId, obj.CategorySN, obj.ValuationType);
                }
                changPriceLogs.Each(o => { o.Barcode = obj.Barcode; o.CompanyId = obj.CompanyId; });
                insertPrice.Each(o => { o.Barcode = obj.Barcode; o.CompanyId = obj.CompanyId; });
                insertSupp.Each(o => { o.Barcode = obj.Barcode; o.CompanyId = obj.CompanyId; });
                obj.ProductCode = CommonRules.ProductCode;
                images.Each(o => o.ProductCode = obj.ProductCode);
                obj.State = 1;
                obj.RaterUID = Sys.CurrentUser.UID;
                obj.CreateDT = DateTime.Now;
                BaseService<ChangePriceLog>.AddRange(changPriceLogs, false);
                if (insertPrice.Any())
                    BaseService<ProductMultPrice>.AddRange(insertPrice, false);
                if (insertSupp.Any())
                    BaseService<ProductMultSupplier>.AddRange(insertSupp, false);
                if (groups.Any())
                    BaseService<ProductGroup>.AddRange(groups, false);
                BaseService<ProductImage>.AddRange(images, false);
                re = ProductService.Add(obj);

                var brandTitle = HttpContext.Current.Request["BrandTitle"];
                var categoryTitle = HttpContext.Current.Request["CategoryTitle"];
                var subUnit = HttpContext.Current.Request["SubUnit"];
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    var url = Config.GetAppSettings("omsurl");
                    if (!url.IsNullOrEmpty())
                    {
                        var pc = new ProductCenter() { Source = 2, Barcode = obj.Barcode, BrandTitle = brandTitle, CategoryTitle = categoryTitle, SubUnit = subUnit, Size = obj.Size, SysPrice = obj.SysPrice, Title = obj.Title, CompanyIds = obj.CompanyId.ToString(), Expiry = obj.Expiry, ExpiryUnit = obj.ExpiryUnit };
                        var commondal = new CommonDAL();
                        pc.BrandClassTitle = commondal.GetBrandClassTitle(obj.BrandSN, obj.CompanyId);
                        var r = HttpClient.HttpPost(url + "api/outerapi/PostProduct", (new List<ProductCenter>() { pc }).ToJson());
                    }
                });
            }
            else
            {
                var product = ProductService.FindById(obj.Id);
                var state = product.State;
                var uid = product.RaterUID;
                var oldSysPrice = product.SysPrice;
                var oldBuyPrice = product.BuyPrice;
                var oldTradePrice = product.TradePrice;
                var oldJoinPrice = product.JoinPrice;
                var valueType = product.ValuationType;
                obj.ToCopyProperty(product, new List<string>() { "CreateDT" });
                product.ValuationType = valueType;
                product.State = state;
                product.RaterUID = uid;
                product.CompanyId = CommonService.CompanyId;
                var prices = BaseService<ProductMultPrice>.FindList(o => o.CompanyId == CommonService.CompanyId && o.Barcode == product.Barcode);
                var suppls = BaseService<ProductMultSupplier>.FindList(o => o.CompanyId == CommonService.CompanyId && o.Barcode == product.Barcode);
                var refeshItems = new List<object>();
                if (insertPrice.Any())
                    BaseService<ProductMultPrice>.AddRange(insertPrice, false);
                if (insertSupp.Any())
                    BaseService<ProductMultSupplier>.AddRange(insertSupp, false);
                foreach (var up in updatePrice)
                {
                    var p = prices.FirstOrDefault(o => o.Barcode == up.Barcode && o.StoreId == up.StoreId);
                    if (p == null) continue;
                    changPriceLogs.Add(new ChangePriceLog()
                    {
                        Barcode = p.Barcode,
                        BatchNo = batchno,
                        CreateDT = DateTime.Now,
                        CreateUID = Sys.CurrentUser.UID,
                        NewPrice = up.Price.GetValueOrDefault(),
                        OldPrice = p.Price.GetValueOrDefault(),
                        OperType = 3,
                        StoreId = up.StoreId,
                        BusinessType = 5,
                        CompanyId = CommonService.CompanyId
                    });
                    p.Price = up.Price;
                    refeshItems.Add(new
                    {
                        StoreId = p.StoreId,
                        Barcode = product.Barcode,
                        ProductCode = product.ProductCode,
                        ProductType = product.Nature == 0 ? (product.ValuationType == 2 ? 1 : 0) : (product.Nature == 1 ? 4 : 3)
                    });
                }
                foreach (var up in deletePrice)
                {
                    var p = prices.FirstOrDefault(o => o.Barcode == up.Barcode && o.StoreId == up.StoreId);
                    if (p == null) continue;
                    changPriceLogs.Add(new ChangePriceLog()
                    {
                        Barcode = p.Barcode,
                        BatchNo = batchno,
                        CreateDT = DateTime.Now,
                        CreateUID = Sys.CurrentUser.UID,
                        NewPrice = p.Price.GetValueOrDefault(),
                        OldPrice = p.Price.GetValueOrDefault(),
                        OperType = 2,
                        StoreId = p.StoreId,
                        BusinessType = 5,
                        CompanyId = CommonService.CompanyId
                    });
                    BaseService<ProductMultPrice>.CurrentRepository.Remove(p, false);
                    refeshItems.Add(new
                    {
                        StoreId = p.StoreId,
                        Barcode = product.Barcode,
                        ProductCode = product.ProductCode,
                        ProductType = product.Nature == 0 ? (product.ValuationType == 2 ? 1 : 0) : (product.Nature == 1 ? 4 : 3)
                    });
                }
                foreach (var dp in deleteSupp)
                {
                    var sup = suppls.FirstOrDefault(o => o.SupplierId == dp.SupplierId);
                    if (sup == null) continue;
                    BaseService<ProductMultSupplier>.CurrentRepository.Remove(sup, false);
                }
                var proImgs = BaseService<ProductImage>.FindList(o => o.CompanyId == product.CompanyId && o.ProductCode == product.ProductCode);
                if (obj.ProductImages != null)
                {
                    foreach (var img in obj.ProductImages)
                    {
                        var image = proImgs.FirstOrDefault(o => o.Id == img.Id);
                        if (image != null)
                        {
                            image.ImagePath = img.ImagePath;
                            image.Status = img.Status;
                            if (img.ImagePath.IsNullOrEmpty())//删除
                            {
                                image.CreateDT = null;
                                image.Status = false;
                            }
                        }
                    }
                }
                for (var i = images.Count - 1; i >= 0; i--)
                {
                    var img = images[i];
                    img.ProductCode = product.ProductCode;
                    var image = proImgs.FirstOrDefault(o => o.Id == img.Id);
                    if (image != null)
                    {
                        img.ToCopyProperty(image);
                        images.RemoveAt(i);
                    }
                }
                #region 修改价格，添加日志
                if (oldSysPrice != obj.SysPrice)
                {//售价
                    changPriceLogs.Add(new ChangePriceLog()
                    {
                        Barcode = obj.Barcode,
                        BatchNo = batchno,
                        CreateDT = DateTime.Now,
                        CreateUID = Sys.CurrentUser.UID,
                        NewPrice = obj.SysPrice,
                        OldPrice = oldSysPrice,
                        OperType = 3,
                        StoreId = "",
                        BusinessType = 4
                    });
                }
                if (oldTradePrice != obj.TradePrice)
                {//批发价
                    changPriceLogs.Add(new ChangePriceLog()
                    {
                        Barcode = obj.Barcode,
                        BatchNo = batchno,
                        CreateDT = DateTime.Now,
                        CreateUID = Sys.CurrentUser.UID,
                        NewPrice = obj.TradePrice,
                        OldPrice = oldTradePrice,
                        OperType = 3,
                        StoreId = "",
                        BusinessType = 1
                    });
                }
                if (oldBuyPrice != obj.BuyPrice)
                {//进价
                    changPriceLogs.Add(new ChangePriceLog()
                    {
                        Barcode = obj.Barcode,
                        BatchNo = batchno,
                        CreateDT = DateTime.Now,
                        CreateUID = Sys.CurrentUser.UID,
                        NewPrice = obj.BuyPrice,
                        OldPrice = oldBuyPrice,
                        OperType = 3,
                        StoreId = "",
                        BusinessType = 2
                    });
                }
                if (oldJoinPrice != obj.JoinPrice)
                {//加盟价
                    changPriceLogs.Add(new ChangePriceLog()
                    {
                        Barcode = obj.Barcode,
                        BatchNo = batchno,
                        CreateDT = DateTime.Now,
                        CreateUID = Sys.CurrentUser.UID,
                        NewPrice = obj.JoinPrice,
                        OldPrice = oldJoinPrice,
                        OperType = 3,
                        StoreId = "",
                        BusinessType = 3
                    });
                }
                #endregion
                if (changPriceLogs.Any())
                {
                    changPriceLogs.Each(o => { o.Barcode = obj.Barcode; o.CompanyId = product.CompanyId; });
                    BaseService<ChangePriceLog>.AddRange(changPriceLogs, false);
                }
                var gps = BaseService<ProductGroup>.FindList(o => o.Barcode == product.Barcode);
                BaseService<ProductGroup>.CurrentRepository.RemoveRange(gps, false);
                groups.Each(o => { o.CompanyId = product.CompanyId; });
                BaseService<ProductGroup>.CurrentRepository.AddRange(groups, false);
                BaseService<ProductImage>.AddRange(images, false);
                re = ProductService.Update(product);
                if (re.Successed)
                {
                    var houses = productDal.GetWareHouseBysn(product.CategorySN, product.CompanyId);
                    var stores = string.Join(",", houses.Select(o => o.StoreId));
                    Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = CommonService.CompanyId, StoreId = stores, Target = "ProductRecord" });
                    Pharos.Infrastructure.Data.Redis.RedisManager.Publish("RefreshProductCache", new List<Pharos.ObjectModels.DTOs.MemoryCacheRefreshQuery>() { new Pharos.ObjectModels.DTOs.MemoryCacheRefreshQuery() { Barcode = product.Barcode, CompanyId = product.CompanyId, StoreId = stores, ProductCode = product.ProductCode, ProductType = product.Nature == 0 ? Pharos.ObjectModels.ProductType.Normal : product.Nature == 1 ? Pharos.ObjectModels.ProductType.Combination : Pharos.ObjectModels.ProductType.Split } });
                    //refeshItems.Add(new
                    //{
                    //    StoreId = string.Join(",",houses.Select(o=>o.StoreId)),
                    //    Barcode = product.Barcode,
                    //    ProductCode = product.ProductCode,
                    //    ProductType = product.Nature == 0 ? (product.ValuationType == 2 ? 1 : 0) : (product.Nature == 1 ? 4 : 3)
                    //});
                    //var parms = new
                    //{
                    //    CompanyId = product.CompanyId,
                    //    RefeshItems = refeshItems
                    //};
                    //var op = CommonService.AppRefresh(parms.ToJson());
                }
            }
            return re;
        }
        public static OpResult SaveSplit(ProductRecord obj, NameValueCollection nvl)
        {
            var op = new OpResult();
            try
            {
                var title = nvl["Title"].Split(',');
                var brand = nvl["BrandSN"].Split(',');
                var category = nvl["Category"].Split(',');
                var subunit = nvl["SubUnitId"].Split(',');
                var size = nvl["Size"].Split(',');
                var buyprice = nvl["BuyPrice"].Split(',');
                var sysprice = nvl["SysPrice"].Split(',');
                var nums = nvl["SaleNum"].Split(',');
                var residueNum = nvl["ResidueNum"];
                var residueSysPrice = nvl["ResidueSysPrice"];
                var residueBuyPrice = nvl["ResidueBuyPrice"];
                if (title.Length <= 0)
                {
                    op.Message = "无数据进行保存!"; return op;
                }
                var list = new List<ProductRecord>();
                var prebar = title.Length.ToString("00") + obj.ProductCode2;
                var maxcode = 1;// ProductService.GetNatureMaxCode(2, obj.ValuationType);
                var maxc = int.Parse(CommonRules.ProductCode);
                obj.CompanyId = CommonService.CompanyId;
                var changPriceLogs = new List<ChangePriceLog>();
                var batchno = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                for (int i = 0; i < title.Length; i++)
                {
                    var product = new ProductRecord();
                    obj.ToCopyProperty(product);
                    maxcode += i;
                    maxc += i;
                    product.Barcode = prebar + maxcode.ToString("00");//10位
                    product.ProductCode = maxc.ToString("000000");
                    product.Title = title[i];
                    product.JoinPrice = product.SysPrice;
                    product.TradePrice = product.SysPrice;
                    product.InventoryWarning = 5;
                    product.ValidityWarning = 5;
                    product.CategorySN = int.Parse(category[i]);
                    if (!brand[i].IsNullOrEmpty())
                        product.BrandSN = int.Parse(brand[i]);
                    if (!subunit[i].IsNullOrEmpty())
                        product.SubUnitId = int.Parse(subunit[i]);
                    if (!buyprice[i].IsNullOrEmpty())
                        product.BuyPrice = decimal.Parse(buyprice[i]);
                    if (!sysprice[i].IsNullOrEmpty())
                        product.SysPrice = decimal.Parse(sysprice[i]);
                    if (!nums[i].IsNullOrEmpty())
                        product.SaleNum = decimal.Parse(nums[i]);
                    product.Size = size[i];
                    product.State = 1;
                    product.RaterUID = Sys.CurrentUser.UID;
                    product.CreateDT = DateTime.Now;
                    op = ValidCategory(product.CategorySN);
                    if (!op.Successed) return op;
                    list.Add(product);
                    changPriceLogs.Add(new ChangePriceLog()
                    {
                        BatchNo = batchno,
                        Barcode = product.Barcode,
                        CreateDT = DateTime.Now,
                        CreateUID = Sys.CurrentUser.UID,
                        NewPrice = product.SysPrice,
                        OldPrice = 0,
                        OperType = 1,
                        StoreId = "",
                        BusinessType = 4,
                        CompanyId = product.CompanyId
                    });
                }
                if (changPriceLogs.Any())
                {
                    BaseService<ChangePriceLog>.AddRange(changPriceLogs, false);
                }
                op = AddRange(list);
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static OpResult SaveSplit(ProductRecord obj)
        {
            var op = new OpResult();
            try
            {
                var parent = Find(o => o.Barcode == obj.OldBarcode);
                if (parent == null)
                {
                    op.Message = "父条码不存在!";
                    return op;
                }
                var prebar = "05" + parent.ProductCode;
                if (!obj.PriceInserted.IsNullOrEmpty())
                {
                    var maxcode = ProductService.GetNatureMaxCode(prebar, 2);
                    var maxc = int.Parse(CommonRules.ProductCode);
                    var insertList = obj.PriceInserted.ToObject<List<ProductRecord>>().Where(o => !o.Title.IsNullOrEmpty()).ToList();
                    var bars = insertList.Where(o=>!o.Barcode.IsNullOrEmpty()).Select(o => o.Barcode);
                    if(bars.Any())
                    {
                        var q = from x in CurrentRepository.Entities.Where(o=>o.CompanyId==CommonService.CompanyId)
                                from y in bars
                                where x.Barcode == y || ("," + x.Barcodes + ",").Contains(","+y+",")
                                select y;
                        if(q.Any())
                        {
                            op.Message = "条码["+string.Join(",",q)+"]已存在！";
                            return op;
                        }
                    }
                    insertList.Each(o =>
                    {
                        o.Nature = 2;
                        o.SupplierId = parent.SupplierId;
                        o.IsAcceptOrder = parent.IsAcceptOrder;
                        o.Favorable = parent.Favorable;
                        o.ValuationType = parent.ValuationType;
                        o.IsReturnSale = parent.IsReturnSale;
                        o.ExpiryUnit = parent.ExpiryUnit;
                        o.OldBarcode = parent.Barcode;
                        o.CategorySN = parent.CategorySN;
                        o.State = parent.State;
                        o.BrandSN = parent.BrandSN;
                        o.SaleRate = parent.SaleRate;
                        o.StockRate = parent.StockRate;
                        o.ValidityWarning = parent.ValidityWarning;
                        o.InventoryWarning = parent.InventoryWarning;
                        o.InventoryWarningMax = parent.InventoryWarningMax;
                        o.SaleRate = parent.SaleRate;
                        o.StockRate = parent.StockRate;
                        o.CityId = parent.CityId;
                        o.Factory = parent.Factory;
                        o.BuyPrice = o.SaleNum.HasValue ? parent.BuyPrice / o.SaleNum.Value : parent.BuyPrice;
                        o.Barcode =o.Barcode.IsNullOrEmpty()? prebar + maxcode.ToString("00000"):o.Barcode;//13位
                        o.ProductCode = maxc.ToString("000000");
                        o.CompanyId = CommonService.CompanyId;
                        o.CreateDT = DateTime.Now;
                        maxcode++;
                        maxc++;
                    });
                    //var barcodes= insertList.Select(o => o.Barcode).ToList();
                    //var list= FindList(o => barcodes.Contains(o.Barcode));
                    //if (list.Any())
                    //{
                    //    op.Message = "条码["+string.Join(",",list.Select(o=>o.Barcode))+"]已存在!";
                    //    return op;
                    //}
                    CurrentRepository.AddRange(insertList, false);
                    #region 操作日志
                    foreach (var item in insertList)
                    {
                        var msg = LogEngine.CompareModelToLog<ProductRecord>(LogModule.产品变价, item);
                        new LogEngine().WriteInsert(msg, LogModule.产品变价);
                    }
                    #endregion
                }
                if (!obj.PriceDeleteed.IsNullOrEmpty())
                {
                    var ids = obj.PriceDeleteed.ToObject<List<ProductRecord>>().Select(o => o.Id).ToList();
                    if (ids.Any())
                    {
                        var list = FindList(o => ids.Contains(o.Id));
                        CurrentRepository.RemoveRange(list, false);
                        #region 操作日志
                        foreach (var item in list)
                        {
                            var msg = LogEngine.CompareModelToLog<ProductRecord>(LogModule.产品变价, null, item);
                            new LogEngine().WriteDelete(msg, LogModule.产品变价);
                        }
                        #endregion
                    }
                }
                var updateList = new List<ProductRecord>();
                var olist = new List<ProductRecord>();
                if (!obj.PriceUpdateed.IsNullOrEmpty())
                {
                    updateList = obj.PriceUpdateed.ToObject<List<ProductRecord>>();
                    var ids = updateList.Select(o => o.Id).ToList();
                    var list = FindList(o => ids.Contains(o.Id));

                    foreach (var item in olist)
                    {
                        var _item = new ProductRecord();
                        ExtendHelper.CopyProperty<ProductRecord>(_item, item);
                        olist.Add(_item);
                    }
                    updateList.Each(o =>
                    {
                        var up = list.FirstOrDefault(i => i.Id == o.Id);
                        if (up != null)
                        {
                            up.Barcode = o.Barcode;
                            up.Title = o.Title;
                            up.SubUnitId = o.SubUnitId;
                            up.SaleNum = o.SaleNum;
                            up.Size = o.Size;
                            up.SysPrice = o.SysPrice;
                            up.CreateDT = DateTime.Now;
                            up.CompanyId = CommonService.CompanyId;
                        }
                    });
                }
                op.Successed = CurrentRepository.Update(updateList);
                if (op.Successed)
                {
                    #region 操作日志
                    for (int i = 0; i < updateList.Count(); i++)
                    {
                        var msg = LogEngine.CompareModelToLog<ProductRecord>(LogModule.产品变价, updateList[i], olist[i]);
                        new LogEngine().WriteUpdate(msg, LogModule.产品变价);
                    }
                    #endregion

                    var houses = productDal.GetWareHouseBysn(obj.CategorySN, obj.CompanyId);
                    var stores = string.Join(",", houses.Select(o => o.StoreId));
                    Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = CommonService.CompanyId, StoreId = stores, Target = "ProductRecord" });
                }
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static object LoadMultPriceList(string barcode)
        {
            if (barcode.IsNullOrEmpty()) return null;
            var query = from a in BaseService<ProductMultPrice>.CurrentRepository.QueryEntity
                        join b in WarehouseService.CurrentRepository.QueryEntity on new { a.StoreId, a.CompanyId } equals new { b.StoreId, b.CompanyId }
                        join c in SupplierService.CurrentRepository.QueryEntity on a.SupplierId equals c.Id into tmp
                        from d in tmp.DefaultIfEmpty()
                        from x in BaseService<ProductRecord>.CurrentRepository.QueryEntity
                        where a.Barcode == barcode && x.Barcode == barcode && a.CompanyId == x.CompanyId && a.CompanyId == CommonService.CompanyId
                        select new
                        {
                            a.StoreId,
                            a.Price,
                            a.Barcode,
                            a.SupplierId,
                            SupplierTitle = d.Title,
                            a.BuyPrice,
                            b.Title,
                            x.IsRelationship
                        };

            return query.ToList();
        }
        public static object LoadMultSupplierList(string barcode)
        {
            if (barcode.IsNullOrEmpty()) return null;
            var query = from a in BaseService<ProductMultSupplier>.CurrentRepository.QueryEntity
                        join b in SupplierService.CurrentRepository.QueryEntity on a.SupplierId equals b.Id
                        join x in BaseService<ProductRecord>.CurrentRepository.QueryEntity on a.Barcode equals x.Barcode
                        where a.Barcode == barcode && x.CompanyId == CommonService.CompanyId
                        select new
                        {
                            a.SupplierId,
                            a.Barcode,
                            b.Title,
                            a.BuyPrice,
                            x.IsRelationship
                        };

            return query.ToList();
        }
        public static IEnumerable<ProductDto> GetProducts(string keyWord, string store, int productBrand, List<int> categories)
        {
            if (categories == null)
            {
                categories = new List<int>() { -1 };
            }
            var categoryDict = ProductCategoryService.CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId && o.State == 1).ToDictionary(o => o.CategorySN, o => o.Title);
            var storeInfo = WarehouseService.Find(o => o.StoreId == store && o.CompanyId == CommonService.CompanyId);
            List<int> canSalecategory = new List<int>();




            if (storeInfo != null)
            {

                canSalecategory = ProductCategoryService.CurrentRepository._context.Database.SqlQuery<int>(
                     @"with my1 as(select * from ProductCategory where ProductCategory.CategoryPSN =0 and CategorySN in (" + (string.IsNullOrEmpty(storeInfo.CategorySN) ? "''" : storeInfo.CategorySN) + @")
                     union all select ProductCategory.* from my1, ProductCategory where my1.CategorySN = ProductCategory.CategoryPSN)
                     select CategorySN from my1"
                ).ToList();
            }
            var query = (from a in BaseService<VwProduct>.CurrentRepository.Entities
                         //from b in ProductBrandService.CurrentRepository.Entities
                         from c in SysDataDictService.CurrentRepository.Entities
                         where
                             //  (a.BrandSN == b.BrandSN)
                          a.SubUnitId == c.DicSN
                         && (a.BrandSN == productBrand || productBrand == -1)
                         && (a.Barcode.Contains(keyWord) || a.ProductCode.Contains(keyWord) || a.Title.Contains(keyWord) || string.IsNullOrEmpty(keyWord))
                         && (categories.Contains(a.CategorySN) || categories.Contains(-1))
                         && (store == "-1" || canSalecategory.Contains(a.CategorySN))
                          && a.CompanyId == CommonService.CompanyId
                         select new ProductDto()
                         {
                             IsWeigh = a.ValuationType == 2,
                             Barcode = a.Barcode,
                             ProductCode = a.ProductCode,
                             Brand = a.BrandSN == null ? "" : ProductBrandService.CurrentRepository.Entities.FirstOrDefault(o => o.BrandSN == a.BrandSN).Title,
                             SysPrice = a.SysPrice,
                             Title = a.Title,
                             Unit = c.Title,
                             Category = a.BigCategoryTitle + (string.IsNullOrEmpty(a.MidCategoryTitle) ? "" : ("/" + a.MidCategoryTitle + (string.IsNullOrEmpty(a.SubCategoryTitle) ? "" : "/" + a.SubCategoryTitle)))
                         }).ToList();
            if (store != "-1" && string.IsNullOrEmpty(store))
            {
                var barcodes = query.Select(o => o.Barcode);
                var pricedict = BaseService<ProductMultPrice>.CurrentRepository.Entities.Where(o => o.StoreId == store && barcodes.Contains(o.Barcode)).ToDictionary(o => o.Barcode, o => o.Price);
                query.ForEach(o =>
                {
                    if (pricedict.Keys.Contains(o.Barcode))
                    {
                        o.SysPrice = pricedict[o.Barcode].GetValueOrDefault();
                    }
                });
            }
            return query;
        }
        /// <summary>
        /// 设置对应门店或批发商售价
        /// </summary>
        /// <typeparam name="T">ProductRecord或VwProduct</typeparam>
        /// <param name="sId">storeId或supplierId</param>
        /// <param name="list"></param>
        /// <param name="type">1－批发价</param>
        public static void SetSysPrice<T>(string sId, IEnumerable<T> list, short type = 0, string supplierId = "") where T : BaseProduct
        {
            var bars = list.Select(o => o.Barcode).ToList();
            var barprices = new List<ProductMultPrice>();
            var date = DateTime.Now.Date;
            if (type == 1)
            {
                if (sId.IsNullOrEmpty()) return;
                var query = from a in BaseService<ProductTradePrice>.CurrentRepository.Entities
                            join b in BaseService<ProductTradePriceList>.CurrentRepository.Entities on a.Id equals b.TradePriceId
                            where a.State == 1 && ("," + a.Wholesaler + ",").Contains("," + sId + ",") && bars.Contains(b.Barcode)
                                && b.EndDate >= date && b.StartDate <= date && a.CompanyId == CommonService.CompanyId
                            orderby a.CreateDT descending
                            select b;

                barprices = query.AsEnumerable().Select(o => new ProductMultPrice()
                {
                    Barcode = o.Barcode,
                    BuyPrice = o.BuyPrice,
                    Price = o.TradePrice
                }).ToList();
            }
            else
            {
                var multPrices = new List<ProductMultPrice>();
                if (!sId.IsNullOrEmpty())
                {
                    if (sId.Contains(","))
                    {
                        barprices = productDal.GetChangePriceByStoreIds(sId, string.Join(",", bars), CommonService.CompanyId) ?? new List<ProductMultPrice>();
                    }
                    else
                    {
                        var query = from a in BaseService<ProductChangePrice>.CurrentRepository.Entities
                                    join b in BaseService<ProductChangePriceList>.CurrentRepository.Entities on a.Id equals b.ChangePriceId
                                    where a.State == 1 && b.State == 1 && ("," + a.StoreId + ",").Contains("," + sId + ",") && bars.Contains(b.Barcode)
                                            && a.CompanyId == CommonService.CompanyId
                                    orderby a.CreateDT descending
                                    select new { a.SupplierId, b.CurBuyPrice, b.CurSysPrice, b.Barcode };
                        if (!supplierId.IsNullOrEmpty())
                            query = query.Where(o => o.SupplierId == supplierId);
                        barprices = query.AsEnumerable().Select(o => new ProductMultPrice()
                        {
                            Barcode = o.Barcode,
                            BuyPrice = o.CurBuyPrice,
                            Price = o.CurSysPrice
                        }).ToList();//变价首先
                    }
                    var exbars = barprices.Select(o => o.Barcode).ToList();
                    var express = DynamicallyLinqHelper.True<ProductMultPrice>().And(o => o.StoreId == sId).And(o => bars.Contains(o.Barcode))
                        .And(o => o.SupplierId == supplierId, supplierId.IsNullOrEmpty()).And(o => !exbars.Contains(o.Barcode), !exbars.Any())
                        .And(o => o.CompanyId == CommonService.CompanyId);
                    multPrices = BaseService<ProductMultPrice>.FindList(express);//一品多价为三
                }
                var multSuppliers = new List<ProductMultSupplier>();
                if (!supplierId.IsNullOrEmpty())
                {
                    multSuppliers = BaseService<ProductMultSupplier>.FindList(o => o.SupplierId == supplierId && bars.Contains(o.Barcode));
                }
                foreach (var supp in multSuppliers)
                {
                    var price = multPrices.FirstOrDefault(o => o.Barcode == supp.Barcode);
                    if (price == null)
                    {
                        decimal? sysPrice = null;//取售价
                        var obj = barprices.FirstOrDefault(o => o.Barcode == supp.Barcode);
                        if (obj != null)
                            sysPrice = obj.Price;
                        else
                        {
                            obj = multPrices.FirstOrDefault(o => o.Barcode == supp.Barcode);
                            if (obj != null)
                                sysPrice = obj.Price;
                            else
                            {
                                var source = list.FirstOrDefault(o => o.Barcode == supp.Barcode);
                                sysPrice = source.SysPrice;
                            }
                        }
                        var _newPMP = new ProductMultPrice() { Barcode = supp.Barcode, BuyPrice = supp.BuyPrice, Price = sysPrice };
                        multPrices.Add(_newPMP);
                        #region 操作日志
                        var msg = LogEngine.CompareModelToLog<ProductMultPrice>(LogModule.产品变价, _newPMP);
                        new LogEngine().WriteInsert(msg, LogModule.产品变价);
                        #endregion
                    }
                    else
                        price.BuyPrice = supp.BuyPrice;//一品多商为二
                }
                barprices.AddRange(multPrices);
            }
            foreach (var o in list)
            {
                var obj = barprices.FirstOrDefault(i => i.Barcode == o.Barcode);
                if (obj == null) continue;
                if (obj.Price.HasValue)
                    o.SysPrice = obj.Price.GetValueOrDefault();
                if (obj.BuyPrice.HasValue)
                    o.BuyPrice = obj.BuyPrice.GetValueOrDefault();
            }
        }
        /// <summary>
        /// 设置对应门店售价
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="dt"></param>
        public static void SetSysPrice(string storeId, System.Data.DataTable dt)
        {
            if (storeId.IsNullOrEmpty() || dt == null) return;
            if (!dt.Columns.Contains("Barcode") || !dt.Columns.Contains("SysPrice")) return;
            var date = DateTime.Now.Date;
            var bars = dt.AsEnumerable().Select(o => o["Barcode"].ToString()).ToList();
            var query = from a in BaseService<ProductChangePrice>.CurrentRepository.Entities
                        join b in BaseService<ProductChangePriceList>.CurrentRepository.Entities on a.Id equals b.ChangePriceId
                        where a.State == 1 && ("," + a.StoreId + ",").Contains("," + storeId + ",") && bars.Contains(b.Barcode)
                            && a.CompanyId == CommonService.CompanyId
                        orderby a.AuditorDT descending, a.CreateDT descending
                        select new { a.SupplierId, b.CurBuyPrice, b.CurSysPrice, b.Barcode };

            var barprices = query.AsEnumerable().Select(o => new ProductMultPrice()
            {
                Barcode = o.Barcode,
                BuyPrice = o.CurBuyPrice,
                Price = o.CurSysPrice
            }).ToList();//变价首先
            var exbars = barprices.Select(o => o.Barcode).ToList();
            var express = DynamicallyLinqHelper.True<ProductMultPrice>().And(o => o.StoreId == storeId).And(o => bars.Contains(o.Barcode))
                .And(o => !exbars.Contains(o.Barcode), !exbars.Any()).And(o => o.CompanyId == CommonService.CompanyId);
            var multPrices = BaseService<ProductMultPrice>.FindList(express);//一品多价为三
            barprices.AddRange(multPrices);
            foreach (DataRow dr in dt.Rows)
            {
                var pro = barprices.FirstOrDefault(i => i.Barcode == dr["Barcode"].ToString());
                if (pro != null)
                {
                    dr["SysPrice"] = pro.Price;
                    if (dt.Columns.Contains("BuyPrice"))
                        dr["BuyPrice"] = pro.BuyPrice;
                }
            }
        }
        /// <summary>
        /// 处理副条码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> SetAssistBarcode<T>(IEnumerable<T> list) where T : BaseProduct, new()
        {
            var ls = list.ToClone();
            ls.Each(o => o.TitleExt = o.Barcode + "（" + o.Title + "）");
            foreach (var obj in list)
            {
                if (obj.Barcodes.IsNullOrEmpty()) continue;
                var bars = obj.Barcodes.Trim().Split(',');
                var title = string.Empty;
                bars.Each(o =>
                {
                    if (!o.IsNullOrEmpty())
                    {
                        var to = new T();
                        obj.ToCopyProperty(to);
                        to.Barcode = o;
                        to.Barcodes = "";
                        to.OldBarcode = obj.Barcode;
                        to.TitleExt =  o +"（" +obj.Title + "）";
                        ls.Add(to);
                    }
                });
            }
            return ls;
        }
        public static void SetStockNum(string sId, IEnumerable<VwProduct> list)
        {
            if (sId.IsNullOrEmpty()) return;
            var bars = list.Select(o => o.Barcode).ToList();
            var inverts = BaseService<Inventory>.FindList(o => bars.Contains(o.Barcode) && o.StoreId == sId && o.CompanyId == CommonService.CompanyId);
            foreach (var pro in list)
            {
                var inv = inverts.FirstOrDefault(o => o.Barcode == pro.Barcode);
                if (inv == null) continue;
                pro.StoreStockNums = inv.StockNumber;
            }
        }
        /// <summary>
        /// 调价记录
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        //public static DataTable LoadChangLogList(string barcode, out int recordCount)
        //{
        //    return productDal.GetChangeLogList(barcode, out recordCount);
        //}
        /// 调价记录
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> LoadChangLogList(string barcode, out int recordCount)
        {
            var query = from a in BaseService<ChangePriceLog>.CurrentRepository.Entities
                        join b in WarehouseService.CurrentRepository.Entities on new { a.StoreId, a.CompanyId } equals new { b.StoreId, b.CompanyId } into ware
                        join c in UserInfoService.CurrentRepository.Entities on a.CreateUID equals c.UID into user
                        from w in ware.DefaultIfEmpty()
                        from u in user.DefaultIfEmpty()
                        where a.OperType != 2 && a.Barcode == barcode && a.CompanyId == CommonService.CompanyId
                        select new
                        {
                            a.Id,
                            a.Barcode,
                            a.BatchNo,
                            a.CreateDT,
                            a.NewPrice,
                            a.OldPrice,
                            OperType = a.OperType == 1 ? "添加" : a.OperType == 2 ? "删除" : "修改",
                            BusinessType = a.BusinessType.HasValue ? (a.BusinessType == 1 ? "批发价" : (a.BusinessType == 2 ? "进价" : (a.BusinessType == 3 ? "加盟价" : (a.BusinessType == 4 ? "售价" : (a.BusinessType == 5 ? "一品多价" : ""))))) : "",
                            StoreTitle = w.Title,
                            FullName = u.FullName
                        };
            recordCount = query.Count();
            return query.ToPageList();
        }
        public static OpResult ProductWeighSet(string inserted, string deleteed, string updateed, string rows)
        {
            var op = new OpResult();
            var list = rows.ToObject<List<WeighingSet>>().Where(o => !o.Code.IsNullOrEmpty()).ToList();
            var inserts = inserted.ToObject<List<WeighingSet>>().Where(o => !o.Code.IsNullOrEmpty()).ToList();
            var deleteIds = deleteed.ToObject<List<WeighingSet>>().Where(o => o.Id != 0).Select(o => o.Id).ToList();
            var updates = updateed.ToObject<List<WeighingSet>>().Where(o => o.Id != 0).ToList();
            var deletes = BaseService<WeighingSet>.FindList(o => deleteIds.Contains(o.Id));
            if (list.GroupBy(o => o.Code).Count() != list.Count)
            {
                op.Message = "秤号存在重复!";
                return op;
            }
            if (list.GroupBy(o => o.IP).Count() != list.Count)
            {
                op.Message = "IP地址存在重复!";
                return op;
            }
            deletes = BaseService<WeighingSet>.CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId).ToList();
            BaseService<WeighingSet>.CurrentRepository.RemoveRange(deletes, false);
            list.Each(o => o.CompanyId = CommonService.CompanyId);
            op = BaseService<WeighingSet>.AddRange(list);
            #region 操作日志
            foreach (var item in deletes)
            {
                var msg = LogEngine.CompareModelToLog(Sys.LogModule.档案管理, null, item);
                new LogEngine().WriteDelete(msg, LogModule.档案管理);
            }
            #endregion

            if (!op.Message.IsNullOrEmpty() && op.Message.Contains("UNIQUE KEY"))
                op.Message = "秤号或IP地址存在重复";
            return op;
        }
        public static IEnumerable<object> FindWeighSetList()
        {
            return BaseService<WeighingSet>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId).OrderBy(o => o.Code).ToList().Select(o => new
            {
                o.Code,
                o.Id,
                o.Title,
                o.Type,
                o.IP,
                o.Serie,
                o.Port,
                Type2 = Enum.GetName(typeof(WeighType), o.Type)
            });
        }
        public static IEnumerable<dynamic> ProductWeighBatchList(NameValueCollection nvc, out int count)
        {
            var selBatch = nvc["selBatch"];
            var store = nvc["store"];
            var searchtext = nvc["searchtext"];
            var communication = short.Parse(nvc["Communication"]);
            var details = new string[] { };
            int batchId = 0;
            if (!selBatch.IsNullOrEmpty() && nvc["dataType"] == "2")
            {
                var batch = BaseService<WeighingBatch>.Find(o => o.CompanyId == CommonService.CompanyId && o.BatchNo == selBatch);
                if (batch != null)
                {
                    details = batch.Details.Split(',');
                    batchId = batch.Id;
                }
            }
            //else if (communication==2)//联机时不管显示全新还是历史
            //{
            //    var start = DateTime.Now.Date;
            //    var end = DateTime.Now.Date.AddDays(1);
            //    var batch = BaseService<WeighingBatch>.FindList(o => o.CompanyId == CommonService.CompanyId && o.Communication == communication && o.StoreId == store && o.CreateDT >= start && o.CreateDT<end).FirstOrDefault();
            //    if (batch != null)
            //    {
            //        details = batch.Details.Split(',');
            //        batchId = batch.Id;
            //    }
            //}
            var q = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId && details.Contains(o.Barcode));
            if (!searchtext.IsNullOrEmpty()) q = q.Where(o => o.Barcode.Contains(searchtext) || o.Title.Contains(searchtext) || o.ProductCode.Contains(searchtext));
            count = q.Count();
            return q.ToPageList().Select(o => new
            {
                o.Barcode,
                o.ProductCode,
                o.Title,
                o.CategoryTitle,
                o.BrandTitle,
                o.Size,
                o.Id,
                o.SubUnit,
                o.SysPrice,
                BatchId = batchId
            });
        }
        public static OpResult RemoveBatchList(int batchId, string bars)
        {
            var obj = BaseService<WeighingBatch>.Find(o => o.Id == batchId);
            var oldbars = obj.Details.Split(',').ToList();
            var delbars = bars.Split(',');
            foreach (var b in delbars)
            {
                if (oldbars.Any(o => o == b))
                    oldbars.Remove(b);
            }
            obj.Details = string.Join(",", oldbars);
            return BaseService<WeighingBatch>.Update(obj);
        }
        public static List<DropdownItem> GetBatchList(string storeId, short communication)
        {
            var express = DynamicallyLinqHelper.Empty<WeighingBatch>().And(o => o.StoreId == storeId).And(o => o.Communication == communication).And(o => o.CompanyId == SysCommonRules.CompanyId);
            var list2 = BaseService<WeighingBatch>.CurrentRepository.Entities.Where(express).OrderByDescending(o => o.CreateDT).ToList();
            var list = list2.Select(o => o.BatchNo)
                .Distinct().ToList().Select(o => new DropdownItem(o)).ToList();
            if (list.Any()) list[0].IsSelected = true;
            return list;
        }
        public static List<DropdownItem> GetDataForSearch(string searchText, string searchField)
        {
            var list = productDal.GetDataForSearch(searchText, searchField, CommonService.CompanyId);
            return list.Select(o => new DropdownItem(o)).ToList();
        }
        public static string SendDevice(string code, string maxRecord, string store)
        {
            if (code.IsNullOrEmpty()) return "设备编号为空!";
            var set = BaseService<WeighingSet>.Find(o => o.Code == code && o.CompanyId == CommonService.CompanyId);
            if (set == null) return "设备编号不存在!";
            if (set.IP.IsNullOrEmpty()) return "该设备IP地址为空!";
            var info = new Pharos.Logic.BLL.DataSynchronism.UpdateFormData();
            info.Datas["Pharos.Logic.LocalEntity.ProductInfo2"] = new List<Pharos.Logic.BLL.DataSynchronism.Dtos.ProductInfoForLocal>();
            info.StoreId = store;
            var data = Pharos.Logic.BLL.DataSynchronism.DataSyncContext.ExportAll(info);
            var list = new List<TPlu>();
            var max = maxRecord.IsNullOrEmpty() ? 0 : int.Parse(maxRecord);
            if (!store.IsNullOrEmpty()) store = store.PadLeft(2, '0');
            var dataDictionaryList = SysDataDictService.CurrentRepository.QueryEntity.Where(o => o.DicPSN == 4 && o.CompanyId == CommonService.CompanyId).ToList();
            var addbars = new List<string>();
            var selBatch = HttpContext.Current.Request["selBatch"];
            var newno = (store.IsNullOrEmpty() ? "0" : store) + "-" + code + "-" + DateTime.Now.ToString("yyyyMMdd");
            if (HttpContext.Current.Request["DataType"] == "1" || selBatch.IsNullOrEmpty()) selBatch = newno;
            var batch = BaseService<WeighingBatch>.Find(o => o.CompanyId == CommonService.CompanyId && o.BatchNo == selBatch);
            var oldbars = batch != null ? batch.Details.Split(',').ToList() : new List<string>();
            foreach (var de in data.Datas)
            {
                if (!de.Value.Any()) continue;
                var pros = de.Value.Take(max);
                int i = 100000;
                foreach (var p in pros)
                {
                    var o = (ProductRecord)p;
                    TPlu tplu = new TPlu();
                    tplu.Name = o.Title;
                    tplu.LFCode = int.Parse(o.Barcode);
                    tplu.Code = o.Barcode;
                    tplu.UnitPrice = Convert.ToInt32(o.SysPrice * 100);
                    tplu.Deptment = store;
                    tplu.WeightUnit = GetWeightUnitBySubUnitId(o.SubUnitId, dataDictionaryList, 0);
                    list.Add(tplu);
                    //if(!oldbars.Contains(o.Barcode))
                    addbars.Add(o.Barcode);
                }
            }
            string msg = "传输完毕!";
            switch (set.Type)
            {
                case WeighType.顶尖:
                    string m = AclasService.TransferData(list, set.IP);
                    if (!m.IsNullOrEmpty())
                        msg = "传输失败!" + m;
                    break;
                default:
                    msg = "暂不支持此类型设备!";
                    break;
            }

            if (msg == "传输完毕!")
            {
                if (batch == null && addbars.Any())
                {
                    BaseService<WeighingBatch>.Add(new WeighingBatch()
                    {
                        BatchNo = selBatch,
                        CompanyId = CommonService.CompanyId,
                        Communication = 2,
                        CreateDT = DateTime.Now,
                        CreateUID = CurrentUser.UID,
                        StoreId = info.StoreId,
                        Details = string.Join(",", addbars)
                    });
                }
                else
                {
                    //oldbars.AddRange(addbars);
                    batch.Details = string.Join(",", addbars);
                    BaseService<WeighingBatch>.Update(batch);
                }
            }
            return msg;
        }
        public static string ExportWeight(string maxRecord, string store)
        {
            var expType = HttpContext.Current.Request["ExportType"];
            var ext = expType == "2" ? ".txt" : expType == "1" ? ".txp" : ".xls";
            var info = new Pharos.Logic.BLL.DataSynchronism.UpdateFormData();
            info.Datas["Pharos.Logic.LocalEntity.ProductInfo2"] = new List<Pharos.Logic.BLL.DataSynchronism.Dtos.ProductInfoForLocal>();
            info.StoreId = store;
            var data = Pharos.Logic.BLL.DataSynchronism.DataSyncContext.ExportAll(info);
            string relativePath = "";
            var root = Sys.SysConstPool.SaveAttachPath(ref relativePath, "temp");
            bool hasFile = false;
            var maxrecord = maxRecord.IsNullOrEmpty() ? 0 : int.Parse(maxRecord);
            string fileName = "称重商品";
            var files = new List<string>();

            var addbars = new List<string>();
            var oldbars = new List<string>();
            var selBatch = HttpContext.Current.Request["selBatch"];
            var newno = "";
            WeighingBatch batch = null;
            if (HttpContext.Current.Request["DataType"] == "1" || selBatch.IsNullOrEmpty())
            {
                var suf = DateTime.Now.ToString("yyyyMMdd");
                batch = BaseService<WeighingBatch>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId && o.Communication == 1 && o.StoreId == info.StoreId && o.BatchNo.EndsWith(suf)).OrderByDescending(o => o.CreateDT).FirstOrDefault();
                var pre = (store.IsNullOrEmpty() ? "0" : store.PadLeft(2, '0')) + "-F";
                if (batch != null)
                {
                    var xh = batch.BatchNo.Substring(batch.BatchNo.IndexOf(pre) + pre.Length, 2);
                    newno = pre + (int.Parse(xh) + 1).ToString("00") + "-" + suf;
                }
                else
                {
                    newno = pre + "01" + "-" + suf;
                }
                batch = null;
            }
            else if (!selBatch.IsNullOrEmpty())
            {
                batch = BaseService<WeighingBatch>.Find(o => o.CompanyId == CommonService.CompanyId && o.Communication == 1 && o.BatchNo == selBatch);
                oldbars = batch.Details.Split(',').ToList();
            }
            foreach (var de in data.Datas)
            {
                if (!de.Value.Any()) continue;

                var list = de.Value;
                var count = maxrecord % list.Count() == 0 ? list.Count() / maxrecord : list.Count() / maxrecord + 1;
                for (int i = 0; i < count; i++)
                {
                    list = de.Value.Skip(i * maxrecord).Take(maxrecord);
                    var fn = fileName + (i + 1) + ext;
                    if (expType == "3")
                    {//3=导出excel文件
                        CreateXls(fileName + (i + 1), list, store);
                        return "";
                    }
                    else
                    {
                        CreateTxt(Path.Combine(root, fn), list, store);
                    }
                    files.Add(Path.Combine(root, fn));
                }
                if (count == 0)
                {
                    var fn = fileName + ext;
                    if (expType == "3")
                    {//3=导出excel文件
                        CreateXls(fileName, list, store);
                        return "";
                    }
                    else
                    {
                        CreateTxt(Path.Combine(root, fn), list, store);
                    }
                    files.Add(Path.Combine(root, fn));
                }
                hasFile = true;
                foreach (ProductRecord o in list)
                {
                    //if (!oldbars.Contains(o.Barcode))
                    addbars.Add(o.Barcode);
                }
            }
            if (hasFile)
            {
                string zipfileName = fileName;
                byte[] buffer;
                if (files.Count > 1)
                {
                    Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(Encoding.UTF8);
                    foreach (var f in files)
                    {
                        if (!File.Exists(f))
                            continue;
                        zip.AddFile(f, "");
                    }
                    using (var stream = new MemoryStream())
                    {
                        zip.Save(stream);
                        zipfileName += ".zip";
                        buffer = stream.GetBuffer();
                        zip.Dispose();
                    }
                }
                else
                {
                    var path = files.FirstOrDefault();
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        zipfileName = zipfileName + Path.GetExtension(path);
                        buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        stream.Close();
                    }
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.StatusCode = 200;
                if (HttpContext.Current.Request.Browser.Browser.Equals("InternetExplorer", StringComparison.CurrentCultureIgnoreCase))
                {
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(zipfileName));
                }
                else
                {
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;fileName=" + zipfileName);
                }
                HttpContext.Current.Response.BinaryWrite(buffer);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
                if (batch == null && addbars.Any())
                {
                    BaseService<WeighingBatch>.Add(new WeighingBatch()
                    {
                        BatchNo = newno,
                        CompanyId = CommonService.CompanyId,
                        Communication = 1,
                        CreateDT = DateTime.Now,
                        CreateUID = CurrentUser.UID,
                        StoreId = info.StoreId,
                        Details = string.Join(",", addbars)
                    });
                }
                else
                {
                    //oldbars.AddRange(addbars);
                    batch.Details = string.Join(",", addbars);
                    BaseService<WeighingBatch>.Update(batch);
                }
                return "";
            }
            else
                return "无数据导出";

        }
        /// <summary>
        /// 验证当前选择类别是否为最后一级的类别
        /// </summary>
        /// <param name="sn">当前类别</param>
        /// <returns></returns>
        static OpResult ValidCategory(int sn)
        {
            var op = new OpResult();
            if (ProductCategoryService.IsExist(o => o.CompanyId == CommonService.CompanyId && o.CategoryPSN == sn))
                op.Message = "请选择最后一组子类别";
            else
                op.Successed = true;
            return op;
        }
        private static string GetCategory(int big, int mid, int sub, Dictionary<int, string> categoryDict)
        {
            var result = string.Empty;
            if (big != -1 && categoryDict.Keys.Contains(big))
            {
                result += categoryDict[big];
                if (mid != -1 && categoryDict.Keys.Contains(mid))
                {
                    result += "/" + categoryDict[mid];
                    if (sub != -1 && categoryDict.Keys.Contains(sub))
                    {
                        result += "/" + categoryDict[sub];
                    }
                }
            }
            return result;
        }

        static void CreateXls(string fullName, IEnumerable<object> list, string storeId)
        {
            var listData = new List<ProductRecord>();
            var dataDictionaryList = BaseService<SysDataDictionary>.CurrentRepository.QueryEntity.Where(o => o.DicPSN == 4).ToList();
            int id = 1;
            foreach (var item in list)
            {
                var temp = item as ProductRecord;
                temp.Id = id;
                id++;
                //1=    重量，2=件 3=kg 4=g  7=500g  100g=8
                temp.SubUnitId = GetWeightUnitBySubUnitId(temp.SubUnitId, dataDictionaryList, 1);
                listData.Add(temp);
            }
            if (!list.Any()) return;
            if (!storeId.IsNullOrEmpty()) storeId = storeId.PadLeft(2, '0');
            List<string> fields = new List<string>() { "Id", "Title", "SysPrice", "SubUnitId", "Barcode", "Barcode" };
            List<string> names = new List<string>() { "编号", "名称", "单价", "单位", "货号", "索引条码" };
            new ExportExcel() { IsBufferOutput = true }.ToExcel(fullName, listData.ToDataTable(), fields.ToArray(), names.ToArray(), null, null, null, false);

        }

        static void CreateTxt(string fullName, IEnumerable<object> list, string storeId)
        {
            if (!list.Any()) return;
            var dataDictionaryList = BaseService<SysDataDictionary>.CurrentRepository.QueryEntity.Where(o => o.DicPSN == 4).ToList();
            System.Text.StringBuilder sb = new StringBuilder();
            if (!storeId.IsNullOrEmpty()) storeId = storeId.PadLeft(2, '0');
            foreach (var obj in list)
            {
                var pro = obj as ProductRecord;
                sb.Append("0");//PLU No.	流水号
                sb.Append("\t");
                sb.Append(pro.Title);//Name	品名
                sb.Append("\t");
                sb.Append(pro.Barcode);//LFCode	生鲜码
                sb.Append("\t");
                sb.Append(pro.Barcode);//Code	条码
                sb.Append("\t");
                sb.Append("17");//Barcode Type	条码类型    （79为18位条码，17为13位条码）
                sb.Append("\t");
                sb.Append((pro.SysPrice * 100).ToString("f0"));//Unit Price	单价
                sb.Append("\t");
                sb.Append(GetWeightUnitBySubUnitId(pro.SubUnitId, dataDictionaryList, 0));//Weight Unit	称重单位
                sb.Append("\t");
                sb.Append(storeId);//Deptment	部门
                sb.Append("\t");
                sb.Append("0");//Tare	皮重
                sb.Append("\t");
                sb.Append("0");//Shelf Time	保存期
                sb.Append("\t");
                sb.Append("0");//Package Type	包装类型
                sb.Append("\t");
                sb.Append("0");//Package Weight	包装重量
                sb.Append("\t");
                sb.Append("0");//Package Tolerance	包装误差
                sb.Append("\t");
                sb.Append("0");//Message1	信息1
                sb.Append("\t");
                sb.Append("0");//Message2	信息2
                sb.Append("\t");
                sb.Append("0");//Account	会计信息
                sb.Append("\t");
                sb.Append("0");//Multi Label	多标签
                sb.Append("\t");
                sb.Append("0");//Rebate	单价折扣
                sb.Append("\t");
                sb.AppendLine("0");//PCS Type	数量类型
            }
            using (var sw = new StreamWriter(fullName, false, Encoding.GetEncoding("gb2312")))
            {
                sw.Write(sb.ToString());
                sw.Close();
            }
        }
        /// <summary>
        /// 获取对应单位
        /// </summary>
        /// <param name="subUnitId"></param>
        /// <param name="dataDictionaryList"></param>
        /// <returns></returns>
        private static int GetWeightUnitBySubUnitId(int subUnitId, List<SysDataDictionary> dataDictionaryList, int type)
        {
            var result = 4;
            var unit = dataDictionaryList.FirstOrDefault(o => o.DicSN == subUnitId);
            if (unit != null)
            {
                var unitTitle = unit.Title.Trim().ToUpper();
                //原代码
                if (type == 0)
                {
                    switch (unitTitle)
                    {
                        case "KG": result = 4;
                            break;
                        case "公斤": result = 4;
                            break;
                        case "500G": result = 7;
                            break;
                        case "斤": result = 7;
                            break;
                        case "G": result = 1;
                            break;
                        case "克": result = 1;
                            break;
                        case "10G": result = 2;
                            break;
                        case "100G": result = 3;
                            break;
                        case "600G": result = 8;
                            break;
                    }
                }
                else if (type == 1)
                {//友声电子秤对应单位
                    result = 1;
                    switch (unitTitle)
                    {
                        case "KG": result = 3;
                            break;
                        case "公斤": result = 3;
                            break;
                        case "500G": result = 7;
                            break;
                        case "斤": result = 7;
                            break;
                        case "G": result = 4;
                            break;
                        case "克": result = 4;
                            break;
                        case "100G": result = 8;
                            break;
                    }

                }
            }

            return result;
        }
        #region 产品变价
        public static System.Data.DataTable FindChangePriceList(NameValueCollection nvl, out int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            nl.Add("CompanyId", CommonService.CompanyId.ToString());
            return productDal.GetChangePriceList(nl, out recordCount);
        }
        public static object LoadChangeDetailList(string mid)
        {
            if (mid.IsNullOrEmpty()) return null;
            var query = from a in BaseService<ProductChangePriceList>.CurrentRepository.Entities
                        join b in CurrentRepository.Entities on a.Barcode equals b.Barcode
                        where b.CompanyId == SysCommonRules.CompanyId && a.ChangePriceId == mid
                        select new
                        {
                            a.Id,
                            b.Barcode,
                            b.Title,
                            b.Size,
                            b.ProductCode,
                            a.OldBuyPrice,
                            a.OldSysPrice,
                            a.CurBuyPrice,
                            a.CurSysPrice,
                            a.CurGrossprofitRate,
                            a.OldGrossprofitRate,
                            a.StartDate,
                            a.EndDate,
                            a.Memo,
                            a.ChangePriceId
                        };
            return query.ToList().Select(o => new
            {
                o.Id,
                o.Barcode,
                o.ProductCode,
                o.Title,
                o.Size,
                o.OldBuyPrice,
                o.OldSysPrice,
                o.CurBuyPrice,
                o.CurSysPrice,
                o.CurGrossprofitRate,
                o.OldGrossprofitRate,
                o.ChangePriceId,
                o.StartDate,
                o.EndDate,
                o.Memo
            });
        }
        public static OpResult SaveOrUpdateChangePrice(ProductChangePrice obj, string inserted, string deleted, string updated)
        {
            if (obj.SupplierId.IsNullOrEmpty())
                return OpResult.Fail("供应商不存在!");
            var insertList = new List<ProductChangePriceList>();
            var deleteList = new List<ProductChangePriceList>();
            var updateList = new List<ProductChangePriceList>();
            if (!inserted.IsNullOrEmpty())
            {
                insertList = inserted.ToObject<List<ProductChangePriceList>>();
                if (insertList != null) insertList = insertList.Where(o => !o.Barcode.IsNullOrEmpty()).ToList();
            }
            if (!deleted.IsNullOrEmpty())
            {
                deleteList = deleted.ToObject<List<ProductChangePriceList>>();
                if (deleteList != null) deleteList = deleteList.Where(o => !o.Barcode.IsNullOrEmpty()).ToList();
            }
            if (!updated.IsNullOrEmpty())
            {
                updateList = updated.ToObject<List<ProductChangePriceList>>();
                if (updateList != null) updateList = updateList.Where(o => !o.Barcode.IsNullOrEmpty()).ToList();
            }
            if (obj.Id.IsNullOrEmpty())
            {
                obj.Id = CommonRules.GUID;
                obj.CreateDT = DateTime.Now;
                obj.CreateUID = Sys.CurrentUser.UID;
                obj.ChangePriceList = insertList;
                obj.CompanyId = CommonService.CompanyId;
                #region 操作日志
                var msg = LogEngine.CompareModelToLog<ProductChangePrice>(LogModule.产品变价, obj);
                new LogEngine().WriteInsert(msg, LogModule.产品变价);
                #endregion
                return BaseService<ProductChangePrice>.Add(obj);
            }
            else
            {
                var change = BaseService<ProductChangePrice>.CurrentRepository.QueryEntity.Include(o => o.ChangePriceList).FirstOrDefault(o => o.Id == obj.Id);
                change.SupplierId = obj.SupplierId;
                change.StoreId = obj.StoreId;
                change.ChangePriceList.AddRange(insertList);
                foreach (var up in updateList)
                {
                    var p = change.ChangePriceList.FirstOrDefault(o => o.Id == up.Id);
                    if (p == null) continue;
                    up.ToCopyProperty(p);
                }
                foreach (var up in deleteList)
                {
                    var p = change.ChangePriceList.FirstOrDefault(o => o.Id == up.Id);
                    if (p == null) continue;
                    BaseService<ProductChangePriceList>.CurrentRepository.Remove(p, false);
                    #region 操作日志
                    var msg = LogEngine.CompareModelToLog<ProductChangePriceList>(LogModule.产品变价, null, up);
                    new LogEngine().WriteDelete(msg, LogModule.产品变价);
                    #endregion
                }
                return BaseService<ProductChangePrice>.Update(change);
            }
        }
        public static OpResult DeleteChangePrice(int[] ids)
        {
            var op = new OpResult();
            try
            {
                var priceList = BaseService<ProductChangePriceList>.FindList(o => ids.Contains(o.Id));
                if (!priceList.Any())
                {
                    op.Message = "查不到数据";
                    return op;
                }
                var mids = priceList.Select(o => o.ChangePriceId).Distinct().ToList();
                var changList = BaseService<ProductChangePrice>.CurrentRepository.Entities.Include(o => o.ChangePriceList).Where(o => mids.Contains(o.Id)).ToList();
                changList.Each(o =>
                {
                    BaseService<ProductChangePriceList>.CurrentRepository.RemoveRange(o.ChangePriceList, false);
                    BaseService<ProductChangePrice>.CurrentRepository.Remove(o);
                });
                #region 操作日志
                foreach (var item in changList)
                {
                    var msg = LogEngine.CompareModelToLog<ProductChangePrice>(LogModule.档案管理, null, item);
                    new LogEngine().WriteDelete(msg, LogModule.档案管理);
                    foreach (var i in item.ChangePriceList)
                    {
                        var _msg = LogEngine.CompareModelToLog<ProductChangePriceList>(LogModule.档案管理, null, i);
                        new LogEngine().WriteDelete(_msg, LogModule.档案管理);
                    }
                }
                #endregion
                //foreach (var id in mids)
                //{
                //    var count = changList.Where(o => o.Id == id).SelectMany(o => o.ChangePriceList).Count();
                //    var dis = priceList.Where(o => o.ChangePriceId == id).ToList();
                //    if (count == dis.Count)
                //    {
                //        var prom = changList.FirstOrDefault(o => o.Id == id);
                //        BaseService<ProductChangePrice>.CurrentRepository.Remove(prom, false);
                //        BaseService<ProductChangePriceList>.CurrentRepository.RemoveRange(dis, true);
                //    }
                //    else
                //    {
                //        BaseService<ProductChangePriceList>.CurrentRepository.RemoveRange(dis, true);
                //    }
                //}
                op.Successed = true;
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static OpResult AuditorChangeState(string ids)
        {
            var op = new OpResult();
            try
            {
                if (ids.IsNullOrEmpty())
                {
                    op.Message = "请选择要处理的项";
                }
                else
                {
                    var id = ids.Split(',').Distinct().ToList();
                    var list = BaseService<ProductChangePrice>.CurrentRepository.Entities.Include(o => o.ChangePriceList).Where(o => id.Contains(o.Id)).ToList();
                    //var list = BaseService<ProductChangePrice>.FindList(o => id.Contains(o.Id));
                    var refeshItems = new List<object>();
                    list.Each(o =>
                    {
                        o.State = 1; o.AuditorUID = Sys.CurrentUser.UID; o.AuditorDT = DateTime.Now;
                        o.ChangePriceList.Each(i =>
                        {
                            i.State = 1;
                            refeshItems.Add(new { StoreId = o.StoreId, Barcode = i.Barcode, ProductCode = "", ProductType = 0 });
                        });
                    });
                    op = BaseService<ProductChangePrice>.Update(list);
                    if (op.Successed)
                    {
                        var parms = new
                        {
                            CompanyId = CommonService.CompanyId,
                            RefeshItems = refeshItems
                        };
                        //CommonService.AppRefresh(parms.ToJson());
                        var stores = string.Join(",", list.Select(o => o.StoreId));
                        var refreshs = list.SelectMany(o => o.ChangePriceList).Select(o => new Pharos.ObjectModels.DTOs.MemoryCacheRefreshQuery() { Barcode = o.Barcode, CompanyId = CommonService.CompanyId, StoreId = stores, ProductType = Pharos.ObjectModels.ProductType.Normal});
                        Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = CommonService.CompanyId, StoreId = stores, Target = "ProductRecord" });
                        Pharos.Infrastructure.Data.Redis.RedisManager.Publish("RefreshProductCache", refreshs);
                    }
                }
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static OpResult SetInvalid(string ids)
        {
            var op = new OpResult();
            if (ids.IsNullOrEmpty())
            {
                op.Message = "请选择要处理的项";
            }
            else
            {
                var id = ids.Split(',').Distinct().ToList();
                var list = BaseService<ProductChangePrice>.CurrentRepository.QueryEntity.Include(o => o.ChangePriceList).Where(o => id.Contains(o.Id)).ToList();
                #region 操作日志 记录原状态
                var oState = new List<ProductChangePrice>();
                foreach (var item in list)
                {
                    ProductChangePrice itm = new ProductChangePrice();
                    ExtendHelper.CopyProperty<ProductChangePrice>(itm, item);
                    oState.Add(itm);
                }
                #endregion
                list.Each(o =>
                {
                    o.State = 2;
                    o.ChangePriceList.Each(i => { i.State = 0; });
                });
                op = BaseService<ProductChangePrice>.Update(list);
                #region 操作日志
                if (op.Successed)
                {
                    for (int i = 0; i < list.Count(); i++)
                    {
                        var msg = LogEngine.CompareModelToLog<ProductChangePrice>(LogModule.档案管理, list[i], oState[i]);
                        new LogEngine().WriteUpdate(msg, LogModule.档案管理);
                    }
                }
                #endregion

                var stores = string.Join(",", list.Select(o => o.StoreId));
                var refreshs = list.SelectMany(o => o.ChangePriceList).Select(o => new Pharos.ObjectModels.DTOs.MemoryCacheRefreshQuery() { Barcode = o.Barcode, CompanyId = CommonService.CompanyId, StoreId = stores, ProductType = Pharos.ObjectModels.ProductType.Normal });
                Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = CommonService.CompanyId, StoreId = stores, Target = "ProductRecord" });
                Pharos.Infrastructure.Data.Redis.RedisManager.Publish("RefreshProductCache", refreshs);
            }
            return op;
        }
        public static DataTable GetCurrentPrice(string barcode, short type)
        {
            var dt = productDal.GetCurrentPrice(barcode, SysCommonRules.CompanyId, type);
            return dt;
        }
        static SaleState SetState(DateTime startDate, DateTime endDate)
        {
            return startDate > DateTime.Now ? Pharos.Logic.SaleState.未开始 : startDate <= DateTime.Now && endDate.AddDays(1) > DateTime.Now ? Pharos.Logic.SaleState.活动中 : Pharos.Logic.SaleState.已过期;
        }

        #endregion
        #region 产品批发价
        public static System.Data.DataTable FindTradePriceList(NameValueCollection nvl, out int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            nl.Add("CompanyId", CommonService.CompanyId.ToString());
            return productDal.GetTradePriceList(nl, out recordCount);
        }
        public static object LoadTradeDetailList(string tid)
        {
            if (tid.IsNullOrEmpty()) return null;
            var query = from a in BaseService<ProductTradePriceList>.CurrentRepository.Entities
                        join b in CurrentRepository.Entities on a.Barcode equals b.Barcode
                        join c in BaseService<ProductTradePrice>.CurrentRepository.Entities on a.TradePriceId equals c.Id
                        where b.CompanyId == c.CompanyId && a.TradePriceId == tid
                        select new
                        {
                            a.Id,
                            b.Barcode,
                            b.Title,
                            b.Size,
                            a.TradePrice,
                            a.BuyPrice,
                            a.SysPrice,
                            a.StartDate,
                            a.EndDate,
                            a.Memo,
                            c.State,
                            a.TradePriceId
                        };
            return query.ToList().Select(o => new
            {
                o.Id,
                o.Barcode,
                o.Title,
                o.Size,
                o.BuyPrice,
                o.SysPrice,
                o.TradePrice,
                StartDate = o.StartDate.ToString("yyyy-MM-dd"),
                EndDate = o.EndDate.HasValue ? o.EndDate.Value.ToString("yyyy-MM-dd") : "",
                o.Memo,
                o.State,
                o.TradePriceId
            });
        }
        public static OpResult SaveOrUpdateTradePrice(ProductTradePrice obj, string inserted, string deleted, string updated)
        {
            var insertList = new List<ProductTradePriceList>();
            var deleteList = new List<ProductTradePriceList>();
            var updateList = new List<ProductTradePriceList>();
            if (!inserted.IsNullOrEmpty())
            {
                insertList = inserted.ToObject<List<ProductTradePriceList>>();
            }
            if (!deleted.IsNullOrEmpty())
            {
                deleteList = deleted.ToObject<List<ProductTradePriceList>>();
            }
            if (!updated.IsNullOrEmpty())
            {
                updateList = updated.ToObject<List<ProductTradePriceList>>();
            }
            obj.CompanyId = CommonService.CompanyId;
            if (obj.Id.IsNullOrEmpty())
            {
                obj.Id = CommonRules.GUID;
                obj.CreateDT = DateTime.Now;
                obj.CreateUID = Sys.CurrentUser.UID;
                obj.State = 0;
                obj.TradePriceList = insertList;
                #region 操作日志
                var msg = LogEngine.CompareModelToLog<ProductTradePrice>(LogModule.档案管理, obj);
                new LogEngine().WriteInsert(msg, LogModule.档案管理);
                #endregion
                return BaseService<ProductTradePrice>.Add(obj);
            }
            else
            {
                var change = BaseService<ProductTradePrice>.CurrentRepository.QueryEntity.Include(o => o.TradePriceList).FirstOrDefault(o => o.Id == obj.Id);
                change.Wholesaler = obj.Wholesaler;
                foreach (var up in updateList)
                {
                    var p = change.TradePriceList.FirstOrDefault(o => o.Id == up.Id);
                    if (p == null) continue;
                    up.ToCopyProperty(p);
                }
                foreach (var up in deleteList)
                {
                    var p = change.TradePriceList.FirstOrDefault(o => o.Id == up.Id);
                    if (p == null) continue;
                    BaseService<ProductTradePriceList>.CurrentRepository.Remove(p, false);
                    #region 操作日志
                    try
                    {
                        var msg = LogEngine.CompareModelToLog<ProductTradePriceList>(LogModule.档案管理, null, p);
                        new LogEngine().WriteInsert(msg, LogModule.档案管理);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    #endregion
                }
                return BaseService<ProductTradePrice>.Update(change);
            }
        }
        public static OpResult DeleteTradePrice(int[] ids)
        {
            var op = new OpResult();
            try
            {
                var priceList = BaseService<ProductTradePriceList>.FindList(o => ids.Contains(o.Id));
                if (!priceList.Any())
                {
                    op.Message = "查不到数据";
                    return op;
                }
                var mids = priceList.Select(o => o.TradePriceId).Distinct().ToList();
                var changList = BaseService<ProductTradePrice>.CurrentRepository.Entities.Include(o => o.TradePriceList).Where(o => mids.Contains(o.Id)).ToList();
                changList.Each(o =>
                {
                    BaseService<ProductTradePriceList>.CurrentRepository.RemoveRange(o.TradePriceList, false);
                    BaseService<ProductTradePrice>.CurrentRepository.Remove(o);
                });
                op.Successed = true;
                #region 操作日志

                foreach (var item in changList)
                {
                    var msg = LogEngine.CompareModelToLog<ProductTradePrice>(LogModule.档案管理, item);
                    new LogEngine().WriteInsert(msg, LogModule.档案管理);
                    foreach (var i in item.TradePriceList)
                    {
                        var _msg = LogEngine.CompareModelToLog<ProductTradePriceList>(LogModule.档案管理, i);
                        new LogEngine().WriteInsert(_msg, LogModule.档案管理);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static OpResult AuditorTradeState(string ids)
        {
            var op = new OpResult();
            try
            {
                if (ids.IsNullOrEmpty())
                {
                    op.Message = "请选择要处理的项";
                }
                else
                {
                    var id = ids.Split(',').Distinct().ToList();
                    var list = BaseService<ProductTradePrice>.FindList(o => id.Contains(o.Id));
                    list.Each(o => { o.State = 1; o.AuditorUID = Sys.CurrentUser.UID; o.AuditorDT = DateTime.Now; });
                    op = BaseService<ProductTradePrice>.Update(list);
                }
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        #endregion
        #region 产品导入
        public static OpResult Import(ImportSet obj, System.Web.HttpFileCollectionBase httpFiles, string fieldName, string columnName)
        {
            var op = new OpResult();
            var errLs = new List<string>();
            int count = 0;
            try
            {
                Dictionary<string, char> fieldCols = null;
                DataTable dt = null;
                op = ImportSetService.ImportSet(obj, httpFiles, fieldName, columnName, ref fieldCols, ref dt);
                if (!op.Successed) return op;
                var categorys = ProductCategoryService.GetAllProductCategory(null).ToList();
                var otherClass = categorys.FirstOrDefault(o => o.Title.StartsWith("其"));
                var brands = ProductBrandService.GetAllProductBrands(null).ToList();
                var otherBrand = brands.FirstOrDefault(o => o.Title.StartsWith("其"));
                var brandClass = SysDataDictService.FindList(o => o.DicPSN == (int)DicType.品牌分类 && o.CompanyId == CommonService.CompanyId);
                var otherBrandClass = brandClass.FirstOrDefault(o => o.Title.StartsWith("其"));
                var units = SysDataDictService.GetSubUnitCategories();
                var otherUnit = units.FirstOrDefault(o => o.Title.StartsWith("其"));
                var suppliers = SupplierService.GetList();
                var products = GetAllProducts();
                var maxCate = ProductCategoryService.MaxSn;
                var maxBrand = ProductBrandService.MaxSN;
                var maxDict = SysDataDictService.GetMaxSN;

                if (!fieldCols.ContainsKey("ValuationType"))
                    return OpResult.Fail("计价方式不存在!");

                var clsIdx = Convert.ToInt32(fieldCols["CategorySN"]) - 65;
                var brandIdx = Convert.ToInt32(fieldCols["BrandSN"]) - 65;
                var unitIdx = Convert.ToInt32(fieldCols["SubUnitId"]) - 65;
                var supplierIdx = Convert.ToInt32(fieldCols["SupplierId"]) - 65;
                var valtypeIdx = Convert.ToInt32(fieldCols["ValuationType"]) - 65;
                var barcodeIdx = Convert.ToInt32(fieldCols["Barcode"]) - 65;
                var titleIdx = Convert.ToInt32(fieldCols["Title"]) - 65;
                var sizeIdx = fieldCols.GetValue("Size").ToType<int>() - 65;
                var sysPriceIdx = fieldCols.GetValue("SysPrice").ToType<int>() - 65;
                var expiryUnitIdx = fieldCols.GetValue("ExpiryUnit").ToType<int>() - 65;
                var expiryIdx = fieldCols.GetValue("Expiry").ToType<int>() - 65;
                count = dt.Rows.Count;
                var pcs = new List<ProductCenter>();
                var commonDal = new CommonDAL();
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        var dr = dt.Rows[i];
                        var pc = new ProductCenter();
                        #region 条码验证
                        var text = dr[barcodeIdx].ToString();
                        pc.Barcode = text;
                        if (text.IsNullOrEmpty())
                        {
                            errLs.Add("行号[" + i +obj.MinRow+ "]条码为空!");
                            dt.Rows.RemoveAt(i);
                            continue;
                        }
                        else if (products.Any(o => o.Barcode == text))
                        {
                            errLs.Add("条码[" + text + "]已存在!");
                            dt.Rows.RemoveAt(i);
                            continue;
                        }
                        else
                        {
                            products.Add(new ProductRecord() { Barcode = text });
                        }
                        #endregion
                        #region 处理类别
                        text = dr[clsIdx].ToString();
                        pc.CategoryTitle = text;
                        if (!text.IsNullOrEmpty())
                        {
                            var cates = text.Split('/');
                            var first = cates.FirstOrDefault();
                            var third = cates.LastOrDefault();
                            var second = "";
                            if (cates.Length == 3)
                            {
                                second = cates[1];
                                third = cates[2];
                            }
                            else if (cates.Length == 2)
                            {
                                second = cates[1];
                                third = null;
                            }
                            else if (cates.Length == 1)
                            {
                                second = null;
                                third = null;
                            }
                            else if (cates.Length > 3)
                            {
                                second = cates[1];
                                third = text.Replace(first + "/" + second + "/", "");
                            }
                            var parent = categorys.FirstOrDefault(o => o.CategoryPSN == 0 && o.Title == first);

                            var cls = parent != null ? categorys.FirstOrDefault(o => o.CategoryPSN == parent.CategorySN && o.Title == second) : null;
                            cls = cls != null && !third.IsNullOrEmpty() ? categorys.FirstOrDefault(o => o.CategoryPSN == cls.CategorySN && o.Title == third) : cls;
                            if (cls != null)
                            {
                                dr[clsIdx] = cls.CategorySN.ToString();
                            }
                            else
                            {
                                if (obj.RefCreate)
                                {
                                    var list = new List<ProductCategory>();
                                    cls = categorys.FirstOrDefault(o => o.CategoryPSN == 0 && o.Title == first);
                                    int psn = 0;
                                    short grade = 2;
                                    if (cls == null)
                                    {
                                        parent = new ProductCategory()
                                        {
                                            CategorySN = ++maxCate,
                                            Title = first,
                                            CategoryCode = ProductCategoryService.MaxCode(0),
                                            Grade = 1,
                                            State = 1,
                                            CompanyId = CommonService.CompanyId
                                        };
                                        list.Add(parent);
                                        if (!second.IsNullOrEmpty())
                                        {
                                            psn = parent.CategorySN;
                                            var child = new ProductCategory()
                                            {
                                                CategoryPSN = psn,
                                                CategorySN = ++maxCate,
                                                Title = second,
                                                CategoryCode = ProductCategoryService.MaxCode(psn),
                                                Grade = grade,
                                                State = 1,
                                                CompanyId = parent.CompanyId
                                            };
                                            list.Add(child);
                                            psn = child.CategorySN;
                                            grade = 3;
                                            if (!third.IsNullOrEmpty())
                                            {
                                                child = new ProductCategory()
                                                {
                                                    CategoryPSN = psn,
                                                    CategorySN = ++maxCate,
                                                    Title = third,
                                                    CategoryCode = ProductCategoryService.MaxCode(psn),
                                                    Grade = grade,
                                                    State = 1,
                                                    CompanyId = parent.CompanyId
                                                };
                                                psn = child.CategorySN;
                                                list.Add(child);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        psn = cls.CategorySN;
                                        if (!second.IsNullOrEmpty())
                                        {
                                            cls = categorys.FirstOrDefault(o => o.CategoryPSN == cls.CategorySN && o.Title == second);
                                            if (cls == null)
                                            {
                                                var maxcode = ProductCategoryService.MaxCode(psn);
                                                cls = new ProductCategory()
                                                {
                                                    CategoryPSN = psn,
                                                    CategorySN = ++maxCate,
                                                    Title = second,
                                                    CategoryCode = maxcode,
                                                    Grade = grade,
                                                    State = 1,
                                                    CompanyId = CommonService.CompanyId
                                                };
                                                list.Add(cls);
                                                psn = cls.CategorySN;
                                                grade = 3;
                                            }
                                            else
                                                psn = cls.CategorySN;
                                            if (!third.IsNullOrEmpty())
                                            {
                                                cls = categorys.FirstOrDefault(o => o.CategoryPSN == cls.CategorySN && o.Title == third);
                                                if (cls == null)
                                                {
                                                    var maxcode = ProductCategoryService.MaxCode(psn);
                                                    cls = new ProductCategory()
                                                    {
                                                        CategoryPSN = psn,
                                                        CategorySN = ++maxCate,
                                                        Title = third,
                                                        CategoryCode = maxcode,
                                                        Grade = grade,
                                                        State = 1,
                                                        CompanyId = parent.CompanyId
                                                    };
                                                    list.Add(cls);
                                                    psn = cls.CategorySN;
                                                }
                                                else
                                                    psn = cls.CategorySN;
                                            }
                                        }
                                    }
                                    ProductCategoryService.AddRange(list);
                                    categorys.AddRange(list);
                                    dr[clsIdx] = list.Any() ? list.LastOrDefault().CategorySN.ToString() : psn.ToString();
                                }
                                else if (otherClass != null)
                                {
                                    dr[clsIdx] = otherClass.CategorySN.ToString();
                                }
                                else
                                {
                                    errLs.Add("条码[" + dr[barcodeIdx] + "]类别不存在!");
                                    dt.Rows.RemoveAt(i);//去除不导入
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            errLs.Add("条码[" + dr[barcodeIdx] + "]类别不存在!");
                            dt.Rows.RemoveAt(i);//去除不导入
                            continue;
                        }
                        #endregion
                        #region 处理品牌
                        text = dr[brandIdx].ToString();
                        pc.BrandTitle = text;
                        if (!text.IsNullOrEmpty())
                        {
                            var cls = brands.FirstOrDefault(o => o.Title == text);
                            if (cls != null)
                            {
                                dr[brandIdx] = cls.BrandSN.ToString();
                                pc.BrandClassTitle = commonDal.GetBrandClassTitle(cls.BrandSN, CommonService.CompanyId);
                            }
                            else
                            {
                                if (obj.RefCreate)
                                {
                                    if (otherBrandClass == null)
                                    {
                                        errLs.Add("条码[" + dr[barcodeIdx] + "]品牌分类不存在!");
                                        dt.Rows.RemoveAt(i);
                                        continue;
                                    }
                                    var data = new ProductBrand()
                                    {
                                        BrandSN = maxBrand++,
                                        Title = text,
                                        ClassifyId = otherBrandClass.DicSN,
                                        JianPin = "",
                                        State = 1,
                                        CompanyId = CommonService.CompanyId
                                    };
                                    ProductBrandService.Add(data);
                                    brands.Add(data);
                                    dr[brandIdx] = data.BrandSN.ToString();
                                    pc.BrandClassTitle = commonDal.GetBrandClassTitle(data.BrandSN, CommonService.CompanyId);
                                }
                                else if (otherBrand != null)
                                {
                                    dr[brandIdx] = otherBrand.BrandSN.ToString();
                                    pc.BrandClassTitle = commonDal.GetBrandClassTitle(otherBrand.BrandSN, CommonService.CompanyId);
                                }
                                else
                                {
                                    errLs.Add("条码[" + dr[barcodeIdx] + "]品牌不存在!");
                                    dt.Rows.RemoveAt(i);
                                    continue;
                                }
                            }
                        }
                        #endregion
                        #region 处理单位
                        text = dr[unitIdx].ToString();
                        pc.SubUnit = text;
                        if (!text.IsNullOrEmpty())
                        {
                            var cls = units.FirstOrDefault(o => o.Title == text);
                            if (cls != null)
                            {
                                dr[unitIdx] = cls.DicSN.ToString();
                            }
                            else
                            {
                                if (obj.RefCreate)
                                {
                                    var data = new Sys.Entity.SysDataDictionary()
                                    {
                                        DicPSN = (int)DicType.计量小单位,
                                        DicSN = maxDict++,
                                        Status = true,
                                        Title = text,
                                        CompanyId = CommonService.CompanyId
                                    };
                                    SysDataDictService.Add(data);
                                    units.Add(data);
                                    dr[unitIdx] = data.DicSN.ToString();
                                }
                                else if (otherUnit != null)
                                {
                                    dr[unitIdx] = otherUnit.DicSN.ToString();
                                }
                                else
                                {
                                    errLs.Add("条码[" + dr[barcodeIdx] + "]单位不存在!");
                                    dt.Rows.RemoveAt(i);
                                    continue;
                                }
                            }
                        }
                        #endregion
                        #region 处理供应商
                        text = dr[supplierIdx].ToString();
                        if (!text.IsNullOrEmpty())
                        {
                            var cls = suppliers.FirstOrDefault(o => o.Title == text || o.FullTitle == text);
                            if (cls != null)
                            {
                                dr[supplierIdx] = cls.Id.ToString();
                            }
                            else
                            {
                                errLs.Add("条码[" + dr[barcodeIdx] + "]供应商不存在!");
                                dt.Rows.RemoveAt(i);
                                continue;
                            }
                        }
                        else
                        {
                            errLs.Add("条码[" + dr[barcodeIdx] + "]供应商为空!");
                            dt.Rows.RemoveAt(i);
                            continue;
                        }
                        #endregion
                        #region 处理计价方式
                        text = dr[valtypeIdx].ToString();
                        if (!text.IsNullOrEmpty())
                        {
                            dr[valtypeIdx] = text.Replace(" ", "").Trim() == "称重" ? 2 : 1;
                        }
                        #endregion
                        #region 处理保质期
                        text = dr.GetValue(expiryUnitIdx).ToString();
                        if (!text.IsNullOrEmpty())
                        {
                            text = text.Trim();
                            pc.ExpiryUnit = Convert.ToInt16(text == "天" ? 1 : text == "月" ? 2 : 3);
                            dr[expiryUnitIdx] = pc.ExpiryUnit;
                        }
                        text = dr.GetValue(expiryIdx).ToString();
                        if (!text.IsNullOrEmpty())
                        {
                            pc.Expiry = text.ToType<short>();
                        }
                        #endregion
                        #region 上传oms
                        pc.Source = 2;
                        pc.Title = dr.GetValue(titleIdx).ToType<string>();
                        pc.Size = dr.GetValue(sizeIdx).ToType<string>();
                        pc.SysPrice = dr.GetValue(sysPriceIdx).ToType<decimal>();
                        pc.CompanyIds = obj.CompanyId.ToString();
                        pcs.Add(pc);
                        #endregion
                    }
                    catch (Exception e)
                    {
                        throw new Exception("创建相关记录失败," + e.Message, e);
                    }
                }
                var maxCode = int.Parse(CommonRules.ProductCode);
                //new Pharos.Logic.DAL.CommonDAL().BulkCopy(obj.TableName, dtCopy);
                StringBuilder sb = new StringBuilder();
                sb.Append("begin tran ");
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("insert into ");
                    sb.Append(obj.TableName);
                    sb.Append("(CompanyId,ProductCode,ValidityWarning,InventoryWarning,State,");
                    sb.Append(string.Join(",", fieldCols.Keys));
                    sb.Append(") values(");
                    sb.AppendFormat("{0},", obj.CompanyId);
                    sb.AppendFormat("'{0}',", (maxCode++).ToString("000000"));
                    sb.Append("5,5,1,");
                    foreach (var de in fieldCols)
                    {
                        var index = Convert.ToInt32(de.Value) - 65;
                        try
                        {
                            var text = dr[index].ToString().Trim();
                            sb.Append("'" + text + "',");
                        }
                        catch (Exception e)
                        {
                            throw new Exception("列选择超过范围!", e);
                        }
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                    sb.Append(");");
                }
                sb.Append(" commit tran");
                if (dt.Rows.Count > 0)
                {
                    commonDal._db.ExecuteNonQueryText(sb.ToString(), null);
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        var url = Config.GetAppSettings("omsurl");
                        if (!url.IsNullOrEmpty())
                        {
                            var r = HttpClient.HttpPost(url + "api/outerapi/PostProduct", pcs.ToJson());
                        }
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
            return CommonService.GenerateImportHtml(errLs, count);
        }
        public static OpResult ImportPrice(ImportSet obj, System.Web.HttpFileCollectionBase httpFiles, string fieldName, string columnName)
        {
            var op = new OpResult();
            var errLs = new List<string>();
            int count = 0;
            try
            {
                Dictionary<string, char> fieldCols = null;
                DataTable dt = null;
                op = ImportSetService.ImportSet(obj, httpFiles, fieldName, columnName, ref fieldCols, ref dt);
                if (!op.Successed) return op;
                var barcodeIdx = Convert.ToInt32(fieldCols["Barcode"]) - 65;
                var rateIdx = fieldCols.GetValue("SaleRate").ToType<int>() - 65;
                var buyPriceIdx = fieldCols.GetValue("BuyPrice").ToType<int>() - 65;
                var sysPriceIdx = fieldCols.GetValue("SysPrice").ToType<int>() - 65;
                var tradePriceIdx = fieldCols.GetValue("TradePrice").ToType<int>() - 65;
                var joinPriceIdx = fieldCols.GetValue("JoinPrice").ToType<int>() - 65;
                if (rateIdx < 0 && buyPriceIdx < 0 && sysPriceIdx < 0 && tradePriceIdx <= 0 && joinPriceIdx < 0)
                    return OpResult.Fail("请选择需要更新的项！");
                var products = GetAllProducts();
                count = dt.Rows.Count;
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    var dr = dt.Rows[i];
                    var text = dr[barcodeIdx].ToString();
                    if (text.IsNullOrEmpty())
                    {
                        errLs.Add("行号[" + i + "]条码为空!");
                        dt.Rows.RemoveAt(i);
                        continue;
                    }
                    if (!products.Any(o => o.Barcode == text || ("," + o.Barcodes + ",").Contains("," + text + ",")))
                    {
                        errLs.Add("行号[" + i + "]条码不存在!");
                        dt.Rows.RemoveAt(i);
                        continue;
                    }
                    if (dr.GetValue(rateIdx).ToString().IsNullOrEmpty() && dr.GetValue(buyPriceIdx).ToString().IsNullOrEmpty()
                        && dr.GetValue(sysPriceIdx).ToString().IsNullOrEmpty() && dr.GetValue(tradePriceIdx).ToString().IsNullOrEmpty()
                        && dr.GetValue(joinPriceIdx).ToString().IsNullOrEmpty())
                    {
                        errLs.Add("行号[" + i + "]条码更新项都为空!");
                        dt.Rows.RemoveAt(i);
                        continue;
                    }
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("begin tran ");
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("update ProductRecord set ");
                    string barcode = null;
                    foreach (var de in fieldCols)
                    {
                        var index = Convert.ToInt32(de.Value) - 65;
                        try
                        {
                            var text = dr[index].ToString().Trim();
                            sb.Append(de.Key + "=");
                            sb.Append("'" + text + "',");
                            barcode = barcode ?? text;
                        }
                        catch (Exception e)
                        {
                            throw new Exception("列选择超过范围!", e);
                        }
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                    sb.AppendFormat(" where companyid={0} and barcode='{1}';", CommonService.CompanyId, barcode);
                }
                sb.Append(" commit tran");
                var commonDal = new CommonDAL();
                if (dt.Rows.Count > 0)
                {
                    commonDal._db.ExecuteNonQueryText(sb.ToString(), null);
                }
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                op.Successed = false;
                Log.WriteError(ex);
                errLs.Add("导入出现异常!");
            }
            return CommonService.GenerateImportHtml(errLs, count);
        }
        #endregion
        /// <summary>
        /// 根据条码查询该产品是否存在业务关系，如果存在业务关系，则将产品表的IsRelationship置为1
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public static bool IsRelationship(string barcode)
        {
            var bar = BaseService<ProductRecord>.Find(o => (o.Barcode == barcode || ("," + o.Barcodes + ",").Contains("," + barcode + ",")) && o.CompanyId == CommonService.CompanyId);
            if (bar.IsRelationship) return true;
            int count = productDal.GetIsRelationship(barcode, CommonService.CompanyId);
            bool isRel = count > 0 ? true : false;
            if (isRel)
            {
                bar.IsRelationship = true;
                BaseService<ProductRecord>.Update(bar);
            }
            return isRel;
            //var bar = BaseService<ProductRecord>.Find(o => o.Barcode == barcode);
            //OpResult re = new OpResult() { Successed = true };
            //try
            //{
            //    if (count > 0)
            //    {
            //        bar.IsRelationship = 1;
            //        //re = BaseService<ProductRecord>.Update(bar);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    re.Message = ex.Message;
            //    Log.WriteError(ex);
            //}
            //return re;
        }
        public static List<ProductRecord> GetAllProducts()
        {
            var compid = CommonService.CompanyId;
            var list = DataCache.Get<List<ProductRecord>>("allproducts" + compid);
            if (list == null)
            {
                list = CurrentRepository.FindList(o => o.CompanyId == compid).ToList();
                //DataCache.Set("allproducts"+compid, list, 3);
            }
            return list;
        }
        public static DataTable GetProductsBybars(List<string> barcodes)
        {
            return productDal.GetProductsBybars(string.Join(",", barcodes), CommonService.CompanyId);
        }
        public static PageResult<object> GetProductsForAPI(int pageIndex, int pageSize, int firstSN,
            int? secondSN, int? threeSN, string barcode)
        {
            firstSN = threeSN.HasValue && threeSN != 0 ? threeSN.Value : secondSN.HasValue && secondSN != 0 ? secondSN.Value : firstSN;
            var categorys = new List<int>() { firstSN };
            categorys = ProductCategoryService.GetChildSNs(categorys, true);
            var qwhere = DynamicallyLinqHelper.Empty<VwProduct>().And(o => categorys.Contains(o.CategorySN), !categorys.Any())
                .And(o => (o.Barcode.Contains(barcode) || (o.Barcodes != null && o.Barcodes.Contains(barcode))), barcode.IsNullOrEmpty())
                .And(o => o.CompanyId == CommonService.CompanyId);

            var query = from o in BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(qwhere)
                        select new
                        {
                            o.Id,
                            o.Barcode,
                            o.Title,
                            o.Size,
                            o.SysPrice,
                            o.DiscountPrice,
                            Url = BaseService<ProductImage>.CurrentRepository.QueryEntity.Where(i => i.CompanyId == o.CompanyId && i.ProductCode == o.ProductCode && !(i.ImagePath == null || i.ImagePath == "")).OrderByDescending(i => i.Status).ThenByDescending(i => i.CreateDT).Select(i => i.ImagePath).FirstOrDefault()
                        };
            var count = query.Count();
            var list = query.OrderBy(o => o.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var result = new PageResult<object>();
            var url = Config.GetAppSettings("ServerIP");
            result.Datas = list.Select(o => new
            {
                o.Barcode,
                o.Title,
                o.Size,
                o.SysPrice,
                o.DiscountPrice,
                Url = o.Url.IsNullOrEmpty() ? "" : url + o.Url.Insert(o.Url.LastIndexOf("."), "_s")
            });
            var pageCount = count / pageSize + (count % pageSize > 0 ? 1 : 0);
            result.Pager = new PageInfo()
            {
                Index = pageIndex,
                Size = pageSize,
                Total = count,
                PageCount = pageCount
            };
            return result;
        }
        public static object GetProductByBarcode(string barcode)
        {
            var pro = BaseService<VwProduct>.Find(o => o.CompanyId == CommonService.CompanyId && (o.Barcode == barcode || ("," + o.Barcodes + ",").Contains(","+barcode+",")));
            if (pro == null) throw new MessageException("条码不存在！");
            var stores = productDal.GetStockNumByStore(BLL.WarehouseService.GetList(), pro.Barcode, pro.Nature, pro.CompanyId);
            var url = Config.GetAppSettings("ServerIP");
            var urls = BaseService<ProductImage>.CurrentRepository.QueryEntity.Where(i => i.CompanyId == pro.CompanyId && i.ProductCode == pro.ProductCode && !(i.ImagePath == null || i.ImagePath == "")).OrderByDescending(i => i.Status).ThenBy(i => i.CreateDT).Select(i => url + i.ImagePath).ToList();
            return new { Barcode = barcode, pro.Title, pro.Size, pro.SysPrice, pro.SubUnit, pro.ValuationTypeTitle, pro.StockNums, pro.DiscountPrice, Urls = urls, Stores = stores };
        }
        //public object GetProductsByBarcodes(string barcodes)
        //{
        //    var result = (from a in CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId && ("," + barcodes + ",").Contains("," + o.Barcode + ","))
        //                  select new
        //                  {
        //                      ProductCode = a.ProductCode,
        //                      Barcode = a.Barcode,
        //                      Title = a.Title,
        //                      CategoryTitle = a.Category,
        //                      SubUnit = a.su,
        //                      SysPrice = a.SysPrice,
        //                      StockNums
        //                  });
        //    return result;
        //}
        /// <summary>
        /// 查询门店下的称重商品
        /// </summary>
        /// <param name="categorySn"></param>
        /// <param name="store"></param>
        /// <param name="valuationType">1-计件或2-称重</param>
        /// <returns></returns>
        public static object GetProductByStore(int categorySn, string store, short valuationType, string searchText, out int count)
        {
            count = 0;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId && o.ValuationType == valuationType);
            //Warehouse 查门店对应的经营品类
            //var warehouse = BaseService<Warehouse>.Find(o => o.CompanyId == CommonService.CompanyId && o.StoreId == store);
            //if (warehouse == null)
            //{
            //    return null;
            //}
            //if (("," + warehouse.CategorySN + ",").Contains("," + categorySn + ","))
            //{
            //查询子类
            if (categorySn > 0)
            {
                var ware = WarehouseService.Find(o => o.StoreId == store && o.CompanyId == CommonService.CompanyId);
                if (ware != null && !ware.CategorySN.IsNullOrEmpty())
                {
                    var categorySNs = ware.CategorySN.Split(',').Select(o => int.Parse(o)).ToList();
                    var childs = ProductCategoryService.GetChildSNs(categorySNs, true);
                    queryProduct = queryProduct.Where(o => childs.Contains(o.CategorySN));
                }
                var categorys = BaseService<ProductCategory>.CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId && (o.CategoryPSN == categorySn || o.CategorySN == categorySn)).Select(o => o.CategorySN).OrderBy(o => o).ToList().Distinct();
                var allcategory = string.Join(",", categorys);
                queryProduct = queryProduct.Where(o => ("," + allcategory + ",").Contains("," + o.CategorySN + ","));
            }
            var q = (from p in queryProduct
                     select new VwProductModel
                     {
                         Id = p.Id,
                         ProductCode = p.ProductCode,
                         Barcode = p.Barcode,
                         CategoryTitle = p.CategoryTitle,
                         Title = p.Title,
                         Size = p.Size,
                         BrandTitle = p.BrandTitle,
                         SysPrice = p.SysPrice,
                         SubUnit = p.SubUnit,
                         Factory = p.Factory,
                         CityTitle = (p.CityId == 0 || p.CityId == -1) ? "" : BaseService<Area>.CurrentRepository.Entities.Where(o => o.AreaID == p.CityId).Select(o => o.Title).FirstOrDefault()
                     });
            if (!searchText.IsNullOrEmpty())
                q = q.Where(o => o.Barcode.Contains(searchText) || o.ProductCode.Contains(searchText) || o.Title.Contains(searchText));
            var list = q.ToList();
            SetSysPrice<VwProductModel>(store, list);
            count = list.Count();
            //商品表的对应数据
            return list;
        }

        public static object GetProductByBatch(string batch, out int count)
        {
            var hd = BaseService<ScaleHistory>.FindList(o => o.CompanyId == CommonService.CompanyId && o.BatchSN == batch).Select(o => o.Barcode).ToList();
            var ps = string.Join(",", hd);
            var result = BaseService<VwProduct>.FindList(o => o.CompanyId == CommonService.CompanyId && ("," + ps + ",").Contains("," + o.Barcode + ","));
            SetSysPrice<VwProduct>(Sys.SysCommonRules.CurrentStore, result);
            count = result.Count();
            return result;
        }

        /// <summary>
        /// 商品传秤
        /// </summary>
        /// <param name="ids"></param>
        public static OpResult SendProduct(string ids, List<string> ips, bool isClear)
        {
            //测试ip连接
            StringBuilder sb = new StringBuilder();
            foreach (var item in ips)
            {
                var r = NetWorkInfo.NetPing(item);
                if (!r)
                {
                    sb.Append("ip:" + item + "无法连接！");
                    continue;
                }
                //查询ip地址的对应信息
                var setting = ScaleSettingsService.Find(o => o.IpAddress == item);
                if (setting == null)
                {
                    sb.Append("未找到ip:" + item + "对应的配置信息！");
                    continue;
                }
                else
                {
                    if (setting.PageModel == 1)
                    {
                        //判断商品数量与配置是否冲突 //重新选商品的判断
                        Regex rg = new Regex(",");
                        MatchCollection mc = rg.Matches(ids);
                        if (mc.Count > setting.KeyCount * setting.PageModel)
                        {
                            sb.Append("电子秤" + setting.Title + "的商品数量不能超过" + setting.KeyCount * setting.PageModel);
                            continue;
                        }
                    }
                }
            }
            if (sb.Length > 0)
            {
                return OpResult.Fail(sb.ToString());
            }

            //查数据
            List<VwProduct> datas = new List<VwProduct>();
            //if (type == 1)
            //{
            if (string.IsNullOrEmpty(ids))
            {
                return OpResult.Fail("请选择传秤商品！");
            }
            datas = BaseService<VwProduct>.FindList(o => o.CompanyId == CommonService.CompanyId && ("," + ids + ",").Contains("," + o.Id + ","));
            //}
            //else
            //{
            //    var hd = BaseService<ScaleHistory>.FindList(o => o.CompanyId == CommonService.CompanyId && o.BatchSN == batch).Select(o => o.Barcode);
            //    var productcodes = new StringBuilder();
            //    foreach (var item in hd)
            //    {
            //        productcodes.Append(item + ",");
            //    }
            //    var ps = productcodes.ToString();
            //    datas = BaseService<VwProduct>.FindList(o => o.CompanyId == CommonService.CompanyId && ("," + ps + ",").Contains("," + o.Barcode + ","));
            //}
            if (datas == null || datas.Count == 0)
            {
                return OpResult.Fail("数据异常，请重试！");
            }
            SetSysPrice<VwProduct>(Sys.SysCommonRules.CurrentStore, datas);

            //传秤

            //数据格式转换
            JHScaleService service = new JHScaleService();
            //取字典单位信息
            var units = BaseService<SysDataDictionary>.FindList(o => o.CompanyId == CommonService.CompanyId && o.DicPSN == 4);

            var formatResult = service.DataFormat(datas, units);
            //数据发送
            var result = service.TransferData(formatResult, ips, isClear);
            if (result.Successed)
            {
                //批次号
                var barchsn = DateTime.Now.ToString("yyyyMMddHHmmss");
                //计入历史批次
                foreach (var item in datas)
                {
                    ScaleHistory h = new ScaleHistory()
                    {
                        CompanyId = CommonService.CompanyId,
                        Store = Sys.SysCommonRules.CurrentStore,
                        BatchSN = barchsn,
                        Barcode = item.Barcode,
                        Title = item.Title,
                        SysPrice = item.SysPrice,
                        SubUnitId = item.SubUnitId,
                        CreateDt = DateTime.Now,
                        CreateUID = Sys.CurrentUser.UID
                    };
                    ScaleHistoryService.Add(h);
                }

                ScaleHistoryService.Update(new ScaleHistory());

                return OpResult.Success("操作成功!");
            }
            else
            {
                return result;
            }
        }
    }

    public class ProductDto
    {
        public string Barcode { get; set; }
        public string ProductCode { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public string Unit { get; set; }
        public decimal SysPrice { get; set; }
        public string ExportTitle { get; set; }
        public bool IsWeigh { get; set; }
    }

    public class VwProductModel : BaseProduct
    {
        //    public int Id { get; set; }
        //    public string ProductCode { get; set; }
        public string CategoryTitle { get; set; }
        //    public string Title { get; set; }
        //    public string Size { get; set; }
        public string BrandTitle { get; set; }
        //    public decimal SysPrice { get; set; }
        public string SubUnit { get; set; }
        public string Factory { get; set; }
        public string CityTitle { get; set; }
        public int? PrintCount { get; set; }//打印张数

        public string SysPriceStr { get; set; }
    }

}
