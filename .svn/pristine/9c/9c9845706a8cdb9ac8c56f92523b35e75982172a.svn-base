using Pharos.Logic.DAL;
using Pharos.Logic.Entity;
using Pharos.Logic.EntityExtend;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;

namespace Pharos.Logic.BLL
{
    /// <summary>
    /// 权限逻辑层
    /// 管理全局权限code信息
    /// </summary>
    public class SysLimitsBLL : BaseService<Sys.Entity.SysLimits>
    {
        public void SyncLimit(List<Sys.Entity.SysLimits> list)
        {
            if (list != null && list.Any())
            {
                var limits = FindList(o => o.CompanyId == CommonService.CompanyId);
                Delete(limits);
                AddRange(list);
            }
        }
        public static new OpResult Delete(List<Sys.Entity.SysLimits> list)
        {
            CurrentRepository.RemoveRange(list);
            return OpResult.Success();
        }
        public static new OpResult AddRange(List<Sys.Entity.SysLimits> entities, bool isSave = true)
        {
            CurrentRepository.AddRange(entities,isSave);
            return OpResult.Success();
        }
    }
}
