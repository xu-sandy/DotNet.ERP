﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Pharos.Logic.ApiData.Pos.Extensions;
using Pharos.Logic.ApiData.Pos.Exceptions;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    public class PaymentFactory
    {
        public static IPay Factory(int companyId, string storeId, string machineSn, string deviceSn, PayMode mode, PayDetails details, decimal receivable)
        {
            var pay = GetPays().FirstOrDefault(o => o.Mode == mode);
            if (pay == null)
            {
                throw new PosException("支付方式未授权或者未开启！");
            }
            pay.Init(companyId, storeId, machineSn, deviceSn);
            pay.PayDetails = details;
            return pay;

        }
        public static IPay Factory(int companyId, string storeId, string machineSn, string deviceSn, PayMode mode, List<PayDetails> details, decimal receivable, Action<IPay> callBack = null)
        {
            IPay pay;
            if (mode == PayMode.Multiply)
            {
                var tempPay = new MultiplyPay();
                tempPay.Init(companyId, storeId, machineSn, deviceSn);
                tempPay.AllPayDetails = details;
                var first =details.FirstOrDefault();
                tempPay.PayDetails = new PayDetails()
                {
                    Amount = details.Sum(o => o.Amount),
                    CreateDt = first.CreateDt,
                    PaySn = first.PaySn,
                    CardNo = string.Empty,
                    WipeZero = details.Sum(o=>o.WipeZero),
                    Change = details.Sum(o => o.Change),
                    Receive = details.Sum(o => o.Receive),
                    Mode = PayMode.Multiply
                };
                pay = tempPay;
            }
            else
            {
                pay = Factory(companyId, storeId, machineSn, deviceSn, mode, details.FirstOrDefault(), receivable);
            }
            if (callBack != null)
                pay.CallBack = callBack;
            return pay;
        }
        public static IEnumerable<IPay> GetPaysStatus(int companyId, string storeId, string machineSn, string deviceSn)
        {
            var pays = GetPays();
            List<IPay> enablePays = new List<IPay>();
            foreach (var item in pays)
            {
                if (item is MultiplyPay) continue;
                try
                {
                    item.Init(companyId, storeId, machineSn, deviceSn);
                }
                catch
                {
                    continue;
                }
                item.RefreshStatus();
                if (item.Enable)
                {
                    enablePays.Add(item);
                }
            }
            return enablePays.OrderBy(o => o.OrderNumber);
        }

        public static IEnumerable<IPay> GetPays()
        {
            var assemblies = new List<Assembly>();
            if (!assemblies.Any())
            {
                assemblies.Add(typeof(PaymentFactory).Assembly);
            }
            IEnumerable<IPay> pays = assemblies.GetImplementedObjectsByInterface<IPay>();
            return pays;
        }
    }
}
