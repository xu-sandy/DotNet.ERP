﻿using Common.Logging;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos;
using Pharos.SyncService.SyncProviders;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pharos.SyncService
{
    public class SyncController
    {
        private string storeId;
        private int companyToken;
        private string endPointConfigurationName;

        private List<string> RunningStates = new List<string>();
        public SyncController()
        {
            storeId = System.Configuration.ConfigurationManager.AppSettings["StoreId"];
            try
            {
                companyToken = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CompanyId"]);
            }
            catch
            {
                companyToken = 0;
            }
            endPointConfigurationName = System.Configuration.ConfigurationManager.AppSettings["EndPointConfigurationName"];
        }

        //public void Close()
        //{
        //    RedisManager.UnSubscribe("SyncDatabase");
        //}
        public void Start()
        {
            try
            {
                if (string.IsNullOrEmpty(storeId) || companyToken == 0)
                {
                    Thread.CurrentThread.Abort();
                    return;
                }
                StoreManager.Subscribe("SyncDatabase", (channel, info) =>
                {
                    if (!string.IsNullOrEmpty(info))
                    {
                        LocalSyncContext localSyncContext = new LocalSyncContext();
                        var kv = localSyncContext.ServiceMappings.FirstOrDefault(o => o.Key.EndsWith(info));
                        DoSynchronize(kv.Key, kv.Value);
                    }
                    else
                    {
                        DoSynchronize();
                    }
                });
                var date = DateTime.Now.Date.AddDays(1).AddHours(2);
                lock (StdSchedulerFactory.SystemPropertyAsInstanceId)
                {
                    IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                    scheduler.Start();       //开启调度器
                    IJobDetail job1 = JobBuilder.Create<SyncJob>()  //创建一个作业
                       .WithIdentity("SyncDatabaseJob", "SyncDatabase")
                       .Build();
                    ITrigger trigger1 = TriggerBuilder.Create()
                                               .WithIdentity("SyncDatabase", "SyncDatabase")
                                              .StartNow()                     //启动时间
                                               .WithSimpleSchedule(x => x         //触发时间，5秒一次。
                                                   .WithIntervalInHours(5)
                                                   .RepeatForever()
                                                   )
                                               .Build();
                    scheduler.ScheduleJob(job1, trigger1);      //把作业，触发器加入调度器。
                }
                //Task.Factory.StartNew(() =>
                //{
                //    DoSynchronize();
                //});
            }
            catch (Exception ex)
            {
                ILog logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);
                logger.Error(ex.Message, ex);
                Console.WriteLine(ex.Message);
            }
        }
        public void DoSynchronize()
        {
            LocalSyncContext localSyncContext = new LocalSyncContext();
            foreach (var item in localSyncContext.ServiceMappings)
            {
                var task = Task.Factory.StartNew((o) =>
                {
                    var temp = (KeyValuePair<string, ISyncDataService>)o;
                    DoSynchronize(temp.Key, temp.Value);
                }, item);
            }
        }
        private Dictionary<string, object> lockObjects = new Dictionary<string, object>();
        private object lockobjforadd = new object();
        private void DoSynchronize(string key, ISyncDataService syncDataService)
        {
            lock (lockobjforadd)
            {
                if (!lockObjects.ContainsKey(key))
                {
                    lockObjects = lockObjects.ToList().Concat(new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>(key, new object()) }).ToDictionary(o => o.Key, o => o.Value);
                }
            }
            try
            {
                var dict = lockObjects;//防止并发
                var lockkv = dict.First(o => o.Key == key);
                object lockobj = new object();
                if (!lockkv.Equals(default(KeyValuePair<string, object>)) && lockkv.Value != null)
                {
                    lockobj = lockkv.Value;
                }
                lock (lockobj)
                {
                    SyncOrchestrator SyncOrchestrator = new Microsoft.Synchronization.SyncOrchestrator();
                    var temp = new KeyValuePair<string, ISyncDataService>(key, syncDataService);
                    var localProvider = new PosDbSyncProvider(companyToken, storeId, temp.Value);
                    localProvider.DestinationCallbacks.ProgressChanged += DestinationCallbacks_ProgressChanged;
                    localProvider.ReadRedisLog = true;
                    if (temp.Value.SyncDirectionOrder == SyncDirectionOrder.Download)
                    {
                        localProvider.Configuration.ConflictResolutionPolicy = ConflictResolutionPolicy.SourceWins;
                    }
                    else
                    {
                        localProvider.Configuration.ConflictResolutionPolicy = ConflictResolutionPolicy.ApplicationDefined;
                        localProvider.DestinationCallbacks.ItemConflicting += LocalItemConflicting;
                    }
                    var remoteProvider = new RemoteProviderProxy(companyToken, storeId, endPointConfigurationName, temp.Key);

                    SyncOrchestrator.LocalProvider = localProvider;
                    SyncOrchestrator.RemoteProvider = remoteProvider;
                    SyncOrchestrator.Direction = temp.Value.SyncDirectionOrder;
                    try
                    {
                        SyncOperationStatisticsShow(SyncOrchestrator.Synchronize(), temp.Value.SyncDirectionOrder, temp.Key);
                    }
                    catch (Exception ex)
                    {
                        ILog logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);
                        logger.Error(ex.Message, ex);
                        Console.WriteLine(ex.Message);
                        localProvider.WriteLog(ex.Message + "   正在准备重试，请稍后！");
                        //DO log
                    }
                }
            }
            catch (Exception ex)
            {
                ILog logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);
                logger.Error(ex.Message, ex);
                Console.WriteLine(ex.Message);
                //DO log
            }

        }
        private void DestinationCallbacks_ProgressChanged(object sender, SyncStagedProgressEventArgs e)
        {
            Console.WriteLine(e.CompletedWork + "/" + e.TotalWork);
        }

        private void LocalItemConflicting(object sender, ItemConflictingEventArgs e)
        {
            switch (e.DestinationChange.ChangeKind)
            {
                case ChangeKind.Deleted:
                    e.SetResolutionAction(ConflictResolutionAction.SourceWins);
                    break;
                case ChangeKind.UnknownItem:
                    e.SetResolutionAction(ConflictResolutionAction.SourceWins);
                    break;
                case ChangeKind.Update:
                    e.SetResolutionAction(ConflictResolutionAction.Merge);
                    break;
            }
        }
        public void SyncOperationStatisticsShow(SyncOperationStatistics syncOperationStatistics, SyncDirectionOrder _SyncDirectionOrder, string name)
        {
            Console.WriteLine(name);
            Console.WriteLine("{1} Download Applied:\t {0}", syncOperationStatistics.DownloadChangesApplied, _SyncDirectionOrder);
            Console.WriteLine("{1} Download Failed:\t {0}", syncOperationStatistics.DownloadChangesFailed, _SyncDirectionOrder);
            Console.WriteLine("{1} Download Total:\t\t {0}", syncOperationStatistics.DownloadChangesTotal, _SyncDirectionOrder);
            Console.WriteLine("{1} Upload Applied Total:\t\t {0}", syncOperationStatistics.UploadChangesApplied, _SyncDirectionOrder);
            Console.WriteLine("{1} Upload Failed Total:\t\t {0}", syncOperationStatistics.UploadChangesFailed, _SyncDirectionOrder);
            Console.WriteLine("{1} Upload Total:\t\t {0}", syncOperationStatistics.UploadChangesTotal, _SyncDirectionOrder);
        }

    }
}
