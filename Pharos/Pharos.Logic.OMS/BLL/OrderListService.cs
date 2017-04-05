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
    /// <summary>
    /// BLL-----采购意向清单
    /// </summary>
    public class OrderListService : BaseService<OrderList>
    {
        [Ninject.Inject]
        // 采购意向清单
        public IBaseRepository<OrderList> OrderListRepository { get; set; }

        public Utility.OpResult SaveOrUpdate(OrderList model)
        {
            model.CreateDT = DateTime.Now;
            model.CreateUID = CurrentUser.UID;
            if (model.Id == 0)
            {
                OrderListRepository.Add(model);
            }
            else
            {
                var source = OrderListRepository.Get(model.Id);
                model.ToCopyProperty(source, new List<string>() { "CreateDT", "CID", "CreateUID" });
            }

            if (OrderListRepository.SaveChanges())
            {
                LogEngine.WriteUpdate(model.Id + "," + model.Title, LogModule.采购意向清单);
            }
            return OpResult.Success();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CID">企业ID</param>
        /// <returns></returns>
        public OpResult Deletes(int CID)
        {
            var list = OrderListRepository.GetQuery(o => o.CID==CID).ToList();
            OrderListRepository.RemoveRange(list);
            return OpResult.Success();
        }
    }
}
