using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Pharos.Logic.OMS.IDAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using System.IO;
namespace Pharos.Logic.OMS.BLL
{
    public class PlanService
    {
        [Ninject.Inject]
        IBaseRepository<Plans> PlanRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<SysDataDictionary> DictRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<SysUser> UserRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Attachment> AttachRepository { get; set; }
        public OpResult SaveOrUpdate(Plans plan)
        {
            if(plan.Id.IsNullOrEmpty())
            {
                plan.Id = Guid.NewGuid().ToString("N");
                plan.CreateDT = DateTime.Now;
                plan.CreateUID = CurrentUser.UID;
                plan.Status = 375;
                PlanRepository.Add(plan, false);
            }
            else
            {
                var source = PlanRepository.GetQuery(o => o.Id == plan.Id).Include(o => o.Attachments).Include(o => o.Replys).FirstOrDefault();
                plan.ToCopyProperty(source, new List<string>() { "CreateDT", "CreateUID", "Status" });
                if (plan.Attachments!=null)
                    source.Attachments.AddRange(plan.Attachments);
                if (plan.Replys != null)
                    source.Replys.AddRange(plan.Replys);
            }
            if (plan.Attachments != null)
            {
                string path = "";
                string fullPath = FileHelper.SaveAttachPath(ref path);
                for (var i = plan.Attachments.Count - 1; i >= 0; i--)
                {
                    var attach = plan.Attachments[i];
                    if (!attach.Bytes.Any())
                    {
                        plan.Attachments.Remove(attach);
                        continue;
                    }
                    attach.CreateDT = DateTime.Now;
                    attach.CreateUID = CurrentUser.UID;
                    attach.ItemId = plan.Id;
                    attach.NewName = Guid.NewGuid().ToString("N") + "." + attach.ExtName;
                    attach.Path = path;
                    using (var fs = new FileStream(fullPath + attach.NewName, FileMode.OpenOrCreate))
                    {
                        fs.Write(attach.Bytes, 0, attach.Bytes.Length);
                        fs.Close();
                    }
                }
            }
            if(plan.Replys!=null)
            {
                plan.Replys.Each(o => {
                    o.CreateDT = DateTime.Now;
                    o.Creater = CurrentUser.FullName;
                    o.CreateUID = CurrentUser.UID;
                    o.PlanId = plan.Id;
                });
            }
            PlanRepository.SaveChanges();
            return OpResult.Success();
        }
        public object IndexPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var type = nvl["Type"].ToType<short>();
            var status = nvl["Status"].ToType<short>();
            var begin = nvl["CreateDT_begin"].ToType<DateTime?>();
            var end = nvl["CreateDT_end"].ToType<DateTime?>();
            if (end.HasValue) end = end.Value.AddDays(1);
            var search = (nvl["SearchText"] ?? "").Trim();
            var assignerUID = (nvl["AssignerUID"] ?? "").Trim();
            var all = nvl["all"];//是否所有计划
            var assigers = assignerUID.Split(',').ToList();
            var queryUser = UserRepository.GetQuery();
            var queryDict = DictRepository.GetQuery();
            var where = DynamicallyLinqHelper.Empty<Plans>().And(o => o.Type == type, type == 0).And(o => o.Status == status, status == 0)
                .And(o => o.CreateDT >= begin, !begin.HasValue).And(o => o.CreateDT < end, !end.HasValue).And(o => o.Content.Contains(search), search.IsNullOrEmpty());
            
            
            if (all.IsNullOrEmpty()) 
                where = where.And(o => (o.CreateUID == CurrentUser.UID || o.AssignerUID == CurrentUser.UID));
            else
            {
                
                if (CurrentUser.HasPermiss(255))//查看所有人员
                {
                    where = where.And(o => (o.Summary == null || o.Summary == ""));
                }
                else if (CurrentUser.HasPermiss(99))//查看本部门及以下
                {
                    var users= CurrentUser.GetAllCreateUIDByDeptId(CurrentUser.DeptId);
                    where = where.And(o =>(users.Contains(o.CreateUID) || o.LeaderUID.Contains(CurrentUser.UID)) && (o.Summary == null || o.Summary == ""));
                }
                else
                {
                    where = where.And(o => o.LeaderUID.Contains(CurrentUser.UID)).And(o => (o.Summary == null || o.Summary == ""));
                }
                where = where.And(o => assigers.Contains(o.AssignerUID), assignerUID.IsNullOrEmpty());
            }
            var query = PlanRepository.GetQuery(where).Include(o => o.Attachments).Include(o => o.Replys);
            var q = from x in query
                    select new
                    {
                        x.Id,
                        x.AssignerUID,
                        Assigner = queryUser.Where(o => o.UserId == x.AssignerUID).Select(o => o.FullName).FirstOrDefault(),
                        Creater = queryUser.Where(o => o.UserId == x.CreateUID).Select(o => o.FullName).FirstOrDefault(),
                        x.Content,
                        AttachCount = x.Attachments.Count,
                        x.CreateDT,
                        x.CreateUID,
                        x.StartDate,
                        x.EndDate,
                        ReplyCount = x.Replys.Count,
                        x.Status,
                        x.Type,
                        x.Summary,
                        StatuTitle = queryDict.Where(o => o.DicSN == x.Status).Select(o => o.Title).FirstOrDefault(),
                        TypeTitle = queryDict.Where(o => o.DicSN == x.Type).Select(o => o.Title).FirstOrDefault(),
                    };
            recordCount = q.Count();
            var list = q.ToPageList();
            return list;
        }
        public object MyReplyIndexPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var type = nvl["Type"].ToType<short>();
            var status = nvl["Status"].ToType<short>();
            var begin = nvl["CreateDT_begin"].ToType<DateTime?>();
            var end = nvl["CreateDT_end"].ToType<DateTime?>();
            if (end.HasValue) end = end.Value.AddDays(1);
            var search = (nvl["SearchText"] ?? "").Trim();
            var detail = nvl["detail"];//是否已批复
            var queryUser = UserRepository.GetQuery();
            var queryDict = DictRepository.GetQuery();
            var where = DynamicallyLinqHelper.Empty<Plans>().And(o => o.Type == type, type == 0).And(o => o.Status == status, status == 0)
                .And(o => o.CreateDT >= begin, !begin.HasValue).And(o => o.CreateDT < end, !end.HasValue).And(o => o.Content.Contains(search), search.IsNullOrEmpty())
                .And(o => o.LeaderUID.Contains(CurrentUser.UID)).And(o => !(o.Summary == null || o.Summary == ""));
            if (detail.IsNullOrEmpty()) where = where.And(o => !o.Replys.Any());
            else where = where.And(o => o.Replys.Any());
            var query = PlanRepository.GetQuery(where).Include(o => o.Attachments).Include(o => o.Replys);
            var q = from x in query
                    select new
                    {
                        x.Id,
                        x.AssignerUID,
                        Assigner = queryUser.Where(o => o.UserId == x.AssignerUID).Select(o => o.FullName).FirstOrDefault(),
                        Creater = queryUser.Where(o => o.UserId == x.CreateUID).Select(o => o.FullName).FirstOrDefault(),
                        x.Content,
                        AttachCount = x.Attachments.Count,
                        x.CreateDT,
                        x.CreateUID,
                        x.StartDate,
                        x.EndDate,
                        ReplyCount = x.Replys.Count,
                        x.Status,
                        x.Type,
                        x.Summary,
                        StatuTitle = queryDict.Where(o => o.DicSN == x.Status).Select(o => o.Title).FirstOrDefault(),
                        TypeTitle = queryDict.Where(o => o.DicSN == x.Type).Select(o => o.Title).FirstOrDefault(),
                    };
            recordCount = q.Count();
            var list = q.ToPageList();
            return list;
        }
        public Plans GetOne(string id)
        {
            return PlanRepository.GetQuery(o => o.Id == id).Include(o => o.Attachments).Include(o => o.Replys).FirstOrDefault() ?? new Plans() { Attachments=new List<Attachment>()};
        }

        public OpResult RemoveFile(string id, string name)
        {
            var obj = AttachRepository.GetQuery(o => o.ItemId == id && o.NewName == name).FirstOrDefault();
            if (obj == null) return OpResult.Fail("ID或参数名不存在！");
            AttachRepository.Remove(obj);
            var fileName = FileHelper.GetRoot + obj.Path + obj.NewName;
            File.Delete(fileName);
            return OpResult.Success();
        }
        public OpResult Preview(string id, string name)
        {
            var obj = AttachRepository.GetQuery(o => o.ItemId == id && o.NewName == name).FirstOrDefault();
            if (obj == null) return OpResult.Fail("ID或参数名不存在！");
            var fileName = FileHelper.GetUrlRoot + obj.Path.Replace("\\", "/") + obj.NewName;
            var fullName = FileHelper.GetRoot + obj.Path + obj.NewName;
            if (File.Exists(fullName))
            {
                if (obj.ExtName.StartsWith("doc", StringComparison.CurrentCultureIgnoreCase))
                {
                    var destFile = fullName.Substring(0, fullName.LastIndexOf(".") + 1) + "pdf";
                    if (!File.Exists(destFile))
                    {
                        Aspose.Words.Document doc = new Aspose.Words.Document(fullName);
                        doc.Save(destFile, Aspose.Words.SaveFormat.Pdf);
                    }
                }
                else if (obj.ExtName.StartsWith("txt", StringComparison.CurrentCultureIgnoreCase))
                {
                    var destFile = fullName.Substring(0, fullName.LastIndexOf(".") + 1) + "pdf";
                    if (!File.Exists(destFile))
                    {
                        var opt=new Aspose.Words.LoadOptions();
                        opt.LoadFormat = Aspose.Words.LoadFormat.Text;
                        opt.Encoding = System.Text.Encoding.GetEncoding("gb2312");
                        Aspose.Words.Document doc = new Aspose.Words.Document(fullName,opt);
                        doc.Save(destFile, Aspose.Words.SaveFormat.Pdf);
                    }
                }
                else if (obj.ExtName.StartsWith("xls", StringComparison.CurrentCultureIgnoreCase))
                {
                    var destFile = fullName.Substring(0, fullName.LastIndexOf(".") + 1) + "pdf";
                    if (!File.Exists(destFile))
                    {
                        Aspose.Cells.Workbook doc = new Aspose.Cells.Workbook(fullName);
                        doc.Save(destFile, Aspose.Cells.SaveFormat.Pdf);
                    }
                }
            }
            fileName = fileName.Substring(0, fileName.LastIndexOf(".") + 1) + "pdf";
            return OpResult.Success(fileName);
        }
        public string DownLoad(int id)
        {
            var obj= AttachRepository.Get(id);
            if(obj!=null)
            {
                var fileName = FileHelper.GetRoot + obj.Path + obj.NewName;
                return fileName;
            }
            return "";
        }

        public OpResult DeletePlan(string[] ids)
        {
            var list= PlanRepository.GetQuery(o => ids.Contains(o.Id)).Include(o => o.Attachments).Include(o => o.Replys).ToList();
            var replys= list.SelectMany(o => o.Replys).ToList();
            if (replys.Any())
                return OpResult.Fail("已批复不能删除！");
            if(list.Any(o=>o.CreateUID!=CurrentUser.UID))
                return OpResult.Fail("非本人创建不能删除！");
            var attachs = list.SelectMany(o => o.Attachments).ToList();
            AttachRepository.RemoveRange(attachs, false);
            PlanRepository.RemoveRange(list);
            attachs.Each(o =>
            {
                var fileName = FileHelper.GetRoot + o.Path + o.NewName;
                File.Delete(fileName);
            });
            return OpResult.Success();
        }

        public OpResult UpdateStatus(string ids, short status)
        {
            var id = ids.Split(',');
            var list= PlanRepository.GetQuery(o => id.Contains(o.Id)).ToList();
            list.Each(o => o.Status = status);
            PlanRepository.SaveChanges();
            return OpResult.Success();
        }
    }
}
