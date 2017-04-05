﻿using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pharos.DBFramework;

namespace Pharos.Logic.BLL
{
    public class ProductBrandBLL:BaseService<ProductBrand>
    {
        DAL.ProductBrandDAL dal = new DAL.ProductBrandDAL();
        public object GetBrandListBySearch(string title, int? classfyId, out int count) 
        {
            DataTable dt = dal.GetBrandListBySearch(title,CommonService.CompanyId, classfyId, out count);
            
            dt.Columns.Add("StateTitle", typeof(string));
            foreach (DataRow dr in dt.Rows)
            {
                dr["StateTitle"] = Enum.GetName(typeof(EnableState), dr["State"]);
            }
            return dt;
        }

        public object GetBrandList(string searchName, int? classfyId, out int count) 
        {
            DataTable dt = dal.GetBrandList(searchName, CommonService.CompanyId, classfyId, out count, 1);
            return dt;
        }

    }
}