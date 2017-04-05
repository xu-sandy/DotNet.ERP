﻿using System.Collections.Generic;

namespace Pharos.Logic.InstalmentDomain.Interfaces
{
    public interface IInstalmentRuleProvider<TParameter>
    {
        /// <summary>
        /// 用于权限过滤，或者其他过滤
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>是否允许分期</returns>
        bool Filter(TParameter parameter);
        /// <summary>
        /// 加载规则
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>分期规则</returns>
        IInstalmentRule<TParameter> LoadRule(TParameter parameter);

        IEnumerable<IInstalmentItem> Run(TParameter parameter);

        IEnumerable<IInstalmentItem> Run(IEnumerable<TParameter> parameter);

    }
}
