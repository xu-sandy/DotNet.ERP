/*----------------------------------------------------------------
 * 功能描述：SMS 常规小助手
 * 创 建 人：蔡少发
 * 创建时间：2015-05-11
//----------------------------------------------------------------*/

using System;
using System.IO;
using System.Web;
using System.Net;
using System.Text;
using System.Web.Security;

namespace Pharos.Utility
{
    /// <summary>
    /// SMS 常规小助手
    /// </summary>
    public class SMSApi
    {
        /// <summary>
        /// 发送短信-示例
        /// </summary>
        public void Test()
        {
            SMSApi sms = new SMSApi();

            //SMS接口集
            sms.SetApi = "http://114.80.227.125:8181/mtutf8/,http://112.65.228.196:8181/mtutf8/";

            //发送内容
            string content = string.Format("【道诚科技】您好，您的注册验证码为：{0}。", 7788);
            
            //开始发送
            SMSApi.SendStatus stat = sms.Send("caishaofa", "test123", "18910389637", content);

        }

        #region 处理过程

        /// <summary>
        /// 设置Api地址（必填）
        /// </summary>
        public string SetApi { get; set; }

        /// <summary>
        /// 发送短信（必填）
        /// </summary>
        /// <param name="uid">主用户账号</param>
        /// <param name="pwd">明文密码</param>
        /// <param name="mobile">接收号码（多个间用,号分隔；小灵通须加区号）</param>
        /// <param name="content">短信内容</param>
        /// <returns></returns>
        public SendStatus Send(string uid, string pwd, string mobile, string content)
        {
            if (!string.IsNullOrEmpty(SetApi))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("uid=").Append(uid);
                sb.Append("&pwd=").Append(FormsAuthentication.HashPasswordForStoringInConfigFile(pwd + uid, "MD5").ToLower());
                sb.Append("&mobile=").Append(mobile);
                sb.Append("&content=").Append(HttpContext.Current.Server.UrlEncode(content));

                ASCIIEncoding code = new ASCIIEncoding();
                byte[] bTemp = code.GetBytes(sb.ToString());
                string[] api = SetApi.Split(',');
                SendStatus ss = doPostRequest(api[0], bTemp);
                if (ss == SendStatus.验证失败)
                {
                    ss = doPostRequest(api[1], bTemp);
                }
                return ss;
            }

            return SendStatus.发送接口不正确;
        }

        private static SendStatus doPostRequest(string url, byte[] bData)
        {
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;

            string strResult = string.Empty;
            try
            {
                hwRequest = (HttpWebRequest)WebRequest.Create(url);
                hwRequest.Timeout = 5000;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;

                Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch (Exception err)
            {
                WriteErrLog(err.ToString());
                return SendStatus.验证失败;
            }

            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (Exception err)
            {
                WriteErrLog(err.ToString());
            }
            string stat = Url.GetUrlParms(strResult, "stat");
            return (SendStatus)Convert.ToInt16(stat);
        }
        private static void WriteErrLog(string strErr)
        {
            //todo: log
        }

        /// <summary>
        /// 发送状态
        /// </summary>
        public enum SendStatus : short
        {
            发送成功 = 100,
            验证失败 = 101,
            短信不足 = 102,
            操作失败 = 103,
            非法字符 = 104,
            内容过多 = 105,
            号码过多 = 106,
            频率过快 = 107,
            号码内容空 = 108,
            禁止频繁单条发送 = 110,
            号码错误 = 112,
            定时时间格式不对 = 113,
            账号被锁 = 114,
            禁止接口发送 = 116,
            绑定IP不正确 = 117,
            系统升级 = 120,
            发送接口不正确 = 130
        }

        #endregion
    }
}
