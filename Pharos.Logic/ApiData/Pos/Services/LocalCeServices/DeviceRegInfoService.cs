using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services.LocalCeServices
{
    public class DeviceRegInfoService : BaseGeneralService<DeviceRegInfo, LocalCeDbContext>
    {
        public static void RegisterDevice(string storeId, string machineSn, string deviceSn, DeviceType type, int companyId)
        {
            var version = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

            CurrentRepository.Add(new DeviceRegInfo()
            {
                CreateDT = DateTime.Now,
                DeviceSN = deviceSn,
                MachineSN = machineSn,
                State = 0,
                StoreId = storeId,
                Type = (short)type,
                CompanyId = companyId,
                SyncItemId = Guid.NewGuid(),
                SyncItemVersion = version
            });
           // RedisManager.Publish("SyncDatabase", "DeviceRegInfo");
            StoreManager.PubEvent("SyncDatabase", "DeviceRegInfo");
        }

        public static bool HasRegister(string storeId, string machineSN, string deviceSn, DeviceType type, int companyId, bool verfyState = true)
        {
            var deviceType = (short)type;
            return CurrentRepository.IsExist(o => o.MachineSN == machineSN && ((o.State == 1 && verfyState) || !verfyState) && o.StoreId == storeId && o.Type == deviceType && o.DeviceSN == deviceSn && o.CompanyId == companyId);
        }
    }
}
