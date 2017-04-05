﻿using System.Data.Entity;
using System.Runtime.Remoting.Messaging;

namespace Pharos.Logic.OMS.DAL
{
    /// <summary>
    /// 上下文简单工厂
    /// <remarks>
    /// 创建：2014.02.05
    /// </remarks>
    /// </summary>
    class ContextFactory
    {
        //关于DbContext的单例问题，看了一些文章讲DbContex是轻量级的，创建的开销不大，另一个DbContext并不能保证线程安全。
        //对于DbContext单例化、静态化很多人反对，但每个操作都进行创建和销毁也不合理，实现一个请求内单例还是比较合适。
        //MSDN中讲CallContext提供对每个逻辑执行线程都唯一的数据槽，而在WEB程序里，每一个请求就是一个逻辑线程
        //所以使用CallContext来实现单个请求之内的DbContext单例是合理的。

        /// <summary>
        /// 获取当前数据上下文
        /// </summary>
        /// <returns></returns>
        //public static EFDbContext GetCurrentContext()
        //{
        //    EFDbContext _dbContext = CallContext.GetData("EFDbContext") as EFDbContext;
        //    if (_dbContext == null)
        //    {
        //        _dbContext = new EFDbContext();
        //        CallContext.SetData("EFDbContext", _dbContext);
        //    }
        //    return _dbContext;
        //}
        /// <summary>
        /// 获取数据上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据上下文类型</typeparam>
        /// <param name="isIsForcedExpired">是否强制数据上下文缓存过期</param>
        /// <returns>数据上下文实例</returns>
        public static TDbContext GetCurrentContext<TDbContext>(bool isIsForcedExpired = false)
            where TDbContext : DbContext, new()
        {
            var key = typeof(TDbContext).ToString();
            TDbContext _dbContext = CallContext.GetData(key) as TDbContext;
            if (isIsForcedExpired) 
            {
                _dbContext = null;
            }
            if (_dbContext == null)
            {
                _dbContext = new TDbContext();
                CallContext.SetData(key, _dbContext);
            }
            return _dbContext;
        }
    }
}
