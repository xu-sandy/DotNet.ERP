﻿using Pharos.Logic.DAL;
using Pharos.Logic.Entity;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL
{
    /// <summary>
    /// 会员活动推送业务实现
    /// </summary>
    public class PushService : BaseService<Push>
    {
        static readonly PushDAL pushDal = new PushDAL();

        /// <summary>
        /// 获取推送记录，用于datagrid列表
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static object FindPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var query = CurrentRepository.QueryEntity.Where(o=>o.CompanyId==CommonService.CompanyId);
            var keyword = nvl["keyword"];
            if (!keyword.IsNullOrEmpty())
                query = query.Where(a => a.Content.Contains(keyword));
            recordCount = query.Count();

            var types = new CommonDic().GetDicList(DicType.推送方式);
            var result = query.ToPageList(nvl);
            foreach (var item in result)
            {
                List<string> typeStringArray = new List<string>();
                if (!item.Type.IsNullOrEmpty())
                {
                    var snArray = item.Type.Split(',');
                    foreach (var sn in snArray)
                    {
                        typeStringArray.Add(types.FirstOrDefault(t => t.DicSN == Convert.ToInt16(sn)) == null ? "其他" : types.FirstOrDefault(t => t.DicSN == Convert.ToInt16(sn)).Title);
                    }
                    item.Type = string.Join(",", typeStringArray.Select(t => t));
                }
            }
            return result;
        }

        public static System.Data.DataTable GetPushResult(string pushId, string typeStr, System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            return pushDal.GetPushResult(pushId, typeStr, nvl, out recordCount);
        }

        public static OpResult Save(Push model, List<int> typeList)
        {
            var type = string.Empty;
            if (typeList != null && typeList.Any())
            {
                if (typeList.Any(a => a == 0))
                    typeList.RemoveAll(a => a == 0);
                type = string.Join(",", typeList);
            }
            else
                return OpResult.Fail("必须选择至少一种推送方式！");

            if (model.Id == 0)
            {
                model.PushId = CommonRules.GUID;
                model.Type = type;
                model.CreateUID = Sys.CurrentUser.UID;
                model.CreateDT = DateTime.Now;
                model.CompanyId = CommonService.CompanyId;
                return Add(model);
            }
            else
            {
                var entity = FindById(model.Id);
                entity.Type = type;
                entity.State = model.State;
                entity.Content = model.Content;
                return Update(entity);
            }
        }

        public static OpResult Push(Push model, List<int> typeList)
        {
            var saveResult = Save(model, typeList);

            if (!typeList.Any())
                return OpResult.Fail("必须选择至少一种推送方式！");
            if (typeList.Any(a => a == 0))  //移除“全部”项
                typeList.RemoveAll(a => a == 0);

            var members = MembersService.GetList();
            try
            {
                foreach (var type in typeList)
                {
                    var membersOfType = members;
                    switch (type)
                    {
                        case 153: //手机号
                            membersOfType = membersOfType.Where(a => a.MobilePhone != null && a.MobilePhone != "").ToList();
                            break;
                        case 154: //Email
                            membersOfType = membersOfType.Where(a => a.Email != null && a.Email != "").ToList();
                            break;
                        //case 155:
                        //    membersOfType = membersOfType.Where(a => a.Weixin != null && a.Weixin != "").ToList();
                        //    break;
                    }

                    if (membersOfType.Any())
                    {
                        List<PushWithMember> pwms = new List<PushWithMember>();
                        foreach (var member in membersOfType)
                        {
                            var pwm = new PushWithMember()
                            {
                                PushId = model.PushId,
                                MemberId = member.MemberId,
                                Channel = Convert.ToInt16(type),
                                State = 0 //待推送
                            };
                            pwms.Add(pwm);
                        }
                        var count = pushDal.InsertPushWithMemberMappings(pwms);
                    }
                }
            }
            catch (Exception e)
            {
                return OpResult.Fail("处理异常！" + e.Message);
            }

            return saveResult;
        }

        public static OpResult Delete(int[] ids)
        {
            var query = CurrentRepository.QueryEntity.Where(a => ids.Contains(a.Id)).ToList();
            if (query.Any(a => a.State != 0))
                return OpResult.Fail("已推送的消息无法删除！");
            return Delete(query);
        }

        public static OpResult PushNow(int[] ids)
        {
            var query = CurrentRepository.QueryEntity.Where(a => ids.Contains(a.Id)).ToList();
            if (query.Any(a => a.Type == null || a.Type == ""))
                return OpResult.Fail("必须选择至少一种推送方式！");
            OpResult op = OpResult.Success();
            foreach (var item in query)
            {
                var typeList = item.Type.Split(',').Select(a => Convert.ToInt32(a)).ToList();
                item.State = 1;
                var singleResult = Push(item, typeList);
                if (!singleResult.Successed)
                {
                    op.Successed = false;
                    op.Message += singleResult.Message;
                }
            }
            return op;
        }
    }
}
