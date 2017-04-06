using Pharos.Logic.OMS.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;
using Pharos.Logic.OMS.IDAL;
namespace Pharos.Logic.OMS.DAL
{
    public class ProductRepository : BaseRepository<ProductRecord>, IProductRepository
    {
        public string GenerateNewBarcode(int categorySN)
        {
            if (categorySN > 0)
            {
                using (EFDbContext db = new EFDbContext())
                {
                    var sql = string.Format("exec Sys_GenerateNewProductCode {0}",  categorySN);
                    return db.Database.SqlQuery<string>(sql).FirstOrDefault();
                }
            }
            return "";
        }
    }
}
