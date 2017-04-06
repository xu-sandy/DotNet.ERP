using System;
using System.Collections.Generic;
using cn.jpush.api;
using cn.jpush.api.push.mode;
using Pharos.JPush;
using Pharos.JPush.Models;

namespace Pharos.JPushService
{
    public class JPushService :IJPush
    {
        //极光配置信息
        public const string APP_KEY = "6be9204c30b9473e87bad4dc";
        public const string MASTER_SECRET = "8aae478411e89f7682ed5af6";

        public void JPushSend(JPushModel model)
        {

            JPushClient client = new JPushClient(APP_KEY, MASTER_SECRET);

            try
            {
                var payload = PushObject_all_alisa_Alter(model.Alias, model.obj);

                var result = client.SendPush(payload);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// 极光针对别名推送
        /// </summary>
        /// <param name="alisa"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static PushPayload PushObject_all_alisa_Alter(string alisa, Dictionary<string,string> obj)
        {
            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();
            pushPayload.audience = Audience.s_alias(alisa);
            pushPayload.notification = new Notification().setAlert(obj.ToString());
            return pushPayload;
        }

        /// <summary>
        /// 全平台内容推送
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        //private static PushPayload PushObject_All_All_Alter(JObject obj)
        //{
        //    var pushPayload = new PushPayload();
        //    pushPayload.platform = Platform.all();
        //    pushPayload.audience = Audience.all();
        //    pushPayload.notification = new Notification().setAlert(obj.ToString());
        //    return pushPayload;
        //}
        
    }
}
