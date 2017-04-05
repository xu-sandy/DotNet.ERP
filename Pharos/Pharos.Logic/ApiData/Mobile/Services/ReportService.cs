using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Pharos.Logic.ApiData.Mobile.Exceptions;
using Pharos.Logic.ApiData.Mobile.Repository;
using Pharos.Logic.BLL;
using Pharos.Logic.ApiData.Pos.Sale;
using Pharos.Utility.Helpers;
namespace Pharos.Logic.ApiData.Mobile.Services
{
    public class ReportService
    {
        static CommonDAL dal = new CommonDAL();
        static Pharos.Logic.DAL.ReportDAL reportDal = new Pharos.Logic.DAL.ReportDAL();
        /// <summary>
        /// 销售类型统计
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="date"></param>
        /// <param name="lx">1-日结,2-月结</param>
        /// <returns></returns>
        public static object SaleTypeReport(string storeId, string date, short lx)
        {
            if (string.IsNullOrWhiteSpace(storeId))
                throw new MessageException("门店号为空!");
            if (string.IsNullOrWhiteSpace(date))
                throw new MessageException("日期为空!");
            var start = DateTime.Parse(date).Date;
            var end = start.AddDays(1);
            if (lx == 2)
            {
                start = new DateTime(start.Year, start.Month, 1);
                end = start.AddMonths(1);
            }
            var giftStatus = new int[] { (int)SaleStatus.ActivityGifts, (int)SaleStatus.POSGift };
            var allist = SaleOrdersService.FindList(o => o.CreateDT >= start && o.CreateDT < end && o.StoreId == storeId && !o.IsTest && o.CompanyId == CommonService.CompanyId);
            var salelist = allist.Where(o => o.Type == 0 && o.State == 0);
            var huanglist = allist.Where(o => o.Type == 1 && o.State == 0);
            var retlist = allist.Where(o => o.Type == 2 || o.State == 1);
            var giftlist = (from a in SaleDetailService.CurrentRepository.Entities
                            from b in SaleOrdersService.CurrentRepository.Entities
                            where a.PaySN == b.PaySN && giftStatus.Contains(a.SalesClassifyId) && b.CreateDT >= start && b.CreateDT < end && b.StoreId == storeId && !b.IsTest && b.CompanyId == CommonService.CompanyId
                            select a).ToList();

            var list = new List<object>()
            {
                new{Type="销售",Count=salelist.Count(),Total=salelist.Sum(o=>o.TotalAmount)},
                new{Type="退货",Count=retlist.Count(),Total=retlist.Sum(o=>Math.Abs(o.TotalAmount))},
                new{Type="赠送",Count=giftlist.Select(o=>o.PaySN).Distinct().Count(),Total=giftlist.Sum(o=>o.PurchaseNumber*o.SysPrice)},
                new{Type="换货",Count=huanglist.Count(),Total=huanglist.Sum(o=>Math.Abs(o.TotalAmount))},
            };
            return list;
        }
        /// <summary>
        ///销售员销售报表
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="date"></param>
        /// <param name="lx">1-日结,2-月结</param>
        /// <returns></returns>
        public static object SaleCashierDateReport(string storeId, string date, string date2, short lx)
        {
            if (string.IsNullOrWhiteSpace(storeId))
                throw new MessageException("门店号为空!");
            if (string.IsNullOrWhiteSpace(date))
                throw new MessageException("日期为空!");
            var nvl = new System.Collections.Specialized.NameValueCollection();
            var start = DateTime.Parse(date).Date;
            var end = start;
            nvl["datelen"] = "10";
            if (lx == 2)
            {
                start = new DateTime(start.Year, start.Month, 1);
                end = date2.IsNullOrEmpty() ? start.AddMonths(1).AddDays(-1) : DateTime.Parse(date2).Date.AddMonths(1).AddDays(-1);
                nvl["datelen"] = "7";
            }
            nvl["date"] = start.ToString("yyyy-MM-dd");
            nvl["date2"] = end.ToString("yyyy-MM-dd");
            nvl["store"] = storeId;

