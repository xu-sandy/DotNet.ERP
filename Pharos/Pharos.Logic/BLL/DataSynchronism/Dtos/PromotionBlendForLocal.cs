using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
	/// <summary>
    /// 组合促销
	/// </summary>
    [Excel("组合信息")]
    public class PromotionBlendForLocal
	{
		/// <summary>
		/// 规则类别（1:组合促销、2:满元促销）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
        [Pharos.Utility.Exclude]
        

        [Excel("规则类别", 1)]
        public short RuleType { get; set; }

		/// <summary>
		/// 促销ID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
        [Pharos.Utility.Exclude]
        

        [Excel("促销ID", 2)]
        public string CommodityId { get; set; }

		/// <summary>
		/// 促销形式（1:现金返回、2:代金券、3:折扣、4:加购按具体商品赠送、5:加购按单价范围赠送）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
        [Excel("促销形式", 3)]
        public short PromotionType { get; set; }

		/// <summary>
		/// 满件数或满N元
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((2))]
		/// </summary>
        [Excel("满件数", 4)]
        public decimal FullNumber { get; set; }

		/// <summary>
		/// 折扣或多少元
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// </summary>
        [Excel("折扣或多少元", 5)]
        public decimal DiscountOrPrice { get; set; }

		/// <summary>
		/// 单价范围（0:不限）
		/// [长度：19，小数位数：4]
		/// [默认值：((0))]
		/// </summary>
        [Excel("单价范围", 6)]
        public decimal PriceRange { get; set; }

		/// <summary>
		/// 允许累加赠送（0:不允许、1:允许）
		/// [长度：5]
		/// [默认值：((1))]
		/// </summary>
        [Excel("允许累加赠送", 7)]
        public short AllowedAccumulate { get; set; }
	}
}
