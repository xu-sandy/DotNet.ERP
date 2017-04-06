using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Pharos.DBFramework;

namespace Pharos.Logic.DAL
{
    /// <summary>
    /// 供应平台
    /// </summary>
    internal class MsppDAL
    {
        DBHelper db = new DBHelper();
        
        /// <summary>
        /// 根据状态，订购日期查询订单配送列表
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="orderDate">订购日期</param>
        /// <returns>订单配送列表</returns>
        public DataTable GetOrderDistributionListBySearch(int? state, DateTime? beginOrderDate, DateTime? endOrderDate)
        {
            var parms = new List<SqlParameter>();
            string sql = "SELECT *,dbo.F_UserNameById(OrdererUID) OrdererTitle FROM dbo.IndentOrder,OrderDistribution WHERE OrderDistribution.IndentOrderId=IndentOrder.Id ";           
            if (state.HasValue)
            {
                sql += " AND State=@State";
                parms.Add(new SqlParameter("@State", state.Value));
            }
            if (beginOrderDate > endOrderDate)
            {
                DateTime? temp = beginOrderDate;
                beginOrderDate = endOrderDate;
                endOrderDate = temp;
            }
            if (beginOrderDate.HasValue)
            {
                sql += " AND (CreateDT>@BeginOrderDate OR CreateDT=@BeginOrderDate)";
                parms.Add(new SqlParameter("@BeginOrderDate", beginOrderDate.Value));
            }
            if (endOrderDate.HasValue)
            {
                sql += " AND (CreateDT<@EndOrderDate OR CreateDT=@EndOrderDate)";
                parms.Add(new SqlParameter("@EndOrderDate", endOrderDate.Value));
            }
           
            return db.DataTableText(sql, parms.ToArray());
        }

    }
}
