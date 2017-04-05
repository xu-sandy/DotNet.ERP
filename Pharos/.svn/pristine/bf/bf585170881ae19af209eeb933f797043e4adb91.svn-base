// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-04-27
// 描述信息：用于管理本系统的全局权限Code信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 权限代码
    /// </summary>
    [Serializable]
    public class SysLimits
    {
        /// <summary>
        /// 权限ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 隶属功能ID（0:顶级）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public int PId { get; set; }

        /// <summary>
        /// 功能名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 功能Code（全局唯一）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 深度（1-9）
        /// [长度：5]
        /// </summary>
        public short Depth { get; set; }

        /// <summary>
        /// 状态（0:关闭/停用、1:显示/默认选中、2:可选）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((2))]
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// 赋值数据
        /// </summary>
        /// <param name="limit"></param>
        public void Replace(SysLimits limit)
        {
            PId = limit.PId;
            Key = limit.Key;
            Code = limit.Code;
            Depth = limit.Depth;
            Status = limit.Status;
        }
        /// <summary>
        /// 根据父级深度设置当前深度
        /// </summary>
        /// <param name="pDepth"></param>
        public void SetDepth(SysLimits pLimit) {
            int pdepth = 0;
            if (pLimit != null)
            {
                pdepth = pLimit.Depth;
            }
            Depth = (short)(pdepth + 1);
        }
    }
}
