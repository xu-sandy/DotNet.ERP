﻿using Pharos.Logic.DAL;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
#if Local
using System.Data.SqlServerCe;
#endif

namespace Pharos.Logic.BLL
{
    public abstract class BaseGeneralService<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext, new()
    {
#if Local
        public BaseGeneralService()
        {
            SqlCeEngine engine = new SqlCeEngine(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            if (false == engine.Verify())
            {
                //  MessageBox.Show("Database is corrupted.");
                engine.Repair(null, RepairOption.RecoverAllOrFail);
            }
        }
#endif
        internal protected static BaseRepository<TEntity, TDbContext> CurrentRepository
        {
            get
            {
                var result = new BaseRepository<TEntity, TDbContext>(IsForcedExpired);
                IsForcedExpired = false;
                return result;
            }
        }
        /// <summary>
        /// 强制上下文过期
        /// </summary>
        public static bool IsForcedExpired { get; set; }
        public static OpResult Add(TEntity entity, bool isSave = true)
        {
            var op = new OpResult();
            try
            {
                CurrentRepository.Add(entity, isSave);
                op.Successed = true;
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static OpResult AddRange(List<TEntity> entities, bool isSave = true)
        {
            var op = new OpResult();
            try
            {
                CurrentRepository.AddRange(entities, isSave);
                op.Successed = true;
            }
            catch (Exception ex)
            {
                var e = Pharos.Sys.LogEngine.ToInnerException(ex);
                op.Message = e.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static OpResult DeleteById(object id)
        {
            return Delete(CurrentRepository.FindById(id));
        }
        public static OpResult Delete(TEntity entity)
        {
            var op = new OpResult();
            try
            {
                if (entity == null) throw new ArgumentNullException("对象不存在");
                CurrentRepository.Remove(entity);
                op.Successed = true;
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static OpResult Delete(List<TEntity> list)
        {
            var op = new OpResult();
            try
            {
                if (!list.Any()) throw new ArgumentNullException("没有对象存在");
                CurrentRepository.RemoveRange(list);
                op.Successed = true;
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static OpResult Update(TEntity entity, bool isSave = true)
        {
            var op = new OpResult();
            try
            {
                CurrentRepository.Update(entity, isSave);
                op.Successed = true;
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static OpResult Update(List<TEntity> entities, bool isSave = true)
        {
            var op = new OpResult();
            try
            {
                CurrentRepository.Update(entities, isSave);
                op.Successed = true;
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                Log.WriteError(ex);
            }
            return op;
        }
        public static TEntity FindById(object id, params string[] includeParams)
        {
            return CurrentRepository.FindById(id, includeParams);
        }
        public static TEntity Find(Expression<Func<TEntity, bool>> whereLambda, params string[] includeParams)
        {
            return CurrentRepository.Find(whereLambda, includeParams);
        }
        public static List<TEntity> FindList(Expression<Func<TEntity, bool>> whereLambda, int takeNum = 0, params string[] includeParams)
        {
            return CurrentRepository.FindList(whereLambda, takeNum, includeParams).ToList();
        }
        public static bool IsExist(Expression<Func<TEntity, bool>> whereLambda)
        {
            return CurrentRepository.IsExist(whereLambda);
        }

        public static Sys.LogEngine Log
        {
            get { return new Sys.LogEngine(); }
        }
    }
}
