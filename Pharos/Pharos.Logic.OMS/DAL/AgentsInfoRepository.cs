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
    /// 代理商档案DAL
    /// </summary>
    public class AgentsInfoRepository : BaseRepository<AgentsInfo>, IAgentsInfoRepository
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="CurrentPage">当前页,从1开始,不是0</param>
        /// <param name="PageSize">每页显示多少条数据</param>
        /// <param name="strw">where条件</param>
        /// <param name="Count">总数</param>
        /// <returns></returns>
        public List<ViewAgentsInfo> getPageList(int CurrentPage, int PageSize, string strw, out int Count)
        {
            string Table = "";
            Table = Table + "AgentsInfo a ";
            Table = Table + "left join SysDataDictionary d on d.DicSN=a.Type ";
            Table = Table + "left join BankCardInfo b on b.AgentsId=a.AgentsId ";
            Table = Table + "left join AgentPay y on y.AgentsId=a.AgentsId ";
            Table = Table + "left join PayApis i on i.ApiNo=y.ApiNo ";
            Table = Table + "left join SysUser u1 on u1.UserId=a.CreateUid ";
            Table = Table + "left join SysUser u2 on u2.UserId=a.AssignUid ";
            Table = Table + "left join AgentsUsers u3 on u3.AgentsLoginId=a.CreateUid ";
            Table = Table + "left join AgentsUsers u4 on u4.AgentsLoginId=a.AssignUid ";

            string Fields = "";
            Fields = Fields + "a.Id,d.Title,a.AgentsId,a.FullName,a.Name,a.Status,a.PAgentsId,a.EndTime,a.AgentAreaNames,i.Title as ApiTitle,y.Cost,y.Lower,i.State as apiState,a.Contract, ";
            Fields = Fields + "a.CorporateName,a.IdCard,a.CompanyPhone,a.Address,a.LinkMan,a.Phone1,a.Phone2,a.QQ,a.Email,a.Weixin,a.CreateTime,u1.FullName as sysCreFullName, ";
            Fields = Fields + "u2.FullName as sysAssFullName,u3.FullName as AgenCreFullName,u4.FullName as AgenAssFullName ";

            string Where = "1=1 ";
            if (strw != "")
            {
                Where = Where + strw;
            }

            string OrderBy = "a.Id desc ";

            return CommonDal.getPageList<ViewAgentsInfo>(Table, Fields, Where, OrderBy, CurrentPage, PageSize, 0, out Count);
        }
    }
}
