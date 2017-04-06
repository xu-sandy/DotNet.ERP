using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL
{
    public class RoleInfoService : BaseService<SysRoles>
    {
        public static IList<SysRoles> GetAllRoles(string keyword = "")
        {
            var entities = RoleInfoService.CurrentRepository.QueryEntity;
            if (!string.IsNullOrEmpty(keyword))
            {
                entities = entities.Where(o => o.Title.Contains(keyword));
            }
            return entities.ToList();
        }
    }
}
