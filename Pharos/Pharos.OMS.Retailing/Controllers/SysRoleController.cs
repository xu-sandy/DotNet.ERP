﻿using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
namespace Pharos.OMS.Retailing.Controllers
{
    public class SysRoleController : BaseController
    {
        #region 私有对象
        [Ninject.Inject]
        SysRoleService RoleService { get; set; }
        [Ninject.Inject]
        DepartMentService DeptService { get; set; }
        #endregion
        [SysPermissionValidate]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = RoleService.GetPageList(Request.Params);
            return ToDataGrid(list, count);
        }

        [HttpPost]
        [SysPermissionValidate(223)]
        public ActionResult Delete(int[] ids)
        {
            return new JsonNetResult(RoleService.Deletes(ids));
        }
        [SysPermissionValidate(222)]
        public ActionResult Save(int? id)
        {
            var obj = new SysRoles() { Type=1};
            var datas = new Dictionary<int,string>();
            if (id.HasValue)
            {
                obj = RoleService.Get(id.Value);
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult FindMenuList(int? roleId)
        {
            var list = RoleService.FindMenuList(roleId);
            return new JsonNetResult(list);
        }
        [HttpPost]
        public ActionResult Save(SysRoles obj)
        {
            var re = RoleService.SaveOrUpdate(obj, Request["MenuIds"], Request["LimitIds"]);
            return new OpActionResult(re);
        }
        [HttpPost]
        public void MoveMenuItem(short mode, int menuId, int roleId)
        {
            RoleService.MoveMenuItem(mode, menuId, roleId);
        }
        [HttpPost]
        public void SetState(short state, string id)
        {
            RoleService.SetState(state, id);
        }

        public ActionResult SaveDept(string roleIds)
        {
            var list = ListToSelect(DeptService.GetFullTitle().Select(o => new SelectListItem() { Text = o.Value, Value = o.Key.ToString() }), emptyTitle: "请选择");
            var datas = new Dictionary<int, string>();
            if (!roleIds.IsNullOrEmpty())
            {
                var roles = RoleService.GetListByRoleId(roleIds.Split(',').Select(o=>int.Parse(o)).ToArray());
                var deptids = string.Join(",", roles.Where(o => !o.DeptId.IsNullOrEmpty()).SelectMany(o => o.DeptId.Split(',')));
                if (!deptids.IsNullOrEmpty())
                {
                    datas = DeptService.GetFullTitle(deptids);
                }
                ViewBag.titles = string.Join("、", roles.Select(o => o.Title));
            }
            ViewBag.depts = list.ToJson();
            ViewBag.datas = datas.Select(o => new { DeptId = o.Key, Title = o.Value }).ToJson();
            return View();
        }
        [HttpPost]
        public ActionResult SaveDept(string roleIds, string inserted, string deleted)
        {
            var re = RoleService.SaveDept(roleIds, inserted, deleted);
            return new OpActionResult(re);
        }
        public ActionResult MoveMenu(string roleIds)
        {
            var roles = RoleService.GetListByRoleId(roleIds.Split(',').Select(o => int.Parse(o)).ToArray());
            ViewBag.titles = string.Join("、", roles.Select(o => o.Title));
            return View();
        }
        [HttpPost]
        public void MoveMenuItems(short mode, int menuId, string roleIds)
        {
            RoleService.MoveMenuItems(mode, menuId, roleIds);
        }
    }
}