﻿using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Entity.View;
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
    /// BLL-----商户分类
    /// </summary>
    public class TraderTypeService : BaseService<TraderType>
    {
        [Ninject.Inject]
        // 商户分类
        public IBaseRepository<TraderType> TraderTypeRepository { get; set; }

        [Ninject.Inject]
        // 商户资料
        IBaseRepository<Traders> TradersRepository { get; set; }

        public Utility.OpResult SaveOrUpdate(TraderType model)
        {
            model.CreateDT = DateTime.Now;
            model.CreateUID = CurrentUser.UID;
            if (model.Id == 0)
            {
                model.TraderTypeId = CommonService.GUID.ToUpper();
                TraderTypeRepository.Add(model);
            }
            else
            {
                var source = TraderTypeRepository.Get(model.Id);
                model.ToCopyProperty(source);
            }

            if (TraderTypeRepository.SaveChanges())
            {
                LogEngine.WriteUpdate(model.Id + "," + model.Title, LogModule.商户分类);
            }
            return OpResult.Success();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public Utility.OpResult Delete(string TraderTypeId)
        {
            var op = new OpResult();
            try
            {
                var Traders = TradersRepository.GetQuery(o => o.TraderTypeId == TraderTypeId.ToUpper());
                if (Traders.Any())
                {
                    op.Message = "无法删除，该类别已经在商户中使用";
                    return op;
                }
                var type = TraderTypeRepository.GetQuery(o => o.TraderTypeId == TraderTypeId);
                TraderTypeRepository.RemoveRange(type.ToList());
                op.Successed = true;
                LogEngine.WriteDelete("删除商户分类", LogModule.商户分类);
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                LogEngine.WriteError(ex);
            }
            return op;
        }

        public List<ViewTraderType> getList()
        {
            var queryTrader = TradersRepository.GetQuery();
            var queryType = TraderTypeRepository.GetQuery();
            var query = from x in queryType
                        let o = from y in queryTrader where y.TraderTypeId == x.TraderTypeId select y
                        select new ViewTraderType
                        {
                            TraderTypeId = x.TraderTypeId,
                            Title = x.Title,
                            Count = o.Count()
                        };
            var list = query.ToList();
            return list;
        }
    }
}
