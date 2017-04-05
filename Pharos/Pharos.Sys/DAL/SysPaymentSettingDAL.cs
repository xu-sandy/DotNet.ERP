﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Sys.Entity;
using System.Data.SqlClient;
using Pharos.Sys.EntityExtend;
using System.Data;
using Pharos.Utility;

namespace Pharos.Sys.DAL
{
    internal class SysPaymentSettingDAL : BaseSysEntityDAL<SysPaymentSetting>
    {
        public SysPaymentSettingDAL() : base("SysPaymentSetting") { }

        /// <summary>
        /// 获取支付配置信息列表
        /// </summary>
        /// <returns></returns>
        internal DataTable GetList(Paging paging, int PayType, string StoreId, int State)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@PayType", PayType),
                    new SqlParameter("@StoreId", StoreId),
                    new SqlParameter("@State", State),
                    new SqlParameter("@CurrentPage", paging.PageIndex),
                    new SqlParameter("@PageSize", paging.PageSize),
                    new SqlParameter("@CompanyId",Sys.SysCommonRules.CompanyId)
                                   };

            return DbHelper.DataTable("Sys_PaymentList", parms, ref paging);
        }

        /// <summary>
        /// 修改支付配置信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal bool Update(SysPaymentSetting model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@StoreId", model.StoreId),
                    new SqlParameter("@PayType", model.PayType),
                    new SqlParameter("@State", model.State),
                    new SqlParameter("@PartnerId", model.PartnerId),
                    new SqlParameter("@AppId", model.AppId),
                    new SqlParameter("@Memo", model.Memo),
                    new SqlParameter("@CheckKey", model.CheckKey),
                    new SqlParameter("@NotifyUrl", model.NotifyUrl),
                    new SqlParameter("@AlterDT", model.AlterDT),
                    new SqlParameter("@AlterUID", model.AlterUID)                              
            };
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update {0} set ", TableName);
            sql.Append("StoreId=@StoreId,");
            sql.Append("PayType=@PayType,");
            sql.Append("State=@State,");
            sql.Append("PartnerId=@PartnerId,");
            sql.Append("AppId=@AppId,");
            sql.Append("Memo=@Memo,");
            sql.Append("CheckKey=@CheckKey,");
            sql.Append("NotifyUrl=@NotifyUrl,");
            sql.Append("AlterDT=@AlterDT,");
            sql.Append("AlterUID=@AlterUID");
            sql.Append(" where Id=@Id");

            int rows = DbHelper.ExecuteNonQueryText(sql.ToString(), parms);
            return rows > 0 ? true : false;
        }

        /// <summary>
        /// 新增支付配置信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal int Insert(SysPaymentSetting model)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@StoreId", model.StoreId),
                    new SqlParameter("@PayType", model.PayType),
                    new SqlParameter("@State", model.State),
                    new SqlParameter("@PartnerId", model.PartnerId),
                    new SqlParameter("@AppId", model.AppId),
                    new SqlParameter("@Memo", model.Memo),
                    new SqlParameter("@CheckKey", model.CheckKey),
                    new SqlParameter("@NotifyUrl", model.NotifyUrl),
                    new SqlParameter("@CreateDT", model.CreateDT),
                    new SqlParameter("@CreateUID", model.CreateUID),
                    new SqlParameter("@CompanyId",model.CompanyId)
            };

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("insert into {0} (", TableName);
            sql.Append("StoreId,PayType,State,PartnerId,AppId,CheckKey,NotifyUrl,CreateDT,CreateUID,CompanyId,Memo)");
            sql.Append(" values (@StoreId,@PayType,@State,@PartnerId,@AppId,@CheckKey,@NotifyUrl,@CreateDT,@CreateUID,@CompanyId,@Memo)");
            sql.Append(";select @@IDENTITY");

            object obj = DbHelper.ExecuteScalarText(sql.ToString(), parms);
            return (obj == null) ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal bool SetState(int id, short state)
        {
            var alterDT = DateTime.Now;
            var alterUid = CurrentUser.UID;

            SqlParameter[] parms = {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@State", state),
                    new SqlParameter("@AlterDT", alterDT),
                    new SqlParameter("@AlterUID", alterUid)
                };

            string sql = string.Format("update {0} set State=@State,AlterDT=@AlterDT,AlterUID=@AlterUID where Id=@Id", TableName);

            int rows = DbHelper.ExecuteNonQueryText(sql, parms);
            return rows > 0 ? true : false;
        }
        /// <summary>
        /// 获取最新配置信息
        /// </summary>
        /// <param name="payType">（1：支付宝，2：微信)</param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        internal SysPaymentSetting GetPaymentSettingBystoreId(int payType, string storeId,int companyId)
        {
            string sql = "SELECT TOP 1 * FROM dbo.SysPaymentSetting WHERE PayType=" + payType + " AND companyId="+companyId+" and State=1 AND (StoreId='0' OR ','+StoreId+',' LIKE '%," + storeId + ",%') ORDER BY ISNULL(AlterDT,CreateDT) desc";
            var list = DbHelper.ExecuteReaderText<SysPaymentSetting>(sql, null);
            if (list == null || !list.Any()) return null;
            return list.FirstOrDefault();
        }
        /// <summary>
        /// 判断门店是否已有支付方式
        /// </summary>
        /// <param name="PayType"></param>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        internal string IsExitStore(int PayType, string StoreId, bool IsUpdate, int Id)
        {
            int n = 0;
            string stores = "";
            string[] arr = StoreId.Split(',');
            var sql = "SELECT Id,StoreId FROM dbo.SysPaymentSetting WHERE State=1 AND PayType=" + PayType+" and companyid="+SysCommonRules.CompanyId;
            var tb = DbHelper.DataTableText(sql, null);
            var storeList = "SELECT DISTINCT StoreId FROM dbo.Warehouse where companyid="+SysCommonRules.CompanyId;
            var storeListTb = DbHelper.DataTableText(storeList, null);

            if (StoreId.StartsWith("-1"))
            {
                for (int x = 0; x < tb.Rows.Count; x++)
                {
                    if (IsUpdate && (int.Parse(tb.Rows[x]["Id"].ToString()) == Id))
                    {
                        continue;
                    }

                    var sqlResult = string.Format("SELECT [dbo].[Comm_F_NumIsInGroup]('{0}','{1}')", "0", tb.Rows[x]["StoreId"].ToString());
                    var result = DbHelper.ExecuteScalarText(sqlResult, null).ToString();
                    if (int.Parse(result) > 0)
                    {
                        stores = "全部门店都";
                        return stores;
                    }

                }
                if (string.IsNullOrEmpty(stores))
                {
                    for (int y = 0; y < storeListTb.Rows.Count; y++)
                    {
                        for (int z = 0; z < tb.Rows.Count; z++)
                        {
                            if (IsUpdate && (int.Parse(tb.Rows[z]["Id"].ToString()) == Id))
                            {
                                continue;
                            }

                            var sqlResult = string.Format("SELECT [dbo].[Comm_F_NumIsInGroup]('{0}','{1}')", storeListTb.Rows[y]["StoreId"].ToString(), tb.Rows[z]["StoreId"].ToString());
                            var result = DbHelper.ExecuteScalarText(sqlResult, null).ToString();
                            var sqlStoreTitle = string.Format("SELECT Title FROM dbo.Warehouse WHERE StoreId='{0}' and companyid={1}", storeListTb.Rows[y]["StoreId"].ToString(),SysCommonRules.CompanyId);
                            var storeTitle = DbHelper.ExecuteScalarText(sqlStoreTitle, null).ToString();

                            if (tb.Rows[z]["StoreId"].ToString() == "0")
                            {
                                if (n > 0)
                                {
                                    stores += "、" + storeTitle;
                                }
                                else
                                {
                                    stores += storeTitle;
                                }
                                n++;
                                break;
                            }
                            else
                            {
                                if (int.Parse(result) > 0)
                                {
                                    if (n > 0)
                                    {
                                        stores += "、" + storeTitle;
                                    }
                                    else
                                    {
                                        stores += storeTitle;
                                    }
                                    n++;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    for (int j = 0; j < tb.Rows.Count; j++)
                    {
                        var input = arr[i].ToString();
                        if (!string.IsNullOrEmpty(input))
                        {
                            if (IsUpdate && (int.Parse(tb.Rows[j]["Id"].ToString()) == Id))
                            {
                                continue;
                            }

                            var store = tb.Rows[j]["StoreId"].ToString();
                            var sqlResult = string.Format("SELECT [dbo].[Comm_F_NumIsInGroup]('{0}','{1}')", input, store);
                            var result = DbHelper.ExecuteScalarText(sqlResult, null).ToString();
                            var sqlStoreTitle = string.Format("SELECT Title FROM dbo.Warehouse WHERE StoreId='{0}' and companyid={1}", arr[i].ToString(),SysCommonRules.CompanyId);
                            var storeTitle = DbHelper.ExecuteScalarText(sqlStoreTitle, null).ToString();
                            if (tb.Rows[j]["StoreId"].ToString() == "0")
                            {
                                if (n > 0)
                                {
                                    stores += "、" + storeTitle;
                                }
                                else
                                {
                                    stores += storeTitle;
                                }
                                n++;
                                break;
                            }
                            else
                            {
                                if (int.Parse(result) > 0)
                                {
                                    if (n > 0)
                                    {
                                        stores += "、" + storeTitle;
                                    }
                                    else
                                    {
                                        stores += storeTitle;
                                    }
                                    n++;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return stores;
        }


    }
}
