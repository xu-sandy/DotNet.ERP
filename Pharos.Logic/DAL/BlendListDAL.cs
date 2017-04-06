using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Pharos.DBFramework;

namespace Pharos.Logic.DAL
{
    internal class BlendListDAL
    {
        DBHelper db = new DBHelper();
        public DataTable GetBlendCategoryList(string CommodityId,short type)
        {
            string sql = @"SELECT a.Id,
					dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,3,1,@companyId) SubCategoryTitle ,
                    dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,2,1,@companyId) MidCategoryTitle ,
                    dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,1,1,@companyId) BigCategoryTitle ,
                    a.BarcodeOrCategorySN CategorySN ,
					a.BrandSN,
                    a.CategoryGrade,
					dbo.F_BrandNameForSN(a.BrandSN) BrandTitle,
					ISNULL((SELECT SUM(AcceptNums)-SUM(PurchaseNumbers) FROM Vw_Product WHERE CategorySN=a.BarcodeOrCategorySN AND BrandSN=a.BrandSN),0) StockNums
                 FROM PromotionBlendList a 
                WHERE  CommodityId =@CommodityId AND BlendType = @BlendType";
            var dt = db.DataTableText(sql, new SqlParameter[] { new SqlParameter("@CommodityId", CommodityId), new SqlParameter("@BlendType", type), new SqlParameter("@companyId", Sys.SysCommonRules.CompanyId) });
            return dt;
        }
        public DataTable GetFreeGiftCategoryList(string CommodityId)
        {
            string sql = @"SELECT a.Id,a.GiftId,a.MinPurchaseNum,a.RestrictionBuyNum,
					dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,3,1,@companyId) SubCategoryTitle ,
                    dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,2,1,@companyId) MidCategoryTitle ,
                    dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,1,1,@companyId) BigCategoryTitle ,
                    a.BarcodeOrCategorySN CategorySN ,
					a.BrandSN,a.GiftType,a.CategoryGrade,
					dbo.F_BrandNameForSN(a.BrandSN) BrandTitle,
					ISNULL((SELECT SUM(AcceptNums)-SUM(PurchaseNumbers) FROM Vw_Product WHERE CategorySN=a.BarcodeOrCategorySN AND BrandSN=a.BrandSN),0) StockNums
                 FROM FreeGiftPurchase a 
                WHERE  CommodityId =@CommodityId AND GiftType=2";
            var dt = db.DataTableText(sql, new SqlParameter[] { new SqlParameter("@CommodityId", CommodityId), new SqlParameter("@companyId", Sys.SysCommonRules.CompanyId) });
            return dt;
        }
    }
}
