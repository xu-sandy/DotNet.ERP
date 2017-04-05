using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using System.Data.SqlClient;
using Pharos.Sys.Entity;
namespace Pharos.Logic.BLL
{
    public class SysDataDictService : BaseService<SysDataDictionary>
    {
        /// <summary>
        /// 供应商分类
        /// </summary>
        /// <returns></returns>
        public static List<SysDataDictionary> GetSupplierTypes()
        {
            return FindList(o=>o.DicPSN==(int)DicType.供应商分类 && o.Status && o.CompanyId==CommonService.CompanyId).OrderBy(o=>o.SortOrder).ToList();
        }
        public static List<SysDataDictionary> GetWholesalerTypes()
        {
            return FindList(o => o.DicPSN == (int)DicType.批发商分类 && o.Status && o.CompanyId == CommonService.CompanyId).OrderBy(o => o.SortOrder).ToList();
        }
        /// <summary>
        /// 供应商类别名+相关商品数量
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSupplierTypeTitles()
        {
            var query=from x in CurrentRepository.Entities
                      let o = from y in SupplierService.CurrentRepository.Entities where y.ClassifyId == x.DicSN select y
                      where x.DicPSN == (int)DicType.供应商分类 && x.Status && x.CompanyId == CommonService.CompanyId
                      select new 
                      { 
                          x.Title,
                          Count=o.Count()
                      };
            return query.ToList().Select(o=>o.Title+"("+o.Count+")").ToList();
        }
        /// <summary>
        /// 批发商类别名+相关商品数量
        /// </summary>
        /// <returns></returns>
        public static List<string> GetWholesaleTypeTitles()
        {
            var query = from x in CurrentRepository.Entities
                        let o = from y in SupplierService.CurrentRepository.Entities where y.ClassifyId == x.DicSN select y
                        where x.DicPSN == (int)DicType.批发商分类 && x.Status && x.CompanyId == CommonService.CompanyId
                        select new
                        {
                            x.Title,
                            Count = o.Count()
                        };
            return query.ToList().Select(o => o.Title + "(" + o.Count + ")").ToList();
        }

        public static IList<SysDataDictionary> GetDictionaryList(DicType psn = DicType.全部)
        {
            return SysDataDictService.FindList(o => o.DicPSN == (int)psn && o.CompanyId == CommonService.CompanyId);
        }

        public static IList<SysDataDictionary> GetChildList(int psn)
        {

            var pids = SysDataDictService.CurrentRepository.QueryEntity.Where(o=>o.CompanyId==CommonService.CompanyId).Select(o => o.DicPSN).Distinct();
            var items = SysDataDictService.FindList(o => o.DicPSN == psn && o.CompanyId == CommonService.CompanyId);

            IList<SysDataDictionary> dict = new List<SysDataDictionary>();
            if (items.Exists(o => pids.Contains(o.DicSN)))
            {
                dict = items;
            }

            return dict;
        }

        public static IList<SysDataDictionary> GetDictionaryItems(int psn)
        {
            var pids = SysDataDictService.CurrentRepository.QueryEntity.Where(o=> o.CompanyId==CommonService.CompanyId).Select(o => o.DicPSN).Distinct();
            var items = SysDataDictService.FindList(o => o.DicPSN == psn && o.CompanyId == CommonService.CompanyId);

            IList<SysDataDictionary> dict = new List<SysDataDictionary>();
            if (!items.Exists(o => pids.Contains(o.DicSN)))
            {
                dict = items;
            }

            return dict;
        }

        public static bool HasItems(int psn)
        {
            var pids = SysDataDictService.CurrentRepository.QueryEntity.Select(o => o.DicPSN).Distinct();
            return SysDataDictService.IsExist(o => o.DicPSN == psn && !pids.Contains(o.DicSN));
        }

        public static bool HasChild(int psn)
        {
            var pids = SysDataDictService.CurrentRepository.QueryEntity.Select(o => o.DicPSN).Distinct();
            return SysDataDictService.IsExist(o => o.DicPSN == psn && pids.Contains(o.DicSN));

        }

        public static OpResult AddDict(SysDataDictionary dict)
        {
            OpResult re = new OpResult();
            try
            {
                string expression = "exec Sys_AddDic @DicPSN,@Title,@Status,@CompanyId";
                var result = SysDataDictService.CurrentRepository._context.Database.ExecuteSqlCommand(
                          expression,
                          new SqlParameter("@DicPSN", dict.DicPSN),
                          new SqlParameter("@Title", dict.Title),
                          new SqlParameter("@Status", dict.Status),
                          new SqlParameter("@CompanyId", CommonService.CompanyId));
            }
            catch (Exception ex)
            {
                re.Successed = false;
                re.Message = ex.Message;
            }
            return re;
        }

