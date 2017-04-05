using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.Logic.OMS.IDAL
{
    public interface IBaseRepository<TEntity>
    {
        TEntity Add(TEntity entity, bool isSave = true);
        List<TEntity> AddRange(List<TEntity> entities, bool isSave = true);
        bool Remove(TEntity entity, bool isSave = true);
        bool RemoveRange(List<TEntity> entities, bool isSave = true);
        /// <summary>
        /// 传入参数，自动验证表单
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool SaveChanges(params TEntity[] entitys);
        TEntity Find(Expression<Func<TEntity, bool>> whereLambda, params string[] includeParams);
        List< TEntity> FindList(Expression<Func<TEntity, bool>> whereLambda, params string[] includeParams);
        TEntity Get(object id);
        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> whereLambda=null);
        TEntity Update(TEntity entity, bool isSave = true);
        TEntity Insert(TEntity entity, bool isSave = true);
        int GetMaxInt(Expression<Func<TEntity, int?>> fieldLambda, int min = 1, int max = int.MaxValue, Expression<Func<TEntity, bool>> whereLambda=null);
        bool IsExists(Expression<Func<TEntity, bool>> whereLambda);
    }
}
