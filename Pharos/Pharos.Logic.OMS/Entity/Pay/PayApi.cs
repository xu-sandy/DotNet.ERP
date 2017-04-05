﻿/**  版本信息模板在安装目录下，可自行修改。
* PayApis.cs
*
* 功 能： N/A
* 类 名： PayApis
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/1/10 10:37:47   N/A    初版
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
	/// 支付接口表：用于管理本系统中的支付接口信息
	/// </summary>
	[Serializable]
	public partial class PayApi
	{
		public PayApi()
		{}
		#region Model
		private int _id;
        private short _state = 0;
		private int _apino;
        private string _apiurl;
        private string _method;
		private string _title;
		private decimal _version=0M;
		private int _trademode;
		private int _channelno;
        private short _opttype;
        private short _channelpaymode;
		private string _memo;
		private DateTime _createdt= DateTime.Now;
		private string _createuid;
		private DateTime _releaseddt= DateTime.Now;
		private string _releaseduid;
		/// <summary>
		/// 记录ID（系统自增）
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 接口状态（0：未发布；1：已发布；2：已失效)
		/// </summary>
        public short State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 接口编号(从1开始累加)
		/// </summary>
		public int ApiNo
		{
			set{ _apino=value;}
			get{return _apino;}
		}
        /// <summary>
		/// 接口地址
		/// </summary>
        public string ApiUrl
		{
			set{ _apiurl=value;}
            get { return _apiurl; }
		}
		/// <summary>
		/// 接口参数名
		/// </summary>
		public string Method
		{
			set{ _method=value;}
			get{return _method;}
		}
		/// <summary>
		/// 接口别名
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 版本号
		/// </summary>
		public decimal Version
		{
			set{ _version=value;}
			get{return _version;}
		}
		/// <summary>
        /// 交易方式（数据字典+枚举：详情 TradeMode（如：购买者付款扫码(主扫支付动态)、商家收款扫码(被扫支付)、退款申请）根据支付方式过滤交易方式
		/// </summary>
		public int TradeMode
		{
			set{ _trademode=value;}
			get{return _trademode;}
		}
		/// <summary>
		/// 渠道编号，关联PayChannelDetails表
		/// </summary>
		public int ChannelNo
		{
			set{ _channelno=value;}
			get{return _channelno;}
		}
		/// <summary>
        /// 操作类型：枚举（0：不限；1：收款；2：退款；3转账；4付款；5：查询；）
		/// </summary>
        public short OptType
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
		/// 备注
		/// </summary>
		public string Memo
		{
			set{ _memo=value;}
			get{return _memo;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateDT
		{
			set{ _createdt=value;}
			get{return _createdt;}
		}
		/// <summary>
		/// 创建人UID
		/// </summary>
		public string CreateUID
		{
			set{ _createuid=value;}
			get{return _createuid;}
		}
		/// <summary>
		/// 发布时间
		/// </summary>
		public DateTime ReleasedDT
		{
			set{ _releaseddt=value;}
			get{return _releaseddt;}
		}
		/// <summary>
		/// 发布人
		/// </summary>
		public string ReleasedUID
		{
			set{ _releaseduid=value;}
			get{return _releaseduid;}
		}
		#endregion Model

	}
}

