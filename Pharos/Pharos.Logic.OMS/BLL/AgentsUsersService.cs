﻿using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Pharos.Logic.OMS.BLL
{
    public class AgentsUsersService : BaseService<AgentsUsers>
    {

        [Ninject.Inject]
        // 代理商账号
        public IBaseRepository<AgentsUsers> agentsUsersRepository { get; set; }


        //代理商档案BLL
        [Ninject.Inject]
        IBaseRepository<AgentsInfo> agentsInfoRepository { get; set; }

        // 代理商账号
        [Ninject.Inject]
        IAgentsUsersRepository uRepository { get; set; }

        public OpResult SaveOrUpdate(AgentsUsers model)
        {
            IQueryable<AgentsUsers> isExist = null;
            if (model.Id == 0)
            {
                isExist = agentsUsersRepository.GetQuery(o => o.LoginName.Trim() == model.LoginName.Trim());
            }
            else
            {
                isExist = agentsUsersRepository.GetQuery(o => o.LoginName.Trim() == model.LoginName.Trim() && o.Id != model.Id);
            }
            if (isExist.Any())
            {
                return OpResult.Fail("该登录账号已经存在");
            }

            if (model.AgentType == 1)
            {
                IQueryable<AgentsUsers> isExistPrimaryAccount = null;
                if (model.Id == 0)
                {
                    isExistPrimaryAccount = agentsUsersRepository.GetQuery(o => o.AgentsId == model.AgentsId && o.AgentType == 1);
                }
                else
                {
                    isExistPrimaryAccount = agentsUsersRepository.GetQuery(o => o.AgentsId == model.AgentsId && o.AgentType == 1 && o.Id != model.Id);
                }
                if (isExistPrimaryAccount.Any())
                {
                    return OpResult.Fail("该代理商已经存在主账号");
                }
            }

            if (!model.FullName.IsNullOrEmpty())
            {
                //汉字转换拼音
                model.QuanPin = Tool.ToPinYin(model.FullName.Trim());
            }
            model.LoginPwd = Pharos.Utility.Security.MD5_Encrypt(model.LoginPwd);
            if (model.Id == 0)
            {
                model.CreateTime = DateTime.Now;
                model.AgentsLoginId = CommonService.GUID.ToUpper();
                agentsUsersRepository.Add(model);
            }
            else
            {
                var source = agentsUsersRepository.Get(model.Id);
                var pwd = source.LoginPwd;
                model.ToCopyProperty(source, new List<string>() { "LoginPwd", "AgentsLoginId", "CreateTime" });
                if (!model.LoginPwd.IsNullOrEmpty() && pwd != model.LoginPwd)
                    source.LoginPwd = model.LoginPwd;
                agentsUsersRepository.SaveChanges();
            }
            return OpResult.Success();
        }

        public AgentsUsers GetOne(object id)
        {
            return agentsUsersRepository.Get(id);
        }

        /// <summary>
        /// 获取代理商账号
        /// </summary>
        /// <returns></returns>
        public List<AgentsUsers> GetAgentsUsersList(Expression<Func<AgentsUsers, bool>> whereLambda = null)
        {
            if (whereLambda != null)
            {
                return agentsUsersRepository.GetQuery(whereLambda).ToList();
            }
            return agentsUsersRepository.GetQuery().ToList();
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="AgentsId">代理商编号</param>
        /// <returns></returns>
        public OpResult Verification(string AgentsId)
        {
            if (!Tool.IsNumber(AgentsId))
            {
                return OpResult.Fail("代理商编号格式错误！");
            }
            //上级代理商代理商编号是否存在
            var aid= Convert.ToInt32(AgentsId);
            if (!agentsInfoRepository.GetQuery(o => o.AgentsId == aid).Any())
            {
                return OpResult.Fail("代理商编号不存在");
            }
            return OpResult.Success();
        }

        public List<AgentsUsers> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            //代理商编号
            var AgentsId = (nvl["AgentsId"] ?? "").Trim();
            //姓名
            var FullName = (nvl["FullName"] ?? "").Trim();
            //登录账号
            var LoginName = (nvl["LoginName"] ?? "").Trim();
            //账号类型
            var AgentType = (nvl["AgentType"] ?? "").Trim();

            var pageIndex = 1;
            var pageSize = 20;
            if (!nvl["page"].IsNullOrEmpty())
                pageIndex = int.Parse(nvl["page"]);
            if (!nvl["rows"].IsNullOrEmpty())
                pageSize = int.Parse(nvl["rows"]);

            string strw = "";

            if (!AgentsId.IsNullOrEmpty())
            {
                if (!Tool.IsNumber(AgentsId) || AgentsId.Length > 6)
                {
                    AgentsId = "0";
                }
                strw = strw + " and u.AgentsId like '%"+AgentsId+"%'";
            }

            if (!FullName.IsNullOrEmpty())
            {
                strw = strw + " and u.FullName like '%" + FullName + "%'";
            }

            if (!LoginName.IsNullOrEmpty())
            {
                strw = strw + " and u.LoginName like '%" + LoginName + "%'";
            }

            if (!AgentType.IsNullOrEmpty())
            {
                strw = strw + " and u.AgentType=" + AgentType;
            }

            List<AgentsUsers> list = uRepository.getPageList(pageIndex, pageSize, strw, out recordCount);
            return list;
        }

        /// <summary>
        /// 将所选项设为
        /// </summary>
        public OpResult upStatus(string ids, int Status)
        {
            var sId = ids.Split(',').Select(o => int.Parse(o));
            var olist = agentsUsersRepository.GetQuery(o => sId.Contains(o.Id)).ToList();
            olist.Each(o => o.Status = Status);
            return OpResult.Result(agentsUsersRepository.SaveChanges());
        }

    }
}
