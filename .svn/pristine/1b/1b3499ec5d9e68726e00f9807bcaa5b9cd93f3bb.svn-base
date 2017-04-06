// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-28
// 描述信息：数据字典-数据访问层
// --------------------------------------------------

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Pharos.DBFramework;
using Pharos.Logic.Entity;

namespace Pharos.Logic.DAL
{
    /// <summary>
    /// 数据字典-数据访问层
    /// </summary>
    internal class SysDataDictionaryDAL
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 根据该父级类别下的子字典项
        /// </summary>
        /// <param name="psn">父级编号</param>
        public DataTable GetDicListByPSN(int psn, bool filter)
        {
            SqlParameter[] parms = {
					new SqlParameter("@DicPSN", psn),
                    new SqlParameter("@CompanyId",Sys.SysCommonRules.CompanyId)
				};
            string sql = string.Empty;
            if (filter)
                sql = "SELECT DicSN,Title FROM dbo.SysDataDictionary WHERE DicPSN=@DicPSN and CompanyId=@CompanyId and Status=1";
            else
                sql = "SELECT DicSN,Title FROM dbo.SysDataDictionary WHERE DicPSN=@DicPSN and CompanyId=@CompanyId ";
            return db.DataTableText(sql, parms);
        }

        public DataTable GetIndentOrderInfo(string id)
        {
            SqlParameter[] parms = {
					new SqlParameter("@Id", id)
				};

            return db.DataTable("Test", parms);
        }
    }
}