            var ds = reportDal.QueryCashierSaleOrderDay(nvl);
            if (ds.Tables.Count > 1)
            {
                var objTotal = ds.Tables[1].AsEnumerable().Where(o => !(o["XSCount"] is DBNull)).Select(o => new
                {
                    SSCount = o["SSCount"],
                    SSMoney = o["SSMoney"],
                    XSCount = o["XSCount"],
                    XSMoney = o["XSMoney"],
                    HUANCount = o["HUANCount"],
                    HUANMoney = o["HUANMoney"],
                    TUIHuanCount = o["TUIHuanCount"],
                    TUIHuanMoney = o["TUIHuanMoney"],
                    TUICount = o["TUICount"],
                    TUIMoney = o["TUIMoney"],
                    CZCount = o["CZCount"],
                    CZMoney = o["CZMoney"],
                    ZSCount = o["ZSCount"],
                    ZSMoney = o["ZSMoney"]
                }).FirstOrDefault();

                var details = ds.Tables[0].AsEnumerable().Select(o => new
                {
                    Title = o["Cashier"],
                    Date = o["Date"],
                    FirstTime = o["FirstTime"].ToType<DateTime>().ToString("yyyy-MM-dd HH:mm:ss"),
                    LastTime = o["LastTime"].ToType<DateTime>().ToString("yyyy-MM-dd HH:mm:ss"),
                    SSCount = o["SSCount"],
                    SSMoney = o["SSMoney"],
                    XSCount = o["XSCount"],
                    XSMoney = o["XSMoney"],
                    HUANCount = o["HUANCount"],
                    HUANMoney = o["HUANMoney"],
                    TUIHuanCount = o["TUIHuanCount"],
                    TUIHuanMoney = o["TUIHuanMoney"],
                    TUICount = o["TUICount"],
                    TUIMoney = o["TUIMoney"],
                    ZSCount = o["ZSCount"],
                    ZSMoney = o["ZSMoney"],
                    MLCount = o["MLCount"],
                    MLMoney = o["MLMoney"],
                    CZCount = o["CZCount"],
                    CZMoney = o["CZMoney"],
                    RLCount = o["RLCount"],
                    RLMoney = o["RLMoney"],
                    XJMoney = o["XJMoney"]
                }).ToList();

                var columns = ds.Tables[0].Columns;
                var dt = new DataTable();
                dt.Columns.Add("type");
                dt.Columns.Add("date");
                dt.Columns.Add("title");
                dt.Columns.Add("name");
                dt.Columns.Add("money", typeof(decimal));
                foreach (DataRow drt in ds.Tables[0].Rows)
                {
                    foreach (DataColumn col in columns)
                    {
                        if (!col.ColumnName.EndsWith("_dy")) continue;
                        var value = drt[col.ColumnName].ToType<decimal>();
                        if (value == 0) continue;
                        var dr = dt.NewRow();
                        dr["name"] = col.ColumnName.Split('_')[0];
                        dr["date"] = drt["Date"];
                        dr["title"] = drt["Cashier"];
                        dr["money"] = value;
                        if (col.ColumnName.EndsWith("_xs_dy"))
                        {
                            dr["type"] = "xs";
                        }
                        else if (col.ColumnName.EndsWith("_tui_dy"))
                        {
                            dr["type"] = "tui";
                        }
                        else if (col.ColumnName.EndsWith("_tuihuang_dy"))
                        {
                            dr["type"] = "tuihuang";
                        }
                        else if (col.ColumnName.EndsWith("_huang_dy"))
                        {
                            dr["type"] = "huang";
                        }
                        dt.Rows.Add(dr);
                    }
                }
                return new { Total = objTotal, Details = details, DetailPays = dt };
            }
            return new { Total = new { }, Details = new object[0], DetailPays = new object[0] };
        }
        /// <summary>
        /// 门店销售报表
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="date"></param>
        /// <param name="lx">1-日结,2-月结</param>
        /// <returns></returns>
        public static object SaleStoreDateReport(string date, string date2, short lx)
        {
            if (string.IsNullOrWhiteSpace(date))
                throw new MessageException("日期为空!");
            var nvl = new System.Collections.Specialized.NameValueCollection();
            var start = DateTime.Parse(date).Date;
            var end = start;
            nvl["datelen"] = "10";
            if (lx == 2)
            {
                start = new DateTime(start.Year, start.Month, 1);
                end = date2.IsNullOrEmpty() ? start.AddMonths(1).AddDays(-1) : DateTime.Parse(date2).Date.AddMonths(1).AddDays(-1);
                nvl["datelen"] = "7";
            }
            nvl["date"] = start.ToString("yyyy-MM-dd");
            nvl["date2"] = end.ToString("yyyy-MM-dd");

