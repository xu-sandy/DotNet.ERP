﻿using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using System.Data;
using Pharos.Logic.OMS.IDAL;
using Pharos.DBFramework;
namespace Pharos.Logic.OMS.BLL
{
    public class BrandService : BaseService<ProductBrand>
    {
        [Ninject.Inject]
        IBaseRepository<ProductBrand> BrandRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<ProductRecord> ProductRepository { get; set; }
        [Ninject.Inject]
        ImportSetService ImportSetService { get; set; }
        [Ninject.Inject]
        IBaseRepository<SysDataDictionary> DataDictRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Traders> TraderRepository { get; set; }
        public Pharos.Utility.OpResult SaveOrUpdate(ProductBrand model)
        {
            var obj = BrandRepository.GetQuery(o => o.Title == model.Title && o.Id != model.Id).FirstOrDefault();
            if (obj!=null)
            {
                model.BrandSN = obj.BrandSN;
                return OpResult.Fail("已存在该品牌!");
            }
            if (model.Id == 0)
            {
                model.BrandSN = MaxSN() + 1;
                model.State = 1;
                BrandRepository.Add(model);
            }
            else
            {
                var source = BrandRepository.Get(model.Id);
                model.ToCopyProperty(source);
                BrandRepository.SaveChanges();
            }
            return OpResult.Success();
        }
        public int MaxSN()
        {
            return BrandRepository.GetQuery().Max(o => (int?)o.BrandSN).GetValueOrDefault();
        }
        public IEnumerable<dynamic> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var title = nvl["title"];
            var classfyId = nvl["classfyId"].ToType<int>();
            var BrandWhere = DynamicallyLinqHelper.Empty<ProductBrand>().And(o => o.Title.Contains(title.Trim()), title.IsNullOrEmpty()).And(o => o.ClassifyId == classfyId, classfyId<=0);
            var queryPro = ProductRepository.GetQuery();
            var queryBus = DataDictRepository.GetQuery(o => o.DicPSN==5);
            var queryBrand = BrandRepository.GetQuery(BrandWhere);
            var queryTrader = TraderRepository.GetQuery();
            var query = from x in queryBrand
                        select new
                        {
                            x.Id,
                            x.Source,
                            x.State,
                            x.Title,
                            x.BrandSN,
                            x.JianPin,
                            StateTitle=x.State==1?"可用":"禁用",
                            ClassifyTitle = queryBus.Where(i => i.DicSN == x.ClassifyId).Select(i => i.Title).FirstOrDefault(),
                            ProductCount = queryPro.Count(i=>i.BrandSN==x.BrandSN),
                            TraderTitle=queryTrader.Where(i=>i.CID==x.Source).Select(o=>o.Title).FirstOrDefault()
                        };
            recordCount = query.Count();
            return query.ToPageList();
        }

        public OpResult Deletes(int[] ids)
        {
            var list= BrandRepository.GetQuery(o => ids.Contains(o.Id)).ToList();
            var brands= list.Select(o => o.BrandSN).ToList();
            if (ProductRepository.GetQuery(o => brands.Contains(o.BrandSN)).Any())
                return OpResult.Fail("商品存在品牌关联!");
            if(BrandRepository.RemoveRange(list))
            {
                LogEngine.WriteDelete(string.Join(",",list.Select(o => o.Title)), LogModule.品牌管理);
            }
            return OpResult.Success();
        }

        public ProductBrand GetOne(object id)
        {
            return BrandRepository.Get(id);
        }


        public List<ProductBrand> GetList()
        {
            return BrandRepository.GetQuery().ToList();
        }

