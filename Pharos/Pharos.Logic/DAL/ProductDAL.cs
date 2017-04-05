using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.DBFramework;
namespace Pharos.Logic.DAL
{
    internal class ProductDAL:BaseDAL
    {
        /// <summary>
        /// 自动生成10位数
        /// </summary>
        /// <param name="categorySN"></param>
        /// <param name="valueType">计价方式(1-计价2-称重)</param>
        /// <returns></returns>
        internal string GenerateNewBarcode(int companyid, int categorySN,int valueType)
        {
            if (categorySN>0)
            {
                using (EFDbContext db = new EFDbContext())
                {
                    var sql = string.Format("exec Sys_GenerateNewProductCode {0},{1},{2}",companyid, categorySN,valueType);
                    return db.Database.SqlQuery<string>(sql).FirstOrDefault();
                }
            }
            return "";
        }
        internal System.Data.DataTable GetChangePriceList(System.Collections.Specialized.NameValueCollection nvl,out int recordCount)
        {
            string sql = @"SELECT dbo.F_SupplierNameById(a.supplierid) SupplierTitle,dbo.F_TrimStrMore(STUFF((SELECT '、'+w.Title FROM dbo.Warehouse w INNER join dbo.SplitString(a.storeId,',',1) s ON s.Value=w.StoreId where w.companyid=a.CompanyId  FOR XML PATH('')),1,1,''),60) as StoreTitle,
                c.Barcode,c.Title,c.Size,b.CurBuyPrice ,b.CurSysPrice , b.OldSysPrice ,b.OldBuyPrice ,b.OldGrossprofitRate,b.CurGrossprofitRate,b.Memo,b.ChangePriceId,b.Id,b.State Flag,a.State,a.CreateDT,
                (CASE WHEN ISNULL(b.EndDate,'')='' THEN CONVERT(VARCHAR(20),b.StartDate,102)+'-不限' ELSE CONVERT(VARCHAR(20),b.StartDate,102)+'至'+CONVERT(VARCHAR(20),b.EndDate,102) end) DateSpacing
                 FROM ProductChangePrice a
                INNER JOIN ProductChangePriceList b ON b.ChangePriceId=a.Id
                INNER JOIN dbo.ProductRecord c ON b.barcode=c.Barcode and a.CompanyId=c.CompanyId where 1=1 and a.CompanyId=" + nvl["CompanyId"];
            if (!nvl["supplierId"].IsNullOrEmpty())
                sql += " and a.supplierid='" + nvl["supplierId"] + "'";
            if (!nvl["storeId"].IsNullOrEmpty())
                sql += " and ','+a.storeId+',' like '%," + nvl["storeId"] + ",%'";
            if (!nvl["state"].IsNullOrEmpty())
                sql += " and a.state=" + nvl["state"];
            if (!nvl["searchText"].IsNullOrEmpty())
                sql += string.Format(" and c.barcode like '%{0}%' or c.title like '%{0}%'", nvl["searchText"].Trim());
            return base.ExceuteSqlForGroupPage(sql,out recordCount);
        }
        internal System.Data.DataTable GetChangeLogList(string barcode, out int recordCount)
        {
            string sql =string.Format(@"SELECT STUFF((SELECT '、'+w.Title FROM dbo.Warehouse w INNER join dbo.SplitString(a.storeId,',',1) s ON s.Value=w.StoreId FOR XML PATH('')),1,1,'') as StoreTitle,
                b.SysPrice,a.AuditorDT,a.CreateDT,dbo.F_UserNameById(a.AuditorUID) FullName,b.Id FROM ProductChangePrice a
                INNER join dbo.ProductChangePriceList b ON b.ChangePriceId = a.Id
                WHERE a.State=1 AND b.Barcode='{0}'", barcode);
            return base.ExceuteSqlForPage(sql, out recordCount);
        }

        internal System.Data.DataTable GetTradePriceList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            string sql = @"SELECT dbo.F_TrimStrMore(STUFF((SELECT '、'+w.Title FROM dbo.Supplier w INNER join dbo.SplitString(a.Wholesaler,',',1) s ON s.Value=w.Id FOR XML PATH('')),1,1,''),60) as SupplierTitle,
                c.Barcode,c.Title,c.Size,b.BuyPrice,b.SysPrice,b.TradePrice,b.Memo,b.TradePriceId,b.Id,a.State,a.CreateDT,
                (CASE WHEN b.EndDate IS NULL THEN CONVERT(VARCHAR(20),b.StartDate,102)+'-不限' ELSE CONVERT(VARCHAR(20),b.StartDate,102)+'-'+CONVERT(VARCHAR(20),b.EndDate,102) end) DateSpacing
                 FROM ProductTradePrice a
                INNER JOIN ProductTradePriceList b ON b.TradePriceId=a.Id
                INNER JOIN dbo.ProductRecord c ON b.barcode=c.Barcode and a.CompanyId=c.CompanyId where 1=1 and a.CompanyId="+nvl["CompanyId"];
            if (!nvl["supplierId"].IsNullOrEmpty())
                sql += " and ','+a.Wholesaler+',' like '%," + nvl["supplierId"] + ",%'";
            if (!nvl["state"].IsNullOrEmpty())
                sql += " and a.state=" + nvl["state"];
            if (!nvl["searchText"].IsNullOrEmpty())
                sql += " and (c.barcode like '%" + nvl["searchText"].Trim() + "%' or c.Title like '%" + nvl["searchText"].Trim() + "%')";
            return base.ExceuteSqlForGroupPage(sql, out recordCount);
        }

