// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：
// 创建时间：2015-07-29
// 描述信息：
// --------------------------------------------------

using Pharos.Sys.Extensions;
using Pharos.Utility;
using System;
using System.Runtime.Serialization;

namespace Pharos.Sys.Entity
{
    /// <summary>
    /// 用于管理本系统的所有业务数据字典信息
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [Excel("数据字典")]
    public class SysDataDictionary : BaseEntity
    {
        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        [OperationLog("ID", false)]
        public override int Id { get; set; }

        [Excel("编号", 1)]
        /// <summary>
        /// 编号（该编号全局唯一）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        [OperationLog("编号", false)]
        public int DicSN { get; set; }
        [Excel("父编号", 2)]
        /// <summary>
        /// 父编号ID（0：顶级）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        [OperationLog("父编号ID", true)]
        public int DicPSN { get; set; }

        [Excel("排序", 3)]
        /// <summary>
        /// 排序（0:无）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [DataMember]
        [OperationLog("排序", false)]
        public int SortOrder { get; set; }

        [Excel("类别名称", 4)]
        /// <summary>
        /// 类别名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        [OperationLog("类别名称", false)]
        public string Title { get; set; }

        [Excel("深度", 5)]
        /// <summary>
        /// 深度(1:一级、2:二级、3:三级、4:四级、9:具体字典)
        /// [长度：5]
        /// [默认值：((1))]
        /// </summary>
        [DataMember]
        [OperationLog("深度", false)]
        public short Depth { get; set; }
        [Excel("状态", 6)]
        /// <summary>
        /// 状态（0:关闭、1:可用）
        /// [长度：1]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        [DataMember]
        [OperationLog("状态", "false:关闭", "true:可用")]
        public bool Status { get; set; }
        /// <summary>
        /// 当前版本
        /// </summary>
        public byte[] SyncItemVersion { get; set; }
        /// <summary>
        /// 同步标识
        /// </summary>
        Guid syncItemId = Guid.NewGuid();
        public Guid SyncItemId { get { return syncItemId; } set { syncItemId = value; } }
    }
}
