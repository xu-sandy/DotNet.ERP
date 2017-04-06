// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：蔡少发
// 创建时间：2016-08-02
// 描述信息：
// --------------------------------------------------

using System;
using Pharos.Sys.Extensions;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 用于管理本系统的所有会员的充值记录信息
    /// </summary>
    [Serializable]
    public class MemberRecharge
    {
        /// <summary>
        /// 记录id
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 公司CID
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 流水号
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [OperationLog("流水号", false)]
        public string RechargeSN { get; set; }

        /// <summary>
        /// 卡号
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [OperationLog("卡号", false)]       
        public string CardId { get; set; }

        /// <summary>
        /// 类型（1：支入；2：支出）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// 充值金额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        [OperationLog("充值金额", false)]  
        public decimal RechargeAmount { get; set; }

        /// <summary>
        /// 赠送金额
        /// [长度：19，小数位数：4]
        /// [允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal? GivenAmount { get; set; }

        /// <summary>
        /// 赠送积分
        /// [长度：10]
        /// [允许为空]
        /// [默认值：((0))]
        /// </summary>
        public int? PresentExp { get; set; }
        /// <summary>
        /// 充前金额
        /// </summary>
        [OperationLog("充前金额", false)] 
        public decimal? BeforeAmount { get; set; }
        /// <summary>
        /// 充值后金额
        /// [长度：19，小数位数：4]
        /// [默认值：((0))]
        /// </summary>
        public decimal AfterAmount { get; set; }

        /// <summary>
        /// 充值后积分
        /// [长度：10]
        /// [默认值：((0))]
        /// </summary>
        public int AfterIntegral { get; set; }

        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 充值时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [OperationLog("充值时间", false)]
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 操作人UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }
        /// <summary>
        /// 充值赠送id 没有为空
        /// </summary>
        public string RechargeGiftId { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        public decimal Number { get; set; }
        /// <summary>
        /// 门店ID(后台充值默认为"00")
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 是否为练习模式
        /// </summary>
        public bool IsTest { get; set; }
        /// <summary>
        /// 门店设备编号（后台充值默认为"00"）
        /// </summary>
        public string MachineSN { get; set; }
    }
}
