using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using qrcode = Pharos.Component.qrcode;
namespace Pharos.Pay.PayResult
{
    public partial class AlipayNotify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SortedDictionary<string, string> sPara = GetRequestPost();
                qrcode.Log.Info(this.GetType().Name, "支付宝进入通知页面，参数量:" + sPara.Count);
                if (sPara.Count > 0)//判断是否有带返回参数
                {
                    var aliNotify = new qrcode._1.Notify();
                    bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);//不是从配置里取

                    qrcode.Log.Info(this.GetType().Name, "notify_id:" + Request.Form["notify_id"] + ",sign=" + Request.Form["sign"] + ",verifyResult=" + verifyResult);
                    string notify_data = Request.Form["notify_data"];
                    qrcode.Log.Info(this.GetType().Name, "notify_data=" + notify_data);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(notify_data);
                    XmlNode root = xmlDoc.DocumentElement;//返回的根节点
                    string is_success = root.SelectSingleNode("trade_status").InnerText;
                    string temp = root.SelectSingleNode("subject").InnerText;//传 的时候把商品名称传成订单号了，所以接收到的也是订单号
                    string orderNo = temp.Split('-')[1].Replace("订单编号", "");//订单号
                    string total_fee = root.SelectSingleNode("total_fee").InnerText;//订单金额
                    string trade_no = root.SelectSingleNode("trade_no").InnerText;//订单号
                    decimal fee = 0;
                    decimal.TryParse(total_fee, out fee);
                    int companyId = 0;
                    if (orderNo.Contains("_"))
                    {
                        companyId = int.Parse(orderNo.Split('_')[0]);
                        orderNo = orderNo.Split('_')[1];
                    }
                    if (is_success == "TRADE_FINISHED" || is_success == "TRADE_SUCCESS")
                    {
                        NotifyDAL.AddOne(new PayNotifyResult()
                        {
                            PaySN = orderNo,
                            CashFee = fee,
                            ApiCode = 14,
                            TradeNo = trade_no,
                            CompanyId = companyId,
                            State = "Success"
                        });
                        Response.Write("success");  //请不要修改或删除
                    }
                    else
                    {
                        NotifyDAL.AddOne(new PayNotifyResult()
                        {
                            PaySN = orderNo,
                            CashFee = fee,
                            ApiCode = 14,
                            TradeNo = trade_no,
                            CompanyId = companyId,
                            State = is_success
                        });
                        Response.Write("fail");  //请不要修改或删除
                    }
                    
                }
                else
                {
                    Response.Write("无通知参数");
                }
            }catch(Exception ex)
            {
                Response.Write("fail");
                Log.Error(typeof(AlipayNotify).Name,"支付宝支付异常!"+ ex.Message+ex.StackTrace);
            }
            
        }
        private SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            var sArray = new SortedDictionary<string, string>();
            var coll = Request.Form;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            return sArray;
        }
    }
}