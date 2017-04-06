using Pharos.Logic;
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
    /// <summary>
    /// DAL商户资料
    /// </summary>
    public class TradersRepository : BaseRepository<Traders>, ITradersRepository
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="Table">表名,支持多表联查</param>
        /// <param name="Fields">字段名</param>
        /// <param name="strw">where条件,不需要加where</param>
        /// <param name="OrderBy">排序条件，不需要加order by</param>
        /// <param name="CurrentPage">当前页,从1开始,不是0</param>
        /// <param name="PageSize">每页显示多少条数据</param>
        /// <param name="GetCount">获取的记录总数，0则获取记录总数，不为0则不获取</param>
        /// <param name="Count">总数</param>
        /// <returns></returns>
        public  List<ViewTrader> getPageList(int CurrentPage, int PageSize, string strw,  out int Count)
        {
            using (EFDbContext db = new EFDbContext())
            {
                string Table = "";
                Table = Table + "Traders t left join SysUser s on s.UserId=t.AssignerUID ";
                Table = Table + "left join SysUser ss on ss.UserId=t.CreateUID ";
                Table = Table + "left join SysDataDictionary d on d.DicSN=t.TrackStautsId and d.Status=1 ";
                Table = Table + "left join SysDataDictionary dd on dd.DicSN=t.ExistStoreNum and dd.Status=1 ";
                Table = Table + "left join SysDataDictionary ddd on ddd.DicSN=t.BusinessModeId and ddd.Status=1 ";
                Table = Table + "left join Area a on a.AreaID=t.CurrentCityId ";
                Table = Table + "left join TraderType p on p.TraderTypeId=t.TraderTypeId ";

                string Fields = "";
                Fields = Fields + "t.Id,s.FullName,d.Title,t.CID,t.Title as khTitle,t.FullTitle as khFullTitle,ddd.Title as khType,a.Title as city,p.Title as tType,dd.Title as ExistStoreNum,";
                Fields = Fields + "(stuff((select ','+Title from Business where ById in (select Value from dbo.SplitString(t.BusinessScopeId,',',1))  for xml path ('')),1,1,'')) as bCategory,";
                Fields = Fields + "(stuff((select ','+Title+'   '+ CONVERT(varchar(200),OrderNum)+UnitName from OrderList where CID=t.CID  for xml path ('')),1,1,'')) as oList,";
                Fields = Fields + "t.LinkMan,t.MobilePhone,t.Status,t.CreateDT,ss.FullName as cFullName";

                string Where = "1=1 ";
                if (strw!="")
                {
                    Where = Where+strw;
                }
                string OrderBy = "t.CID desc ";
                int GetCount = 0;
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

                List<ViewTrader> list=db.Database.SqlQuery<ViewTrader>(sql, parameters).ToList();
                Count = Convert.ToInt32(parameters[7].Value);//输出
                return list;
            }


        }

        /// <summary>
        /// 获取部门ID
        /// </summary>
        public List<ViewDepart> getDepartID(int DeptId = 0)
        {
            using (EFDbContext db = new EFDbContext())
            {
                var sql = "";
                sql = sql + "WITH Depart AS( ";
                sql = sql + "SELECT   DeptId ,PDeptId,Title FROM SysDepartments WHERE DeptId =" + DeptId;
                sql = sql + " UNION ALL ";
                sql = sql + "SELECT   a.DeptId ,a.PDeptId,a.Title FROM SysDepartments AS a,Depart AS b WHERE a.PDeptId = b.DeptId ";
                sql = sql + ") ";
                sql = sql + "SELECT * FROM Depart ";
                List<ViewDepart> list = db.Database.SqlQuery<ViewDepart>(sql).ToList();
                return list;
            }
        }
    }
}
