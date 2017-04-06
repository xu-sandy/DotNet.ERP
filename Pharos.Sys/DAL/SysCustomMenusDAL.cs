using Pharos.Sys.Entity;
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
    internal class SysCustomMenusDAL : BaseSysEntityDAL<SysCustomMenus>
    {
        public SysCustomMenusDAL() : base("SysCustomMenus") { }

        /// <summary>
        /// 根据objId获得对应的菜单权限
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        internal DataTable GetSysCustomMenusByObjId(int objId)
        {
            SqlParameter[] parms ={
                                     new SqlParameter("@ObjId",objId),
                                     new SqlParameter("@companyId",Sys.SysCommonRules.CompanyId)
                                 };
            string sql = "SELECT * FROM dbo.SysCustomMenus where ObjId=@ObjId AND CompanyId=@companyId;";
            return DbHelper.DataTableText(sql, parms);
        }
        ///// <summary>
        ///// 新增菜单权限
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        internal int Insert(SysCustomMenus model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@Type", model.Type),
                    new SqlParameter("@ObjId",model.ObjId),
                    new SqlParameter("@MenuIds", model.MenuIds),
                    new SqlParameter("@CompanyId",model.CompanyId)
                                   };

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("insert into {0} (", TableName);
            sql.Append("Type,ObjId,MenuIds,CompanyId)");
            sql.Append(" values (@Type,@ObjId,@MenuIds,@CompanyId);");
            sql.Append(";select @@IDENTITY");

            object obj = DbHelper.ExecuteScalarText(sql.ToString(), parms);

            return (obj == null) ? 0 : Convert.ToInt32(obj);
        }

        ///// <summary>
        ///// 更新菜单权限
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        internal bool Update(SysCustomMenus model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@Type", model.Type),
                    new SqlParameter("@ObjId", model.ObjId),
                    new SqlParameter("@MenuIds", model.MenuIds),
                                   };

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update {0} set ", TableName);
            sql.Append("Type=@Type,");
            sql.Append("ObjId=@ObjId,");
            sql.Append("MenuIds=@MenuIds");

            sql.Append(" where Id=@Id");

            int rows = DbHelper.ExecuteNonQueryText(sql.ToString(), parms);

            return rows > 0 ? true : false;
        }
    }
}
