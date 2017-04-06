﻿using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Logic.ApiData.Pos.Sale;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using Pharos.ObjectModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services.LocalCeServices
{
    public class ProductRecordService : BaseGeneralService<ProductRecord, LocalCeDbContext>
    {
        /// <summary>
        /// 适配多条码串、一品多码、价格（变价、一品多价、系统售价）
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="barcode"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static ProductInfo GetProductRecod(string storeId, string barcode, int companyId, bool isFindWeigh = false)
        {
            var sql = @"SELECT
'' MultiCode, 
((case when BarcodeMult is null then '' when BarcodeMult is not null then BarcodeMult end)+',' +(case when Barcodes is null then '' when Barcodes is not null then Barcodes end)  ) MultiCodes,
p.ProductCode,
p.Barcode MainBarcode,
p.Title AS Title,
p.Size AS Size,
p.BrandName AS Brand,
p.CategoryPathName AS Category,
p.Unit Unit,
CAST((CASE WHEN p.ValuationType =1THEN 1 ELSE 0 end) AS BIT ) EnableEditNum,
CAST(p.Favorable AS BIT) EnableEditPrice,
p.SysPrice SystemPrice,
p.BuyPrice BuyPrice,
(
CASE WHEN p.ValuationType =2 THEN 1 
WHEN p.ValuationType =1 AND p.Nature=0 THEN 0  
WHEN p.ValuationType =1 AND p.Nature=1 THEN 4 
WHEN p.ValuationType =1 AND p.Nature=2 THEN 3 END)  ProductType
 FROM ProductRecord  p 
where  (p.Barcode='" + barcode + "' or (','+ p.BarcodeMult+',') like ('%," + barcode + ",%') or (','+ p.Barcodes+',') like ('%," + barcode + ",%'))";
            var query = CurrentRepository._context.Database.SqlQuery<ProductInfo>(sql);
            ProductInfo result = query.FirstOrDefault();
            if (result == null && !isFindWeigh)
            {
                throw new SaleException("605", string.Format("未找到商品【{0}】！", barcode));
            }
            else if (result == null && isFindWeigh)
            {
                return result;
            }
            if (!string.IsNullOrEmpty(result.MultiCodes))
            {
                result.MultiCode = result.MultiCodes.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            return result;
        }

        public static IEnumerable<InventoryResult> CheckedPrice(string storeId, int companyId, IEnumerable<int> categorySns, decimal from, decimal to)
        {
            string categorySnsStr = string.Empty;
            if (categorySns != null)
            {
                categorySnsStr = string.Join(",", categorySns.Select(o => o.ToString()));
            }
            return CurrentRepository.Entities.Where(p => categorySnsStr.Contains(p.CategorySN.ToString())).Select(o => new InventoryResult()
            {
                Barcode = o.Barcode,
                Brand = o.Barcode,
                Category = o.CategoryName,
                Inventory = o.Inventory,
                Price = o.SysPrice,
                ProductCode = o.ProductCode,
                Size = o.Size,
                Title = o.Title,
                Unit = o.Unit
            });
        }



        internal static PageResult<InventoryResult> CheckedPrice(string StoreId, int CompanyId, IEnumerable<int> categorySns, decimal from, decimal to, int pageSize, int pageIndex)
        {
            ///Todo CheckedPrice
            string categorySnsStr = string.Empty;
            if (categorySns != null)
            {
                categorySnsStr = '/' + string.Join("/", categorySns.Select(o => o.ToString())) + '/';
            }
            if (categorySnsStr == "/0/")
            {
                categorySnsStr = string.Empty;
            }
            var beforecount = pageSize * (pageIndex - 1);
            var sql = @"select 
t1.Barcode,
t1.Title,
t1.Size,
t1.BrandName as Brand,
t1.CategoryPathName as Category,
t1.Unit,
t1.SysPrice Price,
t1.Inventory,
t1.ProductCode
from ProductRecord t1 WHERE t1.SysPrice BETWEEN " + from + " AND " + to + " and '/'+t1.CategoryPath +'/' like '%" + categorySnsStr + "%'";


            var sqlpaging = sql + " Order by id OFFSET " + beforecount + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            var total = CurrentRepository._context.Database.SqlQuery<InventoryResult>(sql).Count();
            var ranges = CurrentRepository._context.Database.SqlQuery<InventoryResult>(sqlpaging).ToList();

            var pageCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
            var pager = new PageInfo()
            {
                Index = pageIndex,
                Size = pageSize,
                Total = total,
                PageCount = pageCount
            };
            var result = new PageResult<InventoryResult>()
            {
                Datas = ranges,
                Pager = pager
            };
            return result;
        }

        internal static PageResult<InventoryResult> CheckedInventory(string StoreId, int CompanyId, IEnumerable<int> categorySns, string keyword, decimal price, int pageSize, int pageIndex)
        {
            try
            {
                ///Todo CheckedInventory
                string categorySnsStr = string.Empty;
                if (categorySns != null)
                {
                    categorySnsStr = "/" + string.Join("/", categorySns.Select(o => o.ToString())) + "/";
                }
                if (categorySnsStr == "/0/")
                {
                    categorySnsStr = string.Empty;
                }

                var beforecount = pageSize * (pageIndex - 1);
                var sql = @"select 
t1.Barcode,
t1.Title,
t1.Size,
t1.BrandName as Brand,
t1.CategoryPathName as Category,
t1.Unit,
t1.SysPrice Price,
t1.Inventory,
t1.ProductCode
from ProductRecord t1 WHERE  '/'+t1.CategoryPath +'/' like '%" + categorySnsStr + "%' and t1.ValuationType =1 ";

                if (!string.IsNullOrEmpty(keyword))
                {
                    sql += " and ( t1.barcode like '%" + keyword + "%' or t1.productcode like '%" + keyword + "%' or t1.BarcodeMult like '%" + keyword + "%' or t1.Barcodes like '%" + keyword + "%' ) ";
                }
                if (price > 0)
                {
                    sql += " and t1.SysPrice = " + price.ToString() + " ";
                }

                var sqlpaging = sql + " Order by id OFFSET " + beforecount + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                var total = CurrentRepository._context.Database.SqlQuery<InventoryResult>(sql).Count();
                var ranges = CurrentRepository._context.Database.SqlQuery<InventoryResult>(sqlpaging).ToList();

                var pageCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
                var pager = new PageInfo()
                {
                    Index = pageIndex,
                    Size = pageSize,
                    Total = total,
                    PageCount = pageCount
                };
                var result = new PageResult<InventoryResult>()
                {
                    Datas = ranges,
                    Pager = pager
                };
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}