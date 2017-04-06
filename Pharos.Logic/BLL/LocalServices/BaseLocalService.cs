using Pharos.Logic.DAL.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Pharos.Logic.LocalEntity;
using System.Threading;
using Pharos.Utility;
using System.IO;
using Pharos.Logic.DAL;

namespace Pharos.Logic.BLL.LocalServices
{
    public class BaseLocalService<TEntity> : BaseGeneralService<TEntity, SqliteDbContext> where TEntity : Pharos.Logic.LocalEntity.BaseEntity
    {
        public static OpResult Add(TEntity entity, bool isSave = true)
        {
            var op = new OpResult();
            SqliteTrap.PushAction<TEntity>((o1) =>
            {

                try
                {
                    using (var context = new SqliteDbContext())
                    {
                        try
                        {
                            context.Set<TEntity>().Add(o1);
                            context.SaveChanges();
                        }
                        catch { }
                        finally
                        {
                            //One of the following two is enough
                            context.Database.Connection.Close();
                            context.Database.Connection.Dispose(); //THIS OR
                            ContextFactory.GetCurrentContext<SqliteDbContext>(true);

                        }
                    }

                }
                catch (Exception ex)
                {
                    File.AppendAllText("Log.txt", ex.Message + "|" + ex.StackTrace, Encoding.UTF8);
                    //  Log.WriteError(ex);
                }
            }, entity);
            op.Successed = true;

            return op;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSave">isSave 无效</param>
        /// <returns></returns>
        public static OpResult AddRange(List<TEntity> entities, bool isSave = true)
        {
            var op = new OpResult();
            SqliteTrap.PushAction<TEntity>(new Action<IEnumerable<TEntity>>((o1) =>
            {
                try
                {
                    using (var context = new SqliteDbContext())
                    {
                        try
                        {
                            context.Set<TEntity>().AddRange(o1.ToList());
                            context.SaveChanges();
                        }
                        catch { }
                        finally
                        {
                            //One of the following two is enough
                            context.Database.Connection.Close();
                            context.Database.Connection.Dispose(); //THIS OR
                            ContextFactory.GetCurrentContext<SqliteDbContext>(true);

                        }
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText("Log.txt", ex.Message + "|" + ex.StackTrace, Encoding.UTF8);

                    //  Log.WriteError(ex);
                }
            }), entities);
            op.Successed = true;
            return op;
        }
        public static OpResult DeleteById(object id)
        {
            return Delete(CurrentRepository.FindById(id));
        }
        public static OpResult Delete(TEntity entity)
        {
            var op = new OpResult();
            SqliteTrap.PushAction<TEntity>((o1) =>
            {
                try
                {
                    try
                    {
                        if (entity == null) return;
                        var id = o1.GetType().GetProperty("Id").GetValue(o1, null);
                        var entity1 = CurrentRepository.FindById(id);
                        entity1 = entity1.InitEntity<TEntity, TEntity>(o1);
                        CurrentRepository.Remove(entity1);
                    }
                    catch { }
                    finally
                    {
                        //One of the following two is enough
                        CurrentRepository._context.Database.Connection.Close();
                        CurrentRepository._context.Database.Connection.Dispose(); //THIS OR
                        ContextFactory.GetCurrentContext<SqliteDbContext>(true);

                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText("Log.txt", ex.Message + "|" + ex.StackTrace, Encoding.UTF8);

                    // Log.WriteError(ex);
                }
            }, entity);
            op.Successed = true;





            return op;
        }

        public static OpResult Update(TEntity entity, bool isSave = true)
        {
            var op = new OpResult();
            SqliteTrap.PushAction<TEntity>((o1) =>
            {
                try
                {
                    try
                    {
                        var id = o1.GetType().GetProperty("Id").GetValue(o1, null);
                        var entity1 = CurrentRepository.FindById(id);
                        entity1 = entity1.InitEntity<TEntity, TEntity>(o1);
                        var result = CurrentRepository.Update(entity1);
                    }
                    catch { }
                    finally
                    {
                        //One of the following two is enough
                        CurrentRepository._context.Database.Connection.Close();
                        CurrentRepository._context.Database.Connection.Dispose(); //THIS OR
                        ContextFactory.GetCurrentContext<SqliteDbContext>(true);
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText("Log.txt", ex.Message + "|" + ex.StackTrace, Encoding.UTF8);

                    //  Log.WriteError(ex);
                }
            }, entity);
            op.Successed = true;
            return op;
        }

    }

}
