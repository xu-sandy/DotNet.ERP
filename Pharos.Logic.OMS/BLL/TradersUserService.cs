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

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// BLL商家登录账号
    /// </summary>
    public class TradersUserService : BaseService<TradersUser>
    {
        // 商家门店支付通道DAL
        [Ninject.Inject]
        ITradersPaySecretKeyRepository pRepository { get; set; }

        // DAL商家登录账号
        [Ninject.Inject]
        ITradersUserRepository itradersUserRepository { get; set; }

        //商家门店
        [Ninject.Inject]
        IBaseRepository<TradersStore> tradersStoreRepository { get; set; }

        //BLL商户资料
        [Ninject.Inject]
        TradersService tradersService { get; set; }

        //BLL商家门店
        //[Ninject.Inject]
        TradersStoreService tradersStoreService
        {
            get
            {
                return new TradersStoreService();
            }
        }

        //BLL商家支付许可档案
        PayLicenseService payLicenseService
        {
            get
            {
                return new PayLicenseService();
            }
        }

        /// <summary>
        /// 获取CID
        /// </summary>
        public List<TradersStore> GetCIDStore(System.Collections.Specialized.NameValueCollection nvl)
        {
            //关键字
            var keyword = (nvl["keyword"] ?? "").Trim();
            if (keyword == "")
            {
                keyword = "-1";
            }
            string keyw = "'" + keyword + "%'";
            return itradersUserRepository.getCIDStore(" and CID like " + keyw);
        }

        /// <summary>
        /// 获取商户全称
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public string getTradersFullTitle(int CID)
        {
            string TradersFullTitle = "";
            bool isCID = tradersStoreService.isExistByWhere(o=>o.CID==CID&&o.State==1);
            if (isCID)
            {
                Traders traders = tradersService.GetEntityByWhere(o => o.CID == CID);
                if (traders != null)
                {
                    TradersFullTitle = traders.FullTitle;
                }
                else
                {
                    TradersFullTitle = new Traders().FullTitle;
                }
            }
            return TradersFullTitle;
        }

        /// <summary>
        /// 获取门店编号
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public List<TradersStore> getStoreNum(int CID)
        {
            var list = tradersStoreService.GetListByWhere(o=>o.State==1&&o.CID==CID).OrderBy(o=>o.CID).ToList();
            List<TradersStore> list2 = new List<TradersStore>();
            foreach(var v in list)
            {
                TradersStore tradersStore = new TradersStore();
                tradersStore.StoreNumStr = v.StoreNum.ToString();
                tradersStore.TStoreInfoId = v.TStoreInfoId;
                list2.Add(tradersStore);
            }
            list2.Insert(0, new TradersStore() { TStoreInfoId="", StoreNumStr = "请选择" });
            return list2;
        }

        /// <summary>
        /// 获取门店全称
        /// </summary>
        /// <param name="TStoreInfoId"></param>
        /// <returns></returns>
        public TradersStore getStoreFullTitle(string TStoreInfoId)
        {
            TradersStore tradersStore = tradersStoreService.GetEntityByWhere(o=>o.TStoreInfoId==TStoreInfoId&&o.State==1);
            if (tradersStore != null)
            {
                return tradersStore;
            }
            else
            {
                return new TradersStore();
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpResult Verification(System.Collections.Specialized.NameValueCollection nvl, int id)
        {
            //商户号
            var CID = (nvl["CID"] ?? "").Trim();
            //门店编号
            var TStoreInfoId = (nvl["TStoreInfoId"] ?? "").Trim();
            //登录账号
            var LoginName = (nvl["LoginName"] ?? "").Trim();
            //联系电话
            var Phone = (nvl["Phone"] ?? "").Trim();

            if (CID != "")
            {
                if (!Tool.IsNumber(CID))
                {
                    return OpResult.Fail("商户号不存在");
                }
                else
                {
                    int cid = Convert.ToInt32(CID);
                    if (!tradersStoreService.isExistByWhere(o => o.CID == cid&&o.State==1))
                    {
                        return OpResult.Fail("商户号没有门店");
                    } 
                }
            }

            if (TStoreInfoId != "")
            {
                if (!tradersStoreService.isExistByWhere(o => o.TStoreInfoId == TStoreInfoId&&o.State==1))
                {
                    return OpResult.Fail("所选门店编号不可用");
                }
            }

            if (LoginName != "")
            {
                if (id > 0)
                {
                    if (isExistByWhere(o => o.LoginName == LoginName && o.Id != id))
                    {
                        return OpResult.Fail("登录帐号不可用，请使用其它登录账号");
                    }
                }
                else
                {
                    if (isExistByWhere(o => o.LoginName == LoginName))
                    {
                        return OpResult.Fail("登录帐号不可用，请使用其它登录账号");
                    }
                }
            }

            if (Phone != "")
            {
                if (id > 0)
                {
                    if (isExistByWhere(o => o.Phone == Phone && o.Id != id))
                    {
                        return OpResult.Fail("联系电话已经存在");
                    }
                }
                else
                {
                    if (isExistByWhere(o => o.Phone == Phone))
                    {
                        return OpResult.Fail("联系电话已经存在");
                    }
                }
            }

            return OpResult.Success();
        }

        public OpResult Save(TradersUser tradersUser, int id, System.Collections.Specialized.NameValueCollection nvl)
        {
            //验证
            var op = Verification(nvl, id);
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
                            InsertUpdate(tradersUser,id);
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

        public List<ViewTradersUser> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            //状态
            var State = (nvl["State"] ?? "").Trim();
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

            if (State != "0")
            {
                strw = strw + " and u.State=" + State;
            }

            if (!CreateDT_begin.IsNullOrEmpty())
            {
                string c = CreateDT_begin + " " + "00:00:00";
                strw = strw + " and u.CreateDT >='" + c + "'";
            }
            if (!CreateDT_end.IsNullOrEmpty())
            {
                var c = CreateDT_end + " " + "23:59:59";
                strw = strw + " and u.CreateDT <='" + c + "'";
            }

            if (!keywordType.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                if (keywordType == "1")
                {
                    strw = strw + " and u.FullName like '%" + keyword + "%'";
                }
                if (keywordType == "2")
                {
                    strw = strw + " and u.LoginName like '%" + keyword + "%'";
                }
                if (keywordType == "3")
                {
                    strw = strw + " and u.Phone like '%" + keyword + "%'";
                }
            }

            List<ViewTradersUser> list = itradersUserRepository.getPageList(pageIndex, pageSize, strw, out recordCount);
            return list;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state">1是可用，2是暂停，3是注销，4是无效</param>
        /// <returns></returns>
        public OpResult UpState(string ids, int state)
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
                    var idss = ids.Split(',').Select(o => int.Parse(o));
                    UpListByWhere(o => idss.Contains(o.Id), o =>
                    {
                        o.State = state;
                    });
                    return OpResult.Success();
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
        /// <param name="state">1:未审核，2:可用，3:停用</param>
        /// <returns></returns>
        public OpResult ExistState(string ids, int state)
        {
            string msg = "";
            if (state == 2)
            {
                msg = "选择项存在可用账号，无法重复设置可用！";
            }
            else if (state == 3)
            {
                msg = "选择项存在停用账号，无法重复设置停用！";
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
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public OpResult Delete(string ids)
        {
            var idss = ids.Split(',').Select(o => int.Parse(o));
            var list = GetListByWhere(o => idss.Contains(o.Id));
            dels(list);
            return OpResult.Success();
        }

    }
}
