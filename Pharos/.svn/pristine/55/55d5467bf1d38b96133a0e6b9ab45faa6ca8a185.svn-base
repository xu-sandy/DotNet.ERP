using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Pharos.Logic.ApiData.Pos.Exceptions;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    /// <summary>
    /// 多方式支付
    /// </summary>
    public class MultiplyPay : BasePay
    {
        public MultiplyPay()
            : base()
        {
            Mode = PayMode.Multiply;
            Enable = true;
            Title = "多方式支付";
        }
        /// <summary>
        /// CID
        /// </summary>
        public int CompanyId { get; set; }
        [JsonIgnore]
        public IList<PayDetails> AllPayDetails { get; set; }

        public override void SavePayInfomactions()
        {
            var apiCodes = string.Empty;
            foreach (var item in AllPayDetails)
            {
                var details = new List<PayDetails>();
                details.Add(item);
                var pay = PaymentFactory.Factory(CompanyId, StoreId, MachineSn, DeviceSn, item.Mode, details, item.Amount);
                pay.SavePayInfomactions();
                if (!string.IsNullOrEmpty(apiCodes))
                {
                    apiCodes += ",";
                }
                apiCodes += pay.ApiCodes;
            }
            ApiCodes = apiCodes;
            if (CallBack != null)
            {
                CallBack(this);
            }
        }
        public override void Init(int companyId, string storeId, string machineSn, string deviceSn)
        {
            CompanyId = companyId;
            StoreId = storeId;
            MachineSn = machineSn;
            DeviceSn = deviceSn;
        }
        public override void RefreshStatus()
        {
            Enable = true;
        }

    }
}
