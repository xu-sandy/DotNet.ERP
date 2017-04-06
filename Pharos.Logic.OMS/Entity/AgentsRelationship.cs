// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：
// 创建时间：2016-11-21
// 描述信息：用于管理本系统的所有代理商下级关系信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 代理商下级关系
	/// </summary>
	[Serializable]
	public class AgentsRelationship
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		private int _Id;

        /// <summary>
        /// 关系编号
        /// </summary>
        public int RelationshipId
        {
            get;
            set;
        }

		/// <summary>
		/// 代理商编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int AgentsId
		{
			get { return _AgentsId; }
			set { _AgentsId = value; }
		}
		private int _AgentsId;

		/// <summary>
		/// 下级代理商编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int SubAgentsId
		{
			get { return _SubAgentsId; }
			set { _SubAgentsId = value; }
		}
		private int _SubAgentsId;

		/// <summary>
		/// 深度（0-3）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public short Depth
		{
			get { return _Depth; }
			set { _Depth = value; }
		}
		private short _Depth;

		/// <summary>
		/// 状态（1:正常，2:注销）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public short Status
		{
			get { return _Status; }
			set { _Status = value; }
		}
		private short _Status;

		/// <summary>
		/// 创建时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateTime
		{
			get { return _CreateTime; }
			set { _CreateTime = value; }
		}
		private DateTime _CreateTime;
	}
}