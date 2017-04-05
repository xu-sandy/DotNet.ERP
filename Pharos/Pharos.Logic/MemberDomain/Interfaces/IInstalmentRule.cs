using System.Collections.Generic;

namespace Pharos.Logic.InstalmentDomain.Interfaces
{
    public interface IInstalmentRule<TParameter>
    {
        string RuleId { get; set; }

        IEnumerable<IInstalmentItem> GetInstalmentItems(TParameter parameter);
    }
}
