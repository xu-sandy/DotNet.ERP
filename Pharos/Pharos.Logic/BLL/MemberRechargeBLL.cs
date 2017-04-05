﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Infrastructure.Data.Redis;
using Pharos.ObjectModels.Events;
using Pharos.Utility;

namespace Pharos.Logic.BLL
{
    public class MemberRechargeBLL
    {
        private readonly MemberRechargeService _service = new MemberRechargeService();
        /// <summary>
        /// add
        /// </summary>
        /// <param name="_memberRecharge"></param>
        /// <returns></returns>
        public OpResult CreateRecharge(Entity.MemberRecharge _memberRecharge)
        {
            try
            {
                //判断最低充值
                var checkData = (from a in BaseService<Entity.MembershipCard>.CurrentRepository.Entities
                                 where a.CardSN == _memberRecharge.CardId
                                 join b in BaseService<Entity.CardInfo>.CurrentRepository.Entities
                                 on a.CardTypeId equals b.CardTypeId
                                 select new
                                 {
                                     b.MinRecharge,
                                     b.CardType
                                 }).FirstOrDefault();
                if (checkData == null)
                {
                    return OpResult.Fail("卡类型异常！");
                }
                if (checkData.MinRecharge > _memberRecharge.RechargeAmount)
                {
                    return OpResult.Fail("该卡最低充值 " + checkData.MinRecharge + " 元！");
                }
                if (!checkData.CardType.Contains("1"))
                {
                    return OpResult.Fail("非储蓄卡不允许充值！");
                }
                //complete base info
                _memberRecharge.CompanyId = CommonService.CompanyId;
                _memberRecharge.CreateDT = DateTime.Now;
                _memberRecharge.CreateUID = Sys.CurrentUser.UID;
                _memberRecharge.RechargeSN = DateTime.Now.ToString("yyyyMMddHHmmss");    //TODO: rechage SN rule
                _memberRecharge.BeforeAmount = _memberRecharge.BeforeAmount;
                _memberRecharge.AfterAmount = (decimal)_memberRecharge.BeforeAmount + _memberRecharge.RechargeAmount;//计算充值后
                //16-12-19 门店增加 门店 机器 练习模式 区分
                _memberRecharge.MachineSN = "00";//00=后台充值
                _memberRecharge.StoreId = "00";
                _memberRecharge.IsTest = false;
                //_memberRecharge.RechargeGiftId = ruleId;//规则id
                #region 作废赠送
                ////统计充值赠送相关
                //if (stageNumber > 0)
                //{//分期赠送 
                //    var year = DateTime.Now.Year;
                //    var month = DateTime.Now.Month;
                //    var giveDate = Convert.ToDateTime(year + "-" + month + "-" + returnDT);

                //    for (int i = 0; i < stageNumber; i++)
                //    {
                //        Entity.RechargeGiftsStage _stage = new Entity.RechargeGiftsStage()
                //        {
                //            CompanyId = CommonService.CompanyId,
                //            CardId = _memberRecharge.CardId,
                //            RechargeSN = _memberRecharge.RechargeSN,
                //            RuleId = ruleId,
                //            GiftProject = giftProject,//赠送项目 1：钱 2：积分
                //            GiftValue = stageAvg,//每期返还
                //            GiftDate = giveDate.AddMonths(i + 1).ToShortDateString(),
                //            State = 0,//0：未赠送；1：已赠送；2：已失效
                //            CreateDT = DateTime.Now,
                //            CreateUID = Sys.CurrentUser.UID
                //        };
                //        BaseService<Entity.RechargeGiftsStage>.Add(_stage, false);
                //    }
                //    //触发更新
                //    BaseService<Entity.RechargeGiftsStage>.Update(new Entity.RechargeGiftsStage());
                //}
                //else
                //{//即时赠送 
                //    _memberRecharge.AfterAmount = _memberRecharge.GivenAmount;
                //} 
                #endregion
                //更新会员卡金额、积分信息
                var membershipCard = BaseService<Entity.MembershipCard>.CurrentRepository.Entities.Where(o => o.CardSN == _memberRecharge.CardId && o.CompanyId == CommonService.CompanyId).FirstOrDefault();
                if (membershipCard != null)
                {
                    membershipCard.ReChargeTotal += _memberRecharge.RechargeAmount;
                    //membershipCard.GiveTotal += _memberRecharge.GivenAmount;
                    membershipCard.Balance += _memberRecharge.RechargeAmount;
                    //membershipCard.Integer += _memberRecharge.PresentExp;
                }
                //new MembershipCardService().UpdateMembershipCard(membershipCard);

                var result = BaseService<Entity.MemberRecharge>.Add(_memberRecharge);
                if (result.Successed)
                    RedisManager.Publish("RechargeCompleted", new RechargeCompletedEvent()
                    {
                        CompanyId = _memberRecharge.CompanyId,
                        MemberId = membershipCard.MemberId,
                        ReceiveAmount = _memberRecharge.RechargeAmount,
                        SourceRecordId = _memberRecharge.Id.ToString(),
                        OperatorUid = Sys.CurrentUser.UID
                    });
                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// query data
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="keyWold"></param>
        /// <returns></returns>
        public object FindRechargePageList(DateTime? beginDate, DateTime? endDate, string keyWold, int keywordType, int selectType, out int count, ref object footer)
        {
            return _service.FindRechargePageList(beginDate, endDate, keyWold, keywordType,selectType, out count, ref footer);
        }
    }
}
