﻿using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using Pharos.ObjectModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services.LocalCeServices
{
    public class ProductCategorySerivce : BaseGeneralService<ProductCategory, LocalCeDbContext>
    {
        /// <summary>
        /// 获取全部分类
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static IEnumerable<CategoryDAO> GetStoreCategories(string storeId, int companyId)
        {
            try
            {
                var result = CurrentRepository.Entities.Select(o => new CategoryDAO()
                {
                    CategoryPSN = o.CategoryPSN,
                    CategorySN = o.CategorySN,
                    Grade = o.Grade,
                    OrderNum = o.OrderNum,
                    Title = o.Title
                }).ToList();
                return result.OrderBy(o => o.CategorySN);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据分类查询子分类和本身
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="category"></param>
        /// <param name="depth"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static IEnumerable<CategoryDAO> GetLastDepthStoreCategories(string storeId, int category, int depth, int companyId)
        {
            string categoryStr = "/" + category.ToString() + "/";
            var result = CurrentRepository._context.Database.SqlQuery<CategoryDAO>("select CategoryPSN,CategorySN,Grade,OrderNum,Title from ProductCategory where  Grade>= " + depth + " and '/'+CategorySNPath+'/' like '%" + categoryStr + "%'").ToList();
            return result;
        }
    }
}