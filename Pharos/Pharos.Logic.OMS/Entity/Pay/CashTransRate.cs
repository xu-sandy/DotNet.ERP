﻿/**  版本信息模板在安装目录下，可自行修改。
* CashTransRates.cs
*
* 功 能： N/A
* 类 名： CashTransRates
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/1/10 10:37:46   N/A    初版
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
	/// 提现转账费率表：用于管理提现转账费率的基础信息
	/// </summary>
	[Serializable]
	public partial class CashTransRate
	{
		public CashTransRate()
		{}
		#region Model
		private int _id;
		private short _state=0;
		private decimal _version=0M;
		private int _channelno;
		private decimal _basefreeamount=0M;
		private decimal _overamountrate;
		private decimal _singleminfees=0M;
		private int _settlementperiodid=0;
		private int _businesscategoryone;
		private int _businesscategorytwo;
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
		/// 状态（枚举：0：未发布；1：已发布；2：已失效）
		/// </summary>
        public short State
		{
			set{ _state=value;}
			get{return _state;}
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
		/// 渠道编号，关联PayChannelDetails表
		/// </summary>
		public int ChannelNo
		{
			set{ _channelno=value;}
			get{return _channelno;}
		}
		/// <summary>
		/// 基础免费额度（元）
		/// </summary>
		public decimal BaseFreeAmount
		{
			set{ _basefreeamount=value;}
			get{return _basefreeamount;}
		}
		/// <summary>
		/// 超出金额费率（%）
		/// </summary>
		public decimal OverAmountRate
		{
			set{ _overamountrate=value;}
			get{return _overamountrate;}
		}
		/// <summary>
		/// 单笔最低收费（元）
		/// </summary>
		public decimal SingleMinFees
		{
			set{ _singleminfees=value;}
			get{return _singleminfees;}
		}
		/// <summary>
		/// 结算周期
		/// </summary>
		public int SettlementPeriodId
		{
			set{ _settlementperiodid=value;}
			get{return _settlementperiodid;}
		}
		/// <summary>
		/// 经营一级类目
		/// </summary>
		public int BusinessCategoryOne
		{
			set{ _businesscategoryone=value;}
			get{return _businesscategoryone;}
		}
		/// <summary>
		/// 经营二级类目
		/// </summary>
		public int BusinessCategoryTwo
		{
			set{ _businesscategorytwo=value;}
			get{return _businesscategorytwo;}
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

