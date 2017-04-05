﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Pharos.DBFramework;

namespace Pharos.Logic.DAL
{
    /// <summary>
    /// 采购管理->品牌管理
    /// </summary>
    internal class ProductBrandDAL:BaseDAL
    {
        DBHelper db = new DBHelper();
        /// <summary>
        /// 根据品牌名称获取品牌列表
        /// </summary>
        /// <param name="title">品牌名称</param>
        /// <returns>品牌列表</returns>
        public DataTable GetBrandListBySearch(string title, int companyId, int? classfyId, out int count)
        {
            var parms = new List<SqlParameter>();
            string sql = "SELECT a.Id, a.BrandSN, a.Title, a.JianPin, a.State, (SELECT count(*) FROM dbo.ProductRecord WHERE BrandSN=a.BrandSN AND CompanyId=a.CompanyId) Num, b.Title ClassifyTitle FROM dbo.ProductBrand a LEFT JOIN dbo.SysDataDictionary b ON a.ClassifyId=b.DicSN  AND a.CompanyId=b.CompanyId where 1=1 ";
            if (!string.IsNullOrEmpty(title))
            {
                sql += " AND a.Title like '%" + title.Trim() + "%'";
            }
            if (classfyId != null)
            {
                sql += " AND a.ClassifyId=" + classfyId;
            }
            sql += " and a.companyid=" + companyId;
            return base.ExceuteSqlForPage(sql, out count);
        }

        /// <summary>
        /// 根据品牌名称或简拼获取品牌列表
        /// </summary>
        /// <param name="title">品牌名称</param>
        /// <returns>品牌列表</returns>
        public DataTable GetBrandList(string searchName, int companyId, int? classfyId, out int count, int state = -1)
        {
            var parms = new List<SqlParameter>();
            string sql = "SELECT a.Id, a.BrandSN, a.Title, a.JianPin, a.State, (SELECT count(*) FROM dbo.ProductRecord WHERE BrandSN=a.BrandSN) Num, b.Title ClassifyTitle FROM dbo.ProductBrand a LEFT JOIN dbo.SysDataDictionary b ON a.ClassifyId=b.DicSN where 1=1 ";
            if (!string.IsNullOrEmpty(searchName))
            {
                sql += " AND (a.Title like '%" + searchName.Trim() + "%' OR a.JianPin like '%" + searchName.Trim() + "%')";
            }
            if (classfyId != null)
            {
                sql += " AND a.ClassifyId=" + classfyId;
            }
            if (state != -1)
            {
                sql += " AND a.State=" + state;
            }
            sql += " and a.companyid=" + companyId;
            return base.ExceuteSqlForPage(sql, out count);
        }

    }
}
