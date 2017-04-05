﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Quartz;
using Quartz.Impl;

namespace Pharos.Service.Retailing.RechargeGiftsCache
{
    /// <summary>
    /// 充值赠送规则管理器
    /// </summary>
    public class RechargeGiftsManager
    {
        private readonly static RechargeGiftsService rechargeService = new RechargeGiftsService();

        /// <summary>
        /// 创建作业调度
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="companyid"></param>
        private static void CreateSchedulerRechargeGifts(TimeSpan ts, int companyid)
        {
            if (ts == default(TimeSpan)) return;
            lock (StdSchedulerFactory.SystemPropertyAsInstanceId)
            {
                //从工厂中获取一个调度器实例
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();
                var tempTriggerKey = new TriggerKey("RechargeGiftsJob" + companyid, "Marketing");
                if (scheduler.CheckExists(tempTriggerKey))
                {
                    scheduler.UnscheduleJob(tempTriggerKey);
                }
                IJobDetail job = JobBuilder.Create<RechargeGiftsJob>().WithIdentity("RechargeGiftsJob" + companyid, "RechargeGifts").Build();
                job.JobDataMap.Put("rechargeGiftsJob", companyid);

                ITrigger trigger1 = TriggerBuilder.Create()
                                               .WithIdentity("RechargeGiftsJob" + companyid, "RechargeGifts")
                                               .StartAt(DateTime.Now.Add(ts))                        //现在开始
                                               .Build();
                scheduler.ScheduleJob(job, trigger1);      //把作业，触发器加入调度器。
            }
        }
        /// <summary>
        /// 每天执行
        /// </summary>
        /// <param name="info"></param>
        private static void CreateSchedulerRechargeGifts(int companyId)
        {
            var date = DateTime.Now.Date.AddDays(1).AddHours(2);
            lock (StdSchedulerFactory.SystemPropertyAsInstanceId)
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();       //开启调度器
                var tempTriggerKey = new TriggerKey("RechargeGiftsJob" + companyId, "Marketing");
                if (scheduler.CheckExists(tempTriggerKey))
                {
                    scheduler.UnscheduleJob(tempTriggerKey);
                }
                IJobDetail job1 = JobBuilder.Create<RechargeGiftsJob>()  //创建一个作业
                   .WithIdentity("RechargeGiftsJob" + companyId, "RechargeGifts")
                   .Build();
                job1.JobDataMap.Put("rechargeGiftsJob", companyId);
                ITrigger trigger1 = TriggerBuilder.Create()
                                           .WithIdentity("RechargeGiftsJob" + companyId, "RechargeGifts")
                                           .StartAt(date)                        //启动时间
                                           .WithSimpleSchedule(x => x         //触发时间，5秒一次。
                                               .WithIntervalInHours(24)
                                               .RepeatForever()
                                               )
                                           .Build();
                scheduler.ScheduleJob(job1, trigger1);      //把作业，触发器加入调度器。
            }
        }

        /// <summary>
        /// 作业开始
        /// </summary>
        public static void Start()
        {
            //获取所有公司信息
            var companys = new int[] { 1, 2, 3, 4 };//TODO:
            foreach (var item in companys)
            {
                RefreshCompanyRechargeGifts(item);
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="companyid"></param>
        public static void RefreshCompanyRechargeGifts(int companyid)
        {
            var ts = RefreshRechargeGifts(companyid);
            CreateSchedulerRechargeGifts(ts, companyid);
            CreateSchedulerRechargeGifts(companyid);
        }

        /// <summary>
        /// 获取会员充值赠送作业启动间隔
        /// </summary>
        /// <returns></returns>
        public static TimeSpan RefreshRechargeGifts(int companyid)
        {
            var spanTimes = new List<DateTime>();//用于作业调度
            List<RechargeGifts> activingRechargeRules = new List<RechargeGifts>();
            //获取所有的未开始、进行中的规则
            var queryResult = rechargeService.GetInActiveRechargeGiftRules(companyid);
            if (queryResult == null || queryResult.Count() == 0)
            {
                return default(TimeSpan);
            }
            foreach (var item in queryResult)
            {
                var beginDate = DateTime.Parse(item.ExpiryStart);
                var endDate = default(DateTime);
                DateTime.TryParse(item.ExpiryEnd, out endDate);
                //更新状态
                if (endDate != default(DateTime))
                {
                    if (beginDate > DateTime.Now)
                    {//活动未开始
                        //添加活动开始时间
                        spanTimes.Add(beginDate);
                    }
                    else if (beginDate < DateTime.Now && endDate > DateTime.Now)
                    {//当前在活动时间
                        if (item.State == 0)
                        { //更新为活动中状态
                            rechargeService.UpdateRechargeGiftStateNoReturn(1, item.Id.ToString());
                            item.State = 1;
                        }
                        //添加缓存数据
                        activingRechargeRules.Add(item);
                        //添加过期时间

                        if (endDate != default(DateTime))
                            spanTimes.Add(endDate);
                    }
                    else if (endDate < DateTime.Now)
                    {//活动过期 
                        rechargeService.UpdateRechargeGiftStateNoReturn(2, item.Id.ToString());
                    }
                }
                else
                {
                    //判断开始时间
                    if (beginDate > DateTime.Now)
                    {//活动开始
                        if (item.State == 0)
                        {
                            rechargeService.UpdateRechargeGiftStateNoReturn(1, item.Id.ToString());
                            item.State = 1;
                            activingRechargeRules.Add(item);
                        }
                    }
                    else
                    {//活动未开始 
                        spanTimes.Add(beginDate);//添加活动开始时间
                    }
                }

            }
            //数据加入缓存

            //返回作业启动时间间隔
            if (spanTimes.Count > 0)
            {
                return spanTimes.OrderBy(o => o).First() - DateTime.Now;
            }
            else
            {
                return default(TimeSpan);
            }
        }
    }
    /// <summary>
    /// 声明要执行的作业
    /// </summary>
    public class RechargeGiftsJob : IJob
    {
        /// <summary>
        /// context 可以获取当前 job 的各种状态
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            int content = dataMap.GetInt("rechargeGiftsJob");
            RechargeGiftsManager.RefreshRechargeGifts(content);
        }
    }
}