        public OpResult SetState(string ids, short state)
        {
            var sId = ids.Split(',').Select(o => int.Parse(o));
            var olist = BrandRepository.GetQuery(o => sId.Contains(o.Id)).ToList();
            olist.Each(o => o.State = state);
            return OpResult.Result(BrandRepository.SaveChanges());
        }
        public List<ProductBrand> GetBrandInput(string text)
        {
            if(!text.IsNullOrEmpty())
            {
                return BrandRepository.GetQuery(o => (o.JianPin != null && o.JianPin.StartsWith(text)) || o.Title.Contains(text)).Take(20).ToList();
            }
            return new List<ProductBrand>();
        }
        public bool IsExistsTitle(string title)
        {
            return BrandRepository.GetQuery(o => o.Title == title).Any();
        }
        public OpResult Import(ImportSet obj, System.Web.HttpFileCollectionBase httpFiles, string fieldName, string columnName)
        {
            var op = new OpResult();
            var errLs = new List<string>();
            int count = 0;
            try
            {
                Dictionary<string, char> fieldCols = null;
                DataTable dt = null;
                op = ImportSetService.ImportSet(obj, httpFiles, fieldName, columnName, ref fieldCols, ref dt);
                if (!op.Successed) return op;
                var brandClass = DataDictRepository.GetQuery(o => o.DicPSN ==5).ToList();
                var otherClass = brandClass.FirstOrDefault(o => o.Title.StartsWith("其"));
                var brands = GetList();
                var clsIdx = Convert.ToInt32(fieldCols["ClassifyId"]) - 65;
                var titleIdx = Convert.ToInt32(fieldCols["Title"]) - 65;
                count = dt.Rows.Count;
                var max = DataDictRepository.GetQuery().Max(o=>o.DicSN);
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        var dr = dt.Rows[i];
                        var text = dr[clsIdx].ToString();
                        if (text.IsNullOrEmpty()) continue;
                        var cls = brandClass.FirstOrDefault(o => o.Title == text);
                        if (cls != null)
                        {
                            dr[clsIdx] = cls.DicSN.ToString();
                        }
                        else
                        {
                            if (obj.RefCreate)
                            {
                                var data = new SysDataDictionary
                                {
                                    DicPSN=5,
                                    Status = true,
                                    Title = text,
                                    DicSN = max++,
                                    CreateDT=DateTime.Now,
                                    CreateUID=CurrentUser.UID
                                };
                                DataDictRepository.Add(data);
                                brandClass.Add(data);
                                dr[clsIdx] = data.DicSN.ToString();
                            }
                            else if (otherClass != null)
                            {
                                dr[clsIdx] = otherClass.DicSN.ToString();
                            }
                            else
                            {
                                errLs.Add("行业分类[" + text + "]不存在!");
                                dt.Rows.RemoveAt(i);//去除不导入
                                continue;
                            }
                        }
                        text = dr[titleIdx].ToString().Trim();
                        if (brands.Any(o => o.Title == text))
                        {
                            errLs.Add("品牌名称[" + text + "]已存在!");
                            dt.Rows.RemoveAt(i);//去除不导入
                        }
                        else
                        {
                            brands.Add(new ProductBrand() { Title = text });
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("品牌分类处理失败!", e);
                    }
                }
                var maxsn = BrandRepository.GetQuery().Max(o=>(int?)o.BrandSN).GetValueOrDefault();
                StringBuilder sb = new StringBuilder();
                sb.Append("begin tran ");
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("insert into ");
                    sb.Append(obj.TableName);
                    sb.Append("(BrandSN,State,Source,");
                    sb.Append(string.Join(",", fieldCols.Keys));
                    sb.Append(") values(");
                    sb.AppendFormat("{0},", ++maxsn);
                    sb.AppendFormat("1,0,");
                    foreach (var de in fieldCols)
                    {
                        var index = Convert.ToInt32(de.Value) - 65;
                        try
                        {
                            var text = dr[index].ToString();
                            sb.Append("'" + text + "',");
                        }
                        catch (Exception e)
                        {
                            throw new Exception("列选择超过范围!", e);
                        }
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                    sb.Append(");");
                }
                sb.Append(" commit tran");
                if (dt.Rows.Count > 0)
                {
                    op.Successed = new DBHelper().ExecuteNonQueryText(sb.ToString(), null) > 0;
                    LogEngine.WriteInsert("品牌导入", LogModule.档案管理);
                }
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                op.Successed = false;
                LogEngine.WriteError(ex);
                errLs.Add("导入出现异常!");
            }
            return CommonService.GenerateImportHtml(errLs, count);
        }
    }
}
