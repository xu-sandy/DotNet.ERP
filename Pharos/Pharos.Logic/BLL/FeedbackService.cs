using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL
{
    /// <summary>
    /// 售后回访业务实现
    /// </summary>
    public class FeedbackService : BaseService<Feedback>
    {
        public static object FindPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var memberId = nvl["memberId"];
            var query = from f in CurrentRepository.Entities
                        join m in MembersService.CurrentRepository.Entities on f.MemberId equals m.MemberId
                        join w in WarehouseService.CurrentRepository.Entities on m.StoreId equals w.StoreId
                        where f.MemberId == memberId
                        select new
                        {
                            f.Id,
                            f.FeedbackId,
                            f.MemberId,
                            f.Content,
                            f.CreateDT,
                            StoreTitle = w.Title,
                            MemberName = m.RealName
                        };
            var keyword = nvl["keyword"];
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim();
                query = query.Where(a => a.StoreTitle.Contains(keyword) || a.MemberName.Contains(keyword));
            };
            recordCount = query.Count();
            return query.OrderByDescending(a => a.CreateDT).ToList();
        }
    }
}
