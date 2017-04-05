﻿using Pharos.Sys.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharos.Sys.Models;

namespace Pharos.Sys.DAL
{
    internal class SysUserInfoDAL : BaseSysEntityDAL<SysUserInfo>
    {
        public SysUserInfoDAL() : base("SysUserInfo") { }

        /// <summary>
        /// 获取所有的用户信息分页列表
        /// </summary>
        /// <returns></returns>
        internal DataTable GetList(Paging paging, QueryUserModel queryUserModel)
        {
            List<SqlParameter> parms = new List<SqlParameter>() {
                    new SqlParameter("@Key", queryUserModel.Keyword),
                    new SqlParameter("@OrganizationId", queryUserModel.OrganizationId),
                    new SqlParameter("@DepartmentId", queryUserModel.DepartmentId),
                    new SqlParameter("@RroleGroupsId", queryUserModel.RoleGroupsId),
                    new SqlParameter("@CurrentPage", paging.PageIndex),
                    new SqlParameter("@PageSize", paging.PageSize),
                    new SqlParameter("@CompanyId",Sys.SysCommonRules.CompanyId)
            };
            if (queryUserModel.Status.HasValue)
                parms.Add(new SqlParameter("@Status", queryUserModel.Status.Value));
            return DbHelper.DataTable("Sys_UserList", parms.ToArray(), ref paging);
        }
        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        internal string GetMaxUserCode(int companyId)
        {
            string sql = "SELECT MAX(UserCode) UserCode FROM dbo.SysUserInfo where CompanyId=" + companyId;
            var obj = DbHelper.ExecuteScalarText(sql, null);
            if (obj == null || obj is DBNull)
                return "1000";
            else
                return obj.ToString();
        }
        ///// <summary>
        ///// 新增用户
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        internal int Insert(SysUserInfo model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@UID", model.UID),
                    new SqlParameter("@UserCode", model.UserCode),
                    new SqlParameter("@FullName",model.FullName),
                    new SqlParameter("@LoginName", model.LoginName),
                    new SqlParameter("@LoginPwd", Security.MD5_Encrypt(model.LoginPwd)),
                    new SqlParameter("@Sex", model.Sex),
                    new SqlParameter("@BranchId", model.BranchId),
                    new SqlParameter("@BumenId", model.BumenId),
                    new SqlParameter("@BossUId",model.BossUId),
                    new SqlParameter("@PositionId", model.PositionId),
                    new SqlParameter("@PhotoUrl", model.PhotoUrl),
                    new SqlParameter("@Status", model.Status),
                    new SqlParameter("@RoleIds", model.RoleIds),
                    new SqlParameter("@LoginIP",model.LoginIP),
                    new SqlParameter("@LoginDT", model.LoginDT),
                    new SqlParameter("@LoginNum", model.LoginNum),
                    new SqlParameter("@CreateDT", model.CreateDT),
                    new SqlParameter("@CreateUID",model.CreateUID),
                    new SqlParameter("@CompanyId",model.CompanyId)
                                   };

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("insert into {0} (", TableName);
            sql.Append("[UID],UserCode,FullName,LoginName,LoginPwd,Sex,BranchId,BumenId,BossUId,PositionId,PhotoUrl,[Status],RoleIds,LoginIP,LoginDT,LoginNum,CreateDT,CreateUID,CompanyId)");
            sql.Append(" values (@UID,@UserCode,@FullName,@LoginName,@LoginPwd,@Sex,@BranchId,@BumenId,@BossUId,@PositionId,@PhotoUrl,@Status,@RoleIds,@LoginIP,@LoginDT,@LoginNum,@CreateDT,@CreateUID,@CompanyId)");
            sql.Append(";select @@IDENTITY");

            object obj = DbHelper.ExecuteScalarText(sql.ToString(), parms);

