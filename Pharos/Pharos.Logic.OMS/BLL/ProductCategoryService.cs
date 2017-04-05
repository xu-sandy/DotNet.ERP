﻿using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.Logic.OMS.BLL
{
    public class ProductCategoryService : BaseService<ProductCategory>
    {
        [Ninject.Inject]
        public IBaseRepository<ProductCategory> ProductCategoryRepository { get; set; }
        [Ninject.Inject]
        public IBaseRepository<ProductRecord> ProductRepository { get; set; }
        public OpResult SaveOrUpdate(ProductCategory model)
        {
            var op = new OpResult();
            var obj=ProductCategoryRepository.GetQuery(o => o.CategoryPSN == model.CategoryPSN && o.Title == model.Title && o.Id != model.Id).FirstOrDefault();
            if (obj != null)
            {
                model.CategorySN = obj.CategorySN;
                return OpResult.Fail("已存在该名称");
            }
            if (model.Id == 0)
            {
                var max = MaxSN() + 1;
                model.CategorySN = max;
                model.Grade = GetGrade;
                model.CategoryCode = MaxCode(model.CategoryPSN);
                ProductCategoryRepository.Add(model);
            }
            else
            {
                var supp = ProductCategoryRepository.Get(model.Id);
                model.ToCopyProperty(supp);
                ProductCategoryRepository.SaveChanges();
            }
            return OpResult.Success();
        }
        public int MaxSN()
        {
            return ProductCategoryRepository.GetQuery().Max(o => (int?)o.CategorySN).GetValueOrDefault();
        }
        short GetGrade
        {
            get
            {
                var val = HttpContext.Current.Request.Form["CategoryPSN_0"];
                if (val.IsNullOrEmpty()) val = HttpContext.Current.Request["CategoryPSN_0"];
                if (val.IsNullOrEmpty()) return 1;
                var vals = val.Split(',');
                short grade = 0;
                for (int i = vals.Length; i > 0; i--)
                {
                    if (!vals[i - 1].IsNullOrEmpty())
                    {
                        grade = (short)(i + 1); break;
                    }
                }
                return grade;
            }
        }
        public short MaxCode(int psn)
        {
            int max = 0;
            try
            {
                max = ProductCategoryRepository.GetQuery(o => o.CategoryPSN == psn).Max(o =>(int?)o.CategoryCode).GetValueOrDefault();
            }
            catch { max = 0; }
            if (max >= 99) throw new Exception("类别号超过99");
            return Convert.ToInt16(max);
        }
        public IEnumerable<ProductCategory> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var qd = ProductRepository.GetQuery();
            var query = ProductCategoryRepository.GetQuery();
            var qys = from x in query
                      select new
                      {
                          x.Id,
                          x.Title,
                          x.CategoryPSN,
                          x.CategorySN,
                          x.State,
                          x.OrderNum,
                          x.CategoryCode,
                          count = qd.Count(o=>o.CategorySN==x.CategorySN)
                      };

            var parentType = nvl["parentType_0"]??"";
            var state = nvl["state"];
            var searchText = nvl["searchText"];
            if (!state.IsNullOrEmpty())
            {
                var st = short.Parse(state);
                qys = qys.Where(o => o.State == st);
            }
            var alls = qys.OrderBy(o => o.OrderNum).AsEnumerable().Select(x => new ProductCategory()
            {
                Id = x.Id,
                Title = x.Title,
                CategoryPSN = x.CategoryPSN,
                CategorySN = x.CategorySN,
                State = x.State,
                OrderNum = x.OrderNum,
                CategoryCode = x.CategoryCode,
                ProductCount = x.count
            }).ToList();
            var rs = new List<ProductCategory>();
            var cateids = parentType.Split(',').Where(o => !o.IsNullOrEmpty()).Select(o => int.Parse(o)).ToList();
            if(cateids.Any())
            {
                var list = alls.Where(o => o.CategorySN == cateids[0]).ToList();
                if (cateids.Count > 1)
                {
                    list.Add(alls.FirstOrDefault(o => o.CategorySN == cateids[1]));
                    list.AddRange(alls.Where(o => o.CategoryPSN == cateids[1]));
                }
                else
                {
                    var seconds = alls.Where(o => o.CategoryPSN == cateids[0]);
                    list.AddRange(seconds);
                    list.AddRange(alls.Where(o => seconds.Select(i => i.CategorySN).Contains(o.CategoryPSN)));
                }
                alls = list;
            }
            foreach (var item in alls.Where(o => o.CategoryPSN == 0))
            {
                SetChild(alls, item, true);
                rs.Add(item);
            }
            recordCount = rs.Count();
            return rs.AsQueryable().ToPageList();
        }
        void SetChild(List<ProductCategory> alls, ProductCategory category, bool showAll)
        {
            var childs = alls.Where(o => o.CategoryPSN == category.CategorySN);
            if (!showAll) childs = childs.Where(o => o.State == 1);
            if (childs.Any())
            {
                if (category.Childrens == null) category.Childrens = new List<ProductCategory>();
                category.Childrens.AddRange(childs);
            }
            category.CategoryPSNTitle = category.Title;
            foreach (var item in childs)
                SetChild(alls, item, showAll);
        }
        public OpResult Deletes(int[] ids)
        {
            var list = ProductCategoryRepository.GetQuery(o => ids.Contains(o.Id)).ToList();
            var categorys= list.Select(o => o.CategorySN).ToList();
            if (ProductRepository.GetQuery(o => categorys.Contains(o.CategorySN)).Any())
                return OpResult.Fail("无法移除，该分类下存在商品档案信息!");
            ProductCategoryRepository.RemoveRange(list);
            return OpResult.Success();
        }

        public ProductCategory GetOne(object id)
        {
            var obj= ProductCategoryRepository.Get(id);
            if (obj != null && obj.CategoryPSN > 0)
            {
                obj.CategoryPSNTitle = LoopCategory(obj.CategoryPSN).TrimStart('/'); 
            }
            return obj;
        }


        public List<ProductCategory> GetList()
        {
            return ProductCategoryRepository.GetQuery().ToList();
        }

        public List<ProductCategory> GetFirstList()
        {
            return ProductCategoryRepository.GetQuery(o => o.CategoryPSN == 0).ToList();
        }

        public List<ProductCategory> GetChildCategorys(int psn)
        {
            return ProductCategoryRepository.GetQuery(o => o.CategoryPSN == psn && o.State == 1).OrderBy(o => o.OrderNum).ToList();
        }
        public List<int> GetChildSNs(List<int> bigSNs, bool containSelf = false)
        {
            var childs = new List<ProductCategory>();
            var allCategorys = GetList();
            var list = allCategorys.Where(o => bigSNs.Contains(o.CategoryPSN) && o.State == 1);
            var mids = list.Select(o => o.CategorySN).ToList();
            childs.AddRange(list);
            list = allCategorys.Where(o => mids.Contains(o.CategoryPSN) && o.State == 1);
            if (list.Any()) childs.AddRange(list);
            var chs = childs.Select(o => o.CategorySN).ToList();
            if (containSelf) chs.AddRange(bigSNs);
            return chs;
        }
        public void AddRange(List<ProductCategory> list)
        {
            ProductCategoryRepository.AddRange(list);
        }

        public OpResult SetState(string ids, short state)
        {
            var sids = ids.Split(',').Select(o => int.Parse(o)).ToList();
            var list = ProductCategoryRepository.GetQuery(o => sids.Contains(o.Id)).ToList();
            list.ForEach(o => { o.State = state; });
            return OpResult.Result( ProductCategoryRepository.SaveChanges());
        }

        string LoopCategory(int sn)
        {
            var obj = ProductCategoryRepository.Find(o => o.CategorySN == sn);
            if (obj != null)
                return LoopCategory(obj.CategoryPSN) + "/" + obj.Title;
            return "";
        }

    }
}
