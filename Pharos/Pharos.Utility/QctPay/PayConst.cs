﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Utility.QctPay
{
    public class PayConst
    {

        //测试账号
        //public const int MERCHANT_ID = 4;//您的商户号
        //public const int MERCHANT_STOREID = 30005;//您的商户门店号
        //public const string MERCHANT_SECRETKEY = "56D0F28A2AEE4A928BE282D1CCEBF33D";//您的商户安全密钥


        public const int QCTAPICODE_A = 26;
        public const int QCTAPICODE_P = 25;

        public const string SIGNTYPE = "MD5";//签名类型
        public const string VERSION = "1.0";//调用版本
        public const string CHARSET = "utf-8";//字符集格式

        #region QctPay Methods
        public const string QCTTRADE_PAY_BUYERSCAN_DYNA = "qct.trade.pay.buyerscandyna";
        public const string QCTTRADE_PAY_MERCHSCAN = "qct.trade.pay.merchscan";
        public const string QCTTRADE_REFUNDAPPLY = "qct.trade.refund.apply";

        public const string QCTTRADE_NOTIFY_PAY = "qct.trade.notify.pay";
        public const string QCTTRADE_NOTIFY_REFUND = "qct.trade.notify.refund";

        //public const string TEST_URL = "http://27.154.234.10:8016/api/pay/qctpay";
        //public const string RETURN_URL = "http://192.168.10.53:3122/api/mobile/TradeNotify";
        #endregion
    }
}
