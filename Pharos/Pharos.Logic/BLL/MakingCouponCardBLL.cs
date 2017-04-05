﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Pharos.Utility;
using Pharos.Logic.Entity;
using Pharos.Logic.DAL;
using System.Transactions;
using Pharos.Utility.Helpers;
using Pharos.Sys.Entity;

namespace Pharos.Logic.BLL
{
    public class MakingCouponCardBLL : BaseService<MakingCouponCard>
    {
        private MakingCouponCardDAL _dal = new MakingCouponCardDAL();

        #region 制作优惠券列表
        /// <summary>
        /// 获取创建人列表（用于下拉框）
        /// </summary>
        /// <returns>创建人列表</returns>
        public List<SysStoreUserInfo> GetCreatorList()
        {
            var result = new List<SysStoreUserInfo>();
            try
            {
                result = (from a in CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId)
                          join b in BaseService<SysStoreUserInfo>.CurrentRepository.Entities
                          on a.CreateUID equals b.UID
                          select b).Distinct().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

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
        public DataTable FindCreateCouponPageList(Paging paging, int couponType, int couponFrom, short state, string storeIds, string expiryStart, string expiryEnd, string receiveStart, string receiveEnd, string createUID)
        {
            var result = _dal.FindCreateCouponPageList(paging, couponType, couponFrom, state, storeIds, expiryStart, expiryEnd, receiveStart, receiveEnd, createUID);
            return result;
        }
        #endregion

        #region 新增或修改
        /// <summary>
        /// 获取当前CompanyId下当天制作的优惠券批次总数
        /// </summary>
        /// <returns>当前CompanyId下当天制作的优惠券批次总数</returns>
        public int GetBatchCount()
        {
            int count = 0;
            var min = DateTime.Parse(DateTime.Now.ToShortDateString());
            var max = min.AddDays(1);
            count = BaseService<MakingCouponCard>.FindList(o => o.CreateDT >= min && o.CreateDT < max).Where(o => o.CompanyId == CommonService.CompanyId).Count();
            return count;
        }

        /// <summary>
        /// 保存新增或修改
        /// </summary>
        /// <param name="model">优惠券对象</param>
        /// <returns>保存结果</returns>
        public OpResult SaveOrUpdate(MakingCouponCard model)
        {
            OpResult result = new OpResult();
            if (model.Id == 0)
            {
                model.CreateDT = DateTime.Now;
                model.CreateUID = Sys.CurrentUser.UID;
                model.CompanyId = CommonService.CompanyId;
                model.State = 0;
                #region 批次
                var firstBatchSN = DateTime.Now.ToString("yyMMdd") + "01";
                var firstBatchToInt = long.Parse(firstBatchSN);
                var batchCount = GetBatchCount();
                model.BatchSN = (firstBatchToInt + batchCount).ToString();//批次：6位日期 + 2位序号,示例：16081601
                #endregion
                result = BaseService<MakingCouponCard>.Add(model);
                return result;
            }
            else
            {
                var obj = BaseService<MakingCouponCard>.FindById(model.Id);
                if (obj != null)
                {
                    Pharos.Utility.Helpers.ExtendHelper.ToCopyProperty(model, obj);
                    result = BaseService<MakingCouponCard>.Update(obj);
                }
                return result;
            }
        }

        /// <summary>
        /// 获取优惠券单个对象
        /// </summary>
        /// <param name="Id">优惠券Id</param>
        /// <returns></returns>
        public MakingCouponCard FindModelById(int Id)
        {
            var model = BaseService<MakingCouponCard>.FindById(Id);
            return model;
        }

        /// <summary>
        /// 获取品类/具体商品/品牌列表
        /// </summary>
        /// <param name="recordCount">总数</param>
        /// <returns>品类/具体商品/品牌列表</returns>
        public object LoadTypeList(int Id, int type, out int recordCount)
        {
            recordCount = 0;
            if (Id == 0) return null;
            var dal = new Pharos.Logic.DAL.MakingCouponCardDAL();
            var obj = BaseService<MakingCouponCard>.FindById(Id);
            int proTypes = -1;
            string proCodes = "";

            if (obj != null)
            {
                proTypes = obj.ProductTypes;
                proCodes = obj.ProductCode;
            }
            if (proTypes == 2 && proTypes == type)//指定品类
            {
                var dt = dal.GetCategoryList(proCodes);
                var list = dt.AsEnumerable().Select(dr => new
                {
                    BigCategoryTitle = dr["BigCategoryTitle"],
                    MidCategoryTitle = dr["MidCategoryTitle"],
                    SubCategoryTitle = dr["SubCategoryTitle"],
                    CategorySN = dr["CategorySN"]
                }).ToList();
                recordCount = list.Count;
                return list;
            }
            else if (proTypes == 3 && proTypes == type)//具体商品
            {
                var proList = proCodes.Split(',').ToList();
                //var query = queryProduct.Where(o => proList.Contains(o.Barcode));
                var list = new List<VwProduct>();
                for (int i = 0; i < proList.Count(); i++)
                {
                    var item = proList[i];
                    if (!string.IsNullOrEmpty(item))
                    {
                        var express = DynamicallyLinqHelper.Empty<VwProduct>()
                           .And(o => ((o.CompanyId == CommonService.CompanyId) && ((o.Barcode == item) || ("," + o.Barcodes + ",").Contains("," + item + ","))));
                        var model = BaseService<VwProduct>.FindList(express);
                        list.AddRange(model);
                    }
                }
                recordCount = list.Count;
                return list;
            }
            else if (proTypes == 4 && proTypes == type)//指定品牌
            {
                var dt = dal.GetBrandList(proCodes);
                var list = dt.AsEnumerable().Select(dr => new
                {
                    Id = dr["Id"],
                    BrandSN = dr["BrandSN"],
                    Title = dr["Title"],
                    JianPin = dr["JianPin"],
                    Num = dr["Num"],
                    ClassifyTitle = dr["ClassifyTitle"]
                }).ToList();
                recordCount = list.Count;
                return list;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 设置状态
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="ids">优惠券Id(多个用逗号隔开)</param>
        /// <param name="state">要修改的状态</param>
        /// <returns>修改</returns>
        public OpResult SetCouponState(string ids, short state)
        {
            OpResult result = new OpResult();
            try
            {
                var updateData = CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId && ("," + ids + ",").Contains("," + o.Id + ",")).ToList();
                if (updateData.Count() > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (var item in updateData)
                        {
                            if (item.State != state)
                            {
                                item.State = state;
                                result = BaseService<MakingCouponCard>.Update(item);//设置主表状态
                                if (result.Successed && state == 5)//已作废状态：主表值：5，子表值：4
                                {
                                    var ticketList = FindCouponDetailListByBatch(item.BatchSN).ToList();
                                    ticketList.ForEach(o => { o.State = 4; });//设置子表状态为已作废
                                    result = BaseService<CouponCardDetail>.Update(ticketList);
                                }
                                if (!result.Successed)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                result.Successed = true;
                            }
                        }
                        scope.Complete();
                    }
                    if (result.Successed)
                    {
                        return new OpResult() { Successed = true, Message = "操作成功！" };
                    }
                    else
                    {
                        return new OpResult() { Successed = false, Message = "无数据更新！" };
                    }
                }
                else
                {
                    return new OpResult() { Successed = false, Message = "未找到对应数据！" };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 生成优惠券
        /// <summary>
        /// 生成优惠券
        /// </summary>
        /// <param name="id">优惠券Id</param>
        /// <param name="batchSN">批次号</param>
        /// <param name="num">生成数量</param>
        /// <returns>生成结果</returns>
        public OpResult GenerateCoupon(string id, string batchSN, int num)
        {
            OpResult result = new OpResult() { Successed = false, Message = "生成失败！" };
            if ((!string.IsNullOrEmpty(batchSN)) && num > 0)
            {
                TransactionOptions transactionoptions = new TransactionOptions();
                transactionoptions.Timeout = new TimeSpan(0, 10, 0);//事务超时：10分钟
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionoptions))
                    {
                        var ranArray = GetRandString(3, num);//3位不重复的随机字符串数组，数组长度为num
                        if (ranArray[0] == "错误")
                        {
                            return result;
                        }
                        object obj = new object();
                        lock (obj)
                        {
                            var couponList = new List<CouponCardDetail>();
                            for (int i = 0; i < num; i++)
                            {
                                CouponCardDetail model = new CouponCardDetail();
                                model.CreateDT = DateTime.Now;
                                model.CreateUID = Sys.CurrentUser.UID;
                                model.CompanyId = CommonService.CompanyId;
                                model.BatchSN = batchSN;
                                var serialNo = (i + 1).ToString().PadLeft(5, '0');
                                model.TicketNo = batchSN + serialNo;//券号：8位批次号 + 5位序号,示例：1608160100001
                                model.SecurityCode = ranArray[i];//防伪码：3位英文字母随机组合，同一批次内唯一
                                model.State = 0;
                                couponList.Add(model);
                            }
                            BluckHelper.BulkInsertAll<CouponCardDetail>(couponList);//往数据库插入5000条数据约耗时：0.9s
                            result = SetCouponState(id, 1);
                            //result = BaseService<CouponCardDetail>.AddRange(couponList);//往数据库插入5000条数据约耗时：90s
                            //if (result.Successed)
                            //{
                            //    result = SetCouponState(id, 1);
                            //}
                        }
                        scope.Complete();
                    }
                }
                catch (Exception)
                {
                    return result;
                }
                if (result.Successed)
                {
                    return new OpResult() { Successed = true, Message = "生成成功！" };
                }
                else
                {
                    return new OpResult() { Successed = false, Message = "生成失败！" };
                }
            }
            else
            {
                return result;
            }
        }

        #region 随机生成不重复的字符串
        private const string CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //数字+大写字母
        /// <summary>
        /// 随机生成不重复的字符串
        /// </summary>
        /// <param name="len">字符串长度</param>
        /// <param name="count">字符串个数</param>
        /// <returns>不重复的字符串列表</returns>
        private List<string> GetRandString(int len, int count)
        {
            List<string> errMsg = new List<string>();
            double max_value = Math.Pow(36, len);
            if (max_value > long.MaxValue)
            {
                errMsg[0] = "错误";
                errMsg[1] = string.Format("Math.Pow(36, {0}) 超出 long最大值！", len);
                return errMsg;
            }
            long all_count = (long)max_value;
            long stepLong = all_count / count;
            if (stepLong > int.MaxValue)
            {
                errMsg[0] = "错误";
                errMsg[1] = string.Format("stepLong ({0}) 超出 int最大值！", stepLong);
                return errMsg;
            }
            int step = (int)stepLong;
            if (step < 3)
            {
                errMsg[0] = "错误";
                errMsg[1] = "step 不能小于 3!";
                return errMsg;
            }
            long begin = 0;
            List<string> list = new List<string>();
            Random rand = new Random();
            while (true)
            {
                long value = rand.Next(1, step) + begin;
                begin += step;
                list.Add(GetChart(len, value));
                if (list.Count == count)
                {
                    break;
                }
            }
            list = SortByRandom(list);
            return list;
        }

        /// <summary>
        /// 将数字转化成字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetChart(int len, long value)
        {
            StringBuilder str = new StringBuilder();
            while (true)
            {
                str.Append(CHAR[(int)(value % 36)]);
                value = value / 36;
                if (str.Length == len)
                {
                    break;
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 随机排序
        /// </summary>
        /// <param name="charList"></param>
        /// <returns></returns>
        private List<string> SortByRandom(List<string> charList)
        {
            Random rand = new Random();
            for (int i = 0; i < charList.Count; i++)
            {
                int index = rand.Next(0, charList.Count);
                string temp = charList[i];
                charList[i] = charList[index];
                charList[index] = temp;
            }
            return charList;
        }
        #endregion

        #endregion

        #region 领用优惠券

        #region 领用优惠券列表
        /// <summary>
        /// 获取创建人列表（用于下拉框）
        /// </summary>
        /// <returns>创建人列表</returns>
        public List<SysStoreUserInfo> GetDetailCreatorList()
        {
            var result = new List<SysStoreUserInfo>();
            try
            {
                result = (from a in BaseService<CouponCardDetail>.CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId)
                          join b in BaseService<SysStoreUserInfo>.CurrentRepository.Entities
                          on a.Recipients equals b.UID
                          select b).Distinct().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

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
        public DataTable FindTakeCouponPageList(Paging paging, int couponType, int couponFrom, short state, string storeIds, string recipients, string expiryStart, string expiryEnd)
        {
            var result = _dal.FindTakeCouponPageList(paging, couponType, couponFrom, state, storeIds, recipients, expiryStart, expiryEnd);
            return result;
        }
        #endregion

        #region 派发申领
        /// <summary>
        /// 获取未派发完成的优惠券列表(主表)
        /// </summary>
        /// <returns>未派发完成的优惠券列表</returns>
        public List<MakingCouponCard> FindNotReceivedCompletedList()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var result = BaseService<MakingCouponCard>.FindList(o => o.CompanyId == CommonService.CompanyId && o.ReceiveStart.CompareTo(today) <= 0 && (o.ReceiveEnd == null || o.ReceiveEnd.CompareTo(today) >= 0) && (o.State == 1 || o.State == 2));
            return result;
        }

        /// <summary>
        /// 按批次号获取优惠券列表(子表)
        /// </summary>
        /// <param name="batchSN">批次号</param>
        /// <returns>优惠券列表</returns>
        public List<CouponCardDetail> FindCouponDetailListByBatch(string batchSN)
        {
            var result = BaseService<CouponCardDetail>.FindList(o => o.CompanyId == CommonService.CompanyId && o.BatchSN == batchSN);
            return result;
        }

        /// <summary>
        /// 保存派发申领
        /// </summary>
        /// <param name="batchSN">批次号</param>
        /// <param name="takeNum">领取数量</param>
        /// <param name="ticketStart">起始券号</param>
        /// <returns>保存结果</returns>
        public OpResult SaveTakeCoupon(string batchSN, int takeNum, string ticketStart)
        {
            OpResult result = new OpResult() { Successed = false, Message = "T_T派发申领失败！" };
            try
            {
                object obj = new object();
                lock (obj)
                {
                    var batch = FindNotReceivedCompletedList().Where(o => o.BatchSN == batchSN).ToList();
                    if (batch.Count() > 0)
                    {
                        string tStart = "";
                        var ticketList = FindCouponDetailListByBatch(batchSN).OrderBy(o => o.TicketNo).Where(o => o.State == 0).Take(takeNum).ToList();
                        var count = ticketList.Count();
                        if (count > 0)
                        {
                            tStart = ticketList.Select(o => o.TicketNo).FirstOrDefault();
                            if (tStart == ticketStart)
                            {
                                TransactionOptions transactionoptions = new TransactionOptions();
                                transactionoptions.Timeout = new TimeSpan(0, 10, 0);//事务超时：10分钟
                                try
                                {
                                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionoptions))
                                    {
                                        ticketList.ForEach(o => { o.State = 1; o.ReceiveDT = DateTime.Now; o.Recipients = Sys.CurrentUser.UID; });
                                        var re = BaseService<CouponCardDetail>.Update(ticketList);
                                        if (re.Successed)
                                        {
                                            var unTakenCount = FindCouponDetailListByBatch(batchSN).Where(o => o.State == 0 || o.State == 3 || o.State == 4).ToList().Count();
                                            if (unTakenCount > 0)
                                            {
                                                result = SetCouponState(batch[0].Id.ToString(), 2);
                                            }
                                            else
                                            {
                                                result = SetCouponState(batch[0].Id.ToString(), 3);
                                            }
                                            if (result.Successed == true)
                                            {
                                                result.Message = "保存成功";
                                            }
                                        }
                                        scope.Complete();
                                    }
                                }
                                catch (Exception)
                                {
                                    return result;
                                }
                            }
                            else
                            {
                                result.Successed = false;
                                result.Message = "T_T您手慢了！您选择的优惠券已经被派发申领了，请重新选择一批吧！";
                            }
                        }
                        else
                        {
                            result.Successed = false;
                            result.Message = "T_T您手慢了！您选择的优惠券已经被派发申领了，请重新选择一批吧！";
                        }
                    }
                    else
                    {
                        result.Successed = false;
                        result.Message = "T_T您手慢了！您选择的优惠券已经被派发申领了，请重新选择一批吧！";
                    }
                }
            }
            catch (Exception)
            {
                return result;
            }
            return result;
        }
        #endregion

        #endregion
        /// <summary>
        /// 获取该会员的优惠券
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<CouponCardDetail> GetListCouponCardByMemberId(string memberId)
        {
            return BaseService<CouponCardDetail>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId && o.UsePerson == memberId).ToList();
        }

    }
}
