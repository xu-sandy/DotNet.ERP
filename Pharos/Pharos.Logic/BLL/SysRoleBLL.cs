using Pharos.Logic.DAL;
using Pharos.Logic.EntityExtend;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Pharos.Sys.Entity;
namespace Pharos.Logic.BLL
{
    public class SysRoleBLL : BaseService<SysRoles>
    {
        private SysLimitisDAL _dal = new SysLimitisDAL();
        /// <summary>
        /// 角色管理网格分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public PageList<SysRoles> GetPageList(int pageIndex,int pageSize,string rolename)
        {
            if (rolename == null)
            {
                rolename = string.Empty;
            }
            var entities = CurrentRepository.QueryEntity;
            if (!string.IsNullOrEmpty(rolename))
            {
                entities = entities.Where(o => o.Title.Contains(rolename)).OrderBy(r=>r.Title);
            }
            entities = entities.OrderBy(r => r.Title);
            return new PageList<SysRoles>(entities,pageIndex,pageSize);
        }
        /// <summary>
        /// 根据角色Id获得角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysRoles GetRole(string id) {
            var data = FindById(id);
            if (data == null) {
                data = new SysRoles();
            }
            return data;
        }

        public List<SysLimitsExt> GetRoleAllLimits()
        {
            return _dal.GetRoleAllLimits();
        }
        /// <summary>
        /// 更改权限样式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpResult ChangeStatus(string id)
        {
            var result = OpResult.Fail("状态变更失败!");
            try
            {
                var model = FindById(id);
                model.Status = !model.Status;
                result = Update(model);
            }
            catch (Exception e)
            {
                result = OpResult.Fail("状态变更失败!" + e.Message);
            }
            return result;
        }
        public static List<SysCustomMenus> GetCustomMenus()
        {
            return BaseService<SysCustomMenus>.FindList(o => o.CompanyId == CommonService.CompanyId);
        }
        public static void RemoveCustomMenus(List<SysCustomMenus> list)
        {
            BaseService<SysCustomMenus>.CurrentRepository.RemoveRange(list);
        }
        public static void AddCustomMenus(List<SysCustomMenus> list)
        {
            BaseService<SysCustomMenus>.CurrentRepository.AddRange(list);
        }
        public static new OpResult AddRange(List<SysRoles> entities, bool isSave = true)
        {
            CurrentRepository.AddRange(entities);
            return OpResult.Success();
        }
        public static new OpResult Delete(List<SysRoles> list)
        {
            CurrentRepository.RemoveRange(list);
            return OpResult.Success();
        }
    }
}
