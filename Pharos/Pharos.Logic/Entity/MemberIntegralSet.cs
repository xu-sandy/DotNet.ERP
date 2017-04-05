﻿using Pharos.Sys.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 积分设定
    /// </summary>
    public class MemberIntegralSet : SyncEntity
    {
        //[OperationLog("创建人", false)]
        //public int Id { get; set; }
        //public int CompanyId { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        [OperationLog("开始日期", false)]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        [OperationLog("结束日期", false)]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 适用对象,1-内部,2-VIP
        /// </summary>
        [OperationLog("适用对象","1:内部","2:VIP")]
        public string CustomerObj { get; set; }
        /// <summary>
        /// 是否包含促销
        /// </summary>
        [OperationLog("是否包含促销", "1:包含","0:不包含")]
        public bool Promotion { get; set; }
        /// <summary>
        /// 积分比例
        /// </summary>
        [OperationLog("积分比例", false)]
        public short Scale { get; set; }
        /// <summary>
        /// 设定类型(1-消费积分,2-商品积分)
        /// </summary>
        [OperationLog("设定类型", "1:消费积分","2:商品积分")]
        public short Nature { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [OperationLog("操作人", false)]
        public string OperatorUID { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [OperationLog("操作时间", false)]
        public DateTime OperatorTime { get; set; }

        public List<MemberIntegralSetList> ProductList { get; set; }
    }
    public class MemberIntegralSetList
    {
        [OperationLog("Id", false)]
        public int Id { get; set; }
        [OperationLog("IntegralId", false)]
        public int IntegralId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [OperationLog("条码", false)]
        public string BarcodeOrCategorySN { get; set; }
        /// <summary>
        /// 设定类型（1－商品,2－系列）
        /// </summary>
        [OperationLog("设定类型", "1:商品", "2:系列")]
        public short SetType { get; set; }
        /// <summary>
        /// 销售金额
        /// </summary>
        [OperationLog("销售金额", false)]
        public decimal SaleMoney { get; set; }
        /// <summary>
        /// 积分数
        /// </summary>
        [OperationLog("积分数", false)]
        public decimal IntegralCount { get; set; }
        [Pharos.Utility.Exclude]
        public byte[] SyncItemVersion { get; set; }
        [Pharos.Utility.Exclude]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid SyncItemId { get; set; }
    }
}
