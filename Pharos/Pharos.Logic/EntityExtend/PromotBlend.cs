using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class PromotBlend:BasePromotion
    {
        /// <summary>
        /// 规则类别（1:组合促销、2:满元促销）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short RuleType { get; set; }

        /// <summary>
        /// 促销ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CommodityId { get; set; }

        /// <summary>
        /// 促销形式（1:现金返回、2:代金券、3:折扣、4:加购按具体商品赠送、5:加购按单价范围赠送）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short PromotionType2 { get; set; }

        /// <summary>
        /// 满件数或满N元
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((2))]
        /// </summary>
        public decimal FullNumber { get; set; }

        /// <summary>
        /// 折扣或多少元
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal DiscountOrPrice { get; set; }

        /// <summary>
        /// 单价范围（0:不限）
        /// [长度：19，小数位数：4]
        /// [默认值：((0))]
        /// </summary>
        public decimal PriceRange { get; set; }

        /// <summary>
        /// 允许累加赠送（0:不允许、1:允许）
        /// [长度：5]
        /// [默认值：((1))]
        /// </summary>
        public short AllowedAccumulate { get; set; }
        /// <summary>
        /// 3种促销模式
        /// </summary>
        public int SellWay { get; set; }
        public string InsertProducted { get; set; }
        public string DeleteProducted { get; set; }
        public string InsertTypeed { get; set; }
        public string DeleteTypeed { get; set; }
        public string InsertNoTypeed { get; set; }
        public string DeleteNoTypeed { get; set; }
        public string InsertNoProducted { get; set; }
        public string DeleteNoProducted { get; set; }
    }
}
