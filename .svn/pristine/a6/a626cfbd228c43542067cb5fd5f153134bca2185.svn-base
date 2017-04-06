using Pharos.Logic.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.Sys.Entity;

namespace Pharos.Logic.BLL
{
    public class DeviceRegInfoService : BaseService<DeviceRegInfo>
    {
        public static object FindPageList(out int recordCount)
        {
            var query = (from d in CurrentRepository.QueryEntity
                         where d.CompanyId == Sys.SysCommonRules.CompanyId
                         select new
                         {
                             d.StoreId,
                             d.Id,
                             d.Type,
                             d.DeviceSN,
                             d.MachineSN,
                             Store = WarehouseService.CurrentRepository.QueryEntity.Where(o=>o.CompanyId==d.CompanyId && o.StoreId==d.StoreId).Select(o=>o.Title).FirstOrDefault(),
                             d.CreateDT,
                             d.State,
                             Auditor = UserInfoService.CurrentRepository.QueryEntity.Where(o=>o.CompanyId==d.CompanyId && o.UID==d.AuditorUID).Select(o=>o.FullName).FirstOrDefault(),
                             d.Memo
                         }).ToList();
            recordCount = query.Count;
            return query.OrderBy(a => a.CreateDT);
        }

        public static object FindPageListByWhere(int machineType, string store, int status, string keyword, out int recordCount)
        {
            //var query = (from d in CurrentRepository.QueryEntity
            //             join w in WarehouseService.CurrentRepository.QueryEntity on d.StoreId equals w.StoreId into storeTemp
            //             from wt in storeTemp.DefaultIfEmpty()
            //             join u in UserInfoService.CurrentRepository.QueryEntity on d.AuditorUID equals u.UID into userTemp
            //             from ut in userTemp.DefaultIfEmpty()
            //             where d.CompanyId == Sys.SysCommonRules.CompanyId
            //             select new
            //             {
            //                 d.StoreId,
            //                 d.Id,
            //                 d.Type,
            //                 d.DeviceSN,
            //                 d.MachineSN,
            //                 Store = wt == null ? "" : wt.Title,
            //                 d.CreateDT,
            //                 d.State,
            //                 Auditor = ut == null ? "" : ut.FullName,
            //                 d.Memo
            //             });
            string whereStr = " AND d.CompanyId =" + Sys.SysCommonRules.CompanyId;
            if (machineType != -1)
            {
                whereStr += " AND d.Type=" + machineType;
                //query = query.Where(d => d.Type == machineType);
            }
            if (!string.IsNullOrEmpty(store))
            {
                whereStr += " AND d.StoreId=" + store;
                //query = query.Where(d => d.StoreId == store);
            }
            if (status != -1)
            {
                whereStr += " AND d.State=" + status;
                //query = query.Where(d => d.State == status);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereStr += " AND ( d.DeviceSN like '%" + keyword + "%' or " + "d.Memo like '%" + keyword + "%')";
                //query = query.Where(d => d.DeviceSN.Contains(keyword) || d.Memo.Contains(keyword));
            }

            //var result = (from d in query
            //              select new
            //              {
            //                  d.StoreId,
            //                  d.Id,
            //                  d.Type,
            //                  d.DeviceSN,
            //                  d.MachineSN,
            //                  d.Store,
            //                  d.CreateDT,
            //                  d.State,
            //                  d.Auditor,
            //                  d.Memo
            //              }).ToList();
            //recordCount = result.Count;
            //return query.OrderBy(a => a.CreateDT).ToPageList();

            var query = CurrentRepository._context.Database.SqlQuery<CRM_DeviceInfo>(@"SELECT CAST(d.StoreId AS INT) StoreId,d.Id,d.Type,d.DeviceSN, CAST(d.MachineSN AS INT) MachineSN,(SELECT Title FROM dbo.Warehouse WHERE CompanyId=d.CompanyId and StoreId=d.StoreId) Store,d.CreateDT,d.State,s.FullName Auditor,d.Memo FROM dbo.DeviceRegInfo AS d
                                                                                       LEFT  JOIN dbo.SysUserInfo AS s ON d.AuditorUID=s.UID
                                                                                        WHERE 1=1 " + whereStr + " ORDER BY CAST(d.StoreId AS INT) DESC,CAST(MachineSN AS INT) ASC ").ToList();
            recordCount = query.Count;
            return query;
        }

        public static OpResult SetDevState(string ids, short state)
        {
            var sid = ids.Split(',').Select(o => int.Parse(o)).ToList();
            var list = FindList(o => sid.Contains(o.Id));
            if (state == 1)
            {
                var stores = list.Select(o => o.StoreId).ToList();
                var machines = list.Select(o => o.MachineSN).ToList();
                var devices = FindList(o =>o.CompanyId==CommonService.CompanyId && stores.Contains(o.StoreId) && machines.Contains(o.MachineSN) && !sid.Contains(o.Id) && o.State==1);
                devices.ForEach(o =>
                {
                    o.State = 0;
                });
            }
            list.ForEach(o =>
            {
                o.State = state;
                o.AuditorUID = Sys.CurrentUser.UID;
            });
            var op= Update(list);
            if (op.Successed)
            {
                var stores = string.Join(",", list.Select(o => o.StoreId));
                Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = list[0].CompanyId, StoreId = stores, Target = "DeviceRegInfo" });
            }
            return op;
        }

        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static OpResult SetMemo(int id, string memo)
        {
            var list = CurrentRepository.Find(o => o.Id == id);
            list.Memo = memo;
            return Update(list);
        }


    }
}
