﻿using System;
using System.Data.SqlClient;
using System.Configuration;
using Pharos.DBFramework;

namespace Pharos.Pay.PayResult
{
    public class NotifyDAL
    {
        public static void AddOne(PayNotifyResult obj)
        {
            DBHelper db = new DBHelper();
            try
            {
                //
                string sql = @"select count(*) from PayNotifyResult where CompanyId=@CompanyId and PaySN=@PaySN and TradeNo=@TradeNo";
                SqlParameter[] parms = {
                                           new SqlParameter("@CompanyId", obj.CompanyId),
                                           new SqlParameter("@PaySN", obj.PaySN),
                                           new SqlParameter("@TradeNo", obj.TradeNo)
                                       };
                object count = db.ExecuteScalarText(sql, parms);
                if (Convert.ToInt32(count) <= 0)
                {
                    sql = @"INSERT INTO PayNotifyResult([ApiCode],[PaySN],[TradeNo],[CashFee],[State],[CompanyId]) VALUES(@apicode,@paysn,@tradeno,@cashfee,@State,@CompanyId)";
                    SqlParameter[] parmsNew = {
                                new SqlParameter("@apicode",obj.ApiCode),
                                new SqlParameter("@paysn",obj.PaySN),
                                new SqlParameter("@tradeno",obj.TradeNo),
                                new SqlParameter("@cashfee",obj.CashFee),
                                new SqlParameter("@State",obj.State),
                                new SqlParameter("@CompanyId",obj.CompanyId)
                            };
                    if (db.ExecuteNonQueryText(sql, parmsNew) < 1)
                    {
                        sql = @"INSERT INTO [SysLog]([Type],[UId],[Summary],[ClientIP] ,[ServerName]) values(4,'a10b0206a05c4fab97e526db06e72aff',@Summary,'127.0.0.1','dongben')";
                        SqlParameter[] parmsError = {
                                       new SqlParameter("@Summary", "通知结果失败!")
                                   };
                        db.ExecuteNonQueryText(sql, parmsError);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(typeof(NotifyDAL).Name, "通知结果失败!" + ex.Message);
                try
                {
                    string sql = @"INSERT INTO [SysLog]([Type],[UId],[Summary],[ClientIP] ,[ServerName]) values(4,'a10b0206a05c4fab97e526db06e72aff',@Summary,'127.0.0.1','dongben')";
                    SqlParameter[] parmsError = {
                        new SqlParameter("@Summary", "通知结果失败!"+ex.Message)
                    };
                    db.ExecuteNonQueryText(sql, parmsError);
                }
                catch { }
            }

            #region //原先的 临时保留
            /*
            var connStr = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    sql = @"select count(*) from PayNotifyResult where PaySN=@PaySN and TradeNo=@TradeNo";
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@PaySN", obj.PaySN));
                    cmd.Parameters.Add(new SqlParameter("@TradeNo", obj.TradeNo));
                    var result= cmd.ExecuteScalar();
                    if (Convert.ToInt32(result) <= 0)
                    {
                        cmd.Parameters.Clear();
                        sql = @"INSERT INTO PayNotifyResult([ApiCode],[PaySN],[TradeNo],[CashFee],[State],[CompanyId]) VALUES(@apicode,@paysn,@tradeno,@cashfee,@State,@CompanyId)";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddRange(new SqlParameter[] { 
                            new SqlParameter("@apicode",obj.ApiCode),
                            new SqlParameter("@paysn",obj.PaySN),
                            new SqlParameter("@tradeno",obj.TradeNo),
                            new SqlParameter("@cashfee",obj.CashFee),
                            new SqlParameter("@State",obj.State),
                            new SqlParameter("@CompanyId",companyId)
                        });
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(typeof(NotifyDAL).Name, "通知结果失败!" + ex.Message);
                    string sql = @"INSERT INTO [SysLog]([Type],[UId],[Summary],[ClientIP] ,[ServerName]) values(4,'a10b0206a05c4fab97e526db06e72aff',@Summary,'127.0.0.1','dongben')";
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@Summary", "通知结果失败!" + ex.Message+"\r\n"+ex.StackTrace));
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
            }
        */
            #endregion

        }
    }


    /// <summary>
    /// 支付结果
    /// </summary>
    public class PayNotifyResult
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        /// <summary>
        /// 支付方式  14-支付宝13-微信
        /// </summary>
        public int ApiCode { get; set; }
        /// <summary>
        /// 销售单流水号
        /// </summary>
        public string PaySN { get; set; }
        /// <summary>
        /// 交易号
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal CashFee { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 状态 Success-成功 
        /// </summary>
        public string State { get; set; }
    }
}