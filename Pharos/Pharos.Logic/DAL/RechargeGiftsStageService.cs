﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;

namespace Pharos.Logic.BLL
{
    /// <summary>
    /// 充值赠送分期信息
    /// </summary>
    public class RechargeGiftsStageService : BaseService<RechargeGiftsStage>
    {
        /// <summary>
        /// 获取所有未充值的分期信息
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public static List<RechargeGiftsStage> GetNoRechargeStageByCompanyId(int companyid)
        {
            return CurrentRepository.Entities.Where(o => o.CompanyId == companyid && o.State == 0).ToList();
        }
        /// <summary>
        /// 根据id 更改分期赠送状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool UpdateRechargeStageStage(int id, int state)
        {
            var data = CurrentRepository.Entities.Where(o => o.Id == id).FirstOrDefault();
            if (data == null)
            { //没有数据
                return false;
            }
            //TODO:
            //会员金额/积分增加对应值

            //更改数据为已赠送
            data.State = state;
            CurrentRepository.Update(data);
            return true;
        }
    }
}
