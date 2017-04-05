﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Infrastructure.Data.Normalize
{
    public class KeyFactory
    {
        public static string MachineKeyFactory(int companyToken, string storeId, string machineSn, string deviceSn)
        {
            return string.Format("{0}_{1}_{2}_{3}", companyToken, storeId, machineSn, deviceSn);
        }
        public static string SuspendKeyFactory(int companyToken, string storeId, string machineSn, string deviceSn)
        {
            return string.Format("{0}_{1}_{2}_{3}", companyToken, storeId, deviceSn, DateTime.Now.ToString("ddHHmmss"));
        }

        public static string StoreKeyFactory(int companyToken, string storeId)
        {
            return string.Format("{0}_{1}", companyToken, storeId);
        }
        public static string ProductKeyFactory(int companyToken, string storeId, string barcode)
        {
            return string.Format("{0}_{1}_{2}", companyToken, storeId, barcode);
        }

        public static string OrderChangeRefundSaleKeyFactory(int companyToken, string storeId, string machineSn, string deviceSn, int mode)
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}", companyToken, storeId, machineSn, deviceSn, mode);
        }
        public static string UserKeyFactory(int companyToken, string userId)
        {
            return string.Format("{0}_{1}", companyToken, userId);
        }
    }
}
