/**  版本信息模板在安装目录下，可自行修改。
* PayChannelDetails.cs
*
* 功 能： N/A
* 类 名： PayChannelDetails
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/1/10 10:37:48   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 收单渠道管理详情表：用于管理收单渠道的详情信息
	/// </summary>
	[Serializable]
	public partial class PayChannelDetail
	{
		public PayChannelDetail()
		{}
		#region Model
		private int _id;
		private string _channeldetailid;
		private int _channelno;
        private short _state = 0;
        private string _opttype;
        private short _channelpaymode;
		private decimal _monthfreetradeamount=0M;
		private decimal _overservicerate=0M;
		private decimal _singleservfeeuplimit=0M;
		private decimal _singleservfeelowlimit=0M;
		private DateTime _effectivedate;
		private DateTime _createdt= DateTime.Now;
        private string _createuid;
        private bool _isdeleted = false;
		/// <summary>
		/// 记录ID（系统自增）
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 收单渠道详情ID
		/// </summary>
		public string ChannelDetailId
		{
			set{ _channeldetailid=value;}
			get{return _channeldetailid;}
		}
		/// <summary>
		/// 渠道编号(从1开始累加)
		/// </summary>
		public int ChannelNo
		{
			set{ _channelno=value;}
			get{return _channelno;}
		}
		/// <summary>
		/// 状态（枚举：0：未启用；1：启用；）
		/// </summary>
        public short State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
        /// 操作类型：枚举（0：不限；1：收款；2：退款；3转账；4付款；5：查询；）
		/// </summary>
		public string OptType
		{
			set{ _opttype=value;}
			get{return _opttype;}
		}
        /// <summary>
        /// 支付方式（枚举：1：扫码支付；2：网站支付；3：刷卡支付；）
        /// </summary>
        public short ChannelPayMode
        {
            set { _channelpaymode = value; }
            get { return _channelpaymode; }
        }
		/// <summary>
		/// 单月免费交易额度（元）
		/// </summary>
		public decimal MonthFreeTradeAmount
		{
			set{ _monthfreetradeamount=value;}
			get{return _monthfreetradeamount;}
		}
		/// <summary>
		/// 超出金额服务费率（%）
		/// </summary>
		public decimal OverServiceRate
		{
			set{ _overservicerate=value;}
			get{return _overservicerate;}
		}
		/// <summary>
		/// 单笔服务费上限（元）
		/// </summary>
		public decimal SingleServFeeUpLimit
		{
			set{ _singleservfeeuplimit=value;}
			get{return _singleservfeeuplimit;}
		}
		/// <summary>
		/// 单笔服务费下限（元）
		/// </summary>
		public decimal SingleServFeeLowLimit
		{
			set{ _singleservfeelowlimit=value;}
			get{return _singleservfeelowlimit;}
		}
		/// <summary>
		/// 生效日期
		/// </summary>
		public DateTime EffectiveDate
		{
			set{ _effectivedate=value;}
			get{return _effectivedate;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
        public DateTime CreateDT
		{
			set{ _createdt=value;}
            get { return _createdt; }
		}
		/// <summary>
		/// 创建人UID
		/// </summary>
        public string CreateUID
		{
			set{ _createuid=value;}
            get { return _createuid; }
		}
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted
        {
            set { _isdeleted = value; }
            get { return _isdeleted; }
        }
		#endregion Model

	}
}

