﻿using Pharos.DBFramework;
using Pharos.Sys.DAL;
using Pharos.Sys.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharos.Sys.Models;

namespace Pharos.Sys.BLL
{
    public class SysUserInfoBLL
    {
        private SysUserInfoDAL _dal = new SysUserInfoDAL();
        private SysStoreUserInfoDAL _storedal = new SysStoreUserInfoDAL();
        /// <summary>
        /// 获得用户列表
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataTable GetList(Paging paging, QueryUserModel queryUserModel)
        {
            return _dal.GetList(paging, queryUserModel);
        }
        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <param name="selectType"></param>
        /// <returns></returns>
        public List<SysUserInfo> GetUsers(int selectType, string keyWord = "", string selectUid = "")
        {
            return _dal.GetUsers(selectType, keyWord, selectUid);
        }
        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <param name="selectType"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<SysUserInfo> GetUsersForDropDown(int selectType, string uid)
        {
            return _dal.GetUsers(selectType, uid);
        }
        /// <summary>
        /// 根据Id获得用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysUserInfo GetModel(int id)
        {
            var data = _dal.GetById(id);
            if (data == null)
            {
                data = new SysUserInfo();
                data.UserCode = SysCommonRules.UserCode;
            }
            return data;
        }
        /// <summary>
        /// 根据id获取门店用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysStoreUserInfo GetStoreUserInfoById(int id)
        {
            var user = _storedal.GetById(id);
            if (user == null)
            {
                user = new SysStoreUserInfo();
                user.UserCode = SysCommonRules.UserCode;
            }
            return user;
        }
        /// <summary>
        /// 根据id获取门店用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysStoreUserInfo GetStoreUserInfoByUId(string uid)
        {
            var user = _storedal.GetByColumn(uid,"uid");
            if (user == null)
            {
                user = new SysStoreUserInfo();
                user.UserCode = SysCommonRules.UserCode;
            }
            return user;
        }
        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult SaveUser(Entity.SysUserInfo model)
        {
            var result = OpResult.Fail("数据保存失败!");
            try
            {
                model.CompanyId = Sys.SysCommonRules.CompanyId;//16.03.24增加企业标识
                if (_dal.ExistName(model))
                {
                    var oldObj = _dal.GetById(model.Id);
                    if (oldObj != null)
                    {
                        var re = _dal.Update(model);
                        var suserobj = _storedal.GetByColumn(oldObj.UID, "UID");
                        if (suserobj != null)
                        {
                            _storedal.Update(oldObj.UID);
                        }
                        if (re) { result = OpResult.Success("数据保存成功"); }
                    }
                    else
                    {
                        //判断员工编号是否重复
                        if (_dal.ExistsColumn(0, "Id", model.UserCode, "UserCode", SysCommonRules.CompanyId))
                        {
                            result = OpResult.Fail("该编号已存在，请重新输入编号");
                        }
                        else
                        {
                            model.CreateDT = DateTime.Now;
                            model.LoginDT = DateTime.Now;
                            model.UID = SysCommonRules.GUID;
                            if (model.LoginPwd == null)
                            {
                                result = OpResult.Fail("新增用户，密码不能为空!");
                            }
                            else
                            {
                                var auth = CurrentUser.Company;
                                if (auth != null && auth.UserNum > 0)
                                {
                                    if (_dal.GetUsers(1).Count > auth.UserNum)
                                    {
                                        result.Message = "用户数超过允许的数量,不能再添加!";
                                        return result;
                                    }
                                }
                                var re = _dal.Insert(model);
                                if (re > 0) { result = OpResult.Success("数据保存成功"); }
                            }
                        }
                    }
                }
                else
                {
                    result = OpResult.Fail("员工姓名或登录帐号重复");
                }
            }
            catch (Exception ex)
            {
                result = OpResult.Fail("数据保存失败!" + ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 更新系统用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult UpdateUser(Entity.SysUserInfo model)
        {
            try
            {
                _dal.Update(model);
                return OpResult.Success("数据保存成功");
            }
            catch(Exception ex)
            {
                return OpResult.Fail("数据保存失败!" + ex.Message);
            }
        }
        /// <summary>
        /// 更新门店用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult UpdateStoreUser(Entity.SysStoreUserInfo model)
        {
            try
            {
                _storedal.Update(model);
                return OpResult.Success("数据保存成功");
            }
            catch (Exception ex)
            {
                return OpResult.Fail("数据保存失败!" + ex.Message);
            }
        }
        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult SaveUser(Entity.SysStoreUserInfo model)
        {
            var result = OpResult.Fail("数据保存失败!");
            try
            {
                model.CompanyId = Sys.SysCommonRules.CompanyId;//16.03.24增加企业标识
                var data = _storedal.GetStoreUserInfo(model);
                if (data.Count > 0)
                {
                    if (model.LoginPwd == null)
                    {
                        model.LoginPwd = data[0].LoginPwd;
                    }
                }
                if (!(data.Count > 0 && model.Id == 0))
                {
                    var oldObj = _storedal.GetById(model.Id);
                    if (oldObj != null)
                    {
                        var re = _storedal.Update(model);
                        var suserobj = _storedal.GetByColumn(oldObj.UID, "UID");
                        if (suserobj != null)
                        {
                            _storedal.Update(oldObj.UID);
                        }
                        if (re) { result = OpResult.Success("数据保存成功"); }
                    }
                    else
                    {
                        //判断员工编号是否重复
                        if (_storedal.ExistsColumn(0, "Id", model.UserCode, "UserCode", SysCommonRules.CompanyId))
                        {
                            result = OpResult.Fail("该编号已存在，请重新输入编号");
                        }
                        else
                        {
                            //model.CreateDT = DateTime.Now;
                            model.LoginDT = DateTime.Now;
                            model.UID = SysCommonRules.GUID;
                            if (model.LoginPwd == null)
                            {
                                result = OpResult.Fail("新增用户，密码不能为空!");
                            }
                            else
                            {
                                var auth = CurrentUser.Company;
                                if (auth != null && auth.UserNum > 0)
                                {
                                    if (_storedal.GetUsers(1).Count > auth.UserNum)
                                    {
                                        result.Message = "用户数超过允许的数量,不能再添加!";
                                        return result;
                                    }
                                }
                                var re = _storedal.Insert(model, true);
                                if (re > 0) { result = OpResult.Success("数据保存成功"); }
                            }
                        }
                    }
                }
                else
                {
                    string store = Sys.SysCommonRules.CurrentStore;
                    if (("|" + data[0].OperateAuth + "|").Contains("|" + store + ","))
                        result = OpResult.Fail("员工姓名或登录帐号重复");
                    else
                    {
                        data[0].OperateAuth += "|" + model.OperateAuth;
                        var r = _storedal.UpdataOperateAuth(data[0]);
                        if (r)
                            return OpResult.Success("操作成功！");
                        else
                            return OpResult.Success("数据保存失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                result = OpResult.Fail("数据保存失败!" + ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 获得新添加用户的UserCode
        /// </summary>
        /// <returns></returns>
        public int GetNewUserCode()
        {
            var maxUserCode = _dal.GetMaxUserCode(SysCommonRules.CompanyId);
            return int.Parse(maxUserCode) + 1;
        }
        /// <summary>
        /// 获取用户门店角色信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public SysStoreUserInfo GetUserStoreRole(string uid)
        {
            var obj = _storedal.GetByColumn(uid, "UID");
            return obj;
        }
        /// <summary>
        /// 获取所有用户门店角色初始数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserStoreRoles()
        {
            return _storedal.GetList(SysCommonRules.CompanyId);

        }
        public OpResult SaveStoreUserInfo(SysStoreUserInfo model)
        {
            var result = OpResult.Fail("数据保存失败!");
            try
            {
                var obj = _storedal.GetByColumn(model.UID, "UID");
                if (obj != null)
                {
                    if (model.LoginPwd == null)
                    {
                        model.LoginPwd = obj.LoginPwd;
                    }
                }
                var auths = model.OperateAuth;
                if (obj != null)
                {
                    var re = false;
                    if (!string.IsNullOrEmpty(model.OperateAuth))
                    {
                        re = _storedal.UpdataOperateAuth(model);
                    }
                    else
                    {
                        auths = obj.OperateAuth;
                        re = _storedal.Delete(obj.Id);
                    }
                    if (re) { result = OpResult.Success("数据保存成功"); }
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.OperateAuth))
                    {
                        //     new SqlParameter("@FullName",model.FullName),
                        //new SqlParameter("@UserCode",model.UserCode),
                        //new SqlParameter("@LoginPwd",model.LoginPwd),
                        //new SqlParameter("@Sex",model.Sex),
                        //new SqlParameter("@OperateAuth",model.OperateAuth),
                        //new SqlParameter("@Status",model.Status),
                        //new SqlParameter("@LoginName",model.LoginName)

                        if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.UserCode) || string.IsNullOrEmpty(model.LoginName) || string.IsNullOrEmpty(model.LoginPwd))
                        {
                            var oUser = _dal.GetByColumn(model.UID, "UID");
                            if (string.IsNullOrEmpty(model.FullName))
                            {
                                model.FullName = oUser.FullName;
                            }
                            if (string.IsNullOrEmpty(model.UserCode))
                            {
                                model.UserCode = oUser.UserCode;
                            }
                            if (string.IsNullOrEmpty(model.LoginName))
                            {
                                model.LoginName = oUser.LoginName;
                            }
                            if (string.IsNullOrEmpty(model.LoginPwd))
                            {
                                model.LoginPwd = oUser.LoginPwd;
                            }
                            if (model.LoginDT == default(DateTime))
                            {
                                model.LoginDT = DateTime.Now;
                            }
                            if (model.Status == 0)
                            {
                                model.Status = oUser.Status;
                            }
                        }

                        var re = _storedal.Insert(model);
                        if (re > 0) { result = OpResult.Success("数据保存成功"); }
                    }
                    else
                    {
                        result = OpResult.Fail("请选择用户门店角色。");
                    }
                }
                if (result.Successed && !string.IsNullOrWhiteSpace(auths))
                {
                    var stores = new List<string>();
                    foreach (var a in auths.Split('|'))
                    {
                        stores.Add(a.Split(',')[0]);
                    }
                    var storeIds = string.Join(",", stores);
                    //Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = SysCommonRules.CompanyId, StoreId = storeIds, Target = "SysStoreUserInfo" });
                }
            }
            catch (Exception ex)
            {
                result = OpResult.Fail("数据保存失败!" + ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpResult Delete(int id, string uid)
        {
            if (CurrentUser.UID.ToLower() == uid.ToLower())
            {
                return OpResult.Fail("您没有删除用户权限。");
            }
            var obj = _dal.GetById(id);
            if (obj != null)
            {
                if (obj.Status == 1 || obj.Status == 2)
                    return OpResult.Fail("该用户不可删除。");
            }
            var re = _dal.Delete(id);
            var suserobj = _storedal.GetByColumn(uid, "UID");
            if (suserobj != null)
            {
                _storedal.Delete(suserobj.Id);
            }
            return re ? OpResult.Success("删除用户成功") : OpResult.Fail("删除用户失败。");
        }
        /// <summary>
        /// 通过UID删除用户，不考虑其他字段影响
        /// </summary>
        /// <param name="uid">用户UID</param>
        /// <returns></returns>
        public OpResult Delete(string uid)
        {
            var obj = GetModelByUID(uid);
            if (obj != null)
            {
                var re = _dal.Delete(obj.Id);
                return re ? OpResult.Success("删除用户成功。") : OpResult.Fail("删除用户失败。");
            }
            return OpResult.Fail("删除用户失败，找不到该用户。");
        }

        public SysUserInfo GetModelByUID(string uid)
        {
            return _dal.GetByColumn(uid, "UID");
        }
        /// <summary>
        /// 查找用户是否存在
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public object GetUserData(string fullName, string loginName)
        {
            return _storedal.GetUserData(fullName, loginName);
        }

        /// <summary>
        /// 获取门店用户
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public List<SysStoreUserInfo> GetStoreUser(string storeId, string keyword, int state)
        {
            return _storedal.GetStoreUser(storeId, keyword,state) ?? new List<SysStoreUserInfo>();
        }
    }
}
