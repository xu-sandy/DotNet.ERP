using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;

namespace Pharos.Logic.BLL
{
    public class MembershipCardSettingBLL
    {
        private readonly MembershipCardSettingService _service = new MembershipCardSettingService();
        public MembershipCardSetting GetSettingByCompanyId()
        {
            return _service.GetSettingByCompanyId(CommonService.CompanyId);
        }

        public object CreateOrUpdate(MembershipCardSetting setting)
        {
            if (setting.Id == 0)
            {
                setting.CompanyId = CommonService.CompanyId;
                setting.CreateDT = DateTime.Now;
                setting.CreateUID = Sys.CurrentUser.UID;
                return MembershipCardSettingService.Add(setting);
            }
            else
            {
                return _service.UpdateSetting(setting);
            }
        }
    }
}
