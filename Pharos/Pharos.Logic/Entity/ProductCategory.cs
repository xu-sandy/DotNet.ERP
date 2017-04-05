// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有相关的附件基本信息
// --------------------------------------------------

using System;
using System.Runtime.Serialization;
using Pharos.Utility;
using Pharos.Sys.Extensions;

namespace Pharos.Logic.Entity
{
    [Excel("产品分类")]
    /// <summary>
    /// 产品分类表
    /// </summary>
    public partial class ProductCategory : SyncEntity
    {

        [Excel("分类编号",1)]
        /// <summary>
        /// 分类编号（全局唯一） 
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [OperationLog("分类编号", false)]
        public int CategorySN { get; set; }

        [Excel("上级分类", 2)]
        /// <summary>
        /// 上级分类SN
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [OperationLog("上级分类", false)]
        public int CategoryPSN { get; set; }

        [Excel("分类层级", 3)]
        /// <summary>
        /// 分类层级（1:顶级、2：二级、3:三级、4:四级）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [OperationLog("分类层级", false)]
        public short Grade { get; set; }
        /// <summary>
        /// 分类编号,最大99
        /// </summary>
        [OperationLog("分类编号", false)]
        public short CategoryCode { get; set; }

        [Excel("分类名称", 4)]
        /// <summary>
        /// 分类名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [OperationLog("分类名称", false)]
        public string Title { get; set; }

        [Excel("分类顺序", 5)]

        /// <summary>
        /// 顺序
        /// [长度：10]
        /// </summary>
        [OperationLog("顺序", false)]
        public int OrderNum { get; set; }


        /// <summary>
        /// 状态（0:禁用、1:可用）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("状态", 6)]
        [OperationLog("状态", "0:禁用", "1:可用")]
        public short State { get { return state; } set { state = value; } }

        [OperationLog("状态", false)]
        short state = 1;

    }
}
