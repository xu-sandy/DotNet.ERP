﻿using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Pharos.Logic.OMS.BLL
{
    public class AgentsRelationshipService : BaseService<AgentsRelationship>
    {
        [Ninject.Inject]
        // 代理商下级关系
        IBaseRepository<AgentsRelationship> AgentsRelationshipRepository { get; set; }

        /// <summary>
        /// 获取最大RelationshipId
        /// </summary>
        /// <returns></returns>
        public int getMaxRelationshipId()
        {
            return AgentsRelationshipRepository.GetQuery().Max(o => (int?)o.RelationshipId).GetValueOrDefault() + 1;
        }

        /// <summary>
        /// 增加或者修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult SaveOrUpdate(AgentsRelationship model)
        {
            if (model.Id == 0)
            {
                model.CreateTime = DateTime.Now;
                model.RelationshipId = getMaxRelationshipId();
                AgentsRelationshipRepository.Add(model);
            }
            else
            {
                var source = AgentsRelationshipRepository.Get(model.Id);
                model.ToCopyProperty(source, new List<string>() { "RelationshipId", "Status", "CreateTime" });
            }
            AgentsRelationshipRepository.SaveChanges();
            return OpResult.Success();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public OpResult Deletes(int[] ids)
        {
            var op = new OpResult();
            try
            {
                var AgentsRelationships = AgentsRelationshipRepository.GetQuery(o => ids.Contains(o.Id));
                if (!AgentsRelationships.Any())
                {
                    op.Message = "查不到数据";
                    return op;
                }
                AgentsRelationshipRepository.RemoveRange(AgentsRelationships.ToList());
                op.Successed = true;
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                LogEngine.WriteError(ex);
            }
            return op;
        }
    }
}
