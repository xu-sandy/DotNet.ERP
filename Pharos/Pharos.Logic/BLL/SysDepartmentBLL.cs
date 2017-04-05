﻿using Pharos.Logic.DAL;
using Pharos.Logic.Entity;
using Pharos.Logic.EntityExtend;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.Utility.Helpers;
using Pharos.Sys.Entity;


namespace Pharos.Logic.BLL
{
    public class SysDepartmentBLL : BaseService<SysDepartments>
    {
        private SysDepartmentDAL _dal = new SysDepartmentDAL();
        /// <summary>
        /// 查找全部数据
        /// </summary>
        /// <returns></returns>
        public List<SysDepartmentsExt> GetList()
        {
            return _dal.GetList();
        }

        /// <summary>
        /// 根据ID查找权限实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysDepartmentsExt GetDepartment(string id, int pid)
        {
            SysDepartmentsExt model = _dal.GetDepById(id);
            if (model == null)
            {
                model = new SysDepartmentsExt();
                model.PDepId = pid;
                model.Type = (short)(pid + 1);
                var pmodel = FindById(pid);
                if (pmodel != null)
                {
                    model.PDepId = pmodel.Id;
                    model.PTitle = pmodel.Title;
                }
            }
            return model;
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult SaveOrg(SysDepartments model)
        {
            var result = OpResult.Fail("数据保存失败!");
            try
            {
                var savedate = Find(t => t.Id == model.Id);
                if (savedate != null)
                {
                    if (this.IsExits(savedate.SN, savedate.Id.ToString()))
                    {
                        result = OpResult.Fail("代码已存在!");
                    }
                    else
                    {
                        //savedate.Replace(model);
                        result = Update(savedate);
                    }
                }
                else
                {
                    if (this.IsExits(model.SN, ""))
                    {
                        result = OpResult.Fail("代码已存在!");
                    }
                    else
                    {
                        savedate = model;
                        result = Add(savedate);
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpResult ChangeStatus(int id)
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

        #region private
        /// <summary>
        /// sn要求唯一，判断sn是否存在
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        private bool IsExits(string sn, string notId)
        {
            return IsExist(o => o.SN == sn && o.Id != int.Parse(notId));
        }
        #endregion
    }
}
