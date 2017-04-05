using Pharos.Logic.Entity;
using Pharos.Logic.EntityExtend;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL
{
    /// <summary>
    /// 菜单管理业务逻辑
    /// </summary>
    public class SysMenuBLL : BaseService<Sys.Entity.SysMenus>
    {
        public void SyncMenu(List<Sys.Entity.SysMenus> list)
        {
            if(list!=null && list.Any())
            {
                var menus= FindList(o=>o.CompanyId==CommonService.CompanyId);
                Delete(menus);
                AddRange(list);
            }
        }
        public static new OpResult AddRange(List<Sys.Entity.SysMenus> entities, bool isSave = true)
        {
            CurrentRepository.AddRange(entities, isSave);
            return OpResult.Success();
        }
        public static new OpResult Delete(List<Sys.Entity.SysMenus> list)
        {
            CurrentRepository.RemoveRange(list);
            return OpResult.Success();
        }

    }
}
