using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    /// <summary>
    /// 支付接口
    /// </summary>
    public interface IPay
    {
        int CompanyId { get; set; }
        string StoreId { get; set; }
        string MachineSn { get; set; }
        string DeviceSn { get; set; }
        int OrderNumber { get; set; }
        /// <summary>
        /// 支付名称
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// 支付接口地址
        /// </summary>
        [JsonIgnore]
        string ApiUrl { get; set; }
        /// <summary>
        /// 支付图标
        /// </summary>
        string EnableIcon { get; set; }
        /// <summary>
        /// 支付不可用图标
        /// </summary>
        string DisableIcon { get; set; }
        /// <summary>
        /// 支付编码
        /// </summary>
        [JsonIgnore]
        int ApiCode { get; set; }
        /// <summary>
        /// 支付明细
        /// </summary>
        [JsonIgnore]
        PayDetails PayDetails { get; set; }

        /// <summary>
        /// 支付回调
        /// </summary>
        [JsonIgnore]
        Action<IPay> CallBack { get; set; }
        /// <summary>
        /// 支付是否可用
        /// </summary>
        bool Enable { get; set; }
        /// <summary>
        /// 多个支付编码串
        /// </summary>
        string ApiCodes { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        PayMode Mode { get; set; }
        /// <summary>
        /// 设置完成支付,保存支付信息
        /// </summary>
        void SavePayInfomactions();
        /// <summary>
        /// 刷新支付状态
        /// </summary>
        /// <param name="storeId">门店ID</param>
        /// <param name="companyToken">公司ID</param>
        void RefreshStatus();

        /// <summary>
        /// 初始化支付
        /// </summary>
        /// <param name="companyId">公司ID</param>
        void Init(int companyId, string storeId, string machineSn, string deviceSn);
    }
}
