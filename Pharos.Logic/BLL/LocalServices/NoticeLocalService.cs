using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices.LocalRead
{
    public class NoticeLocalService : BaseLocalService<Notice>
    {
        public static IEnumerable<Notice> GetNotices() 
        {
            var date = DateTime.Now.Date;
            return CurrentRepository.FindList(o => o.ExpirationDate > date).ToList();
        }
    }
}
