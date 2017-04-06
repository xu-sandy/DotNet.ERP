using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using Pharos.Logic.DAL;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services
{
    public class DeviceRegInfoService : BaseGeneralService<Pharos.Logic.Entity.DeviceRegInfo, EFDbContext>
    {
        public static void RegisterDevice(string storeId, string machineSn, string deviceSn, DeviceType type, int companyId)
        {
            var entity = new DeviceRegInfo()
            {
                CreateDT = DateTime.Now,
                DeviceSN = deviceSn,
                MachineSN = machineSn,
                State = 0,
                StoreId = storeId,
                Type = (short)type,
                CompanyId = companyId
            };
            CurrentRepository.Add(entity);
            RedisManager.Publish<DeviceRegInfo>("NewDeviceRegInfo", entity);

        }

        public static bool HasRegister(string storeId, string machineSN, string deviceSn, DeviceType type, int companyId, bool verfyState = true)
        {
            var deviceType = (short)type;
            return CurrentRepository.IsExist(o => o.MachineSN == machineSN && ((o.State == 1 && verfyState) || !verfyState) && o.StoreId == storeId && o.Type == deviceType && o.DeviceSN == deviceSn && o.CompanyId == companyId);
        }
    }
}
