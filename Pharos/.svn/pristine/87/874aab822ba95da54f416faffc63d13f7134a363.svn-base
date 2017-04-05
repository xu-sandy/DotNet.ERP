using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Utility;

namespace Pharos.Logic.BLL
{
    public class MembershipCardSettingService : BaseService<MembershipCardSetting>
    {
        public MembershipCardSetting GetSettingByCompanyId(int CID)
        {
            return CurrentRepository.QueryEntity.FirstOrDefault(o => o.CompanyId == CID);
        }

        public OpResult UpdateSetting(MembershipCardSetting setting)
        {
            try
            {
                var data = CurrentRepository.QueryEntity.FirstOrDefault(o => o.Id == setting.Id);
                if (data == null)
                {
                    return OpResult.Fail("找不到原数据！");
                }
                data.StartChar = setting.StartChar;
                data.EndChar = setting.EndChar;
                data.BootSector = setting.BootSector;
                data.CheckSecuritycode = setting.CheckSecuritycode;
                CurrentRepository.Update(data);
                return OpResult.Success("保存成功！");
            }
            catch (Exception e)
            {
                return OpResult.Success(e.Message);
            }
        }
    }
}
