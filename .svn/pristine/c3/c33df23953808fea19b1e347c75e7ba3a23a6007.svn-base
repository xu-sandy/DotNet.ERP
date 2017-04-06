using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    /// <summary>
    /// 产品分类表
    /// </summary>
    [Excel("产品分类")]
    public partial class ProductCategoryForLocal
    {
        /// <summary>
        /// 分类编号（全局唯一） 
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("分类编号", 1)]
        public int CategorySN { get; set; }

        /// <summary>
        /// 上级分类SN
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("上级分类", 2)]
        public int CategoryPSN { get; set; }

        /// <summary>
        /// 分类层级（1:顶级、2：二级、3:三级、4:四级）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("分类层级", 3)]
        public short Grade { get; set; }

        /// <summary>
        /// 分类名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [Excel("分类名称", 4)]
        public string Title { get; set; }
        [Excel("分类顺序", 5)]

        /// <summary>
        /// 顺序
        /// [长度：10]
        /// </summary>
        public int OrderNum { get; set; }
        [Excel("状态", 6)]

        /// <summary>
        /// 状态（0:禁用、1:可用）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short State { get; set; }

    }
}
