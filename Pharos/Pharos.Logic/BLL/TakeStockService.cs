﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Pharos.Logic.Entity;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using Pharos.Sys.BLL;
using System.Web;
using Pharos.Logic.DAL;
namespace Pharos.Logic.BLL
{
    public class TakeStockService : BaseService<StockTaking>
    {
        readonly static CommonDAL dal = new CommonDAL();

        public static DataTable FindPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount, ref object footer, bool ispage = true)
        {
            var nl=new System.Collections.Specialized.NameValueCollection(){nvl};
            if(!nl["categorysn"].IsNullOrEmpty())
            {
                var childs= ProductCategoryService.GetChildSNs(new List<int>() { int.Parse( nl["categorysn"]) });
                nl["categorysn"] = string.Join(",", childs);
            }
            if(!ispage)
            {
                nl["rows"] = int.MaxValue.ToString();
            }
            var ds = dal.FindTakeStockPages(nl, out recordCount);
            //var num = dt.AsEnumerable().Sum(o => o["ActualNumber"].ToType<decimal>());
            decimal ActualNumber = 0, LockNumber=0;
            var dt = ds.Tables[0];
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count>0)
            {
                var dttotal = ds.Tables[1];
                ActualNumber = dttotal.Rows[0]["ActualNumbers"].ToType<decimal>();
                LockNumber = dttotal.Rows[0]["LockNumbers"].ToType<decimal>();
            }
            footer = new List<object>() { 
                new {
                    SubUnit="合计:",
                    ActualNumber=Math.Round( ActualNumber,3),
                    LockNumber=Math.Round( LockNumber,3)
                }
            };
            return dt;
            /*var storeId = nvl["storeId"];
            var userId = nvl["userId"];
            var date1 = nvl["date1"].IsNullOrEmpty()?new Nullable<DateTime>():DateTime.Parse(nvl["date1"]);
            var date2 = nvl["date2"].IsNullOrEmpty()?new Nullable<DateTime>():DateTime.Parse(nvl["date2"]).AddDays(1);
            var searchText = nvl["searchText"];
            var express = DynamicallyLinqHelper.True<TreasuryLocks>().And(o => o.LockStoreID == storeId, storeId.IsNullOrEmpty())
                .And(o => o.LockDate >= date1, !date1.HasValue).And(o => o.LockDate < date2, !date2.HasValue);
            var queryLock = BaseService<TreasuryLocks>.CurrentRepository.QueryEntity.Where(express);
            var queryWare = WarehouseService.CurrentRepository.QueryEntity;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity;
            var queryStock= CurrentRepository.QueryEntity;
            if (!userId.IsNullOrEmpty())
                queryStock = queryStock.Where(o => o.CheckUID == userId);
            var queryUser = UserInfoService.CurrentRepository.QueryEntity;
            var query = from a in queryStock
                        join b in queryLock on a.CheckBatch equals b.CheckBatch
                        join c in queryWare on b.LockStoreID equals c.StoreId
                        join d in queryProduct on a.Barcode equals d.Barcode
                        join e in queryUser on a.CheckUID equals e.UID into g
                        join e in queryUser on a.CreateUID equals e.UID into h
                        from f in g.DefaultIfEmpty()
                        from y in h.DefaultIfEmpty()
                        let o=from x in BaseService<StockTakingLog>.CurrentRepository.QueryEntity where x.CheckBatch==a.CheckBatch && x.Barcode==a.Barcode && x.State==0 select (decimal?)x.Number
                        where b.State==0
                        select new
                        {
                            a.Id,
                            b.CheckBatch,
                            StoreTitle = c.Title,
                            b.LockDate,
                            d.ProductCode,
                            d.Barcode,
                            d.Title,
                            d.BrandTitle,
                            d.SubUnit,
                            ActualNumber=a.ActualNumber??o.Sum(),
                            f.FullName,
                            CreateName=y.FullName,
                            d.CategorySN,
                            a.LockNumber
                        };
            if(!searchText.IsNullOrEmpty())
                query=query.Where(o=>(o.CheckBatch.StartsWith(searchText) || o.Barcode.StartsWith(searchText) || o.Title.Contains(searchText)));
            var ls = new List<object>();
            recordCount = 0;
            if (ispage)
            {
                recordCount = query.Count();
                var list = query.ToPageList();
                ls.AddRange(list);
            }
            else
            {
                ls.AddRange(query.ToList());
            }
            return ls;*/
        }
        public static object FindStockList(string storeId, string batch)
        {
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o=>o.CompanyId==CommonService.CompanyId);
            var queryLock = BaseService<TreasuryLocks>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var query = from a in CurrentRepository.QueryEntity
                        join b in queryLock on a.CheckBatch equals b.CheckBatch
                        join d in queryProduct on a.Barcode equals d.Barcode
                        where a.CheckBatch == batch && b.LockStoreID == storeId && a.CompanyId==CommonService.CompanyId           
                        orderby d.CategorySN ascending
                        select new
                        {
                            a.Id,
                            a.Barcode,
                            d.Title,
                            d.SubUnit,
                            a.ActualNumber,
                            a.LockNumber
                        };
            var list = query.ToList();
            return list;
        }
        public static IEnumerable<dynamic> GetProductInput(string searchName, string checkBatch, string storeId, short? state)
        {
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var query = from d in queryProduct
                        where CurrentRepository.QueryEntity.Any(o=>o.CheckBatch==checkBatch && o.Barcode==d.Barcode && o.CompanyId == CommonService.CompanyId) 
                        && (d.Title.Contains(searchName) || d.Barcode.Contains(searchName) || (d.Barcodes != null && d.Barcodes.Contains(searchName)))
                        && d.CompanyId == CommonService.CompanyId
                        select d;
            if(state.HasValue)
            {
                query = from d in query
                        where !BaseService<StockTakingLog>.CurrentRepository.QueryEntity.Any(o => o.CompanyId == d.CompanyId && o.CheckBatch == checkBatch && o.Barcode == d.Barcode
                        && o.State == 1)
                        select d;
            }
            var list = query.Take(20).ToList();
            ProductService.SetSysPrice(storeId, list);
            //list = ProductService.SetAssistBarcode(list);
            return list.Select(o=> new { o.Barcode,o.Title,o.SubUnit,o.SysPrice,o.CategoryTitle
                ,InitNumber = BaseService<StockTakingLog>.CurrentRepository.QueryEntity.Where(i=>i.CompanyId==CommonService.CompanyId && i.CheckBatch==checkBatch && i.Barcode==o.Barcode && i.State==0).Sum(i=>(decimal?)i.Number).GetValueOrDefault()
            });
        }
        public static object FindBarcodesByBatch(string batch,short containReTake=0)
        {
            if (containReTake == 0)
            {
                var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o=>o.CompanyId==CommonService.CompanyId);
                var queryLog = BaseService<StockTakingLog>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
                var query = from a in CurrentRepository.QueryEntity
                            join d in queryProduct on a.Barcode equals d.Barcode
                            let o = from e in queryLog where a.CheckBatch == e.CheckBatch && a.Barcode == e.Barcode && e.State == 1 select e.Barcode
                            where a.CheckBatch == batch && !o.Contains(a.Barcode) && a.CompanyId==CommonService.CompanyId
                            orderby d.CategorySN ascending
                            select new
                            {
                                a.Id,
                                a.Barcode,
                                d.Title,
                                d.SubUnit,
                                a.ActualNumber,
                                a.LockNumber
                            };
                var list = query.ToList();
                return list;
            }
            else
            {
                var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
                var query = from a in CurrentRepository.QueryEntity
                            join d in queryProduct on a.Barcode equals d.Barcode
                            where a.CheckBatch == batch && a.CompanyId==CommonService.CompanyId
                            orderby d.CategorySN ascending
                            select new
                            {
                                a.Id,
                                a.Barcode,
                                d.Title,
                                d.SubUnit,
                                a.ActualNumber,
                                a.LockNumber
                            };
                var list = query.ToList();
                return list;
            }
        }
        public static object FindStockLogList(string batch, string checkUID,string state)
        {
            if (batch.IsNullOrEmpty()) return null;
            var rows= stockCache.Get(CacheKey(batch));
            if (!rows.IsNullOrEmpty() && state=="0")
            {
                return rows.JsonToDataTable();
            }
            var list = dal.FindStockLogs(CommonService.CompanyId, batch,"",state);
            return list;
        }
        public static OpResult AddStockLock(TreasuryLocks obj)
        {
            var op = new OpResult();
            try
            {
                obj.LockStoreID.IsNullThrow();
                var xh = obj.CheckBatch.Substring(obj.CheckBatch.Length - 2);
                if(int.Parse(xh)>20)
                {
                    op.Message = "每月最多只能锁定20次"; return op;
                }
                var selectBarcodes = HttpContext.Current.Request["selectBarcodes"];
                var barcodes = new string[] { };
                if (selectBarcodes.IsNullOrEmpty())
                {
                    List<int> childsns = null;
                    if (!obj.LockCategorySN.IsNullOrEmpty())
                    {
                        var parsns = obj.LockCategorySN.Split(',').Select(o => int.Parse(o)).ToList();
                        childsns = ProductCategoryService.GetChildSNs(parsns, true);
                    }
                    else
                    {
                        var ware = WarehouseService.Find(o => o.StoreId == obj.LockStoreID && o.CompanyId == CommonService.CompanyId);
                        var parsns = ware.CategorySN.Split(',').Where(o => !o.IsNullOrEmpty()).Select(o => int.Parse(o)).ToList();
                        childsns = ProductCategoryService.GetChildSNs(parsns);
                    }
                    barcodes = ProductService.FindList(o => childsns.Contains(o.CategorySN) && o.CompanyId == CommonService.CompanyId).Select(o => o.Barcode).Distinct().ToArray();
                }
                else
                    barcodes = selectBarcodes.Split(',');

                var dt = dal.GetInventoryBalanceLast(CommonService.CompanyId,obj.LockStoreID, string.Join(",", barcodes));
                if (dt==null || dt.Rows.Count<=0)
                    op.Message = "该门店暂无库存信息";
                else
                {
                    obj.LockDate = DateTime.Now;
                    obj.LockUID = Sys.CurrentUser.UID;
                    obj.CompanyId = CommonService.CompanyId;
                    var stocks = new List<StockTaking>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (stocks.Any(o => o.Barcode == dr["Barcode"].ToString())) continue;
                        stocks.Add(new StockTaking() {
                            Barcode = dr["Barcode"].ToString(),
                            LockNumber = dr["Number"].ToType<decimal>(),
                            CheckBatch = obj.CheckBatch,
                            CreateDT = obj.LockDate,
                            CreateUID = obj.LockUID,
                            CompanyId = obj.CompanyId
                        });
                    }
                    BaseService<TreasuryLocks>.Add(obj, false);
                    op = AddRange(stocks);
                }
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static OpResult SaveOrUpdate(TreasuryLocks obj, string updated, string inserted, string deleted, string actualDate, short state = 1, short hasrepeat = 1)//hasrepeat=1?覆盖:累加
        {
            var op = new OpResult();
            try
            {
                var updateStocks = updated.ToObject<List<StockTakingLog>>().Where(o => !o.Barcode.IsNullOrEmpty());
                var insertStocks = inserted.ToObject<List<StockTakingLog>>().Where(o => !o.Barcode.IsNullOrEmpty());
                var deleteStocks = deleted.ToObject<List<StockTakingLog>>().Where(o => !o.Barcode.IsNullOrEmpty());
                if (!updateStocks.Any() && !insertStocks.Any() && !deleteStocks.Any()) throw new Exception("无更新记录!");
                var barcodes = insertStocks.Select(o => o.Barcode).ToList();
                var stocks = FindList(o => barcodes.Contains(o.Barcode) && o.CheckBatch==obj.CheckBatch && o.CompanyId==CommonService.CompanyId);
                var errls = new Dictionary<int, string>();
                var stocklogs = BaseService<StockTakingLog>.FindList(o => barcodes.Contains(o.Barcode) && o.CheckBatch == obj.CheckBatch && o.State == 1 && o.CompanyId == CommonService.CompanyId);
                if(state==0)
                {
                    var reslogs = BaseService<StockTakingLog>.FindList(o => o.CheckBatch == obj.CheckBatch && o.State == 0 && o.Source!=2 && o.CompanyId == CommonService.CompanyId && o.CreateUID==Sys.CurrentUser.UID);
                    BaseService<StockTakingLog>.CurrentRepository.RemoveRange(reslogs, false);//先删除原先记录
                }
                var stockTakings = new List<StockTaking>();
                if (state == 1 && insertStocks.Any())
                {
                    var bars = insertStocks.Select(o => o.Barcode).ToList();
                    stockTakings = BaseService<StockTaking>.FindList(o => o.CompanyId == CommonService.CompanyId && o.CheckBatch == obj.CheckBatch && bars.Contains(o.Barcode));
                }
                var date = actualDate.ToType<DateTime>();
                foreach (var add in insertStocks)
                {
                    //var sk = stocks.FirstOrDefault(o => o.Barcode == add.Barcode);
                    var q = from a in CurrentRepository.QueryEntity
                            join b in ProductService.CurrentRepository.QueryEntity on new { a.CompanyId, a.Barcode } equals new { b.CompanyId, b.Barcode }
                            where a.CompanyId == CommonService.CompanyId && a.CheckBatch == obj.CheckBatch 
                                && ((","+b.Barcodes+",").Contains(","+add.Barcode+",") || b.Barcode == add.Barcode)
                            select a;
                    if (!q.Any())
                    {
                        errls.Add(add.Id, add.Barcode + "&nbsp;&nbsp;条码不存在");
                        continue;
                    }
                    if (state==1 && stocklogs.Any(o=>o.Barcode==add.Barcode) && add.State==0)//复盘不验证
                    {
                        errls.Add(add.Id, add.Barcode + "&nbsp;&nbsp;条码已复盘");
                        continue;
                    }
                    add.CreateUID = Sys.CurrentUser.UID;
                    add.CheckBatch = obj.CheckBatch;
                    //add.CreateDT = DateTime.Now;
                    add.ActualDate = date;
                    add.CompanyId = CommonService.CompanyId;
                    if (add.Source <= 0)
                        add.Source = 1;
                    BaseService<StockTakingLog>.CurrentRepository.Add(add, false);
                    var stock= stockTakings.FirstOrDefault(o=>o.Barcode == add.Barcode);
                    if(stock!=null && stock.LockNumber!= add.Number)
                        stock.Sure=2;
                }
                if(updateStocks.Any())
                {
                    barcodes= updateStocks.Select(o => o.Barcode).ToList();
                    stocklogs = BaseService<StockTakingLog>.FindList(o => barcodes.Contains(o.Barcode) && o.CheckBatch == obj.CheckBatch && o.CompanyId == CommonService.CompanyId);
                    foreach(var update in updateStocks)
                    {
                        var log= stocklogs.FirstOrDefault(o => o.Id == update.Id);
                        if (log == null) continue;
                        log.Number = update.Number;
                    }
                }
                if(deleteStocks.Any())
                {
                    var ids = deleteStocks.Select(o => o.Id).ToList();
                    stocklogs= BaseService<StockTakingLog>.FindList(o => ids.Contains(o.Id));
                    BaseService<StockTakingLog>.CurrentRepository.RemoveRange(stocklogs, false);
                }
                op = BaseService<StockTakingLog>.Update(stocklogs);
                op.Data = errls;
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static OpResult ReSave(StockTaking obj)
        {
            var op=new OpResult();
            var stock = CurrentRepository.FindById(obj.Id);
            stock.ActualNumber =Math.Round(obj.ActualNumber.GetValueOrDefault(),3);
            stock.CreateUID = Sys.CurrentUser.UID;
            var CheckUID=HttpContext.Current.Request["CheckUID"];
            var log = new StockTakingLog() {
                Barcode = stock.Barcode,
                CheckBatch = stock.CheckBatch,
                CheckUID = CheckUID,
                CreateUID=stock.CreateUID,
                CreateDT=DateTime.Now,
                Number=stock.ActualNumber.GetValueOrDefault(),
                State=1,
                ActualDate=HttpContext.Current.Request["ActualDate"].ToType<DateTime>(),
                CompanyId=CommonService.CompanyId,
                Source=1
            };
            op = BaseService<StockTakingLog>.Add(log);
            return op;
        }
        public static OpResult CrrectSave(int id, decimal crrectNumber)
        {
            var stock = FindById(id);
            //stock.CorrectNumber = crrectNumber;
            return Update(stock);
        }
        public static bool AutoInventoryBalance()
        {
            return dal.AutoInventoryBalance();
        }
        public static OpResult ApprovalPass(string checkBatch)
        {
            var op = new OpResult();
            var tl = BaseService<TreasuryLocks>.Find(o => o.CheckBatch == checkBatch && o.CompanyId==CommonService.CompanyId);
            if (tl.State == 1)
            {
                op.Message = "该批次已审核通过！";
                return op;
            }
            tl.State = 1;
            var stocks= FindList(o => o.CheckBatch == checkBatch && o.CompanyId==tl.CompanyId);//经过纠正记录
            stocks.Each(o => o.Sure = 1);
            var nvl= new System.Collections.Specialized.NameValueCollection();
            nvl.Add("checkBatch", checkBatch);
            nvl.Add("dispType", "1");//差异则纠正
            nvl.Add("state", "0");
            nvl["rows"] = int.MaxValue.ToString();
            int count=0;
            var dt= dal.FindTakeStockPages(nvl, out count).Tables[0];
            
            var logs = new List<InventoryRecord>();
            if (dt!=null && dt.Rows.Count>0)
            {
                var result = AutoInventoryBalance();
                var bars=string.Join(",",dt.AsEnumerable().Select(o=>o["Barcode"]));
                var outInNums = dal.FindOutInNumDetails(bars, tl.LockDate,dt.Rows[0]["LockStoreId"].ToString(),CommonService.CompanyId);
                 //var barcodes = dt.AsEnumerable().Select(o => o["barcode"].ToString()).ToList();
                 //var invents = BaseService<Inventory>.FindList(o => barcodes.Contains(o.Barcode) && o.StoreId == tl.LockStoreID && o.CompanyId == CommonService.CompanyId);
                //var commoditys = dal.GetInventoryBalanceLast(tl.LockStoreID, string.Join(",", barcodes));
                foreach (DataRow dr in dt.Rows)
                {
                    var barcode= dr["Barcode"].ToString();
                    var actualNumber = dr["ActualNumber"].ToType<decimal>();//盘点数量
                    if(outInNums!=null && outInNums.Rows.Count>0)
                    {
                        var where = string.Format("Barcode='{0}' and StoreId='{1}'", barcode,dr["LockStoreId"]);
                        var drs= outInNums.Select(where);
                        var num= drs.Sum(o => (decimal?)o["Num"]);
                        if (num.HasValue) actualNumber += num.Value;//加上盘点之后未审核有库存操作的数量
                    }
                    //var iy = invents.FirstOrDefault(o => o.Barcode == barcode);
                    //decimal number = 0;
                    //var commd= commoditys.FirstOrDefault(o => o.Barcode == barcode);
                    //if (commd != null) number = commd.Number;
                    //if (iy == null)
                    //{
                    //    iy=new Inventory()
                    //    {
                    //        StoreId = tl.LockStoreID,
                    //        Barcode = barcode,
                    //        StockNumber = actualNumber,
                    //        CompanyId=CommonService.CompanyId
                    //    };
                    //    BaseService<Inventory>.Add(iy, false);
                    //}
                    //else
                    //{
                    //    iy.StockNumber = actualNumber;
                    //}
                    logs.Add(new InventoryRecord()
                    {
                        Barcode = barcode,
                        CreateDT = DateTime.Now,
                        CreateUID = Sys.CurrentUser.UID,
                        Number =actualNumber, //actualNumber - number,
                        StoreId = tl.LockStoreID,
                        Source = 15,
                        OperatId=checkBatch,
                        CompanyId=CommonService.CompanyId
                    });
                }
            }
            op=BaseService<TreasuryLocks>.Update(tl);
            if (op.Successed)
            {
                InventoryRecordService.SaveLog(logs);
                Log.WriteInfo("库存纠正：" + logs.ToJson());
            }
            return op;
        }
        public static object GetObjByid(int id)
        {
            /*var queryLock = BaseService<TreasuryLocks>.CurrentRepository.QueryEntity;
            var queryWare = WarehouseService.CurrentRepository.QueryEntity;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity;
            var queryUser = UserInfoService.CurrentRepository.QueryEntity;
            var query = from a in CurrentRepository.QueryEntity
                        join b in queryLock on a.CheckBatch equals b.CheckBatch
                        join c in queryWare on b.LockStoreID equals c.StoreId
                        join d in queryProduct on a.Barcode equals d.Barcode
                        where a.Id==id
                        select new
                        {
                            a.Id,
                            StoreTitle = c.Title,
                            b.LockStoreID,
                            b.LockDate,
                            a.CheckBatch,
                            d.Barcode,
                            d.Title,
                            d.SubUnit,
                            a.ActualNumber
                        };
            return query.FirstOrDefault();*/
            var nl = new System.Collections.Specialized.NameValueCollection();
            nl.Add("id", id.ToString());
            int count=0;
            var dt= dal.FindTakeStockPages(nl, out count).Tables[0];
            var obj= dt.AsEnumerable().Select(dr => new { 
                Id=dr["Id"],
                StoreTitle = dr["StoreTitle"],
                LockStoreID = dr["LockStoreID"],
                LockDate = dr["LockDate"] is DBNull ? DateTime.Now.ToString("yyyy-MM-dd") : dr["LockDate"].ToString(),
                CheckBatch = dr["CheckBatch"],
                Barcode = dr["Barcode"],
                Title = dr["Title"],
                SubUnit = dr["SubUnit"],
                ActualNumber = dr["ActualNumber"],
                LockNumber = dr["LockNumber"],
                CheckUID = dr["CheckUID"],
                FullName = dr["FullName"],
                CorrectNumber = dr["CorrectNumber"],
                InitNumber = dr["InitNumber"].ToType<decimal>().ToAutoString(3)
            }).FirstOrDefault();
            return obj;
        }
        /// <summary>
        /// 获取批次
        /// </summary>
        /// <param name="storeId">门店</param>
        /// <returns></returns>
        public static string GetBatchAndCategory(string storeId)
        {
            var obj = WarehouseService.Find(o => o.StoreId == storeId && o.CompanyId==CommonService.CompanyId);
            var prefix = obj.StoreId.PadLeft(2,'0') + DateTime.Now.ToString("yyMMdd");
            var max = BaseService<TreasuryLocks>.CurrentRepository.QueryEntity.Where(o => o.CheckBatch.StartsWith(prefix) && o.CompanyId == CommonService.CompanyId).Max(o => o.CheckBatch);

            var sns = obj.CategorySN.Split(',').Where(o => !o.IsNullOrEmpty()).Select(o => int.Parse(o)).ToList();
            var items = ProductCategoryService.GetParentCategorys(false).Select(o => new DropdownItem(o.CategorySN.ToString(), o.Title)).ToList();
            items.Insert(0, new DropdownItem("", "请选择"));
            int num = 1;
            if (!max.IsNullOrEmpty())
                num = int.Parse(max.Replace(prefix, "")) + 1;
            var msg = new Pharos.Logic.DAL.CommonDAL().StockLockValidMsg(storeId);
            var item=new { batch=prefix + num.ToString("00"),items=items,msg=msg};
            return item.ToJson();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="httpFiles"></param>
        /// <param name="codeCol">条码列</param>
        /// <param name="countCol">实盘列</param>
        /// <param name="minRow">起始行</param>
        /// <param name="maxRow">截止行</param>
        /// <returns></returns>
        public static OpResult Import(TreasuryLocks obj, System.Web.HttpFileCollectionBase httpFiles, char codeCol, char countCol, int minRow, int maxRow,string checkUID)
        {
            var op = new OpResult();
            try
            {
                if (httpFiles.Count <= 0 || httpFiles[0].ContentLength <= 0)
                {
                    op.Message = "请先选择Excel文件";
                    return op;
                }
                var stream = httpFiles[0].InputStream;
                var ext= httpFiles[0].FileName.Substring(httpFiles[0].FileName.LastIndexOf("."));
                if(!(ext.Equals(".xls",StringComparison.CurrentCultureIgnoreCase) || 
                    ext.Equals(".xlsx",StringComparison.CurrentCultureIgnoreCase)))
                {
                    op.Message = "请先选择Excel文件";
                    return op;
                }
                var over = HttpContext.Current.Request["over"].ToType<short?>();
                int? checkCol = null;
                if (!HttpContext.Current.Request["CheckCol"].IsNullOrEmpty())
                    checkCol =(int)Convert.ToChar(HttpContext.Current.Request["CheckCol"]);
                obj.CompanyId = CommonService.CompanyId;
                var dt = new ExportExcel().ToDataTable(stream, minRow: minRow, maxRow: maxRow);
                var codeIdx = Convert.ToInt32(codeCol)-65;
                var countIdx = Convert.ToInt32(countCol) - 65;
                var users=new List<Sys.Entity.SysUserInfo>();
                if(checkCol.HasValue)
                {
                    var codes= dt.AsEnumerable().Select(o => o[checkCol.Value - 65].ToString()).Distinct().ToList();
                    users= UserInfoService.FindList(o => o.CompanyId == CommonService.CompanyId && codes.Contains(o.UserCode));
                }
                var stocks=new List<StockTakingLog>();
                var errLs = new Dictionary<int,string>();
                int idx = 1;
                foreach(DataRow dr in dt.Rows)
                {
                    idx++;
                    string barcode = "", number="";
                    try
                    {
                        barcode = dr[codeIdx].ToString();
                        number = dr[countIdx].ToString();
                        if(checkCol.HasValue)
                        {
                             var code= dr[checkCol.Value - 65].ToString();
                             if (!code.IsNullOrEmpty())
                             {
                                 var user = users.FirstOrDefault(o => o.UserCode == code);
                                 if (user != null)
                                     checkUID = user.UID;
                                 else
                                 {
                                     errLs.Add(idx, barcode + "&nbsp;&nbsp;盘点员工号不存在！");
                                     continue;
                                 }
                             }
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("列选择超过范围!");
                    }
                    if (barcode.IsNullOrEmpty())
                    {
                        continue;
                    }
                    if(number.IsNullOrEmpty() && !barcode.IsNullOrEmpty())
                    {
                        errLs.Add(idx, barcode + "&nbsp;&nbsp;实盘数量为空");
                        continue;
                    }
                    decimal num=0;
                    if (!decimal.TryParse(number, out num) || num < 0)
                    {
                        errLs.Add(idx, barcode + "&nbsp;&nbsp;实盘数量小于零");
                        continue;
                    }
                    
                    var st= stocks.FirstOrDefault(o=>o.Barcode==barcode);
                    if(st!=null) 
                        st.Number+=num;
                    else
                        stocks.Add(new StockTakingLog()
                        {
                            Id=idx,
                            Barcode=barcode,
                            Number = num,
                            CheckBatch=obj.CheckBatch,
                            CheckUID=checkUID,
                            CreateDT=DateTime.Now,
                            State=over.GetValueOrDefault(),
                            SysPrice=0,
                            CompanyId=obj.CompanyId,
                            Source=3
                        });
                }
                op = SaveOrUpdate(obj,"[]",stocks.ToJson(),"[]",HttpContext.Current.Request["ActualDate"],1);
                var errs=op.Data as Dictionary<int,string>;
                if (errs == null) errs = new Dictionary<int, string>();
                foreach (var de in errs)
                    errLs.Add(de.Key, de.Value);
                if (errLs.Any())
                {
                    var html = "<ul><li>成功导入{0}条数据,余{1}条导入失败!</li><li><a href=\"javascript:void(0)\" onclick=\"viewErr()\">查看失败记录!</a></li></ul>";
                    op.Message = string.Format(html, stocks.Count-errs.Count, errLs.Count);
                    op.Descript = "<dl><dt>以下数据导入失败：</dt>{0}</dl>";
                    string str = "";
                    foreach (var de in errLs)
                    {
                        str += "<dd>行" + de.Key + ":" + de.Value + "</dd>";
                    }
                    op.Descript = string.Format(op.Descript, str);
                }
                else
                    op.Message = "<ul><li>成功导入" + stocks.Count + "条数据!</li></ul>";
                op.Message = System.Web.HttpUtility.UrlEncode(op.Message);
                op.Descript = System.Web.HttpUtility.UrlEncode(op.Descript);
                Log.WriteInsert("盘点导入", Sys.LogModule.库存管理);
            }
            catch(Exception ex)
            {
                op.Message=ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static IEnumerable<DropdownItem> GetCheckerByBatchnoList(string batchno)
        {
            var query = from o in BaseService<Entity.StockTakingLog>.CurrentRepository.QueryEntity
                        where o.CompanyId == Sys.SysCommonRules.CompanyId && o.CheckBatch == batchno && !(o.CheckUID == null || o.CheckUID == "")
                        select new DropdownItem()
                        {
                            Value= o.CheckUID,
                            Text= UserInfoService.CurrentRepository.QueryEntity.Where(i => i.UID == o.CheckUID).Select(i => i.FullName).FirstOrDefault()
                        };
            var list = query.Distinct().ToList();
            return list;
        }
        public static IEnumerable<dynamic> TakeStockLogs(string checkBatch, string barcode)
        {
            var query = BaseService<StockTakingLog>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == Sys.SysCommonRules.CompanyId && o.CheckBatch == checkBatch && o.Barcode == barcode);
            var q = from x in query
                    orderby x.CreateDT
                    select new
                    {
                        x.CreateDT,
                        x.CompanyId,
                        x.Barcode,
                        x.CheckBatch,
                        x.Number,
                        Checker = UserInfoService.CurrentRepository.QueryEntity.Where(o => o.UID == x.CheckUID).Select(o => o.FullName).FirstOrDefault(),
                        StateTitle = x.State == 1 ? "复盘" : "初盘",
                        SourceTitle=x.Source==2?"手机":x.Source==3?"导入":"电脑"
                    };
            return q.ToList().Select(x => new
            {
                CreateDT = x.CreateDT.ToString("yyyy-MM-dd HH:mm:ss"),
                x.CompanyId,
                x.Barcode,
                x.CheckBatch,
                Number=Math.Round(x.Number,3),
                x.Checker,
                x.StateTitle,
                x.SourceTitle
            });
        }
        static readonly StockTakingCache stockCache = new StockTakingCache();
        public static void PostCache(string rows, string checkBatch)
        {
            stockCache.Set(CacheKey(checkBatch), rows);
        }
        public static void RemoveCache(string checkBatch)
        {
            stockCache.Remove(CacheKey(checkBatch));
        }
         static string CacheKey(string checkBatch)
        {
            return Pharos.Sys.CurrentUser.UID + "_" + checkBatch+"_"+CommonService.CompanyId;
        }
         public static DataTable ReportList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount, ref object footer, bool isPage = true)
         {
            var nl = new System.Collections.Specialized.NameValueCollection() { nvl };
            if (!nl["categorysn"].IsNullOrEmpty())
            {
                var childs = ProductCategoryService.GetChildSNs(new List<int>() { int.Parse(nl["categorysn"]) });
                nl["categorysn"] = string.Join(",", childs);
            }
            if (!isPage)
            {
                nl["rows"] = int.MaxValue.ToString();
            }
            var ds = dal.FindTakeStockPages(nl, out recordCount);
            var dt = ds.Tables[0];
            decimal ActualNumber = 0, LockNumber = 0, SubstractNum=0,SubstractTotal = 0, ActualTotal = 0;
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count>0)
            {
                var dttotal = ds.Tables[1];
                ActualNumber =Math.Round( dttotal.Rows[0]["ActualNumbers"].ToType<decimal>(),3);
                LockNumber = Math.Round( dttotal.Rows[0]["LockNumbers"].ToType<decimal>(),3);
                SubstractNum = Math.Round( dttotal.Rows[0]["SubstractNums"].ToType<decimal>(),3);
                SubstractTotal = Math.Round( dttotal.Rows[0]["SubstractTotals"].ToType<decimal>(),3);
                ActualTotal = Math.Round( dttotal.Rows[0]["ActualTotals"].ToType<decimal>(),3);
            }
            footer = new List<object>() { 
                new {ActualNumber=ActualNumber,LockNumber=LockNumber,SubstractNum=SubstractNum,SubstractTotal=SubstractTotal,ActualTotal=ActualTotal, SubUnit="合计:"}
            };
            return dt;
            /*
            var query = from a in CurrentRepository.Entities
                        join b in BaseService<VwProduct>.CurrentRepository.Entities on a.Barcode equals b.Barcode
                        join c in BaseService<TreasuryLocks>.CurrentRepository.Entities on a.CheckBatch equals c.CheckBatch
                        join d in WarehouseService.CurrentRepository.Entities.DefaultIfEmpty() on c.LockStoreID equals d.StoreId
                        let o = from x in BaseService<StockTakingLog>.CurrentRepository.QueryEntity where x.CheckBatch == a.CheckBatch && x.Barcode == a.Barcode && x.State == 0 select (decimal?)x.Number
                        //where c.State==1
                        orderby c.CheckBatch descending, b.CategorySN ascending
                        select new
                        {
                            a.Id,
                            a.CheckBatch,
                            c.LockStoreID,
                            StoreTitle= d.Title,
                            c.LockDate,
                            Pro=b,
                            a.LockNumber,
                            ActualNumber=a.ActualNumber??o.Sum(),
                            b.SysPrice,
                            BuyTotal="",
                            c.State
                        };
            if (!storeId.IsNullOrEmpty())
                query = query.Where(o => o.LockStoreID == storeId);
            if(!checkBatch.IsNullOrEmpty())
                query = query.Where(o => o.CheckBatch == checkBatch);
||||||| .r6102
        public static DataTable ReportList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount, bool isPage = true)
        {
            return dal.FindTakeStockPages(nvl, out recordCount, isPage);
            /*
            var query = from a in CurrentRepository.Entities
                        join b in BaseService<VwProduct>.CurrentRepository.Entities on a.Barcode equals b.Barcode
                        join c in BaseService<TreasuryLocks>.CurrentRepository.Entities on a.CheckBatch equals c.CheckBatch
                        join d in WarehouseService.CurrentRepository.Entities.DefaultIfEmpty() on c.LockStoreID equals d.StoreId
                        let o = from x in BaseService<StockTakingLog>.CurrentRepository.QueryEntity where x.CheckBatch == a.CheckBatch && x.Barcode == a.Barcode && x.State == 0 select (decimal?)x.Number
                        //where c.State==1
                        orderby c.CheckBatch descending, b.CategorySN ascending
                        select new
                        {
                            a.Id,
                            a.CheckBatch,
                            c.LockStoreID,
                            StoreTitle= d.Title,
                            c.LockDate,
                            Pro=b,
                            a.LockNumber,
                            ActualNumber=a.ActualNumber??o.Sum(),
                            b.SysPrice,
                            BuyTotal="",
                            c.State
                        };
            if (!storeId.IsNullOrEmpty())
                query = query.Where(o => o.LockStoreID == storeId);
            if(!checkBatch.IsNullOrEmpty())
                query = query.Where(o => o.CheckBatch == checkBatch);
=======
         public static DataTable ReportList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount, ref object footer, bool isPage = true)
         {
             var dt = dal.FindTakeStockPages(nvl, out recordCount, isPage);
             int countNotPage = 0;
             var dtNotPage = dal.FindTakeStockPages(nvl, out countNotPage, false);
             var lockNumber = dtNotPage.AsEnumerable().Sum(o => o["LockNumber"].ToType<decimal>());
             var actualNumber = dtNotPage.AsEnumerable().Sum(o => o["ActualNumber"].ToType<decimal>());
             var substractNum = dtNotPage.AsEnumerable().Sum(o => o["SubstractNum"].ToType<decimal>());
             var substractTotal = dtNotPage.AsEnumerable().Sum(o => o["SubstractTotal"].ToType<decimal>());
             var actualTotal = dtNotPage.AsEnumerable().Sum(o => o["ActualTotal"].ToType<decimal>());
             footer = new List<object>() { 
                new { 
                    SysPrice="所有合计:",
                    State=-1,
                    LockNumber=lockNumber,
                    ActualNumber=actualNumber,
                    SubstractNum=substractNum,
                    SubstractTotal=substractTotal,
                    ActualTotal=actualTotal
                }
            };
             return dt;

             /*
             var query = from a in CurrentRepository.Entities
                         join b in BaseService<VwProduct>.CurrentRepository.Entities on a.Barcode equals b.Barcode
                         join c in BaseService<TreasuryLocks>.CurrentRepository.Entities on a.CheckBatch equals c.CheckBatch
                         join d in WarehouseService.CurrentRepository.Entities.DefaultIfEmpty() on c.LockStoreID equals d.StoreId
                         let o = from x in BaseService<StockTakingLog>.CurrentRepository.QueryEntity where x.CheckBatch == a.CheckBatch && x.Barcode == a.Barcode && x.State == 0 select (decimal?)x.Number
                         //where c.State==1
                         orderby c.CheckBatch descending, b.CategorySN ascending
                         select new
                         {
                             a.Id,
                             a.CheckBatch,
                             c.LockStoreID,
                             StoreTitle= d.Title,
                             c.LockDate,
                             Pro=b,
                             a.LockNumber,
                             ActualNumber=a.ActualNumber??o.Sum(),
                             b.SysPrice,
                             BuyTotal="",
                             c.State
                         };
             if (!storeId.IsNullOrEmpty())
                 query = query.Where(o => o.LockStoreID == storeId);
             if(!checkBatch.IsNullOrEmpty())
                 query = query.Where(o => o.CheckBatch == checkBatch);
>>>>>>> .r6246
            
             if (isPage)
             {
                 recordCount = query.Count();
                 var list= query.ToPageList();
                 ProductService.SetSysPrice(storeId, list.Select(o => o.Pro).ToList());
                 return list.Select(o=>new{
                     o.Id,
                     o.CheckBatch,
                     o.LockStoreID,
                     o.StoreTitle,
                     o.LockDate,
                     o.LockNumber,
                     o.ActualNumber,
                     SubstractNum=o.ActualNumber-o.LockNumber,
                     SubstractTotal=(o.ActualNumber-o.LockNumber)*o.Pro.BuyPrice,
                     ActualTotal = o.ActualNumber * o.Pro.SysPrice,
                     o.BuyTotal,
                     o.Pro.State,
                     o.Pro.ProductCode,
                     o.Pro.Barcode,
                     o.Pro.Title,
                     o.Pro.CategoryTitle,
                     o.Pro.SubUnit,
                     o.Pro.Size,
                     o.Pro.SupplierTitle,
                     o.Pro.SysPrice
                 });
             }
             else
             {
                 recordCount = 0;
                 var list= query.Where(o=>o.State==1).ToList();
                 ProductService.SetSysPrice(storeId, list.Select(o => o.Pro).ToList());
                 return list.Select(o => new
                 {
                     o.Id,
                     o.CheckBatch,
                     o.LockStoreID,
                     o.StoreTitle,
                     o.LockDate,
                     o.LockNumber,
                     o.ActualNumber,
                     SubstractNum = o.ActualNumber - o.LockNumber,
                     SubstractTotal = (o.ActualNumber - o.LockNumber) * o.Pro.BuyPrice,
                     ActualTotal = o.ActualNumber * o.Pro.SysPrice,
                     o.BuyTotal,
                     o.Pro.State,
                     o.Pro.ProductCode,
                     o.Pro.Barcode,
                     o.Pro.Title,
                     o.Pro.CategoryTitle,
                     o.Pro.SubUnit,
                     o.Pro.Size,
                     o.Pro.SupplierTitle,
                     o.Pro.SysPrice
                 });
             }*/
         }
        class CommodityComp : IEqualityComparer<StockTaking>
        {

            public bool Equals(StockTaking x, StockTaking y)
            {
                return x.Barcode == y.Barcode;
            }

            public int GetHashCode(StockTaking obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}
