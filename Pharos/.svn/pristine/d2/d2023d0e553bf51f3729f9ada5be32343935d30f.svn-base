using Pharos.DBFramework;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pharos.Logic.OMS.DAL
{
    public class BaseDAL<T>
    {
        public BaseDAL()
        {
            TableName = typeof(T).Name;
            DbHelper = new DBHelper();
        }
        public BaseDAL(string tablename)
            : this()
        {
            TableName = tablename;
        }

        #region Field
        /// <summary>
        /// 数据连接DB
        /// </summary>
        public DBHelper DbHelper { get; set; }
        /// <summary>
        /// 当前DAL表名称
        /// </summary>
        public string TableName { get; set; }
        #endregion

        /// <summary>
        /// 根据ID获取数据对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(int id)
        {
            SqlParameter[] parms = {
					new SqlParameter("@id", id)
                                   };

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from {0} where [id]=@id", TableName);

            var objs = DbHelper.DataTableText<T>(sql.ToString(), parms);
            if (objs != null && objs.Count > 0)
                return objs[0];
            else
                return default(T);
        }

        public T GetByColumn(object val, string colName)
        {
            SqlParameter[] parms = {
					new SqlParameter("@val", val)};

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from {0} where [{1}]=@val", TableName, colName);

            var objs = DbHelper.DataTableText<T>(sql.ToString(), parms);
            if (objs != null && objs.Count > 0)
                return objs[0];
            else
                return default(T);
        }
        public bool Delete(int id)
        {
            SqlParameter[] parms = {
					new SqlParameter("@id" , id)};

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("delete from {0} where [id]=@id", TableName);

            return DbHelper.ExecuteNonQueryText(sql.ToString(), parms) > 0;
        }
        /// <summary>
        /// 根据ID判断数据是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExistsById(int id)
        {
            SqlParameter[] parms = {
					new SqlParameter("@id" , id)};

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from {0} where [id]=@id", TableName);

            var obj = DbHelper.ExecuteScalarText(sql.ToString(), parms);

            return (obj == null) ? false : Convert.ToInt32(obj) > 0;
        }
        /// <summary>
        /// 获得列的最大值
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public int MaxVal(string columnName)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select isnull(max({0}),0) from {1}", columnName, TableName);

            var obj = DbHelper.ExecuteScalarText(sql.ToString(), null);

            return (obj == null) ? 0 : Convert.ToInt32(obj);
        }
        public bool ExistsColumn(object objid, string objColumn, object existValue, string existColumn)
        {
            SqlParameter[] parms = {
					new SqlParameter("@objid" , objid),
					new SqlParameter("@existValue" , existValue)};

            string sql = string.Format("select count(1) from {0} where [{1}]<>@objid and [{2}]=@existValue", TableName, objColumn, existColumn);

            var obj = DbHelper.ExecuteScalarText(sql, parms);

            return (obj == null) ? false : Convert.ToInt32(obj) > 0;
        }
        /// <summary>
        /// 自动分页方法
        /// </summary>
        /// <param name="strSql">完整sql语句</param>
        /// <param name="recordCount">记录总数</param>
        /// <param name="nvl">参数值</param>
        /// <returns></returns>
        public List<T> ExceuteSqlForPage(string strSql, out int recordCount, System.Collections.Specialized.NameValueCollection nvl = null)
        {
            nvl = nvl ?? HttpContext.Current.Request.Params;
            var pageIndex = 1;
            var pageSize = 30;
            var sort = "Id";
            var order = "asc";
            if (!nvl["page"].IsNullOrEmpty())
                pageIndex = int.Parse(nvl["page"]);
            if (!nvl["rows"].IsNullOrEmpty())
                pageSize = int.Parse(nvl["rows"]);
            if (!nvl["sort"].IsNullOrEmpty())
                sort = nvl["sort"];
            if (!nvl["order"].IsNullOrEmpty())
                order = nvl["order"];
            order = order.ToLower();
            if (!(order == "asc" || order == "desc"))
                throw new ArgumentException("排序类型错误!");

            string orderSql = string.Format("(ROW_NUMBER() OVER ( ORDER BY [{0}] {1})) AS RSNO", sort, order);
            strSql = string.Format("select * from(select {0},* from ({1}) tb) t", orderSql, strSql);
            var page = new Utility.Paging();
            var parms = new SqlParameter[] { 
                new SqlParameter("@SqlStr",strSql),
                new SqlParameter("@CurrentPage",pageIndex),
                new SqlParameter("@PageSize",pageSize)
            };
            var dt = DbHelper.DataTable("Comm_PageList", parms, ref page);
            recordCount = page.RecordTotal;
            return Pharos.DBFramework.DBHelper.ToEntity.ToList<T>(dt);
        }
        public bool SaveOrUpdate(T obj)
        {
            var type = typeof(T);
            var propers = type.GetProperties();
            string pk = "Id";
            StringBuilder sql = new StringBuilder();
            var parms = new List<SqlParameter>();
            var fieldValue = new Dictionary<string, object>();
            foreach (var p in propers)
            {
                if (!p.CanWrite ||
                       (p.PropertyType.IsGenericType && !p.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))) continue;
                var attrs = p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), false);
                if (attrs.Length > 0)
                    pk = p.Name;
                var value = p.GetValue(obj, null);
                fieldValue[p.Name] = value;
            }
            var id = fieldValue[pk];
            fieldValue.Remove(pk);
            int vl = 0;
            int.TryParse(id.ToString(), out vl);
            if (!id.IsNullOrEmpty() && vl != 0)
            {
                sql.Append("update " + TableName);
                sql.Append(" set ");
                foreach (var de in fieldValue)
                {
                    sql.Append(de.Key + "=@" + de.Key);
                    sql.Append(",");
                    parms.Add(new SqlParameter("@" + de.Key, de.Value));
                }
                sql = sql.Remove(sql.Length - 1, 1);
                sql.Append(" where ");
                sql.Append(pk + "=@" + pk);
                parms.Add(new SqlParameter("@" + pk, id));
            }
            else
            {
                sql.Append("insert " + TableName);
                sql.Append(" (");
                sql.Append(string.Join(",", fieldValue.Select(o => o.Key)));
                sql.Append(") values(");
                sql.Append(string.Join(",", fieldValue.Select(o => "@" + o.Key)));
                sql.Append(")");
                parms.AddRange(fieldValue.Select(o => new SqlParameter("@" + o.Key, o.Value)));
            }
            var s = sql.ToString();
            return DbHelper.ExecuteNonQueryText(sql.ToString(), parms.ToArray()) > 0;
        }
        public bool Delete(object[] ids)
        {
            string sql = "delete from " + TableName + " where id in(" + string.Join(",", ids) + ")";
            //SqlParameter[] parms = new SqlParameter[] { 
            //    new SqlParameter("@ids",string.Join(",", ids))
            //};
            return DbHelper.ExecuteNonQueryText(sql, null) > 0;
        }

        /// <summary>
        /// 分页获取DataTable
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
        public DataSet getPageList(string Table, string Fields, string Where, string OrderBy, int CurrentPage, int PageSize, int GetCount, out int Count)
        {
            Count = 0;
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
            DataSet ds = DbHelper.DataSet("dbo.Pagination", parameters);
            return ds;
        }


    }
}
