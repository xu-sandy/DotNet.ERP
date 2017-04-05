using Pharos.Logic.OMS.DAL;
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
using System.Transactions;
using Newtonsoft.Json.Linq;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// BLL商家支付主密钥
    /// </summary>
    public class TradersPaySecretKeyService : BaseService<TradersPaySecretKey>
    {
        // 商家门店支付通道DAL
        [Ninject.Inject]
        ITradersPaySecretKeyRepository tRepository { get; set; }

        //收单渠道
        [Ninject.Inject]
        IBaseRepository<PayChannelManage> PayChannelManageRepository { get; set; }

        //收单渠道详情
        [Ninject.Inject]
        IBaseRepository<PayChannelDetail> PayChannelDetailRepository { get; set; }

        //商家支付通道
        [Ninject.Inject]
        IBaseRepository<TradersPayChannel> TradersPayChannelRepository { get; set; }

        //商家支付许可
        //[Ninject.Inject]
        PayLicenseService payLicenseService
        {
            get
            {
                return new PayLicenseService();
            }
        }

        //商家支付通道
        //[Ninject.Inject]
        TradersPayChannelService tradersPayChannelService
        {
            get
            {
                return new TradersPayChannelService();
            }
        }

        //商家支付通道
        //[Ninject.Inject]
        ApproveLogService approveLogService
        {
            get
            {
                return new ApproveLogService(); 
            }
        }

        //BLL商家登录账号
        //[Ninject.Inject]
        TradersUserService tradersUserService
        {
            get
            {
                return new TradersUserService();
            }
        }

        //BLL-----商户资料
        TradersService tradersService 
        {
            get
            {
                return new TradersService();
            }
        }
        /// <summary>
        /// 获取CID
        /// </summary>
        public List<PayLicense> GetCIDWhere(System.Collections.Specialized.NameValueCollection nvl)
        {
            //关键字
            var keyword = (nvl["keyword"] ?? "").Trim();
            if (keyword == "")
            {
                keyword = "-1";
            }
            string keyw = "'" + keyword + "%'";
            return tRepository.getListCID(keyw);
        }

        public List<ViewTradersPaySecretKey> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            //指派人
            var AssignerUID = (nvl["AssignerUID"] ?? "").Trim();
            //创建日期（开始）
            var CreateDT_begin = (nvl["CreateDT_begin"] ?? "").Trim();
            //创建日期（结束）
            var CreateDT_end = (nvl["CreateDT_end"] ?? "").Trim();
            //排序方式
            var OrBy = (nvl["OrBy"] ?? "").Trim();
            //关键字类型
            var keywordType = (nvl["keywordType"] ?? "").Trim();
            //关键字
            var keyword = (nvl["keyword"] ?? "").Trim();
            //状态
            var State = (nvl["State"] ?? "").Trim();

            var pageIndex = 1;
            var pageSize = 20;
            if (!nvl["page"].IsNullOrEmpty())
                pageIndex = int.Parse(nvl["page"]);
            if (!nvl["rows"].IsNullOrEmpty())
                pageSize = int.Parse(nvl["rows"]);

            string strw = "";
            string OrderBy = "";

            if (!AssignerUID.IsNullOrEmpty())
            {
                string[] aUID = AssignerUID.Split(',');
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
                    strw = strw + " and p.AssignUID in (" + newAUID + ")";
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

            if (OrBy == "1")
            {
                OrderBy = "p.CreateDT desc ";
            }
            else if (OrBy == "2")
            {
                OrderBy = "p.AuditDT desc ";
            }

            if (State != "-1")
            {
                strw = strw + " and p.State=" + State;
            }

            if (!keywordType.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                if (keywordType == "1")
                {
                    if (!Tool.IsNumber(keyword) || keyword.Length > 7)
                    {
                        keyword = "0";
                    }
                    strw = strw + " and p.CID=" + keyword;
                }
                if (keywordType == "2")
                {
                    strw = strw + " and t.FullTitle like '%" + keyword + "%'";
                }
            }

            List<ViewTradersPaySecretKey> list = tRepository.getPageList(pageIndex, pageSize, strw, out recordCount,OrderBy);
            return list;
        }

        /// <summary>
        /// 获取支付通道
        /// </summary>
        /// <returns></returns>
        public List<PayChannelManage> GetPayChannelManage()
        {
            var query = PayChannelManageRepository.GetQuery(o=>o.State==1);
            return query.ToList();
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <param name="ChannelNo"></param>
        /// <returns></returns>
        public List<ViewPayChannelDetail> GetPayChannelDetail(int ChannelNo)
        {
            List<ViewPayChannelDetail> list = tRepository.GetPayManner(ChannelNo);
            List<ViewPayChannelDetail> list2 = new List<ViewPayChannelDetail>();
            if (list.Count > 0)
            {
                foreach (var v in list)
                {
                    ViewPayChannelDetail viewPayChannelDetail = new ViewPayChannelDetail();
                    viewPayChannelDetail.ChannelDetailId = v.ChannelDetailId;
                    viewPayChannelDetail.ChannelPayMode = v.ChannelPayMode;
                    if (v.ChannelPayMode == 1)
                    {
                        viewPayChannelDetail.PayManner = "扫码支付";
                    }
                    else if (v.ChannelPayMode == 2)
                    {
                        viewPayChannelDetail.PayManner = "网站支付";
                    }
                    else if (v.ChannelPayMode == 3)
                    {
                        viewPayChannelDetail.PayManner = "刷卡支付";
                    }
                    list2.Add(viewPayChannelDetail);
                }
            }
            list2.Insert(0, new ViewPayChannelDetail() { ChannelDetailId = "", PayManner = "请选择" });
            return list2;
        }

        /// <summary>
        /// 获取商家支付通道
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<TradersPayChannel> GetTradersPayChannel(int Id)
        {
            TradersPaySecretKey tradersPaySecretKey = GetEntityById(Id);
            List<TradersPayChannel> list = new List<TradersPayChannel>();
            if (tradersPaySecretKey != null)
            {
                list = TradersPayChannelRepository.GetQuery(o=>o.TPaySecrectId==tradersPaySecretKey.TPaySecrectId).ToList();
            }
            return list;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpResult Verification(System.Collections.Specialized.NameValueCollection nvl, int id, TradersPaySecretKey tradersPaySecretKey)
        {
            var list = tradersService.getUserList().Where(o=>o.UserId==tradersPaySecretKey.AssignUID);
            if (!list.Any())
            {
                return OpResult.Fail("指派人不正确");
            }

            //商户号
            var CID = (nvl["CID"] ?? "").Trim();
            if (CID != "")
            {
                if (!Tool.IsNumber(CID))
                {
                    return OpResult.Fail("商户不存在");
                }
                else
                {
                    int cid = Convert.ToInt32(CID);
                    if (id > 0)
                    {
                        if (isExistByWhere(o => o.CID == cid&&o.Id!=id))
                        {
                            return OpResult.Fail("该商户已经申请过支付通道，无法重复申请");
                        }
                        if (!payLicenseService.isExistByWhere(o => o.CID == cid))
                        {
                            return OpResult.Fail("该商户未申请支付授权许可");
                        }
                    }
                    else
                    {
                        if (isExistByWhere(o => o.CID == cid))
                        {
                            return OpResult.Fail("该商户已经申请过支付通道，无法重复申请");
                        }
                        if (!payLicenseService.isExistByWhere(o => o.CID == cid))
                        {
                            return OpResult.Fail("该商户未申请支付授权许可");
                        }
                    }
                }
            }
            return OpResult.Success();
        }


        public OpResult Save(TradersPaySecretKey tradersPaySecretKey, int id, DateTime dt, System.Collections.Specialized.NameValueCollection nvl)
        {
            //验证
            var op = Verification(nvl, id, tradersPaySecretKey);
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
                            //商家支付主密钥
                            TradersPaySecretKey PaySecretKey = InsertUpdate(tradersPaySecretKey, id);
                            //支付方式
                            string h_PayManner = (nvl["h_PayManner"] ?? "").Trim();
                            if (!h_PayManner.IsNullOrEmpty())
                            {
                                JObject jObj = null;
                                jObj = JObject.Parse(h_PayManner);
                                JArray jlist = JArray.Parse(jObj["TradersPayChannel"].ToString());
                                tradersPayChannelService.DeleteByWhere(o=>o.TPaySecrectId==PaySecretKey.TPaySecrectId);
                                foreach (JObject item in jlist)
                                {
                                    short ChannelPayMode = Convert.ToInt16(item["ChannelPayMode"]);
                                    string PayNotifyUrl = item["PayNotifyUrl"].ToString();
                                    string RfdNotifyUrl = item["RfdNotifyUrl"].ToString();

                                    TradersPayChannel tradersPayChannel = new TradersPayChannel();
                                    tradersPayChannel.TPayChannelId = CommonService.GUID.ToUpper();
                                    tradersPayChannel.TPaySecrectId = PaySecretKey.TPaySecrectId;
                                    tradersPayChannel.ChannelPayMode = ChannelPayMode;
                                    tradersPayChannel.PayNotifyUrl = PayNotifyUrl;
                                    tradersPayChannel.RfdNotifyUrl = RfdNotifyUrl;
                                    tradersPayChannel.State = 1;
                                    tradersPayChannel.CreateUID = CurrentUser.UID;
                                    tradersPayChannel.CreateDT = dt;
                                    tradersPayChannel.ModifyUID = CurrentUser.UID;
                                    tradersPayChannel.ModifyDT = dt;
                                    tradersPayChannelService.InsertUpdate(tradersPayChannel,0);
                                }
                            }
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

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state">1是可用，2是暂停，3是注销，4是无效</param>
        /// <returns></returns>
        public OpResult UpState(string ids, short state)
        {
            OpResult opr = ExistState(ids, state);
            if (!opr.Successed)
            {
                return opr;
            }
            else
            {
                try
                {
                    using (EFDbContext context = new EFDbContext())
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            var idss = ids.Split(',').Select(o => int.Parse(o));
                            DateTime dt = DateTime.Now;
                            string UID = CurrentUser.UID;
                            UpListByWhere(o => idss.Contains(o.Id), o =>
                            {
                                o.State = state;
                                o.AuditDT = dt;
                                o.AuditUID = UID;
                                o.ModifyDT = dt;
                                o.ModifyUID = UID;
                            });

                            string des = "";
                            if (state == (short)TraderPayCchannelState.Enabled)
                            {
                                des = "支付通道已被设为可用";
                            }
                            if (state == (short)TraderPayCchannelState.Pause)
                            {
                                des = "支付通道已被设为暂停";
                            }

                            var listLog = GetListByWhere(o => idss.Contains(o.Id));
                            foreach (var v in listLog)
                            {
                                string LicenseId = payLicenseService.GetEntityByWhere(o=>o.CID==v.CID).LicenseId;
                                ApproveLog approveLog = new ApproveLog();
                                approveLog.ModuleNum = Convert.ToInt16(ApproveLogNum.支付许可);
                                approveLog.ItemId = LicenseId;
                                approveLog.CreateTime = dt;
                                approveLog.OperationType = Convert.ToInt16(ApproveLogType.审批);
                                approveLog.OperatorUID = UID;
                                approveLog.Description = des;
                                //审核日志
                                approveLogService.InsertUpdate(approveLog, 0);
                            }

                            if (state == (short)TraderPayCchannelState.Enabled)
                            {
                                var CIDS = GetListByWhere(o => idss.Contains(o.Id)).Select(o => o.CID);
                                payLicenseService.UpAdmin(CIDS, 2);
                                SaveChanges();
                            }
                            
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

        /// <summary>
        /// 是否已经设置状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state">1是可用，2是暂停，3是注销，4是无效</param>
        /// <returns></returns>
        public OpResult ExistState(string ids, short state)
        {
            string msg = "";
            string msg2 = "";
            if (state == 1)
            {
                msg = "选择项存在可用支付通道，无法重复设置可用！";
                msg2 = "无法设为可用支付通道，因为存在状态为无效的！";
            }
            else if (state == 2)
            {
                msg = "选择项存在暂停支付通道，无法重复暂停！";
                msg2 = "无法暂停状态为无效的！";
            }
            else if (state == 3)
            {
                msg = "选择项存在注销支付通道，无法重复注销！";
                msg2 = "无法注销状态为无效的！";
            }
            else if (state == 4)
            {
                msg = "选择项存在无效支付通道，无法重复设置无效！";
            }
            var idss = ids.Split(',').Select(o => int.Parse(o));
            var list = GetListByWhere(o => idss.Contains(o.Id) && o.State == state);
            if (list.Any())
            {
                return OpResult.Fail(msg);
            }

            var list2 = GetListByWhere(o => idss.Contains(o.Id) && o.State ==(int) TraderPayCchannelState.Invalid);
            if (list2.Any())
            {
                return OpResult.Fail(msg2);
            }

            return OpResult.Success();
        }
    }
}
