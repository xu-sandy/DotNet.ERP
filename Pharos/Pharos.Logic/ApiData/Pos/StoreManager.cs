﻿using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.Sale.Marketings;
using Pharos.Logic.ApiData.Pos.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.ApiData.Pos.Extensions;
using System.Web;
using System.Threading.Tasks;
using System.Threading;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Newtonsoft.Json;
using Pharos.Logic.ApiData.Pos.Cache;
using Pharos.Infrastructure.Data.Normalize;

namespace Pharos.Logic.ApiData.Pos
{
    public static class StoreManager
    {
        public static void Init()
        {
            SwiftNumber.SwiftNumberChanged += (sender, args) =>
            {
                var swiftNumber = sender as SwiftNumber;
                if (sender is PaySn)
                {
                    var paysn = (PaySn)sender;
#if(Local!=true)
                    Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncSerialNumber", new PaySnDto() { CompanyId = paysn.CompanyId, MachineSn = paysn.MachineSn, StoreId = paysn.StoreId, Name = paysn.Name, Number = paysn.GetNumber(), SwiftNumberMode = paysn.SwiftNumberMode });
#endif
#if(Local)
                    StoreManager.PubEvent("SyncSerialNumber", new PaySnDto() { CompanyId = paysn.CompanyId, MachineSn = paysn.MachineSn, StoreId = paysn.StoreId, Name = paysn.Name, Number = paysn.GetNumber(), SwiftNumberMode = paysn.SwiftNumberMode });
#endif
                }
                if (sender is MemberNo)
                {
                    var memberNo = (MemberNo)sender;
#if(Local!=true)
                    Pharos.Infrastructure.Data.Redis.RedisManager.Publish("MemberNo", new MemberNoDto() { CompanyId = memberNo.CompanyId, StoreId = memberNo.StoreId, Name = memberNo.Name, Number = memberNo.GetNumber(), SwiftNumberMode = memberNo.SwiftNumberMode });
#endif
#if(Local)
                    StoreManager.PubEvent("MemberNo", new MemberNoDto() {  CompanyId = memberNo.CompanyId, StoreId = memberNo.StoreId, Name = memberNo.Name, Number = memberNo.GetNumber(), SwiftNumberMode = memberNo.SwiftNumberMode });
#endif
                }

            };

        }


        public static bool IsRunning = false;
        private static readonly string KeyFormat = "{0}*-*{1}";

        public static EventHandler<SubPubEventArgs> LocalSubPub;
        private static List<SubItem> subs = new List<SubItem>();

        private static SyncLogCache syncLogCache = new SyncLogCache();

        public static void SetSyncLog(string key, string content)
        {
            if (syncLogCache == null)
                syncLogCache = new SyncLogCache();
            syncLogCache.Set(key, content);
        }

        public static IEnumerable<string> GetSyncLogs()
        {
            if (syncLogCache == null)
                syncLogCache = new SyncLogCache();
            return syncLogCache.Collections.Values.ToArray();
        }
        public static void PubEvent(string title, string info)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<SubPubEventArgs> temp = LocalSubPub;
            if (temp != null)
                temp(title, new SubPubEventArgs() { Content = info, Title = title });
        }
        public static void Subscribe(string title, Action<string, string> handler)
        {
            if (subs == null)
                subs = new List<SubItem>();
            subs.Add(new SubItem() { Title = title, CallBack = handler });
            if (LocalSubPub == null)
            {
                LocalSubPub += (o, args) =>
                 {
                     var key = o.ToString();
                     var temps = subs.ToList();
                     foreach (var item in temps)
                     {
                         try
                         {
                             if (key == item.Title)
                                 item.CallBack(args.Title, args.Content);
                         }
                         catch { }
                     }
                 };
            }
        }

        private static readonly IDictionary<string, MarketingManager> MarketingCaches = new Dictionary<string, MarketingManager>();

        public static string GetKey(int companyId, string storeId)
        {
            var key = string.Format(KeyFormat, companyId, storeId);
            return key;
        }

#if (Local !=true)
        public static void InitStores(bool isRestart = false)
        {
            if (!isRestart && IsRunning)
            {
                return;
            }
            IsRunning = true;
            var stores = DataAdapterFactory.DefualtDataAdapter.GetWarehouseInformations();
            foreach (var item in stores)
            {
                SetUpStore(item.CompanyId, item.StoreId);
            }
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(new TimeSpan(24, 0, 0));
                InitStores(true);
            });
        }
#endif
        public static void SetUpStore(int companyId, string storeId)
        {
            var key = GetKey(companyId, storeId);
            MarketingManager marketingManager = new MarketingManager(storeId, companyId);
            if (MarketingCaches.ContainsKey(key))
            {
                MarketingCaches.Remove(key);
            }
            MarketingCaches.Add(key, marketingManager);
        }

        public static MarketingManager GetMarketing(int companyId, string storeId)
        {
            var key = GetKey(companyId, storeId);
            if (MarketingCaches.ContainsKey(key))
                return MarketingCaches[key];
            else
                throw new PosException("门店尚未初始化！");
        }

        public static void PubEvent(string title, object content)
        {
            PubEvent(title, JsonConvert.SerializeObject(content));
        }
    }
    public class SubPubEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class SubItem
    {
        public string Title { get; set; }
        public Action<string, string> CallBack { get; set; }
    }
}
