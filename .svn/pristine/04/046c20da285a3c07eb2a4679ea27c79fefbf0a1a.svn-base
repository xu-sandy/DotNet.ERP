using Pharos.Logic.EntityExtend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Pharos.DBFramework;

namespace Pharos.Logic.DAL
{
    public class SysDepartmentDAL
    {
        private DBHelper _db = new DBHelper();
        /// <summary>
        /// 获取组织机构列表数据
        /// </summary>
        /// <returns></returns>
        public List<SysDepartmentsExt> GetList() {
            return GetListBySearch("");
        }
        public SysDepartmentsExt GetDepById(string id)
        {
            var result = GetListBySearch(id);
            if (result != null && result.Count>0) {
                return result[0];
            }
            else
                return null;
        }
        private List<SysDepartmentsExt> GetListBySearch(string id)
        {
            SqlParameter[] parms = {
					new SqlParameter("@id", id)
				};
            var result = _db.DataTable<SysDepartmentsExt>("Sys_DepartmentList", parms);
            return result;
        }
    }

    //internal class SysDataDictionaryDAL
    //{
    //    DBHelper db = new DBHelper();

    //    /// <summary>
    //    /// 根据该父级类别下的子字典项
    //    /// </summary>
    //    /// <param name="psn">父级编号</param>
    //    public DataTable GetDicListByPSN(int psn)
    //    {
    //        SqlParameter[] parms = {
    //                new SqlParameter("@DicPSN", psn)
    //            };

    //        string sql = "SELECT DicSN,Title FROM dbo.SysDataDictionary WHERE DicPSN=@DicPSN; ";

    //        return db.DataTableText(sql, parms);
    //    }
    //}
}
