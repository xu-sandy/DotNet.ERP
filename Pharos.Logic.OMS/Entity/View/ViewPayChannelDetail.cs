using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    /// <summary>
    /// 收单渠道详情
    /// </summary>
    public class ViewPayChannelDetail
    {
        /// <summary>
        /// 收单渠道详情ID
        /// </summary>
        public string ChannelDetailId
        {
            set;
            get;
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayManner
        {
            set;
            get;
        }

        /// <summary>
        /// 渠道支付方式（枚举：1：扫码支付；2：网站支付；3：刷卡支付；）
        /// </summary>
        public short ChannelPayMode
        {
            get;
            set;
        }
    }
}
