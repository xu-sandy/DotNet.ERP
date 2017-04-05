using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System.Linq.Expressions;

namespace Pharos.Logic.OMS.BLL
{
    public abstract class BaseService<T> where T : class
    {
        [Ninject.Inject]
        protected LogEngine LogEngine { get; set; }

        IBaseRepository<T> baseRepository 
        {
            get 
            {
                BaseRepository<T> br = new BaseRepository<T>();
                return br;
            }
        }

        /// <summary>
        /// 根据ID获取一条记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T GetEntityById(int Id)
        {
            return baseRepository.Get(Id);
        }

        /// <summary>
        /// 根据条件获取一条记录
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T GetEntityByWhere(Expression<Func<T, bool>> whereLambda)
        {
            if (whereLambda != null)
            {
               return baseRepository.GetQuery(whereLambda).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据条件获取列表
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> GetListByWhere(Expression<Func<T, bool>> whereLambda = null)
        {
            return baseRepository.GetQuery(whereLambda).ToList();
        }

        /// <summary>
        /// 获取最大编号
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="startInt">编号最小值</param>
        /// <param name="endInt">编号最大值</param>
        /// <returns></returns>
        public int GetMaxInt(Expression<Func<T, int?>> whereLambda, int minInt = 0, int maxInt = int.MaxValue)
        {
            int i = 0;
            if (whereLambda != null)
            {
                i = baseRepository.GetQuery().Max(whereLambda).GetValueOrDefault();
                if (i < minInt)
                {
                    i = minInt;
                }
                else
                {
                    i = i + 1;
                    if (i >= maxInt)
                    {
                        i = -1;
                    }
                }
            }
            else
            {
                i = -1;
            }
            return i;
        }

        /// <summary>
        /// 插入一条记录或者更新一条记录
        /// </summary>
        public T InsertUpdate(T model, int Id, bool isSave=true)
        {
            if (Id > 0)
            {
                return baseRepository.Update(model, isSave);
            }
            else
            {
                return baseRepository.Insert(model,isSave);
            }
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public OpResult DeleteByWhere(Expression<Func<T, bool>> whereLambda,bool isSave=true)
        {
            var list = baseRepository.GetQuery(whereLambda);
            baseRepository.RemoveRange(list.ToList(),isSave);
            return OpResult.Success();
        }

        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public bool isExistByWhere(Expression<Func<T, bool>> whereLambda)
        {
            var isExist = baseRepository.GetQuery(whereLambda);
            if (isExist.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据条件更新多条记录
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public OpResult UpListByWhere(Expression<Func<T, bool>> whereLambda, Action<T> handler,bool isSave=true)
        {
            var list = baseRepository.GetQuery(whereLambda).ToList();
            list.Each(handler);
            if (isSave)
            {
                if (SaveChanges())
                {
                    return OpResult.Success();
                }
                else
                {
                    return OpResult.Fail();
                }
            }
            else
            {
                return OpResult.Success();
            }
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool dels(List<T> list,bool isSave=true)
        {
            return baseRepository.RemoveRange(list, isSave);
        }

        public bool SaveChanges()
        {
            return baseRepository.SaveChanges();
        }
    }
}
