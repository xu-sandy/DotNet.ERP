// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的产品档案所附属的品牌信息
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 品牌信息表
    /// </summary>
    [Excel("品牌信息")]
    public class ProductBrand : BaseEntity
    {
        public Int64 Id { get; set; }
        [ExcelField(@"^[0-9]{1,10}$###品牌分类ID为不大于10位数字且不为空")]

        /// <summary>
        /// 品牌分类ID（来自数据字典表）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        [Excel("品牌分类ID", 1)]
        public int ClassifyId { get; set; }
        [LocalKey]
        [ExcelField(@"^[0-9]{1,10}$###品牌编号为不大于10位数字且不为空")]

        /// <summary>
        /// 品牌编号（全局唯)
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("品牌编号", 2)]
        public int BrandSN { get; set; }
        [ExcelField(@"^[\s,\S]{1,20}$###品牌名称为必填且不超过50字符")]


        /// <summary>
        /// 品牌名称
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        [Excel("品牌名称", 3)]
        public string Title { get; set; }
        [ExcelField(@"^[a-z,A-Z,0-9]{0,10}$###品牌简拼只能为不超过10位字母或数字")]

        /// <summary>
        /// 品牌简拼
        /// [长度：10]
        /// </summary>
        [Excel("品牌简拼", 4)]
        public string JianPin { get; set; }
        [ExcelField(@"^[0,1]$###状态值范围（0:禁用、1:可用）")]

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
