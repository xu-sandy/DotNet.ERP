// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有相关的附件基本信息
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 产品分类表
    /// </summary>
    [Excel("产品分类")]
    public partial class ProductCategory : BaseEntity
    {
        public Int64 Id { get; set; }

        [LocalKey]
        [ExcelField(@"^[0-9]{1,10}$###分类编号为整数且不能为空")]

        /// <summary>
        /// 分类编号（全局唯一） 
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("分类编号", 1)]
        public int CategorySN { get; set; }
        [ExcelField(@"^[0-9]{1,10}$###上级分类为整数且不能为空")]

        /// <summary>
        /// 上级分类SN
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("上级分类", 2)]
        public int CategoryPSN { get; set; }
        [ExcelField(@"^[0-9]{1,10}$###分类层级为整数且不能为空")]

        /// <summary>
        /// 分类层级（1:顶级、2：二级、3:三级、4:四级）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("分类层级", 3)]
        public short Grade { get; set; }
        [ExcelField(@"^[\s,\S]{1,50}$###分类名称为必填且不超过50字符")]

        /// <summary>
        /// 分类名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [Excel("分类名称", 4)]
        public string Title { get; set; }
        [Excel("分类顺序", 5)]
        [ExcelField(@"^[0-9]{0,10}$###分类顺序为不大于10位整数")]

        /// <summary>
        /// 顺序
        /// [长度：10]
        /// </summary>
        public int OrderNum { get; set; }
        [Excel("状态", 6)]
        [ExcelField(@"^[0,1]$###状态值范围（0:禁用、1:可用）")]

        /// <summary>
        /// 状态（0:禁用、1:可用）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short State { get; set; }

    }
}
