using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.EntityExtend;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// 菜单管理业务逻辑
    /// </summary>
    public class SysMenuBLL
    {
        private SysMenusDAL _dal = new SysMenusDAL();

        #region 菜单管理
        /// <summary>
        /// 查找全部数据
        /// </summary>
        /// <returns></returns>
        public List<SysMenusExt> GetList()
        {
            return _dal.GetList();
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public List<SysMenusExt> GetMenusTreeList()
        {
            return _dal.GetMenusTreeList();
        }
        /// <summary>
        /// 移动菜单，更改菜单排序
        /// </summary>
        /// <param name="mode">1：上移，2：下移</param>
        /// <param name="menuId">菜单Id</param>
        /// <returns></returns>
        public OpResult MoveMenuItem(int mode, int menuId)
        {
            var result = OpResult.Fail("菜单项排序变更失败");
            try
            {
                var childrens = _dal.FindParentChilds(menuId);
                var currentMenu = childrens.FirstOrDefault(o => o.MenuId == menuId);
                if (currentMenu != null)
                {
                    switch (mode)
                    {
                        case 1:
                            var minSortOrder = childrens.Min(o => o.SortOrder);
                            if (currentMenu.SortOrder > minSortOrder)
                            {
                                SysMenus previousMenu = null;
                                int i = 1;
                                while (previousMenu == null && (currentMenu.SortOrder - i) >= minSortOrder)
                                {
                                    previousMenu = childrens.FirstOrDefault(o => o.SortOrder == (currentMenu.SortOrder - i));
                                    i++;
                                }
                                var sortOrder = currentMenu.SortOrder;
                                currentMenu.SortOrder = previousMenu.SortOrder;
                                previousMenu.SortOrder = sortOrder;

                                _dal.UpdateMenuOrderIndex(previousMenu);
                                _dal.UpdateMenuOrderIndex(currentMenu);
                            }
                            break;
                        case 2:
                            var maxSortOrder = childrens.Max(o => o.SortOrder);
                            if (currentMenu.SortOrder < maxSortOrder)
                            {
                                SysMenus previousMenu = null;
                                int i = 1;
                                while (previousMenu == null && (currentMenu.SortOrder + i) <= maxSortOrder)
                                {
                                    previousMenu = childrens.FirstOrDefault(o => o.SortOrder == (currentMenu.SortOrder + i));
                                    i++;
                                }
                                var sortOrder = currentMenu.SortOrder;
                                currentMenu.SortOrder = previousMenu.SortOrder;
                                previousMenu.SortOrder = sortOrder;
                                _dal.UpdateMenuOrderIndex(previousMenu);
                                _dal.UpdateMenuOrderIndex(currentMenu);
                            }
                            break;
                    }
                }
                result = OpResult.Success("数据保存成功");
            }
            catch (Exception e)
            {
                result = OpResult.Fail("菜单项排序变更失败" + e.Message);
            }
            return result;
        }

        /// <summary>
        /// 更改菜单状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpResult ChangeStatus(int id)
        {
            var result = OpResult.Fail("状态变更失败");
            try
            {
                var model = _dal.GetById(id);
                model.Status = !model.Status;
                _dal.UpdateStatus(model);
                result = OpResult.Success("数据保存成功");
            }
            catch (Exception e)
            {
                result = OpResult.Fail("状态变更失败" + e.Message);
            }
            return result;
        }
        /// <summary>
        /// 根据Id获得菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pobjid"></param>
        /// <returns></returns>
        public SysMenusExt GetModel(int id, int pobjid)
        {
            SysMenusExt obj = _dal.GetExtModel(id);
            if (obj == null)
            {
                obj = new SysMenusExt();
            }
            if (pobjid != 0)
            {
                var pobj = _dal.GetByColumn(pobjid, "MenuId");
                if (pobj != null)
                {
                    obj.PMenuId = pobj.MenuId;
                    obj.PTitle = pobj.Title;
                }
            }
            return obj;
        }
        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult SaveMenu(SysMenus model)
        {
            var result = OpResult.Fail("数据保存失败!");
            try
            {//todo: Set Depth
                if (_dal.ExistsById(model.Id))
                {
                    var re = _dal.Update(model);
                    if (re) { result = OpResult.Success("数据保存成功"); }
                }
                else
                {
                    var maxObjId = _dal.MaxVal("MenuId");
                    model.MenuId = maxObjId + 1;
                    model.SortOrder = _dal.GetMenuMaxIndex(model.PMenuId) + 1;
                    var re = _dal.Insert(model);
                    if (re > 0) { result = OpResult.Success("数据保存成功"); }
                }
            }
            catch (Exception ex)
            {
                result = OpResult.Fail("数据保存失败!" + ex.Message);
                new LogEngine().WriteError(ex);
            }
            return result;
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return _dal.Delete(id);
        }
        #endregion
    }
}
