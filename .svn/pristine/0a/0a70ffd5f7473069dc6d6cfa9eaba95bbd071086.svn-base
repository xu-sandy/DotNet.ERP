using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class PrivilegeCalcMap : EntityTypeConfiguration<PrivilegeCalc>
    {
        public PrivilegeCalcMap()
        {
            HasMany(o => o.Details).WithRequired(o=>o.Calc).HasForeignKey(o => o.PrivilegeCalcId);
            HasMany(o => o.Results).WithRequired(o=>o.Calc).HasForeignKey(o => o.PrivilegeCalcId);
        }
    }
}
