using Pharos.Logic.MemberDomain.Interfaces;
using Pharos.Logic.MemberDomain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pharos.Logic.MemberDomain.Exceptions;

namespace Pharos.Logic.MemberDomain
{
    public class DefaultIntegralRuleProviderLoader : IIntegralRuleProviderLoader
    {
        public virtual bool TryLoadIntegralRuleProviders(IEnumerable<int> ruleProviderIds, out IEnumerable<IIntegralRuleProvider> ruleProviders)
        {
            var commandAssemblies = new List<Assembly>();
            List<IIntegralRuleProvider> tempRuleProviders = new List<IIntegralRuleProvider>();
            if (!commandAssemblies.Any())
            {
                commandAssemblies.Add(this.GetType().Assembly);
            }
            foreach (var assembly in commandAssemblies)
            {
                try
                {
                    tempRuleProviders.AddRange(assembly.GetImplementedObjectsByInterface<IIntegralRuleProvider>());
                }
                catch (Exception exc)
                {
                    throw new IntegralRuleProviderLoadException(string.Format("加载积分规则提供程序集失败，程序集： {0}!", assembly.FullName), exc);
                }
            }
            ruleProviders = tempRuleProviders;
            return true;
        }


    }
}
