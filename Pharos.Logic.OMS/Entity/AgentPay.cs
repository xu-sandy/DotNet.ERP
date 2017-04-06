// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：
// 创建时间：2016-11-21
// 描述信息：代理商支付渠道
// --------------------------------------------------

using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 代理商支付渠道
	/// </summary>
	[Serializable]
	public class AgentPay
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		private int _Id;

		/// <summary>
		/// 支付渠道GUID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string AgentPayId
		{
			get { return _AgentPayId; }
			set { _AgentPayId = value; }
		}
		private string _AgentPayId;

		/// <summary>
		/// 代理商编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int AgentsId
		{
			get { return _AgentsId; }
			set { _AgentsId = value; }
		}
		private int _AgentsId;

		/// <summary>
        /// 接口编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        public int ApiNo
		{
            get { return _PayChannel; }
            set { _PayChannel = value; }
		}
        private int _PayChannel;

		/// <summary>
		/// 成本费率（%）
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// </summary>
		public decimal Cost
		{
			get { return _Cost; }
			set { _Cost = value; }
		}
		private decimal _Cost;

		/// <summary>
		/// 下级费率（%）
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// </summary>
		public decimal Lower
		{
			get { return _Lower; }
			set { _Lower = value; }
		}
		private decimal _Lower;

		/// <summary>
		/// 创建人（来自AgentsUsers表）
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUid
		{
			get { return _CreateUid; }
			set { _CreateUid = value; }
		}
		private string _CreateUid;

		/// <summary>
		/// 创建时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateTime
		{
			get { return _CreateTime; }
			set { _CreateTime = value; }
		}
		private DateTime _CreateTime;

		/// <summary>
		/// 修改时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime UpdateTime
		{
			get { return _UpdateTime; }
			set { _UpdateTime = value; }
		}
		private DateTime _UpdateTime;

        /// <summary>
        /// 状态
        /// </summary>
        [NotMapped]
        public int Status { get; set; }
	}
}