using Pharos.Logic.OMS.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;
using Pharos.Logic.OMS.IDAL;
using System.Collections.Generic;
using System.Data.Entity;
namespace Pharos.Logic.OMS.DAL
{
    public class ProductModuleVerRepository : BaseRepository<ProductModuleVer>, IDAL.IProductModuleVerRepository
    {
        public ProductModuleVer GetOfficialLast(int productId)
        {
            return base.Entities.Where(o => o.ProductId == productId && o.VerStatus == 2 && o.Status == 1).OrderByDescending(o => o.VerCode).Include(o => o.ProductMenuLimits).FirstOrDefault();
        }
    }
    public class ProductRoleVerRepository : BaseRepository<ProductRoleVer>, IDAL.IProductRoleVerRepository
    {
        public ProductRoleVer GetOfficialLast(int productId)
        {
            return base.Entities.Where(o => o.ProductId == productId && o.VerStatus == 2 && o.Status == 1).OrderByDescending(o => o.VerCode).Include(o => o.ProductRoles).FirstOrDefault();
        }
    }
    public class ProductDictionaryVerRepository : BaseRepository<ProductDictionaryVer>, IDAL.IProductDictionaryVerRepository
    {
        public ProductDictionaryVer GetOfficialLast(int productId)
        {
            return base.Entities.Where(o => o.ProductId == productId && o.VerStatus == 2 && o.Status == 1).OrderByDescending(o => o.VerCode).Include(o => o.ProductDictionaryDatas).FirstOrDefault();
        }
    }
    public class ProductDataVerRepository : BaseRepository<ProductDataVer>, IDAL.IProductDataVerRepository
    {
        public ProductDataVer GetOfficialLast(int productId)
        {
            return base.Entities.Where(o => o.ProductId == productId && o.VerStatus == 2 && o.Status == 1).OrderByDescending(o => o.VerCode).Include(o => o.ProductDataSqls).FirstOrDefault();
        }
    }
}
