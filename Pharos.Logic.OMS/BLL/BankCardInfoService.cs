using Pharos.Logic.OMS.DAL;
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
    public class BankCardInfoService : BaseService<BankCardInfo>
    {
        [Ninject.Inject]
        // 结算账户信息
        public IBaseRepository<BankCardInfo> bankCardInfoRepository { get; set; }

        /// <summary>
        /// 获取最大代理商编号或商家编号
        /// </summary>
        /// <returns></returns>
        public int getMaxOrgId()
        {
            return bankCardInfoRepository.GetQuery().Max(o => (int?)o.CardId).GetValueOrDefault() + 1;
        }

        /// <summary>
        /// 增加或者修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult SaveOrUpdate(BankCardInfo model)
        {
            //删除
            //if (model.Type == 1)
            //{
            //    Delete(model.AgentsId, model.Type);
            //}
            
            if (model.Id == 0)
            {
                model.CreateUid = CurrentUser.UID;
                model.CreateTime = DateTime.Now;
                model.AuditTime = DateTime.Now;
                model.CardId = getMaxOrgId();
                bankCardInfoRepository.Add(model);
            }
            else
            {
                var source = bankCardInfoRepository.Get(model.Id);
                if (model.AuditTime == DateTime.MinValue)
                {
                    model.AuditTime = source.AuditTime;
                }
                model.ToCopyProperty(source, new List<string>() { "OrgId", "Type", "CreateUid", "CreateTime" });
            }
            bankCardInfoRepository.SaveChanges();
            return OpResult.Success();
        }

        public BankCardInfo GetOneById(object id)
        {
            return bankCardInfoRepository.Get(id);
        }

        public BankCardInfo GetOne(int AgentsId)
        {
            return bankCardInfoRepository.GetQuery(o=>o.AgentsId==AgentsId&&o.Type==1).FirstOrDefault();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public bool Delete(int AgentsId,int type)
        {
            var BankCardInfos = bankCardInfoRepository.GetQuery(o => o.AgentsId==AgentsId&&o.Type==type);
            if (BankCardInfos.Any())
            {
                bankCardInfoRepository.RemoveRange(BankCardInfos.ToList());
                return true;
            }
            return false;
        }
    }
}
