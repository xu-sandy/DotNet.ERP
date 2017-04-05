﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Pharos.Logic.OMS.IDAL;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.OMS.DAL
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        internal readonly EFDbContext _context;
        private DbSet<TEntity> _entities;
        public BaseRepository()
        {
            this._context = ContextFactory.GetCurrentContext<EFDbContext>();
        }

        public TEntity Add(TEntity entity, bool isSave = true)
        {
            try
            {
                this.Entities.Add(entity);
                if (isSave)
                {
                    SaveChanges(entity);
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
                    SaveChanges(entities.ToArray());
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
                    return SaveChanges();
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
                    return SaveChanges();
                else
                    return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ReturnException(dbEx);
            }
        }
        public bool SaveChanges(params TEntity[] entitys)
        {
            AutoValidateorField(entitys);
            try
            {
                return this._context.SaveChanges() > 0;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ReturnException(dbEx);
            }
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
        public List<TEntity> FindList(Expression<Func<TEntity, bool>> whereLambda, params string[] includeParams)
        {
            var ents = this.Entities;
            foreach (var inc in includeParams)
                ents.Include(inc);
            var entity = ents.Where<TEntity>(whereLambda);
            return entity.ToList();
        }
        public TEntity Get(object id)
        {
            return Entities.Find(id);
        }
        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> whereLambda=null)
        {
            var _list = this.Entities.Where(t => 1 == 1);
            if (whereLambda != null)
            {
                _list = this.Entities.Where(whereLambda);
            }
            return _list;
        }

        #region Private

        private Exception ReturnException(DbEntityValidationException dbEx)
        {
            var msg = string.Empty;

            foreach (var validationErrors in dbEx.EntityValidationErrors)
                foreach (var validationError in validationErrors.ValidationErrors)
                    msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

            var fail = new Exception(msg, dbEx);
            return fail;
        }
        #endregion

        #region Properties

        protected DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<TEntity>();
                return _entities;
            }
        }
        #endregion

        public TEntity Update(TEntity entity, bool isSave = true)
        {
            var set = this._context.Set<TEntity>();
            set.Attach(entity);
            this._context.Entry<TEntity>(entity).State = EntityState.Modified;
            if (isSave)
            {
                SaveChanges(entity);
            }
            return entity;
        }

        public TEntity Insert(TEntity entity,bool isSave=true) 
        {
            this._context.Set<TEntity>().Add(entity);
            if (isSave)
            {
                SaveChanges(entity);
            }
            return entity;
        }



        public int GetMaxInt(Expression<Func<TEntity, int?>> fieldLambda, int min = 1, int max = int.MaxValue, Expression<Func<TEntity, bool>> whereLambda = null)
        {
            var query = this.Entities.AsNoTracking().Where(o=>true);
            if (whereLambda != null) query = query.Where(whereLambda);
            var maxValue = query.Max(fieldLambda);
            if (!maxValue.HasValue)
                maxValue = min;
            else
                maxValue = maxValue.Value + 1;
            if (maxValue > max)
                throw new ArgumentException("最大值超过"+max);
            return maxValue.GetValueOrDefault();
        }


        public bool IsExists(Expression<Func<TEntity, bool>> whereLambda)
        {
            return Entities.AsNoTracking().Any(whereLambda);
        }

        #region 表单验证
        void AutoValidateorField(params TEntity[] entitys)
        {
            if (entitys == null || !entitys.Any()) return;
            string tableName = "";
            var custom = typeof(TEntity).GetCustomAttributes(typeof(TableAttribute), false);
            if (custom.Length > 0) tableName = ((TableAttribute)custom[0]).Name;
            var entType = typeof(TEntity);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = entType.Name;
            }
            string sql = @"SELECT c.Name,c.length MaxLength,c.isnullable IsNullable,b.name [Type],g.value Descript FROM syscolumns c
                INNER join sysobjects t ON c.id=t.id
                LEFT JOIN systypes b on c.xusertype=b.xusertype 
				left join sys.extended_properties g on c.id=g.major_id and c.colid=g.minor_id
                WHERE t.xtype='U' AND t.name=@tbname";
            var fields = this._context.Database.SqlQuery<ValidateorField>(sql, new System.Data.SqlClient.SqlParameter("@tbname", tableName)).ToList();
            if (fields != null && fields.Any())
            {
                foreach (var ent in entitys)
                {
                    foreach (var p in entType.GetProperties())
                    {
                        var fd = fields.FirstOrDefault(o => o.Name == p.Name);
                        if (fd == null) continue;
                        var val = Convert.ToString(p.GetValue(ent, null));
                        DateTime date = DateTime.Now;
                        int num = 0;
                        var desc = p.Name;
                        if (!string.IsNullOrWhiteSpace(fd.Descript)) desc = fd.Descript;
                        if (fd.IsNullable == 0 && string.IsNullOrWhiteSpace(val))
                            throw new ArgumentNullException("[" + desc + "]不能为空!");
                        else if (fd.Type == "datetime" && !string.IsNullOrWhiteSpace(val) && !DateTime.TryParse(val, out date))
                            throw new ArgumentException("[" + desc + "]参数类型不正确!");
                        else if ((fd.Type == "tinyint" || fd.Type == "smallint" || fd.Type == "int" || fd.Type == "bigint" || fd.Type == "numeric") && !string.IsNullOrWhiteSpace(val) && !int.TryParse(val, out num))
                            throw new ArgumentException("[" + desc + "]参数类型不正确!");
                        else if (p.PropertyType == typeof(string))
                        {
                            var count = System.Text.Encoding.Default.GetByteCount(val);
                            var max = fd.MaxLength;
                            if (fd.Type == "nvarchar")
                            {
                                max /= 2;
                                count = val.Length;
                            }
                            if (max > 0 && max < count)
                                throw new ArgumentException("[" + desc + "]不允许超过[" + max + "]!");
                        }
                    }
                }
            }
        }
        class ValidateorField
        {
            public string Name { get; set; }
            public short? MaxLength { get; set; }
            public int? IsNullable { get; set; }
            public string Type { get; set; }
            public string Descript { get; set; }
        }
        #endregion 
    }
}
