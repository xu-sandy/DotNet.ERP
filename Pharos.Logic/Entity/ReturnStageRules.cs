using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class ReturnStageRules : BaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// 返赠规则ID
        /// </summary>
        public int ReturnRuleId { get; set; }
        /// <summary>
        /// 赠送项目（233：积分；232：返现）来自字典
        /// </summary>
        public short Project { get; set; }
        /// <summary>
        /// 返还时间
        /// </summary>
        public string ReturnDT { get; set; }
        /// <summary>
        /// 状态(0：生效；1：失效)
        /// </summary>
        public short State { get; set; }
        /// <summary>
        /// 返还类型（0：按月返）
        /// </summary>
        public short ReturnType { get; set; }
        /// <summary>
        /// 每期返还金额
        /// </summary>
        public decimal Average { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUID { get; set; }

    }
}
