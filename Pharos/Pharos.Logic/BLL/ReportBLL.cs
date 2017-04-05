﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using Pharos.Utility;
using Pharos.Utility.Helpers;

namespace Pharos.Logic.BLL
{
    public class ReportBLL
    {
        static DAL.ReportDAL dal = new DAL.ReportDAL();
        /// <summary>
        /// 商品销售明细报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryProductSaleDetailsPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer,ref decimal recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var store = nvl["store"];
            var dt = dal.QueryProductSaleDetails(nl);
            decimal purchaseNumber = 0, saleTotal = 0, buyTotal = 0, mle = 0, saleTotal2 = 0, huanNumber = 0, huanTotal = 0, returnNumber = 0, returnTotal = 0, giftNumber = 0, giftTotal = 0, returnHuangTotal = 0, returnHuangNumber=0;

            if (dt.Rows.Count > 0)
            {
                purchaseNumber = dt.Rows[0]["PurchaseNumbers"].ToType<decimal>();
                saleTotal = dt.Rows[0]["SaleTotals"].ToType<decimal>();
                buyTotal = dt.Rows[0]["BuyTotals"].ToType<decimal>();
                huanNumber = dt.Rows[0]["HuanNumbers"].ToType<decimal>();
                huanTotal = dt.Rows[0]["HuanTotals"].ToType<decimal>();
                returnHuangNumber = dt.Rows[0]["ReturnHuangNumbers"].ToType<decimal>();
                returnHuangTotal = dt.Rows[0]["ReturnHuangTotals"].ToType<decimal>();
                returnNumber = dt.Rows[0]["ReturnNumbers"].ToType<decimal>();
                returnTotal = dt.Rows[0]["ReturnTotals"].ToType<decimal>();
                giftNumber = dt.Rows[0]["GiftNumbers"].ToType<decimal>();
                giftTotal = dt.Rows[0]["GiftTotals"].ToType<decimal>();
                mle = dt.Rows[0]["MLES"].ToType<decimal>();
            }
            var desc = "销售合计(已扣除整单、满元的抹零金额):";
            if (nl["parentType"].IsNullOrEmpty() && nl["supplierId"].IsNullOrEmpty() && nl["brandsn"].IsNullOrEmpty())
            {
                var start = DateTime.Parse(nl["date"]);
                var end = DateTime.Parse(nl["date2"]);
                var where = Pharos.Utility.Helpers.DynamicallyLinqHelper.True<Entity.SaleOrders>();
                where = where.And(o => o.CreateDT >= start && o.CreateDT < end).And(o => o.StoreId == store, string.IsNullOrEmpty(store));
                saleTotal2 = SaleOrdersService.CurrentRepository.QueryEntity.Where(where).Sum(o => (decimal?)o.TotalAmount).GetValueOrDefault();
            }
            else
            {
                desc = "销售合计(未扣除整单、满元的抹零金额):";
                saleTotal2 = saleTotal;
            }
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }
            else
                recordCount = saleTotal2;

