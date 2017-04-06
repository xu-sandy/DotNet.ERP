using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using System.Data;
using System.Data.Entity;
using Pharos.Utility;
using Pharos.DBFramework;
using System.IO;
using System.Collections.Specialized;
using Pharos.Utility.Helpers;
using Pharos.Sys.Entity;

namespace Pharos.Logic.BLL
{
    public class ContractBLL : BaseService<Contract>
    {
        DAL.ContractDAL dal = new DAL.ContractDAL();

        public static Contract FindById(string id)
        {
            var obj = CurrentRepository.QueryEntity.Include(o => o.ContractBoths).Include(o => o.Attachments).FirstOrDefault(o => o.Id == id);
            return obj;
        }

        /// <summary>
        /// 根据合同状态，创建人，签订日期查询合同列表
        /// </summary>
        /// <param name="state">合同状态</param>
        /// <param name="createUID">创建人</param>
        /// <param name="beginSigningDate">开始签订日期</param>
        /// <param name="endSigningDate">结束签订日期</param>
        /// <returns>合同列表</returns>
        public object GetContractListBySearch(int state, string createUID, string beginSigningDate, string endSigningDate, out int recordCount)
        {
            DataTable dt = dal.GetContractListBySearch(state, createUID, beginSigningDate, endSigningDate, out recordCount);
            var list = DBHelper.ToEntity.ToList<Contract>(dt);
            if (list == null || !list.Any()) return null;

            var classify = SysDataDictService.GetContractClassify();

            var obj = list.Select(c => new
            {
                c.Id,
                c.ContractSN,
                c.State,
                c.Title,
                Version = (c.Version == 1) ? ("新增 v" + c.Version + ".0") : ("续签 v" + c.Version + ".0"),
                c.ClassifyId,
                c.StartDate,
                c.EndDate,
                CreateDT = c.CreateDT.ToString("yyyy-MM-dd"),
                c.CreateUID,
                c.CreateTitle,
                StateTitle = Enum.GetName(typeof(ContractState), c.State),
                c.SupplierTitle,
                ClassifyTitle = GetClassifyTitle(classify, c.ClassifyId),
                c.AttCount,
                c.isExtend
            }).ToList();
            return obj;
        }

        public static Contract GetObj(string id)
        {
            return CurrentRepository.QueryEntity.Include(o => o.Attachments).Include(o => o.ContractBoths).FirstOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// 获取合同分类名称
        /// </summary>
        /// <param name="classify">数据字典的所有合同分类</param>
        /// <param name="classifyId">合同分类Id</param>
        /// <returns>合同分类名称</returns>
        static string GetClassifyTitle(List<SysDataDictionary> classify, int classifyId)
        {
            var obj = classify.FirstOrDefault(o => o.DicSN == classifyId);
            if (obj == null) return classifyId.ToString();
            return obj.Title;
        }

        /// <summary>
        /// 删除合同（可以批量删除）
        /// </summary>
        /// <param name="ids">Id</param>
        /// <returns>操作结果</returns>

        public static Pharos.Utility.OpResult Delete(string[] ids)
        {
            var list = CurrentRepository.QueryEntity.Where(o => ids.Contains(o.Id)).Include(o => o.ContractBoths).Include(o => o.Attachments).ToList();
            var files = list.SelectMany(o => o.Attachments).ToList();
            AttachService.CurrentRepository.RemoveRange(files, false);
            var op = Delete(list);
            if (op.Successed)
            {
                #region 操作日志
                foreach (var item in list)
                {
                    var msg = Pharos.Sys.LogEngine.CompareModelToLog<Contract>(Sys.LogModule.合同管理, null, item);
                    new Pharos.Sys.LogEngine().WriteDelete(msg, Sys.LogModule.合同管理);
                }
                #endregion
                var root = Sys.SysConstPool.GetRoot;
                foreach (var att in files)
                {
                    var full = Path.Combine(root, att.SaveUrl);
                    if (File.Exists(full)) File.Delete(full);
                }
            }
            return op;
        }

        /// <summary>
        /// 生成合同编号
        /// </summary>
        /// <param name="id">合同id</param>
        /// <returns>合同编号</returns>
        public static string CreateContractSN()
        {
            var firstPart = DateTime.Now.ToString("yyyyMMdd");
            var minToday = DateTime.Parse(DateTime.Now.ToShortDateString());
            var maxToday = minToday.AddDays(1);
            int contractCount = ContractBLL.FindList(o => o.CreateDT >= minToday && o.CreateDT < maxToday).Count();
            string contractSN = "";
            string firstContractSN = firstPart + "0001";
            if (contractCount == 0)
            {
                contractSN = firstContractSN;
            }
            else
            {
                contractSN = (long.Parse(firstContractSN) + contractCount).ToString();
            }
            return contractSN;
        }
    }
}
