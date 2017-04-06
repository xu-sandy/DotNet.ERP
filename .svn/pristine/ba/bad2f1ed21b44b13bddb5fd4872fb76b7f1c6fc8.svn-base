using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Sys.Entity;
using System.Data.SqlClient;
using System.Data;

namespace Pharos.Sys.DAL
{
    internal class SysWebSettingDAL : BaseSysEntityDAL<SysWebSetting>
    {
        public SysWebSettingDAL() : base("SysWebSetting") { }

        /// <summary>
        /// 获取基本配置信息列表
        /// </summary>
        /// <returns></returns>
        internal SysWebSetting GetWebSetting()
        {
            string sql = string.Format("select * from {0} Where CompanyId=" + Sys.SysCommonRules.CompanyId, TableName);
            var result = DbHelper.DataTableText<SysWebSetting>(sql, null);
            return result != null ? result.FirstOrDefault() : null;
        }

        /// <summary>
        /// 更新基本配置信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal bool Update(SysWebSetting model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@TopLogo", model.TopLogo),
                    new SqlParameter("@BottomLogo", model.BottomLogo),
                    new SqlParameter("@AppIcon640", model.AppIcon640),
                    new SqlParameter("@AppIcon960", model.AppIcon960),
                    new SqlParameter("@AppCustomer640", model.AppCustomer640),
                    new SqlParameter("@AppCustomer960", model.AppCustomer960),
                    new SqlParameter("@AppIndexIcon640", model.AppIndexIcon640),
                    new SqlParameter("@AppIndexIcon960", model.AppIndexIcon960),
                    new SqlParameter("@AppIndexbg640", model.AppIndexbg640),
                    new SqlParameter("@AppIndexbg960", model.AppIndexbg960),
                    new SqlParameter("@SysIcon", model.SysIcon),
                    new SqlParameter("@WebsiteUrl", model.WebsiteUrl),
                    new SqlParameter("@CompanyTitle", model.CompanyTitle),
                    new SqlParameter("@Tel", model.Tel),
                    new SqlParameter("@PageTitle", model.PageTitle),
                    new SqlParameter("@SMTPServer", model.SMTPServer),
                    new SqlParameter("@SMTPPort", model.SMTPPort),
                    new SqlParameter("@SMTPShowName", model.SMTPShowName),
                    new SqlParameter("@SMTPSSLPort", model.SMTPSSLPort),
                    new SqlParameter("@SMTPAccount", model.SMTPAccount),
                    new SqlParameter("@SMTPPwd", model.SMTPPwd),
                    new SqlParameter("@EMReqUrlBase", model.EMReqUrlBase),
                    new SqlParameter("@EMAppId", model.EMAppId),
                    new SqlParameter("@EMAppSecret", model.EMAppSecret),
                    new SqlParameter("@EMAppOrg", model.EMAppOrg),
                    new SqlParameter("@EMAppName", model.EMAppName)
            };
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update {0} set ", TableName);
            sql.Append("TopLogo=@TopLogo,");
            sql.Append("BottomLogo=@BottomLogo,");
            sql.Append("AppIcon640=@AppIcon640,");
            sql.Append("AppIcon960=@AppIcon960,");
            sql.Append("AppCustomer640=@AppCustomer640,");
            sql.Append("AppCustomer960=@AppCustomer960,");
            sql.Append("AppIndexIcon640=@AppIndexIcon640,");
            sql.Append("AppIndexIcon960=@AppIndexIcon960,");
            sql.Append("AppIndexbg640=@AppIndexbg640,");
            sql.Append("AppIndexbg960=@AppIndexbg960,");
            sql.Append("SysIcon=@SysIcon,");
            sql.Append("WebsiteUrl=@WebsiteUrl,");
            sql.Append("CompanyTitle=@CompanyTitle,");
            sql.Append("Tel=@Tel,");
            sql.Append("PageTitle=@PageTitle,");
            sql.Append("SMTPServer=@SMTPServer,");
            sql.Append("SMTPPort=@SMTPPort,");
            sql.Append("SMTPShowName=@SMTPShowName,");
            sql.Append("SMTPSSLPort=@SMTPSSLPort,");
            sql.Append("SMTPAccount=@SMTPAccount,");
            sql.Append("SMTPPwd=@SMTPPwd,");
            sql.Append("EMReqUrlBase=@EMReqUrlBase,");
            sql.Append("EMAppId=@EMAppId,");
            sql.Append("EMAppSecret=@EMAppSecret,");
            sql.Append("EMAppOrg=@EMAppOrg,");
            sql.Append("EMAppName=@EMAppName");
            sql.Append(" where Id=@Id");

            int rows = DbHelper.ExecuteNonQueryText(sql.ToString(), parms);
            return rows > 0 ? true : false;
        }

        /// <summary>
        /// 根据CompanyId获取基本配置信息列表
        /// </summary>
        /// <returns></returns>
        internal SysWebSetting GetWebSetting(int CompanyId)
        {
            string sql = string.Format("select * from {0} Where CompanyId=" + CompanyId, TableName);
            var result = DbHelper.DataTableText<SysWebSetting>(sql, null);
            return result != null ? result.FirstOrDefault() : null;
        }

    }
}