        /// <summary>
        /// 根据条码查询该产品是否存在业务关系
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        internal int GetIsRelationship(string barcode,int companyId)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                using (EFDbContext db = new EFDbContext())
                {
                    var sql = @"SELECT dbo.F_GetIsRelationship( '" + barcode + " ',"+companyId+") AS Count";
                    var result=db.Database.SqlQuery<int>(sql).FirstOrDefault();
                    return result;
                }
            }
            else
            { 
                return 0;
            }

        }
        internal System.Data.DataTable GetCurrentPrice(string barcode,int companyId,short type)
        {
            using (EFDbContext db = new EFDbContext())
            {
                var sql = string.Format("exec GetCurrentPrice {0},'{1}',{2}", companyId, barcode, type);
                return db.Database.SqlQueryForDataTatable(sql, System.Data.CommandType.Text);
            }
        }
        internal List<Pharos.Logic.Entity.ProductMultPrice> GetChangePriceByStoreIds(string storeIds,string barcode,int companyId)
        {
            var sql = @"SELECT DISTINCT b.Id,b.Barcode,b.CurBuyPrice BuyPrice,b.CurSysPrice Price FROM dbo.ProductChangePrice a
			INNER JOIN dbo.ProductChangePriceList b ON b.ChangePriceId=a.Id
			CROSS APPLY dbo.SplitString(a.StoreId,',',1) c
			WHERE a.State=1 AND b.State=1 AND a.CompanyId=@CompanyId and (EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=c.Value) OR c.Value='-1') and EXISTS(SELECT 1 FROM dbo.SplitString(@barcode,',',1) WHERE Value=barcode)";
            var parms = new SqlParameter[] { 
                new SqlParameter("@CompanyId", companyId), 
                new SqlParameter("@storeId", storeIds),
                new SqlParameter("@barcode", barcode),
            };
            var list= _db.DataTableText<Pharos.Logic.Entity.ProductMultPrice>(sql, parms);
            return list;
        }
        internal List<Entity.Warehouse> GetWareHouseBysn(int categorysn, int companyId)
        {
            using (EFDbContext db = new EFDbContext())
            {
                var sql = @"SELECT * FROM dbo.Warehouse a CROSS APPLY dbo.F_GetParentCategorys(@categorysn,a.CompanyId) b
                    WHERE a.CompanyId=@CompanyId and EXISTS(SELECT 1 FROM dbo.SplitString(a.CategorySN,',',1) WHERE Value=CAST(b.categorysn AS VARCHAR))";
                var list = db.Database.SqlQuery<Entity.Warehouse>(sql, new SqlParameter("@CompanyId", companyId), new SqlParameter("@categorysn", categorysn)).ToList();
                return list;
            }
        }
        internal System.Data.DataTable GetProductsBybars(string barcode, int companyId)
        {
            var sql =string.Format("SELECT * FROM dbo.Vw_Product_Bundling v WHERE v.CompanyId={0} AND(EXISTS(SELECT 1 FROM dbo.SplitString('{1}',',',1) WHERE Value=v.Barcode) OR (ISNULL(v.Barcodes,'')<>''))",
                companyId,barcode);
            return _db.DataTableText(sql, null);
        }
        internal List<string> GetDataForSearch(string searchText, string searchField, int companyId)
        {
            var sql = @"SELECT DISTINCT top 20 [" + searchField + @"] FROM  dbo.ProductRecord a
                WHERE a.CompanyId=" + companyId;
            sql += " and (" + searchField + " like '%" + searchText + "%' or barcodes like '%"+searchText+"%')";
            using (var db = new EFDbContext())
            {
                return db.Database.SqlQuery<string>(sql).ToList();
            }
        }
        internal IEnumerable<dynamic> GetStockNumByStore(List<Entity.Warehouse> stores, string barcode,int nature, int companyId)
        {
            var dicts = new Dictionary<string, decimal>();
            using (EFDbContext db = new EFDbContext())
            {
                foreach (var store in stores)
                {
                    var sql =string.Format(@"SELECT dbo.F_InventoryNum('{0}','{1}',{2},{3})",store.StoreId,barcode,nature,companyId);
                    dicts[store.Title]=db.Database.SqlQuery<decimal>(sql).FirstOrDefault();
                }
            }
            return dicts.Where(o=>o.Value!=0).Select(o => new {Title=o.Key,Num=o.Value });
        }
        internal System.Data.DataTable GetCurrentPrices(string barcode, int type, int companyId)
        {
            var dt= _db.DataTableText(string.Format("exec GetCurrentPrice @companyId={0},@barcode='{1}',@type={2}",companyId,barcode,type),null);
            return dt;
        }
    }
}
