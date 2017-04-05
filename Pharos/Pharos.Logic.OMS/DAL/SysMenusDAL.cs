﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.EntityExtend;

namespace Pharos.Logic.OMS.DAL
{
    internal class SysMenusDAL : BaseSysEntityDAL<SysMenus>
    {
        public SysMenusDAL() : base("SysMenus") { }

        #region 菜单管理
        /// <summary>
        /// 获取菜单列表数据
        /// </summary>
        /// <returns></returns>
        internal List<SysMenusExt> GetList()
        {
            return GetList(-1);
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal List<SysMenusExt> GetList(int id)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@id", id)
                };

            string sql = "select Id,MenuId,PMenuId,SortOrder,Title,URL,Status from SysMenus order by SortOrder";

            var result = DbHelper.DataTableText<SysMenusExt>(sql, parms);
            return result;
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        internal List<SysMenusExt> GetMenusTreeList()
        {
            var sql = @"SELECT * FROM dbo.SysMenus";
            SqlParameter[] parms = {
            };
            var result = DbHelper.DataTableText<SysMenusExt>(sql, parms);
            return result;
        }
        /// <summary>
        /// 根据菜单Id获得菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal SysMenusExt GetExtModel(int id)
        {
            SysMenusExt obj = null;
            SqlParameter[] parms = {
                    new SqlParameter("@id", id)
                };

            string sql = "select Id,MenuId,PMenuId,SortOrder,Title,URL,Status from SysMenus where [Id]=@id";
            var objs = DbHelper.DataTableText<SysMenusExt>(sql, parms);

            if (objs != null && objs.Count > 0)
                obj = objs[0];
            return obj;
        }
        internal bool Exists(string sn, int notId)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@sn", sn),
                    new SqlParameter("@notId", notId)};

            string sql = string.Format("select count(1) from {0} where [sn]=@sn and [id]<>@notId", TableName);

            var result = DbHelper.ExecuteNonQueryText(sql, parms);
            return result > 0;
        }
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal int Insert(SysMenus model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@menuid", model.MenuId),
                    new SqlParameter("@pmenuid", model.PMenuId),
                    new SqlParameter("@sortorder", model.SortOrder),
                    new SqlParameter("@title", model.Title),
                    new SqlParameter("@url", model.URL),
                    new SqlParameter("@status", model.Status)
                                   };

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("insert into {0} (", TableName);
            sql.Append("menuid,pmenuid,sortorder,title,url,[status])");
            sql.Append(" values (@menuid,@pmenuid,@sortorder,@title,@url,@status)");
            sql.Append(";select @@IDENTITY");

            object obj = DbHelper.ExecuteScalarText(sql.ToString(), parms);
            return (obj == null) ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal bool Update(SysMenus model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@menuid", model.MenuId),
                    new SqlParameter("@pmenuid", model.PMenuId),
                    new SqlParameter("@sortorder", model.SortOrder),
                    new SqlParameter("@title", model.Title),
                    new SqlParameter("@url", model.URL),
                    new SqlParameter("@status", model.Status),
                    new SqlParameter("@id", model.Id)};

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update {0} set ", TableName);
            sql.Append("menuid=@menuid,");
            sql.Append("pmenuid=@pmenuid,");
            sql.Append("sortorder=@sortorder,");
            sql.Append("title=@title,");
            sql.Append("url=@url,");
            sql.Append("status=@status");
            sql.Append(" where id=@id");

            int rows = DbHelper.ExecuteNonQueryText(sql.ToString(), parms);
            return rows > 0 ? true : false;
        }
        /// <summary>
        /// 更新菜单状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal bool UpdateStatus(SysMenus model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@id", model.Id),
                    new SqlParameter("@status", model.Status)};

            string sql = string.Format("update {0} set status=@status where id=@id", TableName);

            int rows = DbHelper.ExecuteNonQueryText(sql, parms);
            return rows > 0 ? true : false;
        }
        /// <summary>
        /// 更新菜单排序号
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        internal bool UpdateMenuOrderIndex(SysMenus model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@id", model.Id),
                    new SqlParameter("@SortOrder", model.SortOrder)};
            string sql = string.Format("update {0} set SortOrder=@SortOrder where id=@id", TableName);

            int rows = DbHelper.ExecuteNonQueryText(sql, parms);
            return rows > 0 ? true : false;
        }
        /// <summary>
        /// 查找菜单项对应父节点的所有子节点
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        internal List<SysMenus> FindParentChilds(int menuId)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@MenuId", menuId)};

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from {0} where PMenuId in (select PMenuId from {0} where MenuId = @MenuId)", TableName);
            var objs = DbHelper.DataTableText<SysMenus>(sql.ToString(), parms);
            return objs;
        }
        internal int GetMenuMaxIndex(int pMenuId)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@PMenuId", pMenuId)};
            string sql = string.Format("select max(SortOrder) from {0} where PMenuId = @PMenuId", TableName);

            object obj = DbHelper.ExecuteScalarText(sql.ToString(), parms);
            return (obj is DBNull) ? 0 : Convert.ToInt32(obj);
        }
        #endregion 菜单管理

    }
}
