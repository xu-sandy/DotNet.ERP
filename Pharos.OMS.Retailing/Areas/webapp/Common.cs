using Newtonsoft.Json.Linq;
using Pharos.Component.qrcode;
using Pharos.Component.qrcode.wx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.OMS.Retailing.Areas.webapp
{
    public class Common
    {
        /// <summary>
        /// 获取微信号
        /// </summary>
        /// <returns></returns>
        public static bool GetNickName(out string name)
        {
            var request=HttpContext.Current.Request;
            Log.Debug(request.RawUrl, "进入微信授权页面");
            var code = request["code"];//有效期5分钟
            Log.Debug(request.RawUrl,"code:"+ code);
            if(!string.IsNullOrWhiteSpace(code))
            {
                string appid = Pharos.Utility.Config.GetAppSettings("appid"), secret = Pharos.Utility.Config.GetAppSettings("secret");
                //超过3个月未使用appsecret,会变换,与商户API安全KEY不同一个
                //网页授权access_token
                var getTokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid="+appid+"&secret="+secret+"&code=" + code + "&grant_type=authorization_code";
                string response = HttpService.Get(getTokenUrl);
                Log.Debug(getTokenUrl, "get access_token : " + response);
                try
                {
                    var json = JObject.Parse(response);

                    var token = Convert.ToString(json.Property("access_token").Value);
                    var openid = Convert.ToString(json.Property("openid").Value);
                    if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(openid))
                    {
                        var getUserUrl = "https://api.weixin.qq.com/sns/userinfo?access_token=" + token + "&openid=" + openid + "&lang=zh_CN";
                        response = HttpService.Get(getUserUrl);
                        Log.Debug(getUserUrl, "get user info : " + response);
                        json = JObject.Parse(response);
                        name= Convert.ToString(json.Property("nickname").Value);
                        return true;
                    }
                }
                catch {
                    name = "获取微信号出现异常！";
                    return false;
                }
            }
            name = "请在微信公众号下进行访问！";
            return false;
        }
    }
}