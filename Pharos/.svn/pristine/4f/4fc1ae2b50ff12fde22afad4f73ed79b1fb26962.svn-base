using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using System.Collections.Specialized;
using Pharos.Utility.Helpers;

namespace Pharos.Logic.BLL
{
    public class OutOfStorageBLL:BaseService<OutboundGoods>
    {
        /// <summary>
        /// 用于datagrid列表
        /// </summary>
        /// <param name="nvl">传递条件</param>
        /// <param name="recordCount">返回总行数</param>
        /// <returns>list</returns>
        public static object FindOutboundGoodsList(NameValueCollection nvl, out int recordCount)
        {
            var queryOutboundGoods = BaseService<OutboundGoods>.CurrentRepository.QueryEntity;
            var queryOutboundList=BaseService<OutboundList>.CurrentRepository.QueryEntity;
            var queryWarehouse = BaseService<Warehouse>.CurrentRepository.QueryEntity;
            var queryProduct = BaseService<ProductRecord>.CurrentRepository.QueryEntity;
            var queryUser = UserInfoService.CurrentRepository.QueryEntity;
            var queryBrand=BaseService<ProductBrand>.CurrentRepository.QueryEntity;

            var query = from x in queryOutboundGoods
                        join y in queryOutboundList on x.Id equals y.Id
                        join z in queryWarehouse on x.StoreId equals z.StoreId 
                        join i in queryProduct on y.Barcode equals i.Barcode
                        join j in queryUser on x.ApplyUID equals j.UID into temp 
                        from k in temp.DefaultIfEmpty()
                        join l in queryUser on x.OperatorUID equals l.UID into temp1
                        from m in temp1.DefaultIfEmpty()
                        join n in queryBrand on i.BrandSN equals n.BrandSN

                        select new
                        {
                            x.Id,
                            x.StoreId,
                            x.ApplyOrgId,
                            StoreTitle= z.Title,
                            i.ProductCode,
                            y.Barcode,
                            ProductTitle= i.Title,
                            i.BrandSN,
                            Brand=n.Title,
                            y.OutboundNumber,
                            x.CreateDT,
                            x.ApplyUID,
                            Apply= k.FullName, 
                            x.OperatorUID,
                            Operator=m.FullName
                        };     
            var store = nvl["Store"];
            var applyOrg = nvl["ApplyOrg"];
            var apply = nvl["Apply"];
            var startDate = nvl["StartDate"];
            var endDate = nvl["EndDate"];

            if (!store.IsNullOrEmpty())
            {
                query = query.Where(o => o.StoreId == store);
            }
            if (!applyOrg.IsNullOrEmpty())
            {
                query = query.Where(o => o.ApplyOrgId == applyOrg);
            }
            if (!apply.IsNullOrEmpty())
            {
                query = query.Where(o => o.ApplyUID == apply);
            }
            if (!startDate.IsNullOrEmpty() && !endDate.IsNullOrEmpty())
            {
                var st1 = DateTime.Parse(startDate);
                var st2 = DateTime.Parse(endDate).AddDays(1);
                query = query.Where(o => o.CreateDT < st2 && o.CreateDT >= st1);
            }
           
            recordCount = query.Count();
            return query.ToPageList(nvl);
        }


    }
}
