using Pharos.DBFramework;
using Pharos.Logic.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Pharos.Logic.DAL
{
    internal class MakingCouponCardDAL
    {
        DBHelper db = new DBHelper();

        #region 制作优惠券列表
        /// <summary>
        /// 获取制作优惠券列表
        /// </summary>
        /// <param name="paging">分页信息</param>
        /// <param name="couponType">类别</param>
        /// <param name="couponFrom">形式</param>
        /// <param name="state">状态</param>
        /// <param name="storeIds">适用门店</param>
        /// <param name="expiryStart">有效期起始</param>
        /// <param name="expiryEnd">有效期截止</param>
        /// <param name="receiveStart">领取期限起始</param>
        /// <param name="receiveEnd">领取期限截止</param>
        /// <param name="createUID">创建人</param>
        /// <returns>制作优惠券列表</returns>
        internal DataTable FindCreateCouponPageList(Paging paging, int couponType, int couponFrom, short state, string storeIds, string expiryStart, string expiryEnd,string receiveStart,string receiveEnd, string createUID)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@CouponType", couponType),
                    new SqlParameter("@CouponFrom", couponFrom),
                    new SqlParameter("@State", state),
                    new SqlParameter("@StoreIds", storeIds),
                    new SqlParameter("@ExpiryStart", expiryStart),
                    new SqlParameter("@ExpiryEnd", expiryEnd),
                    new SqlParameter("@ReceiveStart", receiveStart),
                    new SqlParameter("@ReceiveEnd", receiveEnd),
                    new SqlParameter("@CreateUID", createUID),
                    new SqlParameter("@CurrentPage", paging.PageIndex),
                    new SqlParameter("@PageSize", paging.PageSize),
                    new SqlParameter("@CompanyId",Sys.SysCommonRules.CompanyId)
                                   };
            var result = db.DataTable("Rpt_MakingCouponCard", parms, ref paging);
            return result;
        }
        #endregion

        #region 新增或修改
        /// <summary>
        /// 获取品类列表
        /// </summary>
        /// <param name="ProductCode">品类，多个以逗号隔开</param>
        /// <returns>品类列表</returns>
        public DataTable GetCategoryList(string ProductCode)
        {
            string sql = @"SELECT 
                    dbo.F_ProductCategoryDescForSN(a.Value,1,1,{0}) BigCategoryTitle ,
                    dbo.F_ProductCategoryDescForSN(a.Value,2,1,{0}) MidCategoryTitle ,
                    dbo.F_ProductCategoryDescForSN(a.Value,3,1,{0}) SubCategoryTitle ,
                    a.Value as CategorySN
   	                from dbo.SplitString('" + ProductCode + "',',',1) a ";
            var dt = db.DataTableText(string.Format(sql,Sys.SysCommonRules.CompanyId), null);
            return dt;
        }

        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="ProductCode">品牌，多个以逗号隔开</param>
        /// <returns>品牌列表</returns>
        public DataTable GetBrandList(string ProductCode)
        {
            string sql = @"SELECT 
                    a.Id, a.BrandSN, a.Title, a.JianPin, (SELECT count(*) FROM dbo.ProductRecord WHERE BrandSN=a.BrandSN) Num, b.Title ClassifyTitle 
                    FROM dbo.ProductBrand a 
                    LEFT JOIN dbo.SysDataDictionary b ON a.ClassifyId=b.DicSN 
                    JOIN dbo.SplitString('" + ProductCode + "',',',1) c ON a.BrandSN=c.value";
            var dt = db.DataTableText(sql, null);
            return dt;
        }
        #endregion

        #region 领用优惠券列表
        /// <summary>
        /// 获取领用优惠券列表
        /// </summary>
        /// <param name="paging">分页信息</param>
        /// <param name="couponType">类别</param>
        /// <param name="couponFrom">形式</param>
        /// <param name="state">状态</param>
        /// <param name="storeIds">来源（即优惠券使用门店）</param>
        /// <param name="recipients">派发员</param>
        /// <param name="expiryStart">有效期起始</param>
        /// <param name="expiryEnd">有效期截止</param>
        /// <returns>领用优惠券列表</returns>
        internal DataTable FindTakeCouponPageList(Paging paging, int couponType, int couponFrom, short state, string storeIds, string recipients, string expiryStart, string expiryEnd)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@CouponType", couponType),
                    new SqlParameter("@CouponFrom", couponFrom),
                    new SqlParameter("@State", state),
                    new SqlParameter("@StoreIds", storeIds),
                    new SqlParameter("@Recipients", recipients),
                    new SqlParameter("@ExpiryStart", expiryStart),
                    new SqlParameter("@ExpiryEnd", expiryEnd),
                    new SqlParameter("@CurrentPage", paging.PageIndex),
                    new SqlParameter("@PageSize", paging.PageSize),
                    new SqlParameter("@CompanyId",Sys.SysCommonRules.CompanyId)
                                   };
            var result = db.DataTable("Rpt_CouponCardDetail", parms, ref paging);
            return result;
        }
        #endregion

    } 
}
