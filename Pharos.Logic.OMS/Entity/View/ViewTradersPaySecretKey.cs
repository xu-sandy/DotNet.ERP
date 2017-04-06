using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    /// <summary>
    /// 商家支付通道
    /// </summary>
    public class ViewTradersPaySecretKey
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 商家支付密钥ID（GUID）
        /// </summary>
        public string TPaySecrectId { get; set; }

        /// <summary>
        /// 指派人
        /// </summary>
        public string Assign { get; set; }

        /// <summary>
        /// 通道状态
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 系统商户号
        /// </summary>
        public int? CID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string TradersFullTitle { get; set; }

        /// <summary>
        /// 支付安全Key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 第三方支付商户号
        /// </summary>
        public string MchId3 { get; set; }

        /// <summary>
        /// 第三方支付Key
        /// </summary>
        public string SecretKey3 { get; set; }

        /// <summary>
        /// 支付通道
        /// </summary>
        public string ChannelCode { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public short? ChannelPayMode { get; set; }

        /// <summary>
        /// 支付结果通知URL
        /// </summary>
        public string PayNotifyUrl { get; set; }

        /// <summary>
        /// 退款结果通知URL
        /// </summary>
        public string RfdNotifyUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public string CreateFullName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDT { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditFullName { get; set; }
    }
}
