using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Pharos.Logic.DAL
{
    internal class MemberIntegralSetDAL
    {
        public DataTable LoadTypeList(int id)
        {
            string sql = @"select a.id,a.BarcodeOrCategorySN+'~0' StrId,0 BrandSN,a.SetType,
                    a.BarcodeOrCategorySN,
                    dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,1,1,@companyId) BigCategoryTitle,
                    dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,2,1,@companyId) MidCategoryTitle,
                    dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,3,1,@companyId) SubCategoryTitle,
                    a.SaleMoney,a.IntegralCount
                FROM dbo.MemberIntegralSetList a
                WHERE a.SetType=2 AND a.IntegralId=@id";
            using (EFDbContext db = new EFDbContext())
            {
                return db.Database.SqlQueryForDataTatable(sql,CommandType.Text,new SqlParameter("@id", id), new SqlParameter("@companyId", Sys.SysCommonRules.CompanyId));
            }
        }
    }
}
