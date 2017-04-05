using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Pharos.Logic.OMS.BLL
{
    public class BusinessService
    {
        [Ninject.Inject]
        public IBaseRepository<Business> BusinessRepository { get; set; }
        [Ninject.Inject]
        public IBaseRepository<Traders> TraderRepository { get; set; }
        [Ninject.Inject]
        public IBaseRepository<ProductBrand> BrandRepository { get; set; }
        [Ninject.Inject]
        public IBaseRepository<PayLicense> PayLicenseRepository { get; set; }
        public Pharos.Utility.OpResult SaveOrUpdate(Business model)
        {
            if (BusinessRepository.GetQuery(o => o.Title == model.Title && o.Id != model.Id).Any())
                return Utility.OpResult.Fail("该名称已存在!");
            if(model.Id==0)
            {
                model.ById = CommonService.GUID;
                model.CreateDT = DateTime.Now;
                model.CreateUID = CurrentUser.UID;
                model.Status = 1;
                BusinessRepository.Add(model);
            }
            else
            {
                var source = BusinessRepository.Get(model.Id);
                model.ToCopyProperty(source, new List<string>() { "CreateDT", "CreateUID", "Status" });
                BusinessRepository.SaveChanges();
            }
            return Utility.OpResult.Success();
        }

        public IEnumerable<dynamic> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var query = BusinessRepository.GetQuery();
            var queryTrader = TraderRepository.GetQuery();
            var queryBrand = BrandRepository.GetQuery();
            var title = nvl["title"];
            if (!title.IsNullOrEmpty())
            {
                query = query.Where(o => o.Title.Contains(title) || o.Byname.Contains(title));
            }
            var q = from x in query
                    select new { 
                        x.Id,
                        x.Title,
                        x.Byname,
                        x.ById,
                        x.CreateDT,
                        x.CreateUID,
                        x.Status,
                        StateTitle = x.Status == 1 ? "可用" : "禁用",
                        TraderNum = queryTrader.Count(o => ("," + o.BusinessScopeId + ",").Contains("," + x.ById + ","))
                    };
            recordCount = q.Count();
            return q.ToPageList();
        }
        public List<Entity.Business> GetTreeList(System.Collections.Specialized.NameValueCollection nvl)
        {
            var query = BusinessRepository.GetQuery();
            var queryTrader = TraderRepository.GetQuery();
            var queryBrand = BrandRepository.GetQuery();
            var queryPayLicense = PayLicenseRepository.GetQuery();
            var title = nvl["title"];
            if (!title.IsNullOrEmpty())
            {
                query = query.Where(o => o.Title.Contains(title) || o.Byname.Contains(title));
            }
            var q = from x in query
                    select new
                    {
                        Id= x.Id,
                        Title=x.Title,
                        Byname=x.Byname,
                        ById=x.ById,
                        CreateDT=x.CreateDT,
                        CreateUID=x.CreateUID,
                        Status=x.Status,
                        TraderNum = queryTrader.Count(o => ("," + o.BusinessScopeId + ",").Contains("," + x.ById + ",")),
                        PayLicenseNum=queryPayLicense.Count(o=>o.BusinessId1==x.ById||o.BusinessId2==x.ById),
                        x.ParentId
                    };
            var buss = q.ToList().Select(x => new Business() {
                Id = x.Id,
                Title = x.Title,
                Byname = x.Byname,
                ById = x.ById,
                CreateDT = x.CreateDT,
                CreateUID = x.CreateUID,
                Status = x.Status,
                TraderNum=x.TraderNum,
                PayLicenseNum=x.PayLicenseNum,
                ParentId=x.ParentId
            }).ToList();
            var list = new List<Entity.Business>();
            foreach (var bu in buss.Where(o => o.ParentId.IsNullOrEmpty()))
            {
                SetChilds(bu, buss);
                list.Add(bu);
            }
            return list;
        }
        void SetChilds(Entity.Business bu, List<Entity.Business> alls)
        {
            bu.Childrens = alls.Where(o => o.ParentId == bu.ById).ToList();
            foreach (var child in bu.Childrens)
            {
                SetChilds(child, alls);
            }
        }
        public OpResult Deletes(int[] ids)
        {
            var list= BusinessRepository.GetQuery(o => ids.Contains(o.Id)).ToList();
            BusinessRepository.RemoveRange(list);
            return Utility.OpResult.Success();
        }

        public Business GetOne(object id)
        {
            return BusinessRepository.Get(id);
        }

        public List<Business> GetList(bool all=true)
        {
            var query = BusinessRepository.GetQuery();
            if (!all) query = query.Where(o => o.Status == 1);
            return query.ToList();
        }
        public List<Business> GetParentList(bool all = true)
        {
            var query = BusinessRepository.GetQuery(o=>(o.ParentId==null || o.ParentId==""));
            if (!all) query = query.Where(o => o.Status == 1);
            return query.ToList();
        }
        public OpResult SetState(string ids, short state)
        {
            var sId = ids.Split(',').Select(o => int.Parse(o));
            var olist = BusinessRepository.GetQuery(o => sId.Contains(o.Id)).ToList();
            olist.Each(o => o.Status = state);
            return OpResult.Result(BusinessRepository.SaveChanges());
        }

        public Business GetOneByWhere(Expression<Func<Business, bool>> whereLambda)
        {
            return BusinessRepository.GetQuery(whereLambda).FirstOrDefault();
        }

        public List<Business> GetListByWhere(Expression<Func<Business, bool>> whereLambda)
        {
            return BusinessRepository.GetQuery(whereLambda).ToList();
        }
    }
}
