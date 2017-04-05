﻿/**  版本信息模板在安装目录下，可自行修改。
* TradeResults.cs
*
* 功 能： N/A
* 类 名： TradeResults
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/1/10 10:37:51   N/A    初版
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
	/// 商户交易结果表：用于管理本系统中商户的交易记录的交易结果（由第三方返回）
	/// </summary>
	[Serializable]
	public partial class TradeResult
	{
		public TradeResult()
		{}
		#region Model
		private int _id;
		private string _outtradeno;
		private string _oldouttradeno;
		private string _mchid3;
        private string _storeid3;
        private string _tradeno3;
        private short _tradetype;
		private decimal _receiptamount=0M;
		private string _tradedate;
		private string _tradetime;
        private short _tradestate;
        private short _paychannel = 0;
		private string _signtype;
		private string _version;
		private string _signature;
		private DateTime _createdt= DateTime.Now;
		/// <summary>
		/// 主键自增
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 商户支付订单号
		/// </summary>
		public string OutTradeNo
		{
			set{ _outtradeno=value;}
			get{return _outtradeno;}
		}
		/// <summary>
		/// 商户退款原始商户支付单号（退款订单时该值必传）
		/// </summary>
		public string OldOutTradeNo
		{
			set{ _oldouttradeno=value;}
			get{return _oldouttradeno;}
		}
		/// <summary>
		/// 第三方商户号
		/// </summary>
		public string MchId3
		{
			set{ _mchid3=value;}
			get{return _mchid3;}
		}
        /// <summary>
        /// 第三方门店编号
        /// </summary>
        public string StoreId3
        {
            set { _storeid3 = value; }
            get { return _storeid3; }
        }
		/// <summary>
		/// 第三方返回的交易流水号
		/// </summary>
		public string TradeNo3
		{
			set{ _tradeno3=value;}
			get{return _tradeno3;}
		}
		/// <summary>
		/// 交易类型(0=收入，1=支出)
		/// </summary>
        public short TradeType
		{
			set{ _tradetype=value;}
			get{return _tradetype;}
		}
		/// <summary>
		/// 实际交易金额，以元为单位
		/// </summary>
		public decimal ReceiptAmount
		{
			set{ _receiptamount=value;}
			get{return _receiptamount;}
		}
		/// <summary>
		/// 交易日期，格式：yyyyMMdd
		/// </summary>
		public string TradeDate
		{
			set{ _tradedate=value;}
			get{return _tradedate;}
		}
		/// <summary>
		/// 交易时间，格式：HHmmss
		/// </summary>
		public string TradeTime
		{
			set{ _tradetime=value;}
			get{return _tradetime;}
		}
		/// <summary>
		/// 交易结果若交易类型=收入，则枚举值为：0：未支付，1：支付成功，2：支付失败，3：已撤销，4：支付超时；若交易类型=支出，则枚举值为：0：退款中；1：退款成功；2：退款失败；
		/// </summary>
        public short TradeState
		{
			set{ _tradestate=value;}
			get{return _tradestate;}
		}
		/// <summary>
		/// 上游返回的支付渠道，枚举（0：无；1：WX（微信支付），2：ALIPAY（支付宝支付））
		/// </summary>
        public short PayChannel
		{
			set{ _paychannel=value;}
			get{return _paychannel;}
		}
		/// <summary>
		/// 第三方签名方式
		/// </summary>
		public string SignType
		{
			set{ _signtype=value;}
			get{return _signtype;}
		}
		/// <summary>
		/// 第三方版本号
		/// </summary>
		public string Version
		{
			set{ _version=value;}
			get{return _version;}
		}
		/// <summary>
		/// 第三方数字签名
		/// </summary>
		public string Signature
		{
			set{ _signature=value;}
			get{return _signature;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateDT
		{
			set{ _createdt=value;}
			get{return _createdt;}
		}
		#endregion Model

	}
}

