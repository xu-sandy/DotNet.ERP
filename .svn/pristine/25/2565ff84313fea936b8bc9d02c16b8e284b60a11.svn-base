using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 会员活动推送与会员关联表
    /// </summary>
    public class PushWithMember
    {
        /// <summary>
        /// 记录Id
        /// [主键]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 推送Id
        /// [不允许为空]
        /// </summary>
        public string PushId { get; set; }
        /// <summary>
        /// 会员Id
        /// [不允许为空]
        /// </summary>
        public string MemberId { get; set; }
        /// <summary>
        /// 状态：0-待推送 1-成功 2-失败
        /// [不允许为空]
        /// </summary>
        public short State { get; set; }
        /// <summary>
        /// 推送方式（字典取值：dicSN-152）
        /// [不允许为空]
        /// </summary>
        public short Channel { get; set; }
    }
}
