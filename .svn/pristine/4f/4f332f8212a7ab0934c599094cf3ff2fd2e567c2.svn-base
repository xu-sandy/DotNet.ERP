using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.BLL;
using Quartz;
using Quartz.Impl;

namespace Pharos.Service.Retailing.RechargeGiftsCache
{
    /// <summary>
    /// 充值分期赠送管理器
    /// </summary>
    public class RechargeGiftsStageManager
    {
        /// <summary>
        /// 启动
        /// </summary>
        public static void Start()
        {
            var companyids = new int[] { 1, 103, 104 };
            foreach (var item in companyids)
            {
                RefreshRechargeGiftsStage(item);
            }
        }
        /// <summary>
        /// 充值赠送刷新
        /// </summary>
        /// <param name="companyId"></param>
        public static void RefreshRechargeGiftsStage(int companyId)
        {
            var ts = RefreshStageInfo(companyId);
            CreateSchedulerRechargeGiftsStage(ts, companyId);
            CreateSchedulerRechargeGiftsStage(companyId);//每天固定执行
        }
        /// <summary>
        /// 获取所有分期数据 刷新
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static TimeSpan RefreshStageInfo(int companyId)
        {
            var times = new List<DateTime>();
            //所有未赠送数据
            var giftStages = RechargeGiftsStageService.GetNoRechargeStageByCompanyId(companyId);
            if (giftStages == null || giftStages.Count == 0)
            {
                return default(TimeSpan);
            }
            //遍历数据
            foreach (var item in giftStages)
            {
                if (item.State == 0)
                { //未赠送
                    var startDate = DateTime.Parse(item.GiftDate);
                    if (startDate.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        //赠送该条记录
                        RechargeGiftsStageService.UpdateRechargeStageStage(item.Id, 1);
                    }
                    else if (startDate > DateTime.Now)
                    {//赠送时间没有到
                        times.Add(startDate);
                    }

                }
            }
            if (times.Count > 0)
            {
                return times.OrderBy(o => o).First() - DateTime.Now;
            }
            else
            {
                return default(TimeSpan);
            }
        }
        /// <summary>
        /// 创建作业调度
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="companyid"></param>
        private static void CreateSchedulerRechargeGiftsStage(TimeSpan ts, int companyid)
        {
            if (ts == default(TimeSpan) && ts < new TimeSpan(1)) return;

            lock (StdSchedulerFactory.SystemPropertyAsInstanceId)
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();       //开启调度器
                var tempTriggerKey = new TriggerKey("TriggerGiftsStageJob" + companyid, "GiftsStageJob");
                if (scheduler.CheckExists(tempTriggerKey))
                {
                    scheduler.UnscheduleJob(tempTriggerKey);
                }
                IJobDetail job1 = JobBuilder.Create<RechargeGiftsStageJob>()  //创建一个作业
                   .WithIdentity("JobDetailGiftsStageJob" + companyid, "GiftsStageJob")
                   .Build();
                job1.JobDataMap.Put("rechargeGiftsStageJob", companyid);
                ITrigger trigger1 = TriggerBuilder.Create()
                                           .WithIdentity("TriggerGiftsStageJob" + companyid, "GiftsStageJob")
                                           .StartAt(DateTime.Now.Add(ts))                        //现在开始
                                           .Build();
                scheduler.ScheduleJob(job1, trigger1);      //把作业，触发器加入调度器。
            }
        }
        /// <summary>
        /// 每天执行
        /// </summary>
        /// <param name="info"></param>
        private static void CreateSchedulerRechargeGiftsStage(int companyid)
        {
            var date = DateTime.Now.Date.AddDays(1).AddHours(2);
            lock (StdSchedulerFactory.SystemPropertyAsInstanceId)
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();       //开启调度器
                var tempTriggerKey = new TriggerKey("TriggerGiftsStageJob" + companyid, "GiftsStageJob");
                if (scheduler.CheckExists(tempTriggerKey))
                {
                    scheduler.UnscheduleJob(tempTriggerKey);
                }
                IJobDetail job1 = JobBuilder.Create<RechargeGiftsJob>()  //创建一个作业
                   .WithIdentity("JobDetailGiftsStageJob" + Guid.NewGuid().ToString(), "GiftsStageJob")
                   .Build();
                job1.JobDataMap.Put("rechargeGiftsStageJob", companyid);
                ITrigger trigger1 = TriggerBuilder.Create()
                                           .WithIdentity("TriggerGiftsStageJob" + companyid, "GiftsStageJob")
                                           .StartAt(date)                        //启动时间
                                           .WithSimpleSchedule(x => x         //触发时间，5秒一次。
                                               .WithIntervalInHours(24)
                                               .RepeatForever()
                                               )
                                           .Build();
                scheduler.ScheduleJob(job1, trigger1);      //把作业，触发器加入调度器。
            }
        }

    }
    /// <summary>
    /// 声明作业
    /// </summary>
    public class RechargeGiftsStageJob : IJob
    {
        /// <summary>
        /// context 可以获取当前 job 的各种状态
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            int content = dataMap.GetInt("rechargeGiftsStageJob");
            RechargeGiftsStageManager.RefreshStageInfo(content);
        }
    }
}
