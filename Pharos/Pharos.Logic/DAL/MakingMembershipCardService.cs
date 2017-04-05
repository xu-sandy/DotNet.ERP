﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Logic.Entity.Views;
using Pharos.Sys.Entity;
using Pharos.Utility;
using Pharos.Utility.Helpers;

namespace Pharos.Logic.BLL
{
    public class MakingMembershipCardService : BaseService<MakingMembershipCard>
    {
        public OpResult CreateMakingMembershipCard(MakingMembershipCard _shipCard)
        {
            return MakingMembershipCardService.Add(_shipCard);
        }

        public MakingMembershipCard GetShipCardById(int id)
        {
            return CurrentRepository.FindById(id);
        }
        public OpResult UpdateShipCard(MakingMembershipCard _shipcard)
        {
            try
            {

                var oldShipCard = CurrentRepository.FindById(_shipcard.Id);
                //if (oldShipCard == null || oldShipCard.State >= 0)
                //{
                //    return new OpResult() { Successed = false, Message = "改制卡批次不允许修改！" };
                //}
                oldShipCard.MakeNumber = _shipcard.MakeNumber;
                oldShipCard.CardTypeId = _shipcard.CardTypeId;
                oldShipCard.ExpiryStart = _shipcard.ExpiryStart;
                oldShipCard.ExpiryEnd = _shipcard.ExpiryEnd;
                //oldShipCard.NumberEnd = _shipcard.NumberEnd;
                //oldShipCard.NumberStart = _shipcard.NumberStart;
                oldShipCard.State = _shipcard.State;
                CurrentRepository.Update(oldShipCard);
                return new OpResult() { Successed = true, Message = "操作成功！" };
            }
            catch (Exception e)
            {
                Log.WriteError(e);

                return new OpResult() { Successed = false, Message = e.Message };
            }
        }

