﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Pharos.DBFramework;
using Pharos.Logic.Entity;

namespace Pharos.Logic.DAL
{
    internal class ContractDAL:BaseDAL
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 根据合同状态，创建人，签订日期查询合同列表
        /// </summary>
        /// <param name="state">合同状态</param>
        /// <param name="createUID">创建人</param>
        /// <param name="beginSigningDate">开始签订日期</param>
        /// <param name="endSigningDate">结束签订日期</param>
        /// <returns>合同列表</returns>
        public DataTable GetContractListBySearch(int state, string createUID, string beginSigningDate, string endSigningDate, out int recordCount)
        {
            var parms = new List<SqlParameter>();

            string sql = "SELECT *,dbo.F_SupplierNameById(SupplierId) SupplierTitle,dbo.F_UserNameById(CreateUID) CreateTitle,(select count(*) from [dbo].[Attachment],[dbo].[Contract] where ItemId=Contract.Id and Contract.Id=c.Id group by Contract.Id) AttCount,(select a.Id from [dbo].[Contract] a join [dbo].[Contract] b on b.PId=a.Id where a.Id=c.Id ) isExtend FROM [dbo].[Contract] c  WHERE 1=1 "; //,dbo.F_SupplierNameById(SupplierId) Supplier
            if (state!=-1)
            {               
                sql += " AND State=" + state;
            }
            if (!string.IsNullOrEmpty(createUID))
            {               
                sql += " AND CreateUID=" + "'"+createUID+"'";
            }
            if (!string.IsNullOrEmpty(beginSigningDate)&&!string.IsNullOrEmpty(endSigningDate)&& DateTime.Parse(beginSigningDate) > DateTime.Parse(endSigningDate))
            {               
                var temp = beginSigningDate;
                beginSigningDate = endSigningDate;
                endSigningDate = temp;               
            }
            if (!string.IsNullOrEmpty(beginSigningDate))
            {
                sql += " AND (SigningDate>" + "'" + beginSigningDate + "'" +" OR SigningDate=" +"'" + beginSigningDate +"'" + ")";
            }
            if (!string.IsNullOrEmpty(endSigningDate))
            {
                sql += " AND (SigningDate<" + "'" + endSigningDate + "'" + " OR SigningDate=" + "'" + endSigningDate + "'" + ")";

            }
            sql += " and CompanyId=" + Sys.SysCommonRules.CompanyId;
            return base.ExceuteSqlForPage(sql, out recordCount);
        }
    }
}
