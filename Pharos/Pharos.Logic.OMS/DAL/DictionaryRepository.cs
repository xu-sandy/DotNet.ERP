using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using System.Data.SqlClient;
using System.Linq;
namespace Pharos.Logic.OMS.DAL
{
    public class DictionaryRepository : BaseRepository<SysDataDictionary>, IDictRepository
    {
        public System.Collections.Generic.List<SysDataDictionaryExt> GetPageList(int pageIndex, int pageSize, string key)
        {
            using (EFDbContext db = new EFDbContext())
            {
                var sql = string.Format("exec Sys_DataDicList @Key,@CurrentPage,@PageSize");
                SqlParameter[] parms = {
                    new SqlParameter("@Key", key),
                    new SqlParameter("@CurrentPage", pageIndex),
                    new SqlParameter("@PageSize", pageSize)
                };
                return db.Database.SqlQuery<SysDataDictionaryExt>(sql, parms).ToList();
            }
        }
    }
}