            footer = new List<object>() { 
                new {PurchaseNumber=purchaseNumber,SaleTotal=saleTotal,BuyTotal=buyTotal,MLE=mle,MLL=(saleTotal==0?0:mle/saleTotal).ToString("p"),
                    HuanNumber = huanNumber, HuanTotal = huanTotal, ReturnNumber = returnNumber, ReturnTotal = returnTotal, GiftNumber = giftNumber, 
                    ReturnHuangNumber=returnHuangNumber,ReturnHuangTotal=returnHuangTotal, GiftTotal=giftTotal,SubUnit="合计:"}
            };
            return dt;
        }
        public static System.Data.DataTable QueryProductSaleDetailDaysPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref decimal recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var store = nvl["store"];
            var dt = dal.QueryProductSaleDetailDays(nl);
            decimal purchaseNumber = 0, saleTotal = 0, buyTotal = 0, mle = 0, huanNumber = 0, huanTotal = 0, returnNumber = 0, returnTotal = 0, giftNumber = 0, giftTotal = 0, tuiHuanTotal = 0, tuiHuanNumber=0;
            if(dt.Rows.Count>0)
            {
                var dr = dt.Rows[0];
                purchaseNumber += Convert.ToDecimal(dr["PurchaseNumbers"]);
                saleTotal += Convert.ToDecimal(dr["SaleTotals"]);
                buyTotal += Convert.ToDecimal(dr["BuyTotals"]);
                mle += Convert.ToDecimal(dr["MLEs"]);
                huanNumber += Convert.ToDecimal(dr["huanNumbers"]);
                huanTotal += Convert.ToDecimal(dr["huanTotals"]);
                returnNumber += Convert.ToDecimal(dr["returnNumbers"]);
                returnTotal += Convert.ToDecimal(dr["returnTotals"]);
                tuiHuanNumber += Convert.ToDecimal(dr["TuiHuanNumbers"]);
                tuiHuanTotal += Convert.ToDecimal(dr["TuiHuanTotals"]);
                giftNumber += Convert.ToDecimal(dr["giftNumbers"]);
                giftTotal += Convert.ToDecimal(dr["giftTotals"]);
            }
            recordCount = 0;
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = new List<object>() { 
                new {PurchaseNumber=purchaseNumber,SaleTotal=saleTotal,BuyTotal=buyTotal,MLE=mle,MLL=(saleTotal==0?0:mle/saleTotal).ToString("p"),
                    TuiHuanNumber=tuiHuanNumber,TuiHuanTotal=tuiHuanTotal,HuanNumber = huanNumber, HuanTotal = huanTotal, ReturnNumber = returnNumber, ReturnTotal = returnTotal, GiftNumber = giftNumber, GiftTotal=giftTotal,CreateDT="合计:"}
            };
            return dt;
        }
        /// <summary>
        /// 销售员日结报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryCashierSaleOrderDayPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = nl["date"];
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).ToString("yyyy-MM-dd");
            }
            
            nl.Add("datelen", "10");
            var ds = dal.QueryCashierSaleOrderDay(nl);
            var dt = ds.Tables[0];
            var dttotal = ds.Tables[1];
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }
            footer = dttotal;
            return dt;
        }
        /// <summary>
        /// 销售员月结报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryCashierSaleOrderMonthPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-01");
                nl["date2"] = DateTime.Now.AddMonths(1).AddDays(-1).ToString("yyyy-MM-01");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Parse(nl["date"]).ToString("yyyy-MM-01");
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
            else
            {
                nl["date"] = DateTime.Parse(nl["date"]).ToString("yyyy-MM-01");
                nl["date2"] = DateTime.Parse(nl["date2"]).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
            nl.Add("datelen", "7");
            var ds = dal.QueryCashierSaleOrderDay(nl);
            var dt = ds.Tables[0];
            var dttotal = ds.Tables[1];
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = dttotal;
            return dt;
        }
        /// <summary>
        /// 导购员日结报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QuerySalerSaleOrderDayPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = nl["date"];
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).ToString("yyyy-MM-dd");
            }

            nl.Add("datelen", "10");
            var ds = dal.QuerySalerSaleOrderDay(nl);
            var dt = ds.Tables[0];
            var dttotal = ds.Tables[1];
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }
            footer = dttotal;
            return dt;
        }
        /// <summary>
        /// 导购员月结报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QuerySalerSaleOrderMonthPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-01");
                nl["date2"] = DateTime.Now.AddMonths(1).AddDays(-1).ToString("yyyy-MM-01");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Parse(nl["date"]).ToString("yyyy-MM-01");
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
            else
            {
                nl["date"] = DateTime.Parse(nl["date"]).ToString("yyyy-MM-01");
                nl["date2"] = DateTime.Parse(nl["date2"]).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
            nl.Add("datelen", "7");
            var ds = dal.QuerySalerSaleOrderDay(nl);
            var dt = ds.Tables[0];
            var dttotal = ds.Tables[1];
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = dttotal;
            return dt;
        }
        /// <summary>
        /// 门店日结报表 
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryStoreSaleOrderDayPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-01");
                nl["date2"] = nl["date"];
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).ToString("yyyy-MM-dd");
            }
            else
            {
                nl["date"] = DateTime.Parse(nl["date"]).ToString("yyyy-MM-01");
                nl["date2"] = DateTime.Parse(nl["date2"]).AddMonths(1).ToString("yyyy-MM-01");
            }
            nl.Add("datelen", "10");
            var ds = dal.QueryStoreSaleOrderDay(nl);
            var dt = ds.Tables[0];
            var dttotal = ds.Tables[1];
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = dttotal;
            return dt;
        }
        /// <summary>
        /// 门店月结报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryStoreSaleOrderMonthPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-01");
                nl["date2"] = DateTime.Now.AddMonths(1).ToString("yyyy-MM-01");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Parse(nl["date"]).ToString("yyyy-MM-01");
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddMonths(1).ToString("yyyy-MM-dd");
            }
            else
            {
                nl["date"] = DateTime.Parse(nl["date"]).ToString("yyyy-MM-01");
                nl["date2"] = DateTime.Parse(nl["date2"]).AddMonths(1).ToString("yyyy-MM-01");
            }
            nl.Add("datelen", "7");
            var ds = dal.QueryStoreSaleOrderDay(nl);
            var dt = ds.Tables[0];
            var dttotal = ds.Tables[1];
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = dttotal;
            return dt;
        }
        /// <summary>
        /// 进销存统计报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryInvoicingSummaryPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            if(!nvl["categorySn"].IsNullOrEmpty())
            {
                var categorysn = nl["categorySn"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["categorySn"] = string.Join(",", categorys);
            }
            var dt = dal.QueryInvoicingSummary(nl);
            #region 各变量赋值
           
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    if (dr[col] is DBNull && col.DataType == typeof(decimal))
                        dr.SetValue(col.ColumnName, 0);
                }
            }
            var dtfooter = new DataTable();
            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName.EndsWith("s"))
                {
                    dtfooter.Columns.Add(col.ColumnName.Substring(0, col.ColumnName.Length - 1), col.DataType);
                }
            }
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                var drn = dtfooter.NewRow();
                foreach (DataColumn dc in dtfooter.Columns)
                {
                    drn.SetValue(dc.ColumnName, dr.GetValue(dc.ColumnName + "s").ToType<decimal>());
                }
                dtfooter.Columns.Add("品名");
                dtfooter.Columns.Add("批发毛利率");
                dtfooter.Columns.Add("零售毛利率");
                dtfooter.Columns.Add("销售毛利率");
                drn.SetValue("品名", "合计:");
                var 批发销售金额= drn["批发销售金额"].ToType<decimal>();
                var 批发销售成本 = drn["批发销售成本"].ToType<decimal>();
                var 零售金额 = drn["零售金额"].ToType<decimal>();
                var 零售成本 = drn["零售成本"].ToType<decimal>();
                var 批发毛利率 = (批发销售金额 == 0 ? 0 : ((批发销售金额 - 批发销售成本) / 批发销售金额)).ToString("p");
                var 零售毛利率 = (零售金额 == 0 ? 0 : ((零售金额 - 零售成本) / 零售金额)).ToString("p");
                var 销售毛利率 = 零售毛利率;
                drn.SetValue("批发毛利率", 批发毛利率);
                drn.SetValue("零售毛利率", 零售毛利率);
                drn.SetValue("销售毛利率", 销售毛利率);
                dtfooter.Rows.Add(drn);
            }
            
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }
            else if(dtfooter.Rows.Count>0)
            {
                var dr=dt.NewRow();
                foreach(DataColumn col in dtfooter.Columns)
                {
                    dr[col.ColumnName] = dtfooter.Rows[0][col];
                }
                dt.Rows.Add(dr);
            }
            
            footer = dtfooter;
            #endregion

            return dt;
        }
        /// <summary>
        /// 供应商销售明细月报
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QuerySupplierSaleDetailPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy-MM");
            var dt = dal.QuerySupplierSaleDetail(nl);
            int purchaseNumber = 0;
            decimal saleTotal = 0, buyTotal = 0, mle = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                purchaseNumber += Convert.ToInt32(dr["PurchaseNumber"]);
                saleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                buyTotal += Convert.ToDecimal(dr["BuyTotal"]);
                mle += Convert.ToDecimal(dr["MLE"]);
            }
            footer = new List<object>() { 
                new {PurchaseNumber=purchaseNumber,SaleTotal=saleTotal,BuyTotal=buyTotal,MLE=mle,MLL=(saleTotal==0?0:mle/saleTotal).ToString("p"),SysPrice="合计:"}
            };
            return dt;
        }
        /// <summary>
        /// 品牌销售明细月报
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryBrandSaleDetailPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy-MM");
            var dt = dal.QueryBrandSaleDetail(nl);
            int purchaseNumber = 0;
            decimal saleTotal = 0, buyTotal = 0, mle = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                purchaseNumber += Convert.ToInt32(dr["PurchaseNumber"]);
                saleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                buyTotal += Convert.ToDecimal(dr["BuyTotal"]);
                mle += Convert.ToDecimal(dr["MLE"]);
            }
            footer = new List<object>() { 
                new {PurchaseNumber=purchaseNumber,SaleTotal=saleTotal,BuyTotal=buyTotal,MLE=mle,MLL=(saleTotal==0?0:mle/saleTotal).ToString("p"),SysPrice="合计:"}
            };
            return dt;
        }
        /// <summary>
        /// 商品销售明细日报
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryProductSaleDetailDayPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.Parse(nl["date"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var dt = dal.QueryProductSaleDetailDay(nl);
            int purchaseNumber = 0;
            decimal saleTotal = 0, buyTotal = 0, mle = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                purchaseNumber += Convert.ToInt32(dr["PurchaseNumber"]);
                saleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                buyTotal += Convert.ToDecimal(dr["BuyTotal"]);
                mle += Convert.ToDecimal(dr["MLE"]);
            }
            footer = new List<object>() { 
                new {PurchaseNumber=purchaseNumber,SaleTotal=saleTotal,BuyTotal=buyTotal,MLE=mle,MLL=(saleTotal==0?0:mle/saleTotal).ToString("p"),SysPrice="合计:"}
            };
            return dt;
        }
        /// <summary>
        /// 商品销售明细月报
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryProductSaleDetailPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy-MM");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var dt = dal.QueryProductSaleDetail(nl);
            int purchaseNumber = 0;
            decimal saleTotal = 0, buyTotal = 0, mle = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                purchaseNumber += Convert.ToInt32(dr["PurchaseNumber"]);
                saleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                buyTotal += Convert.ToDecimal(dr["BuyTotal"]);
                mle += Convert.ToDecimal(dr["MLE"]);
            }
            footer = new List<object>() { 
                new {PurchaseNumber=purchaseNumber,SaleTotal=saleTotal,BuyTotal=buyTotal,MLE=mle,MLL=(saleTotal==0?0:mle/saleTotal).ToString("p"),SysPrice="合计:"}
            };
            return dt;
        }
        /// <summary>
        /// 特价销售明细月报
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryPromotionSaleDetailPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy-MM");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn= nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var dt = dal.QueryCommodityPromotionSaleDetailPageList(nl);
            decimal purchaseNumber=0,saleTotal = 0, buyTotal = 0, mle = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                purchaseNumber += Convert.ToDecimal(dr["PurchaseNumber"]);
                saleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                buyTotal += Convert.ToDecimal(dr["BuyTotal"]);
                mle += Convert.ToDecimal(dr["MLE"]);
            }
            footer = new List<object>() { 
                new {PurchaseNumber=purchaseNumber,SaleTotal=saleTotal,BuyTotal=buyTotal,MLE=mle,MLL=(saleTotal==0?0:mle/saleTotal).ToString("p"),DiscountPrice="合计:"}
            };
            return dt;
        }
        /// <summary>
        /// 获取供应商销售汇总月报数据
        /// </summary>
        /// <param name="nvl">查询条件名值对</param>
        /// <param name="footer">统计页脚</param>
        /// <returns></returns>
        public static System.Data.DataTable QuerySupplierStatisticsPageList(NameValueCollection nvl, ref object footer)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy-MM");
            var dt = dal.QuerySupplierStatistics(nl);
            dt.Columns.Add("Percent", typeof(string));
            decimal saleTotal = 0, buyTotal = 0, jml = 0, percent = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                saleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                buyTotal += Convert.ToDecimal(dr["BuyTotal"]);
                jml += Convert.ToDecimal(dr["JML"]);
            }
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                if (saleTotal == 0)
                    dr["Percent"] = "0%";
                else
                    dr["Percent"] = decimal.Round(decimal.Divide((Convert.ToDecimal(dr["SaleTotal"])), saleTotal) * 100, 1) + "%";
                percent += Convert.ToDecimal(dr["Percent"].ToString().Replace("%", ""));
            }
            footer = new List<object>() { 
                new { SaleTotal = saleTotal, BuyTotal = buyTotal, JML = jml, JMLL = saleTotal == 0 ? "0" : decimal.Round(decimal.Divide(jml, saleTotal) * 100, 1) + "%", Percent = percent + "%", SupplierTitle = "合计:" }
            };
            return dt;
        }

        public static System.Data.DataTable QueryBigCategoryStatisticsPageList(NameValueCollection nvl, ref object footer)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy-MM");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var dt = dal.QueryBigCategoryStatistics(nl);
            dt.Columns.Add("Percent", typeof(string));
            decimal saleTotal = 0, buyTotal = 0, jml = 0, percent = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                saleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                buyTotal += Convert.ToDecimal(dr["BuyTotal"]);
                jml += Convert.ToDecimal(dr["JML"]);
            }
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                dr["Percent"] = decimal.Round(decimal.Divide((Convert.ToDecimal(dr["SaleTotal"])), saleTotal) * 100, 1) + "%";
                percent += Convert.ToDecimal(dr["Percent"].ToString().Replace("%", ""));
            }
            footer = new List<object>() { 
                new { SaleTotal = saleTotal, BuyTotal = buyTotal, JML = jml, JMLL = saleTotal == 0 ? "0" : decimal.Round(decimal.Divide(jml, saleTotal) * 100, 1) + "%", Percent = percent + "%", BigCategoryTitle = "合计:" }
            };
            return dt;
        }
        /// <summary>
        /// 首页当日分类销售统计
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static List<SeriesModel> QueryIndexSaleCategorys(ref List<string> categories)
        {
            var dt = dal.QueryIndexSaleDayList(DateTime.Now.ToString("yyyy-MM-dd"), "1", Sys.CurrentUser.StoreId);
            var series = new SeriesModel()
            {
                name = "销量",
                type = "pie",
                radius = "50%",
                center = new string[] { "47%", "70%" },
            };
            var list = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                var mid = dr["MidCategoryTitle"].ToString();
                list.Add(new { value = dr["PurchaseNumber"], name = mid });
                if (!categories.Contains(mid))
                    categories.Add(mid);
            }
            series.data = list;
            return new List<SeriesModel>() { series };
        }
        /// <summary>
        /// 首页当日小时销售统计
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static List<SeriesModel> QueryIndexSaleHour(List<string> categories, TimeSpan[] hours)
        {
            var dt = dal.QueryIndexSaleDayList(DateTime.Now.ToString("yyyy-MM-dd"), "2", Sys.CurrentUser.StoreId);
            var list = new List<SeriesModel>();
            foreach (var cate in categories)
            {
                var series = new SeriesModel()
                {
                    name = cate,
                    type = "line",
                    stack = cate
                };
                series.data = new List<object>();
                list.Add(series);
                for (int i = 0; i < hours.Count(); i++)
                {
                    var min = hours[i].Hours;
                    var max = 0;
                    if (i == hours.Count() - 1)
                        max = 24;
                    else
                        max = hours[i + 1].Hours;
                    var drs = dt.Select("MidCategoryTitle='" + cate + "' and Hour>=" + min + " and Hour<" + max);
                    int data = 0;
                    if (drs.Length > 0)
                        data = drs.Sum(o => Convert.ToInt32(o["PurchaseNumber"]));
                    series.data.Add(data);
                }
            }
            return list;
        }
        /// <summary>
        /// 首页门店分类销售统计
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="stores"></param>
        /// <returns></returns>
        public static List<dynamic> QueryIndexSaleStore(List<string> categories, ref List<string> stores)
        {
            var dt = dal.QueryIndexSaleDayList(DateTime.Now.ToString("yyyy-MM-dd"), "3", Sys.CurrentUser.StoreId);
            var list = new List<dynamic>();

            foreach (DataRow dr in dt.Rows)
            {
                var title = dr["StoreTitle"].ToString();
                if (!stores.Contains(title) && stores.Count < 5)
                    stores.Add(title);
            }
            if (!Sys.CurrentUser.IsStore)
            {
                stores.Clear(); stores.Add("销售额");
            }
            int index = 1;
            foreach (var store in stores)
            {
                dynamic series = new System.Dynamic.ExpandoObject();
                series.name = store;
                series.type = "bar";
                if (index == 1)
                    series.xAxisIndex = index;
                series.data = new List<object>();
                if (Sys.CurrentUser.IsStore)
                {
                    foreach (var cate in categories)
                    {
                        var drs = dt.Select("StoreTitle='" + store + "' and MidCategoryTitle='" + cate + "'");
                        decimal data = 0;
                        if (drs.Length > 0)
                            data = Convert.ToDecimal(drs[0]["PurchaseNumber"]);
                        series.data.Add(data);
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var data = Convert.ToDecimal(dr["PurchaseNumber"]);
                        series.data.Add(data);
                    }
                }
                list.Add(series);
                index++;
            }
            return list;
        }

        public static object QueryGiftAnnualStatisticalPageList(NameValueCollection nvl, ref object footer)
        {
            var dt = dal.QueryGiftAnnualStatisticalPageList(nvl);

            return dt;

        }

        public static object QueryGiftMonthlyStatisticalPageList(NameValueCollection nvl, ref object footer)
        {
            var dt = dal.QueryGiftMonthlyStatisticalPageList(nvl);
            return dt;

        }


        /// <summary>
        /// 销售同比汇总报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QuerySalesSummaryPageList(System.Collections.Specialized.NameValueCollection nvl)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["typeField"] == "AverageAnnual")
            {
                if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy");
            }
            else if (nl["typeField"] == "AverageMonthly")
            {
                if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy-MM");
            }
            else 
            {
                if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
            }
            var dt = dal.QuerySalesSummary(nl);

            return dt;
        }
        /// <summary>
        /// 大类汇总月报
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryBigCategorySummaryPageList(NameValueCollection nvl, ref object footer)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy-MM");
            var dt = dal.QueryBigCategorySummary(nl);
            decimal sumSameSaleTotal = 0, sumSaleTotal = 0, sumSameJML = 0, sumJML = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                sumSameSaleTotal += Convert.ToDecimal(dr["SameSaleTotal"]);
                sumSaleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                sumSameJML += Convert.ToDecimal(dr["SameJML"]);
                sumJML += Convert.ToDecimal(dr["JML"]);

            }
            footer = new List<object>() { 
                new {
                    SameSaleTotal = sumSameSaleTotal,SaleTotal= sumSaleTotal,SameJML=sumSameJML, JML =sumJML, 
                    JMLL = sumSaleTotal == 0 ? "-" :  Math.Round((sumJML / sumSaleTotal) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    SameJMLL = sumSameSaleTotal == 0 ? "-" :  Math.Round((sumSameJML / sumSameSaleTotal) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    SaleYOY = sumSameSaleTotal == 0 ? "-" : Math.Round(((sumSaleTotal - sumSameSaleTotal) / sumSameSaleTotal) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    JMLYOY = sumSameJML == 0 ? "-" :  Math.Round(((sumJML - sumSameJML) / sumSameJML) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    JMLLYOY = sumSameJML == 0 ? "-" :  Math.Round(((sumJML - sumSameJML) / sumSameJML) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    BigCategoryTitle = "合计:"
                }
            };
            return dt;
        }

        /// <summary>
        /// 供应商汇总月报
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QuerySupplierSummaryPageList(NameValueCollection nvl, ref object footer)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy-MM");
            var dt = dal.QuerySupplierSummary(nl);
            decimal sumSameSaleTotal = 0, sumSaleTotal = 0, sumSameJML = 0, sumJML = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                sumSameSaleTotal += Convert.ToDecimal(dr["SameSaleTotal"]);
                sumSaleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                sumSameJML += Convert.ToDecimal(dr["SameJML"]);
                sumJML += Convert.ToDecimal(dr["JML"]);

            }
            footer = new List<object>() { 
                new {
                    SameSaleTotal = sumSameSaleTotal,SaleTotal= sumSaleTotal,SameJML=sumSameJML, JML =sumJML, 
                    JMLL = sumSaleTotal == 0 ? "-" :  Math.Round((sumJML / sumSaleTotal) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    SameJMLL = sumSameSaleTotal == 0 ? "-" :  Math.Round((sumSameJML / sumSameSaleTotal) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    SaleYOY = sumSameSaleTotal == 0 ? "-" : Math.Round(((sumSaleTotal - sumSameSaleTotal) / sumSameSaleTotal) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    JMLYOY = sumSameJML == 0 ? "-" :  Math.Round(((sumJML - sumSameJML) / sumSameJML) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    JMLLYOY = sumSameJML == 0 ? "-" :  Math.Round(((sumJML - sumSameJML) / sumSameJML) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    SupplierTitle = "合计:"
                }
            };
            return dt;
        }


        /// <summary>
        /// 同比明细月报
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryProductSaleSummaryPageList(NameValueCollection nvl, ref object footer)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty()) nl["date"] = DateTime.Now.ToString("yyyy-MM");
            var dt = dal.QueryProductSaleSummary(nl);
            decimal sumSameSaleTotal = 0, sumSaleTotal = 0, sumChainSaleTotal = 0, sumSameJML = 0, sumJML = 0, sumChainJML = 0,
                    sumPurchaseNumber = 0, sumSamePurchaseNumber = 0, sumChainPurchaseNumber = 0,
                    sumBuyPrice = 0, sumSameBuyPrice = 0, sumChainBuyPrice = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                sumSameSaleTotal += Convert.ToDecimal(dr["SameSaleTotal"]);
                sumSaleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                sumChainSaleTotal += Convert.ToDecimal(dr["ChainSaleTotal"]);
                sumSameJML += Convert.ToDecimal(dr["SameJML"]);
                sumJML += Convert.ToDecimal(dr["JML"]);
                sumChainJML += Convert.ToDecimal(dr["ChainJML"]);
                sumPurchaseNumber += Convert.ToDecimal(dr["PurchaseNumber"]);
                sumSamePurchaseNumber += Convert.ToDecimal(dr["SamePurchaseNumber"]);
                sumChainPurchaseNumber += Convert.ToDecimal(dr["ChainPurchaseNumber"]);
                sumBuyPrice += Convert.ToDecimal(dr["BuyPrice"]);
                sumSameBuyPrice += Convert.ToDecimal(dr["SameBuyPrice"]);
                sumChainBuyPrice += Convert.ToDecimal(dr["ChainBuyPrice"]);

            }
            footer = new List<object>() { 
                new {
                    SameSaleTotal = sumSameSaleTotal,SaleTotal= sumSaleTotal,ChainSaleTotal = sumChainSaleTotal,
                    SameJML=sumSameJML, JML =sumJML, ChainJML= sumChainJML,
                    JMLL = sumSaleTotal == 0 ? "-" :  Math.Round((sumJML / sumSaleTotal) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    SameJMLL = sumSameSaleTotal == 0 ? "-" :  Math.Round((sumSameJML / sumSameSaleTotal) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    ChainJMLL = sumChainSaleTotal == 0 ? "-" :  Math.Round((sumChainJML / sumChainSaleTotal) * 100, 2, MidpointRounding.AwayFromZero) + "%",
                    PurchaseNumber = sumPurchaseNumber,SamePurchaseNumber=sumSamePurchaseNumber,ChainPurchaseNumber=sumChainPurchaseNumber,
                    BuyPrice = sumBuyPrice,SameBuyPrice=sumSameBuyPrice,ChainBuyPrice=sumChainBuyPrice,
                    Barcode = "合计:"
                }
            };
            return dt;
        }
        public static object SaleDetail(NameValueCollection nl,ref int count, ref object footer)
        {
            DateTime start = DateTime.Now.Date;
            DateTime end = DateTime.Now.Date;
            if (!nl["date"].IsNullOrEmpty()) start = DateTime.Parse(nl["date"]);
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                end = start.AddDays(1);
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                end = DateTime.Parse(nl["date"]).AddDays(1);
            }
            else
            {
                end = DateTime.Parse(nl["date2"]).AddDays(1);
            }
            var store = nl["store"];
            var barcode = nl["barcode"];
            var paysn = nl["paysn"];
            var repSale = BaseService<Entity.SaleOrders>.CurrentRepository.QueryEntity.Where(o=>o.CompanyId==CommonService.CompanyId);
            repSale = repSale.Where(o => o.CreateDT >= start && o.CreateDT < end && !o.IsTest);
            if (!store.IsNullOrEmpty())
                repSale = repSale.Where(o => o.StoreId == store);

            var query = from b in BaseService<Entity.SaleDetail>.CurrentRepository.Entities
                        join c in BaseService<Entity.VwProduct>.CurrentRepository.Entities on new { b.CompanyId, b.Barcode } equals new { c.CompanyId, c.Barcode }
                        join d in repSale on b.PaySN equals d.PaySN
                        join e in UserInfoService.CurrentRepository.Entities on d.CreateUID equals e.UID into tmp
                        from t in tmp.DefaultIfEmpty()
                        let o = from a in repSale where b.PaySN == a.PaySN select a
                        where o.Any()
                        orderby b.PaySN ascending
                        select new
                        {
                            d.CreateDT,
                            PaySN=d.CustomOrderSn,
                            b.Barcode,
                            b.PurchaseNumber,
                            b.SysPrice,
                            AveragePrice = Math.Round(b.AveragePrice, 2),
                            b.BuyPrice,
                            c.Title,
                            SubTotal = b.PurchaseNumber * b.AveragePrice,
                            t.FullName,
                            c.CategorySN,
                            c.SubUnit
                        };
            var query2 = from a in BaseService<Entity.Bundling>.CurrentRepository.Entities.Where(o=>o.CompanyId==CommonService.CompanyId)
                         join b in BaseService<Entity.SaleDetail>.CurrentRepository.Entities on new { a.CompanyId, Barcode= a.NewBarcode } equals new { b.CompanyId, b.Barcode }
                         join d in repSale on b.PaySN equals d.PaySN
                         join e in UserInfoService.CurrentRepository.Entities on d.CreateUID equals e.UID into tmp
                         from t in tmp.DefaultIfEmpty()
                         select new
                         {
                             CreateDT = DateTime.Now,
                             PaySN=d.CustomOrderSn,
                             Barcode=a.NewBarcode,
                             b.PurchaseNumber,
                             b.SysPrice,
                             AveragePrice=Math.Round(b.AveragePrice,2),
                             BuyPrice=a.BuyPrices,
                             a.Title,
                             SubTotal = b.PurchaseNumber * b.ActualPrice,
                             t.FullName,
                             CategorySN=0,
                             SubUnit="捆"
                         };
            var q = query.Union(query2);
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorys = ProductCategoryService.GetChildSNs(new List<int>() { int.Parse(nl["parentType"]) }, true);
                q = q.Where(o => categorys.Contains(o.CategorySN));
            }
            if (!paysn.IsNullOrEmpty())
            {
                q = q.Where(o => o.PaySN == paysn);
            }
            if (!barcode.IsNullOrEmpty())
            {
                q = q.Where(o => o.Barcode == barcode);
            }
            count = q.Count();
            var list = q.ToPageList();
            footer = new List<object>() { 
                new {PurchaseNumber=list.Sum(o=>o.PurchaseNumber),SubTotal=list.Sum(o=>o.SubTotal),Title="本页合计:"}
            };
            return list;
        }
        public static object OrderDetail(NameValueCollection nl, ref object footer)
        {
            DateTime start = DateTime.Now.Date;
            DateTime end = DateTime.Now.Date;
            if (!nl["date"].IsNullOrEmpty()) start = DateTime.Parse(nl["date"]);
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                end = start.AddDays(1);
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                end = DateTime.Parse(nl["date"]).AddDays(1);
            }
            else
            {
                end = DateTime.Parse(nl["date2"]).AddDays(1);
            }
            var nvl = new NameValueCollection() { nl};
            nvl["date"] = start.ToString("yyyy-MM-dd");
            nvl["date2"] = end.ToString("yyyy-MM-dd");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorys = ProductCategoryService.GetChildSNs(new List<int>() { int.Parse(nl["parentType"]) }, true);
                nvl["parentType"]=string.Join(",",categorys);
            }
            var dt = dal.ProductOrderDetailList(nvl);
            decimal total = 0, IndentNum=0;
            foreach(DataRow dr in dt.Rows)
            {
                total += (decimal)dr["OrderTotal"];
                IndentNum += (decimal)dr["IndentNum"];
            }
            footer = new List<object>() { 
                new {Title="合计:",OrderTotal=total,IndentNum=IndentNum}
            };
            return dt;
        }
        /// <summary>
        /// 会员销售明细日报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryMembersSaleDetailDayPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            var dt = dal.QueryMembersSaleDetailDay(nl);
            int quantity = 0;
            decimal weigh = 0, saleTotal = 0, preferentialPrice = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                quantity += Convert.ToInt32(dr["Quantity"]);
                weigh += Convert.ToDecimal(dr["Weigh"]);
                saleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                preferentialPrice += Convert.ToDecimal(dr["PreferentialPrice"]);
                dr["SaleTotal"] = Convert.ToDecimal(dr["SaleTotal"]).ToString("N2");
                dr["PreferentialPrice"] = Convert.ToDecimal(dr["PreferentialPrice"]).ToString("N2");
            }
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }
            footer = new List<object>() { 
                new {Quantity=quantity,Weigh=weigh,SaleTotal=saleTotal,PreferentialPrice=preferentialPrice,Weixin="本页合计:"}
            };
            return dt;
        }


        /// <summary>
        /// 导购销售明细日报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable QueryShoppingGuideSaleDetailDayPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.Parse(nl["date"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            var dt = dal.QueryShoppingGuideSaleDetailDay(nl);
            int quantity = 0;
            decimal weigh = 0, saleTotal = 0, preferentialPrice = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                quantity += Convert.ToInt32(dr["Quantity"]);
                weigh += Convert.ToDecimal(dr["Weigh"]);
                saleTotal += Convert.ToDecimal(dr["SaleTotal"]);
                preferentialPrice += Convert.ToDecimal(dr["PreferentialPrice"]);
                dr["SaleTotal"] = Convert.ToDecimal(dr["SaleTotal"]).ToString("N2");
                dr["PreferentialPrice"] = Convert.ToDecimal(dr["PreferentialPrice"]).ToString("N2");
            }
            footer = new List<object>() { 
                new {Quantity=quantity,Weigh=weigh,SaleTotal=saleTotal.ToString("N2"),PreferentialPrice=preferentialPrice.ToString("N2"),PaySN="合计:"}
            };
            return dt;
        }
        /// <summary>
        /// 订单详情（会员销售明细日报表、导购销售明细日报表）
        /// </summary>
        /// <param name="paySN"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static object SaleOrderDetail(string paySN, ref object footer)
        {
            var query = from b in BaseService<Entity.SaleDetail>.CurrentRepository.Entities
                        join c in BaseService<Entity.VwProduct>.CurrentRepository.Entities on b.Barcode equals c.Barcode
                        join d in BaseService<Entity.SaleOrders>.CurrentRepository.Entities on b.PaySN equals d.PaySN
                        join e in UserInfoService.CurrentRepository.Entities on d.CreateUID equals e.UID into tmp
                        from t in tmp.DefaultIfEmpty()
                        join f in UserInfoService.CurrentRepository.Entities on d.Salesman equals f.UID into tmp1
                        from s in tmp1.DefaultIfEmpty()
                        where b.PaySN == paySN && !d.IsTest
                        select new
                        {
                            d.CreateDT,
                            ShoppingGuide = s.FullName,
                            Cashier = t.FullName,
                            PaySN=d.CustomOrderSn,
                            b.Barcode,
                            b.PurchaseNumber,
                            SysPrice = b.SysPrice,
                            ActualPrice = b.ActualPrice,
                            BuyPrice = b.BuyPrice,
                            ProductName = c.Title,
                            SubTotal = b.PurchaseNumber * b.ActualPrice,
                            c.SubUnit
                        };
            //var list = query.ToList();
            var query2 = from a in BaseService<Entity.Bundling>.CurrentRepository.Entities
                         join b in BaseService<Entity.SaleDetail>.CurrentRepository.Entities on a.NewBarcode equals b.Barcode
                         join c in BaseService<Entity.SaleOrders>.CurrentRepository.Entities on b.PaySN equals c.PaySN
                         where b.PaySN == paySN
                         select new {
                             CreateDT=DateTime.Now,
                             ShoppingGuide = "",
                             Cashier = "",
                             PaySN=c.CustomOrderSn,
                             b.Barcode,
                             b.PurchaseNumber,
                             SysPrice = b.SysPrice,
                             ActualPrice = b.ActualPrice,
                             BuyPrice = b.BuyPrice,
                             ProductName = a.Title,
                             SubTotal = b.PurchaseNumber * b.ActualPrice,
                             SubUnit="捆"
                         };
            var list= query.Union(query2).ToList();
            footer = new List<object>() { 
                new {PurchaseNumber=list.Sum(o=>o.PurchaseNumber),SysPrice=list.Sum(o=>o.SysPrice).ToString("N2"),
                    ActualPrice=list.Sum(o => o.ActualPrice).ToString("N2"),BuyPrice=list.Sum(o=>o.BuyPrice).ToString("N2"),
                    SubTotal=list.Sum(o=>o.SubTotal).ToString("N2"),Title="合计:"}
            };
            return list;

        }
        /// <summary>
        /// 商品采购明细报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable ProductOrderDetailPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).ToString("yyyy-MM-dd");
            }
            else
                nl["date2"] = DateTime.Parse(nl["date2"]).ToString("yyyy-MM-dd");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var store = nvl["store"];
            var dt = dal.ProductOrderDetails(nl);
            decimal purchaseNumber=0,saleTotal = 0, buyTotal = 0, rateTotal = 0,giftNum=0;
            if(dt.Rows.Count>0)
            {
                var dr = dt.Rows[0];
                purchaseNumber += Convert.ToDecimal(dr["数量s"]);
                giftNum += Convert.ToDecimal(dr["赠品数量s"]);
                saleTotal += Convert.ToDecimal(dr["零售金额s"]);
                buyTotal += Convert.ToDecimal(dr["采购金额s"]);
                rateTotal += Convert.ToDecimal(dr["未税金额s"]);
            }
            
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = new List<object>() { 
                new {数量=purchaseNumber,赠品数量=giftNum,零售金额=saleTotal,采购金额=buyTotal,未税金额=rateTotal,Title="合计:"}
            };
            return dt;
        }
        
        /// <summary>
        /// 销售员日结报表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static System.Data.DataTable StoreStockDetailPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };

            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var dt = dal.StoreStockDetails(nl);
            decimal stockNum = 0, saleTotal = 0, stockTotal1 = 0, stockTotal2 = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                stockNum += Convert.ToDecimal(dr["StockNumber"]);
                saleTotal += Convert.ToDecimal(dr["售价金额"]);
                stockTotal1 += Convert.ToDecimal(dr["成本金额含"]);
                stockTotal2 += Convert.ToDecimal(dr["成本金额未"]);
            }
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = new List<object>() { 
                new {StockNumber=stockNum,售价金额=saleTotal,成本金额含=stockTotal1,成本金额未=stockTotal2,Title="本页合计:"}
            };
            return dt;
        }
        public static DataTable BeforeSaleSummaryPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var store = nvl["store"];
            var dt = dal.BeforeSaleSummarys(nl);
            decimal purchaseNumber = 0, saleTotal = 0, buyTotal = 0, mle = 0, huanNumber = 0, huanTotal = 0, returnNumber = 0, returnTotal = 0, giftNumber = 0, giftTotal = 0, returnHuangTotal = 0, returnHuangNumber = 0;

            if (dt.Rows.Count > 0)
            {
                purchaseNumber = dt.Rows[0]["PurchaseNumbers"].ToType<decimal>();
                saleTotal = dt.Rows[0]["SaleTotals"].ToType<decimal>();
                buyTotal = dt.Rows[0]["BuyTotals"].ToType<decimal>();
                huanNumber = dt.Rows[0]["HuanNumbers"].ToType<decimal>();
                huanTotal = dt.Rows[0]["HuanTotals"].ToType<decimal>();
                returnHuangNumber = dt.Rows[0]["TuiHuanNumbers"].ToType<decimal>();
                returnHuangTotal = dt.Rows[0]["TuiHuanTotals"].ToType<decimal>();
                returnNumber = dt.Rows[0]["ReturnNumbers"].ToType<decimal>();
                returnTotal = dt.Rows[0]["ReturnTotals"].ToType<decimal>();
                giftNumber = dt.Rows[0]["GiftNumbers"].ToType<decimal>();
                giftTotal = dt.Rows[0]["GiftTotals"].ToType<decimal>();
                mle = dt.Rows[0]["MLES"].ToType<decimal>();
            }
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = new List<object>() { 
                new {PurchaseNumber=purchaseNumber,SaleTotal=saleTotal,BuyTotal=buyTotal,MLE=mle,MLL=(saleTotal==0?0:mle/saleTotal).ToString("p"),
                    HuanNumber = huanNumber, HuanTotal = huanTotal, ReturnNumber = returnNumber, ReturnTotal = returnTotal, GiftNumber = giftNumber, 
                    TuiHuanNumber=returnHuangNumber,TuiHuanTotal=returnHuangTotal, GiftTotal=giftTotal,Title="合计:"}
            };
            return dt;
        }
        public static DataTable BreakagePageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var store = nvl["store"];
            var dt = dal.BreakageSummarys(nl);
            decimal BreakageNumber = 0, BreakageTotal = 0, SaleTotal = 0, 未税金额 = 0;

            if (dt.Rows.Count > 0)
            {
                BreakageNumber = dt.Rows[0]["BreakageNumbers"].ToType<decimal>();
                BreakageTotal = dt.Rows[0]["BreakageTotals"].ToType<decimal>();
                SaleTotal = dt.Rows[0]["SaleTotals"].ToType<decimal>();
                未税金额 = dt.Rows[0]["未税金额s"].ToType<decimal>();
            }
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = new List<object>() { 
                new {BreakageNumber=BreakageNumber,BreakageTotal=BreakageTotal,SaleTotal=SaleTotal,未税金额=未税金额,Title="合计:"}
            };
            return dt;
        }
        public static DataTable HouseMovePageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var store = nvl["store"];
            var dt = dal.HouseMoveSummarys(nl);
            decimal ActualQuantitys = 0, 调价金额 = 0, 未税金额 = 0, 税额 = 0;

            if (dt.Rows.Count > 0)
            {
                ActualQuantitys = dt.Rows[0]["ActualQuantitys"].ToType<decimal>();
                调价金额 = dt.Rows[0]["调价金额s"].ToType<decimal>();
                未税金额 = dt.Rows[0]["未税金额s"].ToType<decimal>();
                税额 = dt.Rows[0]["税额s"].ToType<decimal>();
            }
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = new List<object>() { 
                new {ActualQuantity=ActualQuantitys,调价金额=调价金额,未税金额=未税金额,税额=税额,Title="合计:"}
            };
            return dt;
        }
        public static DataTable OutboundPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var store = nvl["store"];
            var dt = dal.OutboundSummarys(nl);
            decimal OutboundNumber = 0, 出库金额 = 0, 未税金额 = 0, SaleTotals = 0;

            if (dt.Rows.Count > 0)
            {
                OutboundNumber = dt.Rows[0]["OutboundNumbers"].ToType<decimal>();
                出库金额 = dt.Rows[0]["出库金额s"].ToType<decimal>();
                未税金额 = dt.Rows[0]["未税金额s"].ToType<decimal>();
                SaleTotals = dt.Rows[0]["SaleTotals"].ToType<decimal>();
            }
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = new List<object>() { 
                new {OutboundNumber=OutboundNumber,出库金额=出库金额,未税金额=未税金额,SaleTotal=SaleTotals,Title="合计:"}
            };
            return dt;
        }
        public static DataTable WholesalPageList(System.Collections.Specialized.NameValueCollection nvl, ref object footer, ref int recordCount)
        {
            var nl = new NameValueCollection() { nvl };
            if (nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                nl["date2"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (!nl["date"].IsNullOrEmpty() && nl["date2"].IsNullOrEmpty())
            {
                nl["date2"] = DateTime.MaxValue.ToString("yyyy-MM-dd");
            }
            else if (nl["date"].IsNullOrEmpty())
            {
                nl["date"] = "2015-01-01";
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
                nl["date2"] = DateTime.Parse(nl["date2"]).AddDays(1).ToString("yyyy-MM-dd");
            if (!nl["parentType"].IsNullOrEmpty())
            {
                var categorysn = nl["parentType"].Split(',').Select(o => int.Parse(o)).ToList();
                var categorys = ProductCategoryService.GetChildSNs(categorysn, true);
                nl["parentType"] = string.Join(",", categorys);
            }
            var store = nvl["store"];
            var dt = dal.WholesalSummarys(nl);
            decimal OutboundNumber = 0, 批发金额 = 0, 未税金额 = 0, SaleTotals = 0;

            if (dt.Rows.Count > 0)
            {
                OutboundNumber = dt.Rows[0]["OutboundNumbers"].ToType<decimal>();
                批发金额 = dt.Rows[0]["批发金额s"].ToType<decimal>();
                未税金额 = dt.Rows[0]["未税金额s"].ToType<decimal>();
                SaleTotals = dt.Rows[0]["SaleTotals"].ToType<decimal>();
            }
            if (nl["ispage"] != "0")//分页
            {
                if (dt.Rows.Count > 0)
                {
                    recordCount = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                }
            }

            footer = new List<object>() { 
                new {OutboundNumber=OutboundNumber,批发金额=批发金额,未税金额=未税金额,SaleTotal=SaleTotals,Title="合计:"}
            };
            return dt;
        }
        #region 首页销售数据
        /// <summary>
        /// 销售量
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public static int GetSalesVolume(DateTime beginTime, DateTime endTime, string storeId = "")
        {
            int quantity = 0;
            var dt = dal.QueryIndexSalesData("1", beginTime, endTime, CommonService.CompanyId,storeId);
            if (dt != null)
            {
                if (dt.Rows != null)
                {
                    quantity = Convert.ToInt32(dt.Rows[0]["TotalQuantity"]);
                }
            }
            return quantity;
        }

        public static void GetHotProduct(DateTime beginTime, DateTime endTime, out List<string> hotProductName, out List<int> hotProductSaleNum, string storeId = "")
        {
            hotProductName = new List<string>();
            hotProductSaleNum = new List<int>();
            int i = 0;
            var dt = dal.QueryIndexSalesData("2", beginTime, endTime,CommonService.CompanyId, storeId);
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                hotProductName.Add(dr["Title"].ToString());
                hotProductSaleNum.Add(Convert.ToInt32(dr["Quantity"]));

                if (i > 10)
                {
                    break;
                }   
                i++;
            }
        }
        
        #endregion
    }
}