        /// <summary>
        /// 单据类别
        /// </summary>
        /// <returns></returns>
        public static List<SysDataDictionary> GetReceiptsCategories()
        {
            return FindList(o => o.DicPSN == (int)DicType.单据类别 && o.Status && o.CompanyId==CommonService.CompanyId).OrderBy(o => o.SortOrder).ToList();
        }

        public static List<SysDataDictionary> GetBigUnitCategories()
        {
            return FindList(o => o.DicPSN == (int)DicType.计量大单位 && o.Status && o.CompanyId == CommonService.CompanyId).OrderBy(o => o.SortOrder).ToList();
        }
        public static List<SysDataDictionary> GetSubUnitCategories()
        {
            return FindList(o => o.DicPSN == (int)DicType.计量小单位 && o.Status && o.CompanyId == CommonService.CompanyId).OrderBy(o => o.SortOrder).ToList();
        }
        /// <summary>
        /// 合同分类
        /// </summary>
        /// <returns></returns>
        public static List<SysDataDictionary> GetContractClassify()
        {
            //var query = CurrentRepository.FindList(o => o.DicPSN == (int)DicType.合同分类 && o.Status).OrderBy(o => o.SortOrder);
            //return query.ToList();
            return FindList(o => o.DicPSN == (int)DicType.合同分类 && o.Status && o.CompanyId == CommonService.CompanyId).OrderBy(o => o.SortOrder).ToList();
        }
        public static List<SysDataDictionary> GetBrandClassify()
        {
            var query = CurrentRepository.FindList(o => o.DicPSN == (int)DicType.品牌分类 && o.Status && o.CompanyId == CommonService.CompanyId).OrderBy(o => o.SortOrder);
            return query.ToList();
        }
        public static int GetMaxSN
        {
            get { return CurrentRepository.QueryEntity.Max(o => (int?)o.DicSN).GetValueOrDefault() + 1; }
        }

        /// <summary>
        /// 订单退换->换货理由
        /// </summary>
        /// <returns></returns>
        public static List<SysDataDictionary> GetReasonTitle()
        {
            return FindList(o => o.DicPSN == (int)DicType.换货理由 && o.Status && o.CompanyId == CommonService.CompanyId).OrderBy(o => o.SortOrder).ToList();
        }

        public static OpResult MoveItem(int mode, int sn)
        {
            var op = OpResult.Fail("顺序移动失败!");
            var obj = Find(o=>o.CompanyId==CommonService.CompanyId && o.DicSN==sn);
            var list = CurrentRepository.QueryEntity.Where(o => o.DicPSN == obj.DicPSN).OrderBy(o=>o.SortOrder).ToList();
            switch (mode)
            {
                case 2://下移
                    var obj1 = list.LastOrDefault();
                    if (obj.Id != obj1.Id)
                    {
                        SysDataDictionary next = null;
                        for (var i = 0; i < list.Count; i++)
                        {
                            if (obj.Id == list[i].Id)
                            {
                                next = list[i + 1]; break;
                            }
                        }
                        if (next != null)
                        {
                            var sort = obj.SortOrder;
                            obj.SortOrder = next.SortOrder;
                            next.SortOrder = sort;
                            op = Update(obj);
                        }
                    }
                    break;
                default:
                    var obj2 = list.FirstOrDefault();
                    if (obj.Id != obj2.Id)
                    {
                        SysDataDictionary prev = null;
                        for (var i=0;i<list.Count;i++)
                        {
                            if (obj.Id == list[i].Id)
                            {
                                prev = list[i - 1]; break;
                            }
                        }
                        if (prev != null)
                        {
                            var sort = obj.SortOrder;
                            obj.SortOrder = prev.SortOrder;
                            prev.SortOrder = sort;
                            op = Update(obj);
                        }
                    }
                    break;
            }
            return op;
        }
        public static new OpResult Delete(List<SysDataDictionary> list)
        {
            CurrentRepository.RemoveRange(list);
            return OpResult.Success();
        }
        public static new OpResult AddRange(List<SysDataDictionary> entities, bool isSave = true)
        {
            CurrentRepository.AddRange(entities, isSave);
            return OpResult.Success();
        }
    }
    
}
