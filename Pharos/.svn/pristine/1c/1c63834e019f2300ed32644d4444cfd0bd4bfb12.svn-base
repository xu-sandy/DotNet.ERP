using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Pharos.Logic.OMS.Entity.View;


namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// BLL-----回访跟踪记录
    /// </summary>
    public class VisitTrackService : BaseService<VisitTrack>
    {
        [Ninject.Inject]
        // 回访跟踪记录
        public IBaseRepository<VisitTrack> VisitTrackRepository { get; set; }

        [Ninject.Inject]
        //帐户管理
        public IBaseRepository<SysUser> SysUserInfoRepository { get; set; }

        public Utility.OpResult SaveOrUpdate(VisitTrack model)
        {
            //model.CreateDT = DateTime.Now;
            if (model.Id == 0)
            {
                VisitTrackRepository.Add(model);
            }
            else
            {
                var source = VisitTrackRepository.Get(model.Id);
                model.ToCopyProperty(source, new List<string>() { "CreateDT", "CID" });
            }

            if (VisitTrackRepository.SaveChanges())
            {
                LogEngine.WriteUpdate("记录ID：" + model.Id, LogModule.回访跟踪记录);
            }
            return OpResult.Success();
        }

        /// <summary>
        /// 获取回访小结
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public List<ViewVisitTrack> getVisitTrackList(int CID)
        {
            var visit = VisitTrackRepository.GetQuery(o => o.CID == CID);
            var sysUserInfo = SysUserInfoRepository.GetQuery();

            var vis = from v in visit
                      join s in sysUserInfo on v.CreateUID equals s.UserId
                      into ss
                      from sss in ss.DefaultIfEmpty()
                      orderby v.VisitDT
                      select new ViewVisitTrack
                      {
                          VisitDT = v.VisitDT,
                          Content = v.Content,
                          FullName = sss == null ? "" : sss.FullName,
                          CreateUID = v.CreateUID,
                          CreateDT=v.CreateDT,
                          UpdateDT=v.UpdateDT
                      };


            return vis.ToList();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CID">企业ID</param>
        /// <returns></returns>
        public OpResult Deletes(int CID)
        {
            var list = VisitTrackRepository.GetQuery(o => o.CID == CID).ToList();
            VisitTrackRepository.RemoveRange(list);
            return OpResult.Success();
        }
    }
}
