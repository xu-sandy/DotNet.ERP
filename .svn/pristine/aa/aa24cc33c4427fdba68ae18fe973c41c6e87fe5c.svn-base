// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：蔡少发
// 创建时间：2016-08-02
// 描述信息：
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 用于管理本系统的充值赠送信息 
	/// </summary>
	[Serializable]
	public class RechargeGiftsStage
	{
		/// <summary>
		/// 记录 ID 
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 公司CID
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public int CompanyId { get; set; }

		/// <summary>
		/// 卡号 GUID 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CardId { get; set; }

		/// <summary>
		/// 充值流水号 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string RechargeSN { get; set; }

		/// <summary>
		/// 赠送规则 GUID 
		/// [长度：40]
		/// </summary>
		public string RuleId { get; set; }

		/// <summary>
		/// 赠送项目（1：金额、2：积分） 
		/// [长度：5]
		/// </summary>
		public short GiftProject { get; set; }

		/// <summary>
		/// 分值 
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// </summary>
		public decimal GiftValue { get; set; }

		/// <summary>
		/// 赠送日期 
		/// [长度：10]
		/// </summary>
		public string GiftDate { get; set; }

		/// <summary>
		/// 赠送状态（0：未赠送；1：已赠送；2：已失效） 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int State { get; set; }

		/// <summary>
		/// 创建时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 创建人
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID { get; set; }
	}
}