        public object GetMakingMembershipCardByWhere(string cardType, string batchSN, short? state, string createUID, DateTime? beginDate, DateTime? endDate, out int count)
        {
            var query = CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId);
            if (!string.IsNullOrEmpty(cardType))
            {
                query = query.Where(o => o.CardTypeId == cardType);
            }
            if (!string.IsNullOrEmpty(batchSN))
            {
                query = query.Where(o => o.BatchSN == batchSN);
            }
            if (state != -1)
            {
                query = query.Where(o => o.State == state);
            }
            if (!string.IsNullOrEmpty(createUID))
            {
                query = query.Where(o => o.CreateUID == createUID);
            }
            if (beginDate != null)
            {
                query = query.Where(o => o.CreateDT > beginDate);
            }
            if (endDate != null)
            {
                endDate = endDate.Value.AddDays(1);
                query = query.Where(o => o.CreateDT < endDate);
            }
            count = query.Count();
            if (count > 0)
            {
                var result = (from a in query
                              join b in BaseService<SysUserInfo>.CurrentRepository.Entities
                              on a.CreateUID equals b.UID into l
                              from def in l.DefaultIfEmpty()
                              join c in BaseService<CardInfo>.CurrentRepository.Entities
                              on a.CardTypeId equals c.CardTypeId
                              select new MakeingMembershipCardViewModel
                              {
                                  Id = a.Id,
                                  BatchSN = a.BatchSN,
                                  CardName = c.CardName,
                                  NumberStart = a.NumberStart,
                                  NumberEnd = a.NumberEnd,
                                  MakeNumber = a.MakeNumber,
                                  State = a.State,
                                  ExpiryStart = a.ExpiryStart,
                                  ExpiryEnd = a.ExpiryEnd,
                                  CreateDT = a.CreateDT,
                                  CreateUID = l.FirstOrDefault() == null ? "" : l.FirstOrDefault().FullName,
                                  CardType = c.Category,
                                  AllRechange = c.CardType.Contains("1") ? "是" : "否",
                                  MinRecharge = c.MinRecharge,
                                  LeadCount = BaseService<MembershipCard>.CurrentRepository.QueryEntity.Where(o => o.BatchSN == a.BatchSN && o.CompanyId == a.CompanyId && o.LeadTime != null).Count()
                                  //IntegrationType = c.IntegrationType
                              }).ToPageList();
                return result;

            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<MakingMembershipCard> GetMakingMembershipCardByIds(string ids)
        {
            var idARR = ids.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(o => Convert.ToInt32(o));
            return CurrentRepository.Entities.Where(o => idARR.Contains(o.Id)).ToList();
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public OpResult UpdateState(short state, string ids, bool isSave = true)
        {
            try
            {
                var query = GetMakingMembershipCardByIds(ids);
                //query.Each(o => o.State = state);
                //var top1MemberCard = BaseService<MembershipCard>.CurrentRepository.Entities.OrderByDescending(o => o.CreateDT).FirstOrDefault();
                //var oldCardNo = top1MemberCard.CardSN;
                //var currentCardNo = string.Empty;
                //int num = 0;
                //if (top1MemberCard != null && top1MemberCard.CardSN.Length > 6)
                //{
                //    oldCardNo = top1MemberCard.CardSN.Substring(top1MemberCard.CardSN.Length - 7, 7);
                //    int.TryParse(oldCardNo, out num);
                //}
                query.ForEach(o => o.State = state);
                #region MyRegion
                //foreach (var item in query)
                //{
                //    if (state == 1 && item.State != 1)
                //    {
                //        for (int i = 0; i < item.MakeNumber; i++)
                //        {
                //            num++;
                //            currentCardNo = num.ToString();
                //            while (currentCardNo.Length < 7)
                //            {
                //                currentCardNo = currentCardNo.Insert(0, "0");
                //            }
                //            //生成卡号：1位卡类型（1=固定面值；2=充值卡） 4位区号 7位商户号 7位序列号（港澳6位）
                //            var cardSn = Convert.ToInt32(item.CardType) + "3640" + "1000101" + currentCardNo;

                //            var securitycode = ExtendHelper.GetRandomStr(7);
                //            var isExit = CurrentRepository.Entities.Any(o => o.SecurityCode == securitycode);
                //            while (isExit)
                //            {
                //                securitycode = ExtendHelper.GetRandomStr(7);
                //                isExit = CurrentRepository.Entities.Any(o => o.SecurityCode == securitycode);
                //            }

                //            MembershipCard _memberCard = new MembershipCard()
                //            {
                //                CompanyId = CommonService.CompanyId,
                //                BatchSN = item.BatchSN,
                //                CardSN = cardSn,
                //                CardType = item.CardType,
                //                ReChargeTotal = 0,
                //                GiveTotal = 0,
                //                Balance = 0,
                //                Integer = 0,
                //                Deposit = 0,
                //                State = 0,
                //                ExpiryStart = item.ExpiryStart,
                //                ExpiryEnd = item.ExpiryEnd,
                //                CreateDT = DateTime.Now,
                //                CreateUID = Sys.CurrentUser.UID,
                //                SecurityCode = securitycode
                //            };

                //            BaseService<MembershipCard>.CurrentRepository.Add(_memberCard, false);
                //        }
                //    }
                //    //else
                //    //{
                //    //    return new OpResult() { Successed = false, Message = "该批次已制卡！" };
                //    //}
                //    item.State = state;
                //} 
                #endregion

                CurrentRepository.Update(new MakingMembershipCard(), isSave);
                return new OpResult() { Successed = true, Message = "操作成功！" };
            }
            catch (Exception e)
            {
                Log.WriteError(e);
                throw e;
            }
        }
        /// <summary>
        /// 取最大批次信息
        /// </summary>
        /// <returns></returns>
        public MakingMembershipCard GetFirstMakingMemberShipCard()
        {
            var date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            return CurrentRepository.Entities.Where(o => o.CreateDT > date).OrderByDescending(o => o.CreateDT).FirstOrDefault();
        }
        //取所有制卡批次
        public IEnumerable<MakingMembershipCard> GetBatchsToDropDown()
        {
            return CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId).Distinct();
        }
        /// <summary>
        /// 取批次去重创建人
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SysUserInfo> GetCreateUserToDropDown()
        {
            try
            {
                return (from a in CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId)
                        join b in BaseService<SysUserInfo>.CurrentRepository.Entities
                        on a.CreateUID equals b.UID
                        select b).Distinct().ToList();
            }
            catch (Exception e)
            {
                Log.WriteError(e);
                throw;
            }
        }
    }
}
