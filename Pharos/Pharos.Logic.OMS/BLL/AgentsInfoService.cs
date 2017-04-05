﻿using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Entity.View;
using Pharos.Logic.OMS.IDAL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Transactions;
using System.Web;
using System.Text.RegularExpressions;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// 代理商档案BLL
    /// </summary>
    public class AgentsInfoService : BaseService<AgentsInfo>
    {

        [Ninject.Inject]
        // 代理商档案
        IBaseRepository<AgentsInfo> AgentsInfoRepository { get; set; }

        // 代理商档案
        [Ninject.Inject]
        IAgentsInfoRepository aRepository { get; set; }

        [Ninject.Inject]
        // 代理商下级关系
        IBaseRepository<AgentsRelationship> AgentsRelationshipRepository { get; set; }

        [Ninject.Inject]
        // 字典
        IBaseRepository<SysDataDictionary> sysDataDictionaryRepository { get; set; }

        //代理商下级关系
        [Ninject.Inject]
        AgentsRelationshipService agentsRelationshipService { get; set; }

        //结算账户信息
        [Ninject.Inject]
        BankCardInfoService bankCardInfoService { get; set; }

        //代理商支付渠道
        [Ninject.Inject]
        AgentPayService agentPayService { get; set; }

        //支付接口
        [Ninject.Inject]
        IBaseRepository<PayApi> payApiRepository { get; set; }

        [Ninject.Inject]
        // 结算账户信息
        public IBaseRepository<BankCardInfo> bankCardInfoRepository { get; set; }

        [Ninject.Inject]
        // 代理商账号
        AgentsUsersService agentsUsersService { get; set; }

        /// <summary>
        /// 获取最大AgentsId
        /// </summary>
        /// <returns></returns>
        public int getMaxAgentsId()
        {
            //return TradersRepository.GetQuery().Max(o => (int?)o.CID).GetValueOrDefault() + 1;
            int AgentsId = 0;
            AgentsId = AgentsInfoRepository.GetQuery().Max(o => (int?)o.AgentsId).GetValueOrDefault();
            if (AgentsId < 100001)
            {
                AgentsId = 100001;
            }
            else
            {
                AgentsId = AgentsId + 1;
                if (AgentsId >= 999999)
                {
                    AgentsId = -1;
                }
            }
            return AgentsId;
        }

        /// <summary>
        /// 增加或者修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fu">证件照</param>
        /// <returns></returns>
        public OpResult SaveOrUpdate(AgentsInfo model, HttpPostedFileBase hpf)
        {
            int maxAgentsId = getMaxAgentsId();

            //上传证件照
            if (!string.IsNullOrEmpty(hpf.FileName))
            {
                string[] s = Tool.fileUpload(hpf, Tool.getIdCardPhotoPath(model.AgentsId == 0 ? maxAgentsId : model.AgentsId));
                if (s[0] != "文件上传成功")
                {
                    return OpResult.Fail(s[0]);
                }
                else
                {
                    model.IdCardPhoto = s[1];
                }
            }

            if (model.Id == 0)
            {
                model.CreateType = 1;
                model.AgentsId = maxAgentsId;
                model.AgentsId2 = maxAgentsId.ToString();
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                model.CreateUid = CurrentUser.UID;
                AgentsInfoRepository.Add(model);
            }
            else
            {
                model.UpdateTime = DateTime.Now;
                var source = AgentsInfoRepository.Get(model.Id);
                if (string.IsNullOrEmpty(hpf.FileName))
                {
                    model.IdCardPhoto = source.IdCardPhoto;
                }
                else
                {
                    //删除文件
                    Tool.deleteFile(Tool.getIdCardPhotoPath(source.AgentsId) + source.IdCardPhoto);
                }
                model.ToCopyProperty(source, new List<string>() { "AgentsId", "AgentsId2", "CreateTime", "CreateUid", "CreateType" });
            }

            AgentsInfoRepository.SaveChanges();
            return OpResult.Success(model.AgentsId.ToString());
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="agentsInfo">基本信息</param>
        /// <param name="hpf">证件照</param>
        /// <param name="nvl"></param>
        /// <returns></returns>
        public OpResult Save(AgentsInfo agentsInfo, HttpPostedFileBase hpf, System.Collections.Specialized.NameValueCollection nvl)
        {
            try
            {
                using (EFDbContext context = new EFDbContext())
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        //基本信息
                        OpResult op = SaveOrUpdate(agentsInfo, hpf);
                        //代理商编号
                        int AgentsId = Convert.ToInt32(op.Message);


                        //删除已经存在的代理商下级关系
                        int[] ids = AgentsRelationshipRepository.GetQuery(o => o.SubAgentsId == AgentsId).Select(o => o.Id).ToArray();
                        agentsRelationshipService.Deletes(ids);

                        //上级代理商编号
                        int pid = Convert.ToInt32(op.Message);
                        //代理商下级关系
                        for (int i = 0; i < 3; i++)
                        {
                            AgentsRelationship agentsRelationship = new AgentsRelationship();
                            AgentsInfo aInfo = AgentsInfoRepository.GetQuery(o => o.AgentsId == pid).FirstOrDefault();
                            pid = aInfo.PAgentsId;
                            agentsRelationship.SubAgentsId = AgentsId;
                            agentsRelationship.AgentsId = pid;
                            agentsRelationship.Status = 1;
                            if (pid == 0 && i == 0)
                            {
                                agentsRelationship.Depth = 0;
                                agentsRelationshipService.SaveOrUpdate(agentsRelationship);
                                break;
                            }
                            else if (pid == 0 && i != 0)
                            {
                                break;
                            }
                            else if (pid != 0)
                            {
                                agentsRelationship.Depth = Convert.ToInt16(i + 1);
                            }
                            agentsRelationshipService.SaveOrUpdate(agentsRelationship);
                        }


                        //结算银行机构
                        string Agency = (nvl["Agency"] ?? "").Trim();
                        //结算账户类型
                        string BillingType = (nvl["BillingType"] ?? "").Trim();
                        //结算卡号
                        string CardNum = (nvl["CardNum"] ?? "").Trim();
                        //账户名称
                        string CardName = (nvl["CardName"] ?? "").Trim();
                        //账户状态
                        string cardStatus = (nvl["cardStatus"] ?? "").Trim();
                        //交易支付通道
                        string PayChannel = (nvl["PayChannel"] ?? "").Trim();
                        //成本费率（%）
                        string Cost = (nvl["Cost"] ?? "").Trim();
                        //下级费率（%）
                        string Lower = (nvl["Lower"] ?? "").Trim();

                        //结算账户
                        BankCardInfo bankCardInfo = new BankCardInfo();
                        bankCardInfo.AgentsId = AgentsId;
                        bankCardInfo.Type = 1;
                        bankCardInfo.Agency = Agency;
                        bankCardInfo.BillingType = BillingType == "" ? -1 : Convert.ToInt32(BillingType);
                        bankCardInfo.CardNum = CardNum;
                        bankCardInfo.CardName = CardName;
                        bankCardInfo.Status = cardStatus == "" ? -1 : Convert.ToInt32(cardStatus);
                        if (agentsInfo.Id > 0)
                        {
                            int bankCardInfoId = 0;
                            BankCardInfo bankCardInfo2 = bankCardInfoService.GetOne(AgentsId);
                            if (bankCardInfo2 != null)
                            {
                                bankCardInfoId = bankCardInfo2.Id;
                            }
                            bankCardInfo.Id = bankCardInfoId;
                        }
                        bankCardInfoService.SaveOrUpdate(bankCardInfo);

                        //支付渠道
                        AgentPay agentPay = new AgentPay();
                        agentPay.AgentsId = AgentsId;
                        agentPay.ApiNo = PayChannel == "" ? -1 : Convert.ToInt32(PayChannel);
                        agentPay.Cost = Cost == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(Cost);
                        agentPay.Lower = Lower == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(Lower);
                        if (agentsInfo.Id > 0)
                        {
                            int agentPayId = 0;
                            AgentPay agentPay2 = agentPayService.GetOne(AgentsId);
                            if (agentPay2 != null)
                            {
                                agentPayId = agentPay2.Id;
                            }
                            agentPay.Id = agentPayId;
                        }
                        agentPayService.SaveOrUpdate(agentPay);

                        //提交事务
                        transaction.Complete();
                        return OpResult.Success();
                    }
                }
            }
            catch (Exception e)
            {
                LogEngine.WriteError(e);
                return OpResult.Fail(e.InnerException.InnerException.Message);
            }
        }


        /// <summary>
        /// 获取字典（SysDataDictionary）
        /// </summary>
        /// <param name="DicSN"></param>
        /// <returns></returns>
        public SysDataDictionary getData(int DicSN)
        {
            return sysDataDictionaryRepository.Find(o=>o.DicSN==DicSN);
        }

        public List<AgentsInfo> GetListWhere(System.Collections.Specialized.NameValueCollection nvl)
        {
            //关键字
            var keyword = (nvl["keyword"] ?? "").Trim();
            string _id = nvl["id"] ?? "";
            int id = _id == "" ? 0 : Convert.ToInt32(_id);

            var query = AgentsInfoRepository.GetQuery();

            query = query.Where(o => o.Status <= 2 && o.Id != id);
            if (!keyword.IsNullOrEmpty())
            {
                query = query.Where(o => o.AgentsId2.StartsWith(keyword));
            }
            else
            {
                query = query.Where(o => o.AgentsId2=="-1");
            }
            return query.Take(30).ToList();
        }


        /// <summary>
        /// 是否存在下级代理商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool isExistPAgentsId(int id)
        {
            AgentsInfo agentsInfo = AgentsInfoRepository.GetQuery(o=>o.Id==id).FirstOrDefault();
            if (agentsInfo == null)
            {
                return false;
            }
            else
            {
                var isExist = AgentsInfoRepository.GetQuery(o =>o.PAgentsId==agentsInfo.AgentsId);
                if (isExist.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 上级代理商代理商编号是否存在
        /// </summary>
        public bool isExistAgentsId(int AgentsId)
        {
            var isExist = AgentsInfoRepository.GetQuery(o => o.AgentsId == AgentsId);
            if (isExist.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AgentsInfo GetOne(object id)
        {
            return AgentsInfoRepository.Get(id);
        }

        /// <summary>
        /// 获取支付接口
        /// </summary>
        /// <returns></returns>
        public List<PayApi> GetPayApiList(Expression<Func<PayApi, bool>> whereLambda = null)
        {
            if (whereLambda != null)
            {
                return payApiRepository.GetQuery(whereLambda).ToList();
            }
            return payApiRepository.GetQuery().ToList();
        }

        /// <summary>
        /// 成本费率是否小于于接口成本费率
        /// </summary>
        /// <param name="ApiNo">接口编号</param>
        /// <param name="cost">成本费率</param>
        /// <returns>1是小于，0是不小于</returns>
        public string[] isCost(int ApiNo,decimal cost)
        {
            string[] str=new string[2];
            str[0] = "0";
            str[1] = "0.00";
            //PayApi payApi = payApiRepository.GetQuery(o => o.ApiNo == ApiNo).FirstOrDefault();
            //if (payApi != null)
            //{
            //    if (cost < payApi.OverServiceRate)
            //    {
            //        str[0] = "1";
            //        str[1] = payApi.OverServiceRate.ToString();
            //        return str;
            //    }
            //}
            return str;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="AgentsId">代理商编号</param>
        /// <returns></returns>
        public OpResult Verification(System.Collections.Specialized.NameValueCollection nvl, int AgentsId)
        {
            //代理商简称
            var Name = (nvl["Name"] ?? "").Trim();
            //代理商全称
            var FullName = (nvl["FullName"] ?? "").Trim();
            //上级代理商编号
            var PAgentsId = (nvl["PAgentsId"] ?? "").Trim();
            //结算卡号
            var CardNum = (nvl["CardNum"] ?? "").Trim();
            //接口编号
            var PayChannel = (nvl["PayChannel"] ?? "").Trim();
            if (PayChannel == "")
            {
                PayChannel = "0";
            }
            //成本费率
            var Cost = (nvl["Cost"] ?? "").Trim();
            if (Cost == "")
            {
                Cost = "0.00";
            }
            //下级费率
            var Lower = (nvl["Lower"] ?? "").Trim();
            if (Lower == "")
            {
                Lower = "0.00";
            }

            if (PAgentsId != "")
            {
                if (!Tool.IsNumber(PAgentsId))
                {
                    return OpResult.Fail("上级代理商编号不存在");
                }
                else
                {
                    //上级代理商代理商编号是否存在
                    if (!isExistAgentsId(Convert.ToInt32(PAgentsId)))
                    {
                        return OpResult.Fail("上级代理商编号不存在");
                    }
                }
            }



            //代理商简称是否存在
            IQueryable<AgentsInfo> isExistName = null;
            if (AgentsId == 0)
            {
                isExistName = AgentsInfoRepository.GetQuery(o => o.Name == Name);
            }
            else
            {
                isExistName = AgentsInfoRepository.GetQuery(o => o.Name == Name && o.AgentsId != AgentsId);
            }
            if (isExistName.Any())
            {
                return OpResult.Fail("该代理商简称已经存在");
            }

            //代理商全称是否存在
            IQueryable<AgentsInfo> isExistFullName = null;
            if (AgentsId == 0)
            {
                isExistFullName = AgentsInfoRepository.GetQuery(o => o.FullName == FullName);
            }
            else
            {
                isExistFullName = AgentsInfoRepository.GetQuery(o => o.FullName == FullName && o.AgentsId != AgentsId);
            }
            if (isExistFullName.Any())
            {
                return OpResult.Fail("该代理商全称已经存在");
            }

            IQueryable<BankCardInfo> isExistCardNum = null;
            if (AgentsId == 0)
            {
                isExistCardNum = bankCardInfoRepository.GetQuery(o => o.CardNum.Trim() == CardNum&&o.Type==1);
            }
            else
            {
                isExistCardNum = bankCardInfoRepository.GetQuery(o => o.CardNum.Trim() == CardNum && o.Type == 1 && o.AgentsId != AgentsId);
            }
            if (isExistCardNum.Any())
            {
                return OpResult.Fail("该结算卡号已经存在");
            }

            if (PAgentsId != "")
            {
                AgentPay agentPay = agentPayService.GetOne(Convert.ToInt32(PAgentsId));
                if (agentPay != null)
                {
                    if (Cost!="0.00"&&agentPay.Lower != Convert.ToDecimal(Cost))
                    {
                        return OpResult.Fail("成本费率必须和上级代理商：" + PAgentsId + "设定的下级费率相同");
                    }
                }
            }

            string[] str=new string[2];
            str = isCost(Convert.ToInt32(PayChannel), Convert.ToDecimal(Cost));
            if (str[0] == "1")
            {
                return OpResult.Fail("成本费率不能小于交易支付通道成本费率");
            }

            return OpResult.Success();
        }

        public List<ViewAgentsInfo> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            //指派人
            var AssignUid = (nvl["AssignUid"] ?? "").Trim();
            //代理商类型
            var Type = (nvl["Type"] ?? "").Trim();
            //创建日期（开始）
            var CreateDT_begin = (nvl["CreateDT_begin"] ?? "").Trim();
            //创建日期（结束）
            var CreateDT_end = (nvl["CreateDT_end"] ?? "").Trim();
            //关键字类型
            var keywordType = (nvl["keywordType"] ?? "").Trim();
            //关键字
            var keyword = (nvl["keyword"] ?? "").Trim();

            var pageIndex = 1;
            var pageSize = 20;
            if (!nvl["page"].IsNullOrEmpty())
                pageIndex = int.Parse(nvl["page"]);
            if (!nvl["rows"].IsNullOrEmpty())
                pageSize = int.Parse(nvl["rows"]);

            string strw = "";

            if (!AssignUid.IsNullOrEmpty())
            {
                string[] aUID = AssignUid.Split(',');
                string newAUID = "";
                if (aUID.Length > 0)
                {
                    for (int i = 0; i < aUID.Length; i++)
                    {
                        if (newAUID == "")
                        {
                            newAUID = "'" + aUID[i] + "'";
                        }
                        else
                        {
                            newAUID = newAUID + ",'" + aUID[i] + "'";
                        }
                    }
                    strw = strw + " and a.AssignUid in (" + newAUID + ")";
                }

            }

            if (!Type.IsNullOrEmpty())
            {
                strw = strw + " and a.Type=" + Type;
            }

            if (!CreateDT_begin.IsNullOrEmpty())
            {
                string c = CreateDT_begin + " " + "00:00:00";
                strw = strw + " and a.CreateTime >='" + c + "'";
            }
            if (!CreateDT_end.IsNullOrEmpty())
            {
                var c = CreateDT_end + " " + "23:59:59";
                strw = strw + " and a.CreateTime <='" + c + "'";
            }

            if (!keywordType.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                if (keywordType == "0")
                {
                    if (!IsNumber(keyword) || keyword.Length > 6)
                    {
                        keyword = "0";
                    }
                    strw = strw + " and a.AgentsId=" + keyword;
                }
                if (keywordType == "1")
                {
                    strw = strw + " and a.FullName like '%" + keyword + "%'";
                }
                if (keywordType == "2")
                {
                    strw = strw + " and a.CorporateName like '%" + keyword + "%'";
                }
                if (keywordType == "3")
                {
                    strw = strw + " and a.LinkMan like '%" + keyword + "%'";
                }
            }

            List<ViewAgentsInfo> list = aRepository.getPageList(pageIndex, pageSize, strw, out recordCount);
            return list;
        }

        /// <summary> 
        /// 判断给定的字符串(strNumber)是否是数值型 
        /// </summary> 
        /// <param name="strNumber">要确认的字符串</param> 
        /// <returns>是则返加true 不是则返回 false</returns> 
        public bool IsNumber(string strNumber)
        {
            return new Regex(@"^([0-9])[0-9]*(\.\w*)?$").IsMatch(strNumber);
        }

        /// <summary>
        /// 设定指派人
        /// </summary>
        public OpResult upAssignUid(string ids, string AssignUid)
        {
            var sId = ids.Split(',').Select(o => int.Parse(o));
            var olist = AgentsInfoRepository.GetQuery(o => sId.Contains(o.Id)).ToList();
            olist.Each(o => o.AssignUid = AssignUid);
            return OpResult.Result(AgentsInfoRepository.SaveChanges());
        }


        /// <summary>
        /// 终止所选代理商
        /// </summary>
        public OpResult StopAgents(string ids)
        {
            try
            {
                var sId = ids.Split(',').Select(o => int.Parse(o));
                var olist = AgentsInfoRepository.GetQuery(o => sId.Contains(o.Id)).ToList();
                var agentsId = olist.Select(o => o.AgentsId).ToArray();
                //下级代理商
                var sub = AgentsInfoRepository.GetQuery(o => agentsId.Contains(o.PAgentsId)).ToList();
                if (sub.Any())
                {
                    return OpResult.Fail("所选代理商存在下级代理商，如需终止请先终止下级代理商！");
                }
                olist.Each(o => o.Status = 3);
                AgentsInfoRepository.SaveChanges();
            }
            catch(Exception e)
            {
                LogEngine.WriteError(e);
                throw new Exception("发生错误!", e);
            }
            return OpResult.Success();
        }

        /// <summary>
        /// 代理商是否存在未到期
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool isExpires(string ids)
        {
            var sId = ids.Split(',').Select(o => int.Parse(o));
            var olist = AgentsInfoRepository.GetQuery(o => sId.Contains(o.Id)).ToList();
            string dt=DateTime.Now.ToString("yyyy-MM-dd")+ " 00:00:00";
            olist = olist.Where(o => Convert.ToDateTime(o.EndTime)>Convert.ToDateTime(dt)).ToList();
            if (olist.Any())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public OpResult InitializeSave(int id)
        {
            if (id > 0)
            {
                //已经到期
                if (!isExpires(id.ToString()))
                {
                    OpResult op = upEndTime(id);
                    if (op.Successed)
                    {
                        return OpResult.Success();
                    }
                    else
                    {
                        return OpResult.Fail("更新状态为到期发生错误！");
                    }
                }
            }
            return OpResult.Success();
        }

        /// <summary>
        /// 更新状态为到期
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpResult upEndTime(int id)
        {
            try
            {
                var olist = AgentsInfoRepository.GetQuery(o => o.Id == id).ToList();
                olist.Each(o =>
                {
                    o.ValidityYear = 0;
                    o.StartTime = "";
                    o.Status = 4;
                });
                AgentsInfoRepository.SaveChanges();
            }
            catch(Exception e)
            {
                LogEngine.WriteError(e);
                throw new Exception("发生错误!", e);
            }
            return OpResult.Success();
        }

        /// <summary>
        /// 续签代理
        /// </summary>
        public OpResult Renewal(System.Collections.Specialized.NameValueCollection nvl)
        {
            //ids
            var ids = (nvl["ids"] ?? "").Trim();
            //续签年
            var ValidityYear = (nvl["ValidityYear"] ?? "").Trim();
            //起始日期
            var StartTime = (nvl["StartTime"] ?? "").Trim();
            //终止日期
            var EndTime = (nvl["EndTime"] ?? "").Trim();

            var sId = ids.Split(',').Select(o => int.Parse(o));
            var olist = AgentsInfoRepository.GetQuery(o => sId.Contains(o.Id)).ToList();
            olist.Each(o => {
                o.ValidityYear = Convert.ToInt32(ValidityYear);
                o.StartTime = StartTime;
                o.EndTime = EndTime;
                o.Status = 2;
            });
            return OpResult.Result(AgentsInfoRepository.SaveChanges());
        }

        /// <summary>
        /// 设定主账号
        /// </summary>
        public OpResult SettingUser(System.Collections.Specialized.NameValueCollection nvl)
        {
            //ids
            var ids = (nvl["ids"] ?? "").Trim();
            //主登录账号
            var LoginName = (nvl["LoginName"] ?? "").Trim();
            //密码
            var LoginPwd = (nvl["LoginPwd"] ?? "").Trim();
            //姓名
            var usersFullName = (nvl["usersFullName"] ?? "").Trim();

            var sId = ids.Split(',').Select(o => int.Parse(o==""?"0":o));
            var agentsInfo = AgentsInfoRepository.GetQuery(o => sId.Contains(o.Id)).ToList().FirstOrDefault();
            if (agentsInfo != null)
            {
                AgentsUsers agentsUsers = agentsUsersService.GetAgentsUsersList(o => o.AgentType == 1 && o.AgentsId == agentsInfo.AgentsId).FirstOrDefault();
                if (agentsUsers == null)
                {
                    agentsUsers = new AgentsUsers();
                    agentsUsers.AgentsId = agentsInfo.AgentsId;
                    //主账户
                    agentsUsers.AgentType = 1;
                }
                if (LoginName != "")
                {
                    agentsUsers.LoginName = LoginName;
                }
                if (LoginPwd != "")
                {
                    agentsUsers.LoginPwd = LoginPwd;
                }
                if (usersFullName != "")
                {
                    agentsUsers.FullName = usersFullName;
                }
                agentsUsers.Status = 1;
                return agentsUsersService.SaveOrUpdate(agentsUsers);
            }
            else
            {
                return OpResult.Fail("发生错误！");
            }
        }

        /// <summary>
        /// 选择交易支付通道
        /// </summary>
        /// <param name="ApiNo">接口编号</param>
        /// <param name="PAgentsId">上级代理商编号</param>
        /// <returns></returns>
        public PayApi SelPayApi(int ApiNo, int PAgentsId)
        {
            PayApi payApi = new PayApi();
            //if (ApiNo > 0)
            //{
            //    if (PAgentsId > 0)
            //    {
            //        AgentPay agentPay = agentPayService.GetOne(PAgentsId); ;
            //        if (agentPay != null)
            //        {
            //            payApi.ApiNo = agentPay.ApiNo;
            //            payApi.OverServiceRate = agentPay.Lower;
            //        }
            //    }
            //    else
            //    {
            //        payApi = GetPayApiList(o => o.ApiNo == ApiNo).FirstOrDefault();
            //    }
            //}
            return payApi;
        }
    }
}
