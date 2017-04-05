using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.MemberDomain.Interfaces
{
    /// <summary>
    /// 场景数据 包含会员信息、场景信息
    /// 【余雄文】
    /// </summary>
    public interface IScene
    {
    }

    public interface IScene<TMember> : IScene
    {
        TMember Member { get; set; }
    }
}
