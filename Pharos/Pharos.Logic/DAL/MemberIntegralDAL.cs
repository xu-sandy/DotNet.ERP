﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pharos.Logic.DAL
{
    public class MemberIntegralDAL:BaseDAL
    {
        public DataTable GetPageList(string text,int companyId, out int recordCount)
        {
            string sql=@"SELECT * FROM(
                SELECT a.*,b.IntegralId,b.BarcodeOrCategorySN,b.SaleMoney,b.SetType,b.IntegralCount,c.Title FROM dbo.MemberIntegralSet a
                INNER join dbo.MemberIntegralSetList b ON b.IntegralId=a.Id
                INNER JOIN dbo.ProductRecord c ON b.BarcodeOrCategorySN=c.Barcode 
                WHERE a.Nature=2 and b.SetType=1
                UNION 
                SELECT a.*,b.IntegralId,b.BarcodeOrCategorySN,b.SaleMoney,b.SetType,b.IntegralCount,c.Title FROM dbo.MemberIntegralSet a
                INNER join dbo.MemberIntegralSetList b ON b.IntegralId=a.Id
                INNER JOIN dbo.ProductCategory c ON b.BarcodeOrCategorySN=CAST(c.CategorySN AS VARCHAR(30))
                WHERE a.Nature=2 and b.SetType=2) tb where 1=1";
            if(!string.IsNullOrWhiteSpace(text))
                sql +=string.Format(" and (Title like '%{0}%' or  BarcodeOrCategorySN like '%{0}%')",text.Trim());
            sql += " and CompanyId=" + companyId;
            return base.ExceuteSqlForPage(sql, out recordCount);
        }
    }
}
