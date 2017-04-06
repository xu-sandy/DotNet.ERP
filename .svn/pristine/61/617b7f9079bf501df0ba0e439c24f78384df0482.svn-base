using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Pharos.Utility.Helpers;

namespace Pharos.Logic.DAL
{
    public class BaseRepository<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext, new()
    {
        internal readonly TDbContext _context;
        private DbSet<TEntity> _entities;
        public BaseRepository(bool isCompulsoryMiss = false)
        {
            this._context = ContextFactory.GetCurrentContext<TDbContext>(isCompulsoryMiss);
        }

        public TEntity Add(TEntity entity, bool isSave = true)
        {
            try
            {
                this.Entities.Add(entity);
                if (isSave)
                {
                    this._context.SaveChanges();
                }
                return entity;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ReturnException(dbEx);
            }
        }
        public List<TEntity> AddRange(List<TEntity> entities, bool isSave = true)
        {
            try
            {
                this.Entities.AddRange(entities);

                if (isSave)
                {
                    this._context.SaveChanges();
                }
                return entities;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ReturnException(dbEx);
            }
        }
        public bool Remove(TEntity entity, bool isSave = true)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);
                if (isSave)
                    return this._context.SaveChanges() > 0;
                else
                    return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ReturnException(dbEx);
            }
        }
        public bool RemoveRange(List<TEntity> entities, bool isSave = true)
        {
            try
            {
                this.Entities.RemoveRange(entities);
                if (isSave)
                    return this._context.SaveChanges() > 0;
                else
                    return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ReturnException(dbEx);
            }
        }
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Entities.Count(predicate);
        }

        public bool Update(TEntity entity, bool isSave = true)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (isSave)
                    return this._context.SaveChanges() > 0;
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ReturnException(dbEx);
            }
        }
        public bool Update(List<TEntity> entities, bool isSave = true)
        {
            try
            {
                if (entities==null)
                    throw new ArgumentNullException("entity");
                if (isSave)
                    return this._context.SaveChanges() > 0;
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ReturnException(dbEx);
            }
        }
        public bool IsExist(Expression<Func<TEntity, bool>> anyLambda)
        {
            
            return this.Entities.Any(anyLambda);
        }
        public TEntity FindById(object id, params string[] includeParams)
        {
            var ents = this.Entities;
            foreach (var inc in includeParams)
                ents.Include(inc);
            TEntity entity = ents.Find(id);
            return entity;
        }
        public TEntity Find(Expression<Func<TEntity, bool>> whereLambda, params string[] includeParams)
        {
            var ents = this.Entities;
            foreach (var inc in includeParams)
                ents.Include(inc);
            TEntity entity = ents.FirstOrDefault<TEntity>(whereLambda);
            return entity;
        }

        public IQueryable<TEntity> FindList(Expression<Func<TEntity, bool>> whereLambda, string orderName, bool isAsc, params string[] includeParams)
        {
            var list = this.Entities.Where(t => 1 == 1);
            if (whereLambda != null)
            {
                list = this.Entities.Where(whereLambda);
            }
            foreach (var inc in includeParams)
                list.Include(inc);
            list = list.OrderBy(orderName, isAsc);
            return list;
        }
        public IQueryable<TEntity> FindList(Expression<Func<TEntity, bool>> whereLambda, int takeNum = 0, params string[] includeParams)
        {
            var _list = this.Entities.Where(t => 1 == 1);
            if (whereLambda != null)
            {
                _list = this.Entities.Where(whereLambda);
            }
            if (takeNum > 0)
                _list = _list.Take(takeNum);
            foreach (var inc in includeParams)
                _list.Include(inc);
            return _list;
        }

        #region Private
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">原IQueryable</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="isAsc">是否正序</param>
        /// <returns>排序后的IQueryable<T></returns>
        private IQueryable<TEntity> OrderBy(IQueryable<TEntity> source, string propertyName, bool isAsc)
        {
            if (source == null) throw new ArgumentNullException("source", "Can not be empty");//不能为空
            if (string.IsNullOrEmpty(propertyName)) return source;
            var _parameter = Expression.Parameter(source.ElementType);
            var _property = Expression.Property(_parameter, propertyName);
            if (_property == null) throw new ArgumentNullException("propertyName", "The property does not exist.");//属性不存在
            var _lambda = Expression.Lambda(_property, _parameter);
            var _methodName = isAsc ? "OrderBy" : "OrderByDescending";
            var _resultExpression = Expression.Call(typeof(Queryable), _methodName, new Type[] { source.ElementType, _property.Type }, source.Expression, Expression.Quote(_lambda));
            return source.Provider.CreateQuery<TEntity>(_resultExpression);
        }
        private Exception ReturnException(DbEntityValidationException dbEx)
        {
            var msg = string.Empty;

            foreach (var validationErrors in dbEx.EntityValidationErrors)
                foreach (var validationError in validationErrors.ValidationErrors)
                    msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

            var fail = new Exception(msg, dbEx);
            return fail;
        }
        private DbEntityEntry GetEntityEntry(TEntity entity)
        {
            return this._context.Entry<TEntity>(entity);
        }
        #endregion

        #region Properties
        public IQueryable<TEntity> QueryEntity
        {
            get
            {
                return this.Entities;
            }
        }

        public DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<TEntity>();
                return _entities;
            }
        }
        #endregion
    }
}
