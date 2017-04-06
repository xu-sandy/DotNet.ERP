using Pharos.Logic.InstalmentDomain.Interfaces;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.InstalmentDomain
{
    public class IntalmentTimerTask : IIntalmentTimerTask
    {
        public void DoTask()
        {
            var identity = "IntalmentTimer";
            var date = DateTime.Now.Date.AddDays(1);
            lock (StdSchedulerFactory.SystemPropertyAsInstanceId)
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();       //开启调度器
                var tempTriggerKey = new TriggerKey(identity, identity);
                if (scheduler.CheckExists(tempTriggerKey))
                {
                    scheduler.UnscheduleJob(tempTriggerKey);
                }
                IJobDetail job1 = JobBuilder.Create<IntalmentTimerTaskJob>()  //创建一个作业
                   .WithIdentity(identity, identity)
                   .Build();
                ITrigger trigger1 = TriggerBuilder.Create()
                                           .WithIdentity(identity, identity)
                                           .StartAt(date)                        //启动时间每天零时
                                           .WithSimpleSchedule(x => x
                                               .WithIntervalInHours(24)
                                               .RepeatForever()
                                               )
                                           .Build();
                scheduler.ScheduleJob(job1, trigger1);      //把作业，触发器加入调度器。
            }
        }
    }
}
