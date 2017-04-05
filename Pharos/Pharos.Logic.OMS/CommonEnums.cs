using System.ComponentModel;
namespace Pharos.Logic.OMS
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum SysUserState : short
    {
        正常 = 1,
        停用 = 2,
        注销 = 3,
    }
    public enum EnableState : int
    {
        禁用 = 0,
        可用 = 1
    }
    /// <summary>
    /// 授权状态
    /// </summary>
    public enum CompanyAuthorizeState : short
    {
        未审 = 0,
        正常 = 1,
        暂停 = 2,
        已过期 = 3
    }

    /// <summary>
    /// 动作名称（审批日志）
    /// </summary>
    public enum ApproveLogType : short
    {
         提交申请=1,
         查阅=2,
         审批=3,
         驳回=4,
         重新提交=5
    }

    /// <summary>
    /// 模块编号（审批日志）
    /// </summary>
    public enum ApproveLogNum : short
    {
        支付许可 = 1
    }
}
