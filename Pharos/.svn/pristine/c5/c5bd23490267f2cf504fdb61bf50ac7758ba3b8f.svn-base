using Pharos.Logic.BLL;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.Store.Retailing.Controllers
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
        public ActionResult GetDepsEasyuiDropdown(int? pDepId, int selecttype = 0)
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

        /// <summary>
        /// 通用-获取品牌下拉数据
        /// </summary>
        /// <param name="showAll">是否显示所有项（涉及状态）</param>
        /// <param name="emptyTitle">空值项显示文本</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBrandsList(bool showAll = false, string emptyTitle = "请选择")
        {
            var data = ProductBrandService.GetList(showAll).Select(o => new DropdownItem(o.BrandSN.ToString(), o.Title)).ToList();
            data.Insert(0, new DropdownItem("", Server.UrlDecode(emptyTitle), true));
            return new JsonNetResult(data);
        }
        public ActionResult GetSuppliersDropdown(string emptyTitle,string value="")
        {
            var list = SupplierService.GetList().Where(o => o.BusinessType == 1).Select(o => new DropdownItem() { Value = o.Id, Text = o.FullTitle }).ToList();
            if (!string.IsNullOrWhiteSpace(emptyTitle) && string.IsNullOrWhiteSpace(value))
                list.Insert(0, new DropdownItem() { Value = "", Text = emptyTitle, IsSelected = true });
            else if(list.Any())
            {
                var obj = list.FirstOrDefault(o => o.Value == value);
                if (obj != null)
                    obj.IsSelected = true;
                else if(emptyTitle!=null && !emptyTitle.StartsWith(" "))
                    list.FirstOrDefault().IsSelected = true;
            }
            return new JsonNetResult(list);
        }
    }
}
