using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using System.Collections.Specialized;
using Pharos.Utility.Helpers;
using System.Data;
using System.Data.Entity;
using Pharos.Utility;
using System.IO;
using Pharos.Sys.Entity;
namespace Pharos.Logic.BLL
{
    public class ReceiptsBLL : BaseService<Receipts>
    {
        //DAL.ReceiptsDAL dal = new DAL.ReceiptsDAL();
        public new static Receipts FindById(object id, params string[] includeParams)
        {
            var code = id.ToString();
            var obj = CurrentRepository.QueryEntity.Include(o => o.Attachments).FirstOrDefault(o => o.Id == code);
            return obj;
        }
        public static OpResult SaveOrUpdate(Receipts obj, System.Web.HttpFileCollectionBase httpFiles)
        {
            var re = new OpResult();
            if(IsExist(o=>o.Number==obj.Number && o.Id!=obj.Id))
            {
                re.Message = "已存在该单据编号";
                return re;
            }
            var files = new List<Attachment>();
            var relativePath = "";
            var path = Sys.SysConstPool.SaveAttachPath(ref relativePath);
            for (int i = 0; i < httpFiles.Count; i++)
            {
                var file = httpFiles[i];
                if (file.ContentLength <= 0) continue;
                var filename = CommonRules.GUID + Path.GetExtension(file.FileName);
                string fullname = path + filename;
                file.SaveAs(fullname);
                files.Add(new Attachment()
                {
                    SourceClassify = 2,
                    Title = Path.GetFileName(file.FileName),
                    Size = file.ContentLength / 1024,
                    SaveUrl = relativePath + filename
                });
            }
            if (string.IsNullOrWhiteSpace(obj.Id))
            {
                obj.Source = 1;
                obj.Id = CommonRules.GUID;
                obj.CreateUID = Sys.CurrentUser.UID;
                obj.StoreId = Sys.CurrentUser.StoreId;
                obj.CreateDT = DateTime.Now;
                obj.Attachments = files;
                re = Add(obj);
            }
            else
            {
                var sour = FindById(obj.Id);
                obj.ToCopyProperty(sour);
                sour.Attachments.AddRange(files);
                re = Update(sour);
            }
            return re;
        }
        public static object FindPageList(NameValueCollection nvl, out int recordCount)
        {
            //var queryUser= CurrentRepository._context.Set<SysUserInfo>();
            //var queryData = CurrentRepository._context.Set<SysDataDictionary>();
            //var queryRecord = CurrentRepository._context.Set<Receipts>();
            var queryUser = BaseService<SysUserInfo>.CurrentRepository.QueryEntity;
            var queryRecord = CurrentRepository.QueryEntity;
            var queryData = BaseService<SysDataDictionary>.CurrentRepository.QueryEntity;
            var query = from x in queryRecord
                        join z in queryData on x.CategoryId equals z.DicSN into tempCate
                        from i in tempCate.DefaultIfEmpty()
                        select new
                        {
                            x.Amount,
                            x.CategoryId,
                            x.Id,
                            x.CreateDT,
                            x.CreateUID,
                            x.Memo,
                            x.Number,
                            x.Pages,
                            x.State,
                            x.Title,
                            CategoryTitle=i.Title,
                            x.Source
                        };


            var Category = nvl["CategoryId"];
            var CreateId = nvl["CreateId"];
            var SupplierId = nvl["SupplierId"];
            var State = nvl["State"];

            if (!Category.IsNullOrEmpty())
            {
                var cate = int.Parse(Category);
                query = query.Where(o => o.CategoryId == cate);//这个是要用OperatorUID还是Operator
            }
            if (!CreateId.IsNullOrEmpty())
            {
                query = query.Where(o => o.CreateUID == CreateId);
            }
            if (!SupplierId.IsNullOrEmpty())
            {
                query = query.Where(o => o.CreateUID == SupplierId);
            }
            if (!State.IsNullOrEmpty())
            {
                var sta = short.Parse(State);
                query = query.Where(o => o.State == sta);
            }
           
