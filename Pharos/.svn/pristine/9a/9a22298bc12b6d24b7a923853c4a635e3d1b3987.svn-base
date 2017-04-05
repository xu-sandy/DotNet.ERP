using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class MembersLocalService : BaseLocalService<Members>
    {

        public static Members GetMemberInfo(string memberCardNum)
        {
            return CurrentRepository.Find(o => o.MemberCardNum == memberCardNum);
        }


    }
}
