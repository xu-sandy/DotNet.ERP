using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    [Serializable]
    public class ViewAgentsInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private int _Id;

        /// <summary>
        /// 代理商类型
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 支付通道
        /// </summary>
        public string ApiTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 成本费率
        /// </summary>
        public decimal Cost
        {
            get;
            set;
        }

        /// <summary>
        /// 下级费率
        /// </summary>
        public decimal Lower
        {
            get;
            set;
        }

        /// <summary>
        /// 通道状态
        /// </summary>
        public short apiState
        {
            get;
            set;
        }

        /// <summary>
        /// 代理商编号，全局唯一（6位，从100001开始递增到999999）
        /// </summary>
        public int AgentsId
        {
            get { return _AgentsId; }
            set { _AgentsId = value; }
        }
        private int _AgentsId;

        /// <summary>
        /// 代理商全称
        /// </summary>
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }
        private string _FullName;

        /// <summary>
        /// 代理商简称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Name;

        /// <summary>
        /// 代理商状态（0:待审，1:正常，2:终止，3:到期）
        /// </summary>
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private int _Status;

        /// <summary>
        /// 上级代理商编号
        /// </summary>
        public int PAgentsId
        {
            get { return _PAgentsId; }
            set { _PAgentsId = value; }
        }
        private int _PAgentsId;

        /// <summary>
        /// 有效-终止日期
        /// </summary>
        public string EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }
        private string _EndTime;

        /// <summary>
        /// 代理区域（城市名称，多个以逗号为分隔，空为不限）
        /// </summary>
        public string AgentAreaNames { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string Contract
        {
            get { return _Contract; }
            set { _Contract = value; }
        }
        private string _Contract;

        /// <summary>
        /// 法人姓名
        /// </summary>
        public string CorporateName
        {
            get { return _CorporateName; }
            set { _CorporateName = value; }
        }
        private string _CorporateName;

        /// <summary>
        /// 法人身份证
        /// </summary>
        public string IdCard
        {
            get { return _IdCard; }
            set { _IdCard = value; }
        }
        private string _IdCard;

        /// <summary>
        /// 公司电话
        /// </summary>
        public string CompanyPhone
        {
            get { return _CompanyPhone; }
            set { _CompanyPhone = value; }
        }
        private string _CompanyPhone;

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        private string _Address;

        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan
        {
            get { return _LinkMan; }
            set { _LinkMan = value; }
        }
        private string _LinkMan;

        /// <summary>
        /// 联系电话1
        /// </summary>
        public string Phone1
        {
            get { return _Phone1; }
            set { _Phone1 = value; }
        }
        private string _Phone1;

        /// <summary>
        /// 联系电话2
        /// </summary>
        public string Phone2
        {
            get { return _Phone2; }
            set { _Phone2 = value; }
        }
        private string _Phone2;

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ
        {
            get { return _QQ; }
            set { _QQ = value; }
        }
        private string _QQ;

        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        private string _Email;

        /// <summary>
        /// 微信号
        /// </summary>
        public string Weixin
        {
            get { return _Weixin; }
            set { _Weixin = value; }
        }
        private string _Weixin;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
        private DateTime _CreateTime;

        /// <summary>
        /// 创建人（来自SysUser表）
        /// </summary>
        public string sysCreFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 指派人（来自SysUser表）
        /// </summary>
        public string sysAssFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人（来自AgentsUsers表）
        /// </summary>
        public string AgenCreFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 指派人（来自AgentsUsers表）
        /// </summary>
        public string AgenAssFullName
        {
            get;
            set;
        }
    }
}
