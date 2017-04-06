﻿using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.ApiData.Mobile.Exceptions;
using Pharos.Logic.ApiData.Mobile.Repository;
namespace Pharos.Logic.BLL
{
    public class NoticeService : BaseService<Notice>
    {
        static CommonDAL dal = new CommonDAL();
        static Sys.LogEngine log = new Sys.LogEngine();
        /// <summary>
        /// 获得最新的公告
        /// </summary>
        /// <param name="takeNum">需要取出的条数</param>
        /// <returns></returns>
        public static List<Notice> GetNewestNotice(int takeNum)
        {
            var now = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            var query = CurrentRepository.QueryEntity.Where(n =>n.CompanyId==Sys.SysCommonRules.CompanyId && n.State == 1);
            if (!Sys.CurrentUser.StoreId.IsNullOrEmpty())
                query = query.Where(o => ("," + o.StoreId + ",").Contains("," + Sys.CurrentUser.StoreId + ","));
            return query.OrderByDescending(n => n.CreateDT).Take(takeNum).ToList();
        }
        /// <summary>
        /// APP调用
        /// </summary>
        /// <returns></returns>
        public static object GetNoticeList(string userCode)
        {
            if (userCode.IsNullOrEmpty()) throw new MessageException("用户编号为空!");
            var user= UserInfoService.Find(o => o.CompanyId == Sys.SysCommonRules.CompanyId && o.UserCode == userCode);
            if (user == null) throw new MessageException("用户不存在!");
            var dt= dal.GetNoticeList(user.UID,false);
            if (dt.Rows.Count > 0)
            {
                var ids = dt.AsEnumerable().Select(o => (int)o["id"]).ToList();
                ReaderService.Add(1, user.UID, ids);
            }
            return dt;
        }
        /// <summary>
        /// 未读条数
        /// </summary>
        /// <param name="sId"></param>
        /// <returns></returns>
        public static object GetNoticeNum(string userCode)
        {
            if (userCode.IsNullOrEmpty()) throw new MessageException("用户编号为空!");
            var user = UserInfoService.Find(o => o.CompanyId == Sys.SysCommonRules.CompanyId && o.UserCode == userCode);
            if (user == null) throw new MessageException("用户不存在!");
            var dt = dal.GetNoticeList(user.UID, true);
            return dt.Rows.Count;
        }
        /// <summary>
        /// 用于datagrid列表
        /// </summary>
        /// <param name="nvl">传递条件</param>
        /// <param name="recordCount">返回总行数</param>
        /// <returns>list</returns>
        public static object FindPageList(NameValueCollection nvl, out int recordCount)
        {
            var query = CurrentRepository.QueryEntity.Where(o=>o.CompanyId==CommonService.CompanyId);
            var keyword = nvl["keyword"].Trim();
            if (!keyword.IsNullOrEmpty())
                query = query.Where(n => n.Theme.Contains(keyword));
            if (!Sys.CurrentUser.StoreId.IsNullOrEmpty())
                query = query.Where(o => ("," + o.StoreId + ",").Contains(","+Sys.CurrentUser.StoreId+",") || o.StoreId.Contains("-1"));
            recordCount = query.Count();
            var pages = query.ToPageList(nvl).Select(o => new { 
                    Id = o.Id,
                    Theme = o.Theme,
                    NoticeContent = o.NoticeContent,
                    StoreId = o.StoreId,
                    State = o.State,
                    Type = o.Type,
                    CreateDT = o.CreateDT,
                    CreateUID = o.CreateUID,
                    ExpirationDateStr = o.ExpirationDate.ToString("yyyy-MM-dd"),
                    BeginDateStr = o.BeginDate.ToString("yyyy-MM-dd")
            });
            return pages;
        }

