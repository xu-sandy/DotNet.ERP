﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Pharos.DBFramework;
using Pharos.Sys.EntityExtend;
using Pharos.Sys.Entity;

namespace Pharos.Sys.DAL
{
    internal class SysDepartmentDAL : BaseSysEntityDAL<SysDepartments>
    {
        public SysDepartmentDAL() : base("SysDepartments") { }

        /// <summary>
        /// 获取组织机构列表数据
        /// </summary>
        /// <returns></returns>
        public List<SysDepartmentsExt> GetExtList()
        {
            return GetExtList(-1);
        }
        /// <summary>
        /// 根据Id获取组织机构列表数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SysDepartmentsExt> GetExtList(int id)
        {
            SqlParameter[] parms = {
					new SqlParameter("@id", id),
                    new SqlParameter("@companyId",Sys.SysCommonRules.CompanyId)
				};
            var result = DbHelper.DataTable<SysDepartmentsExt>("Sys_DepartmentList", parms);
            return result;
        }
        /// <summary>
        /// 根据depId获取组织机构列表数据
        /// </summary>
        /// <param name="depId">机构ID（全局唯一）</param>
        /// <returns></returns>
        public List<SysDepartments> GetListByDepId(int depId)
        {
            SqlParameter[] parms = {
                                       new SqlParameter("@DepId", depId)
                                   };
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from SysDepartments where DepId=@DepId and Status=1");

            var list = DbHelper.DataTableText<SysDepartments>(sql.ToString(), parms);
            return list;
        }
        /// <summary>
        /// 根据类型获得机构/部门列表
        /// </summary>
        /// <returns></returns>
        public List<SysDepartments> GetListByType(int type,int companyId)
        {
            SqlParameter[] parms = {
                                       new SqlParameter("@Type",type),
                                       new SqlParameter("@companyId",companyId)
                                   };
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from SysDepartments where companyId=@companyId and Type=@Type and Status=1");

            var list = DbHelper.DataTableText<SysDepartments>(sql.ToString(), parms);
            return list;
        }
        /// <summary>
        /// 通过pDepId获取部门列表
        /// </summary>
        /// <param name="pDepId"></param>
        /// <returns></returns>
        public List<SysDepartments> GetListByPDepId(int pDepId, int companyId)
        {
            SqlParameter[] parms = { 
                                   new SqlParameter("@PDepId", pDepId),
                                   new SqlParameter("@companyId",companyId)
                                   };
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from SysDepartments where companyId=@companyId and PDepId=@PDepId and Status=1");
            var list = DbHelper.DataTableText<SysDepartments>(sql.ToString(), parms);
            if (list == null)
                list = new List<SysDepartments>();
            return list;
        }
        /// <summary>
        /// 根据Id获取部门列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysDepartmentsExt GetExtModel(int id)
        {
            SysDepartmentsExt model = null;
            var datas = GetExtList(id);
            if (datas != null && datas.Count > 0)
                model = datas[0];
            return model;
        }
        /// <summary>
        /// 判断部门是否存在
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="notId"></param>
        /// <returns></returns>
        public bool Exists(string sn, int notId)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@sn", sn),
					new SqlParameter("@notId", notId)};

            string sql = string.Format("select count(1) from {0} where [sn]=@sn and [id]<>@notId", TableName);

            var result = DbHelper.ExecuteScalarText(sql, parameters);
            var re = Convert.ToInt32(result) > 0 ? true : false;
            return re;
        }
        /// <summary>
        /// 新增组织机构
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(SysDepartments model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("insert into {0}(", TableName);
            strSql.Append("[type],depid,pdepid,sortorder,title,sn,manageruid,deputyuid,indexpageurl,[status],CompanyId)");
            strSql.Append(" values (@type,@depid,@pdepid,@sortorder,@title,@sn,@manageruid,@deputyuid,@indexpageurl,@status,@CompanyId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@type", model.Type),
                    new SqlParameter("@depid", model.DepId),
                    new SqlParameter("@pdepid", model.PDepId),
                    new SqlParameter("@sortorder", model.SortOrder),
                    new SqlParameter("@title", model.Title),
                    new SqlParameter("@sn", model.SN),
                    new SqlParameter("@manageruid", model.ManagerUId),
                    new SqlParameter("@deputyuid", model.DeputyUId),
                    new SqlParameter("@indexpageurl", model.IndexPageUrl),
                    new SqlParameter("@status", model.Status),
                    new SqlParameter("@CompanyId",model.CompanyId)};

            object obj = DbHelper.ExecuteScalarText(strSql.ToString(), parameters);
            return (obj == null) ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 更新组织机构
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(SysDepartments model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update {0} set ", TableName);
            strSql.Append("type=@type,");
            strSql.Append("pdepid=@pdepid,");
            strSql.Append("sortorder=@sortorder,");
            strSql.Append("title=@title,");
            strSql.Append("sn=@sn,");
            strSql.Append("manageruid=@manageruid,");
            strSql.Append("deputyuid=@deputyuid,");
            strSql.Append("indexpageurl=@indexpageurl,");
            strSql.Append("status=@status");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", model.Id),
                    new SqlParameter("@type", model.Type),
                    new SqlParameter("@pdepid", model.PDepId),
                    new SqlParameter("@sortorder", model.SortOrder),
                    new SqlParameter("@title", model.Title),
                    new SqlParameter("@sn",model.SN),
                    new SqlParameter("@manageruid", model.ManagerUId),
                    new SqlParameter("@deputyuid", model.DeputyUId),
                    new SqlParameter("@indexpageurl", model.IndexPageUrl),
                    new SqlParameter("@status", model.Status)};
            int rows = DbHelper.ExecuteNonQueryText(strSql.ToString(), parameters);
            return rows > 0 ? true : false;
        }
        /// <summary>
        /// 更新组织机构状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateStatus(SysDepartments model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update {0} set ", TableName);
            strSql.Append("status=@status");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", model.Id),
                    new SqlParameter("@status", model.Status)};
            int rows = DbHelper.ExecuteNonQueryText(strSql.ToString(), parameters);
            return rows > 0 ? true : false;
        }
    }
}
