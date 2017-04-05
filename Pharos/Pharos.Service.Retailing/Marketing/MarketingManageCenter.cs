﻿using Newtonsoft.Json;
using Pharos.Infrastructure.Data.Normalize;
using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.BLL;
using Pharos.Logic.Cache;
using Pharos.ObjectModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pharos.Service.Retailing.Marketing
{
    public class MarketingManageCenter
    {
        public static MarketingRuleCache marketingRuleCache = new MarketingRuleCache();
        public static ActiveMarketingRuleCache activeMarketingRuleCache = new ActiveMarketingRuleCache();
        private static object lockObj = new object();
        static List<MarketingRefreshInfo> hostedDict = new List<MarketingRefreshInfo>();
        private static IEnumerable<KeyValuePair<MarketingTimelinessLimit, MarketingRule>> Load(int companyId, string storeId)
        {
            MarketingService marketingService = new MarketingService(storeId, companyId);
            return marketingService.GetMarketingRules();
        }
        /// <summary>
        /// 订阅其他系统通知
        /// </summary>
        private static void Subscribe()
        {
            RedisManager.Subscribe("SyncDatabase", (channel, info) =>
            {
                var databaseChanged = JsonConvert.DeserializeObject<DatabaseChanged>(info);
                switch (databaseChanged.Target)
                {
                    case "CommodityBundlingPackage":
                    case "CommodityDiscountPackage":
                    case "CommodityFreeGiftPackage":
                    case "CommodityBlendPackage":
                        //  Console.WriteLine("SyncDatabaseMessage");

                        var storeIds = databaseChanged.StoreId.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
                        foreach (var storeId in storeIds)
                        {
                            RedisManager.Publish("MarketingRefresh", new MarketingRefreshInfo() { StoreId = storeId, CompanyId = databaseChanged.CompanyId });
                            //Console.WriteLine("SyncDatabase-MarketingRefresh");
                        }
                        break;
                }
            });
            RedisManager.Subscribe("MarketingRefresh", (chanel, msg) =>
            {
                Task.Factory.StartNew((obj) =>
                {
                    lock (lockObj)
                    {
                        try
                        {
                            var info = JsonConvert.DeserializeObject<MarketingRefreshInfo>(obj.ToString());
                            var key = KeyFactory.StoreKeyFactory(info.CompanyId, info.StoreId);
                            var rules = Load(info.CompanyId, info.StoreId);
                            marketingRuleCache.Set(key, rules);
                            ToRefreshHosted(info.CompanyId, info.StoreId);
                        }
                        catch { }
                    }
                }, msg);
            });
            RedisManager.Subscribe("RefreshHostedMarketing", (channel, msg) =>
            {
                Task.Factory.StartNew((obj) =>
                {
                    var info = JsonConvert.DeserializeObject<MarketingRefreshInfo>(obj.ToString());
                    var key = KeyFactory.StoreKeyFactory(info.CompanyId, info.StoreId);
                    ToRefreshHosted(info.CompanyId, info.StoreId);
                }, msg);
            });
        }
        /// <summary>
        /// 刷新寄宿
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="storeId"></param>
        private static void ToRefreshHosted(int companyId, string storeId)
        {
            var ts = RefreshHosted(companyId, storeId);
            if (ts == default(TimeSpan)) return;
            var info = new MarketingRefreshInfo() { StoreId = storeId, CompanyId = companyId, Timer = DateTime.Now.Add(ts) };
            lock (hostedDict)
            {
                hostedDict.Add(info);
            }
        }
        /// <summary>
        /// 刷新寄宿规则处理
        /// </summary>
        private static void ToRefreshHostedProcess()
        {
            IEnumerable<MarketingRefreshInfo> items = null;
            lock (hostedDict)
            {
                items = hostedDict.Where(o => (o.Timer - DateTime.Now) <= new TimeSpan(1)).ToList();
            }
            foreach (var item in items)
            {
                ToRefreshHosted(item.CompanyId, item.StoreId);
                lock (hostedDict)
                {
                    hostedDict.Remove(item);
                }
            }

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(new TimeSpan(0, 1, 0));
                ToRefreshHostedProcess();
            });
        }

        public static void InitStore()
        {
            var stores = WarehouseService.GetStores();
            foreach (var item in stores)
            {
                var info = new MarketingRefreshInfo() { StoreId = item.StoreId, CompanyId = item.CompanyId };
                RedisManager.Publish("MarketingRefresh", info);
            }
            Task.Factory.StartNew(() =>//定时更新加载每天数据
            {
                try
                {
                    Thread.Sleep(DateTime.Now.Date.AddDays(1).AddHours(1) - DateTime.Now);
                    InitStore();
                }
                catch { }
            });
        }
        public static void InitStoreMarketing()
        {
            Subscribe();
            Task.Factory.StartNew(() => { ToRefreshHostedProcess(); });
            Task.Factory.StartNew(() => { InitStore(); });
        }

        /// <summary>
        /// 刷新促销规则托管
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <param name="storeId">门店id</param>
        /// <returns>返回下次刷新托管规则的时间间隔</returns>
        private static TimeSpan RefreshHosted(int companyId, string storeId)
        {
            var key = KeyFactory.StoreKeyFactory(companyId, storeId);

            IEnumerable<KeyValuePair<MarketingTimelinessLimit, MarketingRule>> cacheRules = null;

            if (marketingRuleCache.ContainsKey(key))
                cacheRules = marketingRuleCache.Get(key);
            List<MarketingRule> rules = new List<MarketingRule>();
            if (cacheRules == null && cacheRules.Count() == 0)
                return default(TimeSpan);

            var timelinessTimes = new List<DateTime>();

            foreach (var item in cacheRules)
            {
                var now = DateTime.Now;
                item.Value.Enable = true;
                if (item.Key.StartTime <= now && item.Key.OverTime >= now)
                {
                    var ranges = item.Key.TimeRanges;
                    if (ranges == null || ranges.Count() == 0)//无时效约束
                    {
                        rules.Add(item.Value);//启动规则
                        timelinessTimes.Add(item.Key.OverTime);//添加过期时间
                    }
                    else//有时效约束
                    {
                        foreach (var range in ranges)
                        {
                            var start = DateTime.Parse(range.Key);
                            var end = DateTime.Parse(range.Value);
                            if (start <= now && end >= now)
                            {
                                timelinessTimes.Add(end);//添加时效约束
                                rules.Add(item.Value);//启动规则
                                timelinessTimes.Add(item.Key.OverTime);

                            }
                            else if (start >= now)
                            {
                                timelinessTimes.Add(start);//添加启动规则时间
                            }
                        }
                    }
                }
                else if (item.Key.StartTime > now)
                {
                    timelinessTimes.Add(item.Key.StartTime);//添加启动规则时间
                }
            }
            var repeatRanges = rules.Where(o => o.Type == MarketingType.Danpinzhekou).GroupBy(o => o.BarcodeRange.FirstOrDefault());//处理单品折扣失效覆盖
            foreach (var item in repeatRanges)
            {
                if (item.Count() > 1)
                {
                    var maxDate = item.Max(o => o.CreateRuleDate);
                    foreach (var rule in item)
                    {
                        if (rule.CreateRuleDate != maxDate)
                        {
                            rule.Enable = false;
                        }
                        else
                        {
                            var cache = cacheRules.FirstOrDefault(o => o.Value == rule);
                            timelinessTimes.Add(cache.Key.OverTime);
                        }
                    }
                }
            }
            activeMarketingRuleCache.Set(key, rules);
            if (timelinessTimes.Count > 0)
            {
                var dates = timelinessTimes.OrderBy(o => o);
                var date = dates.FirstOrDefault();
                return date - DateTime.Now;
            }
            else
            {
                return default(TimeSpan);
            }
        }
    }

    public class MarketingRefreshInfo
    {
        public string StoreId { get; set; }

        public int CompanyId { get; set; }

        public DateTime Timer { get; set; }
    }
}
