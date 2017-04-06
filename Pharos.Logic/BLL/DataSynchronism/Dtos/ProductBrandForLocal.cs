using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
	/// <summary>
	/// 品牌信息表
	/// </summary>
    [Excel("品牌信息")]
    public class ProductBrandForLocal
	{
		/// <summary>
		/// 品牌分类ID（来自数据字典表）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
        [Excel("品牌分类ID", 1)]
        public int ClassifyId { get; set; }
        

		/// <summary>
		/// 品牌编号（全局唯)
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        [Excel("品牌编号", 2)]
        public int BrandSN { get; set; }

		/// <summary>
		/// 品牌名称
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
        [Excel("品牌名称", 3)]
        public string Title { get; set; }

        /// <summary>
        /// 品牌简拼
        /// [长度：10]
        /// </summary>
        [Excel("品牌简拼", 4)]
        public string JianPin { get; set; }

		/// <summary>
		/// 状态（0:禁用、1:可用） 
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
        [Excel("状态", 5)]
        public short State { get; set; }
	}
}
