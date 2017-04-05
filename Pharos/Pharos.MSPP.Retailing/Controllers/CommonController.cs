using Pharos.Logic.BLL;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.MSPP.Retailing.Controllers
{
    /// <summary>
    /// 公共方法统一调用
    /// </summary>
    public class CommonController : BaseController
    {
        private readonly Pharos.Sys.BLL.SysDepartmentBLL _sysDepartmentBLL;
        private readonly Pharos.Sys.BLL.SysRoleBLL _sysRoleBLL;
        private readonly Pharos.Sys.BLL.SysDataDictionaryBLL _sysDataDictionaryBLL;
        private readonly Pharos.Sys.BLL.SysUserInfoBLL _userBLL;
        public CommonController()
        {
            _sysDepartmentBLL = new Sys.BLL.SysDepartmentBLL();
            _sysRoleBLL = new Sys.BLL.SysRoleBLL();
            _sysDataDictionaryBLL = new Sys.BLL.SysDataDictionaryBLL();
            _userBLL = new Sys.BLL.SysUserInfoBLL();
        }
        //
        // GET: /Common/
        #region 系统管理
        /// <summary>
        /// 通用-获取系统用户下拉数据
        /// </summary>
        /// <param name="selecttype">全部：selecttype=1 请选择selecttype=0</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUsersEUIDropdown(int selecttype = 0,string uid="")
        {
            var datas = ListToSelect(_userBLL.GetUsersForDropDown(selecttype, uid).Select(o => new SelectListItem() { Value = o.UID, Text = o.FullName }), emptyTitle: (selecttype == 1 ? "全部" : "请选择"));
            return new JsonNetResult(datas);
        }
        /// <summary>
        /// 通用-获取机构下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOrgsEasyuiDropdown(int selecttype = 0)
        {
            var datas = ListToSelect(_sysDepartmentBLL.GetListByType(1).Select(o => new SelectListItem() { Value = o.DepId.ToString(), Text = o.Title }), emptyTitle: (selecttype == 1 ? "全部" : "请选择"));
            return new JsonNetResult(datas);
        }
        /// <summary>
        /// 通用-获取部门下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDepsEasyuiDropdown(int pDepId, int selecttype = 0)
        {
            var datas = ListToSelect(_sysDepartmentBLL.GetListByPDepId(pDepId).Select(o => new SelectListItem() { Value = o.DepId.ToString(), Text = o.Title }), emptyTitle: (selecttype == 1 ? "全部" : "请选择"));
            return new JsonNetResult(datas);
        }
        /// <summary>
        /// 通用-获取角色下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRolesEasyuiDropdown(int selecttype = 0)
        {
            var datas = ListToSelect(_sysRoleBLL.GetRoleList().Select(o => new SelectListItem() { Value = o.RoleId.ToString(), Text = o.Title }), emptyTitle: (selecttype == 1 ? "全部" : "请选择"));
            return new JsonNetResult(datas);
        }
        /// <summary>
        /// 通用-根据父级类别下的子数据字典项下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDicsEasyuiDropdown(int psn, int selecttype = 0)
        {
            var datas = ListToSelect(_sysDataDictionaryBLL.GetDicListByPSN(psn).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: (selecttype == 1 ? "全部" : "请选择"));
            return new JsonNetResult(datas);
        }
        #endregion
    }
}
