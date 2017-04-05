// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-04-27
// 描述信息：用于管理本系统的所有业务数据字典信息
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 数据字典
    /// </summary>
    [Excel("数据字典")]
    public class SysDataDictionary : BaseEntity
    {
        public Int64 Id { get; set; }
        [Excel("父编号", 2)]
        [ExcelField(@"^[0-9]{1,10}$###父编号必须为整数")]

        /// <summary>
        /// 父编号ID（0：顶级）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int DicPSN { get; set; }
        [LocalKey]
        [Excel("编号", 1)]
        [ExcelField(@"^[0-9]{1,10}$###编号必须为整数")]

        /// <summary>
        /// 编号（该编号全局唯一）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int DicSN { get; set; }
        [Excel("排序", 3)]
        [ExcelField(@"^[0-9]{1,10}$###排序必须为整数")]

        /// <summary>
        /// 排序（0:无）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public int SortOrder { get; set; }
        [Excel("类别名称", 4)]
        [ExcelField(@"^[\s,\S]{1,50}$###类别名称不允许为空且不大于50个字符")]

        /// <summary>
        /// 类别名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }
        [Excel("深度", 5)]
        [ExcelField(@"^[0-9]{1,10}$###排序必须为整数")]


        /// <summary>
        /// 深度(1:一级、2:二级、3:三级、4:四级、9:具体字典)
        /// [长度：5]
        /// [默认值：((1))]
        /// </summary>
        public short Depth { get; set; }
        [Excel("状态", 6)]
        [ExcelField(@"^[0,1]$###状态值范围（0:关闭、1:可用）")]

        /// <summary>
        /// 状态（0:关闭、1:可用）
        /// [长度：1]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public bool Status { get; set; }
    }
}
