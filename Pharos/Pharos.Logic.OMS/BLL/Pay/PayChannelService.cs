﻿using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using QCT.Pay.Common;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// 金融接口-收单渠道信息
    /// </summary>
    public class PayChannelService
    {
        [Ninject.Inject]
        IBaseRepository<PayChannelManage> PCManageRepost { get; set; }
        [Ninject.Inject]
        IBaseRepository<PayChannelDetail> PCDetailRepost { get; set; }
        [Ninject.Inject]
        IBaseRepository<SysUser> UserRepository { get; set; }
        /// <summary>
        /// 获取收单渠道信息分页数据
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetPaging(System.Collections.Specialized.NameValueCollection nvl, out int totalCount)
        {
            var pms = new
            {
                KeyValues = nvl["KeyValues"] == null ? "" : nvl["KeyValues"].Trim(),
                //State = nvl["State"] == null ? new List<short>() : nvl["State"].Split(',').Select(o => short.Parse(o)).ToList()
                ChannelNo = nvl["ChannelNo"] == null ? 0 : nvl["ChannelNo"].ToType<int>()
            };
            var query = PCManageRepost.GetQuery().Where(o =>
                o.ChannelCode.Contains(pms.KeyValues) || o.ChannelTitle.Contains(pms.KeyValues) || o.Memo.Contains(pms.KeyValues));
            if (pms.ChannelNo > 0)
            {
                query = query.Where(o => o.ChannelNo == pms.ChannelNo);
            }
            //if (pms.State.Count > 0)
            //    query = query.Where(o => pms.State.Contains(o.State));
            //else {
            //    query = query.Where(o => o.State != (short)PayChannelState.Cancel);
            //}
            query = from pm in query
                    join jpd in PCDetailRepost.GetQuery(o=>o.IsDeleted==false) on pm.ChannelNo equals jpd.ChannelNo into ipd
                    from pd in ipd.DefaultIfEmpty()
                    join juc in UserRepository.GetQuery() on pm.CreateUID equals juc.UserId into iuc
                    from uc in iuc.DefaultIfEmpty()
                    join jua in UserRepository.GetQuery() on pm.AuditUID equals jua.UserId into iua
                    from ua in iua.DefaultIfEmpty()
                    select new PayChannelManageExt()
                    {
                        Id = pm.Id,
                        ChannelNo = pm.ChannelNo,
                        ChannelCode = pm.ChannelCode,
                        ChannelTitle = pm.ChannelTitle,
                        State = pm.State,
                        Memo = pm.Memo,
                        AuditDT = pm.AuditDT,
                        AuditUID = pm.AuditUID,
                        Auditer = ua.FullName,
                        CreateDT = pm.CreateDT,
                        CreateUID = pm.CreateUID,
                        Creater = uc.FullName,
                        ChannelDetailId = pd.ChannelDetailId,
                        ChannelPayMode = pd.ChannelPayMode,
                        OptType = pd.OptType,
                        MonthFreeTradeAmount = pd.MonthFreeTradeAmount,
                        OverServiceRate = pd.OverServiceRate,
                        SingleServFeeLowLimit = pd.SingleServFeeLowLimit,
                        SingleServFeeUpLimit = pd.SingleServFeeUpLimit
                    };
            totalCount = query.Count();
            return query.ToPageList();
        }
        /// <summary>
        /// 根据ID获得收到渠道信息Model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PayChannelManageExt GetOne(int id)
        {
            var obj = (from upay in PCManageRepost.GetQuery(o => o.Id == id)
                       join juc in UserRepository.GetQuery() on upay.CreateUID equals juc.UserId into iuc
                       from uc in iuc.DefaultIfEmpty()
                       join jua in UserRepository.GetQuery() on upay.AuditUID equals jua.UserId into iua
                       from ua in iua.DefaultIfEmpty()
                       select new PayChannelManageExt()
                       {
                           Id = upay.Id,
                           ChannelNo = upay.ChannelNo,
                           ChannelCode = upay.ChannelCode,
                           ChannelTitle = upay.ChannelTitle,
                           State = upay.State,
                           Memo = upay.Memo,
                           AuditDT = upay.AuditDT,
                           AuditUID = upay.AuditUID,
                           Auditer = ua.FullName,
                           CreateDT = upay.CreateDT,
                           CreateUID = upay.CreateUID,
                           Creater = uc.FullName
                       }).FirstOrDefault();
            if (obj == null)
                obj = new PayChannelManageExt();
            return obj;
        }
        /// <summary>
        /// 提供支付接口下拉数据（状态为可用的）
        /// </summary>
        /// <returns></returns>
        public List<DropdownItem> GetPayChannelsForPayApi()
        {
            var result = new List<DropdownItem>();
            var query = from pc in PCManageRepost.GetQuery()
                        where pc.State == (short)PayChannelState.Enabled
                        select new { Text = pc.ChannelCode, Value = pc.ChannelNo };
            var data = query.ToList();
            if (data != null && data.Count > 0)
            {
                data.ForEach(o => { result.Add(new DropdownItem() { Text = o.Text, Value = o.Value.ToString() }); });
            }
            return result;
        }
        /// <summary>
        /// 提供给交易流水数据页面的支付通道下拉数据
        /// </summary>
        /// <returns></returns>
        public List<DropdownItem> GetPayChannelsForTrades()
        {
            var result = new List<DropdownItem>();
            var query = from pc in PCManageRepost.GetQuery()
                        where pc.State == (short)PayChannelState.Enabled || pc.State == (short)PayChannelState.Disabled
                        select new { Text = pc.ChannelCode, Value = pc.ChannelNo };
            var data = query.ToList();
            if (data != null && data.Count > 0)
            {
                data.ForEach(o => { result.Add(new DropdownItem() { Text = o.Text, Value = o.Value.ToString() }); });
            }
            return result;
        }
        /// <summary>
        /// 设置收单渠道信息状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public OpResult SetState(int id, short state)
        {
            var entity = PCManageRepost.GetQuery(o => o.Id == id).FirstOrDefault();
            if (entity != null)
            {
                entity.State = state;
                entity.AuditDT = DateTime.Now;
                entity.AuditUID = CurrentUser.UID;
                return OpResult.Result(PCManageRepost.SaveChanges());
            }
            else
            {
                return OpResult.Fail("所选项状态已失效！");
            }
        }
        /// <summary>
        /// 设置注销数据（注销逻辑todo：dddd）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public OpResult CancelPayChannel(int id)
        {
            var entity = PCManageRepost.GetQuery(o => o.Id == id && (o.State == (short)PayChannelState.NotAuditing || o.State == (short)PayChannelState.Disabled)).FirstOrDefault();
            if (entity != null)
            {
                if (entity.State == (short)PayChannelState.NotAuditing)
                { //如果状态为0未审核则直接删除
                    PCManageRepost.Remove(entity, true);
                    return OpResult.Result(true);
                }
                else
                {//如果状态为2停用则逻辑删除
                    entity.State = (short)PayChannelState.Cancel;
                    entity.AuditDT = DateTime.Now;
                    entity.AuditUID = CurrentUser.UID;
                    return OpResult.Result(PCManageRepost.SaveChanges());
                }
            }
            else
            {
                return OpResult.Fail("所选项状态已失效！");
            }
        }
        /// <summary>
        /// 保存或更新收单渠道信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult SaveOrUpdate(PayChannelManage model)
        {
            var source = model;
            var existsObj = PCManageRepost.GetQuery(o => o.Id != model.Id && o.ChannelCode == model.ChannelCode).FirstOrDefault();
            if (existsObj != null)
                return OpResult.Fail(message: "收单渠道已经存在，不可重复");
            if (source.Id > 0)
            {
                source = PCManageRepost.GetQuery(o => o.Id == model.Id).FirstOrDefault();
                model.ToCopyProperty(source, new List<string>() { "Id", "ChannelNo", "ChannelCode", "State", "CreateDT", "CreateUID", "AuditDT", "AuditUID" });
            }
            else
            {
                var existsCodeObj = PCManageRepost.GetQuery(o => o.ChannelCode == model.ChannelCode).FirstOrDefault();
                if (existsCodeObj != null)
                    return OpResult.Fail(message: "收单渠道已经存在，不可重复");

                source.CreateDT = DateTime.Now;
                source.CreateUID = CurrentUser.UID;
                source.ChannelNo = PayRules.GetMaxNo("PayChannelManages", "ChannelNo");
                PCManageRepost.Add(source, false);
            }

            var result = PCManageRepost.SaveChanges();
            if (result)
                return OpResult.Success(data: source);
            else
                return OpResult.Fail(message: "数据未修改，不可保存");
        }
        /// <summary>
        /// 获取服务费率设置表格数据
        /// </summary>
        /// <param name="nvl"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetPayChannelDetail(System.Collections.Specialized.NameValueCollection nvl)
        {
            var pms = new
            {
                IsDeleted = nvl["LogData"] == null ? false : true,
                ChannelNo = nvl["ChannelNo"] == null ? 0 : nvl["ChannelNo"].ToType<int>()
            };
            var query = from pd in PCDetailRepost.GetQuery(o => o.ChannelNo == pms.ChannelNo && o.IsDeleted == pms.IsDeleted)
                        join jua in UserRepository.GetQuery() on pd.CreateUID equals jua.UserId into iua
                        from ua in iua.DefaultIfEmpty()
                        select new
                        {
                            Id = pd.Id,
                            ChannelNo = pd.ChannelNo,
                            ChannelDetailId = pd.ChannelDetailId,
                            ChannelPayMode = pd.ChannelPayMode,
                            OptType = pd.OptType,
                            OverServiceRate = pd.OverServiceRate,
                            MonthFreeTradeAmount = pd.MonthFreeTradeAmount,
                            SingleServFeeLowLimit = pd.SingleServFeeLowLimit,
                            SingleServFeeUpLimit = pd.SingleServFeeUpLimit,
                            EffectiveDate = pd.EffectiveDate,
                            CreateDT = pd.CreateDT,
                            CreateUID = pd.CreateUID,
                            Creater = ua.FullName
                        };
            return query.ToPageList();
        }
        /// <summary>
        /// 是否允许添加收单细目类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult CanAddChannelDetail(PayChannelDetail model)
        {
            var existsObj = PCDetailRepost.GetQuery(o => o.ChannelNo == model.ChannelNo && o.ChannelPayMode == model.ChannelPayMode && o.IsDeleted == false).FirstOrDefault();
            if (existsObj != null)
                return OpResult.Fail(message: "所选支付方式的收单渠道已经存在，不可重复");
            else
                return OpResult.Success();
        }
        /// <summary>
        /// 保存收单渠道细目-服务费率设置保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult SaveChannelDetail(PayChannelDetail model)
        {
            model.ChannelDetailId = CommonService.GUID;
            model.CreateDT = DateTime.Now;
            model.CreateUID = CurrentUser.UID;
            PCDetailRepost.Add(model, false);

            var result = PCDetailRepost.SaveChanges();
            if (result)
                return OpResult.Success(data: model);
            else
                return OpResult.Fail(message: "保存失败");
        }
        /// <summary>
        /// 删除服务费率
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult RemoveDetailById(PayChannelDetail model)
        {
            var data = PCDetailRepost.GetQuery(o => o.ChannelDetailId == model.ChannelDetailId && o.IsDeleted == false).FirstOrDefault();
            if (data != null)
            {
                data.IsDeleted = true;
                var result = PCDetailRepost.SaveChanges();
                if (result)
                    return OpResult.Success(data: data);
                else
                    return OpResult.Fail(message: "数据未修改，不可保存");
            }
            else
            {
                return OpResult.Fail(message: "所选中数据已失效");
            }
        }
    }
}