            var ds = reportDal.QueryStoreSaleOrderDay(nvl);
            if (ds.Tables.Count > 1)
            {
                var objTotal = ds.Tables[1].AsEnumerable().Where(o => !(o["XSCount"] is DBNull)).Select(o => new
                {
                    SSCount = o["SSCount"],
                    SSMoney = o["SSMoney"],
                    XSCount = o["XSCount"],
                    XSMoney = o["XSMoney"],
                    HUANCount = o["HUANCount"],
                    HUANMoney = o["HUANMoney"],
                    TUIHuanCount = o["TUIHuanCount"],
                    TUIHuanMoney = o["TUIHuanMoney"],
                    TUICount = o["TUICount"],
                    TUIMoney = o["TUIMoney"],
                    CZCount = o["CZCount"],
                    CZMoney = o["CZMoney"],
                    ZSCount = o["ZSCount"],
                    ZSMoney = o["ZSMoney"]
                }).FirstOrDefault();

                var details = ds.Tables[0].AsEnumerable().Select(o => new
                {
                    Title = o["Store"],
                    Date = o["Date"],
                    FirstTime = o["FirstTime"].ToType<DateTime>().ToString("yyyy-MM-dd HH:mm:ss"),
                    LastTime = o["LastTime"].ToType<DateTime>().ToString("yyyy-MM-dd HH:mm:ss"),
                    SaleOrderAverage = o["SaleOrderAverage"],
                    SSCount = o["SSCount"],
                    SSMoney = o["SSMoney"],
                    XSCount = o["XSCount"],
                    XSMoney = o["XSMoney"],
                    HUANCount = o["HUANCount"],
                    HUANMoney = o["HUANMoney"],
                    TUIHuanCount = o["TUIHuanCount"],
                    TUIHuanMoney = o["TUIHuanMoney"],
                    TUICount = o["TUICount"],
                    TUIMoney = o["TUIMoney"],
                    CZCount = o["CZCount"],
                    CZMoney = o["CZMoney"],
                    ZSCount = o["ZSCount"],
                    ZSMoney = o["ZSMoney"],
                    MLCount = o["MLCount"],
                    MLMoney = o["MLMoney"],
                    RLCount = o["RLCount"],
                    RLMoney = o["RLMoney"],
                    XJMoney = o["XJMoney"]
                }).ToList();

                var columns = ds.Tables[0].Columns;
                var dt = new DataTable();
                dt.Columns.Add("type");
                dt.Columns.Add("date");
                dt.Columns.Add("title");
                dt.Columns.Add("name");
                dt.Columns.Add("money", typeof(decimal));
                foreach (DataRow drt in ds.Tables[0].Rows)
                {
                    foreach (DataColumn col in columns)
                    {
                        if (!col.ColumnName.EndsWith("_dy")) continue;
                        var value = drt[col.ColumnName].ToType<decimal>();
                        if (value == 0) continue;
                        var dr = dt.NewRow();
                        dr["name"] = col.ColumnName.Split('_')[0];
                        dr["date"] = drt["Date"];
                        dr["title"] = drt["Store"];
                        dr["money"] = value;
                        if (col.ColumnName.EndsWith("_xs_dy"))
                        {
                            dr["type"] = "xs";
                        }
                        else if (col.ColumnName.EndsWith("_tui_dy"))
                        {
                            dr["type"] = "tui";
                        }
                        else if (col.ColumnName.EndsWith("_tuihuang_dy"))
                        {
                            dr["type"] = "tuihuang";
                        }
                        else if (col.ColumnName.EndsWith("_huang_dy"))
                        {
                            dr["type"] = "huang";
                        }
                        dt.Rows.Add(dr);
                    }
                }
                return new { Total = objTotal, Details = details, DetailPays = dt };
            }
            return new { Total = new { }, Details = new object[0], DetailPays = new object[0] };
        }
        /// <summary>
        /// 商品统计
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static object SaleDetailDayReport(string storeId, string date)
        {
            if (string.IsNullOrWhiteSpace(storeId))
                throw new MessageException("门店号为空!");
            if (string.IsNullOrWhiteSpace(date))
                throw new MessageException("日期为空!");
            var start = DateTime.Parse(date).Date;
            var end = start.AddDays(1);
            var dt = dal.SaleDetailReport(storeId, start, end);
            return dt;
        }
        /// <summary>
        /// 销售统计
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="date"></param>
        /// <param name="type">1-品类,2-品牌,3-供应商</param>
        /// <returns></returns>
        public static object SaleDayReport(string storeId, string date, string type)
        {
            if (string.IsNullOrWhiteSpace(storeId))
                throw new MessageException("门店号为空!");
            if (string.IsNullOrWhiteSpace(date))
                throw new MessageException("当前日期为空!");

