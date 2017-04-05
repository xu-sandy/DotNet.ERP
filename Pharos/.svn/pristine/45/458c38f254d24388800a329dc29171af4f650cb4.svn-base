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
    /// 代理商帐号DAL
    /// </summary>
    public class AgentsUsersRepository : BaseRepository<AgentsUsers>, IAgentsUsersRepository
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="CurrentPage">当前页,从1开始,不是0</param>
        /// <param name="PageSize">每页显示多少条数据</param>
        /// <param name="strw">where条件</param>
        /// <param name="Count">总数</param>
        /// <returns></returns>
        public List<AgentsUsers> getPageList(int CurrentPage, int PageSize, string strw, out int Count)
        {
            string Table = "";
            Table = Table + "AgentsUsers u ";

            string Fields = "";
            Fields = Fields + "u.* ";

            string Where = "1=1 ";
            if (strw != "")
            {
                Where = Where + strw;
            }

            string OrderBy = "u.Id desc ";

            return CommonDal.getPageList<AgentsUsers>(Table, Fields, Where, OrderBy, CurrentPage, PageSize, 0, out Count);
        }
    }
}
