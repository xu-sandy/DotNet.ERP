using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.MemberDomain.QuanChengTaoProviders.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class MeteringModeDescriptionAttribute : Attribute
    {
        public MeteringModeDescriptionAttribute(IntegralProviderType type, string description)
        {
            IntegralProviderType = type;
            Description = description;
        }
        public IntegralProviderType IntegralProviderType { get; private set; }
        public string Description { get; private set; }

    }
}
