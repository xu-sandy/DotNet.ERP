using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.DAL;
using Pharos.Logic.Entity.Views;

namespace Pharos.Logic.BLL
{
    public class MemberCardPayDetailsBLL
    {
        private readonly MemberCardPayDetailsService _service = new MemberCardPayDetailsService();
        public List<MemberCardPayDetailsViewModel> GetMemberCardPayDetailsByPageList(string cardType, DateTime? beginDate, DateTime? endDate, int? storeIds, string cardNo, out int count, ref object footer)
        {
            return _service.GetMemberCardPayDetailsByPageList(cardType, beginDate, endDate, storeIds,cardNo, out count,ref footer);
        }
    }
}
