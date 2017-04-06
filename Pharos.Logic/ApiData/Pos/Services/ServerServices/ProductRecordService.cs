using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Logic.ApiData.Pos.Sale;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using Pharos.Logic.DAL;
using Pharos.Logic.Entity;
using Pharos.ObjectModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services
{
    public class ProductRecordService : BaseGeneralService<VwProduct, EFDbContext>
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
            var query = CurrentRepository._context.Database.SqlQuery<ProductInfo>("exec CheckedStoreProductInfo @p0,@p1,@p2", storeId, companyId, barcode).ToList();
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
    }
}
