﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using Pharos.Logic;
using Pharos.Logic.MemberDomain.QuanChengTaoProviders;
using Pharos.CRM.Retailing.Models;
using System.Threading.Tasks;

namespace Pharos.CRM.Retailing.Controllers
{
    public class MembersController : BaseController
    {
        #region 会员资料
        public ActionResult Index()
        {
            ViewBag.stores = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Text = o.Title, Value = o.StoreId }), emptyTitle: "全部");
            ViewBag.levels = ListToSelect(MemberLevelService.GetList().Select(o => new SelectListItem() { Text = o.LevelTitle, Value = o.MemberLevelId }), emptyTitle: "全部");
            ViewBag.groups = ListToSelect(GroupingService.GetList().Select(o => new SelectListItem() { Text = o.Title, Value = o.GroupId }), emptyTitle: "请选择");
            return View();
        }
        /// <summary>
        /// 网格
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FindMemberPageList()
        {
            var count = 0;
            object footer = null;
            var list = MembersService.FindMemberPageList(Request.Params, ref count, ref footer);
            return ToDataGrid(list, count);
        }
        [HttpPost]
        public ActionResult SetValid(string id, short state)
        {
            var ids = id.Split(',').Select(o => o.ToType<int>()).ToList();
            var list = MembersService.FindList(o => ids.Contains(o.Id));
            list.Each(o => o.Status = (short)(state + 1));
            var op = MembersService.Update(list);
            return new JsonNetResult(op);
        }
        /// <summary>
        /// 会员新增
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CreateMember(int? id)
        {
            ViewBag.stores = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Text = o.Title, Value = o.StoreId }), emptyTitle: "请选择");
            var obj = new MemberDatasModel();
            SelectListItem city = null;
            if (id.HasValue)
            {
                //取会员信息
                var memberObj = MembersService.FindById(id.Value);
                ExtendHelper.ToCopyProperty<Members, MemberDatasModel>(memberObj, obj);
                if (obj.CurrentCityId > 0)
                {
                    var areaIds = new int[] { obj.CurrentProvinceId, obj.CurrentCityId, obj.CurrentCountyId };
                    var areas = BaseService<Area>.FindList(o => areaIds.Contains(o.AreaID));
                    var cy = areas.FirstOrDefault(o => o.AreaID == obj.CurrentCityId);
                    if (cy != null)
                    {
                        var text = cy.Title;
                        cy = areas.FirstOrDefault(o => o.AreaID == obj.CurrentProvinceId);
                        city = new SelectListItem();
                        city.Value = obj.CurrentCityId.ToString();
                        city.Text = text;
                        if (cy != null)
                        {
                            city.Value = cy.AreaID.ToString() + "," + city.Value;
                            city.Text = cy.Title.ToString() + "/" + city.Text;
                        }
                        cy = areas.FirstOrDefault(o => o.AreaID == obj.CurrentCountyId);
                        if (cy != null)
                        {
                            city.Value += "," + cy.AreaID.ToString();
                            city.Text += "/" + cy.Title.ToString();
                        }
                        city.Value = city.Value.TrimEnd(',');
                        city.Text = city.Text.TrimEnd('/');
                    }
                }
                //取会员卡信息
                obj.MembershipCards = _membershipCardBLL.GetMembershipCardByMember(obj.MemberId);
                obj.Coupons = new MakingCouponCardBLL().GetListCouponCardByMemberId(obj.MemberId);
                //取优惠券信息 xtodo
            }
            ViewBag.city = city;
            ViewBag.levels = ListToSelect(MemberLevelService.GetList().Select(o => new SelectListItem() { Text = o.LevelTitle, Value = o.MemberLevelId }), emptyTitle: "请选择");
            ViewBag.groups = ListToSelect(GroupingService.GetList().Select(o => new SelectListItem() { Text = o.Title, Value = o.GroupId }), emptyTitle: "请选择");
            return View(obj);
        }
        /// <summary>
        /// 会员新增
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateMember(Members objData)
        {
            var obj = objData;
            var re = new OpResult();
            string msg = "";

            var cityInfo = Request.Params["CurrentCityId_0"];
            if (!string.IsNullOrEmpty(cityInfo))
            {
                if (cityInfo.Contains(","))
                {
                    var cityInfoArry = cityInfo.Split(',');
                    obj.CurrentProvinceId = Convert.ToInt32(cityInfoArry[0]);
                    obj.CurrentCityId = Convert.ToInt32(cityInfoArry[1]);
                    if (cityInfoArry.Length == 3)
                        obj.CurrentCountyId = Convert.ToInt32(cityInfoArry[2]);
                }
                else
                {
                    obj.CurrentProvinceId = Convert.ToInt32(cityInfo);
                    obj.CurrentCityId = 0;
                }
            }

            if (!MembersService.CheckMsg(obj, ref msg))
                re.Message = msg;
            else if (obj.Id == 0)
            {
                obj.MemberId = CommonRules.GUID;
                obj.MemberNo = CommonRules.MemberNum(obj.StoreId);
                obj.CreateDT = DateTime.Now;
                obj.CreateUID = Sys.CurrentUser.UID;
                obj.CompanyId = CommonService.CompanyId;


                re = MembersService.Add(obj);
            }
            else
            {
                var member = MembersService.FindById(obj.Id);
                obj.ToCopyProperty(member, new List<string>() { "CompanyId", "CreateDT", "CreateUID", "MemberId", "MemberNum" });
                re = MembersService.Update(member);
            }
            #region 操作日志
            var _msg = Pharos.Sys.LogEngine.CompareModelToLog<Members>(Sys.LogModule.会员管理, obj);
            new Pharos.Sys.LogEngine().WriteInsert(_msg, Sys.LogModule.会员管理);
            #endregion
            if (re.Successed)
            {
                Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = CommonService.CompanyId, StoreId = obj.StoreId, Target = "Member" });
            }
            return Content(re.ToJson());
        }
        /// <summary>
        /// 新增会员分组
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMemberGroup()
        {
            return View();
        }
        /// <summary>
        /// 新增会员分组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateMemberGroup(string name)
        {
            var re = new OpResult();
            if (!GroupingService.IsExist(o => o.Title == name && o.CompanyId == CommonService.CompanyId))
            {
                var model = new Grouping()
                {
                    Channel = 1,
                    Title = name,
                    GroupId = CommonRules.GUID,
                    State = 1,
                    CompanyId = CommonService.CompanyId
                };
                re = GroupingService.Add(model);
                if (re.Successed)
                {
                    #region 操作日志
                    var _msg = Pharos.Sys.LogEngine.CompareModelToLog<Grouping>(Sys.LogModule.会员分组, model);
                    new Pharos.Sys.LogEngine().WriteInsert(_msg, Sys.LogModule.会员管理);
                    #endregion
                }
            }
            else
                re.Message = "该分组名称已存在";
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult MemberGroupListPartial()
        {
            var list = GroupingService.GetList().Select(o => o.Title);
            return PartialView(list);
        }
        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteMemberGroup(string id)
        {
            var obj = GroupingService.Find(o => o.Title == id && o.CompanyId == CommonService.CompanyId);
            var re = new OpResult();
            if (MembersService.IsExist(o => o.MemberGroupId == obj.GroupId))
                re.Message = "存在会员关联不允许删除!";
            else
                re = GroupingService.Delete(obj);
            return new JsonNetResult(re);
        }
        [HttpPost]
        public ActionResult MoveType(string ids, string groupId)
        {
            var sids = ids.Split(',').Select(o => int.Parse(o));
            var list = MembersService.FindList(o => sids.Contains(o.Id));
            list.Each(o => { o.MemberGroupId = groupId; });
            return new JsonNetResult(MembersService.Update(list));
        }
        [HttpPost]
        public ActionResult GetMemberGroupList()
        {
            var list = GroupingService.GetList();
            var json = list.Select(o => new DropdownItem(o.GroupId, o.Title)).ToList();
            json.Insert(0, new DropdownItem("", "将所选项移到指定分组"));
            return Content(json.ToJson());
        }
        #endregion

        #region 卡反结算
        public ActionResult MemberCardPayBackBalance()
        {
            return View();
        } 
        #endregion

        #region 会员充值
        MemberRechargeBLL _memberRechargeBll = new MemberRechargeBLL();
        public ActionResult RechargeIndex()
        {
            return View();
        }
        /// <summary>
        /// get datagrid datas by where 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="keyWold"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FindRechargePageList(DateTime? beginDate, DateTime? endDate, string keyWold, int keywoldType, int selectType)
        {
            int count = 0;
            object footer = null;
            var result = _memberRechargeBll.FindRechargePageList(beginDate, endDate, keyWold.Replace(" ", ""), keywoldType,selectType, out count, ref footer);
            return ToDataGrid(result, count, footer);
        }
        [HttpGet]
        public ActionResult CreateRecharge()
        {
            return View();
        }
        /// <summary>
        /// add a new memberrecharge entity
        /// </summary>
        /// <param name="_memberRecharge"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateRecharge(MemberRecharge _memberRecharge)
        {
            Task.Factory.StartNew(() =>
            {
                var _msg = Pharos.Sys.LogEngine.CompareModelToLog<MemberRecharge>(Sys.LogModule.会员充值, _memberRecharge);
                new Pharos.Sys.LogEngine().WriteInsert(_msg, Sys.LogModule.会员充值);
            });
            _memberRecharge.CardId = _memberRecharge.CardId.Replace(" ", "");
            var result = _memberRechargeBll.CreateRecharge(_memberRecharge);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 获取充值的卡信息
        /// </summary>
        /// <param name="cardSN"></param>
        /// <returns></returns>
        public ActionResult GetRechargeCardByCardSn(string cardSN)
        {
            var result = _membershipCardBLL.GetRechargeCardByCardSn(cardSN);
            return Content(result.ToJson());
        }
        #endregion
        #region 会员卡
        private readonly MembershipCardBLL _membershipCardBLL = new MembershipCardBLL();

        public ActionResult MemberCardIndex()
        {
            var cardtype = _cardInfoBLL.GetAllMemberCardType().Where(o => !o.CardType.Contains("2")).Select(o => new SelectListItem() { Text = o.CardName, Value = o.CardTypeId });
            ViewBag.CardType = ListToSelect(cardtype, emptyTitle: "全部");
            ViewBag.cardTypeToDropDown = cardtype.ToJson();
            return View();
        }
        /// <summary>
        /// 会员卡列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="state"></param>
        /// <param name="beOverdue"></param>
        /// <param name="dueTimeStart"></param>
        /// <param name="dueTimeEnd"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FindMemberCardPageList(string type, short? state, int beOverdue, DateTime? dueTimeStart, DateTime? dueTimeEnd, string keyWord)
        {
            var count = 0;
            var types = new int[] { 1, 3 };
            var result = _membershipCardBLL.FindMembershipCardPageList(type, state, beOverdue, dueTimeStart, dueTimeEnd, keyWord.Replace(" ", ""), types, out count);
            return ToDataGrid(result, count);
        }
        /// <summary>
        /// 发卡窗体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DispatchMemberCard(int? id)
        {
            MembershipCard _memberCard = new MembershipCard();
            if (id != null && id != 0)
            {
                _memberCard = _membershipCardBLL.FindMembershipCardById((int)id);
            }
            return View(_memberCard);
        }
        /// <summary>
        /// 发卡，绑定卡与人
        /// </summary>
        /// <param name="_membershipCard"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DispatchMemberCard(MembershipCard _membershipCard)
        {
            _membershipCard.CardSN = _membershipCard.CardSN.Replace(" ", "");
            if (string.IsNullOrEmpty(_membershipCard.CardSN))
            {
                return Content(OpResult.Fail("卡号信息异常，请刷新重试！").ToJson());
            }
            if (string.IsNullOrEmpty(_membershipCard.MemberId) && _membershipCard.MemberId.Length != 32)//存的不是32guid视为异常
            {
                return Content(OpResult.Fail("会员信息异常，请刷新重试！").ToJson());
            }
            var result = _membershipCardBLL.CreateOrUpdateMembershipCard(_membershipCard);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 会员卡更改卡状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult UpdateCardState(short state, string ids)
        {
            var result = _membershipCardBLL.UpdateMemberCardState(state, ids);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 更改卡类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult UpdateCardType(string type, string ids)
        {
            var result = _membershipCardBLL.UpdateCardType(type, ids);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 会员查找
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult GetMembersBySearch(string param)
        {
            var result = MembersService.GetMembersBySearch(param);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 根据卡号查找对应会员
        /// </summary>
        /// <param name="cardSN"></param>
        /// <returns></returns>
        public ActionResult GetMembersCardByCardSn(string cardSN)
        {
            var result = _membershipCardBLL.GetMembersCardByCardSn(cardSN.Replace(" ", ""));
            return Content(result.ToJson());
        }
        /// <summary>
        /// 模糊查找会员卡信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult GetMemberCardByContainsParam(string param)
        {
            var result = _membershipCardBLL.GetMemberCardByContainsParam(param);
            return Content(result.ToJson());
        }
        public ActionResult ImportCard(string cardType)
        {
            if (string.IsNullOrEmpty(cardType))
            {
                cardType = "1,2,3";
            }
            var obj = BaseService<ImportSet>.Find(o => o.CompanyId == CommonService.CompanyId && o.TableName == "MembershipCard");
            var cards = _cardInfoBLL.GetAllMemberCardType();
            var card = cards.Where(o => cardType.Contains(o.CardType));
            //var select = card == null ? null : card.CardTypeId;
            ViewBag.cardType = ListToSelect(card.Select(o => new SelectListItem() { Text = o.CardName, Value = o.CardTypeId }), emptyTitle: "请选择");
            ViewBag.stores = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Text = o.Title, Value = o.StoreId }), emptyTitle: "请选择");
            return View(obj ?? new ImportSet());
        }
        /// <summary>
        /// 会员卡导入
        /// </summary>
        /// <param name="imp"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportCard(ImportSet imp)
        {
            imp.TableName = "MembershipCard";
            imp.CompanyId = CommonService.CompanyId;
            var op = MembershipCardService.Import(imp, Request.Files, Request["FieldName"], Request["ColumnName"]);
            return Content(op.ToJson());
        }

        #endregion
        #region 制作会员卡
        MakingMembershipCardBLL _makeMembershipCardBLL = new MakingMembershipCardBLL();
        public ActionResult CreateCardIndex()
        {
            //类型
            ViewBag.cardType = ListToSelect(_cardInfoBLL.GetAllMemberCardType().Select(o => new SelectListItem() { Text = o.CardName, Value = o.CardTypeId }), emptyTitle: "全部");
            //批次
            ViewBag.batchs = ListToSelect(_makeMembershipCardBLL.GetBatchsToDropDown().Select(o => new SelectListItem() { Text = o.BatchSN, Value = o.BatchSN }), emptyTitle: "全部");
            //创建人
            ViewBag.users = ListToSelect(_makeMembershipCardBLL.GetCreateUserToDropDown().Select(o => new SelectListItem() { Text = o.FullName, Value = o.UID }), emptyTitle: "全部");
            return View();
        }
        /// <summary>
        /// 网格
        /// </summary>
        /// <param name="cardType"></param>
        /// <param name="batchSN"></param>
        /// <param name="state"></param>
        /// <param name="createUID"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FindCreateCardPageList(string cardType, string batchSN, short? state, string createUID, DateTime? beginDate, DateTime? endDate)
        {
            int count = 0;
            var result = _makeMembershipCardBLL.GetMakingMembershipCardByWhere(cardType, batchSN, state, createUID, beginDate, endDate, out count);
            return ToDataGrid(result, count);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateMemberCard(int? id)
        {
            MakingMembershipCard _shipCard = new MakingMembershipCard();

            if (id != null)
            {
                _shipCard = _makeMembershipCardBLL.GetShipCardById((int)id);
            }
            ViewBag.CardType = ListToSelect(_cardInfoBLL.GetAllMemberCardType().Where(o => o.State == 0).Select(o => new SelectListItem() { Text = o.CardName + "(" + (o.Category == 1 ? "电子卡" : o.Category == 2 ? "磁卡" : "IC卡") + ")", Value = o.CardTypeId }), emptyTitle: "请选择", emptyValue: "-1");
            //ViewBag.CardType = ListToSelect(_cardInfoBLL.GetAllMemberCardType().Select(o => new SelectListItem() { Text = o.CardName, Value = o.Id.ToString() }));
            return View(_shipCard);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="_shipCard"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateMemberCard(MakingMembershipCard _shipCard)
        {
            var result = _makeMembershipCardBLL.CreateMakingMembershipCard(_shipCard);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 更改批次状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult UpdateMemberCardState(short state, string ids)
        {
            var result = _makeMembershipCardBLL.UpdateState(state, ids);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 会员卡批次--生成会员卡
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult GenerateMemberCard(string ids)
        {
            var result = _makeMembershipCardBLL.GenerateMemberCard(ids);
            return Content(result.ToJson());
        }

        /// <summary>
        /// 批次导出会员卡信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult ExportMemberCard(string ids)
        {
            var result = _makeMembershipCardBLL.ExportMemberCard(ids);
            if (result.Successed)
            {
                return new EmptyResult();
            }
            else
            {
                return RedirectAlert("CreateCardIndex", result.Message, "Members");
            }
        }

        #endregion
        #region 领用优惠券

        private readonly MakingCouponCardBLL _couponCardBLL = new MakingCouponCardBLL();

        #region 领用优惠券列表
        public ActionResult TakeCouponIndex()
        {
            ViewBag.types = ListToSelect(SysDataDictService.GetDictionaryList(Logic.DicType.优惠券类别).Where(o => o.Status == true).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.forms = ListToSelect(SysDataDictService.GetDictionaryList(Logic.DicType.优惠券形式).Where(o => o.Status == true).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部");
            //ViewBag.users = ListToSelect(UserInfoService.GetList().Select(o => new SelectListItem() { Value = o.UID, Text = o.FullName }), emptyTitle: "全部");//, Selected = o.UID == Logic.CurrentUser.UID
            ViewBag.users = ListToSelect(_couponCardBLL.GetDetailCreatorList().Select(o => new SelectListItem() { Text = o.FullName, Value = o.UID }), emptyTitle: "全部");
            return View();
        }

        [HttpPost]
        public ActionResult FindTakeCouponPageList(int page = 1, int rows = 20)
        {
            int couponType = -1;
            int couponFrom = -1;
            short state = -1;
            string storeIds = "-1";
            string recipients = "-1";
            string expiryStart = "-1";
            string expiryEnd = "-1";

            if (!Request["couponType"].IsNullOrEmpty())
            {
                couponType = int.Parse(Request["couponType"].ToString().Trim());
            }
            if (!Request["couponFrom"].IsNullOrEmpty())
            {
                couponFrom = int.Parse(Request["couponFrom"].ToString().Trim());
            }
            if (!Request["state"].IsNullOrEmpty())
            {
                state = Convert.ToInt16(Request["state"].ToString().Trim());
            }
            if (!Request["storeIds"].IsNullOrEmpty())
            {
                storeIds = Request["storeIds"].ToString().Trim();
            }
            if (!Request["recipients"].IsNullOrEmpty())
            {
                recipients = Request["recipients"].ToString().Trim();
            }
            if (!Request["expiryStart"].IsNullOrEmpty())//有效期
            {
                expiryStart = Request["expiryStart"].ToString().Trim();
            }
            if (!Request["expiryEnd"].IsNullOrEmpty())
            {
                expiryEnd = Request["expiryEnd"].ToString().Trim();
            }

            var paging = new Paging() { PageIndex = page, PageSize = rows };
            var result = _couponCardBLL.FindTakeCouponPageList(paging, couponType, couponFrom, state, storeIds, recipients, expiryStart, expiryEnd);
            return ToDataGrid(result, paging.RecordTotal);
        }
        #endregion

        #region 派发申领
        public ActionResult TakeCoupon()
        {
            ViewBag.batchSN = ListToSelect(_couponCardBLL.FindNotReceivedCompletedList().Select(o => new SelectListItem() { Value = o.BatchSN, Text = o.BatchSN }), emptyTitle: "请选择");
            ViewBag.recipientTitle = Sys.CurrentUser.FullName;
            return View();
        }

        [HttpPost]
        public ActionResult TakeCoupon(CouponCardDetail model)
        {
            string batchSN = model.BatchSN;
            string ticketStart = model.TicketNo;
            int takeNum = int.Parse(Request["takeNum"].ToString().Trim());
            var result = _couponCardBLL.SaveTakeCoupon(batchSN, takeNum, ticketStart);
            return Content(result.ToJson());
        }

        /// <summary>
        /// 根据批次号获取未派发的优惠券总数
        /// </summary>
        /// <param name="batchSN">批次号</param>
        /// <returns>未派发的优惠券总数</returns>
        public ActionResult GetCouponDetailCountByBatch(string batchSN)
        {
            OpResult result = new OpResult();
            var batchList = _couponCardBLL.FindNotReceivedCompletedList().Select(o => o.BatchSN);
            int count = 0;
            bool hasBatch = false;
            foreach (var item in batchList)
            {
                if (item == batchSN)
                {
                    var ticketList = _couponCardBLL.FindCouponDetailListByBatch(item).OrderBy(o => o.TicketNo).Where(o => o.State == 0).Select(o => o.TicketNo);
                    count = ticketList.Count();
                    hasBatch = true;
                    break;
                }
            }
            if (hasBatch)
            {
                result.Successed = true;
                result.Message = count.ToString();
            }
            else
            {
                result.Successed = false;
                result.Message = "该批次不存在";
            }
            return Content(result.ToJson());
        }

        /// <summary>
        /// 根据批次号与领取数量获取券号范围
        /// </summary>
        /// <param name="batchSN">批次号</param>
        /// <param name="count">领取数量</param>
        /// <returns>券号范围</returns>
        public ActionResult GetTicketNos(string batchSN, int takeCount)
        {
            OpResult result = new OpResult();
            var batchList = _couponCardBLL.FindNotReceivedCompletedList().Select(o => o.BatchSN);
            string ticketStart = "", ticketEnd = "";
            int count = 0;
            bool hasBatch = false;
            foreach (var item in batchList)
            {
                if (item == batchSN)
                {
                    var ticketList = _couponCardBLL.FindCouponDetailListByBatch(item).OrderBy(o => o.TicketNo).Where(o => o.State == 0).Select(o => o.TicketNo);
                    count = ticketList.Count();
                    if (count > 0)
                    {
                        ticketStart = ticketList.FirstOrDefault();
                        ticketEnd = ticketList.Skip(takeCount - 1).Take(1).SingleOrDefault();
                    }
                    hasBatch = true;
                    break;
                }
            }
            if (hasBatch)
            {
                result.Successed = true;
                result.Message = string.Format("{0};{1};", ticketStart, ticketEnd);
            }
            else
            {
                result.Successed = false;
                result.Message = "该批次不存在";
            }
            return Content(result.ToJson());
        }
        #endregion

        #endregion
        #region 制作优惠券

        //private readonly MakingCouponCardBLL _couponCardBLL = new MakingCouponCardBLL();

        #region 制作优惠券列表
        public ActionResult CreateCouponIndex()
        {
            ViewBag.types = ListToSelect(SysDataDictService.GetDictionaryList(Logic.DicType.优惠券类别).Where(o => o.Status == true).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.forms = ListToSelect(SysDataDictService.GetDictionaryList(Logic.DicType.优惠券形式).Where(o => o.Status == true).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部");
            ViewBag.users = ListToSelect(_couponCardBLL.GetCreatorList().Select(o => new SelectListItem() { Text = o.FullName, Value = o.UID }), emptyTitle: "全部");
            //ViewBag.users = ListToSelect(UserInfoService.GetList().Select(o => new SelectListItem() { Value = o.UID, Text = o.FullName }), emptyTitle: "全部");//, Selected = o.UID == Logic.CurrentUser.UID
            return View();
        }

        [HttpPost]
        public ActionResult FindCreateCouponPageList(int page = 1, int rows = 20)
        {
            int couponType = -1;
            int couponFrom = -1;
            short state = -1;
            string storeIds = "-1";
            string expiryStart = "-1";
            string expiryEnd = "-1";
            string receiveStart = "-1";
            string receiveEnd = "-1";
            string createUID = "-1";

            if (Request["couponType"] != "")
            {
                couponType = int.Parse(Request["couponType"].ToString().Trim());
            }
            if (Request["couponFrom"] != "")
            {
                couponFrom = int.Parse(Request["couponFrom"].ToString().Trim());
            }
            if (Request["state"] != "-1")
            {
                state = Convert.ToInt16(Request["state"].ToString().Trim());
            }
            if (Request["storeIds"] != "")
            {
                storeIds = Request["storeIds"].ToString().Trim();
            }
            if (Request["expiryStart"] != "")//有效期
            {
                expiryStart = Request["expiryStart"].ToString().Trim();
            }
            if (Request["expiryEnd"] != "")
            {
                expiryEnd = Request["expiryEnd"].ToString().Trim();
            }
            if (Request["receiveStart"] != "")//领取期限
            {
                receiveStart = Request["receiveStart"].ToString().Trim();
            }
            if (Request["receiveEnd"] != "")
            {
                receiveEnd = Request["receiveEnd"].ToString().Trim();
            }
            if (Request["createUID"] != "")
            {
                createUID = Request["createUID"].ToString().Trim();
            }
            var paging = new Paging() { PageIndex = page, PageSize = rows };
            var result = _couponCardBLL.FindCreateCouponPageList(paging, couponType, couponFrom, state, storeIds, expiryStart, expiryEnd, receiveStart, receiveEnd, createUID);
            return ToDataGrid(result, paging.RecordTotal);
        }
        #endregion

        #region 新增或修改
        /// <summary>
        /// 获取当前ConpanyId下当天制作的优惠券批次总数，如超过99批，则不可再新增
        /// </summary>
        /// <returns>当前ConpanyId下当天制作的优惠券批次总数</returns>
        public ActionResult GetBatchCount()
        {
            var result = _couponCardBLL.GetBatchCount();
            return Content(result.ToJson());
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="Id">优惠券Id</param>
        /// <returns>页面</returns>
        public ActionResult CreateCoupon(string Id)
        {
            ViewBag.types = ListToSelect(SysDataDictService.GetDictionaryList(Logic.DicType.优惠券类别).Where(o => o.Status == true).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            ViewBag.forms = ListToSelect(SysDataDictService.GetDictionaryList(Logic.DicType.优惠券形式).Where(o => o.Status == true).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部", emptyValue: "-1");
            var parents = ProductCategoryService.GetParentTypes().Select(o => new DropdownItem() { Value = o.CategorySN.ToString(), Text = o.Title }).ToList();//品类
            ViewBag.parenttypes = parents.ToJson();
            parents = ProductBrandService.GetList().Select(o => new DropdownItem(o.BrandSN.ToString(), o.Title)).ToList();
            ViewBag.brands = parents.ToJson();
            var obj = new MakingCouponCard();
            if (!Id.IsNullOrEmpty())
            {
                obj = _couponCardBLL.FindModelById(int.Parse(Id));
            }
            return View(obj.IsNullThrow());
        }

        /// <summary>
        /// 保存新增或修改
        /// </summary>
        /// <param name="model">优惠券对象</param>
        /// <returns>保存结果</returns>
        [HttpPost]
        public ActionResult CreateCoupon(MakingCouponCard model)
        {
            var storeIds = Request["StoreIds"];
            if (storeIds.Contains("-1"))
            {
                storeIds = "-1,";
            }
            model.StoreIds = storeIds;
            var result = _couponCardBLL.SaveOrUpdate(model);
            return Content(result.ToJson());
        }

        /// <summary>
        /// 输入自动完成品牌
        /// </summary>
        /// <param name="searchName">关键字</param>
        /// <returns>品牌列表</returns>
        [HttpPost]
        public ActionResult GetBrandInput(string searchName)
        {
            if (searchName.IsNullOrEmpty())
            {
                return new EmptyResult();
            }
            else
            {
                int count = 0;
                var list = new ProductBrandBLL().GetBrandList(searchName, null, out count);
                return ToDataGrid(list, 20);
            }
        }

        /// <summary>
        /// 品类/具体商品/品牌列表
        /// </summary>
        /// <param name="Id">优惠券Id</param>
        /// <param name="type">适用商品：品类/具体商品/品牌</param>
        /// <returns>品类/具体商品/品牌列表</returns>
        [HttpPost]
        public ActionResult LoadTypeList(string Id, int type)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(Id))
            {
                var list = _couponCardBLL.LoadTypeList(int.Parse(Id), type, out count);
                return ToDataGrid(list, count);
            }
            else
            {
                return ToDataGrid(null, 0);
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
        public ActionResult SetCouponState(string ids, short state = 5)
        {
            var result = _couponCardBLL.SetCouponState(ids, state);
            return Content(result.ToJson());
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
        public ActionResult GenerateCoupon(string id, string batchSN, int num)
        {
            var result = _couponCardBLL.GenerateCoupon(id, batchSN, num);
            return Content(result.ToJson());
        }
        #endregion

        #endregion
        #region 充值赠送
        private readonly RechargeGiftsBLL _rechargeGiftBll = new RechargeGiftsBLL();
        /// <summary>
        /// index
        /// </summary>
        /// <returns></returns>
        public ActionResult RechargeGiftIndex()
        {
            ViewBag.users = ListToSelect(_rechargeGiftBll.GetCreateUserToDropDown().Select(o => new SelectListItem() { Text = o.FullName, Value = o.UID }), emptyTitle: "全部");
            return View();
        }
        /// <summary>
        /// find rechangegift page list by where 
        /// </summary>
        /// <returns></returns>
        public ActionResult FindRechargeGiftPageList(int? state, string createUID)
        {
            int count = 0;
            if (state == null) { state = -1; }
            var result = _rechargeGiftBll.FindRechargeGiftPageList((int)state, createUID, out count);
            return ToDataGrid(result, count);
        }
        /// <summary>
        /// add or update
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateRechargeGift(int? id)
        {
            RechargeGifts _rechargeGift = new RechargeGifts();
            if (id != null && id != 0)
            {
                _rechargeGift = _rechargeGiftBll.GetRechargeGiftsById((int)id);
            }
            return View(_rechargeGift);
        }
        /// <summary>
        /// add or update a  rechargegift rule
        /// </summary>
        /// <param name="_rechargeGift"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateRechargeGift(RechargeGifts _rechargeGift)
        {
            var result = _rechargeGiftBll.CreateRechargeGift(_rechargeGift);
            return Content(result.ToJson());
        }
        /// <summary>
        /// update rechargegift rule state
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult UpdateRechargeGiftState(int state, string ids)
        {
            var result = _rechargeGiftBll.UpdateRechargeGiftState(state, ids);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 根据充值金额取符合条件的充值赠送信息
        /// </summary>
        /// <param name="rechargeMoney"></param>
        /// <returns></returns>
        public ActionResult GetRechargeGiftInfo(decimal rechargeMoney)
        {
            var result = _rechargeGiftBll.GetRechargeGiftInfo(rechargeMoney);
            return Content(result.ToJson());
        }
        #endregion
        #region 生日提醒
        public ActionResult BirthdayRemindIndex()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindBirthdayRemindPageList()
        {
            return ToDataGrid(null, 0);
        }
        public ActionResult CreateBirthdayRemind()
        {
            return View();
        }
        #endregion
        #region 会员等级
        MemberLevelBLL _memberLevelBll = new MemberLevelBLL();
        public ActionResult MemberGradeIndex()
        {
            return View();
        }
        /// <summary>
        /// 网格
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FindMemberGradePageList()
        {
            int count = 0;
            var result = _memberLevelBll.FindMemberLevelByCompanyId(out count);
            return ToDataGrid(result, count);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateMemberGrade(int? id)
        {
            MemberLevel _level = new MemberLevel();
            if (id.HasValue)
            {
                _level = _memberLevelBll.GetMemberLevelById((int)id);
            }
            return View(_level);
        }
        /// <summary>
        /// 新建会员等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateMemberGrade(MemberLevel level)
        {
            var result = _memberLevelBll.CreateMemberLevel(level);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 会员等级状态更新
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult UpdateMemberLevelState(short state, string ids)
        {
            var result = _memberLevelBll.UpdateMemberStateByIds(state, ids);
            return Content(result.ToJson());
        }
        #endregion
        #region 积分规则
        private readonly IntegralRuleBLL _integralRuelBLL = new IntegralRuleBLL();
        private readonly SysDataDictionaryBLL _dicBLL = new SysDataDictionaryBLL();

        public ActionResult IntegralRuleIndex()
        {
            return View();
        }
        public ActionResult FindIntegralRulePageList(short ruleTypeId, string value)
        {
            int count = 0;
            var result = _integralRuelBLL.FindIntegralRuleList(ruleTypeId, value, out count);
            return ToDataGrid(result, count);
        }
        [HttpGet]
        public ActionResult CreateIntegralRule(int? id = null)
        {
            IntegralRule _rule = new IntegralRule();
            if (id != null)
            {
                _rule = _integralRuelBLL.GetIntegralRuleById((int)id);
            }
            //客户等级
            ViewBag.MemberLevels = ListToSelect(_memberLevelBll.GetAllMemberLevelInfo().Select(o => new SelectListItem() { Text = o.LevelTitle, Value = o.MemberLevelId }));
            //付款方式
            ViewBag.ApiTypes = ListToSelect(ApiLibraryService.GetAllApiLibrary().Select(o => new SelectListItem() { Text = o.Title, Value = o.ApiCode.ToString() }));
            //ViewBag.ApiTypes = ListToSelect(_dicBLL.GetDicListByPSN(10).Select(o => new SelectListItem() { Text = o.Title, Value = o.DicSN.ToString() }));
            //品类
            //ViewBag.parenttypes = ListToSelect(ProductCategoryService.GetParentTypes().Select(o => new SelectListItem() { Value = o.CategorySN.ToString(), Text = o.Title }), emptyTitle: "全部");

            var parents = ProductCategoryService.GetParentTypes().Select(o => new DropdownItem() { Value = o.CategorySN.ToString(), Text = o.Title }).ToList();
            //parents.Insert(0, new DropdownItem("", "请选择"));
            ViewBag.parenttypes = parents.ToJson();
            parents = ProductBrandService.GetList().Select(o => new DropdownItem(o.BrandSN.ToString(), o.Title)).ToList();
            //parents.Insert(0, new DropdownItem("", "请选择"));
            ViewBag.brands = parents.ToJson();

            return View(_rule);
        }
        [HttpPost]
        public ActionResult CreateIntegralRule(IntegralRule rule)
        {
            var result = _integralRuelBLL.CreateIntegralRule(rule);
            return Content(result.ToJson());
        }
        public ActionResult UpdateIntegralRuleState(string ids, short state = 1)
        {
            var result = _integralRuelBLL.UpdateIntegralRule(ids, state);
            return Content(result.ToJson());
        }
        /// <summary>
        /// 取该会员等级下所有可用规则
        /// </summary>
        /// <param name="memberLevel"></param>
        /// <returns></returns>
        public ActionResult GetActivingIntergralRuleByMemberLevel(string memberLevel)
        {
            var result = _integralRuelBLL.GetActivingIntergralRuleByMemberLevel(memberLevel);
            return Content(result.Count().ToJson());
        }

        /// <summary>
        /// 按指定品类时取品类明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetAllTypeByRule3(int id)
        {
            List<ProductCategory> result = new List<ProductCategory>();
            int count = 0;
            if (id != 0)
            {
                //Id StrId CategorySN CategoryGrade BrandSN BigCategoryTitle MidCategoryTitle SubCategoryTitle BrandTitle StockNums 
                var currentRule = _integralRuelBLL.FindIntegralRuleById(id);
                //查对应品类
                if (currentRule.RuleTypeId == 3)
                {
                    List<string> currentValue = new List<string>();
                    if (currentRule.RuleTypeValue.Trim(',').Contains(","))
                    {
                        currentValue = currentRule.RuleTypeValue.Trim(',').Split(',').ToList();
                    }
                    else
                    {
                        currentValue.Add(currentRule.RuleTypeValue);
                    }
                    foreach (var item in currentValue)
                    {
                        ProductCategory _category = new ProductCategory();
                        var category = new ProductCategoryService().GetProductCategoryByCategorySN(Convert.ToInt32(item));

                        while (category.CategoryPSN != 0)
                        {
                            switch (category.Grade)
                            {
                                case 4:
                                    break;
                                case 3:
                                    _category.SubCategory = category.CategorySN;
                                    _category.SubCategoryTitle = category.Title;
                                    break;
                                case 2:
                                    _category.MidCategory = category.CategorySN;
                                    _category.MidCategoryTitle = category.Title;
                                    break;
                                case 1:
                                    _category.CategorySN = category.CategorySN;
                                    _category.BigCategoryTitle = category.Title;
                                    break;
                            }

                            category = new ProductCategoryService().GetProductCategoryByCategorySN(Convert.ToInt32(category.CategoryPSN));

                        }
                        if (category.CategoryPSN == 0)
                        {
                            _category.CategorySN = category.CategorySN;
                            _category.BigCategoryTitle = category.Title;
                        }
                        result.Add(_category);
                        count++;
                    }
                }

            }
            return ToDataGrid(result, count);
        }
        public class ProductCategory
        {
            public int Id { get; set; }
            public int CategorySN { get; set; }
            public int CategoryGrade { get; set; }
            public string BigCategoryTitle { get; set; }
            public int MidCategory { get; set; }
            public string MidCategoryTitle { get; set; }
            public int SubCategory { get; set; }
            public string SubCategoryTitle { get; set; }
            public int BandSN { get; set; }
            public string BandTitle { get; set; }
            public string StockNums { get; set; }

        }
        #endregion
        #region 卡片类型
        CardInfoBLL _cardInfoBLL = new CardInfoBLL();
        public ActionResult MemberCardTypeIndex()
        {
            return View();
        }
        /// <summary>
        /// 查询现有卡类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FindMemberCardTypeList()
        {
            int count = 0;
            var cardinfos = _cardInfoBLL.FindMemberCardTypeList(out count);
            return ToDataGrid(cardinfos, count);
        }
        [HttpGet]
        public ActionResult CreateMemberCardType(int? id)
        {
            CardInfo _info = new CardInfo();
            if (id != null && id != 0)
            {
                _info = CardInfoService.FindById(id);
            }
            return View(_info);
        }
        /// <summary>
        /// 新建卡类型
        /// </summary>
        /// <param name="cardinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateMemberCardType(CardInfo cardinfo)
        {
            if (cardinfo.CardType.Contains("2"))
            {
                if (cardinfo.DefaultPrice <= 0)
                {
                    return Content(OpResult.Fail("购物卡请设置初始金额！").ToJson());
                }
            }
            var result = _cardInfoBLL.CreateMemberCardType(cardinfo);
            if (result.Successed)
            {
                return Content(result.ToJson());
            }
            else
            {
                return Content(result.Message);
            }
        }
        /// <summary>
        /// 更改状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateCardInfoState(short state, string ids)
        {
            var result = _cardInfoBLL.UpdateCardState(state, ids);
            return Content(result.ToJson());
        }
        #endregion
        #region 返赠方案
        ReturnRulesBLL _returnRuleBll = new ReturnRulesBLL();
        /// <summary>
        /// 返赠方案首页
        /// </summary>
        /// <returns></returns>
        public ActionResult ReturnRulesIndex()
        {
            return View();
        }
        /// <summary>
        /// 返赠新建
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateReturnRules(int? id)
        {
            ReturnRules rules = new ReturnRules();
            if (id > 0)
            {
                rules = _returnRuleBll.FindReturnRuleById((int)id);
            }
            //ViewBag.Adapters
            //ViewBag.Adapters = ListToSelect(_dicBLL.GetDicListByPSN(204).Select(o => new SelectListItem() { Text = o.Title, Value = o.DicSN.ToString() }), emptyTitle: "全部");
            var adapters = QuanChengTaoIntegralRuleService.GetRuleProvidersIdAndTitle().Select(o => new SelectListItem() { Text = o.Value, Value = o.Key.ToString() });
            //从枚举里取对应的适配器
            ViewBag.Adapters = ListToSelect(adapters, emptyTitle: "请选择", emptyValue: "-1");
            List<AdapterModes> modes = new List<AdapterModes>();
            //取每个适配器对应的mode
            if (ViewBag.Adapters != null)
            {
                foreach (var item in adapters)
                {

                    var titles = QuanChengTaoIntegralRuleService.GetMeteringModesTitleAndValue(Convert.ToInt32(item.Value));
                    AdapterModes mode = new AdapterModes()
                    {
                        Adapter = item.Value,
                        Modes = titles
                    };

                    modes.Add(mode);
                }
            }
            //客户等级
            ViewBag.MemberLevels = ListToSelect(_memberLevelBll.GetAllMemberLevelInfo().Select(o => new SelectListItem() { Text = o.LevelTitle, Value = o.MemberLevelId }), emptyTitle: "全部", emptyValue: "-1");
            ViewBag.Modes = modes.ToJson();
            //获取运算符
            //var signs = _dicBLL.GetDicListByPSN(222, true);
            var signs = Pharos.Logic.MemberDomain.QuanChengTaoProviders.Extensions.EnumExtensions.GetEnumDescription<LogicalOperatorType>();
            //ViewBag.LeftSign
            ViewBag.LeftSign = ListToSelect(signs.Select(o => new SelectListItem() { Text = o.Value, Value = o.Key.ToString() }));
            //    ViewBag.RightSign
            ViewBag.RightSign = ListToSelect(signs.Where(o => o.Key == 226 || o.Key == 227).Select(o => new SelectListItem() { Text = o.Value, Value = o.Key.ToString() }));
            //        ViewBag.Mode
            //            ViewBag.OperationType
            ViewBag.OperationType = ListToSelect(_dicBLL.GetDicListByPSN(234, true).Select(o => new SelectListItem() { Text = o.Title, Value = o.DicSN.ToString() }));
            // 赠送类型               ViewBag.GivenType
            ViewBag.GivenType = ListToSelect(_dicBLL.GetDicListByPSN(231, true).Select(o => new SelectListItem() { Text = o.Title, Value = o.DicSN.ToString() }));
            //付款方式
            ViewBag.ApiTypes = ListToSelect(ApiLibraryService.GetAllApiLibrary().Select(o => new SelectListItem() { Text = o.Title, Value = o.ApiCode.ToString() }));

            var parents = ProductCategoryService.GetParentTypes().Select(o => new DropdownItem() { Value = o.CategorySN.ToString(), Text = o.Title }).ToList();
            ViewBag.parenttypes = parents.ToJson();
            parents = ProductBrandService.GetList().Select(o => new DropdownItem(o.BrandSN.ToString(), o.Title)).ToList();
            ViewBag.brands = parents.ToJson();
            return View(rules);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="_rules"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateReturnRules(ReturnRules _rules)
        {
            var result = new OpResult();
            if (_rules.Id == 0)
            {
                result = _returnRuleBll.CreateReturnRule(_rules);
            }
            else
            {
                result = _returnRuleBll.UpdateReturnRule(_rules);
            }
            return Content(result.ToJson());
        }

        public class AdapterModes
        {
            /// <summary>
            /// 对应适配器id
            /// </summary>
            public string Adapter { get; set; }
            /// <summary>
            /// 适配器对应模式
            /// </summary>
            public IDictionary<int, string> Modes { get; set; }
        }
        /// <summary>
        /// 网格填充数据
        /// </summary>
        /// <returns></returns>
        public ActionResult FindReturnRulesList()
        {
            int count = 0;
            var result = _returnRuleBll.FindReturnRulesList(out count);
            return ToDataGrid(result, count);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetReturnRuleById(int adapert, int id)
        {
            //适配器是否=
            if (id > 0)
            {

                int count = 0;
                var _rules = _returnRuleBll.FindReturnRuleById(id);
                if (_rules != null)
                {
                    if (_rules.Adapters == adapert)
                    {
                        //对应明细
                        switch (adapert)
                        {
                            //case 218://付款方式
                            //    break;
                            case 219://品类
                                List<ProductCategory> result = new List<ProductCategory>();
                                List<string> currentValue = new List<string>();
                                if (_rules.LimitItems.Trim(',').Contains(","))
                                {
                                    currentValue = _rules.LimitItems.Trim(',').Split(',').ToList();
                                }
                                else
                                {
                                    currentValue.Add(_rules.LimitItems);
                                }
                                foreach (var item in currentValue)
                                {
                                    ProductCategory _category = new ProductCategory();
                                    var category = new ProductCategoryService().GetProductCategoryByCategorySN(Convert.ToInt32(item));

                                    while (category.CategoryPSN != 0)
                                    {
                                        switch (category.Grade)
                                        {
                                            case 4:
                                                break;
                                            case 3:
                                                _category.SubCategory = category.CategorySN;
                                                _category.SubCategoryTitle = category.Title;
                                                break;
                                            case 2:
                                                _category.MidCategory = category.CategorySN;
                                                _category.MidCategoryTitle = category.Title;
                                                break;
                                            case 1:
                                                _category.CategorySN = category.CategorySN;
                                                _category.BigCategoryTitle = category.Title;
                                                break;
                                        }

                                        category = new ProductCategoryService().GetProductCategoryByCategorySN(Convert.ToInt32(category.CategoryPSN));

                                    }
                                    if (category.CategoryPSN == 0)
                                    {
                                        _category.CategorySN = category.CategorySN;
                                        _category.BigCategoryTitle = category.Title;
                                    }
                                    result.Add(_category);
                                    count++;
                                }
                                return ToDataGrid(result, count);
                            case 218://具体商品
                                break;
                        }
                    }
                }
            }

            return ToDataGrid(null, 0);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult UpdateReturnRulesState(int state, string ids)
        {
            var result = _returnRuleBll.UpdateReturnRulesState(state, ids);
            return Content(result.ToJson());
        }
        private readonly ReturnStageRulesBLL _stageRuleBll = new ReturnStageRulesBLL();
        /// <summary>
        /// 创建分期赠送规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateRuleStage(int returnRuleId)
        {
            var result = _stageRuleBll.GetStageRuleByReturnRuleId(returnRuleId);
            return View(result);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="_stageRule"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateRuleStage(ReturnStageRules _stageRule)
        {
            var result = _stageRuleBll.CreateOrUpdateStageRule(_stageRule);
            return Content(result.ToJson());
        }
        #endregion

        #region 购物卡
        public ActionResult ShippingCardIndex()
        {
            var cardtype = _cardInfoBLL.GetAllMemberCardType().Where(o => o.CardType == "2").Select(o => new SelectListItem() { Text = o.CardName, Value = o.CardTypeId });
            ViewBag.CardType = ListToSelect(cardtype, emptyTitle: "全部");
            ViewBag.cardTypeToDropDown = cardtype.ToJson();
            return View();
        }

        [HttpPost]
        public ActionResult FindShippingCardPageList(string type, short? state, int beOverdue, DateTime? dueTimeStart, DateTime? dueTimeEnd, string keyWord)
        {
            var count = 0;
            var types = new int[] { 2 };
            var result = _membershipCardBLL.FindMembershipCardPageList(type, state, beOverdue, dueTimeStart, dueTimeEnd, keyWord.Replace(" ", ""), types, out count);
            return ToDataGrid(result, count);
        }
        #endregion

        #region 返赠方案调整

        #endregion

        #region 刷卡基础配置
        private readonly MembershipCardSettingBLL _settingBll = new MembershipCardSettingBLL();
        /// <summary>
        /// 配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MembershipCardSetting()
        {
            MembershipCardSetting setting = new MembershipCardSetting();
            var _setting = _settingBll.GetSettingByCompanyId();
            if (_setting != null)
            {
                setting = _setting;
            }
            return View(setting);
        }
        /// <summary>
        /// 配置保存
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MembershipCardSetting(MembershipCardSetting setting)
        {
            var result = _settingBll.CreateOrUpdate(setting);
            return Content(result.ToJson());
        }
        #endregion

        #region 积分兑换单
        private readonly IntegralRecordsBLL _integralRecords = new IntegralRecordsBLL();
        /// <summary>
        /// 兑换信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ExchangeInfo()
        {
            return View();
        }
        /// <summary>
        /// 网格
        /// </summary>
        /// <returns></returns>
        public ActionResult IntegralRecordPageList()
        {
            int count = 0;
            var result = _integralRecords.GetIntegralRecordPageList(Request.Params, out count);
            return ToDataGrid(result, count);
        }
        /// <summary>
        /// 积分赠送分期明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult IntegralRecordDetails(string id)
        {
            ViewBag.id = id;
            return View();
        }
        /// <summary>
        /// 积分赠送分期明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult IntegralRecordDetailPageList(string id)
        {
            int count = 0;
            var result = _integralRecords.GetIntegralRecordDetailPageList(id, out count);
            return ToDataGrid(result, count);
        }
        #endregion

        #region 卡消费明细
        private readonly MemberCardPayDetailsBLL payDetailbll = new MemberCardPayDetailsBLL();
        /// <summary>
        /// 卡消费明细
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberCardPayDetail()
        {
            var cardtype = _cardInfoBLL.GetAllMemberCardType().Select(o => new SelectListItem() { Text = o.CardName, Value = o.CardTypeId });
            ViewBag.CardType = ListToSelect(cardtype, emptyTitle: "全部");
            ViewBag.shops = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="cardtype"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ActionResult FindMemberCardPayDetailPageList(string cardtype, DateTime? beginDate, DateTime? endDate, int? storeIds, string cardNo)
        {
            int count = 0;
            object footer = null;
            var result = payDetailbll.GetMemberCardPayDetailsByPageList(cardtype, beginDate, endDate, storeIds, cardNo.Replace(" ", ""), out count, ref footer);
            return ToDataGrid(result, count, footer);
        }
        #endregion
    }
}