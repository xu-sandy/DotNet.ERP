using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Web.Http;

namespace Pharos.Api.Retailing.Controllers.Mobile
{
    /// <summary>
    /// IM聊天
    /// </summary>
    [RoutePrefix("api/mobile")]
    public class ChatController : ApiController
    {
        /// <summary>
        /// 获取IM联系人列表
        /// </summary>
        /// <param name="account">（可选）当前用户登录账号</param>
        /// <returns>状态为“正常”的用户列表（包含信息：IMUserName，FullName，PhotoUrl）。若username不为空，则过滤username。</returns>
        [HttpPost]
        [Route("GetContactsList")]
        public object GetContactsList([FromBody]JObject requestParams)
        {
            string account = requestParams.Property("account", true);
            return Pharos.Logic.BLL.UserInfoService.GetUsersInfoForIMContacts(account);
        }

        /// <summary>
        /// 获取IM单个用户信息
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns>IM用户信息(（包含信息：IMUserName、PhotoUrl，FullName，UserCode，Sex，Department，Signature，Mobile）</returns>
        [HttpPost]
        [Route("GetIMUserInfo")]
        public object GetIMUserInfo([FromBody]JObject requestParams)
        {
            string imUserName = requestParams.Property("imUserName", true);
            return Pharos.Logic.BLL.UserInfoService.GetUserInfoForIM(imUserName);
        }

        /// <summary>
        /// 手机APP端获取环信AppKey
        /// </summary>
        /// <returns>环信AppKey</returns>
        [HttpPost]
        [Route("IMAppKey")]
        public object IMAppKey()
        {
            return new { IMAppKey = string.Format("{0}#{1}",Pharos.Logic.ApiData.Mobile.Services.ChatService.EmChat.OrgName,Pharos.Logic.ApiData.Mobile.Services.ChatService.EmChat.AppName) };
        }
    }
}
