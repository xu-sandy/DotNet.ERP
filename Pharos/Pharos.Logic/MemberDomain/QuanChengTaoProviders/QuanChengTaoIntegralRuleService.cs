﻿using Pharos.Logic.MemberDomain.QuanChengTaoProviders.Extensions;
using System.Collections.Generic;
using System.Linq;
using Pharos.Logic.MemberDomain.Interfaces;
using Pharos.Logic.MemberDomain.Extensions;
using System;
using System.Reflection;
using Pharos.Logic.InstalmentDomain;
using Pharos.ObjectModels.Events;
using Pharos.Logic.Entity;
using Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment;
using Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment.InstalmentParameters;
using Pharos.Logic.InstalmentDomain.QuanChengTaoInstalment.InstalmentItems;
using Pharos.Logic.BLL;

namespace Pharos.Logic.MemberDomain.QuanChengTaoProviders
{
    public class QuanChengTaoIntegralRuleService
    {
        public static IDictionary<int, string> GetRuleProvidersIdAndTitle()
        {
            return EnumExtensions.GetIntegralProviderTypeTitleAndValue();
        }
        public static IDictionary<int, string> GetMeteringModesTitleAndValue(int integralProviderType)
        {
            return EnumExtensions.GetMeteringModeDescriptionAttributeTitleAndValue((IntegralProviderType)integralProviderType);
        }
        internal static void SubscribeIntegralEvents()
        {
            var eventHandlerAssemblies = new List<Assembly>();
            List<IEventHandler> eventHandlers = new List<IEventHandler>();
            if (!eventHandlerAssemblies.Any())
            {
                eventHandlerAssemblies.Add(typeof(QuanChengTaoIntegralRuleService).Assembly);
            }
            foreach (var assembly in eventHandlerAssemblies)
            {
                try
                {
                    eventHandlers.AddRange(assembly.GetImplementedObjectsByInterface<IEventHandler>());
                }
                catch (Exception exc)
                {
                    //throw new IntegralRuleProviderLoadException(string.Format("加载积分规则提供程序集失败，程序集： {0}!", assembly.FullName), exc);
                }
            }
            foreach (var item in eventHandlers) //事后要是改成 sub/pub 模式这里做订阅即可
            {
                item.Handler();
            }
        }


        internal static void RunIntalmentTimerTask()
        {
            IntalmentTimerTask timer = new IntalmentTimerTask();
            timer.DoTask();
        }

        public static void Run()
        {
            SubscribeIntegralEvents();
            RunIntalmentTimerTask();
        }
        public static void OrderIntegral(OrderCompletedEvent orderCompleteEvent)
        {
            RunIntegral(orderCompleteEvent, 1);

        }
        internal static void RechargeIntegral(RechargeCompletedEvent rechargeCompleteEvent)
        {
            RunIntegral(rechargeCompleteEvent, 2);
        }

        private static void RunIntegral(IIntegralEvent integralEvent, int sourceType)
        {
            if (string.IsNullOrEmpty(integralEvent.MemberId)) return;
            var member = MembersService.GetMember(integralEvent.MemberId, integralEvent.CompanyId);
            var rounder = new IntegralRoundFactory().CreateRounder(integralEvent.CompanyId);
            var ruleProviders = new QuanChengTaoIntegralRuleFactory(integralEvent.CompanyId).CreateRuleProviders(new DefaultIntegralRuleProviderLoader());
            var flower = new QuanChengTaoIntegralRuleFlowProvider(integralEvent.CompanyId, rounder);
            var integralKV = new Dictionary<IIntegralRule, decimal>();
            foreach (var ruleProvider in ruleProviders)
            {
                var integrals = flower.DoFlow(integralEvent, ruleProvider, member);
                foreach (var item in integrals)
                {
                    integralKV.Add(item.Key, item.Value);
                }
            }
            var createdt = DateTime.Now;
            //record 
            List<IntegralRecords> records = new List<IntegralRecords>();
            foreach (var item in integralKV)
            {
                var integralRecords = new IntegralRecords()
                {
                    CreateDt = createdt,
                    Id = Guid.NewGuid().ToString("N"),
                    Integral = item.Value,
                    IntegralRuleId = Convert.ToInt32(item.Key.Id),
                    Source = integralEvent.SourceRecordId,
                    SourceType = sourceType,
                    CompanyId = integralEvent.CompanyId,
                    MemberId = integralEvent.MemberId,
                    StoreId = integralEvent.StoreId,
                    OperatorUid = integralEvent.OperatorUid
                };
                records.Add(integralRecords);
            }
            if (records.Count == 0) return;
            //分期
            QuanChengTaoInstalmentRuleProvider instalmentRuleProvider = new QuanChengTaoInstalmentRuleProvider();
            var integralsForInstalment = records.Select(o => new QuanChengTaoIntegralInstalment() { IntegralRecordId = o.Id, CompanyId = o.CompanyId, Integral = o.Integral, IntegralRuleId = o.IntegralRuleId });
            var instalments = instalmentRuleProvider.Run(integralsForInstalment);
            var instalmentRecords = new List<InstalmentRecord>();
            foreach (var item in instalments)
            {
                var instalmentR = item as QuanChengTaoInstalmentItem;
                var instalmentRecord = new InstalmentRecord()
                {
                    IntegralRecordId = instalmentR.IntegralRecordId,
                    CompanyId = integralEvent.CompanyId,
                    CreateDT = createdt,
                    InstalmentDT = instalmentR.InstalmentDT,
                    InstalmentRuleId = Convert.ToInt32(item.InstalmentRuleId),
                    Integral = item.InstalmentNumber,
                    MemberId = integralEvent.MemberId
                };
                instalmentRecords.Add(instalmentRecord);
            }
            if (instalmentRecords.Count == 0)
            {
                member.UsableIntegral += records.Sum(o => o.Integral);
                BaseService<IntegralRecords>.AddRange(records);
            }
            else
            {
                var noInstalments = records.Where(o => !instalmentRecords.Any(p => p.IntegralRecordId == o.Id)).ToList();
                member.UsableIntegral += noInstalments.Sum(o => o.Integral);
                BaseService<IntegralRecords>.AddRange(records, false);
                BaseService<InstalmentRecord>.AddRange(instalmentRecords);
            }
        }
    }
}
