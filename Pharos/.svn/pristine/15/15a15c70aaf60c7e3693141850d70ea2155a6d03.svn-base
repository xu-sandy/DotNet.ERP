using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.DAL;
using Pharos.Logic.Entity;
using Pharos.Utility;

namespace Pharos.Logic.BLL
{
    public class ScaleSettingsBLL
    {
        private readonly ScaleSettingsService service = new ScaleSettingsService();

        public List<ScaleSettings> FindPageList(out int count)
        {
            return service.FindPageListByCID(CommonService.CompanyId, out count);
        }

        public OpResult CreateSetting(ScaleSettings setting)
        {
            if (string.IsNullOrEmpty(setting.IpAddress))
            {
                return OpResult.Fail("IP地址数据异常！");
            }
            var r = ScaleSettingsService.IsExist(p => p.IpAddress == setting.IpAddress && p.CompanyId == Sys.SysCommonRules.CompanyId && p.Store == Sys.SysCommonRules.CurrentStore);
            if (r)
            {
                return OpResult.Fail("IP地址重复！");
            }
            setting.CreateDt = DateTime.Now;
            setting.CreateUID = Sys.CurrentUser.UID;
            setting.CompanyId = Sys.SysCommonRules.CompanyId;
            setting.Store = Sys.SysCommonRules.CurrentStore;
            return ScaleSettingsService.Add(setting);
        }

        public List<ScaleSettings> GetAllSettingsByStore()
        {
            var store = Sys.SysCommonRules.CurrentStore;
            var datas = ScaleSettingsService.FindList((o) => o.CompanyId == Sys.SysCommonRules.CompanyId && o.Store == store);
            return datas;
        }

        public OpResult RemoveSetting(int id)
        {
            var data = ScaleSettingsService.FindById(id);
            if (data == null)
            {
                return OpResult.Fail("未找到对应数据！");
            }
            else
            {
                return ScaleSettingsService.Delete(data);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public ScaleSettings GetSettingByIp(string ip)
        {
            return ScaleSettingsService.Find(o => o.IpAddress == ip);
        }
    }
}
