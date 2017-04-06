using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Sys;
using Pharos.Utility;

namespace Pharos.Logic.BLL
{
    /// <summary>
    /// 卡类型Bll
    /// </summary>
    public class CardInfoBLL
    {
        CardInfoService _service = new CardInfoService();
        /// <summary>
        /// 查询现有卡类型
        /// </summary>
        /// <returns></returns>
        public object FindMemberCardTypeList(out int count)
        {
            return _service.FindMemberCardTypeList(out count);
        }
        /// <summary>
        /// 新建卡类型
        /// </summary>
        /// <param name="cardInfo"></param>
        /// <returns></returns>
        public OpResult CreateMemberCardType(CardInfo cardInfo)
        {
            //判断名称重复
            var data = BaseService<CardInfo>.CurrentRepository.Entities.FirstOrDefault(o => o.CompanyId == CommonService.CompanyId && cardInfo.CardName == o.CardName && o.Id != cardInfo.Id);
            if (data != null)
            {
                return OpResult.Fail("卡片名称重复！");
            }
            if (cardInfo.Id == 0)
            {
                cardInfo.CardTypeId = SysCommonRules.GUID;
                cardInfo.CreateDT = DateTime.Now;
                cardInfo.CreateUID = Sys.CurrentUser.UID;
                cardInfo.CompanyId = CommonService.CompanyId;
                return _service.CreateMemberCardType(cardInfo);
            }
            else
            {
                return _service.UpdateMemberCardType(cardInfo);
            }
        }
        /// <summary>
        /// 设置卡类型状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpResult UpdateCardState(short state, string ids)
        {
            return _service.UpdateState(state, ids);
        }

        public List<CardInfo> GetAllMemberCardType()
        {
            return _service.GetAllMemberCardType();
        }
    }
}
