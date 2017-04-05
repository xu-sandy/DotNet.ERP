﻿using Pharos.Sys.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.Sys.DAL
{
    internal class SysLogDAL : BaseSysEntityDAL<SysLog>
    {
        public SysLogDAL() : base("SysLog") { }

        /// <summary>
        /// 获得日志列表
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal DataTable GetList(Paging paging, string key, int type,string start,string end)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@startDate", start),
                    new SqlParameter("@endDate", end),
                    new SqlParameter("@Type", type),
                    new SqlParameter("@Key", key),
                    new SqlParameter("@CurrentPage", paging.PageIndex),
                    new SqlParameter("@PageSize", paging.PageSize),
                    new SqlParameter("@CompanyId",Sys.SysCommonRules.CompanyId)
                                   };

            return DbHelper.DataTable("Sys_LogList", parms, ref paging);
        }
        /// <summary>
        /// 删除多个日志记录
        /// </summary>
        /// <param name="ids">日志ID数组</param>
        /// <returns></returns>
        internal bool DeleteRange(int[] ids)
        {
            SqlParameter[] parms = { new SqlParameter() };
            var idsStr = string.Join(",", ids);

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("delete from {0} where [id] in ({1})", "SysLog", idsStr);

            return DbHelper.ExecuteNonQueryText(sql.ToString(), parms) > 0;
        }
        /// <summary>
        /// 清空日志记录
        /// </summary>
        /// <returns></returns>
        internal bool DeleteAll()
        {
            SqlParameter[] parms = { new SqlParameter() };
            StringBuilder sql = new StringBuilder();
            //sql.AppendFormat("truncate table {0}", "SysLog");
            sql.AppendFormat("DELETE {0} WHERE CompanyId={1}", "SysLog", Sys.SysCommonRules.CompanyId);

            return DbHelper.ExecuteNonQueryText(sql.ToString(), parms) > 0;
        }
    }
}
