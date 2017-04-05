using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using System.Data.Entity;
using Pharos.Sys.Entity;
namespace Pharos.Logic.BLL
{
    public class ContractSerivce : BaseService<Contract>
    {
        public static object LoadContractList(NameValueCollection nvl, out int recordCount)
        {
            var sId = nvl["sId"];
            var qc = CurrentRepository.QueryEntity.Where(o=>o.CompanyId==CommonService.CompanyId);
            if (!sId.IsNullOrEmpty())
                qc = qc.Where(x => x.SupplierId == sId);
            var qatt = BaseService<Attachment>.CurrentRepository.QueryEntity;
            var qd = BaseService<SysDataDictionary>.CurrentRepository.QueryEntity;
            var query = from x in qc
                        let o = from y in qatt
                                where x.Id == y.ItemId && y.SourceClassify == 1
                                select y
                        join z in qd on x.ClassifyId equals z.DicSN into temp
                        from h in temp.DefaultIfEmpty()
                        select new
                        {
                            x.Id,
                            x.PId,
                            x.SigningDate,
                            x.StartDate,
                            x.EndDate,
                            x.State,
                            x.Title,
                            x.Version,
                            x.CreateDT,
                            x.ContractSN,
                            x.ClassifyId,
                            ClassTitle = h.Title,
                            Attachs = o.Count()
                        };
            recordCount = query.Count();
            //var sql = query.ToString();
            return query.ToList().Select(o => new {
                o.Id,
                o.PId,
                o.SigningDate,
                o.StartDate,
                o.EndDate,
                State=Enum.GetName(typeof(ContractState), o.State),
                o.Title,
                Version="V"+o.Version+".0",
                CreateDT=o.CreateDT.ToString("yyyy-MM-dd"),
                o.ContractSN,
                o.ClassifyId,
                o.ClassTitle,
                o.Attachs,
            });
        }
        public static Contract FindById(string id)
        {
            var obj = CurrentRepository.QueryEntity.Include(o => o.ContractBoths).Include(o=>o.Attachments).FirstOrDefault(o => o.Id == id);
            return obj;
        }

        public static List<RemindContractDao> GetContractRemind()
        {
            var qc = CurrentRepository.QueryEntity.Where(o=>o.CompanyId==CommonService.CompanyId); 
            var qs = BaseService<Supplier>.CurrentRepository.QueryEntity;
            var query = from x in qc
                        join y in qs on x.SupplierId equals y.Id
                        select new RemindContractDao
                        { 
                            Title = x.Title,
                            ContractSN = x.ContractSN,
                            SupplierTitle =y.Title,
                            SupplierFullTitle = y.FullTitle,
                            EndDate = x.EndDate
                        };
            var result = query.ToList();
            result =result.Where(o => Convert.ToDateTime(o.EndDate) >= Convert.ToDateTime(DateTime.Now.ToShortDateString())
                                                            && Convert.ToDateTime(o.EndDate) <= Convert.ToDateTime(DateTime.Now.AddDays(30).ToShortDateString())).ToList();
            return result;
        }

        public class RemindContractDao
        {

            public string Title { get; set; }

            public string ContractSN { get; set; }

            public string SupplierFullTitle { get; set; }
            public string SupplierTitle { get; set; }

            public string EndDate { get; set; }

        }
    }
}
