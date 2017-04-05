﻿using Newtonsoft.Json.Linq;
using Pharos.Api.Retailing.Models;
using Pharos.Api.Retailing.Models.Pos;
using Pharos.Infrastructure.Data.Normalize;
using Pharos.Logic.ApiData.Pos;
using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Logic.ApiData.Pos.Sale;
using Pharos.Logic.ApiData.Pos.Sale.AfterSale;
using Pharos.Logic.ApiData.Pos.Sale.Category;
using Pharos.Logic.ApiData.Pos.Sale.Payment;
using Pharos.Logic.ApiData.Pos.Sale.Suspend;
using Pharos.Logic.ApiData.Pos.User;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using Pharos.ObjectModels.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace Pharos.Api.Retailing.Controllers
{
    /// <summary>
    /// 销售相关接口
    /// </summary>
    [RoutePrefix("api")]
    public class SaleController : ApiController
    {
        [Route("ApiEnable")]
        [HttpPost]
        public bool ApiEnable()
        {
            return true;
        }
        /// <summary>
        /// 启用门店促销、重置门店促销、刷新门店促销
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns>无返回</returns>
        [Route("RefreshMarketing")]
        [HttpPost]
        public object RefreshMarketing([FromBody] BaseApiParams requestParams)
        {
            StoreManager.SetUpStore(requestParams.CID, requestParams.StoreId);
            return null;
        }

        /// <summary>
        /// 公告
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("Announcements")]
        [HttpPost]
        public IEnumerable<Announcement> Announcements([FromBody] PaysStatusRequest requestParams)
        {
            return new WarehouseManager().Announcements(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
        }
        [Route("GetAreas")]
        [HttpPost]
        public IEnumerable<object> GetAreas([FromBody]AreaInfoBaseApiParams requestParams)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return dataAdapter.GetAreas(requestParams.PID);
        }
        [Route("GetStoredValueCardPayDetails")]
        [HttpPost]
        public IEnumerable<StoredValueCardPayDetailsItem> GetStoredValueCardPayDetails([FromBody]FindStoredValueCardPayDetailsRequest requestParams)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return dataAdapter.GetStoredValueCardPayDetails(requestParams.CardNo, requestParams.Start, requestParams.End);
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("StoredValueCardRecharge")]
        [HttpPost]
        public StoredValueCardRechargeResult StoredValueCardRecharge([FromBody] StoredValueCardRechargeRequest requestParams)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return dataAdapter.StoredValueCardRecharge(requestParams.Type, requestParams.CardNo, requestParams.Amount);
        }

        [Route("AddMember")]
        [HttpPost]
        public object AddMember([FromBody]AddMemberRequest requestParams)
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);

            if (string.IsNullOrEmpty(requestParams.MemberNo))
            {
                var strStoreId = ("000" + requestParams.StoreId);
                strStoreId = strStoreId.Substring(strStoreId.Length - 2, 2);
                var gangoutai = new int[] { 33, 34, 35 };
                string old = dataAdapter.GetLastMemberNo();
                var num = "";
                if (!string.IsNullOrEmpty(old))
                    num = old.Substring(old.Length - 6);

                MemberNo mn = new MemberNo(requestParams.CID, requestParams.StoreId, num);

                if (gangoutai.Contains(requestParams.ProvinceID))
                {
                    requestParams.MemberNo = string.Format("2{0}{1}{2}", requestParams.CID.ToString("0000000"), strStoreId, mn.ToString());
                }
                else
                {
                    requestParams.MemberNo = string.Format("1{0}{1}{2}", requestParams.CID.ToString("0000000"), strStoreId, mn.ToString());
                }
            }
            if (string.IsNullOrEmpty(requestParams.CardNo))
            {
                throw new PosException("绑定会员卡号不能为空！");
            }
            if (string.IsNullOrEmpty(requestParams.RealName))
            {
                throw new PosException("姓名不能为空！");
            }
            if (string.IsNullOrEmpty(requestParams.MobilePhone))
            {
                throw new PosException("手机不能为空！");
            }
            dataAdapter.AddMember(new MemberDto()
            {
                Address = requestParams.Address,
                Birthday = requestParams.Birthday,
                CurrentCityId = requestParams.CurrentCityId,
                Email = requestParams.Email,
                MemberNo = requestParams.MemberNo,
                Weixin = requestParams.Weixin,
                Zhifubao = requestParams.Zhifubao,
                MobilePhone = requestParams.MobilePhone,
                RealName = requestParams.RealName,
                Sex = requestParams.Sex,
                ProvinceID = requestParams.ProvinceID,
                CurrentCountyId = requestParams.CurrentCountyId,
                CardNo = requestParams.CardNo,
                YaJin = requestParams.YaJin
            });
            return null;
        }

        /// <summary>
        /// 活动
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("Activities")]
        [HttpPost]
        public IEnumerable<Activity> Activities([FromBody] PaysStatusRequest requestParams)
        {
            return new WarehouseManager().Activities(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
        }
        /// <summary>
        /// 设备注册
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>无返回</returns>
        [Route("RegisterDevice")]
        [HttpPost]
        public object RegisterDevice([FromBody] DeviceRequest requestParams)
        {
            MachinesSettings.RegisterDevice(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn, requestParams.Type);
            return null;
        }
        [Route("SaleOrderAdd")]
        [HttpPost]
        public OrdersDetails SaleOrderAdd([FromBody] SaleAddRequest requestParams)
        {
            var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            shoppingcart.Add(requestParams.Barcode, requestParams.Status);
            shoppingcart.RunMarketings();
            var result = new OrdersDetails()
            {
                BuyList = shoppingcart.GetBuyList(),
                Gifts = new List<GiftResult>(),
                Statistics = shoppingcart.GetSaleStatistics()
            };
            ShoppingCartFactory.ResetCache(shoppingcart, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return result;
        }
        [Route("SaleOrderEdit")]
        [HttpPost]
        public OrdersDetails SaleOrderEdit([FromBody] SaleEditRequest requestParams)
        {
            var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            shoppingcart.Edit(requestParams.Barcode, requestParams.Number, requestParams.SalePrice, requestParams.Status, requestParams.HasEditPrice, requestParams.RecordId);
            shoppingcart.RunMarketings();
            var result = new OrdersDetails()
            {
                BuyList = shoppingcart.GetBuyList(),
                Gifts = new List<GiftResult>(),
                Statistics = shoppingcart.GetSaleStatistics()
            };
            ShoppingCartFactory.ResetCache(shoppingcart, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return result;
        }
        [Route("SaleOrderEnableMarketing")]
        [HttpPost]
        public OrdersDetails SaleOrderEnableMarketing([FromBody] SaleEnableMarketingRequest requestParams)
        {
            var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            shoppingcart.EnableRangeMarketings = requestParams.EnableRangeMarketings;
            var result = new OrdersDetails()
            {
                BuyList = shoppingcart.GetBuyList(),
                Gifts = new List<GiftResult>(),
                Statistics = shoppingcart.GetSaleStatistics()
            };
            ShoppingCartFactory.ResetCache(shoppingcart, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return result;
        }
        [Route("SaleOrderRemove")]
        [HttpPost]
        public OrdersDetails SaleOrderRemove([FromBody] SaleRemoveRequest requestParams)
        {
            var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            shoppingcart.Remove(requestParams.Barcode, requestParams.Status, requestParams.HasEditPrice, requestParams.RecordId);
            shoppingcart.RunMarketings();
            var result = new OrdersDetails()
            {
                BuyList = shoppingcart.GetBuyList(),
                Gifts = new List<GiftResult>(),
                Statistics = shoppingcart.GetSaleStatistics()
            };
            ShoppingCartFactory.ResetCache(shoppingcart, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return result;
        }

        /// <summary>
        /// 取消/清空销售清单接口
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>

        [Route("ClearOrder")]
        [HttpPost]
        public OrdersDetails ClearOrder([FromBody] MachineInfo requestParams)
        {
            var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            shoppingcart.Clear();
            var result = new OrdersDetails()
            {
                BuyList = shoppingcart.GetBuyList(),
                Gifts = new List<GiftResult>(),
                Statistics = shoppingcart.GetSaleStatistics()
            };
            ShoppingCartFactory.ResetCache(shoppingcart, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return result;
        }
        /// <summary>
        /// 查库存
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("GetProductStock")]
        [HttpPost]
        public PageResult<InventoryResult> ProductStock(int pageSize, int pageIndex, InventoryRequest requestParams)
        {
            WarehouseManager warehouse = new WarehouseManager();
            var result = warehouse.CheckedInventory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn, requestParams.CategorySns, requestParams.Keyword, requestParams.Price, pageSize, pageIndex);
            return result;
        }

        /// <summary>
        /// 店长授权
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("Authorization")]
        [HttpPost]
        public object Authorization(AuthorizationRequest requestParams)
        {
            if (requestParams.Password == null)
            {
                requestParams.Password = string.Empty;
            }
            if (Salesclerk.VerfyStoreManagerOperateAuth(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Password))
            {
                return null;
            }
            throw new PosException("400", "密码错误，授权失败！");
        }

        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("SetMember")]
        [HttpPost]
        public MemberInfo SetMember(SetMemberRequest requestParams)
        {
            var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            var memnerInfo = shoppingcart.SetMember(requestParams.CardNo, requestParams.Phone, requestParams.To);
            ShoppingCartFactory.ResetCache(shoppingcart, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return memnerInfo;
        }
        [Route("SetActivityId")]
        [HttpPost]
        public object SetActivityId(SetActivityRequest requestParams)
        {
            var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            shoppingcart.SetActivityId(requestParams.ActivityId);
            ShoppingCartFactory.ResetCache(shoppingcart, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return null;
        }
        /// <summary>
        /// 门店产品分类
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("GetProductCategory")]
        [HttpPost]
        public Category GetProductCategory(CategoryRequest requestParams)
        {
            return new CategoryTree().GetCategoryTree(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, DataAdapterFactory.DEFUALT);
        }
        /// <summary>
        /// 出入款
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("PosIncomePayout")]
        [HttpPost]
        public object PosIncomePayout(PosIncomePayoutRequest requestParams)
        {
            // var salesclerk = new Salesclerk();
            Salesclerk.PosIncomePayout(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.UserCode, requestParams.Password, requestParams.Money, requestParams.Type);
            return null;
        }
        /// <summary>
        /// 挂单
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("HandBill")]
        [HttpPost]
        public OrdersDetails HandBill([FromBody] HandBillRequest requestParams)
        {
            var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            if (shoppingcart.RecordCount == 0)
            {
                throw new PosException("订单列表为空不能挂单！");
            }
            SaleSuspend.Suspend(shoppingcart);
            var result = new OrdersDetails()
            {
                BuyList = shoppingcart.GetBuyList(),
                Gifts = new List<GiftResult>(),
                Statistics = shoppingcart.GetSaleStatistics()
            };
            ShoppingCartFactory.ResetCache(shoppingcart, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return result;
        }
        /// <summary>
        /// 读单
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("ReadHandBill")]
        [HttpPost]
        public OrdersDetails ReadHandBill([FromBody] HandBillRequest requestParams)
        {
            var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            var barcodes = SaleSuspend.Read(shoppingcart, requestParams.OrderSn);
            shoppingcart.RunMarketings();
            var result = new OrdersDetails()
            {
                BuyList = shoppingcart.GetBuyList(),
                Gifts = new List<GiftResult>(),
                Statistics = shoppingcart.GetSaleStatistics()
            };
            ShoppingCartFactory.ResetCache(shoppingcart, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return result;
        }
        /// <summary>
        /// 挂单清单
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("HandBillList")]
        [HttpPost]
        public SuspendList HandBillList([FromBody] HandBillRequest requestParams)
        {
            return SuspendList.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, MachinesSettings.CachePath);
        }
        /// <summary>
        /// 获取挂单数量
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Route("HandBillNum")]
        [HttpPost]
        public int HandBillNum([FromBody] HandBillRequest requestParams)
        {
            var result = SuspendList.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, MachinesSettings.CachePath);
            if (result == null)
                return 0;
            return result.Count;
        }
        /// <summary>
        /// 撤销挂单
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("RemoveHandBill")]
        [HttpPost]
        public SuspendList RemoveHandBill([FromBody] HandBillRequest requestParams)
        {
            var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            var result = SaleSuspend.Remove(shoppingcart, requestParams.OrderSn);
            ShoppingCartFactory.ResetCache(shoppingcart, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            return result;
        }
        /// <summary>
        /// 通过日期查找历史订单
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("FindBills")]
        [HttpPost]
        public IEnumerable<BillListItem> FindBills([FromBody] FindBillsRequest requestParams)
        {
            return BillHistory.GetBills(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.QueryMachineSn, requestParams.Date, requestParams.Range, requestParams.DeviceSn, requestParams.PaySn, requestParams.Cashier);
        }

        /// <summary>
        /// 通过流水号查找历史订单
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("FindBillHistory")]
        [HttpPost]
        public BillHistoryInfo FindBillHistory([FromBody] FindBillHistoryRequest requestParams)
        {
            return BillHistory.GetBillDetails(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.PaySn, requestParams.DeviceSn);
        }
        /// <summary>
        /// 获取退换货原因列表
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("GetReason")]
        [HttpPost]
        public IEnumerable<ReasonItem> GetReason([FromBody] ReasonRequest requestParams)
        {
            IEnumerable<ReasonItem> result = null;
            switch (requestParams.Type)
            {
                case 1:
                    result = OrderChangeRefundSale.GetChangeReason(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
                    break;
                case 2:
                    result = OrderChangeRefundSale.GetRefundReason(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
                    break;
                default:
                    throw new PosException("无法获取原因列表！");
            }
            return result;
        }
        /// <summary>
        /// 添加退换货订单数据
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Route("AddRefundOrChangeInfo")]
        [HttpPost]
        public object AddRefundOrChangeInfo([FromBody]ChangeRefundRequest requestParams)
        {
            var orderChangeRefund = OrderChangeFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Mode, requestParams.DeviceSn);
            return orderChangeRefund.Add(requestParams.Barcode, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Status);
        }
        /// <summary>
        /// 编辑退换货商品信息
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Route("EditRefundOrChangeInfo")]
        [HttpPost]
        public object EditRefundOrChangeInfo([FromBody]ChangeRefundRequest requestParams)
        {
            var orderChangeRefund = OrderChangeFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Mode, requestParams.DeviceSn);
            return orderChangeRefund.Edit(requestParams.Barcode, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.ChangeNumber, requestParams.ChangePrice, requestParams.RecordId, requestParams.ProductType);
        }
        /// <summary>
        /// 移除退换货商品信息
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Route("RemoveRefundOrChangeInfo")]
        [HttpPost]
        public object RemoveRefundOrChangeInfo([FromBody]ChangeRefundRequest requestParams)
        {
            var orderChangeRefund = OrderChangeFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Mode, requestParams.DeviceSn);
            return orderChangeRefund.Remove(requestParams.Barcode, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.RecordId, requestParams.ProductType);
        }
        /// <summary>
        /// 清除退换货数据信息
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Route("ClearRefundOrChangeInfo")]
        [HttpPost]
        public object ClearRefundOrChangeInfo([FromBody]ChangeRefundRequest requestParams)
        {
            OrderChangeFactory.Disposable(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Mode, requestParams.DeviceSn);
            //var orderChangeRefund = OrderChangeFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CompanyId, requestParams.Mode);
            //orderChangeRefund.Clear();
            return null;
        }
        /// <summary>
        /// 清除退换货数据信息
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Route("SetSaleMan")]
        [HttpPost]
        public SaleManInfo SetSaleMan([FromBody]SetSaleManRequest requestParams)
        {
            switch (requestParams.Source)
            {
                case 0:
                    {
                        var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
                        var result = shoppingcart.SetSaleMan(requestParams.SaleMan);
                        ShoppingCartFactory.ResetCache(shoppingcart, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
                        return result;
                    }
                case 1:
                case 2:
                    {
                        var orderChangeRefund = OrderChangeFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Mode, requestParams.DeviceSn);
                        var result = orderChangeRefund.SetSaleMan(requestParams.SaleMan, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
                        return result;
                    }
                default:
                    throw new PosException("未能设置导购员，请重试！");
            }
        }
        /// <summary>
        /// 退换货无需支付时保存数据
        /// </summary>
        /// <returns></returns>
        [Route("NoNeedPaySave")]
        [HttpPost]
        public object NoNeedPaySave([FromBody]RefundRequest requestParams)
        {
            var orderChangeRefund = OrderChangeFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Mode, requestParams.DeviceSn);

            return orderChangeRefund.SaveRecord(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Reason, requestParams.Amount, requestParams.Amount, null, requestParams.DeviceSn);
            //return null;
        }

        /// <summary>
        /// 取当前流水号
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("GetRefundAllOrderSn")]
        [HttpPost]
        public string GetRefundAllOrderSn([FromBody] BaseApiParams requestParams)
        {
            return OrderChangeRefundSale.GetRefundAllCustomOrderSn(requestParams.StoreId, requestParams.MachineSn, requestParams.CID);
        }
        static object lockobjforadd = new object();
        private static Dictionary<string, object> lockobjs = new Dictionary<string, object>();
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>

        [Route("Pay")]
        [HttpPost]
        public object Pay([FromBody] PayRequest requestParams)
        {
            if (requestParams == null || requestParams.Payway == null || requestParams.Payway.Count() == 0)
            {
                throw new PosException("未知支付方式！");
            }
            string key = string.Format("{0}-{1}-{2}-{3}", requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
            lock (lockobjforadd)
            {
                if (!lockobjs.ContainsKey(key))
                {
                    lockobjs = lockobjs.ToList().Concat(new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>(key, new object()) }).ToDictionary(o => o.Key, o => o.Value);
                }
            }
            var dict = lockobjs;//防止并发
            var lockkv = dict.First(o => o.Key == key);
            object lockobj = new object();
            if (!lockkv.Equals(default(KeyValuePair<string, object>)) && lockkv.Value != null)
            {
                lockobj = lockkv.Value;
            }

            DateTime createDt = DateTime.Now;

            lock (lockobj)
            {

                switch (requestParams.Mode)
                {
                    case PayAction.RefundAll://退单
                        return DoPay(requestParams.CID, requestParams.StoreId, requestParams.MachineSn, requestParams.DeviceSn, Guid.NewGuid().ToString("N"), requestParams, createDt, (o) =>
                          {
                              OrderChangeRefundSale.RefundAll(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Reason, requestParams.OldOrderSn, requestParams.OrderAmount, requestParams.DeviceSn, o, createDt);
                          });
                    case PayAction.Refund://退货
                        var orderRefund = OrderChangeFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, AfterSaleMode.Refunding, requestParams.DeviceSn);
                        return DoPay(requestParams.CID, requestParams.StoreId, requestParams.MachineSn, requestParams.DeviceSn, orderRefund.PaySn, requestParams, createDt, (o) =>
                        {
                            orderRefund.SaveRecord(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Reason, requestParams.OrderAmount, requestParams.Receivable, o, requestParams.DeviceSn);
                        });
                    case PayAction.Change://换货支付
                        var orderChange = OrderChangeFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, AfterSaleMode.Changing, requestParams.DeviceSn);
                        return DoPay(requestParams.CID, requestParams.StoreId, requestParams.MachineSn, requestParams.DeviceSn, orderChange.PaySn, requestParams, createDt, (o) =>
                         {
                             orderChange.SaveRecord(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Reason, requestParams.OrderAmount, requestParams.Receivable, o, requestParams.DeviceSn);
                         });

                    case PayAction.Sale://销售支付

                        var shoppingcart = ShoppingCartFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
                        var orderList = shoppingcart.GetOrdeList();
                        if (orderList == null || orderList.Count() == 0)
                        {
                            throw new PosException("该商品已结算，遇到网络异常，请手动按 Q 清空购物车！");
                        }
                        try
                        {
                            return DoPay(requestParams.CID, requestParams.StoreId, requestParams.MachineSn, requestParams.DeviceSn, shoppingcart.OrderSN, requestParams, createDt, (o) =>
                            {
                                shoppingcart.Record(o.ApiCodes, requestParams.OrderAmount, requestParams.Receivable, requestParams.DeviceSn, createDt);
                            });
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                }
            }
            return createDt;
        }

        private object DoPay(int companyId, string storeId, string machineSn, string deviceSn, string orderSn, PayRequest requestParams, DateTime createDt, Action<IPay> callback)
        {

            PayMode payMode;
            if (requestParams.Payway.Count() > 1)
            {
                payMode = PayMode.Multiply;

            }
            else
            {
                payMode = (PayMode)requestParams.Payway.FirstOrDefault().Type;
            }
            List<PayDetails> payway = new List<PayDetails>();
            IPay pay;

            foreach (var item in requestParams.Payway)
            {
                payway.Add(new PayDetails()
                {
                    Amount = item.Amount,
                    ApiOrderSn = item.PayOrderSn,
                    PaySn = orderSn,
                    CardNo = item.CardNo,
                    WipeZero = item.WipeZero,
                    Change = item.Change,
                    Receive = item.Receive,
                    CreateDt = createDt,
                    Mode = (PayMode)item.Type
                });
            }
            pay = PaymentFactory.Factory(companyId, storeId, machineSn, deviceSn, payMode, payway, requestParams.Receivable, callback);
            pay.SavePayInfomactions();
            return createDt;
        }

        [Route("BackgroundPayment")]
        [HttpPost]
        public object BackgroundPayment([FromBody] BackgroundPaymentRequest requestParams)
        {
            var pay = PaymentFactory.Factory(
                requestParams.CID,
                requestParams.StoreId,
                requestParams.MachineSn,
                requestParams.DeviceSn,
                requestParams.Mode,
                new PayDetails()
                {
                    CardNo = requestParams.CardNo,
                    CardPassword = requestParams.CardPassword,
                    Amount = requestParams.Amount
                },
                requestParams.Amount);
            if (pay is IBackgroundPayment)
            {
                return ((IBackgroundPayment)pay).RequestPay();
            }
            else if (pay is IBackgroundPaymentWithoutWait)
            {
                return ((IBackgroundPaymentWithoutWait)pay).RequestPay();
            }
            return null;
        }

        [Route("GetBackgroundPaymentState")]
        [HttpPost]
        public ThirdPartyPaymentStatus GetBackgroundPaymentState([FromBody] BackgroundPaymentRequest requestParams)
        {
            var pay = PaymentFactory.Factory(
               requestParams.CID,
               requestParams.StoreId,
               requestParams.MachineSn,
               requestParams.DeviceSn,
               requestParams.Mode,
               new PayDetails()
               {
                   CardNo = requestParams.CardNo,
                   CardPassword = requestParams.CardPassword,
                   Amount = requestParams.Amount
               },
               requestParams.Amount);
            if (pay is IBackgroundPayment)
            {
                return ((IBackgroundPayment)pay).GetPayStatus();
            }
            return ThirdPartyPaymentStatus.Unknown;
        }


        [HttpPost]
        [Route("QCTPayCallBack")]

        public object QCTPayCallBack([FromBody]JObject obj)
        {
            var mch_id = Convert.ToInt32(obj.Property("mch_id"));//
            var store_id = obj.Property("store_id").ToString();//门店
            var out_trade_no = obj.Property("out_trade_no").ToString();//单号
            var receipt_amount = Convert.ToDecimal(obj.Property("receipt_amount"));//支付金额
            var pay_status = obj.Property("pay_status").ToString();//notpay=支付中；paysuccess=支付成功；payfail=支付失败;paycancel=已撤销;paytimeout=支付
            if (pay_status != "NOTPAY")
            {
                PayNotifyResultService.AddOne(new Logic.Entity.PayNotifyResult() { ApiCode = 25, CashFee = receipt_amount, CompanyId = mch_id, CreateDT = DateTime.Now, PaySN = out_trade_no, TradeNo = string.Empty, State = pay_status });
            }
            return new { return_code = "00000", return_msg = "" };
        }

        /// <summary>
        /// 获取所有支付方式状态及图标
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("PaysStatus")]
        [HttpPost]
        public IEnumerable<IPay> PaysStatus([FromBody] PaysStatusRequest requestParams)
        {
            return PaymentFactory.GetPaysStatus(
                requestParams.CID,
                requestParams.StoreId,
                requestParams.MachineSn,
                requestParams.DeviceSn);
        }
        /// <summary>
        /// 日结/月结(从每月1日开始计算)
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns>返回结果</returns>
        [Route("DayReport")]
        [HttpPost]
        public DayReportResult DayReport([FromBody]DayReportRequest requestParams)
        {
            DateTime date = requestParams.Date;
            DateTime endDate = requestParams.EndDate;
            if (requestParams.Mode == Mode.Month)
            {
                date = new DateTime(requestParams.Date.Year, requestParams.Date.Month, 1);
                endDate = date.AddMonths(1);
            }
            WarehouseManager manager = new WarehouseManager();
            return manager.DayMonthReport(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, date, endDate, requestParams.Range, requestParams.DeviceSn);

        }
        /// <summary>
        /// APP icon 和公司信息
        /// </summary>
        /// <returns>返回结果</returns>
        [Route("AppInfo")]
        [HttpPost]
        public object AppInfo()
        {
            var result = new
            {
                FullName = ConfigurationManager.AppSettings["FullName"],
                Name = ConfigurationManager.AppSettings["Name"],
                IconUrl = string.Format("http://{0}:{1}/{2}", Request.RequestUri.Host, Request.RequestUri.Port, ConfigurationManager.AppSettings["IconUrl"]),
                AppVersion = ConfigurationManager.AppSettings["AppVersion"]
            };
            return result;
        }
        [Route("GetStoreName")]
        [HttpPost]
        public string GetStoreName([FromBody] BaseApiParams requestParams)
        {
            try
            {
                var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
                return dataAdapter.GetStoreName();
            }
            catch (PosException)
            {
                throw;
            }
            catch
            {
                throw new PosException("无法获取门店信息");
            }
        }

        /// <summary>
        /// 退换货单设置原单号
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Route("SetOldCustomerOrderSn")]
        [HttpPost]
        public object SetOldCustomerOrderSn([FromBody]SetOldCustomerOrderSnRequset requestParams)
        {
            try
            {
                var orderChangeRefund = OrderChangeFactory.Factory(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.Mode, requestParams.DeviceSn);
                var result = orderChangeRefund.SetOldOrderSN(requestParams.OldCustomerOrderSN, requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.DeviceSn);
                return result;
            }
            catch (PosException)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取满足条件的用户信息
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Route("GetAuthUsers")]
        [HttpPost]
        public IEnumerable<UserInfo> GetAuthUsers([FromBody]UserInfoRequest requestParams)
        {
            return Salesclerk.GetAuthUsers(requestParams.StoreId, requestParams.MachineSn, requestParams.CID, requestParams.storeOperateAuth);
        }
    }

}