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
using System.Transactions;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// BLL商家门店
    /// </summary>
    public class TradersStoreService : BaseService<TradersStore>
    {
        // 代理商档案
        [Ninject.Inject]
        ITradersStoreRepository tRepository { get; set; }

        // 商家门店支付通道DAL
        [Ninject.Inject]
        ITradersPaySecretKeyRepository pRepository { get; set; }

        //商家支付许可
        [Ninject.Inject]
        PayLicenseService payLicenseService { get; set; }

        //BLL商家登录账号
        //[Ninject.Inject]
        TradersUserService tradersUserService
        {
            get
            {
                return new TradersUserService(); 
            }
        }


        //BLL商家结算账户
        BankAccountService bankAccountService
        {
            get
            {
                return new BankAccountService();
            }
        }

        //BLL商家支付主密钥
        TradersPaySecretKeyService tradersPaySecretKeyService
        {
            get
            {
                return new TradersPaySecretKeyService();
            }
        }

        // BLL-----商户资料
        TradersService tradersService
        {
            get
            {
                return new TradersService();
            }
        }

        public List<ViewTradersStore> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            //指派人
            var AssignerUID = (nvl["AssignerUID"] ?? "").Trim();
            //创建日期（开始）
            var CreateDT_begin = (nvl["CreateDT_begin"] ?? "").Trim();
            //创建日期（结束）
            var CreateDT_end = (nvl["CreateDT_end"] ?? "").Trim();
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
                    strw = strw + " and s.AssignUID in (" + newAUID + ")";
                }
            }

            if (!CreateDT_begin.IsNullOrEmpty())
            {
                string c = CreateDT_begin + " " + "00:00:00";
                strw = strw + " and s.CreateDT >='" + c + "'";
            }
            if (!CreateDT_end.IsNullOrEmpty())
            {
                var c = CreateDT_end + " " + "23:59:59";
                strw = strw + " and s.CreateDT <='" + c + "'";
            }

            if (State!="-1")
            {
                strw = strw + " and s.State="+State;
            }

            if (!keywordType.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                if (keywordType == "1")
                {
                    if (!Tool.IsNumber(keyword) || keyword.Length > 6)
                    {
                        keyword = "0";
                    }
                    strw = strw + " and s.CID=" + keyword;
                }
                if (keywordType == "2")
                {
                    if (!Tool.IsNumber(keyword) || keyword.Length > 10)
                    {
                        keyword = "0";
                    }
                    strw = strw + " and s.StoreNum=" + keyword;
                }
                if (keywordType == "3")
                {
                    strw = strw + " and s.StoreName like '%" + keyword + "%'";
                }
                if (keywordType == "4")
                {
                    strw = strw + " and e.LoginName like '%" + keyword + "%'";
                }
            }

            List<ViewTradersStore> list = tRepository.getPageList(pageIndex, pageSize, strw, out recordCount);
            return list;
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
            return pRepository.getListCID(keyw, " and State=3 ");
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpResult Verification(System.Collections.Specialized.NameValueCollection nvl, int id, TradersStore tradersStore)
        {
            //商户号
            var CID = (nvl["CID"] ?? "").Trim();
            //门店名称
            var StoreName = (nvl["StoreName"] ?? "").Trim();

            if (CID != "")
            {
                if (!Tool.IsNumber(CID))
                {
                    return OpResult.Fail("商户号不存在");
                }
                else
                {
                    int cid = Convert.ToInt32(CID);
                    if (!payLicenseService.isExistByWhere(o => o.CID == cid))
                    {
                        return OpResult.Fail("该商户号未申请支付许可");
                    }
                    else
                    {
                        if (!payLicenseService.isExistByWhere(o => o.CID == cid && o.State == (short)TraderPayLicenseState.Audited))
                        {
                            return OpResult.Fail("该商户号当前商户信息状态不是已审核，无法新增/修改门店");
                        }
                        else if (!bankAccountService.isExistByWhere(o => o.CID == cid && o.State == (int)TraderBalanceAccountState.Enabled))
                        {
                            return OpResult.Fail("该商户号当前结算账户状态不是可用，无法新增/修改门店");
                        }
                        else if (!tradersPaySecretKeyService.isExistByWhere(o => o.CID == cid && o.State == (int)TraderPayCchannelState.Enabled))
                        {
                            return OpResult.Fail("该商户号当前支付通道状态不是可用，无法新增/修改门店");
                        }
                        else
                        {
                            var list = tradersService.getUserList().Where(o => o.UserId == tradersStore.AssignUID);
                            if (!list.Any())
                            {
                                return OpResult.Fail("指派人不正确");
                            }

                            if (id > 0)
                            {
                                if (isExistByWhere(o => o.CID == cid && o.StoreName == StoreName && o.Id != id))
                                {
                                    return OpResult.Fail("该门店名称已经存在");
                                }
                            }
                            else
                            {
                                if (isExistByWhere(o => o.CID == cid && o.StoreName == StoreName))
                                {
                                    return OpResult.Fail("该门店名称已经存在");
                                }
                            }

                        }
                    }
                }
            }
            return OpResult.Success();
        }

        public OpResult Save(TradersStore tradersStore, int id, System.Collections.Specialized.NameValueCollection nvl)
        {
            //验证
            var op = Verification(nvl, id, tradersStore);
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
                            InsertUpdate(tradersStore,id);
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
            if (state == 1)
            {
                msg = "选择项存在可用门店，无法重复设置可用！";
            }
            else if (state == 2)
            {
                msg = "选择项存在暂停门店，无法重复暂停！";
            }
            else if (state == 3)
            {
                msg = "选择项存在注销门店，无法重复注销！";
            }
            else if (state == 4)
            {
                msg = "选择项存在无效门店，无法重复设置无效！";
            }
            var idss = ids.Split(',').Select(o => int.Parse(o));
            var list = GetListByWhere(o => idss.Contains(o.Id) && o.State == state);
            if (list.Any())
            {
                return OpResult.Fail(msg);
            }
            return OpResult.Success();
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
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public OpResult Delete(string ids)
        {
            var idss = ids.Split(',').Select(o => int.Parse(o));
            var list = GetListByWhere(o=>idss.Contains(o.Id));
            var TStoreInfoIds = list.Select(o=>o.TStoreInfoId);
            var listUser = tradersUserService.GetListByWhere(o => TStoreInfoIds.Contains(o.TStoreInfoId));
            if (listUser.Any())
            {
                return OpResult.Fail("所选门店存在商家登录账号，无法删除！");
            }
            else
            {
                dels(list);
                return OpResult.Success();
            } 
        }

        /// <summary>
        /// 获取最大门店编号
        /// </summary>
        /// <returns></returns>
        public string getMaxStoreNum()
        {
            ViewMaxStoreNum viewMaxStoreNum = tRepository.getMaxStoreNum();
            long StoreNum = 0;
            if (viewMaxStoreNum != null)
            {
                StoreNum = viewMaxStoreNum.MaxStoreNum;
                if (StoreNum <= 100)
                {
                    StoreNum = 100;
                }
                StoreNum = StoreNum + 1;
            }
            else
            {
                StoreNum = 101;
            }
            return StoreNum.ToString().PadLeft(13, '0'); 
        }
    }
}