            return (obj == null) ? 0 : Convert.ToInt32(obj);
        }
        ///// <summary>
        ///// 更新用户
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        internal bool Update(SysUserInfo model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@FullName", model.FullName),
                    new SqlParameter("@Sex", model.Sex),
                    new SqlParameter("@LoginName",model.LoginName),
                    new SqlParameter("@LoginPwd", string.IsNullOrEmpty(model.LoginPwd)?model.LoginPwd:Security.MD5_Encrypt(model.LoginPwd)),
                    new SqlParameter("@BranchId", model.BranchId),
                    new SqlParameter("@BumenId", model.BumenId),
                    new SqlParameter("@PositionId", model.PositionId),
                    new SqlParameter("@Status", model.Status),
                    new SqlParameter("@RoleIds", model.RoleIds)
                                   };

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update {0} set ", TableName);
            sql.Append("FullName=@FullName,");
            sql.Append("Sex=@Sex,");
            sql.Append("LoginName=@LoginName,");
            if (model.LoginPwd != null)
            {
                sql.Append("LoginPwd=@LoginPwd,");
            }
            sql.Append("BranchId=@BranchId,");
            sql.Append("BumenId=@BumenId,");
            sql.Append("PositionId=@PositionId,");
            sql.Append("Status=@Status,");
            sql.Append("RoleIds=@RoleIds");

            sql.Append(" where Id=@Id");

            int rows = DbHelper.ExecuteNonQueryText(sql.ToString(), parms);

            return rows > 0 ? true : false;
        }
        internal bool ExistName(SysUserInfo model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@CompanyId", model.CompanyId),
                    new SqlParameter("@FullName", model.FullName),
                    new SqlParameter("@LoginName", model.LoginName)};
            string sql = "SELECT COUNT(*) FROM dbo.SysUserInfo WHERE CompanyId=@CompanyId and Id<>@Id AND (FullName=@FullName OR LoginName=@LoginName);";

            var obj = DbHelper.ExecuteScalarText(sql, parms);

            return Convert.ToInt32(obj) > 0 ? false : true;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //internal bool UpdateStatus(SysRoles model)
        //{
        //    SqlParameter[] parms = {
        //            new SqlParameter("@id", model.Id),
        //            new SqlParameter("@status", model.Status)};

        //    string sql = string.Format("update {0} set status=@status where id=@id", TableName);

        //    int rows = DbHelper.ExecuteNonQueryText(sql, parms);
        //    return rows > 0 ? true : false;
        //}

        internal List<SysUserInfo> GetUsers(int selectType, string keyWord = "", string selectUID = "")
        {
            string sql = string.Empty;
            if (selectType == 1)
            { //全部
                sql = "SELECT [UID],FullName FROM dbo.SysUserInfo where 1=1 AND CompanyId=" + Sys.SysCommonRules.CompanyId;//16.03.24增加企业过滤
            }
            else
            { //排除注销
                sql = "SELECT [UID],FullName,LoginName FROM dbo.SysUserInfo WHERE [Status]=1 AND CompanyId=" + Sys.SysCommonRules.CompanyId;//16.03.24增加企业过滤
            }
            if (!string.IsNullOrWhiteSpace(keyWord) && !string.IsNullOrWhiteSpace(selectUID))
                sql += string.Format(" and (LoginName like '%{0}%' or fullname like '%{0}%' {1})", keyWord, "or uid='" + selectUID + "'");
            else if (!string.IsNullOrWhiteSpace(selectUID))
                sql += string.Format(" or uid='{0}'", selectUID);
            var objs = DbHelper.DataTableText<SysUserInfo>(sql, null);
            return objs;
        }
        internal List<SysUserInfo> GetUsersForDropDown(int selectType, string uid)
        {
            string sql = string.Empty;
            if (selectType == 1)
            { //全部
                sql = "SELECT [UID],FullName FROM dbo.SysUserInfo where 1=1";
            }
            else
            { //排除锁定、注销
                sql = "SELECT [UID],FullName FROM dbo.SysUserInfo WHERE [Status]=1";
            }
            if (!string.IsNullOrWhiteSpace(uid))
                sql += string.Format(" or [UID]='{0}'", uid);
            var objs = DbHelper.DataTableText<SysUserInfo>(sql, null);
            return objs;
        }
    }
}