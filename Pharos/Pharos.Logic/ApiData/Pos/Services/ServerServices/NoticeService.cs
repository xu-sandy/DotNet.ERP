using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using Pharos.Logic.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services
{
    public class NoticeService : BaseGeneralService<Pharos.Logic.Entity.Notice, EFDbContext>
    {

        public static IEnumerable<Announcement> Announcements(string storeId, string machineSn, int companyId)
        {
            var date = DateTime.Now.Date;
            return CurrentRepository.Entities.Where(o => ("," + o.StoreId + ",").Contains("," + storeId + ",") && o.CompanyId == companyId && o.State == 1 && o.BeginDate <= date && o.ExpirationDate >= date)
                .Select(o => new Announcement()
                {
                    Content = o.NoticeContent,
                    Theme = o.Theme,
                    ImgUrl = o.Url
                }).ToList();
        }

        internal static IEnumerable<Activity> Activities(string storeId, string machineSn, int companyId)
        {
            var date = DateTime.Now.Date;
            return CurrentRepository.Entities.Where(o => (("," + o.StoreId + ",").Contains(",-1,") || ("," + o.StoreId + ",").Contains("," + storeId + ",")) && o.CompanyId == companyId && o.Type == 2 && o.State == 1 && o.BeginDate <= date && o.ExpirationDate >= date)
                .Select(o => new Activity()
                {
                    Theme = o.Theme,
                    Id = o.Id
                }).ToList();
        }
    }
}
