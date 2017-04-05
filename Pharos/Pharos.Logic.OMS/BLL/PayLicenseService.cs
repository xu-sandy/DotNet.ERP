﻿using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Entity.View;
using Pharos.Logic.OMS.IDAL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.Transactions;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// BLL商家支付许可档案
    /// </summary>
    public class PayLicenseService : BaseService<PayLicense>
    {
        [Ninject.Inject]
        // 商户资料
        IBaseRepository<Traders> TradersRepository { get; set; }

        [Ninject.Inject]
        // 代理商档案
        IBaseRepository<AgentsInfo> AgentsInfoRepository { get; set; }

        [Ninject.Inject]
        // 行业信息
        IBaseRepository<Business> BusinessRepository { get; set; }

        [Ninject.Inject]
        IBaseRepository<CompanyAuthorize> CompanyAuthorizeRepository { get; set; }

        // 商家支付许可档案
        [Ninject.Inject]
        IPayLicenseRepository pRepository { get; set; }


        [Ninject.Inject]
        IBaseRepository<Area> areaRepository { get; set; }

        [Ninject.Inject]
        //BLL-----商户资料
        TradersService tradersService { get; set; }

        [Ninject.Inject]
        //代理商档案BLL
        AgentsInfoService agentsInfoService { get; set; }

        //[Ninject.Inject]
        //BLL商家结算账户
        BankAccountService bankAccountService
        {
            get
            {
                return new BankAccountService();
            }
        }

        [Ninject.Inject]
        //BLL审批日志
        ApproveLogService approveLogService { get; set; }

        // [Ninject.Inject]
        //BLL商家支付主密钥
        TradersPaySecretKeyService tradersPaySecretKeyService
        {
            get 
            {
                return new TradersPaySecretKeyService();
            } 
        }

        //[Ninject.Inject]
        //BLL商家登录账号
        TradersUserService tradersUserService
        {
            get
            {
                return new TradersUserService();
            }
        }

       // [Ninject.Inject]
        //BLL商家支付通道
        TradersPayChannelService tradersPayChannelService
        {
            get
            {
                return new TradersPayChannelService(); ;
            }
        }

        //[Ninject.Inject]
        //BLL商家门店
        TradersStoreService tradersStoreService
        {
            get
            {
                return new TradersStoreService();
            }
        }

        [Ninject.Inject]
        // 字典
        IBaseRepository<SysDataDictionary> dataRepository { get; set; }

        /// <summary>
        /// 获取CID
        /// </summary>
        /// <param name="nvl"></param>
        /// <returns></returns>
        public List<Traders> GetCIDWhere(System.Collections.Specialized.NameValueCollection nvl)
        {
            //关键字
            var keyword = (nvl["keyword"] ?? "").Trim();
            if (keyword == "")
            {
                keyword = "-1";
            }
            string keyw = "'"+keyword+"%'";
            return pRepository.getListCID(keyw);
        }
        /// <summary>
        /// 获取代理商编号
        /// </summary>
        /// <param name="nvl"></param>
        /// <returns></returns>
        public List<AgentsInfo> GetAgentsIdWhere(System.Collections.Specialized.NameValueCollection nvl)
        {
            //关键字
            var keyword = (nvl["keyword"] ?? "").Trim();
            var query = AgentsInfoRepository.GetQuery();

            query = query.Where(o => o.Status == 2);
            if (!keyword.IsNullOrEmpty())
            {
                query = query.Where(o => o.AgentsId2.StartsWith(keyword));
            }
            else
            {
                query = query.Where(o => o.AgentsId2 == "-1");
            }
            return query.Take(30).ToList();
        }

        /// <summary>
        /// 获取城市
        /// </summary>
        /// <param name="nvl"></param>
        /// <returns></returns>
        public List<Area> GetCityWhere(System.Collections.Specialized.NameValueCollection nvl)
        {
            //关键字
            var keyword = (nvl["keyword"] ?? "").Trim();
            var query = areaRepository.GetQuery();

            query = query.Where(o => o.Type==3);
            if (!keyword.IsNullOrEmpty())
            {
                query = query.Where(o => o.Title.StartsWith(keyword)||o.QuanPin.StartsWith(keyword));
            }
            else
            {
                query = query.Where(o => o.AreaID == -1);
            }
            return query.Take(30).ToList();
        }

        /// <summary>
        ///  获取城市ID
        /// </summary>
        /// <returns></returns>
        public int getCityID(string title)
        {
            return areaRepository.GetQuery(o=>o.Title==title).Select(o=>o.AreaID).FirstOrDefault();
        }

        /// <summary>
        ///  获取城市名称
        /// </summary>
        /// <returns></returns>
        public string getCityTitle(int AreaID)
        {
            return areaRepository.GetQuery(o => o.AreaID == AreaID).Select(o => o.Title).FirstOrDefault();
        }

        /// <summary>
        /// 城市是否存在
        /// </summary>
        /// <param name="AreaID"></param>
        /// <returns></returns>
        public bool isExistCity(string Title)
        {
            Title = Title.Trim();
            var query = areaRepository.GetQuery(o=>o.Title==Title&&o.Type==3);
            if (query.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hpf"></param>
        /// <param name="payLicense"></param>
        /// <param name="type">1是资质或证；2是企业证件；3是身份证正面；4是身份证反面</param>
        /// <returns></returns>
        public string UploadFile(HttpPostedFileBase hpf, PayLicense payLicense, int type)
        {
            if (!string.IsNullOrEmpty(hpf.FileName))
            {
                string picUrl = "";
                if (type == 1)
                {
                    picUrl = Tool.getPLicensePicPath(payLicense.LicenseId, type) + payLicense.ECertificateUrl1;
                }
                else if (type == 2)
                {
                    picUrl = Tool.getPLicensePicPath(payLicense.LicenseId, type) + payLicense.ECertificateUrl2;
                }
                else if (type == 3)
                {
                    picUrl = Tool.getPLicensePicPath(payLicense.LicenseId, type) + payLicense.IDCardUrl1;
                }
                else if (type == 4)
                {
                    picUrl = Tool.getPLicensePicPath(payLicense.LicenseId, type) + payLicense.IDCardUrl2;
                }
                //删除文件
                Tool.deleteFile(picUrl);
                string[] s = Tool.fileUpload(hpf, Tool.getPLicensePicPath(payLicense.LicenseId, type));
                if (s[0] != "文件上传成功")
                {
                    return "error";
                }
                else
                {
                    return s[1];
                }
            }
            return "error";
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="fileCollection"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpResult Verification(System.Collections.Specialized.NameValueCollection nvl, HttpFileCollectionBase fileCollection, int id, PayLicense payLicense)
        {
            //商户号
            var CID = (nvl["CID"] ?? "").Trim();
            //服务商号
            var AgentsId = (nvl["AgentsId"] ?? "").Trim();
            //营业期限开始日期
            var OperatingStartDate = (nvl["OperatingStartDate"] ?? "").Trim();
            //营业期限结束日期
            var OperatingEndDate = (nvl["OperatingEndDate"] ?? "").Trim();
            //开户银行城市
            var BankCityTitle = (nvl["BankCityTitle"] ?? "").Trim();
            //支付后台登录账号
            var LoginName = (nvl["LoginName"] ?? "").Trim();
            //资质或证件
            HttpPostedFileBase file1 = fileCollection[0];
            //企业证件
            HttpPostedFileBase file2 = fileCollection[1];
            //身份证正面
            HttpPostedFileBase file3 = fileCollection[2];
            //身份证反面
            HttpPostedFileBase file4 = fileCollection[3];

            var list = tradersService.getUserList().Where(o=>o.UserId==payLicense.DesigneeId);
            if (!list.Any())
            {
                return OpResult.Fail("指派人不正确");
            }

            if (CID != "")
            {
                if (!Tool.IsNumber(CID))
                {
                    return OpResult.Fail("商户号不存在");
                }
                else
                {
                    int cid = Convert.ToInt32(CID);
                    var q = CompanyAuthorizeRepository.GetQuery(o => o.Status == 1 && o.CID == cid);
                    if (!q.Any())
                    {
                        return OpResult.Fail("商户号未申请软件许可或不是正常的软件许可");
                    }
                    if (!tradersService.isExistByWhere(o => o.Status == 1 && o.CID == cid))
                    {
                        return OpResult.Fail("商户号在客户档案不存在或不是已审");
                    }
                }
            }

            int intCID = 0;
            if (CID != "")
            {
                intCID = Convert.ToInt32(CID);
            }

            bool isExist = false;
            if (id == 0)
            {
                isExist = isExistByWhere(o => o.CID == intCID);
            }
            else
            {
                isExist = isExistByWhere(o => o.CID == intCID && o.Id!=id);
            }
            if (isExist)
            {
                return OpResult.Fail("该商户号已经申请过支付许可，无法重复申请");
            }

            if (AgentsId != "")
            {
                if (!Tool.IsNumber(AgentsId))
                {
                    return OpResult.Fail("服务商号不存在");
                }
                else
                {
                    int agentsId = Convert.ToInt32(AgentsId);
                    //未修改
                    if (!agentsInfoService.isExistByWhere(o=>o.AgentsId==agentsId))
                    {
                        return OpResult.Fail("服务商号不存在");
                    }
                }
            }

            if (OperatingStartDate != "" && OperatingEndDate!="")
            {
                DateTime startDateTime = Convert.ToDateTime(OperatingStartDate);
                startDateTime = startDateTime.AddMonths(4);
                DateTime endDateTime = Convert.ToDateTime(OperatingEndDate);
                if (DateTime.Compare(startDateTime, endDateTime) >= 0)
                {
                    return OpResult.Fail("营业期限结束日期必须大于开始日期4个月");
                }
            }

            if (BankCityTitle != "")
            {
                if (!isExistCity(BankCityTitle))
                {
                    return OpResult.Fail("开户银行城市不存在");
                }
            }

            if (LoginName != "")
            {
                if (id > 0)
                {
                    PayLicense pLicense = pRepository.GetEntityByWhere(id);
                    if (pLicense != null)
                    {
                        int cid = Convert.ToInt32(CID);
                        TradersUser tradersUser = tradersUserService.GetEntityByWhere(o => o.CID == pLicense.CID && o.AccountType == 1);
                        if (tradersUser != null && cid == pLicense.CID)
                        {
                            if (tradersUserService.isExistByWhere(o => o.LoginName == LoginName && o.Id != tradersUser.Id))
                            {
                                return OpResult.Fail("支付后台登录账号不可用，请使用其它账号");
                            }
                        }
                        else
                        {
                            if (tradersUserService.isExistByWhere(o => o.LoginName == LoginName))
                            {
                                return OpResult.Fail("支付后台登录账号不可用，请使用其它账号");
                            }
                        }
                    } 
                }
                else
                {
                    if (tradersUserService.isExistByWhere(o => o.LoginName == LoginName))
                    {
                        return OpResult.Fail("支付后台登录账号不可用，请使用其它账号");
                    }
                }
            }

            string format = "*.jpg,*.jpeg,*.gif,*.png,*.bmp";

            string f1 = Tool.ValidateFile(file1, 1000, format);
            if (f1 != "success")
            {
                return OpResult.Fail("资质或证件" + f1);
            }

            string f2 = Tool.ValidateFile(file2, 1000, format);
            if (f2 != "success")
            {
                return OpResult.Fail("企业证件" + f2);
            }

            string f3 = Tool.ValidateFile(file3, 1000, format);
            if (f3 != "success")
            {
                return OpResult.Fail("身份证正面" + f3);
            }

            string f4 = Tool.ValidateFile(file4, 1000, format);
            if (f4 != "success")
            {
                return OpResult.Fail("身份证反面" + f4);
            }

            return OpResult.Success();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="payLicense"></param>
        /// <param name="id"></param>
        /// <param name="nvl"></param>
        /// <param name="fileCollection"></param>
        /// <returns></returns>
        public OpResult Save(PayLicense payLicense, int id, DateTime dt, System.Collections.Specialized.NameValueCollection nvl, HttpFileCollectionBase fileCollection)
        {
            //验证
            var op = Verification(nvl,fileCollection,id,payLicense);
            if (!op.Successed)
            {
                return op;
            }
            else
            {
                try
                {
                    using (EFDbContext context = new EFDbContext())
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            //资质或证件
                            HttpPostedFileBase file1 = fileCollection[0];
                            //企业证件
                            HttpPostedFileBase file2 = fileCollection[1];
                            //身份证正面
                            HttpPostedFileBase file3 = fileCollection[2];
                            //身份证反面
                            HttpPostedFileBase file4 = fileCollection[3];
                            string fileName1 = UploadFile(file1,payLicense,1);
                            string fileName2 = UploadFile(file2, payLicense, 2);
                            string fileName3 = UploadFile(file3, payLicense, 3);
                            string fileName4 = UploadFile(file4, payLicense, 4);
                            if (fileName1 != "error")
                            {
                                payLicense.ECertificateUrl1 = fileName1;
                            }
                            
                            if (fileName2 != "error")
                            {
                                payLicense.ECertificateUrl2 = fileName2;
                            }
                            
                            if (fileName3 != "error")
                            {
                                payLicense.IDCardUrl1 = fileName3;
                            }
                            
                            if (fileName4 != "error")
                            {
                                payLicense.IDCardUrl2 = fileName4;
                            }
                            //经营信息、商户信息
                            PayLicense pLicense = InsertUpdate(payLicense,id);

                            //商户号（结算账户）
                            string CID3 = (nvl["CID3"] ?? "").Trim();
                            //财务联系人
                            string bankLinkMan = (nvl["bankLinkMan"] ?? "").Trim();
                            //账户类型
                            string AccountType = (nvl["AccountType"] ?? "").Trim();
                            //开户名称
                            string AccountName = (nvl["AccountName"] ?? "").Trim();
                            //联系人电话
                            string bankPhone = (nvl["bankPhone"] ?? "").Trim();
                            //开户银行城市
                            string BankCityTitle = (nvl["BankCityTitle"] ?? "").Trim();
                            //开户银行
                            string BankName = (nvl["BankName"] ?? "").Trim();
                            //开户支行
                            string BranchName = (nvl["BranchName"] ?? "").Trim();
                            //银行账号
                            string AccountNumber = (nvl["AccountNumber"] ?? "").Trim();
                            BankAccount bankAccount = new BankAccount();
                            if (id > 0)
                            {
                                bankAccount = bankAccountService.GetEntityByWhere(o=>o.LicenseId==pLicense.LicenseId);
                            }
                            else
                            {
                                bankAccount.BAccountId = CommonService.GUID.ToUpper();
                                bankAccount.LicenseId = pLicense.LicenseId;
                                bankAccount.State = 1;
                                bankAccount.CreateUID = CurrentUser.UID;
                                bankAccount.CreateDT = dt;
                            }
                            bankAccount.ModifyUID = CurrentUser.UID;
                            bankAccount.ModifyDT = dt;
                            bankAccount.CID = CID3 == "" ? 0 : Convert.ToInt32(CID3);
                            bankAccount.LinkMan = bankLinkMan == "" ? null : bankLinkMan;
                            bankAccount.AccountType = AccountType == "" ? Convert.ToInt16(0) : Convert.ToInt16(AccountType);
                            bankAccount.AccountName = AccountName == "" ? null : AccountName;
                            bankAccount.Phone = bankPhone == "" ? null : bankPhone;
                            bankAccount.BankCityId = getCityID(BankCityTitle);
                            bankAccount.BankName = BankName == "" ? null : BankName;
                            bankAccount.BranchName = BranchName == "" ? null : BranchName;
                            bankAccount.AccountNumber = AccountNumber == "" ? null : AccountNumber;
                            if (id > 0)
                            {
                                if (bankAccount.State == (int)TraderBalanceAccountState.Reject)
                                {
                                    bankAccount.State = (int)TraderBalanceAccountState.NotAuditing;
                                }
                            }
                            //结算账户
                            bankAccountService.InsertUpdate(bankAccount,bankAccount.Id);

                            int CID4 = CID3 == "" ? 0 : Convert.ToInt32(CID3);
                            TradersUser tradersUser = new TradersUser();
                            if (id > 0)
                            {
                                tradersUser = tradersUserService.GetEntityByWhere(o=>o.CID==CID4&&o.AccountType==1);
                            }
                            if(id==0||tradersUser==null)
                            {
                                tradersUser = new TradersUser();
                                tradersUser.TUserId = CommonService.GUID.ToUpper();
                                tradersUser.SysCreateUID = CurrentUser.UID;
                                tradersUser.CreateDT = dt;
                            }
                            tradersUser.CID = CID3 == "" ? 0 : Convert.ToInt32(CID3);
                            tradersUser.LoginName = payLicense.LoginName;
                            tradersUser.FullName = payLicense.AdminName;
                            tradersUser.AccountType = 1;
                            if (id == 0)
                            {
                                tradersUser.State = 1;
                                tradersUser.IsHide = 1;
                            }
                            //商家登录账号
                            tradersUserService.InsertUpdate(tradersUser, tradersUser.Id);

                            ApproveLog approveLog = new ApproveLog();
                            approveLog.ModuleNum = Convert.ToInt16(ApproveLogNum.支付许可);
                            approveLog.ItemId = pLicense.LicenseId;
                            approveLog.CreateTime = dt;
                            approveLog.OperatorUID = CurrentUser.UID;
                            if (id == 0)
                            {
                                approveLog.OperationType = Convert.ToInt16(ApproveLogType.提交申请);
                                approveLog.Description = "已提交支付许可材料信息申请";
                            }
                            else
                            {
                                approveLog.OperationType = Convert.ToInt16(ApproveLogType.重新提交);
                                approveLog.Description = "重新提交了支付许可材料信息申请";
                            }
                            //审核日志
                            approveLogService.InsertUpdate(approveLog,0);

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
        }

        public List<ViewPayLicense> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            //指派人
            var AssignUid = (nvl["AssignUid"] ?? "").Trim();
            //申请日期（开始）
            var CreateDT_begin = (nvl["CreateDT_begin"] ?? "").Trim();
            //申请日期（结束）
            var CreateDT_end = (nvl["CreateDT_end"] ?? "").Trim();
            //服务商号
            var AgentsId = (nvl["AgentsId"] ?? "").Trim();
            //状态
            var State = (nvl["State"] ?? "").Trim();
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
                    strw = strw + " and p.DesigneeId in (" + newAUID + ")";
                }

            }

            if (!CreateDT_begin.IsNullOrEmpty())
            {
                string c = CreateDT_begin + " " + "00:00:00";
                strw = strw + " and p.CreateDT >='" + c + "'";
            }
            if (!CreateDT_end.IsNullOrEmpty())
            {
                var c = CreateDT_end + " " + "23:59:59";
                strw = strw + " and p.CreateDT <='" + c + "'";
            }

            if (!AgentsId.IsNullOrEmpty())
            {
                if (!Tool.IsNumber(AgentsId) || AgentsId.Length > 6)
                {
                    AgentsId = "0";
                }
                strw = strw + " and p.AgentsId=" + AgentsId;
            }

            if (!State.IsNullOrEmpty())
            {
                short sta = Convert.ToInt16(State);
                //未审核
                if (sta == (short)TraderPayLicenseState.NotAuditing)
                {
                    strw = strw + " and (p.State=1 or b.State=1 or k.State=0 or (select COUNT(*) from TradersPaySecretKey where CID=p.CID)=0)";
                }
                //被驳回
                if (sta == (short)TraderPayLicenseState.Reject)
                {
                    strw = strw + " and (p.State=2 or b.State=3 or k.State=0 or (select COUNT(*) from TradersPaySecretKey where CID=p.CID)=0)";
                }
                //已审核
                if (sta == (short)TraderPayLicenseState.Audited)
                {
                    strw = strw + " and (p.State=3 or b.State=2 or k.State=1)";
                }
                //暂停
                if (sta == (short)TraderPayLicenseState.Pause)
                {
                    strw = strw + " and (p.State=4 or b.State=4 or k.State=2)";
                }
                //注销
                if (sta == (short)TraderPayLicenseState.Cancel)
                {
                    strw = strw + " and (p.State=5 or b.State=5 or k.State=3)";
                }
                //无效
                if (sta == (short)TraderPayLicenseState.Invalid)
                {
                    strw = strw + " and (p.State=6 or b.State=6 or k.State=4)";
                }
            }

            if (!keywordType.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                if (keywordType == "0")
                {
                    if (!Tool.IsNumber(keyword) || keyword.Length > 7)
                    {
                        keyword = "0";
                    }
                    strw = strw + " and p.CID=" + keyword;
                }
                if (keywordType == "1")
                {
                    strw = strw + " and t.FullTitle like '%" + keyword + "%'";
                }
            }

            List<ViewPayLicense> list = pRepository.getPageList(pageIndex, pageSize, strw, out recordCount);
            return list;
        }

        /// <summary>
        /// 设为通过
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state">3是设为通过（设为已审核）</param>
        /// <returns></returns>
        public OpResult UpStatus(string ids, short state)
        {
            try
            {
                var idss = ids.Split(',').Select(o => int.Parse(o));
                var list = GetListByWhere(o => idss.Contains(o.Id) && o.State == state);
                if (list.Any())
                {
                    return OpResult.Fail("所选项存在已审核，无法重复审核！");
                }
                var list2 = GetListByWhere(o => idss.Contains(o.Id) && (o.State == (short)TraderPayLicenseState.Cancel || o.State == (short)TraderPayLicenseState.Invalid));
                if (list2.Any())
                {
                    return OpResult.Fail("只能审核商户信息状态为：未审核、被驳回、暂停");
                }
                UpListByWhere(o => idss.Contains(o.Id), o => o.State = state, false);

                var listLog = GetListByWhere(o => idss.Contains(o.Id));
                foreach (var v in listLog)
                {
                    ApproveLog approveLog = new ApproveLog();
                    approveLog.ModuleNum = Convert.ToInt16(ApproveLogNum.支付许可);
                    approveLog.ItemId = v.LicenseId;
                    approveLog.CreateTime = DateTime.Now;
                    approveLog.OperationType = Convert.ToInt16(ApproveLogType.审批);
                    approveLog.OperatorUID = CurrentUser.UID;
                    if (state == (short)TraderPayLicenseState.Audited)
                    {
                        approveLog.Description = "审核通过：支付许可材料信息审核通过";
                    }
                    //审核日志
                    approveLogService.InsertUpdate(approveLog, 0, false);
                }

                var CIDS = GetListByWhere(o => idss.Contains(o.Id)).Select(o=>o.CID);

                UpAdmin(CIDS,0);

                SaveChanges();

                return OpResult.Success();
            }
            catch (Exception e)
            {
                LogEngine.WriteError(e);
                return OpResult.Fail(e.Message);
            }
        }

        /// <summary>
        /// 驳回商户信息申请
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="reason"></param>
        /// <param name="specific"></param>
        /// <returns></returns>
        public OpResult Overrule(string ids, int reason, string specific)
        {
            SysDataDictionary data = dataRepository.GetQuery(o=>o.DicSN==reason).FirstOrDefault();
            var idss = ids.Split(',').Select(o => int.Parse(o));
            OpResult op = UpListByWhere(o => idss.Contains(o.Id), o => o.State = (short)TraderPayLicenseState.Reject);
            if (op.Successed)
            {
                var listLog = GetListByWhere(o => idss.Contains(o.Id));
                foreach (var v in listLog)
                {
                    ApproveLog approveLog = new ApproveLog();
                    approveLog.ModuleNum = Convert.ToInt16(ApproveLogNum.支付许可);
                    approveLog.ItemId = v.LicenseId;
                    approveLog.CreateTime = DateTime.Now;
                    approveLog.OperationType = Convert.ToInt16(ApproveLogType.驳回);
                    approveLog.OperatorUID = CurrentUser.UID;
                    approveLog.Description = "被驳回："+data.Title+"，"+specific;
                    //审核日志
                    approveLogService.InsertUpdate(approveLog, 0);
                }
            }
            return op;
        }

        /// <summary>
        /// 是否存在已经驳回
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public OpResult ExistOverrule(string ids)
        {
            var idss = ids.Split(',').Select(o => int.Parse(o));
            var list = GetListByWhere(o => idss.Contains(o.Id) && o.State == (short)TraderPayLicenseState.Reject);
            if (list.Any())
            {
                return OpResult.Fail("所选项存在商户信息状态为被驳回，无法重复驳回");
            }
            var list2 = GetListByWhere(o => idss.Contains(o.Id) && o.State == (short)TraderPayLicenseState.Cancel);
            if (list2.Any())
            {
                return OpResult.Fail("无法驳回商户信息状态为注销的记录");
            }
            var list3 = GetListByWhere(o => idss.Contains(o.Id) && o.State == (short)TraderPayLicenseState.Invalid);
            if (list3.Any())
            {
                return OpResult.Fail("无法驳回商户信息状态为无效的记录");
            }
            return OpResult.Success();
        }

        /// <summary>
        /// 是否已经设置所选商户支付状态、是否能设置状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state">4是暂停，5是注销，6是无效</param>
        /// <returns></returns>
        public OpResult ExistStatusSel(string ids, short state)
        {
            string msg = "";
            if (state == (short)TraderPayLicenseState.Pause)
            {
                msg = "选择项存在已暂停支付许可，无法重复暂停！";
            }
            else if (state == (short)TraderPayLicenseState.Cancel)
            {
                msg = "选择项存在已注销支付许可，无法重复注销！";
            }
            else if (state == (short)TraderPayLicenseState.Invalid)
            {
                msg = "选择项存在无效支付许可，无法重复设为无效！";
            }
            var idss = ids.Split(',').Select(o => int.Parse(o));
            var list = GetListByWhere(o => idss.Contains(o.Id) && o.State == state);
            if (list.Any())
            {
                return OpResult.Fail(msg);
            }

            if (state == (short)TraderPayLicenseState.Pause)
            {
                var list2 = GetListByWhere(o => idss.Contains(o.Id) && (o.State == (short)TraderPayLicenseState.Cancel || o.State == (short)TraderPayLicenseState.Invalid));
                if (list2.Any())
                {
                    return OpResult.Fail("无法暂停注销和无效的支付许可！");
                }
            }

            if (state == (short)TraderPayLicenseState.Cancel)
            {
                var list3 = GetListByWhere(o => idss.Contains(o.Id) && o.State == (short)TraderPayLicenseState.Invalid);
                if (list3.Any())
                {
                    return OpResult.Fail("无法注销无效的支付许可！");
                }
            }

            if (state == (short)TraderPayLicenseState.Invalid)
            {
                var CID=GetListByWhere(o => idss.Contains(o.Id)).Select(o=>o.CID);
                var list4 = tradersStoreService.GetListByWhere(o => CID.Contains(o.CID));
                if (list4.Any())
                {
                    return OpResult.Fail("所选支付许可在商家门店管理存在记录，无法设置无效！");
                }
            }

            return OpResult.Success();
        }

        /// <summary>
        /// 设为通过、列入黑名单
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state">3是设为通过，9是列入黑名单</param>
        /// <returns></returns>
        public OpResult UpStatusSel(string ids, short state)
        {
            var idss = ids.Split(',').Select(o => int.Parse(o));
            string msg = "";
            if (state == 3)
            {
                msg = "所选项存在已审核通过，无法重复审核！";
            }
            else if (state == 9)
            {
                msg = "所选项存在已列入黑名单，无法重复列入黑名单！";
            }
            var list = GetListByWhere(o => idss.Contains(o.Id) && o.State == state);
            if (list.Any())
            {
                return OpResult.Fail(msg);
            }
            OpResult op = UpListByWhere(o => idss.Contains(o.Id), o => o.State = state);
            if (op.Successed)
            {
                var listLog = GetListByWhere(o => idss.Contains(o.Id));
                foreach (var v in listLog)
                {
                    ApproveLog approveLog = new ApproveLog();
                    approveLog.ModuleNum = Convert.ToInt16(ApproveLogNum.支付许可);
                    approveLog.ItemId = v.LicenseId;
                    approveLog.CreateTime = DateTime.Now;
                    approveLog.OperationType = Convert.ToInt16(ApproveLogType.审批);
                    approveLog.OperatorUID = CurrentUser.UID;
                    if (state == 3)
                    {
                        approveLog.Description = "审核通过：支付许可材料信息审核通过";
                    }
                    else if (state == 9)
                    {
                        approveLog.Description = "列入黑名单";
                    }
                    //审核日志
                    approveLogService.InsertUpdate(approveLog, 0);
                }
            }
            return op;
        }

        /// <summary>
        /// 设置所选商户支付状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state">4是暂停，5是注销，6是无效</param>
        /// <returns></returns>
        public OpResult SetStatus(string ids, short state,string specific)
        {
            try
            {
                var idss = ids.Split(',').Select(o => int.Parse(o));
                //无效
                if (state == (short)TraderPayLicenseState.Invalid)
                {
                    var list = GetListByWhere(o => idss.Contains(o.Id));
                    var LicenseId = list.Select(o => o.LicenseId);
                    var CID = list.Select(o => o.CID);

                    var listStore = tradersStoreService.GetListByWhere(o => CID.Contains(o.CID));
                    if (listStore.Any())
                    {
                        return OpResult.Fail("所选支付许可在商家门店管理存在记录，无法设置无效！");
                    }

                    UpListByWhere(o => idss.Contains(o.Id), o => o.State = state, false);
                    bankAccountService.UpListByWhere(o => LicenseId.Contains(o.LicenseId), o => o.State = (int)TraderBalanceAccountState.Invalid, false);
                    tradersPaySecretKeyService.UpListByWhere(o => CID.Contains(o.CID), o => o.State = (int)TraderPayCchannelState.Invalid, false);
                }
                else
                {
                    UpListByWhere(o => idss.Contains(o.Id), o => o.State = state, false);
                }

                var listLog = GetListByWhere(o => idss.Contains(o.Id));
                foreach (var v in listLog)
                {
                    ApproveLog approveLog = new ApproveLog();
                    approveLog.ModuleNum = Convert.ToInt16(ApproveLogNum.支付许可);
                    approveLog.ItemId = v.LicenseId;
                    approveLog.CreateTime = DateTime.Now;
                    approveLog.OperationType = Convert.ToInt16(ApproveLogType.审批);
                    approveLog.OperatorUID = CurrentUser.UID;
                    if (state == (short)TraderPayLicenseState.Pause)
                    {
                        approveLog.Description = "被暂停支付" + (specific == "" ? "" : "：" + specific);
                    }
                    else if (state == (short)TraderPayLicenseState.Cancel)
                    {
                        approveLog.Description = "被注销支付" + (specific == "" ? "" : "：" + specific);
                    }
                    else if (state == (short)TraderPayLicenseState.Invalid)
                    {
                        approveLog.Description = "被设为无效支付" + (specific == "" ? "" : "：" + specific);
                    }

                    //审核日志
                    approveLogService.InsertUpdate(approveLog, 0, false);
                }

                SaveChanges();
                return OpResult.Success();
            }
            catch (Exception e)
            {
                LogEngine.WriteError(e);
                return OpResult.Fail(e.Message);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Utility.OpResult Deletes(string ids)
        {
            var idss = ids.Split(',').Select(o => int.Parse(o));
            var op = new OpResult();
            try
            {
                var list = GetListByWhere(o => idss.Contains(o.Id));
                var CID = list.Select(o => o.CID);
                if (!list.Any())
                {
                    op.Message = "查不到数据";
                    op.Successed = false;
                    return op;
                }

                var list1 = GetListByWhere(o => idss.Contains(o.Id)&&o.CreateUID!=CurrentUser.UID);
                if (list1.Any())
                {
                    op.Message = "只能删除当前登录用户提交的记录！";
                    op.Successed = false;
                    return op;
                }

                var listStore = tradersStoreService.GetListByWhere(o => CID.Contains(o.CID));
                if (listStore.Any())
                {
                    op.Message = "所选支付许可在商家门店管理存在记录，无法删除！";
                    op.Successed = false;
                    return op;
                }

                var list2 = GetListByWhere(o => idss.Contains(o.Id) && o.State != (short)TraderPayLicenseState.Invalid);
                if (list2.Any())
                {
                    op.Message = "只能删除商户信息状态无效、结算账户状态无效、支付通道无效或未开通的记录！";
                    op.Successed = false;
                    return op;
                }

                var lid = list.Select(o => o.LicenseId);
                var list3 = bankAccountService.GetListByWhere(o => lid.Contains(o.LicenseId) && o.State != (int)TraderBalanceAccountState.Invalid);
                if (list3.Any())
                {
                    op.Message = "只能删除商户信息状态无效、结算账户状态无效、支付通道无效或未开通的记录！";
                    op.Successed = false;
                    return op;
                }

                var list4 = tradersPaySecretKeyService.GetListByWhere(o => CID.Contains(o.CID) && o.State != (int)TraderPayCchannelState.Invalid);
                if (list4.Any())
                {
                    op.Message = "只能删除商户信息状态无效、结算账户状态无效、支付通道无效或未开通的记录！";
                    op.Successed = false;
                    return op;
                }

                var LicenseIds = list.Select(o=>o.LicenseId);
                var CIDS = list.Select(o => o.CID);

                var delLog=approveLogService.GetListByWhere(o=>LicenseIds.Contains(o.ItemId));
                approveLogService.dels(delLog,false);

                var delPay = tradersPaySecretKeyService.GetListByWhere(o => CIDS.Contains(o.CID));
                var TPaySecrectId = delPay.Select(o=>o.TPaySecrectId);
                var delChannel = tradersPayChannelService.GetListByWhere(o=>TPaySecrectId.Contains(o.TPaySecrectId));
                tradersPayChannelService.dels(delChannel,false);
                tradersPaySecretKeyService.dels(delPay,false);

                var delBank = bankAccountService.GetListByWhere(o => LicenseIds.Contains(o.LicenseId));
                bankAccountService.dels(delBank,false);

                var delLicense = GetListByWhere(o => LicenseIds.Contains(o.LicenseId));
                dels(delLicense,false);

                SaveChanges();

                op.Successed = true;
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                op.Successed = false;
                LogEngine.WriteError(ex);
            }
            return op;
        }

        /// <summary>
        /// 获取一级类目
        /// </summary>
        /// <returns></returns>
        public List<Business> getBusiness1(string defaultTitle = "请选择")
        {
            var business1 = BusinessRepository.GetQuery(o => o.Status == 1 && (o.ParentId==null||o.ParentId=="")).ToList();
            business1.Insert(0, new Business() { ById = "", Title = defaultTitle });
            return business1;
        }

        /// <summary>
        /// 获取二级类目
        /// </summary>
        /// <returns></returns>
        public List<Business> getBusiness2(string ParentId, string defaultTitle = "请选择")
        {
            List<Business> business2 = new List<Business>();
            if (!ParentId.IsNullOrEmpty())
            {
                business2 = BusinessRepository.GetQuery(o => o.Status == 1 && o.ParentId == ParentId).ToList();
            }
            business2.Insert(0, new Business() { ById = "", Title = defaultTitle });
            return business2;
        }
        
        /// <summary>
        /// 更新商家管理员登录账号（没有SaveChanges）
        /// </summary>
        /// <param name="cids"></param>
        /// <param name="type">0是支付许可，1是商家结算账户，2是商家支付通道</param>
        /// <returns></returns>
        public OpResult UpAdmin(IEnumerable<int?> cids, int type)
        {
            foreach(var v in cids)
            {
                var isLicense = isExistByWhere(o => o.CID == v && o.State == (short)TraderPayLicenseState.Audited);
                var isBank = bankAccountService.isExistByWhere(o => o.CID == v && o.State == (int)TraderBalanceAccountState.Enabled);
                var isPay = tradersPaySecretKeyService.isExistByWhere(o => o.CID == v && o.State == (int)TraderPayCchannelState.Enabled);
                bool isUp = false;
                //支付许可
                if (type == 0)
                {
                    if (isBank && isPay)
                    {
                        isUp = true;
                    }
                }
                //商家结算账户
                if (type == 1)
                {
                    if (isLicense && isPay)
                    {
                        isUp = true;
                    }
                }
                //商家支付通道
                if (type == 2)
                {
                    if (isLicense && isBank)
                    {
                        isUp = true;
                    }
                }
                if (isUp)
                {
                    TradersUser tradersUser = tradersUserService.GetEntityByWhere(o=>o.CID==v&&o.AccountType==1);
                    if (tradersUser != null)
                    {
                        tradersUser.State = 2;
                        tradersUser.IsHide = 0;
                        tradersUserService.InsertUpdate(tradersUser, tradersUser.Id, false);
                    }
                }

            }
            return OpResult.Success();
        }
    }
}
