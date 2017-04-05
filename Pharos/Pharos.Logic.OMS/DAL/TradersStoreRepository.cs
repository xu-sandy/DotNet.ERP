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
    /// DAL商家门店
    /// </summary>
    public class TradersStoreRepository : BaseRepository<TradersStore>, ITradersStoreRepository
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="CurrentPage">当前页,从1开始,不是0</param>
        /// <param name="PageSize">每页显示多少条数据</param>
        /// <param name="strw">where条件</param>
        /// <param name="Count">总数</param>
        /// <returns></returns>
        public List<ViewTradersStore> getPageList(int CurrentPage, int PageSize, string strw, out int Count)
        {
            string Table = "";
            Table = Table + "TradersStore s ";
            Table = Table + "left join SysUser u on u.UserId=s.AssignUID ";
            Table = Table + "left join SysUser u2 on u2.UserId=s.CreateUID ";
            Table = Table + "left join SysUser u3 on u3.UserId=s.AuditUID ";
            Table = Table + "left join TradersUser e on e.TUserId=s.MainAccount ";

            string Fields = "";
            Fields = Fields + "s.Id,u.FullName as Assign,s.State,s.CID,s.StoreNum,s.StoreName,e.LoginName,s.StoreNum3,s.QRCode,s.CreateDT,u2.FullName as CreatePerson,S.AuditDT,u3.FullName as AuditPerson ";

            string Where = "1=1 ";
            if (strw != "")
            {
                Where = Where + strw;
            }

            string OrderBy = "s.Id desc ";

            return CommonDal.getPageList<ViewTradersStore>(Table, Fields, Where, OrderBy, CurrentPage, PageSize, 0, out Count);
        }

        /// <summary>
        /// 获取最大门店编号
        /// </summary>
        public ViewMaxStoreNum getMaxStoreNum()
        {
            using (EFDbContext db = new EFDbContext())
            {
                var sql = "select MAX(CONVERT(bigint,StoreNum)) as MaxStoreNum from TradersStore";
                List<ViewMaxStoreNum> list = db.Database.SqlQuery<ViewMaxStoreNum>(sql).ToList();
                return list.FirstOrDefault();
            }
        }
    }
}
