using Pharos.DBFramework;
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
    public class OMSSysUserInfoBLL
    {
        private OMSSysUserInfoDAL _dal = new OMSSysUserInfoDAL();
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
        /// 根据Id获得用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OMS_SysUserInfo GetModel(int id)
        {
            var data = _dal.GetById(id);
            if (data == null)
            {
                data = new OMS_SysUserInfo();
            }
            return data;
        }
        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult SaveUser(Entity.OMS_SysUserInfo model)
        {
            var result = OpResult.Fail("数据保存失败!");
            try
            {
                if (_dal.ExistName(model))
                {
                    var oldObj = _dal.GetById(model.Id);
                    if (oldObj != null)
                    {
                        var re = _dal.Update(model);
                        if (re) { result = OpResult.Success("数据保存成功"); }
                    }
                    else
                    {
                        //判断员工编号是否重复
                        if (_dal.ExistsColumn(0, "Id", model.UserCode, "UserCode",SysCommonRules.CompanyId))
                        {
                            result = OpResult.Fail("该编号已存在，请重新输入编号");
                        }
                        else
                        {
                            model.CreateDT = DateTime.Now;
                            model.LoginDT = DateTime.Now;
                            model.UID = SysCommonRules.GUID;
                            model.UserCode = (int.Parse(_dal.GetMaxUserCode() + 1)).ToString();
                            if (model.LoginPwd == null)
                            {
                                result = OpResult.Fail("新增用户，密码不能为空!");
                            }
                            else
                            {
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
        /// 获得新添加用户的UserCode
        /// </summary>
        /// <returns></returns>
        public int GetNewUserCode()
        {
            var maxUserCode = _dal.GetMaxUserCode();
            return int.Parse(maxUserCode) + 1;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpResult Delete(object[] ids)
        {
            var currentUser = _dal.GetByColumn(CurrentUser.UID, "UID");
            if (ids.Contains(currentUser.Id))
            {
                return OpResult.Fail("您不能删除自身用户。");
            }
            var re = _dal.Delete(ids);
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

        public OMS_SysUserInfo GetModelByUID(string uid)
        {
            return _dal.GetByColumn(uid, "UID");
        }
        public OMS_SysUserInfo Login(string loginName,string password)
        {
            var user = _dal.GetByColumn(loginName, "loginName");
            if (user!=null && user.Status == 1 && user.LoginPwd == password)
                return user;
            return null;
        }
    }
}
