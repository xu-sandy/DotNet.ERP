using Pharos.Logic.InstalmentDomain.Interfaces;
using System.Collections.Generic;

namespace Pharos.Logic.InstalmentDomain
{
    public abstract class BaseInstalmentRuleProvider<TInstalmentRule, TParameter> : IInstalmentRuleProvider<TParameter>
        where TInstalmentRule : IInstalmentRule<TParameter>, new()
    {
        /// <summary>
        /// 用于权限过滤，或者其他过滤
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual bool Filter(TParameter parameter)
        {
            return true;
        }


        public virtual IInstalmentRule<TParameter> LoadRule(TParameter parameter)
        {
            return null;
        }



        public virtual IEnumerable<IInstalmentItem> Run(TParameter parameter)
        {
            if (Filter(parameter))
            {
                var rule = LoadRule(parameter);
                if (rule != null)
                {
                    return rule.GetInstalmentItems(parameter);
                }
            }
            return null;
        }

        public virtual IEnumerable<IInstalmentItem> Run(IEnumerable<TParameter> parameters)
        {
            List<IInstalmentItem> instalmentItems = new List<IInstalmentItem>();
            foreach (var parameter in parameters)
            {
                var result = Run(parameter);
                if (result != null)
                {
                    instalmentItems.AddRange(result);
                }
            }
            return instalmentItems;
        }
    }
}
