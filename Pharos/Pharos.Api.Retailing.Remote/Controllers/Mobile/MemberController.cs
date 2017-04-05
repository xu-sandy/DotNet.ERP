using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Web.Http;
using Pharos.Api.Retailing.Models.Mobile;
using Pharos.Logic.BLL;
using Pharos.Logic.ApiData.Mobile.Exceptions;

namespace Pharos.Api.Retailing.Controllers.Mobile
{
    [RoutePrefix("api/mobile")]
    public class MemberController : ApiController
    {
        /// <summary>
        /// 根据会员卡获取会员积分
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMemberIntegralByCardSN")]
        public object GetMemberIntegralByCardSN([FromBody]MemberIntegralRequest requestParams)
        {
            if (string.IsNullOrEmpty(requestParams.CardSN))
            {
                throw new MessageException("参数信息不完整!");
            }
            var memberCardInfo = MembershipCardService.Find(o => o.CardSN == requestParams.CardSN && o.CompanyId == requestParams.CID);
            if (memberCardInfo == null)
            {
                throw new MessageException("会员卡不存在!");
            }
            if (memberCardInfo.State != 1)
            {
                var err = string.Empty;
                switch (memberCardInfo.State)
                {
                    case 0:
                        err = "未激活";
                        break;
                    case 2:
                        err = "已挂失";
                        break;
                    case 3:
                        err = "已作废";
                        break;
                    case 4:
                        err = "已退卡";
                        break;
                    default:
                        err = "状态异常";
                        break;
                }
                throw new MessageException("该会员卡" + err);
            }
            var member = MembersService.Find(o => o.MemberId == memberCardInfo.MemberId);
            return new { member.UsableIntegral, memberCardInfo.CardSN, memberCardInfo.MemberId };
        }
    }
}