using System;
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
    internal class TradersDAL : BaseSysEntityDAL<Traders>
    {
        public TradersDAL() : base("Traders") { }

        /// <summary>
        /// 获取客户汇总
        /// </summary>
        /// <param name="strw">where条件,不需要加where</param>
        /// <param name="CurrentPage">当前页,从1开始,不是0</param>
        /// <param name="PageSize">每页显示多少条数据</param>
        /// <returns></returns>
        internal DataSet GetList(string strw, int CurrentPage, int PageSize)
        {
            string Where = "1=1 ";
            if (strw != "")
            {
                Where = Where + strw;
            }
            SqlParameter[] parameters = { 
                                    new SqlParameter("@Where",SqlDbType.NVarChar,4000),
                                    new SqlParameter("@CurrentPage",SqlDbType.Int,4),
                                    new SqlParameter("@PageSize",SqlDbType.Int,4),
                                    new SqlParameter("@Count",SqlDbType.Int,4)
                                    };

            parameters[0].Value = Where;
            parameters[1].Value = CurrentPage;
            parameters[2].Value = PageSize;
            parameters[3].Direction = ParameterDirection.Output;
            DataSet ds = DbHelper.DataSet("dbo.Rpt_TradersSum", parameters);
            return ds;
        }
    }
}
