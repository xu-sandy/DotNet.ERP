using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
namespace Pharos.Logic.BLL
{
    public class GroupingService:BaseService<Grouping>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel">使用类型</param>
        /// <returns></returns>
        public static List<Grouping> GetList(short channel=1)
        {
            return CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId && o.State == 1 && o.Channel == channel).ToList();
        }
    }
}