        /// <summary>
        /// 保存或修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static OpResult NoticeSaveOrUpdate(Notice obj)
        {
            var re = new OpResult();
            try
            {
                if (obj.Id == 0)
                {
                    obj.CreateDT = DateTime.Now;
                    obj.CreateUID = Sys.CurrentUser.UID;
                    obj.CompanyId = CommonService.CompanyId;
                    re = Add(obj);
                    #region 写入日志
                    string msg = "成功新增公告！";
                    var module = Pharos.Sys.LogModule.通知公告;
                    if (re.Successed)
                    {
                        string TypeName = "", storeTitle = "";
                        TypeName = (obj.Type == 1 ? "公告" : "活动");
                        storeTitle = obj.StoreId == "0" ? "后台系统" : CommonService.GetStoreTitleList(obj.StoreId);
                        msg += "<br />Id=" + obj.Id + "，";
                        msg += "<br />显示范围=" + storeTitle + "，类型=" + TypeName + "，状态=" + Enum.GetName(typeof(NoticeState), obj.State) + "，主题=" + obj.Theme + "，开始日期=" + obj.BeginDate.ToString("yyyy-MM-dd") + "，截止日期=" + obj.ExpirationDate.ToString("yyyy-MM-dd") + "。";
                    }
                    else
                    {
                        msg = "新增公告失败！";
                    }                  
                    log.WriteInsert(msg, module);
                    #endregion
                }
                else
                {
                    var notice = CurrentRepository.QueryEntity.FirstOrDefault(n => n.Id == obj.Id);
                    var isUpdateStore = obj.StoreId != notice.StoreId;
                    var isUpdateType = obj.Type != notice.Type;
                    var isUpdateState = obj.State != notice.State;
                    var isUpdateTheme = obj.Theme != notice.Theme;
                    var isUpdateBegindate = obj.BeginDate.ToString("yyyy-MM-dd") != notice.BeginDate.ToString("yyyy-MM-dd");
                    var isUpdateExpirationdate = obj.ExpirationDate.ToString("yyyy-MM-dd") != notice.ExpirationDate.ToString("yyyy-MM-dd");
                    var isUpdateContent = obj.NoticeContent != notice.NoticeContent;
                    bool isUpdate = isUpdateStore || isUpdateType || isUpdateState || isUpdateTheme || isUpdateBegindate || isUpdateExpirationdate || isUpdateContent;

                    if (notice != null)
                    {
                        notice.Theme = obj.Theme;
                        notice.NoticeContent = obj.NoticeContent;
                        notice.StoreId = obj.StoreId;
                        notice.State = obj.State;
                        notice.Type = obj.Type;
                        notice.BeginDate = obj.BeginDate;
                        notice.ExpirationDate = obj.ExpirationDate;
                        re = Update(notice);
                        #region 写入日志
                        string msg = "成功修改公告！";
                        var module = Pharos.Sys.LogModule.通知公告;
                        if (re.Successed)
                        {
                            int n = 0;
                            if(isUpdate)
                            {
                                msg += "<br />Id=" + obj.Id + "，<br />";                           
                                if (isUpdateStore)
                                {
                                    var storeTitle = obj.StoreId == "0" ? "后台系统" : CommonService.GetStoreTitleList(obj.StoreId);
                                    msg += "显示范围=" + storeTitle;
                                    n = n + 1;
                                }
                                if (isUpdateType)
                                {
                                    var TypeName = (obj.Type == 1 ? "公告" : "活动");
                                    msg += n > 0 ? "，类型=" + TypeName : "类型=" + TypeName;
                                    n = n + 1;
                                }
                                if (isUpdateState)
                                {
                                    var StateTitle = Enum.GetName(typeof(NoticeState), obj.State);
                                    msg += n > 0 ? "，状态=" + StateTitle : "状态=" + StateTitle;
                                    n = n + 1;
                                }
                                if (isUpdateTheme)
                                {
                                    msg += n > 0 ? "，主题=" + obj.Theme : "主题=" + obj.Theme;
                                    n = n + 1;
                                }
                                if(isUpdateBegindate)
                                {
                                    msg += n > 0 ? "，开始日期=" + obj.BeginDate.ToString("yyyy-MM-dd") : "开始日期=" + obj.BeginDate.ToString("yyyy-MM-dd");
                                    n = n + 1;
                                }
                                if (isUpdateExpirationdate)
                                {
                                    msg += n > 0 ? "，开始日期=" + obj.ExpirationDate.ToString("yyyy-MM-dd") : "开始日期=" + obj.ExpirationDate.ToString("yyyy-MM-dd");
                                    n = n + 1;
                                }
                                if (isUpdateContent)
                                {
                                    msg += n > 0 ? "，详细内容=" + obj.NoticeContent : "详细内容=" + obj.NoticeContent;
                                }
                                msg += "。";
                                log.WriteUpdate(msg, module);
                            }                    
                        }
                        else
                        {
                            msg = "修改公告失败！";
                            log.WriteUpdate(msg, module);
                        }                       
                        #endregion
                    }
                }
                if (re.Successed)
                {
                    Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = obj.CompanyId, StoreId = obj.StoreId, Target = "Notice" });
                }
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                Log.WriteError(ex);
            }
            return re;
        }
        /// <summary>
        /// 更改通知状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static OpResult ChangeState(long[] ids, int type)
        {
            var re = new OpResult();
            try
            {
                List<Notice> noticeList = new List<Notice>();
                foreach (var id in ids) {
                    var notice = CurrentRepository.QueryEntity.FirstOrDefault(n => n.Id == id);
                    if (notice != null)
                    {
                        notice.State = (short)type;
                        noticeList.Add(notice);
                    }
                    re = Update(noticeList);
                    #region 写入日志
                    string msg = "成功修改公告状态！";
                    var module = Pharos.Sys.LogModule.通知公告;
                    if (re.Successed)
                    {
                        var StateTitle = Enum.GetName(typeof(NoticeState), type);
                        msg += "<br />Id=" + id + "，<br />状态=" + StateTitle + "。";
                    }
                    else
                    {
                        msg = "修改公告状态失败！";
                    }
                    log.WriteUpdate(msg, module);
                    #endregion
                }
            }
            catch(Exception ex)
            {
                re.Message = ex.Message;
                Log.WriteError(ex);
            }
            return re;

        }

        /// <summary>
        /// 删除所选通知
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static OpResult DeleteByIds(long[] ids)
        {
            var re = new OpResult();
            try
            {
                List<Notice> noticeList = new List<Notice>();
                foreach (var id in ids)
                {
                    var notice = CurrentRepository.QueryEntity.FirstOrDefault(n => n.Id == id);
                    if (notice != null)
                    {
                        noticeList.Add(notice);
                    }               
                } 
                if(CurrentRepository.RemoveRange(noticeList, true))
                        re.Successed = true;
                #region 写入日志
                string msg = "成功删除公告！";
                var module = Pharos.Sys.LogModule.通知公告;
                if (re.Successed)
                {
                    for (int i = 0; i < noticeList.Count(); i++)
                    {
                        var noticeTemp=noticeList[i];
                        var TypeName = (noticeTemp.Type == 1 ? "公告" : "活动");
                        msg += "<br />Id=" + noticeTemp.Id + "，<br />类型=" + TypeName + ",状态=" + Enum.GetName(typeof(NoticeState), noticeTemp.State) + "，主题=" + noticeTemp.Theme + "，开始日期=" + noticeTemp.BeginDate.ToString("yyyy-MM-dd") + "，截止日期=" + noticeTemp.ExpirationDate.ToString("yyyy-MM-dd") + "。";
                        log.WriteDelete(msg, module);
                        msg = "成功删除公告！";
                    }                                            
                }
                else
                {
                    msg = "删除公告失败！";
                    log.WriteDelete(msg, module);
                }
                #endregion
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                Log.WriteError(ex);
            }
            return re;

        }
    }

}