            //recordCount = query.Count();
            //var sql = query.ToString();
            //var list= query.ToPageList();
            //return list.Select(x => new
            recordCount = query.Count();
            var list= query.ToPageList(nvl);
            var userIds = list.Where(o => o.Source == 1).Select(o => o.CreateUID).ToList();
            var supplierIds = list.Where(o => o.Source == 2).Select(o => o.CreateUID).ToList();
            var users= queryUser.Where(o=>userIds.Contains(o.UID)).ToList();
            var suppliers=SupplierService.FindList(o=>supplierIds.Contains(o.Id));
            return list.Select(x => new
            {
                x.Amount,
                x.CategoryId,
                x.Id,
                x.CreateDT,
                x.CreateUID,
                x.Memo,
                x.Number,
                x.Pages,
                x.State,
                x.Title,
                CreateTitle=CreatorTitle(x.Source,x.CreateUID,suppliers,users),
                x.CategoryTitle,
                StateTitle = GetStateTitle(x.State),
                SourceTitle=x.Source==1?"本公司":"供应商"
            });
        }


        public static OpResult Delete(string[] ids)
        {
            var op=new OpResult();
            var list= CurrentRepository.QueryEntity.Where(o => ids.Contains(o.Id)).Include(o => o.Attachments).ToList();
            if (!list.Any(o => o.CreateUID == Sys.CurrentUser.UID))
            {
                op.Message = "非交单人不允许删除!";
                return op;
            }
            var files = list.SelectMany(o => o.Attachments).ToList();
            AttachService.CurrentRepository.RemoveRange(files, false);
            op = Delete(list);
            if (op.Successed)
            {
                var root = Sys.SysConstPool.GetRoot;
                foreach (var att in files)
                {
                    var full = Path.Combine(root, att.SaveUrl);
                    if (File.Exists(full)) File.Delete(full);
                }
            }
            return op;
        }
        ///// <summary>
        ///// 根据类别，交单人，状态查询单据列表
        ///// </summary>
        ///// <param name="categoryId">类别</param>
        ///// <param name="createUID">交单人</param>
        ///// <param name="state">状态</param>
        ///// <returns>单据列表</returns>
        //public object GetReceiptsListBySearch(int? categoryId, int? createUID, int? state)
        //{
        //    DataTable dt = dal.GetReceiptsListBySearch(categoryId, createUID, state);

        //    var category = SysDataDictService.GetReceiptsCategories();
        //    var list= DBHelper.ToEntity.ToList<Receipts>(dt);
        //    var obj= list.Select(o => new { 
        //        o.Amount,
        //        o.CreateDT,
        //        o.CreateUID,
        //        o.Id,
        //        o.Memo,
        //        o.Number,
        //        o.Pages,
        //        o.State,
        //        o.StoreId,
        //        o.Title,
        //        o.CreateTitle,
        //        StateTitle=GetStateTitle(o.State),
        //        CategoryTitle = GetCategoryTitle(category, o.CategoryId)
        //    });
        //    return obj;
        //}
        static string GetStateTitle(short state)
        {
            var name = Enum.GetName(typeof(ReceipState), state);
            return name;
        }
        static string CreatorTitle(short source,string id,List<Supplier> suppliers,List<SysUserInfo> users)
        {
            if(source==1)
            {
                var obj= users.FirstOrDefault(o => o.UID == id);
                if (obj == null) return "";
                return obj.FullName;
            }
            else
            {
                var obj = suppliers.FirstOrDefault(o => o.Id == id);
                if (obj == null) return "";
                return obj.Title;
            }
        }
        //static string GetCategoryTitle(List<SysDataDictionary> category, int categoryId)
        //{
        //    var obj = category.FirstOrDefault(o => o.DicSN == categoryId);
        //    if (obj == null) return categoryId.ToString();
        //    return obj.Title;
        //}

    }
}