            int saleDayTakeNum = 10;
            var start = DateTime.Parse(date).Date;
            var end = start.AddDays(1);
            //var query = from a in BaseService<Entity.SaleOrders>.CurrentRepository.Entities
            //            join b in BaseService<Entity.SaleDetail>.CurrentRepository.Entities on a.PaySN equals b.PaySN
            //            join c in BaseService<Entity.VwProduct>.CurrentRepository.Entities on b.Barcode equals c.Barcode into tmp
            //            from d in tmp.DefaultIfEmpty()
            //            where a.CreateDT >= start && a.CreateDT < end && a.StoreId==storeId && d.MidCategoryTitle!=null
            //            group new { d,b } by new { d.BigCategoryTitle, d.MidCategoryTitle, d.SubCategoryTitle } into g
            //            select new
            //            {
            //                g.Key.BigCategoryTitle,
            //                g.Key.MidCategoryTitle,
            //                g.Key.SubCategoryTitle,
            //                SaleTotal=g.Sum(o=>o.b.ActualPrice*o.b.PurchaseNumber),
            //                BuyTotal=g.Sum(o=>o.b.BuyPrice*o.b.PurchaseNumber),
            //                PurchaseNumber=g.Sum(o=>o.b.PurchaseNumber)
            //            };


            if (type == "1")
            {
                var query = from a in BaseService<Entity.SaleOrders>.CurrentRepository.Entities
                            join b in BaseService<Entity.SaleDetail>.CurrentRepository.Entities on new { a.CompanyId, a.PaySN } equals new { b.CompanyId, b.PaySN }
                            join c in BaseService<Entity.VwProduct>.CurrentRepository.Entities on new { b.Barcode, b.CompanyId } equals new { c.Barcode, c.CompanyId } into tmp
                            from d in tmp.DefaultIfEmpty()
                            where a.CompanyId == CommonService.CompanyId && a.CreateDT >= start && a.CreateDT < end && a.StoreId == storeId && d.MidCategoryTitle != null && a.State == 0 && a.Type == 0
                            group new { d, b } by new { d.CategoryTitle } into g
                            select new
                            {
                                g.Key.CategoryTitle,
                                SaleTotal = g.Sum(o => o.b.ActualPrice * o.b.PurchaseNumber),
                                BuyTotal = g.Sum(o => o.b.BuyPrice * o.b.PurchaseNumber),
                                PurchaseNumber = g.Sum(o => o.b.PurchaseNumber)
                            };
                var list = query.OrderByDescending(o => o.SaleTotal).ToList();
                var otherSaleTotal = list.Skip(saleDayTakeNum).Sum(o => (decimal?)o.SaleTotal);
                var otherBuyTotal = list.Skip(saleDayTakeNum).Sum(o => (decimal?)o.BuyTotal);
                var total = list.Sum(o => o.SaleTotal);
                var results = list.Take(saleDayTakeNum).Select(o => new
                {
                    Title = o.CategoryTitle.Substring(o.CategoryTitle.LastIndexOf("/") + 1),
                    o.SaleTotal,
                    Profit = o.SaleTotal - o.BuyTotal,
                    OccupyRate = (o.SaleTotal / total).ToString("p")
                }).ToList();
                if (otherSaleTotal.HasValue && otherSaleTotal != 0)
                {
                    results.Add(new
                    {
                        Title = "其它",
                        SaleTotal = otherSaleTotal.GetValueOrDefault(),
                        Profit = otherSaleTotal.GetValueOrDefault() - otherBuyTotal.GetValueOrDefault(),
                        OccupyRate = (otherSaleTotal.GetValueOrDefault() / total).ToString("p")
                    });
                }
                return results;
            }
            else if (type == "2")
            {
                var query = from a in BaseService<Entity.SaleOrders>.CurrentRepository.Entities
                            join b in BaseService<Entity.SaleDetail>.CurrentRepository.Entities on new { a.CompanyId, a.PaySN } equals new { b.CompanyId, b.PaySN }
                            join c in BaseService<Entity.VwProduct>.CurrentRepository.Entities on new { b.Barcode, b.CompanyId } equals new { c.Barcode, c.CompanyId } into tmp
                            from d in tmp.DefaultIfEmpty()
                            where a.CompanyId == CommonService.CompanyId && a.CreateDT >= start && a.CreateDT < end && a.StoreId == storeId && !(d.BrandTitle == "" || d.BrandTitle == null) && a.State == 0 && a.Type == 0
                            group new { d, b } by new { d.BrandSN, d.BrandTitle } into g
                            select new
                            {
                                g.Key.BrandTitle,
                                SaleTotal = g.Sum(o => o.b.ActualPrice * o.b.PurchaseNumber),
                                BuyTotal = g.Sum(o => o.b.BuyPrice * o.b.PurchaseNumber),
                                PurchaseNumber = g.Sum(o => o.b.PurchaseNumber)
                            };
                var list = query.OrderByDescending(o => o.SaleTotal).ToList();
                var otherSaleTotal = list.Skip(saleDayTakeNum).Sum(o => (decimal?)o.SaleTotal);
                var otherBuyTotal = list.Skip(saleDayTakeNum).Sum(o => (decimal?)o.BuyTotal);
                var total = list.Sum(o => o.SaleTotal);
                var results = list.Take(saleDayTakeNum).Select(o => new
                {
                    Title = o.BrandTitle,
                    o.SaleTotal,
                    Profit = o.SaleTotal - o.BuyTotal,
                    OccupyRate = (o.SaleTotal / total).ToString("p")
                }).ToList();
                if (otherSaleTotal.HasValue && otherSaleTotal != 0)
                {
                    results.Add(new
                    {
                        Title = "其它",
                        SaleTotal = otherSaleTotal.GetValueOrDefault(),
                        Profit = otherSaleTotal.GetValueOrDefault() - otherBuyTotal.GetValueOrDefault(),
                        OccupyRate = (otherSaleTotal.GetValueOrDefault() / total).ToString("p")
                    });
                }
                return results;
            }
            else if (type == "3")
            {
                var query = from a in BaseService<Entity.SaleOrders>.CurrentRepository.Entities
                            join b in BaseService<Entity.SaleDetail>.CurrentRepository.Entities on new { a.CompanyId, a.PaySN } equals new { b.CompanyId, b.PaySN }
                            join c in BaseService<Entity.VwProduct>.CurrentRepository.Entities on new { b.Barcode, b.CompanyId } equals new { c.Barcode, c.CompanyId } into tmp
                            from d in tmp.DefaultIfEmpty()
                            where a.CompanyId == CommonService.CompanyId && a.CreateDT >= start && a.CreateDT < end && a.StoreId == storeId && !(d.SupplierTitle == "" || d.SupplierTitle == null) && a.State == 0 && a.Type == 0
                            group new { d, b } by new { d.SupplierId, d.SupplierTitle } into g
                            select new
                            {
                                g.Key.SupplierTitle,
                                SaleTotal = g.Sum(o => o.b.ActualPrice * o.b.PurchaseNumber),
                                BuyTotal = g.Sum(o => o.b.BuyPrice * o.b.PurchaseNumber),
                                PurchaseNumber = g.Sum(o => o.b.PurchaseNumber)
                            };
                var list = query.OrderByDescending(o => o.SaleTotal).ToList();
                var otherSaleTotal = list.Skip(saleDayTakeNum).Sum(o => (decimal?)o.SaleTotal);
                var otherBuyTotal = list.Skip(saleDayTakeNum).Sum(o => (decimal?)o.BuyTotal);
                var total = list.Sum(o => o.SaleTotal);
                var results = list.Take(saleDayTakeNum).Select(o => new
                {
                    Title = o.SupplierTitle,
                    o.SaleTotal,
                    Profit = o.SaleTotal - o.BuyTotal,
                    OccupyRate = (o.SaleTotal / total).ToString("p")
                }).ToList();
                if (otherSaleTotal.HasValue && otherSaleTotal != 0)
                {
                    results.Add(new
                    {
                        Title = "其它",
                        SaleTotal = otherSaleTotal.GetValueOrDefault(),
                        Profit = otherSaleTotal.GetValueOrDefault() - otherBuyTotal.GetValueOrDefault(),
                        OccupyRate = (otherSaleTotal.GetValueOrDefault() / total).ToString("p")
                    });
                }
                return results;
            }
            else
                return null;
        }
        /// <summary>
        /// 赠送统计
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static object SaleGiftReport(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
                throw new MessageException("当前日期为空!");
            var start = DateTime.Parse(date).Date;
            start = new DateTime(start.Year, start.Month, 1);
            var end = start.AddMonths(1);
            var dt = dal.SaleGiftReport(start, end);
            var list = new List<object>();
            int i = 1;
            var total = dt.AsEnumerable().Sum(o => (decimal)o["GiftTotal"]);
            decimal otherSaleTotal = 0, otherNumber = 0;
            foreach (DataRow dr in dt.Rows)
            {
                decimal num = (decimal)dr["PurchaseNumber"];
                decimal subtotal = (decimal)dr["GiftTotal"];
                if (i <= 10)
                {
                    list.Add(new { Title = dr["Title"], PurchaseNumber = num, GiftTotal = subtotal, OccupyRate = (total == 0 ? 0 : (decimal)dr["GiftTotal"] / total).ToString("p") });
                }
                else
                {
                    otherNumber += num;
                    otherSaleTotal += subtotal;
                }
                i++;
            }
            if (list.Count >= 10)
                list.Add(new { Title = "其它门店", PurchaseNumber = otherNumber, GiftTotal = otherSaleTotal, OccupyRate = (total == 0 ? 0 : otherSaleTotal / total).ToString("p") });
            return list;
        }
        /// <summary>
        /// 缺货统计
        /// </summary>
        /// <returns></returns>
        public static object StockOutReport()
        {
            var query = from a in InventoryService.CurrentRepository.Entities
                        join b in ProductService.CurrentRepository.Entities on new { a.Barcode, a.CompanyId } equals new { b.Barcode, b.CompanyId }
                        where a.CompanyId == CommonService.CompanyId && a.StockNumber < (b.InventoryWarning ?? 0)
                        orderby a.StockNumber ascending
                        select new
                        {
                            b.Barcode,
                            b.Title,
                            a.StockNumber
                        };
            var list = query.Take(10).ToList();
            return list;
        }
        /// <summary>
        /// 过剩统计
        /// </summary>
        /// <returns></returns>
        public static object StockMoreReport()
        {
            var query = from a in InventoryService.CurrentRepository.Entities
                        join b in ProductService.CurrentRepository.Entities on new { a.Barcode, a.CompanyId } equals new { b.Barcode, b.CompanyId }
                        where a.CompanyId == CommonService.CompanyId && a.StockNumber > (b.InventoryWarningMax ?? 0)
                        orderby a.StockNumber descending
                        select new
                        {
                            b.Barcode,
                            b.Title,
                            a.StockNumber
                        };
            var list = query.Take(10).ToList();
            return list;
        }
        /// <summary>
        /// 会员统计
        /// </summary>
        /// <param name="date"></param>
        /// <param name="lx">1-日,2-月,3-全部</param>
        /// <returns></returns>
        public static object MemberReport(string date, short lx)
        {
            var start = new DateTime(2015, 1, 1);
            var end = DateTime.Now;
            if (lx != 3)
            {
                if (string.IsNullOrWhiteSpace(date))
                    throw new MessageException("当前日期为空!");
                if (lx == 1)
                {
                    start = DateTime.Parse(date).Date;
                    end = start.AddDays(1);
                }
                if (lx == 2)
                {
                    start = DateTime.Parse(date).Date;
                    end = start.AddMonths(1);
                }
            }
            var query = from x in MembersService.CurrentRepository.Entities
                        join y in WarehouseService.CurrentRepository.Entities on new { x.StoreId, x.CompanyId } equals new { y.StoreId, y.CompanyId }
                        where x.CompanyId == CommonService.CompanyId && x.CreateDT >= start && x.CreateDT < end && x.Status == 1
                        group new { x, y } by new { x.StoreId, y.Title } into g
                        select new
                        {
                            g.Key.Title,
                            Count = g.Count()
                        };
            var list = query.ToList();
            var total = list.Sum(o => o.Count);
            return list.OrderByDescending(o => o.Count).Select(o => new
            {
                o.Title,
                o.Count,
                OccupyRate = (o.Count / (decimal)total).ToString("p")
            });
        }
        static readonly Pharos.Logic.DAL.SaleOrderDAL saleOrderDal = new Pharos.Logic.DAL.SaleOrderDAL();
        /// <summary>
        ///手机帐单
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="type">类型(0-正常，1-换货，2-退货,3-退单)</param>
        /// <param name="time">时间(0-今日，1-昨天，2-本周)</param>
        /// <param name="cashier">收银员</param>
        /// <param name="saler">导购员</param>
        /// <returns></returns>
        public static object SaleDateBills(string storeId, string type, string time, string cashier, string saler, int pageIndex, int pageSize)
        {
            var nl = new System.Collections.Specialized.NameValueCollection();
            switch (time)
            {
                case "1":
                    nl["date"] = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    nl["date2"] = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                case "2":
                    var week = (DateTime.Now.DayOfWeek==DayOfWeek.Sunday?7:(byte)DateTime.Now.DayOfWeek)-1;
                    nl["date"] = DateTime.Now.AddDays(-week).ToString("yyyy-MM-dd");
                    nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    break;
                default:
                    nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                    nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    break;
            }
            nl["store"] = storeId;
            nl["Type"] = type;
            nl["cashier"] = cashier;
            nl["saler"] = saler;
            nl["page"] = pageIndex.ToString();
            nl["rows"] = pageSize.ToString();
            nl["sortField"] = "desc";
            var dt = saleOrderDal.QuerySaleOrdersPageList(nl);
            var count = 0;
            var result = new Pharos.Logic.ApiData.Pos.ValueObject.PageResult<object>();
            if (dt.Rows.Count > 0)
            {
                count = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                var groups = dt.AsEnumerable().GroupBy(dr => new
                {
                    PaySn = dr["PaySN"],
                    CustomOrderSn = dr["CustomOrderSn"],
                    ApiTitle = dr["ApiTitle"],
                    CreateDT =DateFormat((DateTime)dr["CreateDT"]),
                    Type = dr["Type"],
                    Amount = dr["TotalAmount"],
                    TotalAmount = dr["TotalAmounts"]
                }).Select(o=>o.Key).ToList();
                result.Datas = groups;
            }
            var pageCount = count / pageSize + (count % pageSize > 0 ? 1 : 0);
            result.Pager = new Pharos.Logic.ApiData.Pos.ValueObject.PageInfo()
            {
                Index = pageIndex,
                Size = pageSize,
                Total = count,
                PageCount = pageCount
            };
            return result;
        }
        public static object SaleMonthBillSummarys()
        {
            var nvl = new System.Collections.Specialized.NameValueCollection();
            nvl["date"]= DateTime.Now.AddMonths(-5).ToString("yyyy-MM-01");
            nvl["date2"] = DateTime.Now.AddMonths(1).ToString("yyyy-MM-01");
            nvl["datelen"]= "7";
            nvl["ispage"] = "0";
            nvl["sortField"] = "Date asc";
            var dt= reportDal.QueryStoreSaleOrderDay(nvl).Tables[0];
            var list= dt.AsEnumerable().GroupBy(o => o["Date"]).Select(o => new { 
                date= o.Key+"月",
                money=Math.Round(o.Sum(i=>i["SSMoney"].ToType<decimal>()),2)
            }).ToList();
            var now = DateTime.Now;
            for (var date = now.AddMonths(-5); date < now.AddMonths(1); )
            {
                var d = date.ToString("yyyy-MM");
                if(!list.Any(o=>o.date.StartsWith(d)))
                    list.Add(new { date = d + "月", money = 0m });
                date = date.AddMonths(1);
            }
            var ls = list.Select(o => new
            {
                date = o.date.Split('-')[1],
                o.money
            });
            return ls;
        }
        public static object SaleDateBillSummarys(string type, string date, string date2)
        {
            var nvl = new System.Collections.Specialized.NameValueCollection();
            switch (type)
            {
                case "2"://指定日期
                    nvl["date"] = date;
                    nvl["date2"] =date2.IsNullOrEmpty()? DateTime.Parse(date).AddDays(1).ToString("yyyy-MM-dd")
                        : DateTime.Parse(date2).AddDays(1).ToString("yyyy-MM-dd");
                    break;
                case "3"://指定日期时间
                    nvl["date"] = date;
                    nvl["date2"] =date2.IsNullOrEmpty()? DateTime.Parse(date).AddDays(1).ToString("yyyy-MM-dd")
                        : DateTime.Parse(date2).AddMinutes(1).ToString("yyyy-MM-dd");
                    break;
                default:
                    nvl["date"] = DateTime.Parse(date).ToString("yyyy-MM-01");
                    nvl["date2"] =date2.IsNullOrEmpty()? DateTime.Parse(date).AddMonths(1).ToString("yyyy-MM-01")
                        : DateTime.Parse(date2).AddMonths(1).ToString("yyyy-MM-01");
                    break;
            }
            nvl["datelen"] = "7";
            nvl["ispage"] = "0";
            var dt = reportDal.QueryStoreSaleOrderDay(nvl).Tables[1];
            var xshj = dt.Rows[0]["XSMoney"].ToType<decimal>();
            var huanghj = dt.Rows[0]["HUANMoney"].ToType<decimal>();
            var tuihuanghj = dt.Rows[0]["TUIHuanMoney"].ToType<decimal>()+dt.Rows[0]["TUIMoney"].ToType<decimal>();
            var xsdetails = new List<object>();
            var huangdetails = new List<object>();
            var tuihuangdetails = new List<object>();
            foreach(DataColumn dc in dt.Columns)
            {
                if(dc.ColumnName.EndsWith("_xs_dy"))
                {
                    xsdetails.Add(new { title = dc.ColumnName.Replace("_xs_dy", ""),money=dt.Rows[0][dc].ToType<decimal>() });
                }
                if (dc.ColumnName.EndsWith("_tui_dy"))
                {
                    var name = dc.ColumnName.Replace("_tui_dy", "");
                    tuihuangdetails.Add(new { title = name, money = dt.Rows[0][dc].ToType<decimal>() + dt.Rows[0][name + "_tuihuang_dy"].ToType<decimal>() });
                }
                if (dc.ColumnName.EndsWith("_huang_dy"))
                {
                    huangdetails.Add(new { title = dc.ColumnName.Replace("_huang_dy", ""), money = dt.Rows[0][dc].ToType<decimal>() });
                }
            }
            var start=DateTime.Parse(nvl["date"]);
            var end=DateTime.Parse(nvl["date2"]);
            var recharges =( from x in MemberRechargeService.CurrentRepository.QueryEntity
                        join y in MembershipCardService.CurrentRepository.QueryEntity on new { x.CompanyId, x.CardId } equals new { y.CompanyId, CardId=y.CardSN }
                        join z in CardInfoService.CurrentRepository.QueryEntity on new { y.CompanyId, y.CardTypeId } equals new { z.CompanyId, z.CardTypeId }
                             where x.CompanyId == CommonService.CompanyId && !x.IsTest && x.CreateDT >= start && x.CreateDT <= end
                        select new {
                            x.Type,
                            x.RechargeAmount,
                            z.CardName
                        }).ToList();
            var czls = recharges.Where(o => o.Type == 1);
            var czcarddetails= czls.GroupBy(o=>o.CardName).Select(o=>new{
                card= o.Key,
                money = o.Sum(i => i.RechargeAmount)
            }).ToList();
            var fczls = recharges.Where(o => o.Type ==2);
            var fczcarddetails = fczls.GroupBy(o => o.CardName).Select(o => new
            {
                card = o.Key,
                money = o.Sum(i => i.RechargeAmount)
            }).ToList();

