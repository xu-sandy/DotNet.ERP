﻿using Pharos.DBFramework;
using Pharos.Sys.DAL;
using Pharos.Sys.Entity;
using Pharos.Sys.EntityExtend;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pharos.Sys.BLL
{
    public class SysRoleBLL
    {
        private SysRolesDAL _dal = new SysRolesDAL();

        /// <summary>
        /// 获得角色列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetList()
        {
            return (Sys.CurrentUser.RoleIds == "9") ? _dal.GetAllList() : _dal.GetList();
        }
        /// <summary>
        /// 根据角色Id获得角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysRoles GetModel(int id)
        {
            var data = _dal.GetById(id);
            if (data == null)
            {
                data = new SysRoles();
            }
            return data;
        }
        public SysRoles GetModelByRoleId(int roleId)
        {
            var data = _dal.GetBuRoleOd(roleId);
            var list = DBHelper.ToEntity.ToList<SysRoles>(data);
            return list[0];
        }
        /// <summary>
        /// 根据角色roleid获取用户的角色信息
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetRoleLimitsByUId(string uid)
        {
            var objs = new Dictionary<int, int>();
            var dt = _dal.GetRoleLimitsByUId(uid);
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objs.Add((int)dt.Rows[i]["Value"], (int)dt.Rows[i]["Value"]);
                }
            }
            return objs;
        }
        /// <summary>
        /// 根据角色roleid获取用户的角色信息
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetRoleLimitsByRoleId(string roleid)
        {
            var objs = new Dictionary<int, int>();
            var dt = _dal.GetRoleLimitsByRoleId(roleid, SysCommonRules.CompanyId);
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objs.Add((int)dt.Rows[i]["Value"], (int)dt.Rows[i]["Value"]);
                }
            }
            return objs;
        }
        /// <summary>
        /// 获得所有的权限列表
        /// </summary>
        /// <returns></returns>
        public List<SysLimitsExt> GetAllLimitList()
        {
            return _dal.GetAllLimitList();
        }
        /// <summary>
        /// 获取所有的角色列表
        /// </summary>
        /// <returns></returns>
        public List<SysRoles> GetRoleList()
        {
            return _dal.GetRoleList(SysCommonRules.CompanyId);
        }

        /// <summary>
        /// 更改权限状态
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
        /// 保存角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult SaveRole(SysRoles model)
        {
            var result = OpResult.Fail("数据保存失败!");
            try
            {
                //if (_dal.ExistsTitle(model.RoleId, model.Title))
                //{
                //    result = OpResult.Fail("角色名称不能重复！");
                //}
                //else {
                model.CompanyId = Sys.SysCommonRules.CompanyId;
                if (_dal.ExistsById(model.Id))
                {
                    var re = _dal.Update(model);
                    if (re) { result = OpResult.Success("数据保存成功"); }
                }
                else
                {
                    if (_dal.ExistsTitle(model.RoleId, model.Title, model.CompanyId))
                    {
                        result = OpResult.Fail("角色名称不能重复，或该名称为系统保留！");
                        return result;
                    }
                    var maxObjId = _dal.MaxVal("RoleId",SysCommonRules.CompanyId);
                    model.RoleId = maxObjId + 1;
                    var re = _dal.Insert(model);
                    if (re > 0) { result = OpResult.Success("数据保存成功"); }
                }
                //}
            }
            catch (Exception ex)
            {
                result = OpResult.Fail("数据保存失败!" + ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return _dal.Delete(id);
        }
        /// <summary>
        /// 保存角色权限信息
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="limitIds"></param>
        /// <returns></returns>
        public OpResult SaveModel(string roleid, string limitIds)
        {
            var result = OpResult.Fail("数据保存失败!");
            try
            {
                var obj = _dal.GetByColumn(roleid, "RoleId");
                if (obj != null)
                {
                    obj.LimitsIds = limitIds;
                    var re = _dal.UpdateLimitsId(obj);
                    if (re) { result = OpResult.Success("数据保存成功"); }
                }
            }
            catch (Exception ex)
            {
                result = OpResult.Fail("数据保存失败!" + ex.Message);
            }
            return result;
        }
    }
}
