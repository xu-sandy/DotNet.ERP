using Pharos.Logic.DAL;
using Pharos.Logic.DAL.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;

namespace Pharos.Logic.BLL.LocalServices
{
    public class SqliteTrap
    {
        static bool running = false;
        static bool actionExecuted = false;
        static Queue<SqliteTrapParams> actions = new Queue<SqliteTrapParams>();
        static SqliteTrap()
        {
            running = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(Running));
        }
        public static bool EnableClose { get { return actions.Count == 0 && actionExecuted; } }
        private static void Running(object obj)
        {
            while (running)
            {
                if (actions.Count > 0)
                {
                    actionExecuted = false;
                    var _params = actions.Dequeue();
                    if (_params != null)
                    {
                        try
                        {
                            _params.Action(_params.Datas);
                            if (_params.Callback != null)
                            {
                                _params.Callback();
                            }
                        }
                        catch { }
                    }
                }
                actionExecuted = true;

                Thread.Sleep(10);
            }
        }

        public static void PushAction<T>(Action<T> action, T entity)
        where T : class
        {
            actions.Enqueue(new SqliteTrapParams() { Action = action, Datas = entity });
        }

        public static void PushAction<T>(Action<IEnumerable<T>> action, IEnumerable<T> datas)
        where T : class
        {
            actions.Enqueue(new SqliteTrapParams() { Action = action, Datas = datas });
        }

        public static void PushAction<T>(Action<IEnumerable<T>> action, Action callback, IEnumerable<T> datas)
        where T : class
        {
            actions.Enqueue(new SqliteTrapParams() { Action = action, Datas = datas, Callback = callback });
        }

        public static void PushAction<T>(Action<T> action, Action callback, T entity)
        where T : class
        {
            actions.Enqueue(new SqliteTrapParams() { Action = action, Datas = entity, Callback = callback });
        }
    }
    public class SqliteTrapParams
    {
        public dynamic Datas { get; set; }


        public dynamic Action { get; set; }

        public Action Callback { get; set; }
    }
}