            return new
            {
                xs = new { total = xshj, detail = xsdetails },
                hr = new { total = huanghj, detail = huangdetails },
                th = new { total = tuihuanghj, detail = tuihuangdetails },
                zs = new { total = dt.Rows[0]["ZSMoney"].ToType<decimal>(), count = dt.Rows[0]["ZSCount"].ToType<decimal>() },
                rl = new { total = dt.Rows[0]["RLMoney"].ToType<decimal>(), count = dt.Rows[0]["RLCount"].ToType<decimal>() },
                ml = new { total = dt.Rows[0]["MLMoney"].ToType<decimal>(), count = dt.Rows[0]["MLCount"].ToType<decimal>() },
                cz = new { total = czls.Sum(o => o.RechargeAmount), detail = czcarddetails },//充值收入
                fcz = new { total = fczls.Sum(o => o.RechargeAmount), detail = fczcarddetails },//反结算支出
            };
        }
        static string DateFormat(DateTime time)
        {
            if (time.Date == DateTime.Now.Date)
                return "今天 " + time.ToString("HH:mm");
            else if (time.Date == DateTime.Now.AddDays(-1).Date)
                return "昨天 " + time.ToString("HH:mm");
            else
                return time.ToString("MM-dd HH:mm");
        }
        static string TypeFormat(string type)
        {
            var tp = (SaleType)Enum.Parse(typeof(SaleType), type);
            return tp.GetEnumDescription();
        }
    }
}
