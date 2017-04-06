using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using System.Collections.Specialized;
using Pharos.Sys.Entity;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using System.IO;
using System.Data;
using System.Data.Entity;

namespace Pharos.Logic.BLL
{
    public class InvoiceBLL:BaseService<Receipts>
    {
        public new static Receipts FindById(object id, params string[] includeParams)
        {
            var code = id.ToString();
            var obj = CurrentRepository.QueryEntity.Include(o => o.Attachments).FirstOrDefault(o => o.Id == code);
            return obj;
        }

        /// <summary>
        /// 单据列表
        /// </summary>
        /// <param name="nvl">查询条件</param>
        /// <param name="recordCount">返回总行数</param>
        /// <returns>列表</returns>
        public static object FindPageList(NameValueCollection nvl, out int recordCount)
        {
            var queryReceipts = BaseService<Receipts>.CurrentRepository.QueryEntity.Where(o=>o.CompanyId==CommonService.CompanyId);
            var querySupplier = BaseService<Supplier>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryData = BaseService<SysDataDictionary>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var query = from x in queryReceipts
                        join y in querySupplier on x.CreateUID equals y.Id into tempSupplier
                        join z in queryData on x.CategoryId equals z.DicSN into tempCate
                        from h in tempSupplier.DefaultIfEmpty()
                        from i in tempCate.DefaultIfEmpty()
                        where (x.CreateUID==Pharos.Sys.SupplierUser.SupplierId && x.Source==2 )
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
                            CreateTitle = h.Title,
                            CategoryTitle = i.Title
                        };
            var Category = nvl["CategoryId"];
            //var CreateId = nvl["CreateId"];
            var State = nvl["State"];

            if (!Category.IsNullOrEmpty())
            {
                var cate = int.Parse(Category);
                query = query.Where(o => o.CategoryId == cate);//这个是要用OperatorUID还是Operator
            }
            //if (!CreateId.IsNullOrEmpty())
            //{
            //    query = query.Where(o => o.CreateUID == CreateId);
            //}
            if (!State.IsNullOrEmpty())
            {
                var sta = short.Parse(State);
                query = query.Where(o => o.State == sta);
            }

            string userID = Pharos.Sys.SupplierUser.SupplierId;
            query = query.Where(o=>o.CreateUID == userID);

            recordCount = query.Count();
            return query.ToPageList(nvl).Select(x => new
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
                x.CreateTitle,
                x.CategoryTitle,
                StateTitle = GetStateTitle(x.State)
            });
        }
        
        static string GetStateTitle(short state)
        {
            var name = Enum.GetName(typeof(ReceipState), state);
            return name;
        }

        /// <summary>
        /// 新增/修改单据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="httpFiles"></param>
        /// <returns></returns>
        public static OpResult SaveOrUpdate(Receipts obj, System.Web.HttpFileCollectionBase httpFiles)
        {
            var re = new OpResult();
            if (IsExist(o => o.Number == obj.Number && o.Id != obj.Id))
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
            obj.CompanyId = CommonService.CompanyId;
            if (string.IsNullOrWhiteSpace(obj.Id))
            {
                obj.Id = CommonRules.GUID;
                obj.CreateUID = Pharos.Sys.SupplierUser.SupplierId;
                //obj.StoreId = CurrentUser.StoreId;
                obj.CreateDT = DateTime.Now;
                obj.Source = 2;
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

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OpResult Delete(string[] ids)
        {
            var op = new OpResult();
            var list = CurrentRepository.QueryEntity.Where(o => ids.Contains(o.Id)).Include(o => o.Attachments).ToList();
            //if (!list.Any(o => o.CreateUID == Pharos.Sys.SupplierUser.SupplierId))
            //{
            //    op.Message = "非交单人不允许删除!";
            //    return op;
            //}
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

    }
}
