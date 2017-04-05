﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Utility.Helpers;
namespace Pharos.Logic.DAL
{
    public class CommonDAL:BaseDAL
    {
        public void ExecuteTranList(params BatchTranEntity[] entities)
        {
            if (entities == null || !entities.Any()) return;
            using (EFDbContext db = new EFDbContext())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var entity in entities)
                        {
                            if (!entity.Entities.Any()) continue;
                            var type = entity.EntityType;
                            switch (entity.CmdType)
                            {
                                case CommandEnum.Delete:
                                    db.Set(type).RemoveRange(entity.Entities);
                                    break;
                                case CommandEnum.Insert:
                                    db.Set(type).AddRange(entity.Entities);
                                    break;
                                case CommandEnum.Update:
                                    break;
                                case CommandEnum.Sql:
                                    entity.Entities.Each(o =>
                                    {
                                        string sql = string.Join(";", o);
                                        db.Database.ExecuteSqlCommand(sql);
                                    });
                                    break;
                            }
                        }
                        db.SaveChanges();
                        tran.Commit();
                    }
                    catch(Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public string StockLockValidMsg(string storeId)
        {
            if (!string.IsNullOrWhiteSpace(storeId))
            {
                using (EFDbContext db = new EFDbContext())
                {
                    var sql = string.Format("SELECT dbo.F_StockLockValidMsg('{0}',"+Sys.SysCommonRules.CompanyId+")", storeId);
                    return db.Database.SqlQuery<string>(sql).FirstOrDefault();
                }
            }
            return "";
        }
        public DataSet FindTakeStockPages(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var storeId = nvl["storeId"];
            var userId = nvl["userId"];
            var date1 = nvl["date1"];
            var date2 = nvl["date2"];
            var searchText = (nvl["searchText"] ?? "").Trim();
            var checkBatch = nvl["checkBatch"];
            var barcode = nvl["barcode"];
            var state = nvl["state"];
            var dispType = nvl["dispType"];
            var sure = nvl["sure"];
            var id = nvl["id"];
            var categorysn = nvl["categorysn"];
            //SubstractTotal--盈亏金额 ,ActualTotal--售价金额
            string sql = @"select *,dbo.F_SysPriceByBarcode(LockStoreID,Barcode,CompanyId) SysPrice,
                [dbo].[F_BuyPriceByBarcode](SupplierId,Barcode,CompanyId)*(number-LockNumber) SubstractTotal,
                dbo.F_StoreNameById(LockStoreID,companyid) StoreTitle,
                dbo.F_SupplierNameById(SupplierId) AS SupplierTitle,
                dbo.F_BrandNameForSN(BrandSN) as BrandTitle,
                dbo.F_UserNameById(CreateUID) CreateName ,
                dbo.F_UserNameById(CheckUID) FullName
                from Vw_StockTaking v ";
            string where = " where 1=1 and CompanyId=" + Sys.SysCommonRules.CompanyId;
            if (!storeId.IsNullOrEmpty())
                where += " and LockStoreID='" + storeId + "'";
            if (!userId.IsNullOrEmpty())
                where += " and CheckUID='" + userId + "'";
            if (!searchText.IsNullOrEmpty())
                where += " and (Barcode like '%" + searchText + "%' or Title like '%" + searchText + "%')";
            if (!barcode.IsNullOrEmpty())
                where += " and Barcode='" + barcode + "'";
            if (!checkBatch.IsNullOrEmpty())
                where += " and CheckBatch ='" + checkBatch + "'";
            if (!date1.IsNullOrEmpty() && !date2.IsNullOrEmpty())
                where += " and LockDate between '" + date1 + "' and '" + date2 + "'";
            else if (!date1.IsNullOrEmpty())
                where += " and LockDate>='" + date1 + "'";
            else if (!date2.IsNullOrEmpty())
                where += " and LockDate<'" + date2 + "'";
            if (!id.IsNullOrEmpty())
                where += " and id='" + id + "'";
            if(!state.IsNullOrEmpty())
                where += " and state=" + state + "";
            if (dispType == "1")//差异
                where += " and (SubstractNum is not null and SubstractNum<>0) ";
            else if (dispType == "2")//未盘
                where += " and SubstractNum is null";
            else if (dispType == "3")//已盘
                where += " and SubstractNum is not null";
            if(!sure.IsNullOrEmpty())
                where += " and Sure="+sure;
            if (!categorysn.IsNullOrEmpty())
                where += " and categorysn in(" + categorysn+")";
            recordCount = 0;
            sql = "select *,ActualNumber*SysPrice ActualTotal from ("+sql+where+") nt";
            DataSet ds = new DataSet();
            ds.Tables.Add(base.ExceuteSqlForPage(sql, out recordCount, nvl));
            sql = "SELECT SUM(ActualNumber) ActualNumbers,SUM(LockNumber) LockNumbers,SUM(SubstractNum) SubstractNums,SUM(SubstractTotal) SubstractTotals,SUM(ActualTotal) ActualTotals FROM ("+sql+") nt2";
            ds.Tables.Add(_db.DataTableText(sql, null));
            return ds;
        }
        public DataTable FindTakeStockList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var storeId = nvl["storeId"];
            var userId = nvl["userId"];
            var date1 = nvl["date1"];
            var date2 = nvl["date2"];
            var searchText = (nvl["searchText"] ?? "").Trim();
            var checkBatch = nvl["checkBatch"];
            var barcode = nvl["barcode"];
            var state = nvl["state"];
            var dispType = nvl["dispType"];
            var sure = nvl["sure"];
            var id = nvl["id"];
            var categorysn = nvl["categorysn"];
            //if (!date2.IsNullOrEmpty()) date2 = DateTime.Parse(date2).AddDays(1).ToString("yyyy-MM-dd");
            string sql = @"select * from Vw_StockTaking v ";
            string where = " where 1=1 and CompanyId=" + Sys.SysCommonRules.CompanyId;
            if (!storeId.IsNullOrEmpty())
                where += " and LockStoreID='" + storeId + "'";
            if (!userId.IsNullOrEmpty())
                where += " and CheckUID='" + userId + "'";
            if (!searchText.IsNullOrEmpty())
                where += " and (Barcode like '%" + searchText + "%' or Title like '%" + searchText + "%')";
            if (!barcode.IsNullOrEmpty())
                where += " and Barcode='" + barcode + "'";
            if (!checkBatch.IsNullOrEmpty())
                where += " and CheckBatch ='" + checkBatch + "'";
            if (!date1.IsNullOrEmpty() && !date2.IsNullOrEmpty())
                where += " and LockDate between '" + date1 + "' and '" + date2 + "'";
            else if (!date1.IsNullOrEmpty())
                where += " and LockDate>='" + date1 + "'";
            else if (!date2.IsNullOrEmpty())
                where += " and LockDate<'" + date2 + "'";
            if (!id.IsNullOrEmpty())
                where += " and id='" + id + "'";
            if (!state.IsNullOrEmpty())
                where += " and state=" + state + "";
            if (dispType == "1")//差异
                where += " and (SubstractNum is not null and SubstractNum<>0) ";
            else if (dispType == "2")//未盘
                where += " and SubstractNum is null";
            else if (dispType == "3")//已盘
                where += " and SubstractNum is not null";
            if (!sure.IsNullOrEmpty())
                where += " and Sure=" + sure;
            if (!categorysn.IsNullOrEmpty())
                where += " and categorysn in(" + categorysn + ")";
            recordCount = 0;
            return base.ExceuteSqlForPage(sql + where, out recordCount, nvl);
        }
        public DataTable FindStockLogs(int companyId, string checkBatch, string checkUID,string state)
        {
            string sql = @"SELECT a.Barcode,b.Title,b.SubUnit,b.CategoryTitle,a.Id,a.SysPrice,a.CreateDT,dbo.F_UserNameById(a.CheckUID) FullName,a.CheckUID,a.State,{1},a.InitNumber FROM Vw_StockTaking_Log a 
				INNER JOIN dbo.Vw_Product b ON b.CompanyId = a.CompanyId AND (b.Barcode = a.Barcode OR ','+b.Barcodes+',' LIKE '%,'+a.Barcode+',%')
				WHERE a.CompanyId=@CompanyId and a.CheckBatch=@CheckBatch and a.state in({0}) and a.source in(1,3)";
            if (!checkUID.IsNullOrEmpty())
                sql += " and checkUID='" + checkUID + "'";
            sql = string.Format(sql, state == "0" ? "0" : "0,1", state == "0" ? "a.Number" : "(SELECT top 1 Number FROM StockTakingLog WHERE CompanyId=a.CompanyId AND CheckBatch=a.CheckBatch AND Barcode=a.Barcode AND State=1 ORDER BY CreateDT desc) AS Number");
            //if (state == "0")//初盘过滤已复盘
                //sql += " AND not EXISTS(SELECT 1 FROM StockTakingLog WHERE CompanyId=a.CompanyId AND CheckBatch=a.CheckBatch AND Barcode=a.Barcode AND State=1)";
            sql += "  ORDER BY CreateDT";
            return _db.DataTableText(sql, new SqlParameter[] { 
                new SqlParameter("@CompanyId",companyId),
                new SqlParameter("@CheckBatch",checkBatch)
            });
        }
        public DataTable FindOutInNumDetails(string bars,DateTime takingTime,string storeId,int companyId)
        {
            var parms = new SqlParameter[] { 
                new SqlParameter("@barcodes",bars),
                new SqlParameter("@TakingTime",takingTime.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqlParameter("@storeId",storeId),
                new SqlParameter("@companyId",companyId),
            };
            var dt = _db.DataTable("OutInNumDetails", parms);
            return dt;
        }
        public DataTable GetInventoryBalanceLast(int companyId, string storeId, string barcodes)
        {
            if (!string.IsNullOrWhiteSpace(storeId))
            {
                var sql = string.Format(@"SELECT a.Value Barcode,ISNULL(b.StockNumber,0) Number,a.Id FROM SplitString('{0}',',',1) a
                LEFT JOIN (
					SELECT * FROM dbo.Inventory WHERE CompanyId={1} AND StoreId='{2}'
                ) b ON a.Value=b.Barcode", barcodes,companyId,storeId);
                return _db.DataTableText(sql, null);
            }
            return null;
        }
        public DataTable GetProductsBySupplierId(string supplierId, out int recordCount)
        {

            var sql = @"SELECT *,ISNULL((SELECT SUM(acceptnum) FROM dbo.Vw_OrderList 
                WHERE ((OrderType='采购' AND State=5) OR OrderType='入库') and SupplierId=v.SupplierId AND barcode=v.barcode GROUP BY Barcode),0) AcceptNum FROM dbo.Vw_Product v
                where SupplierId='" + supplierId+"'";
            return base.ExceuteSqlForPage(sql, out recordCount);
        }
        public DataTable GetTuiHuanPages(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var sql = @"SELECT isnull(a.ReturnDT,a.CreateDT) CreateDT,a.PaySN,a.Receive,b.Id,b.Barcode,b.Title,b.ProductCode,b.PurchaseNumber,(CASE WHEN a.State=1 THEN '退整单' WHEN a.type=1 THEN '换货' ELSE '退货' END) ReturnType,dbo.F_UserNameById(a.CreateUID) FullName  FROM dbo.SaleOrders a 
                INNER JOIN dbo.SaleDetail b ON a.PaySN=b.PaySN AND b.CompanyId = a.CompanyId
                WHERE NOT (a.State=0 AND a.Type=0) and a.companyid=" +Sys.SysCommonRules.CompanyId;
            if (!nvl["startDate"].IsNullOrEmpty())
                sql += " and isnull(a.ReturnDT,a.CreateDT)>='" + nvl["startDate"] + "'";
            if (!nvl["endDate"].IsNullOrEmpty())
                sql += " and isnull(a.ReturnDT,a.CreateDT)<DATEADD(DAY,1,'" + nvl["endDate"] + "')";
            if (!nvl["returnType"].IsNullOrEmpty())
            {
                if (nvl["returnType"] == "0")
                    sql += " and a.State=1";
                else
                    sql += " and a.Type=" + nvl["returnType"];
            }
            return base.ExceuteSqlForGroupPage(sql,out recordCount);
        }
        public bool AutoInventoryBalance()
        {
            try
            {
                _db.ExecuteNonQueryText("EXEC dbo.Auto_OrderToInventory;EXEC dbo.Auto_NoBusinessDataBalance_Day @day = '';",null);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 更新退换库存量
        /// </summary>
        public void UpdateTuiHuanStock()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                _db.ExecuteNonQuery("Auto_OrderToInventory");
            });
        }
        public void BulkCopy(string tableName, DataTable dt)
        {
            SqlTransaction sqlTran = null;
            try
            {
                var connStr= Utility.Config.GetAppSettings("ConnectionString");
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    sqlTran= conn.BeginTransaction();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, sqlTran))
                    {
                        bulkCopy.DestinationTableName = tableName;
                        bulkCopy.BatchSize = 2000;//缓存大小
                        foreach (DataColumn dc in dt.Columns)
                            bulkCopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                        bulkCopy.WriteToServer(dt);
                        sqlTran.Commit();
                    }
                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                if (sqlTran != null)
                {
                    try { sqlTran.Rollback(); }
                    catch { }
                }
                throw ex;
            }
        }
        public string GetBrandClassTitle(int? brandsn, int companyId)
        {
            if(brandsn.HasValue)
            {
                var parms=new SqlParameter[]{
                    new SqlParameter("@brandsn",brandsn.Value),
                    new SqlParameter("@companyId",companyId),
                };
                var obj= _db.ExecuteScalarText("SELECT d.Title FROM dbo.SysDataDictionary d WHERE EXISTS( SELECT 1 FROM dbo.ProductBrand WHERE  CompanyId=d.CompanyId AND ClassifyId=DicSN AND BrandSN=@brandsn) AND d.CompanyId=@companyId", parms);
                if (!obj.IsNullOrEmpty()) return obj.ToString();
            }
            return "";
        }
    }
    public static class DbExtend
    {
        public static DataTable SqlQueryForDataTatable(this Database db, string sql,CommandType type, params SqlParameter[] parms)
        {
            using (var conn = (SqlConnection)db.Connection)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = type;
                cmd.CommandTimeout = 60;
                if (parms != null)
                {
                    parms.Each(o =>
                    {
                        cmd.Parameters.Add(o);
                    });
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                conn.Close();//连接需要关闭
                conn.Dispose();
                return table;
            }
        }
    }
}
