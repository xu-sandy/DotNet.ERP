using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Pharos.Logic.ApiData.Mobile.Exceptions;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Logic.BLL;
using Pharos.Logic.DAL;
using Pharos.Logic.Entity;
using Pharos.Logic.ApiData.Pos.ValueObject;

namespace Pharos.Logic.ApiData.Mobile.Services
{
    public class TakeStockService
    {
        static CommonDAL dal = new CommonDAL();
        public static object GetStoreBatchnoList(string storeId)
        {
            if (string.IsNullOrWhiteSpace(storeId))
                throw new MessageException("门店号为空!");
            var list= BaseService<Entity.TreasuryLocks>.FindList(o => o.LockStoreID == storeId && o.State==0 && o.CompanyId==Sys.SysCommonRules.CompanyId).OrderByDescending(o=>o.LockDate).Select(o=>o.CheckBatch).ToList();
            return list;
        }
        public static object GetCheckerByBatchnoList(string batchno)
        {
            if (string.IsNullOrWhiteSpace(batchno))
                throw new MessageException("批次号为空!");

            var list= BLL.TakeStockService.GetCheckerByBatchnoList(batchno);
            return list.Select(o => new { Checker = o.Text, CheckUID=o.Value });
        }
        public static PageResult<object> GetTakestockList(string storeId, string checkBatch, string searchText, string userCode, string dispType, int pageindex, int pagesize,string sure)
        {
            if (string.IsNullOrWhiteSpace(storeId))
                throw new MessageException("门店号为空!");
            if (string.IsNullOrWhiteSpace(checkBatch))
                throw new MessageException("批次为空!");
            
            var nvl = new System.Collections.Specialized.NameValueCollection();
            nvl.Add("storeId", storeId);
            nvl.Add("checkBatch", checkBatch);
            nvl.Add("userId", userCode);
            nvl.Add("sure", sure);
            //nvl.Add("date1", takeDate);
            //nvl.Add("date2", takeDate);
            nvl.Add("searchText", searchText);
            nvl.Add("dispType",dispType);
            nvl.Add("sort", "CategoryTitle asc,Barcode");
            nvl.Add("order", "asc");
            nvl.Add("page", pageindex.ToString());
            nvl.Add("rows", pagesize.ToString());
            int count=0;
            var dt= dal.FindTakeStockList(nvl, out count);
            var result = new PageResult<object>();
            result.Datas =dt.AsEnumerable().Select(o => new
            {
                Barcode=o["Barcode"],
                Title=o["Title"],
                SubUnit=o["SubUnit"],
                CheckBatch=o["CheckBatch"],
                ActualNumber = o["ActualNumber"] is DBNull ? 0 : o["ActualNumber"],
                LockNumber=o["LockNumber"],
                Sure = o["Sure"],
                CreateDT = o["LockDate"],
                CategoryTitle = o["CategoryTitle"]
            });
            var pageCount = count / pagesize + (count % pagesize > 0 ? 1 : 0);
            result.Pager = new PageInfo()
            {
                Index = pageindex,
                Size = pagesize,
                Total = count,
                PageCount = pageCount
            };
            return result;
        }
        public static object GetTakestockBybarcode(string storeId, string checkBatch,string barcode,string userCode)
        {
            if (string.IsNullOrWhiteSpace(storeId))
                throw new MessageException("门店号为空!");
            if (string.IsNullOrWhiteSpace(checkBatch))
                throw new MessageException("批次为空!");
            if (string.IsNullOrWhiteSpace(barcode))
                throw new MessageException("商品条码为空!");
            var nvl = new System.Collections.Specialized.NameValueCollection();
            nvl.Add("storeId", storeId);
            nvl.Add("checkBatch", checkBatch);
            nvl.Add("barcode", barcode);
            nvl.Add("userId", userCode);
            int count = 0;
            var dt = dal.FindTakeStockList(nvl, out count);
            if (dt==null || dt.Rows.Count<=0)
                throw new MessageException("该门店和批次下无此商品条码!");
            var o=dt.Rows[0];
            return new{
                Barcode = o["Barcode"],
                Title = o["Title"],
                SubUnit = o["SubUnit"],
                CheckBatch = o["CheckBatch"],
                Size=o["Size"],
                CategoryTitle = o["CategoryTitle"],
                ActualNumber = o["ActualNumber"] is DBNull ? 0 : o["ActualNumber"],
                CreateDT = o["LockDate"],
                LockNumber = o["LockNumber"],
                Sure = o["Sure"],
                ActualState = o["ActualState"]
            };
        }
        public static object GetTakestockByinputBarcode(string storeId, string checkBatch, string barcode)
        {
            if (string.IsNullOrWhiteSpace(storeId))
                throw new MessageException("门店号为空!");
            if (string.IsNullOrWhiteSpace(checkBatch))
                throw new MessageException("批次为空!");
            if (string.IsNullOrWhiteSpace(barcode))
                throw new MessageException("商品条码为空!");
            var nvl = new System.Collections.Specialized.NameValueCollection();
            nvl.Add("storeId", storeId);
            nvl.Add("checkBatch", checkBatch);
            nvl.Add("searchText", barcode);
            int count = 0;
            var dt = dal.FindTakeStockList(nvl, out count);
            if (dt == null || dt.Rows.Count <= 0)
                throw new MessageException("该门店和批次下无商品条码!");
            return dt.AsEnumerable().Select(o=>new 
            {
                Barcode = o["Barcode"],
                Title = o["Title"],
                SubUnit = o["SubUnit"],
                CheckBatch = o["CheckBatch"],
                Size = o["Size"],
                ActualNumber = o["ActualNumber"] is DBNull ? 0 : o["ActualNumber"],
                CreateDT = ((DateTime)(o["LockDate"] is DBNull ? o["CreateDT"] : o["LockDate"])).ToString("yyyy-MM-dd")
            });
        }
        public static void SaveStock(string storeId, string checkBatch, string checkUID,Dictionary<string,decimal?> barnums)
        {
            if (string.IsNullOrWhiteSpace(storeId))
                throw new MessageException("门店号为空!");
            if (string.IsNullOrWhiteSpace(checkBatch))
                throw new MessageException("盘点批次为空!");
            if (string.IsNullOrWhiteSpace(checkUID))
                throw new MessageException("盘点员为空!");
            if (!barnums.Any())
                throw new MessageException("盘点内容为空!");

            var barcodes = barnums.Keys.ToList();
            var query = from a in BaseService<StockTaking>.CurrentRepository.QueryEntity
                        join b in BaseService<TreasuryLocks>.CurrentRepository.QueryEntity on new { a.CheckBatch, a.CompanyId } equals new { b.CheckBatch, b.CompanyId }
                        where a.CompanyId == Sys.SysCommonRules.CompanyId && b.LockStoreID == storeId && a.CheckBatch == checkBatch && barcodes.Contains(a.Barcode)
                        select new
                        {
                            a.Barcode,
                            a.ActualNumber,
                            a.CheckBatch,
                            b.State,
                            SysPrice = ProductService.CurrentRepository.QueryEntity.Where(o => o.CompanyId == a.CompanyId 
                                && (o.Barcode == a.Barcode || ("," + o.Barcodes + ",").Contains("," + a.Barcode + ","))).Select(o => o.SysPrice).FirstOrDefault()
                        };
            var stocks = query.ToList();
            if (!stocks.Any())
                throw new MessageException("批次或条码不存在!");
            if (stocks.Any(o=>o.State==1))
                throw new MessageException("该批次已通过审核,不能再盘点!");
            var user = UserInfoService.Find(o =>o.CompanyId==Sys.SysCommonRules.CompanyId && o.UserCode == checkUID);
            if (user == null)
                throw new MessageException("用户编码不存在!");
            var stocklogs = BaseService<StockTakingLog>.FindList(o => o.CompanyId == Sys.SysCommonRules.CompanyId && barcodes.Contains(o.Barcode) && o.CheckBatch == checkBatch && o.State == 1);
            var insertLog = new List<StockTakingLog>();
            var date = DateTime.Now;
            foreach (var de in barnums)
            {
                var sk = stocks.FirstOrDefault(o => o.Barcode == de.Key);
                if (sk == null)
                {
                    throw new MessageException("条码[" + de.Key + "]不存在!");
                }
                if (stocklogs.Any(o => o.Barcode == de.Key))
                {
                    throw new MessageException("条码[" + de.Key + "]条码已复盘!");
                }
                if (!de.Value.HasValue) continue;
                insertLog.Add(new StockTakingLog()
                {
                    Barcode=sk.Barcode,
                    CheckBatch=sk.CheckBatch,
                    CheckUID = user.UID,
                    CreateDT = date,
                    CreateUID=user.UID,
                    ActualDate=date,
                    Number=de.Value.Value,
                    SysPrice=sk.SysPrice,
                    Source=2,
                    CompanyId=Sys.SysCommonRules.CompanyId
                });
            }
            if (insertLog.Any())
            {
                BaseService<StockTakingLog>.AddRange(insertLog);
            }
        }
        public static void ReSaveStock(string storeId, string checkBatch, string checkUID, string barcode, decimal? number,short sure)
        {
            if (string.IsNullOrWhiteSpace(storeId))
                throw new MessageException("门店号为空!");
            if (string.IsNullOrWhiteSpace(checkBatch))
                throw new MessageException("盘点批次为空!");
            if (string.IsNullOrWhiteSpace(checkUID))
                throw new MessageException("盘点员为空!");
            if (string.IsNullOrWhiteSpace(barcode))
                throw new MessageException("条码为空!");
            if (!number.HasValue)
                throw new MessageException("实盘数量为空!");
           
            var query = from a in BaseService<StockTaking>.CurrentRepository.QueryEntity
                        join b in BaseService<TreasuryLocks>.CurrentRepository.QueryEntity on new { a.CheckBatch, a.CompanyId } equals new { b.CheckBatch, b.CompanyId }
                        where a.CompanyId == Sys.SysCommonRules.CompanyId && b.LockStoreID == storeId && a.CheckBatch == checkBatch && a.Barcode == barcode
                        select new
                        {
                            a,
                            a.Barcode,
                            a.ActualNumber,
                            a.CheckBatch,
                            b.State,
                            a.CompanyId,
                            SysPrice = ProductService.CurrentRepository.QueryEntity.Where(o => o.CompanyId == a.CompanyId
                                && (o.Barcode == a.Barcode || ("," + o.Barcodes + ",").Contains("," + a.Barcode + ","))).Select(o => o.SysPrice).FirstOrDefault()
                        };
            var stock = query.FirstOrDefault();
            if (stock == null)
                throw new MessageException("批次或条码不存在!");
            if (stock.State == 1)
                throw new MessageException("该批次已通过审核,不能再盘点!");

            var user = UserInfoService.Find(o => o.CompanyId == Sys.SysCommonRules.CompanyId && o.UserCode == checkUID);
            if(user==null)
                throw new MessageException("用户编码不存在!");
            stock.a.Sure =(short)(sure==0?0:1);
            //var st= BaseService<StockTaking>.Find(o => o.CompanyId == stock.CompanyId && o.CheckBatch == stock.CheckBatch && o.Barcode == stock.Barcode);
            var log = new StockTakingLog()
            {
                Barcode = stock.Barcode,
                CheckBatch = stock.CheckBatch,
                CheckUID = user.UID,
                CreateUID = user.UID,
                CreateDT = DateTime.Now,
                ActualDate=DateTime.Now,
                SysPrice=stock.SysPrice,
                Number = number.GetValueOrDefault(),
                State = 1,
                Source=2,
                CompanyId = Sys.SysCommonRules.CompanyId
            };
            BaseService<StockTakingLog>.Add(log);
        }
        public static IEnumerable<dynamic> TakeStockLogs(string checkBatch, string barcode)
        {
            if (string.IsNullOrWhiteSpace(checkBatch))
                throw new MessageException("盘点批次为空!");
            if (string.IsNullOrWhiteSpace(barcode))
                throw new MessageException("条码为空!");

            return BLL.TakeStockService.TakeStockLogs(checkBatch, barcode);
        }
    }
}
