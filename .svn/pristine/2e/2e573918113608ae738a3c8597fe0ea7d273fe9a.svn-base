using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Pharos.Sys.DAL;
using Pharos.Utility;
using Pharos.Utility.Helpers;

namespace Pharos.Sys.BLL
{
    public class OMSCompanyAuthrizeBLL
    {
        public static List<Entity.OMS_CompanyAuthorize> FindPageList(NameValueCollection parms, out int count)
        {
            return NinjectData.OMSCompanyAuthrizeDAL.FindPageList(parms,out count);
        }
        public static Entity.OMS_CompanyAuthorize GetById(int id)
        {
            return NinjectData.OMSCompanyAuthrizeDAL.GetById(id);
        }
        public static Entity.OMS_CompanyAuthorize GetByCode(int code)
        {
            return NinjectData.OMSCompanyAuthrizeDAL.GetByColumn(code, "Code");
        }
        public static OpResult Save(Entity.OMS_CompanyAuthorize obj)
        {
            var op=new OpResult();
            try
            {
                obj.AppProper = obj.AppProper ?? "N";
                obj.StoreProper = obj.StoreProper ?? "N";
                obj.SupplierProper = obj.SupplierProper ?? "N";
                obj.WholesalerProper = obj.WholesalerProper ?? "N";
                obj.Useable = obj.Useable ?? "N";
                obj.PosMinorDisp = obj.PosMinorDisp ?? "N";
                if(obj.Id==0)
                {
                    obj.CreateDT = DateTime.Now;
                    obj.Code = NinjectData.OMSCompanyAuthrizeDAL.MaxVal("Code",SysCommonRules.CompanyId);
                    obj.Code = obj.Code < 101 ? 101 : obj.Code + 1;
                }
                else
                {
                    var res= NinjectData.OMSCompanyAuthrizeDAL.GetById(obj.Id);
                    if(res!=null)
                    {
                        obj.CreateDT = res.CreateDT;
                        if(new SysAuthorize().ValidateCompany(obj,res))
                            obj.SerialNo = res.SerialNo;
                    }
                }
                if (op.Message.IsNullOrEmpty() && NinjectData.OMSCompanyAuthrizeDAL.SaveOrUpdate(obj))
                    op.Successed = true;
            }catch(Exception ex)
            {
                op.Message = ex.Message;
                new LogEngine().WriteError("授权保存失败!",ex);
            }
            return op;
        }
        public static OpResult Update(Entity.OMS_CompanyAuthorize obj)
        {
            var op = new OpResult();
            try
            {
                if (obj.Id == 0)
                    op = Save(obj);
                else
                {
                    var res = NinjectData.OMSCompanyAuthrizeDAL.GetById(obj.Id);
                    if (res != null)
                    {
                        res.Title = obj.Title;
                        res.FullTitle = obj.FullTitle;
                        res.Category = obj.Category;
                        res.LinkMan = obj.LinkMan;
                        res.Phone = obj.Phone;
                        res.UserNum = obj.UserNum;
                        res.StoreNum = obj.StoreNum;
                        res.SerialNo = obj.SerialNo;
                        obj.Code = res.Code;
                    }
                    op.Successed = NinjectData.OMSCompanyAuthrizeDAL.SaveOrUpdate(res);
                }
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                new LogEngine().WriteError("授权保存失败!", ex);
            }
            return op;
        }
        public static OpResult Delete(object[] ids)
        {
            try
            {
                if (NinjectData.OMSCompanyAuthrizeDAL.Delete(ids))
                    return OpResult.Success();
                return OpResult.Fail("存在问题!");
            }
            catch(Exception ex)
            {
                return OpResult.Fail(ex.Message);
            }
        }
        public static OpResult SetState(string ids, short state)
        {
            var op = new OpResult();
            try
            {
                ids = ids.TrimEnd(',');
                if (NinjectData.OMSCompanyAuthrizeDAL.SetState(ids, state))
                    op.Successed = true;
            }
            catch(Exception ex)
            {
                new LogEngine().WriteError("授权保存失败!", ex);
                op.Message = ex.Message;
            }
            return op;
        }
    }
}
