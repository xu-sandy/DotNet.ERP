using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.EntityExtend;
using Pharos.Logic.OMS.Entity.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.OMS.BLL;

namespace Pharos.OMS.Retailing.Controllers
{
    public class MenuController : BaseController
    {
        #region 菜单管理

        [Ninject.Inject]
        SysMenuBLL _menuBLL { get; set; }

        /// <summary>
        /// 菜单管理-页面加载
        /// </summary>
        /// <returns></returns>
        public ActionResult Menus()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MoveMenuItem(int mode, int menuId)
        {
            var result = _menuBLL.MoveMenuItem(mode, menuId);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 菜单管理-页面数据加载
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMenus()
        {
            var datas = _menuBLL.GetList();
            //构造树数据
            List<SysMenusExt> models = new List<SysMenusExt>();
            if (datas != null && datas.Count > 0)
            {
                datas.Where(t => t.PMenuId == 0).Each(t =>
                {
                    models.Add(GetMenuChildsEasyuiGridData(t, datas));
                });
            }
            return ToDataGrid(models, 2);
        }
        /// <summary>
        /// 菜单管理-新增或编辑表单-页面加载
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pobjid"></param>
        /// <returns></returns>
        public ActionResult MenusSave(int id = 0, int pobjid = -1)
        {
            var model = _menuBLL.GetModel(id, pobjid);
            return View(model);
        }
        /// <summary>
        /// 菜单管理-新增或编辑表单-数据保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MenusSave(SysMenus model)
        {
            var result = _menuBLL.SaveMenu(model);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 菜单管理-更改菜单状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeMenuStatusById(int id)
        {
            var result = _menuBLL.ChangeStatus(id);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 菜单管理-根据id移除菜单信息
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveMenuById(int id)
        {
            var re = _menuBLL.Delete(id);
            return Content(re.ToJson());
        }
        /// <summary>
        /// 菜单管理-菜单下拉树数据源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpPost]
        [HttpGet]
        public ActionResult GetMenusTreeList(int id)
        {
            var datas = _menuBLL.GetList();
            //构造树数据
            List<EasyuiTree> models = new List<EasyuiTree>();
            datas.Where(t => t.PMenuId == 0).Each(t =>
            {
                models.Add(GetMenusChildsEasyuiTreeData(t, datas));
            });
            return new JsonNetResult(models);
        }
        #region private
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private SysMenusExt GetMenuChildsEasyuiGridData(SysMenusExt model, List<SysMenusExt> source)
        {
            var childs = source.Where(s => s.PMenuId == model.MenuId);
            if (childs.Count() > 0)
            {
                model.Childs = new List<SysMenusExt>();
                childs.Each(t =>
                {
                    model.Childs.Add(GetMenuChildsEasyuiGridData(t, source));
                });
            }
            return model;
        }
        /// <summary>
        /// 构造菜单下拉树子集数据
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private EasyuiTree GetMenusChildsEasyuiTreeData(SysMenusExt menu, List<SysMenusExt> source)
        {
            var model = new EasyuiTree { id = menu.MenuId.ToString(), text = menu.Title };
            var childs = source.Where(s => s.PMenuId == menu.MenuId && s.Status == true);
            if (childs.Count() > 0)
            {
                model.children = new List<EasyuiTree>();
                childs.Each(t =>
                {
                    model.children.Add(GetMenusChildsEasyuiTreeData(t, source));
                });
            }
            return model;
        }
        #endregion
        #endregion 菜单管理
    }
}
