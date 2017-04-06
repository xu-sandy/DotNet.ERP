using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharos.Logic.OMS.Entity.View;
using System.Data;
using System.Data.SqlClient;
using Pharos.Logic.OMS.IDAL;


namespace Pharos.Logic.OMS.DAL
{
    public static class CommonDal
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="Table">表名,支持多表联查</param>
        /// <param name="Fields">字段名</param>
        /// <param name="Where">where条件,不需要加where</param>
        /// <param name="OrderBy">排序条件，不需要加order by</param>
        /// <param name="CurrentPage">当前页,从1开始,不是0</param>
        /// <param name="PageSize">每页显示多少条数据</param>
        /// <param name="GetCount">获取的记录总数，0则获取记录总数，不为0则不获取</param>
        /// <param name="Count">总数</param>
        /// <returns></returns>
        public static List<T> getPageList<T>(string Table, string Fields, string Where, string OrderBy, int CurrentPage, int PageSize, int GetCount, out int Count) 
        {
            using (EFDbContext db = new EFDbContext())
            {
                Count = 0;
                var sql = string.Format("exec Pagination @Table,@Fields,@Where,@OrderBy,@CurrentPage,@PageSize,@GetCount,@Count out");
                SqlParameter[] parameters = { 
                                    new SqlParameter("@Table",SqlDbType.NVarChar,1000),
                                    new SqlParameter("@Fields",SqlDbType.Text),
                                    new SqlParameter("@Where",SqlDbType.NVarChar,4000),
                                    new SqlParameter("@OrderBy",SqlDbType.VarChar,1000),
                                    new SqlParameter("@CurrentPage",SqlDbType.Int,4),
                                    new SqlParameter("@PageSize",SqlDbType.Int,4),
                                    new SqlParameter("@GetCount",SqlDbType.Int,4),
                                    new SqlParameter("@Count",SqlDbType.Int,4)
                                    };
                parameters[0].Value = Table;
                parameters[1].Value = Fields;
                parameters[2].Value = Where;
                parameters[3].Value = OrderBy;
                parameters[4].Value = CurrentPage;
                parameters[5].Value = PageSize;
                parameters[6].Value = GetCount;
                parameters[7].Direction = ParameterDirection.Output;

                List<T> list = db.Database.SqlQuery<T>(sql, parameters).ToList();
                Count = Convert.ToInt32(parameters[7].Value);//输出
                return list;
            }

        }
    }
}
