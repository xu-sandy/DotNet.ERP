using Pharos.Logic.Entity;
using Pharos.Logic.EntityExtend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Pharos.DBFramework;

namespace Pharos.Logic.DAL
{
    /// <summary>
    /// 权限-数据访问层
    /// </summary>
    internal class SysLimitisDAL
    {
        DBHelper _db = new DBHelper();
        /// <summary>
        /// 获得权限下拉树的数据
        /// </summary>
        /// <returns></returns>
        public List<SysLimits> GetLimitBranches()
        { 
            SqlParameter[] parms = {
					new SqlParameter("@where1", "0")
				};
            string sql = "SELECT * FROM dbo.SysLimits WHERE PId=0 OR Id in (SELECT Pid FROM dbo.SysLimits)";
            return _db.DataTableText<SysLimits>(sql, parms);
        }
        public List<SysLimitsExt> GetRoleAllLimits()
        {
            var result = _db.DataTable<SysLimitsExt>("Sys_AllLimitList");
            return result;
        }
    }
}
